using BankManagement.MobileApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement.MobileApp.Repositories
{
    public class PosPurchaseRepository : IRepository<PosPurchase>
    {
        private readonly SQLiteAsyncConnection _database;

        public PosPurchaseRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<PosPurchase>().Wait();
        }

        public Task<int> SaveMessageAsync(PosPurchase message)
        {
            return _database.InsertAsync(message);
        }

        public Task<List<PosPurchase>> GetTransactionsAsync()
        {
            return _database.Table<PosPurchase>().ToListAsync();
        }

        public Task<List<PosPurchase>> GetFilteredTransactionsAsync(string searchTerm, DateTime date)
        {
            return _database.Table<PosPurchase>()
                .Where(p => p.TransactionDate.Date == date.Date &&
                            (string.IsNullOrEmpty(searchTerm) ||
                             p.Merchant.Contains(searchTerm) || p.Account.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
