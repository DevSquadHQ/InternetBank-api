using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Identity;

namespace InternetBank.Core.Domain.Entities
{
	public class OtpCode
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long OtpId { get; set; }
		public string PhoneNumber { get; set; }
		public string Code { get; set; }
		public DateTime ExpirationTime { get; set; }
		public bool IsValid { get; set; } = true;

	}
}
