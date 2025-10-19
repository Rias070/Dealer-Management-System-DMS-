using System;
using System.Collections.Generic;

namespace CompanyDealer.DAL.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string VIN { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;

        // Quan hệ
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // Liên kết nhiều-nhiều
        public ICollection<InventoryVehicle> InventoryVehicles { get; set; } = new List<InventoryVehicle>();

        // Lịch sử lái thử
        public ICollection<TestDriveRecord> TestDriveRecords { get; set; } = new List<TestDriveRecord>();
    }
}
