using BankManagement.MobileApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement.MobileApp.Repositories
{
    public class MadaPayPurchaseRepository : IRepository<MadaPayPurchase>
    {
        private readonly SQLiteAsyncConnection _database;

        public MadaPayPurchaseRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<MadaPayPurchase>().Wait();
        }

        public Task<int> SaveMessageAsync(MadaPayPurchase message)
        {
            return _database.InsertAsync(message);
        }

        public Task<List<MadaPayPurchase>> GetTransactionsAsync()
        {
            return _database.Table<MadaPayPurchase>().ToListAsync();
        }

        public Task<List<MadaPayPurchase>> GetFilteredTransactionsAsync(string searchTerm, DateTime date)
        {
            return _database.Table<MadaPayPurchase>()
                .Where(p => p.TransactionDate.Date == date.Date &&
                            (string.IsNullOrEmpty(searchTerm) ||
                             p.Merchant.Contains(searchTerm) || p.Card.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
