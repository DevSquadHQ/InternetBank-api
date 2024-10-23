using InternetBank.Core.Domain.Entities;
using InternetBank.Core.DTO;
using System;

namespace InternetBank.Core.ServiceContracts;

public interface IAccountsService
{
    string GenerateRandomCardNumber();
    string GenerateUniqueAccountNumber();
    string GenerateRandomAccountNumber();
    string GenerateRandomCvv2();
    DateTime GenerateExpireDate();
    string SetStaticPassword();
    Task<RegisterAccountResponseDTO> CreateAccount(RegisterAccountDTO registerAccountDTO);
}
