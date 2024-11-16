using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InternetBank.Core.DTO;
using InternetBank.Core.Identity;
using InternetBank.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace InternetBank.Core.Services
{
	public class JwtService :IJwtService
	{
		private readonly IConfiguration _configuration;

		public JwtService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		/// <summary>
		/// Generate a JWT Token using the given user's information and the configuration settings.
		/// </summary>
		/// <param name="user">ApplicationUser object</param>
		/// <returns>AuthenticationResponse that include token</returns>
		public async Task<string> CreateJwtToken(List<Claim> claimList , DateTime Expiration)
		{

			//Security Key + hash 
			//Create a SymmetricSecurityKey object using the key specified in the configuration.
			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
			 Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]) //convert Key TO byte array
			  );

			//Create a SigningCredentials object with the security Key and the HMACSHA256 algorithm.
			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);// hash and hashdata



			//create a JwtSecurityToken object with the given issuer,claims,expiration,and signingCredentials. 
			JwtSecurityToken tokenGenerator =  new JwtSecurityToken(
				issuer:_configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claimList,
				expires: Expiration,
				signingCredentials:signingCredentials
				
				);


			//Generate Token based on Those algorithm that we Write before this line 
			//Create a JwtSecurityTokenHandler object and use it to write the token as a string
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.WriteToken(tokenGenerator); //Generate token


			//Create and return an AuthenticationResponse object containing the token ,user email,username
			//and token and expiration time.
			//return new AuthenticationResponse()
			//{
			//	Token = token, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName,
			//	Expiration = expiration
			//};

			return token;

		}
	}
}
