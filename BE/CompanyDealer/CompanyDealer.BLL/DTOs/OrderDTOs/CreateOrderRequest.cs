using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.BLL.DTOs.OrderDTOs
{
    public class CreateOrderRequest
    {
        public Guid DealerId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid VehicleId { get; set; }

        public decimal VehicleAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerContact { get; set; } = string.Empty;
    }

}
