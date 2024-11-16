using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace InternetBank.Core.TokenHandler
{
	public class TokenTypeAuthorizationHandler:AuthorizationHandler<TokenTypeRequirement>
	{
		private readonly IJwtBlacklistService _jwtBlacklistService;

		public TokenTypeAuthorizationHandler(IJwtBlacklistService jwtBlacklistService)
		{
			_jwtBlacklistService = jwtBlacklistService;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenTypeRequirement requirement)
		{


			// Get TokenType
			var tokenType = context.User.Claims.FirstOrDefault(c => c.Type == "type")?.Value;
			if (string.IsNullOrEmpty(tokenType))
			{
				Console.WriteLine("TokenType claim is missing.");
			}




			var httpContext = context.Resource as HttpContext;
			var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
			if (token != null)
			{
				// Checking Blacklist
				if (await _jwtBlacklistService.IsTokenBlacklistedAsync(token))
				{
					context.Fail(); 
					return;
				}
			}



			if (tokenType == requirement.TokenType)
			{
				context.Succeed(requirement); //  Authorization
			}
			else
			{
				context.Fail(); // Authorization
			}

		}
	}
}
