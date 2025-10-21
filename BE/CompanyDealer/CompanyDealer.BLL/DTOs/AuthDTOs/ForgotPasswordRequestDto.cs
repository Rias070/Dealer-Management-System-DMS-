using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.BLL.DTOs.AuthDTOs
{
    public class ForgotPasswordRequestDto
    {
        public string Username { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
