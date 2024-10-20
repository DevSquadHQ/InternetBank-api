﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class AuthenticationResponse
	{
		public string? FirstName { get; set; } = string.Empty;
		public string? LastName { get; set; } = string.Empty;
		public string? Token { get; set; } = string.Empty;
		public string? Email { get; set; } = string.Empty;

		public DateTime Expiration { get; set; }


	}
}
