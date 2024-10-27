using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class BalanceDTO
	{
		public long AccountId { get; set; }
		public long Amount { get; set; }
		public string AccountNumber { get; set; }
	}
}
