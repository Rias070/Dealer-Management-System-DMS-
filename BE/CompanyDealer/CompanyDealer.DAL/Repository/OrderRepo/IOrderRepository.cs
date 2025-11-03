using CompanyDealer.DAL.Models;

namespace CompanyDealer.DAL.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByOrderNumberAsync(string orderNumber);
        Task AddAsync(Order order);
        Task SaveChangesAsync();
    }
}
