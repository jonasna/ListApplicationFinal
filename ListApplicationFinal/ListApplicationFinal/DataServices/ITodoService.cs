using System.Collections.Generic;
using System.Threading.Tasks;
using ListApplicationFinal.Domain;

namespace ListApplicationFinal.DataServices
{
    public interface ITodoService
    {
        TodoList AddList(TodoList list);
        TodoList RemoveList(TodoList list);
        bool UpdateList(TodoList list);
        ICollection<TodoList> GetAllLists();
        Task<TodoList> AddListAsync(TodoList list);
        Task<TodoList> RemoveListAsync(TodoList list);
        Task<bool> UpdateListAsync(TodoList list);
        Task<ICollection<TodoList>> GetAllListsAsync();
    }
}