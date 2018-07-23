using System.Collections.ObjectModel;
using ListApplicationFinal.Domain;
using Prism.Commands;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public interface IDraggingCommandArgs
    {
        int NewIndex { get; }
        int OldIndex { get; }
    }

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

        private DelegateCommand<TodoListItem> _itemTappedCommand;
        public DelegateCommand<TodoListItem> ItemTappedCommand =>
            _itemTappedCommand ?? (_itemTappedCommand = new DelegateCommand<TodoListItem>(ExecuteItemTappedCommand));

        private void ExecuteItemTappedCommand(TodoListItem tappedItem)
        {
            tappedItem.Complete = !tappedItem.Complete;
        }

        private DelegateCommand<int?> _swipeCommand;
        public DelegateCommand<int?> SwipeCommand =>
            _swipeCommand ?? (_swipeCommand = new DelegateCommand<int?>(ExecuteSwipeCommand));

        private void ExecuteSwipeCommand(int? itemIndex)
        {
            if (itemIndex.HasValue)
            {
                ToDoCollection.RemoveAt(itemIndex.Value);
            }
        }

        private DelegateCommand<IDraggingCommandArgs> _draggingCommand;
        public DelegateCommand<IDraggingCommandArgs> DraggingCommand =>
            _draggingCommand ?? (_draggingCommand = new DelegateCommand<IDraggingCommandArgs>(ExecuteDraggingCommand));

        private void ExecuteDraggingCommand(IDraggingCommandArgs dragginArgs)
        {
            if (dragginArgs == null) return;
            ToDoCollection.Move(dragginArgs.OldIndex, dragginArgs.NewIndex);
        }

        protected override void ConfigureOnNavigatedTo(INavigationParameters parameters)
        {
            if (ToDoCollection == null && Todo != null)
            {
                ToDoCollection = new ObservableCollection<TodoListItem>(Todo);
            }
        }

        protected override void ConfigureOnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(KnownNavigationParameters.XamlParam, out TodoList todo))
            {
                Todo = todo;

                if (Todo.Count == 0)
                {
                    Todo.Add(new TodoListItem("Test", "Swagboy testing"));
                    Todo.Add(new TodoListItem("Another Test Item", "This description is slighty longer than the one above. This is very exciting"));
                }

                Title = todo.Name;
            }
        }
    }
}