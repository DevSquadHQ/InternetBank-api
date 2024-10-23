using InternetBank.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
    public class RegisterAccountResponseDTO
    {
        public AccountType accountType { get; set; }
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
}
