using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.UserRepo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompanyDealer.BLL.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account?> GetAccountByIdAsync(Guid accountId);
        Task<Account?> UpdateAccountAsync(Account account);
        Task DeleteAccountByIdAsync(Guid accountId);
        Task ActivateAccountAsync(Guid accountId);
        Task DeactivateAccountAsync(Guid accountId);
        Task<IEnumerable<Account>> GetAccountsByDealerIdAsync(Guid dealerId);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(Guid accountId)
        {
            return await _accountRepository.GetByIdAsync(accountId);
        }

        public async Task<Account?> UpdateAccountAsync(Account account)
        {
            var existing = await _accountRepository.GetByIdAsync(account.Id);
            if (existing == null)
                return null;

            existing.Name = account.Name;
            existing.ContactPerson = account.ContactPerson;
            existing.Email = account.Email;
            existing.Phone = account.Phone;
            existing.Address = account.Address;
            existing.IsActive = account.IsActive;
            existing.Username = account.Username;
            existing.Password = account.Password;
            existing.DealerId = account.DealerId;
            // Roles update logic can be added as needed

            await _accountRepository.UpdateAsync(existing);
            return existing;
        }

        public async Task DeleteAccountByIdAsync(Guid accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account != null)
            {
                await _accountRepository.DeleteAsync(account);
            }
        }

        public async Task ActivateAccountAsync(Guid accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account != null && !account.IsActive)
            {
                account.IsActive = true;
                await _accountRepository.UpdateAsync(account);
            }
        }

        public async Task DeactivateAccountAsync(Guid accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account != null && account.IsActive)
            {
                account.IsActive = false;
                await _accountRepository.UpdateAsync(account);
            }
        }

        public async Task<IEnumerable<Account>> GetAccountsByDealerIdAsync(Guid dealerId)
        {
            return await _accountRepository.GetAccountsByDealerIdAsync(dealerId);
        }
    }
}
