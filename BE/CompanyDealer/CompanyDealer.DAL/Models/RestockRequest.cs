using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.DAL.Models
{
    public class RestockRequest
    {
        public Guid id { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;
        public Guid DealerId { get; set; }
        public Dealer Dealer { get; set; } = null!;
        public Guid VehicleId { get; set; }
        public String VehicleName { get; set; } = null!;
        public Vehicle Vehicle { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public DateTime ResponseDate { get; set; }
        public string AcceptenceLevel { get; set; } = "Dealer"; // Dealer, Company
        public string AcceptedBy { get; set; }
        public string ReasonRejected { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Completed
        public string Description { get; set; } 

    }
}
