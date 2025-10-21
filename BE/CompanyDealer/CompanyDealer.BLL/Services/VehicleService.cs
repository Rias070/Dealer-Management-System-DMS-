using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs;
using CompanyDealer.BLL.DTOs.VehicleDTOs;
using CompanyDealer.BLL.ExceptionHandle;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.VehicleRepo;

namespace CompanyDealer.BLL.Services
{
    public class VehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<List<VehicleDto>> GetAllAsync()
        {
            var entities = await _vehicleRepository.GetAllAsync();
            return entities.Select(MapToDto).ToList();
        }

        public async Task<VehicleResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await _vehicleRepository.GetByIdAsync(id);
            if (entity == null)
                throw new ApiException.NotFoundException($"Vehicle with id '{id}' not found");

            return new VehicleResponseDto
            {
                Success = true,
                Message = "Vehicle retrieved",
                VehicleId = entity.Id,
                Vehicle = MapToDto(entity)
            };
            }

        public async Task<VehicleResponseDto> CreateAsync(VehicleRequestDto request)
            {
            var vehicle = new Vehicle
            {
                Id = request.Id ?? Guid.NewGuid(),
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
                VIN = request.VIN,
                Color = request.Color,
                Price = request.Price,
                Description = request.Description,
                IsAvailable = request.IsAvailable,
                CategoryId = request.CategoryId
            };

            var created = await _vehicleRepository.CreateAsync(vehicle);

            return new VehicleResponseDto
            {
                Success = true,
                Message = "Vehicle created successfully",
                VehicleId = created.Id,
                Vehicle = MapToDto(created)
            };
            }

        public async Task<VehicleResponseDto> UpdateAsync(VehicleRequestDto request)
        {
            if (!request.Id.HasValue)
                throw new ApiException.BadRequestException("Vehicle Id is required for update");

            var existing = await _vehicleRepository.GetByIdAsync(request.Id.Value);
            if (existing == null)
                throw new ApiException.NotFoundException($"Vehicle with id '{request.Id}' not found");

            existing.Make = request.Make;
            existing.Model = request.Model;
            existing.Year = request.Year;
            existing.VIN = request.VIN;
            existing.Color = request.Color;
            existing.Price = request.Price;
            existing.Description = request.Description;
            existing.IsAvailable = request.IsAvailable;
            existing.CategoryId = request.CategoryId;

            var updated = await _vehicleRepository.UpdateAsync(existing);

            return new VehicleResponseDto
            {
                Success = true,
                Message = "Vehicle updated successfully",
                VehicleId = updated?.Id ?? existing.Id,
                Vehicle = MapToDto(updated ?? existing)
            };
        }

        public async Task<VehicleResponseDto> DeleteAsync(Guid id)
        {
            var existing = await _vehicleRepository.GetByIdAsync(id);
            if (existing == null)
                throw new ApiException.NotFoundException($"Vehicle with id '{id}' not found");

            var deleted = await _vehicleRepository.DeleteAsync(id);

            return new VehicleResponseDto
        {
                Success = deleted,
                Message = deleted ? "Vehicle deleted successfully" : "Vehicle not deleted",
                VehicleId = id,
                Vehicle = deleted ? MapToDto(existing) : null
            };
        }

        private static VehicleDto MapToDto(Vehicle v)
            {
            if (v == null) return null;

            return new VehicleDto
            {
        }

        public Task<bool> DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }
}