using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs.InventoryDTOs;
using CompanyDealer.BLL.ExceptionHandle;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.InventoryRepo;

namespace CompanyDealer.BLL.Services
{
    public class InventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<List<InventoryDto>> GetAllAsync()
        {
            var entities = await _inventoryRepository.GetAllAsync();
            return entities.Select(MapToDto).ToList();
        }

        public async Task<InventoryResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await _inventoryRepository.GetByIdAsync(id);
            if (entity == null)
                throw new ApiException.NotFoundException($"Inventory with id '{id}' not found");

            return new InventoryResponseDto
            {
                Success = true,
                Message = "Inventory retrieved",
                InventoryId = entity.Id,
                Inventory = MapToDto(entity)
            };
        }

        public async Task<InventoryResponseDto> CreateAsync(InventoryRequestDto request)
        {
            var inventory = new Inventory
            {
                Id = request.Id ?? Guid.NewGuid(),
                LastUpdated = request.LastUpdated ?? DateTime.UtcNow,
                DealerId = request.DealerId
            };

            var created = await _inventoryRepository.CreateAsync(inventory);

            return new InventoryResponseDto
            {
                Success = true,
                Message = "Inventory created successfully",
                InventoryId = created.Id,
                Inventory = MapToDto(created)
            };
        }

        public async Task<InventoryResponseDto> UpdateAsync(InventoryRequestDto request)
        {
            if (!request.Id.HasValue)
                throw new ApiException.BadRequestException("Inventory Id is required for update");

            var existing = await _inventoryRepository.GetByIdAsync(request.Id.Value);
            if (existing == null)
                throw new ApiException.NotFoundException($"Inventory with id '{request.Id}' not found");

            existing.LastUpdated = request.LastUpdated ?? DateTime.UtcNow;
            existing.DealerId = request.DealerId;

            var updated = await _inventoryRepository.UpdateAsync(existing);

            return new InventoryResponseDto
            {
                Success = true,
                Message = "Inventory updated successfully",
                InventoryId = updated?.Id ?? existing.Id,
                Inventory = MapToDto(updated ?? existing)
            };
        }

        public async Task<InventoryResponseDto> DeleteAsync(Guid id)
        {
            var existing = await _inventoryRepository.GetByIdAsync(id);
            if (existing == null)
                throw new ApiException.NotFoundException($"Inventory with id '{id}' not found");

            var deleted = await _inventoryRepository.DeleteAsync(id);

            return new InventoryResponseDto
            {
                Success = deleted,
                Message = deleted ? "Inventory deleted successfully" : "Inventory not deleted",
                InventoryId = id,
                Inventory = deleted ? MapToDto(existing) : null
            };
        }
        public async Task<List<Vehicle>> GetVehicleInInventory(Guid dealerId)
        {
            return await _inventoryRepository.GetVehicleInInventory(dealerId);
        }

        private static InventoryDto MapToDto(Inventory i)
        {
            if (i == null) return null!;

            return new InventoryDto
            {
                Id = i.Id,
                LastUpdated = i.LastUpdated,
                DealerId = i.DealerId
            };
        }
    }
}