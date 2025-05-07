using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement.MobileApp.Repositories
{
    public interface IRepository<T>
    {
        Task<int> SaveMessageAsync(T message);
        Task<List<T>> GetTransactionsAsync();
        Task<List<T>> GetFilteredTransactionsAsync(string searchTerm, DateTime date);
    }
}
