using System;

namespace CompanyDealer.DAL.Models
{
    public class Quotation
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string Details { get; set; } = string.Empty;
        public DateTime ValidUntil { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        
        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
