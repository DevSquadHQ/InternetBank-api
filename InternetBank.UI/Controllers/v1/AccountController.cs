using Asp.Versioning;
using InternetBank.Core.DTO;
using InternetBank.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternetBank.UI.Controllers.v1
{
    /// <summary>
    /// Account Manager
    /// </summary>
    [ApiVersion("1.0")]
	[Authorize(Policy = "RequireLoginToken")]
	public class AccountController : CustomControllerBase
    {
        private readonly IAccountsService _accountsService;

        public AccountController(IAccountsService accountsService)
        {
	        _accountsService = accountsService;
        }
        /// <summary>
        /// Create BankAccount for existing User
        /// </summary>
        /// <param name="registerAccountDTO"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<RegisterAccountResponseDTO>> Register([FromBody]RegisterAccountDTO registerAccountDTO){
	        RegisterAccountResultDTO accounts = await _accountsService.CreateAccount(registerAccountDTO);
	        if (accounts.success == false)
	        {
		        return Problem(accounts.message, statusCode: 401);
	        }

	        return Ok(accounts.responseAccountDto);
        }

        /// <summary>
        /// Change BankAccount Password 
        /// </summary>
        /// <param name="accountPasswordDto"></param>
        /// <returns></returns>
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeAccountPasswordDto accountPasswordDto)
        {
	        var result = await _accountsService.ChangePassword(accountPasswordDto);
	        if (result.Success == false)
	        {
		        return Problem(result.Message, statusCode:result.statusCode);
	        }
            return Ok(result.Message);
        }

        /// <summary>
        /// Show the user's balance 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("balance/{accountId}")]
        public async Task<ActionResult<BalanceDTO>> GetBalance(long accountId)
        {
	        var balance = await _accountsService.GetBalance(accountId);
	        if (balance == null)
	        {
		        return NotFound("!حساب یافت نشد");
	        }
	        return Ok(balance);
        }


        /// <summary>
        /// Blocking the user's account by accountID
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpPut("block/{accountId}")]
        public async Task<IActionResult> BlockAccount(long accountId)
        {
	        try
	        {
		        var result = await _accountsService.BlockAccountAsync(accountId);
		        if (result.isSuccess == false)
		        {
			        return NotFound(result.Message);
		        }
		        return Ok(result.Message);
	        }
	        catch (Exception ex)
	        {
		        return BadRequest(ex.Message);
	        }
        }
        /// <summary>
        /// Unblocking the user's account by accountID 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpPut("unblock/{accountId}")]
        public async Task<IActionResult> UnblockAccount(long accountId)
        {
	        try
	        {
		        var result = await _accountsService.UnblockAccountAsync(accountId);
		        if (result.isSuccess == false)
		        {
			        return NotFound(result.Message);
		        }
				return Ok(result.Message);
	        }
	        catch (Exception ex)
	        {
		        return BadRequest(ex.Message);
	        }
        }


        /// <summary>
        /// Get All User BankAccounts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<AccountDetailsResponseDTO>> GetAllAccountDetails()
        {
	        if (!ModelState.IsValid) return BadRequest(ModelState);

	        var result = await _accountsService.GetAllAccountDetails();

	        if (result.isSuccess == false)
	        {
		        return NotFound(result.message);
	        }
            return Ok(result.accountDetails);
        }

        /// <summary>
        /// Get User BankAccount
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("{accountId}")]
        public async Task<ActionResult<AccountDetailDTO>> GetUserAccountDetail(long accountId)
        {
	        var result = await _accountsService.GetAccountDetail(accountId);
	        if (result.isSuccess == false)
	        {
		        return NotFound(result.message);
	        }

	        return Ok(result.accountDetail);
        }

        /// <summary>
        /// Delete AccountById
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpDelete("{accountId}")]
        public async Task<IActionResult> DeleteAccount(long accountId)
        {
	        var result = await _accountsService.DeleteAccountById(accountId);
	        if (result)
	        {
				return Ok();
			}

	        return BadRequest();
        }

        /// <summary>
        /// ForgotPassword For Account
        /// </summary>
        /// <param name="forgotPasswordAccountDto"></param>
        /// <returns></returns>
        [HttpPut("forgot-password")]
        public async Task<IActionResult> ForgotAccountPassword(ForgotPasswordAccountDTO forgotPasswordAccountDto)
        {
	        var result = await _accountsService.ForgotAccountPassword(forgotPasswordAccountDto.newPassword,
		        forgotPasswordAccountDto.accountId);
	        if (result)
	        {
		        return Ok();
	        }
            return BadRequest();
        }

	}
}
