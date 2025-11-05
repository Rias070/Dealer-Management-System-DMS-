using System;
using System.Collections.Generic;

namespace CompanyDealer.DAL.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerContact { get; set; } = string.Empty;
        
        // Navigation properties

        //Dealer
        public Guid DealerId { get; set; }
        public Dealer Dealer { get; set; } = null!;

        //Customer
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        
        //Vehicles
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public decimal VehicleAmount { get; set; }

        //Sale Contract
        public SaleContract? SaleContract { get; set; }
        
        //Bill
        public Bill? Bill { get; set; }

        // Collections
        public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
        public ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
    }
}
