using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.DTO;
using InternetBank.Core.Identity;

namespace InternetBank.Core.Domain.RepositoryContracts
{
	public interface IUsersRepository
	{
		Task<IdentityResult> CreateUser(ApplicationUser user , RegisterUserDTO registerUserDto);

		Task<ApplicationUser?> LoginUser(LoginUserDTO loginUserDto);

		Task<bool> NationalCodeExistsAsync(string nationalCode);
	}
}
