using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.ServiceContracts;
using StackExchange.Redis;

namespace InternetBank.Core.Services
{
	public class JwtBlacklistService:IJwtBlacklistService
	{
		private readonly IConnectionMultiplexer _connectionMultiplexer;

		public JwtBlacklistService(IConnectionMultiplexer connectionMultiplexer)
		{
			_connectionMultiplexer = connectionMultiplexer;
		}


		/// <summary>
		/// after Delete User JWT TOken Goes TO BlackList in Redis
		/// </summary>
		/// <param name="token"></param>
		/// <param name="expiration"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task AddToBlacklistAsync(string token, TimeSpan expiration)
		{
			try
			{
				// Check Connection IsConnected or not
				if (_connectionMultiplexer.IsConnected)
				{
					var _db = _connectionMultiplexer.GetDatabase();
					bool success = await _db.StringSetAsync($"blacklist:{token}", "blacklisted", expiration);
					if (!success)
					{
						throw new Exception("Failed to add token to blacklist.");
					}
				}
				else
				{
					throw new Exception("Redis connection is not established.");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error occurred while adding token to blacklist.", ex);
			}
		}

		public async Task<bool> IsTokenBlacklistedAsync(string token)
		{
			try
			{
				// Check Connection IsConnected or not
				if (_connectionMultiplexer.IsConnected)
				{
					var _db = _connectionMultiplexer.GetDatabase();
					return await _db.KeyExistsAsync($"blacklist:{token}");
				}
				else
				{
					throw new Exception("Redis connection is not established.");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error occurred while checking if token is blacklisted.", ex);
			}
		}

		public async Task RemoveFromBlacklistAsync(string token)
		{
			try
			{
				if (_connectionMultiplexer.IsConnected)
				{
					var _db = _connectionMultiplexer.GetDatabase();
					bool deleted = await _db.KeyDeleteAsync($"blacklist:{token}");
					if (!deleted)
					{
						throw new Exception("Failed to remove token from blacklist.");
					}
				}
				else
				{
					throw new Exception("Redis connection is not established.");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error occurred while removing token from blacklist.", ex);
			}
		}
	}
}
