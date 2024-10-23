using System;
using InternetBank.Core.Domain.Entities;
using InternetBank.Core.DTO;
namespace InternetBank.Core.Domain.RepositoryContracts;

public interface IAccountRepository
{
    Task<Account> CreateAccount(Account account);
}
