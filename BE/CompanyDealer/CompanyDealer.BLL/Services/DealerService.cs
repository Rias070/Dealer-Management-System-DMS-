using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs.DealerDTOs;
using CompanyDealer.BLL.ExceptionHandle;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.DealerRepo;

namespace CompanyDealer.BLL.Services
{
    public class DealerService
    {
        private readonly IDealerRepository _dealerRepository;

        public DealerService(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
        }

        public async Task<List<DealerDto>> GetAllAsync()
        {
            var entities = await _dealerRepository.GetAllAsync();
            return entities.Select(MapToDto).ToList();
        }

        public async Task<List<DealerDto>> GetActiveDealersAsync()
        {
            var entities = await _dealerRepository.GetActiveDealersAsync();
            return entities.Select(MapToDto).ToList();
        }

        public async Task<DealerResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await _dealerRepository.GetByIdAsync(id);
            if (entity == null)
                throw new ApiException.NotFoundException($"Dealer with id '{id}' not found");

            return new DealerResponseDto
            {
                Success = true,
                Message = "Dealer retrieved successfully",
                DealerId = entity.Id,
                Dealer = MapToDto(entity)
            };
        }

        public async Task<DealerResponseDto> CreateAsync(DealerRequestDto request)
        {
            var dealer = new Dealer
            {
                Id = request.Id ?? Guid.NewGuid(),
                Name = request.Name,
                Location = request.Location,
                ContactInfo = request.ContactInfo,
                RegistrationDate = DateTime.UtcNow,
                IsActive = request.IsActive
            };

            var created = await _dealerRepository.CreateAsync(dealer);

            return new DealerResponseDto
            {
                Success = true,
                Message = "Dealer created successfully",
                DealerId = created.Id,
                Dealer = MapToDto(created)
            };
        }

        public async Task<DealerResponseDto> UpdateAsync(Guid id, DealerRequestDto request)
        {
            var existingDealer = await _dealerRepository.GetByIdAsync(id);
            if (existingDealer == null)
                throw new ApiException.NotFoundException($"Dealer with id '{id}' not found");

            existingDealer.Name = request.Name;
            existingDealer.Location = request.Location;
            existingDealer.ContactInfo = request.ContactInfo;
            existingDealer.IsActive = request.IsActive;

            var updated = await _dealerRepository.UpdateAsync(existingDealer);
            if (updated == null)
                throw new ApiException.BadRequestException("Failed to update dealer");

            return new DealerResponseDto
            {
                Success = true,
                Message = "Dealer updated successfully",
                DealerId = updated.Id,
                Dealer = MapToDto(updated)
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var exists = await _dealerRepository.ExistsAsync(id);
            if (!exists)
                throw new ApiException.NotFoundException($"Dealer with id '{id}' not found");

            return await _dealerRepository.DeleteAsync(id);
        }

        private static DealerDto MapToDto(Dealer entity)
        {
            return new DealerDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Location = entity.Location,
                ContactInfo = entity.ContactInfo,
                RegistrationDate = entity.RegistrationDate,
                IsActive = entity.IsActive
            };
        }
    }
}
