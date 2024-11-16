using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class BlockUnblockDTO
	{
		public bool isSuccess { get; set; }
		public string? Message { get; set; } =String.Empty;
	}
}
