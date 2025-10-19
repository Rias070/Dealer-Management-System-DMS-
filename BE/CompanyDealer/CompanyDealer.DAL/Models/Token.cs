using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.DAL.Models
{
    public class Token
    {
        public Guid Id { get; set; }

        public string RefreshToken { get; set; }

        public Guid AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
