using CompanyDealer.BLL.DTOs.SaleContractDTOs;
using CompanyDealer.BLL.ExceptionHandle;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.SaleContractRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyDealer.BLL.Services
{
    public interface ISaleContractService
    {
        Task<SaleContractResponse> CreateAsync(CreateSaleContractRequest dto);
        Task<SaleContractResponse?> GetByIdAsync(Guid id);
        Task<IEnumerable<SaleContractResponse>> GetAllAsync();
        Task<bool> DeactivateAsync(Guid id);
        Task<SaleContractResponse> ApproveAsync(Guid id, string dealerSignature);
        Task<SaleContractResponse> RejectAsync(Guid id, string reason);
    }

    public class SaleContractService : ISaleContractService
    {
        private readonly ApplicationDbContext _context;

        public SaleContractService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SaleContractResponse> CreateAsync(CreateSaleContractRequest dto)
        {
            // Validate Order
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .FirstOrDefaultAsync(o => o.Id == dto.OrderId);

            if (order == null)
                throw new KeyNotFoundException("Order not found. Please create the order before adding a contract.");

            // Check if this order already has a contract
            var existingContract = await _context.SaleContracts
                .FirstOrDefaultAsync(c => c.OrderId == dto.OrderId);
            if (existingContract != null)
                throw new InvalidOperationException("A contract already exists for this order.");

            // Create contract
            var contract = new SaleContract
            {
                Id = Guid.NewGuid(),
                ContractNumber = $"CON-{DateTime.UtcNow.Ticks}",
                SignDate = DateTime.UtcNow,
                Terms = dto.Terms,
                CustomerSignature = dto.CustomerSignature,
                OrderId = dto.OrderId,
                IsActive = true
            };

            _context.SaleContracts.Add(contract);
            await _context.SaveChangesAsync();

            return new SaleContractResponse
            {
                Id = contract.Id,
                ContractNumber = contract.ContractNumber,
                SignDate = contract.SignDate,
                Terms = contract.Terms,
                CustomerSignature = contract.CustomerSignature,
                DealerSignature = contract.DealerSignature,
                IsActive = contract.IsActive,
                OrderNumber = order.OrderNumber,
                CustomerName = contract.Order.Customer.Name,
                DealerName = order.Dealer.Name
            };
        }

        public async Task<SaleContractResponse?> GetByIdAsync(Guid id)
        {
            var contract = await _context.SaleContracts
                .Include(c => c.Order)
                    .ThenInclude(o => o.Customer)
                .Include(c => c.Order.Dealer)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return null;

            return new SaleContractResponse
            {
                Id = contract.Id,
                ContractNumber = contract.ContractNumber,
                SignDate = contract.SignDate,
                Terms = contract.Terms,
                CustomerSignature = contract.CustomerSignature,
                DealerSignature = contract.DealerSignature,
                IsActive = contract.IsActive,
                OrderNumber = contract.Order.OrderNumber,
                CustomerName = contract.Order.Customer.Name,
                DealerName = contract.Order.Dealer.Name
            };
        }

        public async Task<IEnumerable<SaleContractResponse>> GetAllAsync()
        {
            return await _context.SaleContracts
                .Include(c => c.Order)
                    .ThenInclude(o => o.Customer)
                .Include(c => c.Order.Dealer)
                .Select(contract => new SaleContractResponse
                {
                    Id = contract.Id,
                    ContractNumber = contract.ContractNumber,
                    SignDate = contract.SignDate,
                    Terms = contract.Terms,
                    CustomerSignature = contract.CustomerSignature,
                    DealerSignature = contract.DealerSignature,
                    IsActive = contract.IsActive,
                    OrderNumber = contract.Order.OrderNumber,
                    CustomerName = contract.Order.Customer.Name,
                    DealerName = contract.Order.Dealer.Name
                })
                .ToListAsync();
        }

        public async Task<bool> DeactivateAsync(Guid id)
        {
            var contract = await _context.SaleContracts
                .Include(c => c.Order)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
                return false;

            contract.IsActive = false;

            contract.Order.Status = "Canceled";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SaleContractResponse> ApproveAsync(Guid id, string dealerSignature)
        {
            var contract = await _context.SaleContracts
                .Include(c => c.Order)
                .ThenInclude(o => o.Dealer)
                .Include(c => c.Order.Customer)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
                throw new KeyNotFoundException("Contract not found.");

            if (contract.Status == "Approved")
                throw new InvalidOperationException("Contract has already been approved.");

            if (contract.Status == "Rejected")
                throw new InvalidOperationException("Contract was previously rejected.");

            contract.Status = "Approved";
            contract.ApprovalDate = DateTime.UtcNow;
            contract.DealerSignature = dealerSignature;
            await _context.SaveChangesAsync();

            return new SaleContractResponse
            {
                Id = contract.Id,
                ContractNumber = contract.ContractNumber,
                Terms = contract.Terms,
                Status = contract.Status,
                ApprovalDate = contract.ApprovalDate,
                DealerSignature = contract.DealerSignature,
                CustomerSignature = contract.CustomerSignature,
                OrderNumber = contract.Order.OrderNumber,
                DealerName = contract.Order.Dealer.Name,
                CustomerName = contract.Order.Customer.Name,
                SignDate = contract.SignDate,
                IsActive = contract.IsActive
            };
        }

        public async Task<SaleContractResponse> RejectAsync(Guid id, string reason)
        {
            var contract = await _context.SaleContracts
                .Include(c => c.Order)
                .ThenInclude(o => o.Dealer)
                .Include(c => c.Order.Customer)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
                throw new KeyNotFoundException("Contract not found.");

            if (contract.Status == "Approved")
                throw new InvalidOperationException("Contract has already been approved and cannot be rejected.");

            contract.Status = "Rejected";
            contract.ApprovalDate = DateTime.UtcNow;
            contract.RejectReason = reason;
            contract.DealerSignature = string.Empty;

            contract.Order.Status = "Canceled";
            contract.Order.TotalAmount = 0;

            await _context.SaveChangesAsync();

            return new SaleContractResponse
            {
                Id = contract.Id,
                ContractNumber = contract.ContractNumber,
                Terms = contract.Terms,
                Status = contract.Status,
                ApprovalDate = contract.ApprovalDate,
                RejectReason = contract.RejectReason,
                DealerSignature = contract.DealerSignature,
                CustomerSignature = contract.CustomerSignature,
                OrderNumber = contract.Order.OrderNumber,
                DealerName = contract.Order.Dealer.Name,
                CustomerName = contract.Order.Customer.Name,
                SignDate = contract.SignDate,
                IsActive = contract.IsActive
            };
        }
    }
}
