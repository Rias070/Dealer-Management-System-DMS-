using System;

namespace CompanyDealer.DAL.Models
{
    public class InventoryVehicle
    {
        public Guid InventoryId { get; set; }
        public Inventory Inventory { get; set; } = null!;

        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
