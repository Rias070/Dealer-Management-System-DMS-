using CompanyDealer.BLL.DTOs.OrderDTOs;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.OrderRepo;
using Microsoft.EntityFrameworkCore;

namespace CompanyDealer.BLL.Services
{   
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        Task<OrderResponse?> GetByIdAsync(Guid id);
        Task<OrderResponse> CreateAsync(CreateOrderRequest dto);
        Task<OrderResponse?> UpdateAsync(Guid id, UpdateOrderRequest dto);
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
            return orders.Select(MapToResponse);
        }

        public async Task<OrderResponse?> GetByIdAsync(Guid id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            return order == null ? null : MapToResponse(order);
        }

        public async Task<OrderResponse> CreateAsync(CreateOrderRequest  dto)
        {
            // --- Validate Customer ---
            var customer = await _orderRepo.GetCustomerByIdAsync(dto.CustomerId);
            if (customer == null)
                throw new KeyNotFoundException("Customer not found");

            // --- Validate Dealer ---
            var dealer = await _orderRepo.GetDealerByIdAsync(dto.DealerId);
            if (dealer == null)
                throw new KeyNotFoundException("Dealer not found.");

            // --- Validate Vehicle ---
            var vehicle = await _orderRepo.GetVehicleByIdAsync(dto.VehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException("Vehicle not found.");

            // --- Create Order ---
            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = $"ORD-{DateTime.UtcNow.Ticks}",
                OrderDate = DateTime.UtcNow,
                DealerId = dto.DealerId,
                CustomerId = dto.CustomerId,
                VehicleId = dto.VehicleId,
                VehicleAmount = dto.VehicleAmount,
                TotalAmount = vehicle.Price * dto.VehicleAmount,
                Status = "Waiting For Approval",
                CustomerName = dto.CustomerName ?? customer.Name,
                CustomerContact = dto.CustomerContact ?? customer.Phone,
            };

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveChangesAsync();

            return MapToResponse(order);
        }

        public async Task<OrderResponse?> UpdateAsync(Guid id, UpdateOrderRequest dto)
        {
            var existing = await _orderRepo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.TotalAmount = dto.TotalAmount;
            existing.Status = dto.Status;
            existing.CustomerName = dto.CustomerName;
            existing.CustomerContact = dto.CustomerContact;

            await _orderRepo.UpdateAsync(existing);
            await _orderRepo.SaveChangesAsync();

            return MapToResponse(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return false;

            await _orderRepo.DeleteAsync(order);
            await _orderRepo.SaveChangesAsync();
            return true;
        }

        private static OrderResponse MapToResponse(Order o) => new()
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            Status = o.Status,
            CustomerName = o.Customer?.Name ?? "",
            CustomerContact = o.Customer?.Phone ?? o.Customer?.Email ?? "",
            DealerName = o.Dealer?.Name ?? "",
            VehicleModel = o.Vehicle?.Model ?? ""
        };
    }
}
