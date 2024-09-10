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
            _database.CreateTableAsync<MadaAtheerPosPurchase>().Wait();
            _database.CreateTableAsync<TransferBetweenAccounts>().Wait();
            _database.CreateTableAsync<IncomingFundsTransferSarie>().Wait();
            _database.CreateTableAsync<PosPurchase>().Wait();
            _database.CreateTableAsync<UnknownTemplate>().Wait();
        }

        public Task<int> SaveMessageAsync(object message)
        {
            return message switch
            {
                OnlinePurchase onlinePurchase => _database.InsertAsync(onlinePurchase),
                MadaPayPurchase madaPayPurchase => _database.InsertAsync(madaPayPurchase),
                MadaAtheerPosPurchase madaAtheerPosPurchase => _database.InsertAsync(madaAtheerPosPurchase),
                TransferBetweenAccounts transfer => _database.InsertAsync(transfer),
                IncomingFundsTransferSarie incomingTransfer => _database.InsertAsync(incomingTransfer),
                PosPurchase posPurchase => _database.InsertAsync(posPurchase),
                UnknownTemplate unknownTemplate => _database.InsertAsync(unknownTemplate),
                _ => throw new ArgumentException("Unknown message type"),
            };
        }

        public Task<List<UnknownTemplate>> GetUnknownTemplatesAsync()
        {
            return _database.Table<UnknownTemplate>().ToListAsync();
        }
    }
}
