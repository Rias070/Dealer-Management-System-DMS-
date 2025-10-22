using System;

namespace CompanyDealer.BLL.DTOs.VehicleDTOs
{
    public class VehicleCreateUpdateDto
    {
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public string VIN { get; set; } = null!;
        public string Color { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public Guid CategoryId { get; set; }
    }
}