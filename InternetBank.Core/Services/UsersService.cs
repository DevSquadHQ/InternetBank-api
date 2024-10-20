using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Core.DTO;
using InternetBank.Core.Identity;
using InternetBank.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace InternetBank.Core.Services
{
	public class UsersService:IUserService
	{
		private readonly IUsersRepository _usersRepository;

		public UsersService(IUsersRepository usersRepository)
		{
			_usersRepository = usersRepository;
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

	}
}
