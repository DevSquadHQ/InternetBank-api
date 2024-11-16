using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.Helpers
{
	public static class CardHelper
	{
		private static readonly Dictionary<string, string> BankBins = new Dictionary<string, string>
		{
			{ "melli", "603799" },
			{ "mellat", "610433" },
			{ "saderat", "603769" },
			{ "sepah", "589210" },
			{ "tejarat", "585983" },
			{"ansar","627381"},
			{"ayandeh","636214"},
			{"dey","502938"},
			{"etebari_tosee","628157"},
			{"gardeshgari","505416"},
			{"ghavvamin","639599"},
			{"kar_afarin","627488"},
			{"keshavarzi","603770"},
			{"maskan","628023"},
			{"mehr_e_eghtesad","639370"},
			{"mehr_e_iranian","606373"},
			{"pasargad","502229"},
			{"post_bank","627760"},
			{"refah","589463"},
			{"saanat_va_maadan","627961"},
			{"saman","621986"},
			{"sarmayeh","639607"},
			{"shahr","504706"},
			{"sina","639346"},
			{"tosee_saderat","627648"},
			{"tosee_taavon","502908"}

		};

		public static bool IsValidCardNumber(string cardNumber)
		{
			int sum = 0;
			bool alternate = false;

			for (int i = cardNumber.Length - 1; i >= 0; i--)
			{
				int n = int.Parse(cardNumber[i].ToString());

				if (alternate)
				{
					n *= 2;
					if (n > 9) n -= 9;
				}

				sum += n;
				alternate = !alternate;
			}

			return (sum % 10 == 0);
		}

		public static string? GenerateCardNumber(string bankName, int length = 16)
		{
			if (!BankBins.TryGetValue(bankName, out var bin))
			{
				return null;
			}

			var random = new Random();
			var cardNumber = new StringBuilder(bin);

			while (cardNumber.Length < length - 1)
			{
				cardNumber.Append(random.Next(0, 10).ToString());
			}

			for (int i = 0; i < 10; i++)
			{
				var fullCardNumber = cardNumber.ToString() + i.ToString();
				if (IsValidCardNumber(fullCardNumber))
				{
					cardNumber.Append(i);
					break;
				}
			}

			return cardNumber.ToString();
		}
	}
}
