using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.DTO;
using InternetBank.Core.Identity;
using Microsoft.AspNetCore.Identity;

namespace InternetBank.Core.ServiceContracts
{
	public  interface IUserService
	{
		Task<RegisterResultDTO> AddUser(RegisterUserDTO registerUserDto);
		Task<AuthenticationResponse?> LoginUser(LoginUserDTO loginUserDto);

		string GetUserId();

		Task<bool> OtpVerified();


		Task<bool>DeleteUser();



	}
}
