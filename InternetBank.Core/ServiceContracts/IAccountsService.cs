using InternetBank.Core.DTO;
using InternetBank.Core.Enums;
using System;

namespace InternetBank.Core.ServiceContracts;

public interface IAccountsService
{
	string GenerateAccountNumber(AccountType accountType);

	string GenerateCardNumber();

	string GenerateCVV2();
	string GenerateStaticPassword();
    Task<RegisterAccountResultDTO> CreateAccount(RegisterAccountDTO registerAccountDTO);
    Task<ChangeAccountPasswordResponseDTO> ChangePassword(ChangeAccountPasswordDto accountPasswordDto);
    Task<BalanceDTO> GetBalance(long AccountId);
    Task BlockAccountAsync(long accountId);
    Task UnblockAccountAsync(long accountId);

}
