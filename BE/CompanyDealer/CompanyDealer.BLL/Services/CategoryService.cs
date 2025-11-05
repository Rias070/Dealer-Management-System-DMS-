using CompanyDealer.BLL.DTOs.CategoryDTOs;
using CompanyDealer.BLL.DTOs.DealerDTOs;
using CompanyDealer.BLL.DTOs.VehicleDTOs;
using CompanyDealer.BLL.ExceptionHandle;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.CategoryRepo;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CompanyDealer.BLL.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync();
        Task<CategoryResponse> GetByIdAsync(Guid id);
        Task<CategoryResponse> CreateAsync(CategoryRequest dto);
        Task<CategoryResponse> UpdateAsync(Guid id, CategoryRequest dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly  ICategoryRepository? _categoryRepository;

        public async Task<CategoryResponse> CreateAsync(CategoryRequest dto)
        {
            var Category = new Category
            {
                Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description
            };
            
            
            var created = await _categoryRepository.CreateAsync(Category);

            return new CategoryResponse
            {
                Success = true,
                Id = created.Id,
                Name = created.Name,
                Description = created.Description
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var exists = await _categoryRepository.GetByIdAsync(id);
            if (exists == null)
                throw new ApiException.NotFoundException($"Category with id '{id}' not found");

            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
        {
            var entities = await _categoryRepository.GetAllAsync();
            return (IEnumerable<CategoryResponse>)entities.Select(MapToDto).ToList();
        }

        public async Task<CategoryResponse> GetByIdAsync(Guid id) 
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            if (entity == null)
                throw new ApiException.NotFoundException($"Category with id '{id}' not found");

            return new CategoryResponse
            {
                Success = true,
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public async Task<CategoryResponse> UpdateAsync(Guid id, CategoryRequest dto)
        {
            var existedEntity = await _categoryRepository.GetByIdAsync(id);
            if (existedEntity == null)
                throw new ApiException.NotFoundException($"Category with id '{id}' not found");

            existedEntity.Name = dto.Name;
            existedEntity.Description = dto.Description;

            await _categoryRepository.UpdateAsync(existedEntity);

            return new CategoryResponse
            {
                Success = true,
                Id = existedEntity.Id,
                Name = existedEntity.Name,
                Description = existedEntity.Description
            };
        }

        private static CategoryDto MapToDto(Category entity)
        {
            return new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description

            };
        }
    }
}
