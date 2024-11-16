using DNTPersianUtils.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class ValidateOtpRequestDTO
	{
		[Required (ErrorMessage = "کد وارد نشده")]
		public string Code { get; set; }

		[Required(ErrorMessage = "لطفا شماره تلفن خود را وارد کنید"), ValidIranianMobileNumber(ErrorMessage = "شماره تلفن وارد شده معتبر نیست"),]
		[RegularExpression(@"^0\d{10}$", ErrorMessage = "Phone number must start with 0 and be exactly 11 digits")]
		public string PhoneNumber { get; set; }
	}
}
