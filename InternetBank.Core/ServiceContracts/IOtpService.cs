using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.DTO;

namespace InternetBank.Core.ServiceContracts
{
	public interface IOtpService
	{
		Task<SmSErrorManagerDTO> GenerateOtpAsync(string phoneNumber);
		Task<bool> ValidateOtpAsync(string phoneNumber, string code);
	}
}
