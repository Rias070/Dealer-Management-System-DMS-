using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs.SaleContractDTOs;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.SaleContractRepo;
using Microsoft.EntityFrameworkCore;

namespace CompanyDealer.BLL.Services
{
    public interface ISaleContractService
    {
        Task<IEnumerable<SaleContractResponse>> GetAllAsync(
            Guid? orderId = null,
            string? contractNumber = null,
            DateTime? signDate = null,
            string? terms = null,
            string? customerSignature = null,
            string? dealerSignature = null,
            bool? isActive = null);

        Task<SaleContractResponse?> GetByIdAsync(Guid id);
        Task<SaleContractResponse?> GetByContractNumberAsync(string contractNumber);
        Task<SaleContractResponse> CreateAsync(CreateSaleContractRequest request);
        Task<SaleContractResponse?> UpdateAsync(Guid id, UpdateSaleContractRequest request);
        Task<bool> DeleteAsync(Guid id);
    }

    public class SaleContractService : ISaleContractService
    {
        private readonly ISaleContractRepository _saleContractRepository;
        private readonly ApplicationDbContext _context;

        public SaleContractService(ISaleContractRepository saleContractRepository, ApplicationDbContext context)
        {
            _saleContractRepository = saleContractRepository;
            _context = context;
        }

        // 🔹 1. Get all with optional filters
        public async Task<IEnumerable<SaleContractResponse>> GetAllAsync(
            Guid? orderId = null,
            string? contractNumber = null,
            DateTime? signDate = null,
            string? terms = null,
            string? customerSignature = null,
            string? dealerSignature = null,
            bool? isActive = null)
        {
            var query = _context.SaleContracts.AsQueryable();

            if (orderId.HasValue)
                query = query.Where(c => c.OrderId == orderId);
            if (!string.IsNullOrEmpty(contractNumber))
                query = query.Where(c => c.ContractNumber.Contains(contractNumber));
            if (signDate.HasValue)
                query = query.Where(c => c.SignDate.Date == signDate.Value.Date);
            if (!string.IsNullOrEmpty(terms))
                query = query.Where(c => c.Terms.Contains(terms));
            if (!string.IsNullOrEmpty(customerSignature))
                query = query.Where(c => c.CustomerSignature.Contains(customerSignature));
            if (!string.IsNullOrEmpty(dealerSignature))
                query = query.Where(c => c.DealerSignature.Contains(dealerSignature));
            if (isActive.HasValue)
                query = query.Where(c => c.IsActive == isActive);

            var contracts = await query
                .Include(c => c.Order)
                .AsNoTracking()
                .ToListAsync();

            return contracts.Select(MapToResponse);
        }

        // 🔹 2. Get by Id
        public async Task<SaleContractResponse?> GetByIdAsync(Guid id)
        {
            var contract = await _saleContractRepository.GetByIdAsync(id);
            return contract == null ? null : MapToResponse(contract);
        }

        // 🔹 3. Get by Contract Number
        public async Task<SaleContractResponse?> GetByContractNumberAsync(string contractNumber)
        {
            var contract = await _saleContractRepository.GetByContractNumberAsync(contractNumber);
            return contract == null ? null : MapToResponse(contract);
        }

        // 🔹 4. Create
        public async Task<SaleContractResponse> CreateAsync(CreateSaleContractRequest request)
        {
            // Prevent duplicate contract number
            var existing = await _saleContractRepository.GetByContractNumberAsync(request.ContractNumber);
            if (existing != null)
                throw new InvalidOperationException($"Contract number '{request.ContractNumber}' already exists.");

            var contract = new SaleContract
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                ContractNumber = request.ContractNumber,
                SignDate = request.SignDate,
                Terms = request.Terms,
                CustomerSignature = request.CustomerSignature,
                DealerSignature = request.DealerSignature,
                IsActive = request.IsActive
            };

            var created = await _saleContractRepository.CreateAsync(contract);
            return MapToResponse(created);
        }

        // 🔹 5. Update
        public async Task<SaleContractResponse?> UpdateAsync(Guid id, UpdateSaleContractRequest request)
        {
            var contract = await _context.SaleContracts.FindAsync(id);
            if (contract == null)
                return null;

            // Update only provided fields
            if (request.SignDate.HasValue)
                contract.SignDate = request.SignDate.Value;
            if (!string.IsNullOrEmpty(request.Terms))
                contract.Terms = request.Terms;
            if (!string.IsNullOrEmpty(request.CustomerSignature))
                contract.CustomerSignature = request.CustomerSignature;
            if (!string.IsNullOrEmpty(request.DealerSignature))
                contract.DealerSignature = request.DealerSignature;
            if (request.IsActive.HasValue)
                contract.IsActive = request.IsActive.Value;

            await _saleContractRepository.UpdateAsync(contract);
            return MapToResponse(contract);
        }

        // 🔹 6. Delete
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _saleContractRepository.DeleteAsync(id);
        }

        // 🔹 Helper: map entity → response
        private static SaleContractResponse MapToResponse(SaleContract c)
        {
            return new SaleContractResponse
            {
                Id = c.Id,
                OrderId = c.OrderId,
                ContractNumber = c.ContractNumber,
                SignDate = c.SignDate,
                Terms = c.Terms,
                CustomerSignature = c.CustomerSignature,
                DealerSignature = c.DealerSignature,
                IsActive = c.IsActive
            };
        }
    }
}
