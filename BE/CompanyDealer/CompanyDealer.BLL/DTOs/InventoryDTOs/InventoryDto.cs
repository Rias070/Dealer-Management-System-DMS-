using System;

namespace CompanyDealer.BLL.DTOs.InventoryDTOs
{
    public class InventoryDto
    {
        public Guid Id { get; set; }
        public DateTime LastUpdated { get; set; }
        public Guid DealerId { get; set; }
    }
}