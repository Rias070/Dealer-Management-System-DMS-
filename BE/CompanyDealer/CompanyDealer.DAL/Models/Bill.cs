using System;

namespace CompanyDealer.DAL.Models
{
    public class Bill
    {
        public Guid Id { get; set; }
        public string BillNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        
        // Navigation property
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}
