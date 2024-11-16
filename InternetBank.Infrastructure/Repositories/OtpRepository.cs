using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Domain.Entities;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace InternetBank.Infrastructure.Repositories
{
	public class OtpRepository:IOtpRepository
	{
		private readonly ApplicationDbContext _context;

		public OtpRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task AddOtpCodeAsync(OtpCode otpCode)
		{
			_context.OtpCodes.Add(otpCode);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Get Otp Code From DB
		/// </summary>
		/// <param name="phoneNumber"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public async Task<OtpCode> GetLatestOtpCodeAsync(string phoneNumber, string code)
		{
			return await _context.OtpCodes
				.Where(o => o.PhoneNumber == phoneNumber && o.Code == code)
				.OrderByDescending(o => o.ExpirationTime)
				.FirstOrDefaultAsync();
		}

		/// <summary>
		/// Remove Otp From Db When COde failed
		/// </summary>
		/// <param name="otpCode"></param>
		/// <returns></returns>
		public async Task RemoveOtpCodeAsync(OtpCode otpCode)
		{
			_context.OtpCodes.Remove(otpCode);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Update Otp Code status
		/// </summary>
		/// <param name="otpCode"></param>
		/// <returns></returns>
		public async Task UpdateOtpCodeAsync(OtpCode otpCode)
		{
			_context.OtpCodes.Update(otpCode);
			await _context.SaveChangesAsync();
		}
	}
}
