using System;
using InternetBank.Core.Enums;
namespace InternetBank.Core.Domain.Entities;

public class Account
{
    //sets as a random number
    public int AccountId { get; set; }
    //sets as a random number
    public string AccountNumber { get; set; }
    //sets as a random number
    public string CardNumber { get; set; }
    //sets as a random number
    public string Cvv2 { get; set; }
    //sets as a random number
    public DateTime ExpireDate { get; set; }
    public AccountType accountType { get; set; }
    
    //sets as a random number
    public string AccountStaticPassword { get; set; }
    public string Amount { get; set; }
}
