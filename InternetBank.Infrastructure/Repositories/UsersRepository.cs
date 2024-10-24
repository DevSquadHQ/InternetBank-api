using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Core.DTO;
using InternetBank.Core.Identity;
using InternetBank.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace InternetBank.Infrastructure.Repositories
{
	public class UsersRepository:IUsersRepository
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;

		public UsersRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}
		public async Task<bool> NationalCodeExistsAsync(string nationalCode)
		{
			return await _context.Users.AnyAsync(u => u.NationalCode == nationalCode);
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
