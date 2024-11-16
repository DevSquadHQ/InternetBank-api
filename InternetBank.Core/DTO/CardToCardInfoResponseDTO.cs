using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.DTO
{
	public class CardToCardInfoResponseDTO
	{
		public string SourceCardNumber { get; set; }
		public string DestinationCardNumber { get; set; }
		public string DestinationBank { get; set; }

		public string DestinationPersonName { get; set; }

		public long Amount { get; set; }

}
}
