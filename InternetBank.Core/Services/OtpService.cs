using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Domain.Entities;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Core.DTO;
using InternetBank.Core.ServiceContracts;
using IPE.SmsIrClient;
using IPE.SmsIrClient.Models.Requests;
using IPE.SmsIrClient.Models.Results;

namespace InternetBank.Core.Services
{
	public class OtpService:IOtpService
	{
		private readonly IOtpRepository _otpRepository;
		private readonly ISmsService _smsService;

		public OtpService(IOtpRepository otpRepository, ISmsService smsService)
		{
			_otpRepository = otpRepository;
			_smsService = smsService;
		}

		/// <summary>
		/// Generate-OTP-COde-Send-TO-USER
		/// </summary>
		/// <param name="phoneNumber"></param>
		/// <returns></returns>
		public async Task<SmSErrorManagerDTO> GenerateOtpAsync(string phoneNumber)
		{
			var code = new Random().Next(100000, 999999).ToString();
			var expirationTime = DateTime.UtcNow.AddMinutes(2);

			var otpCode = new OtpCode
			{
				PhoneNumber = phoneNumber,
				Code = code,
				ExpirationTime = expirationTime,
				IsValid = true
			};

			await _otpRepository.AddOtpCodeAsync(otpCode);
			string messageText = $"کاربرگرامی این پیام تستی از طریق سایت می باشد و هیچ گونه اعتبار قانونی ندارد\n کدتایید شما: {code}";
			var response = await _smsService.SmsSender(messageText , phoneNumber);

			return response;
		}


		/// <summary>
		/// Validate-OTP-COde
		/// </summary>
		/// <param name="phoneNumber"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public async Task<bool> ValidateOtpAsync(string phoneNumber, string code)
		{
			var otp = await _otpRepository.GetLatestOtpCodeAsync(phoneNumber, code);

			if (otp != null && otp.ExpirationTime > DateTime.UtcNow && otp.IsValid)
			{
				otp.IsValid = false;
				await _otpRepository.UpdateOtpCodeAsync(otp);
				return true;
			}

			if (otp != null)
			{
				await _otpRepository.RemoveOtpCodeAsync(otp);
			}

			return false;
		}
	}
}
