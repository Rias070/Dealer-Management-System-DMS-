using System;

namespace CompanyDealer.BLL.DTOs.CustomerDTOs
{
    public class CustomerRequestDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? Dob { get; set; }
        public bool IsActive { get; set; }
    }
}
