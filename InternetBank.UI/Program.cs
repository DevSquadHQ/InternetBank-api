using System.Text;
using Asp.Versioning;
using InternetBank.Core.Domain.Entities;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Core.Identity;
using InternetBank.Core.ServiceContracts;
using InternetBank.Core.Services;
using InternetBank.Infrastructure.DbContext;
using InternetBank.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
	//Response
	options.Filters.Add(new ProducesAttribute("application/json"));
	//Request
	options.Filters.Add(new ConsumesAttribute("application/json"));


});

builder.Services.AddApiVersioning(config =>
{
	config.ApiVersionReader = new UrlSegmentApiVersionReader();
	//reads version number from request url at "apiVersion" constraint

	//Default Version If not mentioned
	config.DefaultApiVersion = new ApiVersion(1, 0);
	config.AssumeDefaultVersionWhenUnspecified = true;
	config.ReportApiVersions = true;
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddScoped<IUserService,UsersService > ();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IAccountsService,AccountsService>();
builder.Services.AddTransient<IAccountRepository,AccountRepository>();
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();




// Swagger
builder.Services.AddEndpointsApiExplorer(); //Generates description for all endpoints
builder.Services.AddSwaggerGen(options =>
{
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,"api.xml"));

	options.SwaggerDoc("v1",new Microsoft.OpenApi.Models.OpenApiInfo(){Title = "InternetBank API",Version = "1.0"});

}); // generate OpenAPI Specification



//Identity
//Todo make Strong Password in options
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders()
	.AddUserStore<UserStore<ApplicationUser,ApplicationRole,ApplicationDbContext,long>>()
	.AddRoleStore<RoleStore<ApplicationRole,ApplicationDbContext,long>>();



builder.Services.AddAuthorization(options =>{});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger(); //create endpoint for swagger.json
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json","1.0");
	}); //create swagger UI for testing all web ApI endpoints/ action methods
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
