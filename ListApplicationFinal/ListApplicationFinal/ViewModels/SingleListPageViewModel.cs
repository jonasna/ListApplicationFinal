using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ListApplicationFinal.DataServices;
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
        private readonly ITodoStore _todoStore;

        public SingleListPageViewModel(INavigationService navigationService,
            ITodoStore todoStore) : base(navigationService)
        {
            _todoStore = todoStore;
        }

        #region Properties and fields

        private bool _changeMade;
        private bool ChangeMade
        {
            get => _changeMade;
            set => SetProperty(ref _changeMade, value);
        }
        
        public ITodoList List { get; set; }

        private ObservableCollection<ITodo> _toDos;
        public ObservableCollection<ITodo> Todos
        {
            get => _toDos;
            set => SetProperty(ref _toDos, value);
        }

        private bool _isRefreshing = true;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        #endregion

        private DelegateCommand<ITodo> _itemTappedCommand;
        public DelegateCommand<ITodo> ItemTappedCommand =>
            _itemTappedCommand ?? (_itemTappedCommand = new DelegateCommand<ITodo>(ExecuteItemTappedCommand));

        private void ExecuteItemTappedCommand(ITodo tappedItem)
        {
            tappedItem.Complete = !tappedItem.Complete;
            ChangeMade = true;
        }

        private DelegateCommand<int?> _swipeCommand;
        public DelegateCommand<int?> SwipeCommand =>
            _swipeCommand ?? (_swipeCommand = new DelegateCommand<int?>(ExecuteSwipeCommand));

        private void ExecuteSwipeCommand(int? itemIndex)
        {
            if (itemIndex.HasValue)
            {
                Todos.RemoveAt(itemIndex.Value);
                ChangeMade = true;
            }
        }

        private DelegateCommand<IDraggingCommandArgs> _draggingCommand;
        public DelegateCommand<IDraggingCommandArgs> DraggingCommand =>
            _draggingCommand ?? (_draggingCommand = new DelegateCommand<IDraggingCommandArgs>(ExecuteDraggingCommand));

        private async void ExecuteDraggingCommand(IDraggingCommandArgs draggingArgs)
        {
            if (draggingArgs == null) return;

            await Task.Delay(100); // If this is not here the visual list will not be updated correctly
            Todos.Move(draggingArgs.OldIndex, draggingArgs.NewIndex);
            ChangeMade = true;
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand)).ObservesProperty(() => ChangeMade);

        private async void ExecuteSaveCommand()
        {
            List.ItemCollection = Todos;
            await _todoStore.UpdateListAsync(List.Id, List);
            ChangeMade = false;
        }

        private bool CanExecuteSaveCommand() => ChangeMade;

        #region Initialization

        protected override void ConfigureOnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(KnownNavigationParameters.XamlParam, out ITodoList todo))
            {
                List = todo;
            }
        }

        protected override async void ConfigureOnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(KnownNavigationParameters.XamlParam))
            {
                await Refresh();
            }
        }

        private async Task Refresh()
        {
            IsRefreshing = true;

            List = await _todoStore.GetListAsync(List.Id);
            Todos = new ObservableCollection<ITodo>(List.ItemCollection);

            await Task.Delay(100); // For testing purposes
            IsRefreshing = false;
            ChangeMade = false;
        }

        #endregion
    }
}