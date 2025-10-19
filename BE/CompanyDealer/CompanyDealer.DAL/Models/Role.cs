using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.DAL.Models
{
    public class Role
    {
        public Guid Id { get; set; }

        public string RoleName { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Account> Accounts { get; set; } // many to many
    }
}
