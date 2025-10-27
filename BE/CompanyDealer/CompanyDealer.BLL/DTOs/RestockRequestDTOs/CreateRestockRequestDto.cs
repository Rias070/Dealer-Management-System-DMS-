using System;

namespace CompanyDealer.BLL.DTOs.RestockRequestDTOs
{
    public class CreateRestockRequestDto
    {
        public Guid AccountId { get; set; }
        public Guid DealerId { get; set; }
        public Guid VehicleId { get; set; }
        public string VehicleName { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }
}