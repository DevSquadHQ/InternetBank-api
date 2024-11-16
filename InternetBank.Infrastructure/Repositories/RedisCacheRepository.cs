using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Caching.Distributed;

namespace InternetBank.Infrastructure.Repositories
{
	public class RedisCacheRepository:IRedisCacheRepository
	{
		private readonly IDistributedCache _distributedCache;

		public RedisCacheRepository(IDistributedCache distributedCache)
		{
			_distributedCache = distributedCache;
		}

		/// <summary>
		/// Set Into cash
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="expirationTime"></param>
		/// <returns></returns>
		public async Task SetStringAsync(string key, string value, TimeSpan? expirationTime = null)
		{
			var options = new DistributedCacheEntryOptions();

			if (expirationTime.HasValue)
			{
				options.SetAbsoluteExpiration(expirationTime.Value);
			}

			await _distributedCache.SetStringAsync(key, value, options);
		}

		/// <summary>
		/// Get Item from cash
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<string> GetStringAsync(string key)
		{
			return await _distributedCache.GetStringAsync(key);
		}

		/// <summary>
		/// Remove From cash
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task RemoveAsync(string key)
		{
			await _distributedCache.RemoveAsync(key);
		}
	}
}
