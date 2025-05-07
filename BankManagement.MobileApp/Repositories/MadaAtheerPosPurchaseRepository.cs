using BankManagement.MobileApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement.MobileApp.Repositories
{
    public class MadaAtheerPosPurchaseRepository : IRepository<MadaAtheerPosPurchase>
    {
        private readonly SQLiteAsyncConnection _database;

        public MadaAtheerPosPurchaseRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<MadaAtheerPosPurchase>().Wait();
        }

        public Task<int> SaveMessageAsync(MadaAtheerPosPurchase message)
        {
            return _database.InsertAsync(message);
        }

        public Task<List<MadaAtheerPosPurchase>> GetTransactionsAsync()
        {
            return _database.Table<MadaAtheerPosPurchase>().ToListAsync();
        }

        public Task<List<MadaAtheerPosPurchase>> GetFilteredTransactionsAsync(string searchTerm, DateTime date)
        {
            return _database.Table<MadaAtheerPosPurchase>()
                .Where(t => t.TransactionDate.Date == date.Date &&
                            (string.IsNullOrEmpty(searchTerm) ||
                             t.Merchant.Contains(searchTerm) || t.Account.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
