using BankManagement.MobileApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement.MobileApp.Repositories
{
    public class TransferBetweenAccountsRepository : IRepository<TransferBetweenAccounts>
    {
        private readonly SQLiteAsyncConnection _database;

        public TransferBetweenAccountsRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<TransferBetweenAccounts>().Wait();
        }

        public Task<int> SaveMessageAsync(TransferBetweenAccounts message)
        {
            return _database.InsertAsync(message);
        }

        public Task<List<TransferBetweenAccounts>> GetTransactionsAsync()
        {
            return _database.Table<TransferBetweenAccounts>().ToListAsync();
        }

        public Task<List<TransferBetweenAccounts>> GetFilteredTransactionsAsync(string searchTerm, DateTime date)
        {
            return _database.Table<TransferBetweenAccounts>()
                .Where(t => t.TransactionDate.Date == date.Date &&
                            (string.IsNullOrEmpty(searchTerm) ||
                             t.DebitAccount.Contains(searchTerm) || t.CreditAccount.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
