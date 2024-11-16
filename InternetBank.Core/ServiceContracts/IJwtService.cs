using InternetBank.Core.DTO;
using InternetBank.Core.Identity;
using System;
using System.Security.Claims;


namespace InternetBank.Core.ServiceContracts
{
	public interface IJwtService
	{
		Task<string> CreateJwtToken(List<Claim>claimList,DateTime Expiration); 
	}
}
