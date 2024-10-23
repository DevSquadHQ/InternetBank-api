using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternetBank.UI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AccountController : CustomControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService= accountService;
        }
        [HttpPost("accountregister")]
        public async Task<IActionResult> Register([FromBody]RegisterAccountDTO registerAccountDTO){
            var accounts = await _accountService.CreateAccount(registerAccountDTO);
            return Ok(accounts);
        }
    }
}
