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
		public async Task<AuthenticationResponse> CreateJwtToken(ApplicationUser user)
		{
		  DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));
			//Payload
			//Create an array of Claim object representing the user's claims,such as their ID,name,email,etc.
		  Claim[] claims = new Claim[]
		  {
			  new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // subject (user ID)
			  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT unique ID
			  new Claim(JwtRegisteredClaimNames.Iat,
				  DateTime.UtcNow.ToString()), //Issued at (date and time Right now When token is generation
			  new Claim(ClaimTypes.NameIdentifier, user.Email.ToString()), //unique name identifier of the user (Email)
			  new Claim(ClaimTypes.Name, user.FirstName.ToString()), // FirstName of the user

		  };


			//Security Key + hash 
			//Create a SymmetricSecurityKey object using the key specified in the configuration.
			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
			 Encoding.UTF8.GetBytes( _configuration["Jwt:Key"]) //convert Key TO byte array
			  );

			//Create a SigningCredentials object with the security Key and the HMACSHA256 algorithm.
			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);// hash and hashdata



			//create a JwtSecurityToken object with the given issuer,claims,expiration,and signingCredentials. 
			JwtSecurityToken tokenGenerator = new JwtSecurityToken(
				_configuration["Jwt:Issuer"],
				//_configuration["Jwt:Audience"],
				claims:claims,
				expires:expiration,
				signingCredentials:signingCredentials
				
				);

			//Generate Token based on Those algorithm that we Write before this line 
			//Create a JwtSecurityTokenHandler object and use it to write the token as a string
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			string token = tokenHandler.WriteToken(tokenGenerator); //Generate token


			//Create and return an AuthenticationResponse object containing the token ,user email,username
			//and token and expiration time.
			return new AuthenticationResponse()
			{
				Token = token, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName,
				Expiration = expiration
			};

		}
	}
}
