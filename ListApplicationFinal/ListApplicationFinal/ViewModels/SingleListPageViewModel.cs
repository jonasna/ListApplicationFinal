using System.Collections.Generic;
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

        private string ListId { get; set; }

        #region Properties

        private ObservableCollection<ITodo> _toDos;
        public ObservableCollection<ITodo> Todos
        {
            get => _toDos;
            set => SetProperty(ref _toDos, value);
        }

        #endregion

        private DelegateCommand<ITodo> _itemTappedCommand;
        public DelegateCommand<ITodo> ItemTappedCommand =>
            _itemTappedCommand ?? (_itemTappedCommand = new DelegateCommand<ITodo>(ExecuteItemTappedCommand));

        private void ExecuteItemTappedCommand(ITodo tappedItem)
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
                Todos.RemoveAt(itemIndex.Value);
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
        }

        #region Initialization

        protected override async void ConfigureOnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(KnownNavigationParameters.XamlParam, out ITodoList todo))
            {
                Title = todo.Name;
                ListId = todo.Id;
                Todos = new ObservableCollection<ITodo>(await Load());
            }
        }

        private async Task<IEnumerable<ITodo>> Load()
        {
            var list = await _todoStore.GetListAsync(ListId);
            return list.ItemCollection;
        }

        #endregion

    }
}