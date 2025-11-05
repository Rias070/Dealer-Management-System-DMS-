using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.BLL.DTOs.SaleContractDTOs
{
    public class UpdateSaleContractRequest
    {
        public DateTime? SignDate { get; set; }
        public string? Terms { get; set; }
        public string? CustomerSignature { get; set; }
        public string? DealerSignature { get; set; }
        public bool? IsActive { get; set; }
    }
}
