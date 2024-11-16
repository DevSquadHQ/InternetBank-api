using System;
using InternetBank.Core.Domain.Entities;
using InternetBank.Core.DTO;
namespace InternetBank.Core.Domain.RepositoryContracts;

public interface IAccountRepository
{
    Task<Account> CreateAccount(Account account);
    Task<Account> GetAccountById(long accountId);
    Task<bool> ChangePassword(Account account);
    Task<BalanceDTO> GetBalance(long accountId);
    Task<Account> GetByIdAsync(long accountId);
    Task UpdateAsync(Account account);
    Task<int> GetUserAccountCount(long userId);
    Task<List<AccountDetailsDTO>> GetAllUserAccount(long userId);

    Task<AccountDetailDTO> GetAccountDetailById(long accountId);
    Task<bool> DeleteAccountById(long accountId);
    Task<List<Account>> GetAccountsByUserIdAsync(long userId);
    Task<Account> GetAccountByCardNumberAsync(string cardNumber);

}
