using Asp.Versioning;
using InternetBank.Core.DTO;
using InternetBank.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace InternetBank.UI.Controllers.v1
{
	[ApiVersion("1.0")]
	[Authorize(Policy = "RequireLoginToken")]
	public class TransactionController : CustomControllerBase
	{
		private readonly ITransactionsService _transactionsService;

		public TransactionController(ITransactionsService transactionsService)
		{
			_transactionsService = transactionsService;
		}

		/// <summary>
		/// Card To Card
		/// </summary>
		/// <param name="cardToCardDTO"></param>
		/// <returns></returns>
		[HttpPost("send-otp")]
		public async Task<IActionResult> CardToCard(CardToCardDTO cardToCardDTO)
		{
			//Validation 
			if (ModelState.IsValid == false)
			{
				string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage));
				return Problem(errorMessage, statusCode: 400);
			}

			if (cardToCardDTO.ExpireDate == DateTime.MinValue)
			{
				return BadRequest(new { message = "تاریخ نامعتبر است. لطفاً تاریخ را به فرمت درست وارد کنید." });
			}
			var result = await _transactionsService.SendSms(cardToCardDTO);
			if (result.isSuccess == false)
			{
				return BadRequest(result.message);
			}

			return Ok();
		}

		/// <summary>
		/// Transfer-Money With valid OtpCode
		/// </summary>
		/// <param name="transferMoneyRequestDto"></param>
		/// <returns></returns>
		[HttpPost("transfer-money")]
		public async Task<IActionResult> TransferMoney(TransferMoneyRequestDTO transferMoneyRequestDto)
		{
			//Validation 
			if (ModelState.IsValid == false)
			{
				string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage));
				return Problem(errorMessage, statusCode: 400);
			}

			var isValid = await _transactionsService.TransferMoney(transferMoneyRequestDto);
			if (isValid)
			{
				return Ok();
			}

			return BadRequest(new { Message = "The transaction was not completed successfully" });
		}





		/// <summary>
		/// Get All Info From CardToCard Object
		/// </summary>
		/// <param name="cardToCardDto"></param>
		/// <returns></returns>
		[HttpGet("info")]
		public async Task<ActionResult<CardToCardInfoResponseDTO>>GetCardToCardInfo()
		{
			var result = await _transactionsService.GetCardToCardInfo();
			if (result.isSuccess == false)
			{
				return BadRequest(result.message);
			}
			return Ok(result.CardToCardInfoResponseDTO);
		}


		/// <summary>
		/// Send TransactionReport TO User
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="isSuccess"></param>
		/// <returns></returns>
		[HttpGet("report")]
		public async Task<ActionResult<List<TransactionReportResponseDTO>>> GetTransactionReport([FromQuery] TransactionReportRequestDTO transactionReportRequestDto)
		{
			//Validation 
			if (ModelState.IsValid == false)
			{
				string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage));
				return Problem(errorMessage, statusCode: 400);
			}
			if (transactionReportRequestDto.From == DateTime.MinValue)
			{
				return BadRequest(new { message = "تاریخ نامعتبر است. لطفاً تاریخ را به فرمت درست وارد کنید." });
			}
			if (transactionReportRequestDto.To == DateTime.MinValue)
			{
				return BadRequest(new { message = "تاریخ نامعتبر است. لطفاً تاریخ را به فرمت درست وارد کنید." });
			}

			// تبدیل تاریخ به UTC در صورت نیاز
			transactionReportRequestDto.From = transactionReportRequestDto.From?.Date;
			transactionReportRequestDto.To = transactionReportRequestDto.To?.Date;

			var result = await _transactionsService.TransactionReport(transactionReportRequestDto);

			if (result.IsNullOrEmpty())
			{
				return NotFound();
			}
			return Ok(result);
		}
	}
}
