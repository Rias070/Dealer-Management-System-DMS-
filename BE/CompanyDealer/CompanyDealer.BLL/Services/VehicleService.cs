using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.VehicleRepo;

namespace CompanyDealer.BLL.Services
{
    public class VehicleService
    {
        private readonly VehicleRepository _repository;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(VehicleRepository repository, ILogger<VehicleService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            if (vehicle.Id == Guid.Empty) vehicle.Id = Guid.NewGuid();

            // Business defaults
            vehicle.IsAvailable = vehicle.IsAvailable;

            try
            {
                return await _repository.CreateAsync(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vehicle");
                throw;
            }
        }

        public Task<Vehicle?> GetByIdAsync(Guid id)
            => _repository.GetByIdAsync(id);

        public Task<List<Vehicle>> GetAllAsync()
            => _repository.GetAllAsync();

        public async Task<Vehicle?> UpdateAsync(Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));

            try
            {
                return await _repository.UpdateAsync(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vehicle {VehicleId}", vehicle.Id);
                throw;
            }
        }

        public Task<bool> DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }
}