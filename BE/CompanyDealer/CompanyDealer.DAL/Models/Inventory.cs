using System;
using System.Collections.Generic;

namespace CompanyDealer.DAL.Models
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Quan hệ với Dealer
        public Guid DealerId { get; set; }
        public Dealer Dealer { get; set; } = null!;

        // Liên kết nhiều-nhiều
        public ICollection<InventoryVehicle> InventoryVehicles { get; set; } = new List<InventoryVehicle>();
    }
}
