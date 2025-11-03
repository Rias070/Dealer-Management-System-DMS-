using CompanyDealer.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.BLL.DTOs.SaleContractDTOs
{
    public class SaleContractResponse
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
        public DateTime SignDate { get; set; }
        public string Terms { get; set; } = string.Empty;
        public string CustomerSignature { get; set; } = string.Empty;
        public string DealerSignature { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
