using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DialogServices.Service;
using ListApplicationFinal.DataServices;
using ListApplicationFinal.Domain;
using ListApplicationFinal.Toast;
using Prism.Commands;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public interface IDraggedCommandArgs
    {
        int NewIndex { get; }
        int OldIndex { get; }
        bool IsValid { get; }
    }

    public class SingleListPageViewModel : VmBase, IConfirmNavigationAsync
    {
        private readonly ITodoStore _todoStore;
        private readonly IDialogService _dialogService;
        private readonly IToastProvider _toastProvider;

        public SingleListPageViewModel(INavigationService navigationService,
            ITodoStore todoStore, IDialogService dialogService, IToastProvider toastProvider) : base(navigationService)
        {
            _todoStore = todoStore;
            _dialogService = dialogService;
            _toastProvider = toastProvider;
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

        private bool _isRefreshing;
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

        private DelegateCommand<IDraggedCommandArgs> _draggedCommand;
        public DelegateCommand<IDraggedCommandArgs> DraggedCommand =>
            _draggedCommand ?? (_draggedCommand = new DelegateCommand<IDraggedCommandArgs>(ExecuteDraggedCommand));

        private async void ExecuteDraggedCommand(IDraggedCommandArgs draggedArgs)
        {
            if (!draggedArgs.IsValid || draggedArgs.NewIndex == draggedArgs.OldIndex) return;

            await Task.Delay(100); // This is needed for updating the visual list

            Todos.Move(draggedArgs.OldIndex, draggedArgs.NewIndex);
            ChangeMade = true;
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand)).ObservesProperty(() => ChangeMade);

        private async void ExecuteSaveCommand()
        {
            List.ItemCollection = Todos;
            await _todoStore.UpdateListAsync(List.Id, List);
            _toastProvider.ShortMessage("Changes was saved!");
            ChangeMade = false;
        }

        private bool CanExecuteSaveCommand() => ChangeMade;

        private DelegateCommand _addNewItemCommand;
        public DelegateCommand AddNewItemCommand =>
            _addNewItemCommand ?? (_addNewItemCommand = new DelegateCommand(ExecuteAddNewItemCommand));

        private async void ExecuteAddNewItemCommand()
        {
            var newItemName = await _dialogService.StringQueryDialog("Add a to-do");
            if (newItemName != null)
            {
                var item = new TodoItemDto {Name = newItemName};
                Todos.Add(item);
                ChangeMade = true;
            }
        }

        #region Initialization

        protected override void ConfigureOnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(KnownNavigationParameters.XamlParam, out ITodoList todo))
            {
                List = todo;
                IsRefreshing = true;
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

            var delay = new Random().Next(5001);
            await Task.Delay(delay); // For testing purposes

            IsRefreshing = false;
            ChangeMade = false;
        }

        #endregion

        public async Task<bool> CanNavigateAsync(INavigationParameters parameters)
        {
            if (!ChangeMade) return true;

            if (await _dialogService.QuestionDialog("Continue? Changes will be lost.",
                "Unsaved changes!"))
                return true;

            return false;
        }

        public async void NavigateBack()
        {
            await NavigationService.GoBackAsync();
        }

        public void NotifyRefresh()
        {
            RaisePropertyChanged(nameof(IsRefreshing));
        }
    }
}