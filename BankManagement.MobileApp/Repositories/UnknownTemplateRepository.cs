using BankManagement.MobileApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement.MobileApp.Repositories
{
    public class UnknownTemplateRepository : IRepository<UnknownTemplate>
    {
        private readonly SQLiteAsyncConnection _database;

        public UnknownTemplateRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<UnknownTemplate>().Wait();
        }

        public Task<int> SaveMessageAsync(UnknownTemplate message)
        {
            return _database.InsertAsync(message);
        }

        public Task<List<UnknownTemplate>> GetTransactionsAsync()
        {
            return _database.Table<UnknownTemplate>().ToListAsync();
        }

        public Task<List<UnknownTemplate>> GetFilteredTransactionsAsync(string searchTerm, DateTime date)
        {
            return _database.Table<UnknownTemplate>()
                .Where(t => string.IsNullOrEmpty(searchTerm) || t.RawMessage.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
