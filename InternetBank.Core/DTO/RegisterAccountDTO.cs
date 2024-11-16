using System;
using System.ComponentModel.DataAnnotations;
using InternetBank.Core.Enums;
namespace InternetBank.Core.DTO;
public class RegisterAccountDTO
{
    [Required]
    public AccountType accountType { get; set; }
    [Required, Range(100000, long.MaxValue, ErrorMessage = "حداقل مقدار برای افتتاح حساب 100000 ریال می باشد")]
    public long Amount { get; set; }
    [Required(ErrorMessage = "اسم بانک که قصد افتتاح حساب دارید باید ارسال شود")]
    public string BankName { get; set; }
}
