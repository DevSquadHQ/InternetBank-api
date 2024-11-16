using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class AccountDetailDTOResponse
	{
		public AccountDetailDTO? accountDetail { get; set; }
		public string message { get; set; }
		public bool isSuccess { get; set; }
	}
}
