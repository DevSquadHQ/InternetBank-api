using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using InternetBank.Core.DTO;
using InternetBank.Core.ServiceContracts;
using InternetBank.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternetBank.UI.Controllers.v1
{
	[ApiVersion("1.0")]
	public class OtpController:CustomControllerBase
	{
		private readonly IOtpService _otpService;
		private readonly IJwtService _jwtService;

		public OtpController(IOtpService service, IJwtService jwtService)
		{
			_otpService = service;
			_jwtService = jwtService;
		}
		/// <summary>
		/// Send-Otp-Code-TO-User
		/// </summary>
		/// <param name="sendOtpRequest"></param>
		/// <returns></returns>
		[HttpPost("send-otp")]
		public async Task<IActionResult> SendOtp(SendOtpRequestDTO sendOtpRequest)
		{
			var result = await _otpService.GenerateOtpAsync(sendOtpRequest.PhoneNumber);
			if (result.isSuccess == false)
			{
				return BadRequest(result.message);
			}
			return Ok(); 
		}

		/// <summary>
		/// Verify-Otp-Code
		/// </summary>
		/// <param name="validateOtpRequest"></param>
		/// <returns></returns>
		[HttpPost("verify-otp")]
		public async Task<IActionResult> VerifyOtp(ValidateOtpRequestDTO validateOtpRequest)
		{
			var isValid = await _otpService.ValidateOtpAsync(validateOtpRequest.PhoneNumber, validateOtpRequest.Code);
			if (isValid)
			{
				//Payload
				//Create an array of Claim object representing the user's claims,such as their ID,name,email,etc.
				var claims = new List<Claim>()
				{
					new Claim("OtpVerified", "true"),
					new Claim("type", "otp"),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT unique ID

				};
				DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(5));

				var tokenResponse = await _jwtService.CreateJwtToken(claims,expiration);
				return Ok(tokenResponse);
			}
			else
			{
				return BadRequest(new { Message = "Invalid or expired OTP. Please request a new one." });
			}
		}
	}
}
