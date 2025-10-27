using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs.RoleDTOs;

namespace CompanyDealer.BLL.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();

        Task<RoleDto?> GetByIdAsync(Guid id);

        Task<RoleDto?> CreateAsync(CreateRoleDto request);

        Task<RoleDto?> UpdateAsync(UpdateRoleDto request);

        Task<bool> DeleteAsync(Guid id);
    }
}