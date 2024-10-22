using System;
using System.ComponentModel.DataAnnotations;
using InternetBank.Core.Enums;
namespace InternetBank.Core.DTO;
public class RegisterAccountDTO
{
    [Required]
    public AccountType accountType { get; set; }
    [Required, Range(100000, int.MaxValue, ErrorMessage = "حداقل مقدار برای افتتاح حساب 10,0000 تومان می باشد")]
    public string Amount { get; set; }
    //sets as a random number
    public string AccountNumber { get; set; }
    //sets as a random number
    public string CardNumber { get; set; }
    //sets as a random number
    public string cvv2 { get; set; }
    //sets as a random number
    public DateTime ExpireDate { get; set; }
    //sets as a random number
    public string AccountStaticPassword { get; set; }

}
