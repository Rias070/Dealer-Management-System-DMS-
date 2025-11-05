using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;

namespace CompanyDealer.DAL.Repository.OrderRepo
{
    public interface IOrderRepository
    {
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<Dealer?> GetDealerByIdAsync(Guid id);
        Task<Vehicle?> GetVehicleByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
        Task SaveChangesAsync();
    }
}
