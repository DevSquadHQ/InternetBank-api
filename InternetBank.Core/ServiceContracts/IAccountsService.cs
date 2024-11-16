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
    Task<BlockUnblockDTO> BlockAccountAsync(long accountId);
    Task<BlockUnblockDTO> UnblockAccountAsync(long accountId);

    Task<AccountDetailsResponseDTO> GetAllAccountDetails();
    Task<AccountDetailDTOResponse> GetAccountDetail(long accountId);
    Task<bool> DeleteAccountById(long accountId);

    Task<bool> ForgotAccountPassword(string newPasssowrd , long accountId);
}
