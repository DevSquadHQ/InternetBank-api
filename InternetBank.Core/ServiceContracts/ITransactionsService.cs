using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.DTO;

namespace InternetBank.Core.ServiceContracts
{
	public interface ITransactionsService
	{
		//Task<bool> SendSms(CardToCardDTO cardToCardDto);
		Task<SmSErrorManagerDTO> SendSms(CardToCardDTO cardToCardDto);
		Task<bool> TransferMoney(TransferMoneyRequestDTO transferMoneyRequestDto);
		Task<List<TransactionReportResponseDTO>>TransactionReport(TransactionReportRequestDTO transactionReportRequestDto);
		Task<CardToCardInfoManager> GetCardToCardInfo ();
	}
}
