using InternetBank.Core.DTO;
using InternetBank.Core.Identity;
using System;


namespace InternetBank.Core.ServiceContracts
{
	public interface IJwtService
	{
		Task<AuthenticationResponse> CreateJwtToken(ApplicationUser user); 
	}
}
