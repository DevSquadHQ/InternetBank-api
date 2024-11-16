using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Asp.Versioning;
using InternetBank.Core.Domain.Entities;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Core.Identity;
using InternetBank.Core.ServiceContracts;
using InternetBank.Core.Services;
using InternetBank.Core.TokenHandler;
using InternetBank.Infrastructure.DbContext;
using InternetBank.Infrastructure.Repositories;
using InternetBank.Infrastructure.Sms;
using InternetBank.UI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
	//Response
	//options.Filters.Add(new ProducesAttribute("application/json"));
	//Request
	//options.Filters.Add(new ConsumesAttribute("application/json"));


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


//All Services
builder.Services.AddScoped<IUserService,UsersService > ();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IAccountsService,AccountsService>();
builder.Services.AddTransient<IAccountRepository,AccountRepository>();
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<ITransactionsService, TransactionsService>();
builder.Services.AddTransient<IOtpRepository, OtpRepository>();
builder.Services.AddTransient<IOtpService, OtpService>();
builder.Services.AddSingleton<IAuthorizationHandler, TokenTypeAuthorizationHandler>();
builder.Services.AddTransient<ISmsService, SendSmsService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddSingleton<IJwtBlacklistService, JwtBlacklistService>();
// Added Redis Services Redis Cache
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IRedisCacheRepository, RedisCacheRepository>();

// Redis Configuration
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
	var configuration = builder.Configuration.GetConnectionString("Redis");
	return ConnectionMultiplexer.Connect(configuration);
});

// Configure Distributed Cache (Session)
builder.Services.AddStackExchangeRedisCache(options =>
{
	var configuration = builder.Configuration.GetConnectionString("Redis");
	options.Configuration = configuration;
	options.InstanceName = "Session_"; 
});




// Swagger
builder.Services.AddEndpointsApiExplorer(); //Generates description for all endpoints
builder.Services.AddSwaggerGen(options =>
{
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,"api.xml"));

	options.SwaggerDoc("v1",new Microsoft.OpenApi.Models.OpenApiInfo(){Title = "InternetBank API",Version = "1.0"});
	options.AddSecurityDefinition(name:JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
	{
		Description = "Enter the Bearer Authorization : `Bearer Generated-JWT-Token`",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,

	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
		new OpenApiSecurityScheme
		{
			Reference = new OpenApiReference
			{
				Type = ReferenceType.SecurityScheme,
				Id = JwtBearerDefaults.AuthenticationScheme
			}
		},new string[]{}
		}
	});


}); // generate OpenAPI Specification



//Identity
//Todo make Strong Password in options
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders()
	.AddUserStore<UserStore<ApplicationUser,ApplicationRole,ApplicationDbContext,long>>()
	.AddRoleStore<RoleStore<ApplicationRole,ApplicationDbContext,long>>();

//JWT
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateAudience = true,
		ValidateIssuer = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		ValidateLifetime = true, //check token Expiration is expired
		ClockSkew = TimeSpan.Zero,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
		ValidateIssuerSigningKey = true, //check and validate Signature//Take Key that was store in appsettings.json and convert it to byte array

	};
	options.SaveToken = true; // HttpContext
	options.RequireHttpsMetadata = false;
});


//COrsPolicy
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins", builder =>
	{
		builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

//JWT Token Policy
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("RequireOtpToken" , policy =>
	{
		policy.Requirements.Add(new TokenTypeRequirement("otp"));
	});

	options.AddPolicy("RequireLoginToken", policy =>
	{
		policy.Requirements.Add(new TokenTypeRequirement("login"));
	});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger(); //create endpoint for swagger.json
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json","1.0");
	}); //create swagger UI for testing all web ApI endpoints/ action methods

	app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
//CorsPolicy
app.UseCors("AllowAllOrigins");



app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
