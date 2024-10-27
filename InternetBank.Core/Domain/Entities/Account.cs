using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternetBank.Core.Enums;
using InternetBank.Core.Identity;

namespace InternetBank.Core.Domain.Entities;

public class Account
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long AccountId { get; set; }
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
    public long Amount { get; set; }

    public byte IsBlocked { get; set; }

    public long UserId { get; set; }
    public ApplicationUser User { get; set; }
}
