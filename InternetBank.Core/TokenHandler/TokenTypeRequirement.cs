using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace InternetBank.Core.TokenHandler
{
	public class TokenTypeRequirement :IAuthorizationRequirement
	{
		public string TokenType { get; }

		public TokenTypeRequirement(string tokenType)
		{
			TokenType = tokenType;
		}
	}
}
