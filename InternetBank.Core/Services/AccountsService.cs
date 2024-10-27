using System;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Core.Enums;
using InternetBank.Core.ServiceContracts;
using InternetBank.Core.DTO;
using InternetBank.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace InternetBank.Core.Services;

public class AccountsService : IAccountsService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserService _userService;
	private readonly IPasswordHasher<Account> _passwordHasher;

    public AccountsService(IAccountRepository accountRepository, IUserService userService, IPasswordHasher<Account> passwordHasher)
    {
	    _accountRepository =accountRepository;
	    _userService = userService;
	    _passwordHasher = passwordHasher;
    }

    //Create Account Number with userid
    public string GenerateAccountNumber(AccountType accountType)
    {
	    // Generate random account number based on the user ID and account type
	    string userIdPart = _userService.GetUserId();  //  user ID from the logged-in user
	    string accountTypePart = accountType == AccountType.jari ? "2" : "1";  // 1 for قرض الحسنه, 2 for جاری
	    string randomTwoDigits = new Random().Next(10, 99).ToString();
		string randomPart = new Random().Next(1000, 9999).ToString();

		if (userIdPart.Length < 2)
		{
			userIdPart = "00"+userIdPart; //005
			return $"{randomTwoDigits}.{userIdPart}{randomPart}.{accountTypePart}";
		}
		if (userIdPart.Length<3)
		{
			userIdPart = "0" + userIdPart; //099
			return $"{randomTwoDigits}.{userIdPart}{randomPart}.{accountTypePart}";
		}

		return $"{randomTwoDigits}.{userIdPart}{randomPart}.{accountTypePart}";
    }

    //Create CardNumber
    public string GenerateCardNumber()
    {
	    // Generate random 16-digit card number
	    return $"{new Random().Next(1000, 9999)} {new Random().Next(1000, 9999)} {new Random().Next(1000, 9999)} {new Random().Next(1000, 9999)}";
    }

    //Create CVV2
    public string GenerateCVV2()
    {
	    // Generate random 4-digit CVV2
	    return new Random().Next(1000, 9999).ToString();
    }

    //Create StaticPassword
    public string GenerateStaticPassword()
    {
	    // Generate random 6-digit static password
	    return new Random().Next(100000, 999999).ToString();
    }

    //Create bankAccount for user
	public async Task<RegisterAccountResultDTO> CreateAccount(RegisterAccountDTO registerAccountDTO)
    {
	    var userId = _userService.GetUserId();
		if (string.IsNullOrEmpty(userId))
		{
			return new RegisterAccountResultDTO() { message = "شما باید وارد حساب کاربری خود شوید", success = false };
		}

		//Get number of User bank Account
		long userIdLong = long.Parse(userId); //Change String userId type TO long
		int accountCount = await _accountRepository.GetUserAccountCount(userIdLong);

		if (accountCount >= 2)
		{
			return new RegisterAccountResultDTO() { message = "شما نمی‌توانید بیش از دو حساب داشته باشید", success = false };
		}
		// Create account
		var newAccount = new Account
	    {
		    accountType = registerAccountDTO.accountType,
		    Amount = registerAccountDTO.Amount,
		    AccountNumber = GenerateAccountNumber(registerAccountDTO.accountType),
		    CardNumber = GenerateCardNumber(),
		    Cvv2 = GenerateCVV2(),
		    ExpireDate = DateTime.UtcNow.AddYears(5),
			UserId = userIdLong
	    };
		//Hash Password
	    string staticPassword = GenerateStaticPassword();
	    newAccount.AccountStaticPassword = _passwordHasher.HashPassword(newAccount,staticPassword);
		var accountResponse=await _accountRepository.CreateAccount(newAccount);
        RegisterAccountResponseDTO registerAccountResponseDTO = new RegisterAccountResponseDTO()
        {
            AccountId = accountResponse.AccountId,
            AccountNumber = accountResponse.AccountNumber,
            CardNumber = accountResponse.CardNumber,
            cvv2= accountResponse.Cvv2,
            ExpireDate = accountResponse.ExpireDate,
            AccountStaticPassword = staticPassword,
            Amount = accountResponse.Amount,
            accountType = accountResponse.accountType,
        };
        return new RegisterAccountResultDTO() { success = true, responseAccountDto = registerAccountResponseDTO };
    }

	//Change BankAccountPassword
	public async Task<ChangeAccountPasswordResponseDTO> ChangePassword(ChangeAccountPasswordDto accountPasswordDto)
	{
		//GetAccount If exist
		var account = await _accountRepository.GetAccountById(accountPasswordDto.AccountId);
		if (account == null )
			return new ChangeAccountPasswordResponseDTO() { Message = "حساب پیدا نشد", Success = false ,statusCode = 404};

		//verify the old password
		var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(account, account.AccountStaticPassword, accountPasswordDto.OldPassword);
		if (passwordVerificationResult == PasswordVerificationResult.Failed)
		{
			return new ChangeAccountPasswordResponseDTO() { Message = "رمز قبلی اشتباه است", Success = false ,statusCode = 400};
		}
		if (accountPasswordDto.NewPassword != accountPasswordDto.ConfirmNewPassword)
			return new ChangeAccountPasswordResponseDTO() { Message = "رمز جدید و تکرار آن یکسان نیست", Success = false ,statusCode = 400};


		// Change password logic here
		account.AccountStaticPassword = _passwordHasher.HashPassword(account, accountPasswordDto.NewPassword);
		bool changePasswordResult =await _accountRepository.ChangePassword(account);
		 if (changePasswordResult==false)
		 {
			 return new ChangeAccountPasswordResponseDTO() { Message = "رمز جدید ثبت نشد", Success = false };
		}

		 return new ChangeAccountPasswordResponseDTO() { Message = "رمز حساب با موفقیت تغییر یافت", Success = true };

	}

	// Show the user's balance by accountId
	public async Task<BalanceDTO> GetBalance(long accountId)
	{
		return await _accountRepository.GetBalance(accountId);
	}

	// Blocking an account by accountId
	public async Task BlockAccountAsync(long accountId)
	{
		var account = await _accountRepository.GetByIdAsync(accountId);
		if (account == null)
			throw new Exception(".حساب یافت نشد");

		account.IsBlocked = 1; // Set to 1 for blocked
		await _accountRepository.UpdateAsync(account);
	}
	// Unblocking an account by accountId
	public async Task UnblockAccountAsync(long accountId)
	{
		var account = await _accountRepository.GetByIdAsync(accountId);
		if (account == null)
			throw new Exception(".حساب یافت نشد");

		account.IsBlocked = 0; // Set to 0 for unblocked
		await _accountRepository.UpdateAsync(account);
	}
}
