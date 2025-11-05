using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyDealer.DAL.Repository.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Dealer)
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.Dealer)
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Order order)
        {
            _context.Orders.Remove(order);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<Customer?> GetCustomerByIdAsync(Guid id)
        => _context.Customers.FirstOrDefaultAsync(c => c.Id == id);

        public Task<Dealer?> GetDealerByIdAsync(Guid id)
            => _context.Dealers.FirstOrDefaultAsync(d => d.Id == id);

        public Task<Vehicle?> GetVehicleByIdAsync(Guid id)
            => _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
    }
}
