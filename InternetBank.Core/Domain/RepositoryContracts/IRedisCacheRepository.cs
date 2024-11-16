using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.Domain.RepositoryContracts
{
	public interface IRedisCacheRepository
	{
		Task SetStringAsync(string key, string value, TimeSpan? expirationTime = null);
		Task<string> GetStringAsync(string key);
		Task RemoveAsync(string key);
	}
}
