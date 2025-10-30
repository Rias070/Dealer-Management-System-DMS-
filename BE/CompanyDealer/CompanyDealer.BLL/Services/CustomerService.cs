using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs.CustomerDTOs;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.CustomerRepo;

namespace CompanyDealer.BLL.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<Customer>> GetAll()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.ToList();
        }

        public async Task<Customer?> GetById(Guid id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<Customer> Create(CustomerRequestDto request)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                Dob = request.Dob,
                CreatedAt = DateTime.UtcNow,
                IsActive = request.IsActive
            };
            await _customerRepository.AddAsync(customer);
            return customer;
        }

        public async Task<bool> Update(Guid id, CustomerRequestDto request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return false;

            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.Phone = request.Phone;
            customer.Address = request.Address;
            customer.Dob = request.Dob;
            customer.IsActive = request.IsActive;

            await _customerRepository.UpdateAsync(customer);
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return false;

            await _customerRepository.DeleteAsync(customer);
            return true;
        }
    }
}
