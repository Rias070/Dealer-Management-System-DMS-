using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.BLL.DTOs.RestockRequestDTOs
{
    public class FillRestockDto
    {
        public Guid VehicleId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }
}
