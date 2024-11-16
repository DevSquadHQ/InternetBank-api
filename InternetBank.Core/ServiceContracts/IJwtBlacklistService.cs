using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.ServiceContracts
{
	public interface IJwtBlacklistService
	{
		Task AddToBlacklistAsync(string token, TimeSpan expiration);
		Task<bool> IsTokenBlacklistedAsync(string token);
		Task RemoveFromBlacklistAsync(string token);
	}
}
