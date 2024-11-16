using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DNTPersianUtils.Core;
using InternetBank.Core.Domain.Entities;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Core.DTO;
using InternetBank.Core.Helpers;
using InternetBank.Core.ServiceContracts;
using IPE.SmsIrClient;
using Microsoft.Extensions.Caching.Distributed;


namespace InternetBank.Core.Services
{
	public class TransactionsService:ITransactionsService
	{
		private readonly ISmsService _smsService;
		private readonly IUserService _userService;
		private readonly IUsersRepository _usersRepository;
		private readonly IOtpRepository _otpRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly IDistributedCache _distributedCache;

		public TransactionsService(ISmsService smsService, IOtpRepository otpRepository, IUserService userService, IUsersRepository usersRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository, IDistributedCache distributedCache)
		{
			_smsService = smsService;
			_otpRepository = otpRepository;
			_userService = userService;
			_usersRepository = usersRepository;
			_accountRepository = accountRepository;
			_transactionRepository = transactionRepository;
			_distributedCache = distributedCache;
		}

		/// <summary>
		/// SendSms for User with Otp Code
		/// </summary>
		/// <param name="cardToCardDto"></param>
		/// <returns></returns>
		public async Task<SmSErrorManagerDTO> SendSms(CardToCardDTO cardToCardDto)
		{
			if (cardToCardDto.Amount < 10000 || cardToCardDto.Amount > 50000000)
				return new SmSErrorManagerDTO() { isSuccess = false, message = "موجودی کافی نمی باشد" };

			var userId = long.Parse(_userService.GetUserId());
			var user = await _usersRepository.GetUserById(userId);
			var otp = new Random().Next(10000, 99999).ToString();
			var expirationTime = DateTime.UtcNow.AddMinutes(2);


			// Save Data In Redis
			var cacheKey = $"CardToCard:{userId}";
			var cardToCardJson = JsonSerializer.Serialize(cardToCardDto);
			await _distributedCache.SetStringAsync(
				cacheKey,
				cardToCardJson,
				new DistributedCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
				});



			var otpCode = new OtpCode
			{
				PhoneNumber = user.PhoneNumber,
				Code = otp,
				ExpirationTime = expirationTime,
				IsValid = true
			};

			await _otpRepository.AddOtpCodeAsync(otpCode);
			string messageText = $"کاربرگرامی این پیام تستی از طریق سایت می باشد و هیچ گونه اعتبار قانونی ندارد \nشبیه ساز تستی من\nمبلغ: {cardToCardDto.Amount}\nتاریخ: {DateTime.Now.ToShortPersianDateString():yyyy/MM/dd}\n" +
			                     $"ساعت: {DateTime.Now:HH:mm}\n" +
			                     $"شماره کارت: {cardToCardDto.SourceCardNumber.Substring(cardToCardDto.SourceCardNumber.Length - 6)}*******{cardToCardDto.SourceCardNumber.Substring(0, 4)}\n" +
			                     $"رمز یکبار مصرف(پویا): {otp}";
			

			 var result = await _smsService.SmsSender(messageText, user.PhoneNumber);

			 return result;

		}






		/// <summary>
		/// TransferMoney and ValidateOtp-Code
		/// </summary>
		/// <param name="transferMoneyRequestDto"></param>
		/// <returns></returns>
		public async Task<bool> TransferMoney(TransferMoneyRequestDTO transferMoneyRequestDto)
		{
			var userId = long.Parse(_userService.GetUserId());
			var user = await _usersRepository.GetUserById(userId);
			var otp = await _otpRepository.GetLatestOtpCodeAsync(user.PhoneNumber, transferMoneyRequestDto.OtpCode);
			string description="";
			bool isSuccess = true;



			// Take CardTOCard object from Redis
			var cacheKey = $"CardToCard:{userId}";
			var cachedCardToCardJson = await _distributedCache.GetStringAsync(cacheKey);
			if (string.IsNullOrEmpty(cachedCardToCardJson))
			{
				description = "اطلاعات تراکنش پیدا نشد.";
				return false;
			}
			
			var cardToCard = JsonSerializer.Deserialize<CardToCardDTO>(cachedCardToCardJson);
			if (cardToCard == null)
			{
				description="اطلاعات تراکنش نامعتبر است.";
				return false;
			}

			// Remove CardToCard object From Redis
			await _distributedCache.RemoveAsync(cacheKey);

			// Checking Otp Code
			if (otp == null)
			{
				description = "OTP not found.";
				isSuccess = false;
			}
			else if (otp.ExpirationTime <= DateTime.UtcNow || !otp.IsValid)
			{
				// If the OTP is expired or already invalid, remove it
				await _otpRepository.RemoveOtpCodeAsync(otp);
				description = "Invalid OTP or OTP expired.";
				isSuccess = false;
			}
			else
			{
				// Mark OTP as invalid
				otp.IsValid = false;
				await _otpRepository.UpdateOtpCodeAsync(otp);
			}



			// Take All User's BankAccount
			var userAccounts = await _accountRepository.GetAccountsByUserIdAsync(userId);
			var sourceAccount = userAccounts.FirstOrDefault(a => a.CardNumber == cardToCard.SourceCardNumber);
			if (sourceAccount.IsBlocked == 1)
			{
				description = "Account Is blocked .";
				isSuccess =  false;
			}
			// Take Destination Cart
			var destinationAccount = await _accountRepository.GetAccountByCardNumberAsync(cardToCard.DestinationCardNumber);

			// Checking DestinationCard Is for User Or not
			bool isDestinationOwnedByUser = userAccounts.Any(a => a.CardNumber == cardToCard.DestinationCardNumber);
			if (sourceAccount == null || destinationAccount == null)
			{
				description = "حساب‌های مبدا یا مقصد پیدا نشد.";
				isSuccess = false;
			}
			//check Amount
			if (sourceAccount.Amount < transferMoneyRequestDto.Amount)
			{
				description = "Insufficient balance.";
				isSuccess =  false; 
			}
			// Validate ExpireDate
			if (sourceAccount.ExpireDate.Date < DateTime.UtcNow.Date || cardToCard.ExpireDate.Date< DateTime.UtcNow.Date)
			{
				description="The card's Expire Date has already passed.";
				isSuccess =  false;
			}

			// If destinationAccount is For  person
			if (isDestinationOwnedByUser)
			{

				if (isSuccess)
				{
					sourceAccount.Amount -= transferMoneyRequestDto.Amount;
					destinationAccount.Amount += transferMoneyRequestDto.Amount;
					await _accountRepository.UpdateAsync(sourceAccount);
					await _accountRepository.UpdateAsync(destinationAccount);
					description = "The transaction was completed successfully";
				}
				
			}
			else
			{
				// If destinationAccount is For another person
				// TransactionMoney
				if (isSuccess)
				{
					sourceAccount.Amount -= transferMoneyRequestDto.Amount;
					destinationAccount.Amount += transferMoneyRequestDto.Amount;
					await _accountRepository.UpdateAsync(sourceAccount);
					await _accountRepository.UpdateAsync(destinationAccount);
					description = "The transaction was completed successfully";
				}

			}


			var parsedDate = DateTime.SpecifyKind(cardToCard.ExpireDate, DateTimeKind.Utc);
			// SaveTransaction Into DB
			var transaction = new Transaction
			{
				SourceCardNumber = cardToCard.SourceCardNumber,
				DestinationCardNumber = cardToCard.DestinationCardNumber,
				Amount = transferMoneyRequestDto.Amount,
				AccountId = sourceAccount.AccountId,
				Description = description,
				CVV2 = sourceAccount.Cvv2,
				isSuccess = isSuccess,
				ExpireDate = parsedDate,
				CreatedDateTime = DateTime.UtcNow
			};
			var result = await _transactionRepository.AddTransAction(transaction);
			if (result==false || isSuccess==false)
			{
				return false;
			}
			return true;

		}

		/// <summary>
		/// Report Transaction TO user If date not exist --> just last 5 Transaction 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="isSuccess"></param>
		/// <returns></returns>
		public async Task<List<TransactionReportResponseDTO>> TransactionReport(TransactionReportRequestDTO transactionReportRequestDto)
		{



			// Turn Into UTC DATETIME
			DateTime? from = transactionReportRequestDto.From?.Date;
			DateTime? to = transactionReportRequestDto.To?.Date;

			if (from.HasValue)
				from = DateTime.SpecifyKind(from.Value, DateTimeKind.Utc);

			if (to.HasValue)
				to = DateTime.SpecifyKind(to.Value, DateTimeKind.Utc);

			var transactions = await _transactionRepository.GetTransactions(from, to, transactionReportRequestDto.isSuccess);

			var result = transactions.Select(t => new TransactionReportResponseDTO()
			{
				Amount = t.Amount,
				IsSuccess = t.isSuccess,
				CreatedDateTime = t.CreatedDateTime,
				AccountId = t.AccountId,
				Description = t.Description,
				DestinationCardNumber = t.DestinationCardNumber
			}).ToList();

			return result;
		}

		/// <summary>
		/// GetAllInfo About Transaction And send IT to Client
		/// </summary>
		/// <returns></returns>
		public async Task<CardToCardInfoManager> GetCardToCardInfo()
		{
			var userId = long.Parse(_userService.GetUserId());
			// Take CardTOCard object from Redis
			var cacheKey = $"CardToCard:{userId}";
			var cachedCardToCardJson = await _distributedCache.GetStringAsync(cacheKey);
			if (string.IsNullOrEmpty(cachedCardToCardJson))
			{
				return new CardToCardInfoManager() { isSuccess = false , message = "اطلاعات تراکنش پیدا نشد." };
			}
			var cardToCard = JsonSerializer.Deserialize<CardToCardDTO>(cachedCardToCardJson);
			if (cardToCard == null)
			{
				return new CardToCardInfoManager() { isSuccess = false, message = "اطلاعات تراکنش نامعتبر است." };
			}

			var destinationAccount = await _accountRepository.GetAccountByCardNumberAsync(cardToCard.DestinationCardNumber);
			var destinationUser = await _usersRepository.GetUserById(destinationAccount.UserId);
			string? bankName = CardNumberHelper.GetBankName(destinationAccount.CardNumber);
			if ( destinationAccount == null || destinationUser == null || bankName==null)
			{

				return new CardToCardInfoManager() { isSuccess = false, message = "اطلاعات مربوط به کارت مقصد یافت نشد" };
			}

			var info = new CardToCardInfoResponseDTO
			{
				SourceCardNumber = cardToCard.SourceCardNumber,
				DestinationCardNumber = cardToCard.DestinationCardNumber,
				Amount = cardToCard.Amount,
				DestinationPersonName = destinationUser.FirstName + " " + destinationUser.LastName,
				DestinationBank = bankName

			};
			return new CardToCardInfoManager(){isSuccess = true , CardToCardInfoResponseDTO = info};
		}
	}
}
