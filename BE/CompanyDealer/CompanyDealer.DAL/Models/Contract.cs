using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.DAL.Models
{
    public class Contract
    {
        public Guid ContractId { get; set; } = Guid.NewGuid();

        public Guid RestockRequestId { get; set; }
        public RestockRequest RestockRequest { get; set; }

        

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpirationDate { get; set; }

        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Active"; // Pending / Active / Expired / Cancelled / Completed

        public string? Notes { get; set; }
    }
}
