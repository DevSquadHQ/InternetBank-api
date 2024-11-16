using InternetBank.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class TransactionReportResponseDTO
	{
		public long Amount { get; set; }
		public bool IsSuccess { get; set; }

		[JsonConverter(typeof(CustomDateTimeConverter))]
		public DateTime CreatedDateTime { get; set; }
		public long AccountId { get; set; }
		public string? Description { get; set; }
		public string DestinationCardNumber { get; set; }
	}
}
