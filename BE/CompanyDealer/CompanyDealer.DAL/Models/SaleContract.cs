using System;

namespace CompanyDealer.DAL.Models
{
    public class SaleContract
    {
        public Guid Id { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
        public DateTime SignDate { get; set; } = DateTime.UtcNow;
        public string Terms { get; set; } = string.Empty;
        public string CustomerSignature { get; set; } = string.Empty;
        public string DealerSignature { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        
        // Navigation property
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}
