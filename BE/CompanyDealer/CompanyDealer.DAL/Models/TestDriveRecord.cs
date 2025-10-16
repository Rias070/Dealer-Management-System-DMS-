using System;

namespace CompanyDealer.DAL.Models
{
    public class TestDriveRecord
    {
        public Guid Id { get; set; }
        public DateTime TestDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerContact { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        // Navigation properties
        public Guid DealerId { get; set; }
        public Dealer Dealer { get; set; } = null!;
        
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
