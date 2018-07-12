using System;
using Prism.Mvvm;

namespace ListApplicationFinal.Domain
{
    public class TodoListItem : BindableBase
    {
        public TodoListItem(string name, string description = null)
        {
            _name = name;
            _description = description ?? string.Empty;
            _complete = false;
            _itemIdGuid = Guid.NewGuid();
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _complete;
        public bool Complete
        {
            get => _complete;
            set => SetProperty(ref _complete, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private Guid _itemIdGuid;
        public Guid ItemIdGuid
        {
            get => _itemIdGuid;
            set => SetProperty(ref _itemIdGuid, value);
        }
    }
}