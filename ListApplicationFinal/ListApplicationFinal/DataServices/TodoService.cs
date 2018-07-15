using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListApplicationFinal.Domain;

namespace ListApplicationFinal.DataServices
{
    public class TodoService : ITodoService
    {
        private readonly Dictionary<Guid, TodoList> _listCollection;

        public TodoService()
        {
            _listCollection = new Dictionary<Guid, TodoList>();

            if(_listCollection.Count == 0)
                Init();
        }

        public TodoList AddList(TodoList list)
        {
            if (!_listCollection.ContainsKey(list.Id))
            {
                _listCollection[list.Id] = list;
                return list;
            }

            return null;
        }

        public TodoList RemoveList(TodoList list)
        {
            if (_listCollection.Remove(list.Id))
                return list;
            return null;
        }

        public bool UpdateList(TodoList list)
        {
            if (_listCollection.ContainsKey(list.Id))
            {
                _listCollection[list.Id] = list;
                return true;
            }

            return false;
        }

        public ICollection<TodoList> GetAllLists()
        {
            return _listCollection.Values;
        }

        public Task<TodoList> AddListAsync(TodoList list)
        {
            return Task.Run(() => AddList(list));
        }

        public Task<bool> UpdateListAsync(TodoList list)
        {
            return Task.Run(() => UpdateList(list));
        }

        public Task<TodoList> RemoveListAsync(TodoList list)
        {
            return Task.Run(() => RemoveList(list));
        }

        public Task<ICollection<TodoList>> GetAllListsAsync()
        {
            return Task.Run(() => GetAllLists());
        }

        public async void Init()
        {
            await InitAsync();
        }

        public Task InitAsync()
        {
            return Task.Run(() =>
            {
                AddList(new TodoList("List", "SwagBoy1335"));
                AddList(new TodoList("List", "SwagBoy1335"));
                AddList(new TodoList("List", "SwagBoy1335"));
                AddList(new TodoList("List", "SwagBoy1335"));
                AddList(new TodoList("List", "SwagBoy1335"));
                AddList(new TodoList("List", "SwagBoy1335"));
                AddList(new TodoList("List", "SwagBoy1335"));
                AddList(new TodoList("List", "SwagBoy1335"));
                AddList(new TodoList("List", "SwagBoy1335"));
                AddList(new TodoList("List", "SwagBoy1335"));
                AddList(new TodoList("List", "SwagBoy1335"));
                AddList(new TodoList("List", "SwagBoy1335"));
            });
        }

    }
}
