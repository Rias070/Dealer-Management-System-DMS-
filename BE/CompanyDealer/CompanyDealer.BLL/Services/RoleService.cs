using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs.RoleDTOs;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.RoleRepo;

namespace CompanyDealer.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var roles = await _roleRepository.FindAsync(r => !r.IsDeleted);
            return roles.Select(r => new RoleDto { Id = r.Id, RoleName = r.RoleName });
        }

        public async Task<RoleDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) return null;
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null || role.IsDeleted) return null;
            return new RoleDto { Id = role.Id, RoleName = role.RoleName };
        }

        public async Task<RoleDto?> CreateAsync(CreateRoleDto request)
        {
            if (string.IsNullOrWhiteSpace(request?.RoleName)) return null;

            var existing = await _roleRepository.GetByNameAsync(request.RoleName);
            if (existing != null && !existing.IsDeleted)
                return null; // already exists

            var role = new Role
            {
                Id = Guid.NewGuid(),
                RoleName = request.RoleName.Trim(),
                IsDeleted = false
            };

            await _roleRepository.AddAsync(role);
            return new RoleDto { Id = role.Id, RoleName = role.RoleName };
        }

        public async Task<RoleDto?> UpdateAsync(UpdateRoleDto request)
        {
            if (request == null || request.Id == Guid.Empty) return null;

            var role = await _roleRepository.GetByIdAsync(request.Id);
            if (role == null || role.IsDeleted) return null;

            role.RoleName = request.RoleName.Trim();
            await _roleRepository.UpdateAsync(role);

            return new RoleDto { Id = role.Id, RoleName = role.RoleName };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty) return false;

            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null || role.IsDeleted) return false;

            // soft-delete
            role.IsDeleted = true;
            await _roleRepository.UpdateAsync(role);
            return true;
        }
    }
}