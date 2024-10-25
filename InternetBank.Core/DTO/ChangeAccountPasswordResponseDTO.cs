using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class ChangeAccountPasswordResponseDTO
	{
		public bool Success { get; set; }
		public string Message { get; set; }

		public int statusCode { get; set; }
	}
}
