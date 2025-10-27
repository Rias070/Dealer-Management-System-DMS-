using System;

namespace CompanyDealer.BLL.DTOs.RoleDTOs
{
    public class UpdateRoleDto
    {
        public Guid Id { get; set; }

        public string RoleName { get; set; } = null!;
    }
}