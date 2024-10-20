using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternetBank.UI.Controllers
{
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	public class CustomControllerBase : ControllerBase
	{
	}
}
