using System;
using InternetBank;
using InternetBank.Core.Enums;
using InternetBank.Core.Domain.Entities;
using InternetBank.Core.DTO;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Infrastructure.DbContext;
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
       return await _context.SaveChangesAsync();
    }

}
