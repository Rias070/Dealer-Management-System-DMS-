using CompanyDealer.BLL.DTOs.Order;
using CompanyDealer.DAL.Interfaces;
using CompanyDealer.DAL.Models;

namespace CompanyDealer.BLL.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        Task<OrderResponse?> GetByOrderNumberAsync(string orderNumber);
        Task<OrderResponse> CreateAsync(OrderCreateRequest request);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
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
                DealerName = o.Dealer?.Name ?? string.Empty
            });
        }

        public async Task<OrderResponse?> GetByOrderNumberAsync(string orderNumber)
        {
            var order = await _orderRepository.GetByOrderNumberAsync(orderNumber);
            if (order == null) return null;

            return new OrderResponse
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CustomerName = order.CustomerName,
                CustomerContact = order.CustomerContact,
                DealerId = order.DealerId,
                DealerName = order.Dealer?.Name ?? string.Empty
            };
        }

        public async Task<OrderResponse> CreateAsync(OrderCreateRequest request)
        {
            var order = new Order
            {
                OrderNumber = request.OrderNumber,
                TotalAmount = request.TotalAmount,
                Status = request.Status,
                CustomerName = request.CustomerName,
                CustomerContact = request.CustomerContact,
                DealerId = request.DealerId
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return new OrderResponse
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CustomerName = order.CustomerName,
                CustomerContact = order.CustomerContact,
                DealerId = order.DealerId,
                DealerName = order.Dealer?.Name ?? string.Empty
            };
        }
    }
}
