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
		Task<ApplicationUser?> LoginUser(LoginUserDTO loginUserDto);

		string GetUserId();

	}
}
