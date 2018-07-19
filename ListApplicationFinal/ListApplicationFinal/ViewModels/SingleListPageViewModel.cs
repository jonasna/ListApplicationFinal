using System.Collections.ObjectModel;
using ListApplicationFinal.Domain;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.ListView.XForms;

namespace ListApplicationFinal.ViewModels
{
    public class SingleListPageViewModel : VmBase
    {
        private TodoList Todo { get; set; }

        public SingleListPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region Properties

        private ObservableCollection<TodoListItem> _toDoCollection;
        public ObservableCollection<TodoListItem> ToDoCollection
        {
            get => _toDoCollection;
            set => SetProperty(ref _toDoCollection, value);
        }

        #endregion

        private DelegateCommand<ItemTappedEventArgs> _itemTappedCommand;
        public DelegateCommand<ItemTappedEventArgs> ItemTappedCommand =>
            _itemTappedCommand ?? (_itemTappedCommand = new DelegateCommand<ItemTappedEventArgs>(ExecuteItemTappedCommand));

        private void ExecuteItemTappedCommand(ItemTappedEventArgs itemTapped)
        {
            var item = (TodoListItem) itemTapped.ItemData;
            item.Complete = !item.Complete;
        }

        protected override void ConfigureOnNavigatedTo(INavigationParameters parameters)
        {
            if (ToDoCollection == null && Todo != null)
            {
                ToDoCollection = new ObservableCollection<TodoListItem>(Todo);
                if (ToDoCollection.Count == 0)
                {
                    ToDoCollection.Add(new TodoListItem("An item", "This is a test!"));
                }
            }
        }

        protected override void ConfigureOnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(KnownNavigationParameters.XamlParam, out TodoList todo))
            {
                Todo = todo;
            }             
        }
    }
}