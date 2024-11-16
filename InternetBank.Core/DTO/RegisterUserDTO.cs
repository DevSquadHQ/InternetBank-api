using DNTPersianUtils.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Helpers;
using System.Text.Json.Serialization;

namespace InternetBank.Core.DTO
{
	public class RegisterUserDTO
	{
		[Required, ShouldContainOnlyPersianLetters(ErrorMessage = "لطفا زبان کیبورد را به فارسی تغییر دهید")]
		public string FirstName { get; set; }
		[Required, ShouldContainOnlyPersianLetters(ErrorMessage = "لطفا زبان کیبورد را به فارسی تغییر دهید")]
		public string LastName { get; set; }
		[Required, ValidIranianNationalCode(ErrorMessage = "کدملی معتبر نمی باشد")]
		public string NationalCode { get; set; }
		[Required ,MinAge(18, ErrorMessage = "سن باید بالای 18 سال باشد.")]
		[JsonConverter(typeof(CustomDateTimeConverter))]
		public DateTime Birthdate { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "لطفا شماره تلفن خود را وارد کنید"), ValidIranianMobileNumber(ErrorMessage = "شماره تلفن وارد شده معتبر نیست"),]
		[RegularExpression(@"^0\d{10}$", ErrorMessage = "Phone number must start with 0 and be exactly 11 digits")]
		public string PhoneNumber { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "پسورد با تاییدیه پسورد مطابقت ندارد")]
		public string ConfirmPassword { get; set; }

	}




}
