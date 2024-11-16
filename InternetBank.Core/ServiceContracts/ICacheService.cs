using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.ServiceContracts
{
	public interface ICacheService
	{
		Task SetCacheAsync(string key, string value, TimeSpan? expirationTime = null);
		Task<string> GetCacheAsync(string key);
		Task RemoveCacheAsync(string key);
	}
}
