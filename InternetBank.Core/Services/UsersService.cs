using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Core.DTO;
using InternetBank.Core.Identity;
using InternetBank.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternetBank.Core.Services
{
	public class UsersService:IUserService
	{
		private readonly IUsersRepository _usersRepository;
		private readonly IHttpContextAccessor _contextAccessor;

		public UsersService(IUsersRepository usersRepository, IHttpContextAccessor contextAccessor)
		{
			_usersRepository = usersRepository;
			_contextAccessor = contextAccessor;
		}
		public async Task<RegisterResultDTO> AddUser(RegisterUserDTO registerUserDto)
		{

			//Create User
			ApplicationUser user = new ApplicationUser()
			{
				Email = registerUserDto.Email,
				PhoneNumber = registerUserDto.PhoneNumber,
				FirstName = registerUserDto.FirstName,
				LastName = registerUserDto.LastName,
				NationalCode = registerUserDto.NationalCode,
				Birthdate = registerUserDto.Birthdate,
				UserName = registerUserDto.Email
			};
			if (await _usersRepository.NationalCodeExistsAsync(user.NationalCode))
			{
				return new RegisterResultDTO(){ErrorMessage = "کد ملی اشتباه است یا قبلا ثبت شده" ,result = IdentityResult.Failed(),user = null};
			}

			var result=  await _usersRepository.CreateUser(user, registerUserDto);
			return new RegisterResultDTO(){result = result , user = user};
		}



		/// <summary>
		/// Login User Service 
		/// </summary>
		/// <param name="loginUserDto"></param>
		/// <returns>User object </returns>
		public async Task<ApplicationUser?> LoginUser(LoginUserDTO loginUserDto)
		{
			return await _usersRepository.LoginUser(loginUserDto);
		}

		//Get UserID from existing user
		public string GetUserId()
		{
			return _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		}
	}
}
