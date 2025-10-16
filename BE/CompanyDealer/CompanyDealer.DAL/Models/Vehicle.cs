using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
        // Navigation properties
        public Guid InventoryId { get; set; }
        public Inventory Inventory { get; set; } = null!;
        
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        
        public ICollection<TestDriveRecord> TestDriveRecords { get; set; } = new List<TestDriveRecord>();
    }
}
