using System;

namespace CompanyDealer.BLL.DTOs.InventoryDTOs
{
    public class InventoryResponseDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Guid? InventoryId { get; set; }
        public InventoryDto? Inventory { get; set; }
    }
}