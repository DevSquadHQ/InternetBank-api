using Asp.Versioning;
using InternetBank.Core.DTO;
using InternetBank.Core.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;
using DNTPersianUtils.Core;
using InternetBank.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;

namespace InternetBank.UI.Controllers.v1
{
	/// <summary>
	/// UserController 
	/// </summary>
	[ApiVersion("1.0")]
	public class UserController : CustomControllerBase
	{
		private readonly SignInManager<ApplicationUser>  _signInManager;
		private readonly IUserService _userService;
		private readonly RoleManager<ApplicationRole> _roleManager;

		/// <summary>
		/// Inject Identity Services 
		/// </summary>
		/// <param name="userManager"></param>
		/// <param name="signInManager"></param>
		/// <param name="roleManager"></param>
		public UserController(SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IUserService userService)
		{
			_signInManager = signInManager;
			_roleManager = roleManager;
			_userService = userService;
		}

		/// <summary>
		/// For checking some Validation and Create User Or Return errorMessage
		/// </summary>
		/// <param name="registerUserDto"></param>
		/// <returns></returns>
		[Authorize(Policy = "RequireOtpToken")]
		[HttpPost("register")]
		public async Task<ActionResult<ApplicationUser>> CreateUser(RegisterUserDTO registerUserDto)
		{

			//Validation 
			if (ModelState.IsValid == false)
			{
				string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage));
				return Problem(errorMessage,statusCode:400);
			}

			if (registerUserDto.Birthdate == DateTime.MinValue)
			{
				return BadRequest(new { message = "تاریخ نامعتبر است. لطفاً تاریخ را به فرمت درست وارد کنید." });
			}

			if (!await _userService.OtpVerified())
			{
				return BadRequest("OTP not verified.");
			}


			var regesterResult = await _userService.AddUser(registerUserDto);

			if (regesterResult.result.Succeeded)
			{

				return Ok(regesterResult.user);
			}
			else
			{
				
				string errorMessage = 
					string.Join(" | ", regesterResult.result.Errors.Select(e => e.Description));//Error1 | Error2 ....
				if (regesterResult.ErrorMessage!=null)
				{
					return Problem(errorMessage+regesterResult.ErrorMessage , statusCode:409);
				}

				return Problem(errorMessage , statusCode:400);
			}



		}


		/// <summary>
		/// Login user with information
		/// </summary>
		/// <param name="loginUser"></param>
		/// <returns>FirstName , LastName , Email From user</returns>
		[HttpPost("login")]
		public async Task<IActionResult> LoginUser(LoginUserDTO loginUserDto)
		{
			//Validation 
			if (ModelState.IsValid == false)
			{
				string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage));
				return BadRequest(errorMessage);
			}

			await _signInManager.SignOutAsync();
			//lockoutONFailure Is false But If set it True user can try 3 or 4 time wrong password then acc will be lock
			var result = await _signInManager
				.PasswordSignInAsync(loginUserDto.Email, loginUserDto.Password,isPersistent:false,lockoutOnFailure:true);

			if (result.Succeeded)
			{
				AuthenticationResponse? user = await _userService.LoginUser(loginUserDto);

				if (user.user == null)
				{
					return NoContent();
				}

				//Sign-in
				await _signInManager.SignInAsync(user.user, isPersistent: false);

				return Ok(user.token);



			}
			else
			{
				return Problem("رمز عبور یا ایمیل اشتباه وارد شده",statusCode:401);
			}


		}

		/// <summary>
		/// Logout  User 
		/// </summary>
		/// <returns></returns>

		[HttpPost("logout")]
		public async Task<IActionResult> GetUserLogOut()
		{
			await _signInManager.SignOutAsync();
			return NoContent();
		}

		/// <summary>
		/// Delete USer With User'sAccount
		/// </summary>
		/// <returns></returns>
		[Authorize(Policy = "RequireLoginToken")]
		[HttpDelete]
		public async Task<IActionResult> DeleteUser()
		{
			var result = await _userService.DeleteUser();
			if (result)
			{
				return Ok();
			}

			return BadRequest();
		}


	}
}
