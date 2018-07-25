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

        private readonly IApplicationUserService _userService;
        private readonly IDialogService _dialogService;
        private readonly ITodoStore _todoStore;

        public ListsOverviewPageViewModel(INavigationService navigationService,
                                          ITodoStore todoStore,
                                          IApplicationUserService userService,
                                          IDialogService dialogService) : base(navigationService)

        {
            Title = "List Collection";

            _userService = userService;
            _dialogService = dialogService;
            _todoStore = todoStore;
        }

        #region Properties

        private ObservableCollection<ITodoList> _listCollection = new ObservableCollection<ITodoList>();
        public ObservableCollection<ITodoList> ListCollection
        {
            get => _listCollection;
            set => SetProperty(ref _listCollection, value);
        }

        private ITodoList _selectedItem;
        public ITodoList SelectedItem
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

            var list = await _todoStore.AddListAsync(new ListDto {Name = listName, Owner = _userService.DisplayName });

            if (list != null)
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

        private async void ExecuteDeleteListCommand()
        {
            if (!await _todoStore.DeleteListAsync(SelectedItem.Id)) return;
            ListCollection.Remove(SelectedItem);
            SelectedItem = null;
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

        protected override async void ConfigureOnNavigatingTo(INavigationParameters parameters)
        {
            ListCollection = new ObservableCollection<ITodoList>(await _todoStore.GetAllListsAsync());
        }

        private Task InitializationTask { get; set; }

        #endregion
    }
}