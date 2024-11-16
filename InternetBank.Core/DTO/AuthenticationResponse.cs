using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Identity;

namespace InternetBank.Core.DTO
{
	public class AuthenticationResponse
	{
     public ApplicationUser user { get; set; }

	 public string token { get; set; }

	}
}
