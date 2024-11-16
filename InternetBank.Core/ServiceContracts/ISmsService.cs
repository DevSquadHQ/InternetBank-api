using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.DTO;

namespace InternetBank.Core.ServiceContracts
{
	public interface ISmsService
	{
		Task<SmSErrorManagerDTO> SmsSender(string message, string phoneNumber);
	}
}
