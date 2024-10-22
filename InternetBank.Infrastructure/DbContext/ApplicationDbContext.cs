﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Identity;
using InternetBank.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InternetBank.Infrastructure.DbContext
{
	public class ApplicationDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,long>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
		public ApplicationDbContext(){}
		public DbSet<Account> Accounts { get; set; }
	}
}