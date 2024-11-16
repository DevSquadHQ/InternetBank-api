using System;
using InternetBank;
using InternetBank.Core.Enums;
using InternetBank.Core.Domain.Entities;
using InternetBank.Core.DTO;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace InternetBank.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;
    public AccountRepository(ApplicationDbContext context)
    {
        _context =context;     
    }
	/// <summary>
	/// create account for User
	/// </summary>
	/// <param name="account"></param>
	/// <returns></returns>
	public async Task<Account> CreateAccount(Account account){
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        return account;
    }


	/// <summary>
	/// Get User By Id If exist
	/// </summary>
	/// <param name="accountId"></param>
	/// <returns></returns>
	public async Task<Account> GetAccountById(long accountId)
    {
	    var account = await _context.Accounts.FirstOrDefaultAsync(a =>a.AccountId == accountId);
	    if (account == null)
	    {
		    return account;

	    }
        return account;
    }

	/// <summary>
	/// Update Old password To NewPassword
	/// </summary>
	/// <param name="account"></param>
	/// <returns></returns>
	public async Task<bool> ChangePassword(Account account)
    {
	    _context.Accounts.Update(account);
	   var result =  await _context.SaveChangesAsync();
	   if (result < 1)
	   {
		   return false;
	   }
        return true;
    }

	/// <summary>
	/// Show the user's balance by accountId
	/// </summary>
	/// <param name="accountId"></param>
	/// <returns></returns>
	public async Task<BalanceDTO> GetBalance(long accountId)
    {
	    var account = await _context.Accounts
		    .Where(a => a.AccountId == accountId)
		    .Select(a => new BalanceDTO
		    {
			    AccountId = a.AccountId,
			    Amount = a.Amount,
			    AccountNumber = a.AccountNumber
		    })
		    .FirstOrDefaultAsync();

	    return account;
    }

	/// <summary>
	/// Get Account byId
	/// </summary>
	/// <param name="accountId"></param>
	/// <returns></returns>
    public async Task<Account> GetByIdAsync(long accountId)
    {
	    return await _context.Accounts.FindAsync(accountId);
    }

	/// <summary>
	/// Update ................
	/// </summary>
	/// <param name="account"></param>
	/// <returns></returns>
    public async Task UpdateAsync(Account account)
    {
	    _context.Accounts.Update(account);
	    await _context.SaveChangesAsync();
    }

	/// <summary>
	/// Count For number OF BankAccount User 
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	public async Task<int> GetUserAccountCount(long userId)
    {
	    return await _context.Accounts.CountAsync(a=>a.UserId == userId);
    }

	/// <summary>
	/// Get All UsersAccount With UserId
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
    public async Task<List<AccountDetailsDTO>> GetAllUserAccount(long userId)
    {
	    return await _context.Accounts.Where(account => account.UserId == userId).Select(account => new AccountDetailsDTO()
	    {
            AccountId = account.AccountId,
            AccountNumber = account.AccountNumber,
            CardNumber = account.CardNumber
	    }).ToListAsync();
    }

    /// <summary>
    /// Get Account Details
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public async Task<AccountDetailDTO?> GetAccountDetailById(long accountId)
    {
	    return await _context.Accounts.Where(a => a.AccountId == accountId)
		    .Select(acc => new AccountDetailDTO()
		    {
			    AccountId = acc.AccountId,
			    AccountNumber = acc.AccountNumber,
			    CardNumber = acc.CardNumber,
			    cvv2 = acc.Cvv2,
			    ExpireDate = acc.ExpireDate,
			    accountType = acc.accountType,
                Amount = acc.Amount

		    }).FirstOrDefaultAsync();
    }


	/// <summary>
	/// Delete Account By Id
	/// </summary>
	/// <param name="accountId"></param>
	/// <returns></returns>
	public async Task<bool> DeleteAccountById(long accountId)
    {
	    var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
	    if (account !=null)
	    {
		     _context.Accounts.Remove(account);
		     await _context.SaveChangesAsync();
		     return true;
	    }

	    return false;
    }

	/// <summary>
	/// GetAccountByUSerID 
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	public async Task<List<Account>> GetAccountsByUserIdAsync(long userId)
	{
		return await _context.Accounts.Where(a => a.UserId == userId).ToListAsync();
	}

	/// <summary>
	/// Get account By Card Number
	/// </summary>
	/// <param name="cardNumber"></param>
	/// <returns></returns>
	public async Task<Account> GetAccountByCardNumberAsync(string cardNumber)
	{
		return await _context.Accounts.FirstOrDefaultAsync(a => a.CardNumber == cardNumber);
	}
}
