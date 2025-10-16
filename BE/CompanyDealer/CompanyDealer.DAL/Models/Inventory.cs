using System;
using System.Collections.Generic;

namespace CompanyDealer.DAL.Models
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public Guid DealerId { get; set; }
        public Dealer Dealer { get; set; } = null!;
        
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
