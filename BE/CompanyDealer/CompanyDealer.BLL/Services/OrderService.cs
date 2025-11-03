using CompanyDealer.BLL.DTOs.OrderDTOs;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository;

namespace CompanyDealer.BLL.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        Task<OrderResponse?> GetByIdAsync(Guid id);
        Task<OrderResponse?> GetByOrderNumberAsync(string orderNumber);
        Task<OrderResponse> CreateAsync(CreateOrderRequest request);
        Task<OrderResponse?> UpdateAsync(Guid id, UpdateOrderRequest request);
        Task<bool> DeleteAsync(Guid id);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;

        public OrderService(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            return orders.Select(o => new OrderResponse
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CustomerName = o.CustomerName,
                CustomerContact = o.CustomerContact,
                DealerId = o.DealerId,
                DealerName = o.Dealer?.Name
            });
        }

        public async Task<OrderResponse?> GetByIdAsync(Guid id)
        {
            var o = await _orderRepo.GetByIdAsync(id);
            if (o == null) return null;

            return new OrderResponse
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CustomerName = o.CustomerName,
                CustomerContact = o.CustomerContact,
                DealerId = o.DealerId,
                DealerName = o.Dealer?.Name
            };
        }

        public async Task<OrderResponse?> GetByOrderNumberAsync(string orderNumber)
        {
            var o = await _orderRepo.GetByOrderNumberAsync(orderNumber);
            if (o == null) return null;

            return new OrderResponse
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CustomerName = o.CustomerName,
                CustomerContact = o.CustomerContact,
                DealerId = o.DealerId,
                DealerName = o.Dealer?.Name
            };
        }

        public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = request.OrderNumber,
                OrderDate = request.OrderDate,
                TotalAmount = request.TotalAmount,
                Status = request.Status,
                CustomerName = request.CustomerName,
                CustomerContact = request.CustomerContact,
                DealerId = request.DealerId
            };

            var created = await _orderRepo.CreateAsync(order);
            return await GetByIdAsync(created.Id) ?? throw new Exception("Failed to retrieve created order");
        }

        public async Task<OrderResponse?> UpdateAsync(Guid id, UpdateOrderRequest request)
        {
            var existing = await _orderRepo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.TotalAmount = request.TotalAmount;
            existing.Status = request.Status;
            existing.CustomerName = request.CustomerName;
            existing.CustomerContact = request.CustomerContact;

            var updated = await _orderRepo.UpdateAsync(existing);
            return updated == null ? null : await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _orderRepo.DeleteAsync(id);
        }
    }
}
