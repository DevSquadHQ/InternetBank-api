using DNTPersianUtils.Core;
using InternetBank.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class CardToCardDTO
	{
		[Required ,ValidIranShetabNumber(ErrorMessage = "کارت مبدا معتبر نمی باشد")]
		public string SourceCardNumber { get; set; }
		[Required, ValidIranShetabNumber(ErrorMessage = "کارت مقصد معتبر نمی باشد")]
		public string DestinationCardNumber { get; set; }

		[Required(ErrorMessage = "CVV2 is required.")]
		[StringLength(4, MinimumLength = 4, ErrorMessage = "CVV2 must be exactly 4 characters long.")]
		[RegularExpression("^[0-9]{4}$", ErrorMessage = "CVV2 must be a 4-digit number.")]
		public string CVV2 { get; set; }
		[Required]

		[JsonConverter(typeof(CustomDateTimeConverter))]
		public DateTime ExpireDate { get; set; }
		[Required]
		[Range(10000, 50000000, ErrorMessage = "مبلغ باید بین ۱۰۰۰ تا ۵۰۰۰۰۰۰ تومان باشد.")]
		public long Amount { get; set; }
	}
}
