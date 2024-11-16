using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class TransactionReportRequestDTO
	{
		public  DateTime? From { get; set; }
		public DateTime? To { get; set; }
		public bool? isSuccess { get; set; }
	}
}
