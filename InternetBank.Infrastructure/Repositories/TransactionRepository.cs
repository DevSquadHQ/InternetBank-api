using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Domain.Entities;
using InternetBank.Core.Domain.RepositoryContracts;
using InternetBank.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace InternetBank.Infrastructure.Repositories
{
	public class TransactionRepository:ITransactionRepository
	{
		private readonly ApplicationDbContext _context;

		public TransactionRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Add Transaction TO Db
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public async Task<bool> AddTransAction(Transaction transaction)
		{
			await _context.Transactions.AddAsync(transaction);
			var result = await _context.SaveChangesAsync();
			if (result < 1)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Get ALl Transactions True or False --Filters can apply
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="isSuccess"></param>
		/// <returns></returns>
		public async Task<List<Transaction>> GetTransactions(DateTime? from = null, DateTime? to = null, bool? isSuccess = null)
		{

			var query = _context.Transactions.AsQueryable();

			// Filter just For date
			if (from.HasValue)
			{
				var fromDate = from.Value.Date;
				query = query.Where(t => t.CreatedDateTime.Date >= fromDate);
			}

			if (to.HasValue)
			{
				var toDate = to.Value.Date;
				query = query.Where(t => t.CreatedDateTime.Date <= toDate);
			}

			// Filter if isSuccess or not
			if (isSuccess.HasValue)
			{
				query = query.Where(t => t.isSuccess == isSuccess.Value);
			}

			// Take 5 last Transaction When We don't have Date or other values
			if (!from.HasValue && !to.HasValue)
			{
				query = query.OrderByDescending(t => t.CreatedDateTime)
					.Take(5);
			}
			else
			{
				query = query.OrderByDescending(t => t.CreatedDateTime);
			}

			return await query.ToListAsync();
		}
	}
}
