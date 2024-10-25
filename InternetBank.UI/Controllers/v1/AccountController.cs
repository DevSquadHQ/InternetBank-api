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
	        RegisterAccountResponseDTO accounts = await _accountsService.CreateAccount(registerAccountDTO);
            return Ok(accounts);
        }

        /// <summary>
        /// Change BankAccount Password 
        /// </summary>
        /// <param name="accountPasswordDto"></param>
        /// <returns></returns>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeAccountPasswordDto accountPasswordDto)
        {
	        var result = await _accountsService.ChangePassword(accountPasswordDto);
	        if (result.Success == false)
	        {
		        return Problem(result.Message, statusCode:result.statusCode);
	        }
            return Ok(result.Message);
        }
    }
}
