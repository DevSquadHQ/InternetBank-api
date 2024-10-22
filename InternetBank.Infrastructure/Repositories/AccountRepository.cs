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
    private static HashSet<string> existingAccountNumbers = new HashSet<string>();
    //this is for generating random 16 digits number for Card number 
    private string GenerateRandomCardNumber()
    {
        Guid guid = Guid.NewGuid();
        byte[] bytes = guid.ToByteArray();
        long number = BitConverter.ToInt64(bytes, 0) & 0xFFFFFFFFFFFF;
        return number.ToString("D16");
    }
    // this is generating Account Number
    // Return the unique account number
    private string GenerateUniqueAccountNumber()
    {
        string accountNumber;
        do
        {
            accountNumber = GenerateRandomAccountNumber();
        } while (!existingAccountNumbers.Add(accountNumber));
        return accountNumber;
    }

    private string GenerateRandomAccountNumber()
    {
        Guid guid = Guid.NewGuid();
        byte[] bytes = guid.ToByteArray();
        long number = BitConverter.ToInt64(bytes, 0) & 0x7FFFFFFFFFFFFFFF;
        long accountId = (number % 10000000000);
        if (accountId < 1000000000)
        {
            accountId += 1000000000;
        }
        switch (accountType)
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
    private string GenerateRandomCvv2()
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
    private DateTime GenerateExpireDate()
    {
        DateTime currentDateTime = DateTime.Now.Date;
        DateTime accountExpireDate = currentDateTime.AddYears(5);
        return currentDateTimeExpires;
    }
    private string SetStaticPassword()
    {
        Guid guid = Guid.NewGuid();
        byte[] bytes = guid.ToByteArray();

        int number = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 1000000;

        return number.ToString("D6");
    }
    // create account repository method
    public async Task<RegisterAccountDTO> CreateAccount(RegisterAccountDTO registerAccountDTO){
        var account = new Account(){
            AccountNumber =GenerateUniqueAccountNumber(),
            CardNumber = GenerateRandomCardNumber(),
            Cvv2 = GenerateRandomCvv2(),
            ExpireDate = GenerateExpireDate(),
            AccountStaticPassword = SetStaticPassword(),
            Amount = registerAccountDTO.Amount,
            accountType = registerAccountDTO.accountType,
        }; 
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        return account.AccountId;
    }

}
