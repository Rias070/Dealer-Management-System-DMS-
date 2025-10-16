using System;
using System.Collections.Generic;

namespace CompanyDealer.DAL.Models
{
    public class Dealer
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<DealerContract> DealerContracts { get; set; } = new List<DealerContract>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<TestDriveRecord> TestDriveRecords { get; set; } = new List<TestDriveRecord>();
        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
