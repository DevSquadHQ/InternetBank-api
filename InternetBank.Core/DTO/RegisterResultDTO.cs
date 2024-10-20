using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Identity;
using Microsoft.AspNetCore.Identity;

namespace InternetBank.Core.DTO
{
	public class RegisterResultDTO
	{
		public IdentityResult result { get; set; }	
		public ApplicationUser user { get; set; }
	}
}
