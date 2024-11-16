using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class ForgotPasswordAccountDTO
	{
		public long accountId { get; set; }
		public string newPassword { get; set; }
	}
}
