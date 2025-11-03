using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CompanyDealer.DAL.Repository.ContractRepo
{
    public class ContractRepository : GenericRepository<Contract>, IContractRepository
    {
        public ContractRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Contract>> GetByDealerId(Guid dealerId)
        {
            return await _context.Set<Contract>()
                                 .Where(c => c.DealerId == dealerId)
                                 .ToListAsync();
        }
    }
}
