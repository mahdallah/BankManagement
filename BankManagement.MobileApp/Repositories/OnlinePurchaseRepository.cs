using BankManagement.MobileApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement.MobileApp.Repositories
{
    public class OnlinePurchaseRepository : IRepository<OnlinePurchase>
    {
        private readonly SQLiteAsyncConnection _database;

        public OnlinePurchaseRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<OnlinePurchase>().Wait();
        }

        public Task<int> SaveMessageAsync(OnlinePurchase message)
        {
            return _database.InsertAsync(message);
        }

        public Task<List<OnlinePurchase>> GetTransactionsAsync()
        {
            return _database.Table<OnlinePurchase>().ToListAsync();
        }

        public Task<List<OnlinePurchase>> GetFilteredTransactionsAsync(string searchTerm, DateTime date)
        {
            return _database.Table<OnlinePurchase>()
                .Where(p => p.TransactionDate.Date == date.Date &&
                            (string.IsNullOrEmpty(searchTerm) ||
                             p.Merchant.Contains(searchTerm) || p.Account.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
