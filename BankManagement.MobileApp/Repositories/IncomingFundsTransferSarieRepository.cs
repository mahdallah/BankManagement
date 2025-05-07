using BankManagement.MobileApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement.MobileApp.Repositories
{
    public class IncomingFundsTransferSarieRepository : IRepository<IncomingFundsTransferSarie>
    {
        private readonly SQLiteAsyncConnection _database;

        public IncomingFundsTransferSarieRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<IncomingFundsTransferSarie>().Wait();
        }

        public Task<int> SaveMessageAsync(IncomingFundsTransferSarie message)
        {
            return _database.InsertAsync(message);
        }

        public Task<List<IncomingFundsTransferSarie>> GetTransactionsAsync()
        {
            return _database.Table<IncomingFundsTransferSarie>().ToListAsync();
        }

        public Task<List<IncomingFundsTransferSarie>> GetFilteredTransactionsAsync(string searchTerm, DateTime date)
        {
            return _database.Table<IncomingFundsTransferSarie>()
                .Where(t => t.TransactionDate.Date == date.Date &&
                            (string.IsNullOrEmpty(searchTerm) ||
                             t.From.Contains(searchTerm) || t.ToAccount.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
