using CompanyDealer.DAL.Models;

namespace CompanyDealer.DAL.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task<Order?> GetByOrderNumberAsync(string orderNumber);
        Task<Order> CreateAsync(Order order);
        Task<Order?> UpdateAsync(Order order);
        Task<bool> DeleteAsync(Guid id);
    }
}
