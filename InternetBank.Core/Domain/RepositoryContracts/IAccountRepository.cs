using System;
using InternetBank.Core.DTO;
namespace InternetBank.Core.Domain.RepositoryContracts;

public interface IAccountRepository
{
    Task<RegisterAccountDTO> CreateAccount(RegisterAccountDTO registerAccountDTO);
}
