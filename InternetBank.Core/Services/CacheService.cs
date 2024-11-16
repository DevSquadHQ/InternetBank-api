using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Core.ServiceContracts;

namespace InternetBank.Core.Services
{
	public class CacheService:ICacheService
	{
		private readonly IRedisCacheRepository _redisCacheRepository;

		public CacheService(IRedisCacheRepository redisCacheRepository)
		{
			_redisCacheRepository = redisCacheRepository;
		}

		/// <summary>
		/// Set Into Redis
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="expirationTime"></param>
		/// <returns></returns>
		public async Task SetCacheAsync(string key, string value, TimeSpan? expirationTime = null)
		{
			await _redisCacheRepository.SetStringAsync(key, value, expirationTime);
		}

		/// <summary>
		/// Get From Redis
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<string> GetCacheAsync(string key)
		{
			return await _redisCacheRepository.GetStringAsync(key);
		}

		/// <summary>
		/// Remove From Redis
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task RemoveCacheAsync(string key)
		{
			await _redisCacheRepository.RemoveAsync(key);
		}
	}
}
