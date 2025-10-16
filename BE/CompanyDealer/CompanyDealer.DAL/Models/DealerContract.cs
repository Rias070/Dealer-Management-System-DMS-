using System;

namespace CompanyDealer.DAL.Models
{
    public class DealerContract
    {
        public Guid Id { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Terms { get; set; } = string.Empty;
        public decimal ContractValue { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public Guid DealerId { get; set; }
        public Dealer Dealer { get; set; } = null!;
    }
}
