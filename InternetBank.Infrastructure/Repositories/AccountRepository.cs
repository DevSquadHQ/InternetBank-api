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
// create account repository method
    public async Task<Account> CreateAccount(Account account){
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task<Account> GetAccountById(long accountId)
    {
	    var account = await _context.Accounts.FirstOrDefaultAsync(a =>a.AccountId == accountId);
	    if (account == null)
	    {
		    return account;

	    }
        return account;
    }

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
}
