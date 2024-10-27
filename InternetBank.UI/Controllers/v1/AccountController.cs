using Asp.Versioning;
using InternetBank.Core.DTO;
using InternetBank.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternetBank.UI.Controllers.v1
{
    /// <summary>
    /// Account Manager
    /// </summary>
    [ApiVersion("1.0")]
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
        /// Show the user's balance (Fifth)
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("balance/{accountId}")]
        public async Task<IActionResult> GetBalance(long accountId)
        {
	        var balance = await _accountsService.GetBalance(accountId);
	        if (balance == null)
	        {
		        return NotFound("!حساب یافت نشد");
	        }
	        return Ok(balance);
        }


        /// <summary>
        /// Blocking the user's account by accountID (seventh)
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpPut("block/{accountId}")]
        public async Task<IActionResult> BlockAccount(long accountId)
        {
	        try
	        {
		        await _accountsService.BlockAccountAsync(accountId);
		        return Ok(".حساب مسدود شد");
	        }
	        catch (Exception ex)
	        {
		        return BadRequest(ex.Message);
	        }
        }
        /// <summary>
        /// Unblocking the user's account by accountID (eighth)
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpPut("unblock/{accountId}")]
        public async Task<IActionResult> UnblockAccount(long accountId)
        {
	        try
	        {
		        await _accountsService.UnblockAccountAsync(accountId);
		        return Ok(".حساب رفع مسدودیت شد");
	        }
	        catch (Exception ex)
	        {
		        return BadRequest(ex.Message);
	        }
        }
	}
}
