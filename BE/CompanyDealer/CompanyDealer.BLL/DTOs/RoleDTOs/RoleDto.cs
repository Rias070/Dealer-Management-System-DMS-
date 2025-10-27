using CompanyDealer.BLL.DTOs.RoleDTOs;
using System;

namespace CompanyDealer.BLL.DTOs.RoleDTOs
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        public string RoleName { get; set; } = null!;
    }
}