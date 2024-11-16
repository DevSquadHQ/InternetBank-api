using DNTPersianUtils.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class TransferMoneyRequestDTO
	{
		public long Amount { get; set; }
		public string OtpCode { get; set; }

	}
}
