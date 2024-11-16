using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBank.Core.Domain.Entities;

namespace InternetBank.Core.Domain.RepositoryContracts
{
	public interface ITransactionRepository
	{
		Task<bool> AddTransAction(Transaction  transaction);

		Task<List<Transaction>>
			GetTransactions(DateTime? from = null, DateTime? to = null, bool? isSuccess = null);

	}
}
