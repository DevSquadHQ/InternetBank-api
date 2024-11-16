using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.Helpers
{
	public static class CardNumberHelper
	{
		// Dictionary WIth Bank info
		private static readonly Dictionary<string, string> BankBinDictionary = new()
		{
			{ "melli", "603799" },
			{ "mellat", "610433" },
			{ "saderat", "603769" },
			{ "sepah", "589210" },
			{ "tejarat", "585983" },
			{ "ansar", "627381" },
			{ "ayandeh", "636214" },
			{ "dey", "502938" },
			{ "etebari_tosee", "628157" },
			{ "gardeshgari", "505416" },
			{ "ghavvamin", "639599" },
			{ "kar_afarin", "627488" },
			{ "keshavarzi", "603770" },
			{ "maskan", "628023" },
			{ "mehr_e_eghtesad", "639370" },
			{ "mehr_e_iranian", "606373" },
			{ "pasargad", "502229" },
			{ "post_bank", "627760" },
			{ "refah", "589463" },
			{ "saanat_va_maadan", "627961" },
			{ "saman", "621986" },
			{ "sarmayeh", "639607" },
			{ "shahr", "504706" },
			{ "sina", "639346" },
			{ "tosee_saderat", "627648" },
			{ "tosee_taavon", "502908" }
		};

		// return bank Name
		public static string? GetBankName(string cardNumber)
		{
			// Checking Card
			if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 6)
			{
				return null;
			}

			// Substring first 6 number from Card
			var bin = cardNumber.Substring(0, 6);

			// Search Bank and CardNumber in Dictionary
			var bank = BankBinDictionary.FirstOrDefault(x => x.Value == bin);
			if (bank.Key == null)
			{
				return null; // not found card
			}
			// BankName
			return bank.Key;
		}
	}
}
