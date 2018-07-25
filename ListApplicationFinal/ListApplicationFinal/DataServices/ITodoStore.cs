using System.Collections.Generic;
using System.Threading.Tasks;
using ListApplicationFinal.Domain;

namespace ListApplicationFinal.DataServices
{
    public interface ITodoStore
    {
        Task<ITodoList> AddListAsync(ITodoList item);
        Task<ITodoList> UpdateListAsync(string id, ITodoList item);
        Task<bool> DeleteListAsync(string id);
        Task<IEnumerable<ITodoList>> GetAllListsAsync();
        Task<ITodoList> GetListAsync(string id);
    }
}