using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ListApplicationFinal.Domain;

namespace ListApplicationFinal.DataServices
{
    public class TodoItemDto : INotifyPropertyChanged, ITodo
    {
        public string Name { get; set; }

        private bool _complete;
        public bool Complete
        {
            get => _complete;
            set
            {
                if (_complete == value) return;
                _complete = value;
                OnPropertyChanged(nameof(Complete));
            }
        }

        public string Description { get; set; } = string.Empty;

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public object Clone()
        {
            return new TodoItemDto()
            {
                Name = Name,
                _complete = _complete,
                Description = Description
            };
        }
    }

    public class ListDto : ITodoList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public DateTime PointOfCreation { get; set; } = DateTime.Now;
        public IEnumerable<ITodo> ItemCollection { get; set; } = new ITodo[0];
    }

    public class FakeTodoStore : ITodoStore
    {
        private readonly Dictionary<string, ITodoList> _collection;

        public FakeTodoStore()
        {
            _collection = new Dictionary<string, ITodoList>();
            Initialize();
        }

        private void Initialize()
        {
            for (var i = 0; i < 10; i++)
            {
                var todo = new ListDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = $"List {i+1}",
                    Owner = "SwagBoy",
                    PointOfCreation = DateTime.Now.AddMinutes(-i),
                    ItemCollection = new List<ITodo>
                    {
                        new TodoItemDto
                        {
                            Name = "Item 1",
                            Description = "This is a test item."
                        },
                        new TodoItemDto
                        {
                            Name = "Item 2",
                            Description = "This is a test item."
                        },
                        new TodoItemDto
                        {
                            Name = "Item 3",
                            Description = "This is a test item."
                        },
                        new TodoItemDto
                        {
                            Name = "Item 4",
                            Description = "This is a test item."
                        },
                        new TodoItemDto
                        {
                            Name = "Item 5",
                            Description = "This is a test item."
                        }
                    }
                };

                _collection.Add(todo.Id, todo);
            }
        }

        public Task<ITodoList> AddListAsync(ITodoList item)
        {
            return Task.Run(() =>
            {
                item.Id = Guid.NewGuid().ToString();

                if (_collection.ContainsKey(item.Id))
                    return null;

                var newItem = CopyModelFull(item);

                _collection.Add(newItem.Id, newItem);

                return item;
            });
        }

        public Task<bool> DeleteListAsync(string id)
        {
            return Task.Run(() => _collection.Remove(id));
        }

        public Task<IEnumerable<ITodoList>> GetAllListsAsync()
        {
            return Task.Run(() => _collection.Values.Select(CopyModel));
        }

        public Task<ITodoList> GetListAsync(string id)
        {
            return Task.Run(() =>
            {
                if (!_collection.ContainsKey(id))
                    return null;

                return CopyModelFull(_collection[id]);
            });
        }

        public Task<ITodoList> UpdateListAsync(string id, ITodoList item)
        {
            return Task.Run(() =>
            {
                if (!_collection.ContainsKey(id))
                    return null;

                if (id != item.Id)
                    return null;

                _collection[id] = CopyModelFull(item);

                return item;
            });
        }

        private static ITodoList CopyModelFull(ITodoList model)
        {
            var newItem = CopyModel(model);
            newItem.ItemCollection = model.ItemCollection.Clone();
            return newItem;
        }

        private static ITodoList CopyModel(ITodoList model)
        {
            var newItem = new ListDto
            {
                Id = model.Id,
                Name = model.Name,
                Owner = model.Owner,
                PointOfCreation = model.PointOfCreation,
                ItemCollection = null
            };
            return newItem;
        }

    }

    internal static class Extensions
    {
        public static IEnumerable<T> Clone<T>(this IEnumerable<T> toClone) where T : ICloneable
        {
            return toClone.Select(item => (T) item.Clone()).ToList();
        }
    }
}