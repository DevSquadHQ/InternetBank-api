﻿using System;
using Microsoft.AspNetCore.Identity;


namespace InternetBank.Core.Identity
{
	public class ApplicationUser:IdentityUser<long>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		
		public string NationalCode { get; set; }

		public DateTime Birthdate { get; set; }

	}
}
