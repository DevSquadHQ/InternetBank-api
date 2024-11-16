using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using InternetBank.Core.DTO;
using InternetBank.Core.ServiceContracts;
using IPE.SmsIrClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InternetBank.Infrastructure.Sms
{
	public  class SendSmsService:ISmsService
	{
		private  readonly IConfiguration _configuration;

		public SendSmsService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public  async Task<SmSErrorManagerDTO> SmsSender(string message ,string phoneNumber)
		{
			try
			{
				SmsIr smsIr = new SmsIr(_configuration["SMS:IApi"]);
				long I = long.Parse(_configuration["SMS:Internal"]);
				string messageText = message;
				string[] mobile = { phoneNumber };
				var response = await smsIr.BulkSendAsync(I, messageText, mobile);

			}
			catch (Exception ex)
			{
				string errorName = ex.GetType().Name;

				switch (errorName)
				{
					case "UnauthorizedException":
						return new SmSErrorManagerDTO() { isSuccess = false , message = "توکن نامعتبر است یا دسترسی رد شده است." };
					case "LogicalException":
						return new SmSErrorManagerDTO() { isSuccess = false, message = "پارامترهای ورودی صحیح نیستند. لطفاً بررسی کنید." };
					case "TooManyRequestException":
						return new SmSErrorManagerDTO() { isSuccess = false, message = "تعداد درخواست‌ها بیش از حد مجاز است." };
					case "UnexpectedException":
						return new SmSErrorManagerDTO() { isSuccess = false, message = "یک خطای غیرمنتظره در سرور رخ داده است." };
					default:
						return new SmSErrorManagerDTO() { isSuccess = false, message = "خطای نامشخصی رخ داده است." };
				}

			}
			return new SmSErrorManagerDTO() { isSuccess = true };
		}
	}
}
