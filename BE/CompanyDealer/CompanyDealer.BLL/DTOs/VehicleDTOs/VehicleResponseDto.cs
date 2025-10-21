using System;
using CompanyDealer.BLL.DTOs;

namespace CompanyDealer.BLL.DTOs.VehicleDTOs
{
    public class VehicleResponseDto
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        // Id of the created/updated/deleted vehicle when applicable
        public Guid VehicleId { get; set; }

        public VehicleDto Vehicle { get; set; }
    }
}