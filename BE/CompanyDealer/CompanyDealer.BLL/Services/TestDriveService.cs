using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs.TestDriveDTOs;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.TestDriveRepo;
using CompanyDealer.DAL.Repository.VehicleRepo;
using CompanyDealer.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CompanyDealer.BLL.Services
{
    public interface ITestDriveService
    {
        Task<IEnumerable<TestDriveResponse>> GetAllAsync(Guid? dealerId = null, Guid? vehicleId = null, string? status = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<TestDriveResponse> GetByIdAsync(Guid id);
        Task<TestDriveResponse> CreateAsync(CreateTestDriveRequest request, Guid createdBy, string createdByName);
        Task<TestDriveResponse> UpdateAsync(Guid id, UpdateTestDriveRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<TestDriveResponse> ApproveAsync(Guid id, ApproveTestDriveRequest request, string approvedByName);
        Task<TestDriveResponse> RejectAsync(Guid id, RejectTestDriveRequest request, string rejectedByName);
        Task<IEnumerable<TestDriveResponse>> GetTestDriveByDealerIdAsync(Guid dealerId);
    }

    public class TestDriveService : ITestDriveService
    {
        private readonly ITestDriveRepository _testDriveRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ApplicationDbContext _context;

        public TestDriveService(
            ITestDriveRepository testDriveRepository,
            IVehicleRepository vehicleRepository,
            ApplicationDbContext context)
        {
            _testDriveRepository = testDriveRepository;
            _vehicleRepository = vehicleRepository;
            _context = context;
        }

        public async Task<IEnumerable<TestDriveResponse>> GetAllAsync(
            Guid? dealerId = null, 
            Guid? vehicleId = null, 
            string? status = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            IEnumerable<TestDriveRecord> testDrives;

            if (dealerId.HasValue)
            {
                testDrives = await _testDriveRepository.GetByDealerIdAsync(dealerId.Value);
            }
            else if (vehicleId.HasValue)
            {
                testDrives = await _testDriveRepository.GetByVehicleIdAsync(vehicleId.Value);
            }
            else if (fromDate.HasValue && toDate.HasValue)
            {
                testDrives = await _testDriveRepository.GetByDateRangeAsync(fromDate.Value, toDate.Value);
            }
            else
            {
                testDrives = await _testDriveRepository.GetAllAsync();
            }

            // Filter by status if provided
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<TestDriveStatus>(status, true, out var statusEnum))
            {
                testDrives = testDrives.Where(t => t.Status == statusEnum);
            }

            return testDrives.Select(MapToResponse);
        }

        public async Task<TestDriveResponse> GetByIdAsync(Guid id)
        {
            var testDrive = await _testDriveRepository.GetByIdAsync(id);
            if (testDrive == null)
                throw new KeyNotFoundException($"Test drive with ID {id} not found");

            return MapToResponse(testDrive);
        }

        public async Task<TestDriveResponse> CreateAsync(CreateTestDriveRequest request, Guid createdBy, string createdByName)
        {
            // Validate test date is in the future
            if (request.TestDate <= DateTime.UtcNow)
                throw new ArgumentException("Test date must be in the future");

            // Validate vehicle exists and is available
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle with ID {request.VehicleId} not found");

            if (!vehicle.IsAvailable)
                throw new InvalidOperationException("Vehicle is not available for test drive");

            // Validate dealer exists
            var dealer = await _context.Dealers.FindAsync(request.DealerId);
            if (dealer == null)
                throw new KeyNotFoundException($"Dealer with ID {request.DealerId} not found");

            if (!dealer.IsActive)
                throw new InvalidOperationException("Dealer is not active");

            // Check for schedule conflicts
            var hasConflict = await _testDriveRepository.HasScheduleConflictAsync(request.VehicleId, request.TestDate);
            if (hasConflict)
                throw new InvalidOperationException("This vehicle already has a test drive scheduled within 2 hours of the requested time");

            var testDrive = new TestDriveRecord
            {
                Id = Guid.NewGuid(),
                TestDate = request.TestDate,
                CustomerName = request.CustomerName,
                CustomerContact = request.CustomerContact,
                Notes = request.Notes ?? string.Empty,
                DealerId = request.DealerId,
                VehicleId = request.VehicleId,
                Status = TestDriveStatus.Pending,
                CreatedBy = createdBy,
                CreatedByName = createdByName,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _testDriveRepository.CreateAsync(testDrive);
            return MapToResponse(created);
        }

        public async Task<TestDriveResponse> UpdateAsync(Guid id, UpdateTestDriveRequest request)
        {
            var testDrive = await _testDriveRepository.GetByIdAsync(id);
            if (testDrive == null)
                throw new KeyNotFoundException($"Test drive with ID {id} not found");

            // Update only provided fields
            if (request.TestDate.HasValue)
            {
                if (request.TestDate.Value <= DateTime.UtcNow)
                    throw new ArgumentException("Test date must be in the future");

                // Check for conflicts with the new date
                var hasConflict = await _testDriveRepository.HasScheduleConflictAsync(
                    testDrive.VehicleId, 
                    request.TestDate.Value, 
                    id);
                    
                if (hasConflict)
                    throw new InvalidOperationException("This vehicle already has a test drive scheduled within 2 hours of the requested time");

                testDrive.TestDate = request.TestDate.Value;
            }

            if (!string.IsNullOrEmpty(request.CustomerName))
                testDrive.CustomerName = request.CustomerName;

            if (!string.IsNullOrEmpty(request.CustomerContact))
                testDrive.CustomerContact = request.CustomerContact;

            if (request.Notes != null)
                testDrive.Notes = request.Notes;

            if (request.DealerId.HasValue)
            {
                var dealer = await _context.Dealers.FindAsync(request.DealerId.Value);
                if (dealer == null)
                    throw new KeyNotFoundException($"Dealer with ID {request.DealerId.Value} not found");
                testDrive.DealerId = request.DealerId.Value;
            }

            if (request.VehicleId.HasValue)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId.Value);
                if (vehicle == null)
                    throw new KeyNotFoundException($"Vehicle with ID {request.VehicleId.Value} not found");
                testDrive.VehicleId = request.VehicleId.Value;
            }

            // If updating a rejected test drive, reset status to pending
            if (testDrive.Status == TestDriveStatus.Rejected)
            {
                testDrive.Status = TestDriveStatus.Pending;
                testDrive.RejectionReason = string.Empty;
                testDrive.RejectedAt = null;
            }

            var updated = await _testDriveRepository.UpdateAsync(testDrive);
            return MapToResponse(updated);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var exists = await _testDriveRepository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException($"Test drive with ID {id} not found");

            return await _testDriveRepository.DeleteAsync(id);
        }

        public async Task<TestDriveResponse> ApproveAsync(Guid id, ApproveTestDriveRequest request, string approvedByName)
        {
            var testDrive = await _testDriveRepository.GetByIdAsync(id);
            if (testDrive == null)
                throw new KeyNotFoundException($"Test drive with ID {id} not found");

            if (testDrive.Status != TestDriveStatus.Pending)
                throw new InvalidOperationException($"Only pending test drives can be approved. Current status: {testDrive.Status}");

            // Check for conflicts again before approving
            var hasConflict = await _testDriveRepository.HasScheduleConflictAsync(
                testDrive.VehicleId, 
                testDrive.TestDate, 
                id);
                
            if (hasConflict)
                throw new InvalidOperationException("Cannot approve: This vehicle already has an approved test drive scheduled within 2 hours");

            testDrive.Status = TestDriveStatus.Approved;
            testDrive.ApprovedBy = request.ApprovedBy;
            testDrive.ApprovedByName = approvedByName;
            testDrive.ApprovedAt = DateTime.UtcNow;

            var updated = await _testDriveRepository.UpdateAsync(testDrive);
            return MapToResponse(updated);
        }

        public async Task<TestDriveResponse> RejectAsync(Guid id, RejectTestDriveRequest request, string rejectedByName)
        {
            var testDrive = await _testDriveRepository.GetByIdAsync(id);
            if (testDrive == null)
                throw new KeyNotFoundException($"Test drive with ID {id} not found");

            if (testDrive.Status != TestDriveStatus.Pending)
                throw new InvalidOperationException($"Only pending test drives can be rejected. Current status: {testDrive.Status}");

            testDrive.Status = TestDriveStatus.Rejected;
            testDrive.ApprovedBy = request.RejectedBy;
            testDrive.ApprovedByName = rejectedByName;
            testDrive.RejectionReason = request.RejectionReason;
            testDrive.RejectedAt = DateTime.UtcNow;

            var updated = await _testDriveRepository.UpdateAsync(testDrive);
            return MapToResponse(updated);
        }
        public async Task<IEnumerable<TestDriveResponse>> GetTestDriveByDealerIdAsync(Guid dealerId)
        {
            var testDrives = await _testDriveRepository.GetByDealerIdAsync(dealerId);
            return testDrives.Select(MapToResponse);
        }

        private TestDriveResponse MapToResponse(TestDriveRecord testDrive)
        {
            return new TestDriveResponse
            {
                Id = testDrive.Id,
                TestDate = testDrive.TestDate,
                CustomerName = testDrive.CustomerName,
                CustomerContact = testDrive.CustomerContact,
                Notes = testDrive.Notes,
                Status = testDrive.Status.ToString(),
                CreatedBy = testDrive.CreatedBy,
                CreatedByName = testDrive.CreatedByName,
                CreatedAt = testDrive.CreatedAt,
                ApprovedBy = testDrive.ApprovedBy,
                ApprovedByName = testDrive.ApprovedByName,
                ApprovedAt = testDrive.ApprovedAt,
                RejectionReason = testDrive.RejectionReason,
                RejectedAt = testDrive.RejectedAt,
                DealerId = testDrive.DealerId,
                Dealer = testDrive.Dealer != null ? new DealerInfo
                {
                    Id = testDrive.Dealer.Id,
                    Name = testDrive.Dealer.Name,
                    Location = testDrive.Dealer.Location
                } : null,
                VehicleId = testDrive.VehicleId,
                Vehicle = testDrive.Vehicle != null ? new VehicleInfo
                {
                    Id = testDrive.Vehicle.Id,
                    Make = testDrive.Vehicle.Make,
                    Model = testDrive.Vehicle.Model,
                    Year = testDrive.Vehicle.Year,
                    Color = testDrive.Vehicle.Color,
                    VIN = testDrive.Vehicle.VIN
                } : null
            };
        }
    }
}
