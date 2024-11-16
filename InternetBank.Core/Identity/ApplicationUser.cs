using System;
using InternetBank.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace InternetBank.Core.Identity
{
	public class ApplicationUser:IdentityUser<long>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		
		public string NationalCode { get; set; }

		public DateTime Birthdate { get; set; }

		public ICollection<Account> Accounts { get; set; }

		public bool IsActive { get; set; } = false;

	}
}
