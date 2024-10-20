using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Core.DTO;
using InternetBank.Core.Identity;
using Microsoft.AspNetCore.Identity;

namespace InternetBank.Infrastructure.Repositories
{
	public class UsersRepository:IUsersRepository
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UsersRepository(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}
		public async Task<IdentityResult> CreateUser(ApplicationUser user, RegisterUserDTO registerUserDto)
		{
			IdentityResult result = await _userManager.CreateAsync(user, registerUserDto.Password);

			return result;
		}

		public async Task<ApplicationUser?> LoginUser(LoginUserDTO loginUserDto)
		{
			ApplicationUser? user = await _userManager.FindByEmailAsync(loginUserDto.Email);
			return user;
		}
	}
}
