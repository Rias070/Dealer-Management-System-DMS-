using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyDealer.DAL.Repository.TestDriveRepo
{
    public class TestDriveRepository : ITestDriveRepository
    {
        private readonly ApplicationDbContext _context;

        public TestDriveRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TestDriveRecord>> GetAllAsync()
        {
            return await _context.TestDriveRecords
                .Include(t => t.Dealer)
                .Include(t => t.Vehicle)
                .OrderByDescending(t => t.TestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TestDriveRecord>> GetByDealerIdAsync(Guid dealerId)
        {
            return await _context.TestDriveRecords
                .Include(t => t.Dealer)
                .Include(t => t.Vehicle)
                .Where(t => t.DealerId == dealerId)
                .OrderByDescending(t => t.TestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TestDriveRecord>> GetByVehicleIdAsync(Guid vehicleId)
        {
            return await _context.TestDriveRecords
                .Include(t => t.Dealer)
                .Include(t => t.Vehicle)
                .Where(t => t.VehicleId == vehicleId)
                .OrderByDescending(t => t.TestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TestDriveRecord>> GetByStatusAsync(TestDriveStatus status)
        {
            return await _context.TestDriveRecords
                .Include(t => t.Dealer)
                .Include(t => t.Vehicle)
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.TestDate)
                .ToListAsync();
        }

        public async Task<TestDriveRecord?> GetByIdAsync(Guid id)
        {
            return await _context.TestDriveRecords
                .Include(t => t.Dealer)
                .Include(t => t.Vehicle)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TestDriveRecord> CreateAsync(TestDriveRecord testDrive)
        {
            _context.TestDriveRecords.Add(testDrive);
            await _context.SaveChangesAsync();
            
            // Reload with navigation properties
            return await GetByIdAsync(testDrive.Id) ?? testDrive;
        }

        public async Task<TestDriveRecord> UpdateAsync(TestDriveRecord testDrive)
        {
            _context.TestDriveRecords.Update(testDrive);
            await _context.SaveChangesAsync();
            
            // Reload with navigation properties
            return await GetByIdAsync(testDrive.Id) ?? testDrive;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var testDrive = await _context.TestDriveRecords.FindAsync(id);
            if (testDrive == null)
                return false;

            _context.TestDriveRecords.Remove(testDrive);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.TestDriveRecords.AnyAsync(t => t.Id == id);
        }

        public async Task<bool> HasScheduleConflictAsync(Guid vehicleId, DateTime testDate, Guid? excludeTestDriveId = null)
        {
            // Check if there's any approved test drive for the same vehicle within 2 hours
            var timeWindow = TimeSpan.FromHours(2);
            var conflictStart = testDate.AddHours(-2);
            var conflictEnd = testDate.AddHours(2);

            var query = _context.TestDriveRecords
                .Where(t => t.VehicleId == vehicleId &&
                           t.Status == TestDriveStatus.Approved &&
                           t.TestDate >= conflictStart &&
                           t.TestDate <= conflictEnd);

            if (excludeTestDriveId.HasValue)
            {
                query = query.Where(t => t.Id != excludeTestDriveId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<TestDriveRecord>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.TestDriveRecords
                .Include(t => t.Dealer)
                .Include(t => t.Vehicle)
                .Where(t => t.TestDate >= fromDate && t.TestDate <= toDate)
                .OrderByDescending(t => t.TestDate)
                .ToListAsync();
        }
    }
}
