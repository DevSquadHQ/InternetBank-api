using InternetBank.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.Domain.RepositoryContracts
{
	public interface IOtpRepository
	{
		Task AddOtpCodeAsync(OtpCode otpCode);
		Task<OtpCode> GetLatestOtpCodeAsync(string phoneNumber, string code);
		Task RemoveOtpCodeAsync(OtpCode otpCode);
		Task UpdateOtpCodeAsync(OtpCode otpCode);
	}
}
