using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace InternetBank.UI.Middleware
{
	
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context); 
			}
			//catch JsonException
			catch (JsonException ex) 
			{
				//ContentType TO json
				context.Response.ContentType = "application/json"; 
				//status code 400 
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest; 

				//create Object for Error message
				var errorResponse = new
				{
					message = "Invalid JSON format",
					details = ex.Message
				};
				//Send Error to client
				await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse)); 
			}
			//Catch Other Exception
			catch (Exception ex) 
			{
				context.Response.ContentType = "application/json"; 
				//set Status Code to 500
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				//create Object for Error message
				var errorResponse = new
				{
					message = "An unexpected error occurred",
					details = ex.Message
				};
				//Send Error to client
				await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse)); 
			}
		}
	}

}
