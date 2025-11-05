using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.BLL.DTOs.SaleContractDTOs
{
    public class CreateSaleContractRequest
    {
        public Guid OrderId { get; set; }
        public string Terms { get; set; } = string.Empty;
        public string CustomerSignature { get; set; } = string.Empty;
    }
}
