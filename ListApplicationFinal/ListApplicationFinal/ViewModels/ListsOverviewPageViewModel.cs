using System.Collections.ObjectModel;
using ListApplicationFinal.DataServices;
using ListApplicationFinal.Domain;
using Prism.Commands;
using Prism.Navigation;
using ListApplicationFinal.Threading;
using Prism.Services;

namespace ListApplicationFinal.ViewModels
{
    public class ListsOverviewPageViewModel : VmBase
    {

        private readonly ITodoService _todoService;
        private readonly IApplicationUserService _userService;

        public ListsOverviewPageViewModel(INavigationService navigationService,
                                          ITodoService todoService,
                                          IApplicationUserService userService) : base(navigationService)

        {
            Title = "List Collection";
            _todoService = todoService;
            _userService = userService;
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

        #endregion

        #region Create List Command

        private DelegateCommand _createNewListCommand;
        public DelegateCommand CreateNewListCommand =>
            _createNewListCommand ?? (_createNewListCommand = new DelegateCommand(ExecuteCreateNewListCommand, CanExecuteCreateNewListCommand)).ObservesProperty(() => ListCollection);

        private void ExecuteCreateNewListCommand()
        {
            TodoList list;
            if ((list = _todoService.AddList(new TodoList("List", _userService.DisplayName))) != null)
            {
                ListCollection.Add(list);
                SelectedItem = list;
            }
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

        private bool CanExecuteDeleteListCommand() => SelectedItem != null;

        #endregion

        #region Modify List Command

        private DelegateCommand _modifyListCommand;
        public DelegateCommand ModifyListCommand =>
            _modifyListCommand ?? (_modifyListCommand = new DelegateCommand(ExecuteModifyListCommand, CanExecuteModifyListCommand)).ObservesProperty(() => SelectedItem);

        private void ExecuteModifyListCommand()
        {

        }

        private bool CanExecuteModifyListCommand() => SelectedItem != null;

        #endregion

        #region Initiation

        protected override async void ConfigureOnNavigatingTo(INavigationParameters parameters)
        {
            ListCollection = new ObservableCollection<TodoList>(await _todoService.GetAllListsAsync());
        }

        #endregion
    }
}