using System;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Core.Domain.DTO;
using InternetBank.Core.Enums;
using InternetBank.Core.ServiceContracts;
using InternetBank.Core.DTO;
using InternetBank.Core.Domain.Entities;

namespace InternetBank.Core.Services;

public class AccountsService : IAccountsService
{
    private readonly IAccountRepository _accountRepository;
    public AccountsService(IAccountRepository accountRepository)
    {
        _accountRepository =accountRepository;
    }
    public static HashSet<string> existingAccountNumbers = new HashSet<string>();
    //this is for generating random 16 digits number for Card number 
    public string GenerateRandomCardNumber()
    {
        Guid guid = Guid.NewGuid();
        byte[] bytes = guid.ToByteArray();
        long number = BitConverter.ToInt64(bytes, 0) & 0xFFFFFFFFFFFF;
        return number.ToString("D16");
    }
    public string GenerateUniqueAccountNumber()
    {
        string accountNumber;
        do
        {
            accountNumber = GenerateRandomAccountNumber();
        } while (!existingAccountNumbers.Add(accountNumber));
        return accountNumber;
    }

    public string GenerateRandomAccountNumber()
    {
        Guid guid = Guid.NewGuid();
        byte[] bytes = guid.ToByteArray();
        long number = BitConverter.ToInt64(bytes, 0) & 0x7FFFFFFFFFFFFFFF;
        long accountId = (number % 10000000000);
        if (accountId < 1000000000)
        {
            accountId += 1000000000;
        }
        switch (AccountType)
        {
            case accountType.gharzolhasaneh:
                accountNumber = (accountNumber / 10) * 10 + 1;
                break;
            case accountType.jari:
                accountNumber = (accountNumber / 10) * 10 + 2;
                break;
        }
        return accountNumber.ToString();
    }
    // Generate a new CVV2
    public string GenerateRandomCvv2()
    {
        Guid guid = Guid.NewGuid();
        byte[] bytes = guid.ToByteArray();

        int cvv2 = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 10000;
        if (cvv2 < 1000)
        {
            cvv2 += 1000;
        }
        return cvv2.ToString();
    }
    public DateTime GenerateExpireDate()
    {
        DateTime currentDateTime = DateTime.Now.Date;
        DateTime accountExpireDate = currentDateTime.AddYears(5);
        return currentDateTimeExpires;
    }
    public string SetStaticPassword()
    {
        Guid guid = Guid.NewGuid();
        byte[] bytes = guid.ToByteArray();

        int number = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 1000000;

        return number.ToString("D6");
    }
    public async Task<RegisterAccountResponseDTO> CreateAccount(RegisterAccountDTO registerAccountDTO)
    {
        var account = new Account()
        {
            AccountNumber = GenerateUniqueAccountNumber(),
            CardNumber = GenerateRandomCardNumber(),
            Cvv2 = GenerateRandomCvv2(),
            ExpireDate = GenerateExpireDate(),
            AccountStaticPassword = SetStaticPassword(),
            Amount = registerAccountDTO.Amount,
            accountType = registerAccountDTO.accountType,
        };
        var accountResponse=await _accountRepository.CreateAccount(account);
        RegisterAccountResponseDTO registerAccountResponseDTO = new RegisterAccountResponseDTO()
        {
            AccountNumber = accountResponse.AccountNumber,
            CardNumber = accountResponse.CardNumber,
            cvv2= accountResponse.cvv2,
            ExpireDate = accountResponse.ExpireDate,
            AccountStaticPassword = accountResponse.AccountStaticPassword,
            Amount = accountResponse.Amount,
            accountType = accountResponse.accountType,
        };
        return registerAccountResponseDTO;
    }
}
