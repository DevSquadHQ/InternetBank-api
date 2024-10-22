using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternetBank.UI.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : CustomControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterAccountDTO registerAccountDTO){
            var accounts = await _accountRepository.CreateAccount(registerAccountDTO);
            return Ok(AccountId);
        }
    }
}
