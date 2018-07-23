using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DialogServices.Service;
using ListApplicationFinal.DataServices;
using ListApplicationFinal.Domain;
using Prism.Commands;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class ListsOverviewPageViewModel : VmBase
    {

        private readonly ITodoService _todoService;
        private readonly IApplicationUserService _userService;
        private readonly IDialogService _dialogService;

        public ListsOverviewPageViewModel(INavigationService navigationService,
                                          ITodoService todoService,
                                          IApplicationUserService userService,
                                          IDialogService dialogService) : base(navigationService)

        {
            Title = "List Collection";
            _todoService = todoService;
            _userService = userService;
            _dialogService = dialogService;
        }

        #region Properties

        private ObservableCollection<TodoList> _listCollection;
        public ObservableCollection<TodoList> ListCollection
        {
            get => _listCollection;
            set => SetProperty(ref _listCollection, value);
        }

        private TodoList _selectedItem;
        public TodoList SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private bool CanModifyOrDelete => SelectedItem != null;

        #endregion

        #region Create List Command

        private DelegateCommand _createNewListCommand;
        public DelegateCommand CreateNewListCommand =>
            _createNewListCommand ?? (_createNewListCommand = new DelegateCommand(ExecuteCreateNewListCommand, CanExecuteCreateNewListCommand)).ObservesProperty(() => ListCollection);

        private async void ExecuteCreateNewListCommand()
        {
            var listName = await _dialogService.StringQueryDialog("Please enter the list name");
            if (listName == null) return;

            TodoList list;
            if ((list = _todoService.AddList(new TodoList(listName, _userService.DisplayName))) == null) return;
            ListCollection.Add(list);
            SelectedItem = list;
        }

        private bool CanExecuteCreateNewListCommand() => ListCollection != null;

        #endregion

        #region Delete List Command

        private DelegateCommand _deleteListCommand;

        public DelegateCommand DeleteListCommand =>
            _deleteListCommand ?? (_deleteListCommand = new DelegateCommand(ExecuteDeleteListCommand, CanExecuteDeleteListCommand)).ObservesProperty(() => SelectedItem);

        private void ExecuteDeleteListCommand()
        {
            if (_todoService.RemoveList(SelectedItem) != null)
            {
                ListCollection.Remove(SelectedItem);
                SelectedItem = null;
            }
        }

        private bool CanExecuteDeleteListCommand() => CanModifyOrDelete;

        #endregion

        #region Modify List Command

        private DelegateCommand _modifyListCommand;
        public DelegateCommand ModifyListCommand =>
            _modifyListCommand ?? (_modifyListCommand = new DelegateCommand(ExecuteModifyListCommand, CanExecuteModifyListCommand)).ObservesProperty(() => SelectedItem);

        private async void ExecuteModifyListCommand()
        {
            await NavigationService.NavigateAsync("SingleListPage", new NavigationParameters{{ KnownNavigationParameters.XamlParam, SelectedItem}});
        }

        private bool CanExecuteModifyListCommand() => CanModifyOrDelete;

        #endregion

        #region Initialization

        protected override async void ConfigureOnNavigatedTo(INavigationParameters parameters)
        {
            await InitializationTask;
        }

        protected override void ConfigureOnNavigatingTo(INavigationParameters parameters)
        {
            Init();
        }

        private Task InitializationTask { get; set; }

        private void Init()
        {
            InitializationTask = Task.Run(() =>
            {
                var list = _todoService.GetAllLists();
                ListCollection = new ObservableCollection<TodoList>(list);
            });
        }

        #endregion
    }
}