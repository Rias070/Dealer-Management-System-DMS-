using CompanyDealer.DAL.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.DAL.Repository.ContractRepo
{
    public interface IContractRepository : IGenericRepository<Contract>
    {
        
    }
}
