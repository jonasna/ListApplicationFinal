using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Prism.Mvvm;

namespace ListApplicationFinal.Domain
{
    public class TodoList : ObservableCollection<TodoListItem>
    {
        public TodoList(string name, string owner)
        {
            _name = name;
            _owner = owner;
            _pointOfCreation = DateTime.Now;
            _id = Guid.NewGuid();
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        private string _owner;
        public string Owner
        {
            get => _owner;
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Owner)));
                }
            }
        }

        private DateTime _pointOfCreation;
        public DateTime PointOfCreation
        {
            get => _pointOfCreation;
            set
            {
                if (_pointOfCreation != value)
                {
                    _pointOfCreation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(PointOfCreation)));
                }
            }
        }

        private Guid _id;
        public Guid Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Id)));
                }
            }
        }
    }
}