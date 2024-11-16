using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class AccountDetailsDTO
	{
		public long AccountId { get; set; }
		public string AccountNumber { get; set; }
		public string CardNumber { get; set; }
	}
}
