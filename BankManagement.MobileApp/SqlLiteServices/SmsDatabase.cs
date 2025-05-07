using BankManagement.MobileApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement.MobileApp.SqlLiteServices
{
    public class SmsDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public SmsDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<OnlinePurchase>().Wait();
            _database.CreateTableAsync<MadaPayPurchase>().Wait();
            _database.CreateTableAsync<PosPurchase>().Wait();
        }
    }
}
