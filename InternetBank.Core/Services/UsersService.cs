using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
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
using Microsoft.Extensions.Configuration;

namespace InternetBank.Core.Services
{
	public class UsersService:IUserService
	{
		private readonly IUsersRepository _usersRepository;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IJwtService _jwtService;
		private readonly IConfiguration _configuration;
		private readonly IJwtBlacklistService _blacklistService;

		public UsersService(IUsersRepository usersRepository, IHttpContextAccessor contextAccessor, IJwtService jwtService, IConfiguration configuration, IJwtBlacklistService blacklistService)
		{
			_usersRepository = usersRepository;
			_contextAccessor = contextAccessor;
			_jwtService = jwtService;
			_configuration = configuration;
			_blacklistService = blacklistService;
		}

		/// <summary>
		/// Add user TO DB
		/// </summary>
		/// <param name="registerUserDto"></param>
		/// <returns></returns>
		public async Task<RegisterResultDTO> AddUser(RegisterUserDTO registerUserDto)
		{

			var parsedDate = DateTime.SpecifyKind(registerUserDto.Birthdate, DateTimeKind.Utc);
			//Create User
			ApplicationUser user = new ApplicationUser()
			{
				Email = registerUserDto.Email,
				PhoneNumber = registerUserDto.PhoneNumber,
				FirstName = registerUserDto.FirstName,
				LastName = registerUserDto.LastName,
				NationalCode = registerUserDto.NationalCode,
				Birthdate = parsedDate,
				UserName = registerUserDto.Email,
				IsActive = true
			};
			if (await _usersRepository.NationalCodeExistsAsync(user.NationalCode))
			{
				return new RegisterResultDTO(){ErrorMessage = "کد ملی اشتباه است یا قبلا ثبت شده" ,result = IdentityResult.Failed(),user = null};
			}

			var result=  await _usersRepository.CreateUser(user, registerUserDto);
			return new RegisterResultDTO(){result = result , user = user };
		}



		/// <summary>
		/// Login User Service 
		/// </summary>
		/// <param name="loginUserDto"></param>
		/// <returns>User object </returns>
		public async Task<AuthenticationResponse?> LoginUser(LoginUserDTO loginUserDto)
		{
			var user =  await _usersRepository.LoginUser(loginUserDto);

			if (user == null)
			{
				return null;
			}
			//Payload
			//Create an array of Claim object representing the user's claims,such as their ID,name,email,etc.
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim("type", "login"),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT unique ID

			};
			DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));

			var tokenResponse =  await _jwtService.CreateJwtToken(claims , expiration);

			return new AuthenticationResponse() { user = user, token = tokenResponse };
		}

		//Get UserID from existing user
		public string GetUserId()
		{
			return _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		}

		/// <summary>
		/// Verify Otp Code
		/// </summary>
		/// <returns></returns>
		public async Task<bool> OtpVerified()
		{
			var claimsPrincipal = _contextAccessor.HttpContext?.User;
			if (claimsPrincipal == null)
			{
				return false;
			}

			var otpVerifiedClaim =
				claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "OtpVerified" && c.Value == "true");
			return otpVerifiedClaim != null;
		}

		/// <summary>
		/// Delete User With UserId in Jwt
		/// </summary>
		/// <returns></returns>
		public async Task<bool> DeleteUser()
		{
			//taken Token
			var token = _contextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")
				.Last();
			if (!string.IsNullOrEmpty(token))
			{
				//Added To blackList in Redis
				await _blacklistService.AddToBlacklistAsync(token, TimeSpan.FromHours(1));
			}

			var userId =  long.Parse(GetUserId());
			return await _usersRepository.DeleteUserById(userId);
		}
	}
}
