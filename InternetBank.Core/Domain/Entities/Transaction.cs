using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.Domain.Entities
{
	public class Transaction
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long TransactionId { get; set; }
		public string SourceCardNumber { get; set; }
		public string DestinationCardNumber { get; set; }
		public string CVV2 { get; set; }
		public DateTime ExpireDate { get; set; }
		public long Amount { get; set; }
		public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
		public long AccountId { get; set; }
		public Account Account { get; set; }

		public string?  Description { get; set; } = string.Empty;

		public bool isSuccess { get; set; }

	}
}
