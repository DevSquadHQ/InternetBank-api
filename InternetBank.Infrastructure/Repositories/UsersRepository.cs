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
		/// <summary>
		/// Check nationalCodeExist Or not
		/// </summary>
		/// <param name="nationalCode"></param>
		/// <returns></returns>
		public async Task<bool> NationalCodeExistsAsync(string nationalCode)
		{
			return await _context.Users.AnyAsync(u => u.NationalCode == nationalCode);
		}

		/// <summary>
		/// DeleteUserByID
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<bool> DeleteUserById(long userId)
		{
			var user = await _context.Users.FindAsync(userId);
			if (user != null)
			{
				await _userManager.DeleteAsync(user);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Get-User---FullInfo
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<ApplicationUser?> GetUserById(long userId)
		{
			return await _context.Users.FindAsync(userId);
		}


		/// <summary>
		/// Create User
		/// </summary>
		/// <param name="user"></param>
		/// <param name="registerUserDto"></param>
		/// <returns></returns>
		public async Task<IdentityResult> CreateUser(ApplicationUser user, RegisterUserDTO registerUserDto)
		{

			IdentityResult result = await _userManager.CreateAsync(user, registerUserDto.Password);
			return result;
		}

		/// <summary>
		/// LoginUser With JWT
		/// </summary>
		/// <param name="loginUserDto"></param>
		/// <returns></returns>
		public async Task<ApplicationUser?> LoginUser(LoginUserDTO loginUserDto)
		{
			ApplicationUser? user = await _userManager.FindByEmailAsync(loginUserDto.Email);
			return user;
		}

	}
}
