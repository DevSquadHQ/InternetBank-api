using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class RegisterAccountResultDTO
	{
		public RegisterAccountResponseDTO? responseAccountDto { get; set; }

		public string? message { get; set; }

		public bool success { get; set; }

	}
}
