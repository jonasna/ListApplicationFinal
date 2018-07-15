using System.Collections.ObjectModel;
using ListApplicationFinal.DataServices;
using ListApplicationFinal.Domain;
using Prism.Commands;
using Prism.Navigation;
using ListApplicationFinal.Threading;

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
            set => SetProperty(ref _selectedItem, value, CheckSelectedItem);
        }

        private DelegateCommand _createNewListCommand;
        public DelegateCommand CreateNewListCommand =>
            _createNewListCommand ?? (_createNewListCommand = new DelegateCommand(ExecuteCreateNewListCommand, () => ListCollection != null)).ObservesProperty(() => ListCollection);

        private void ExecuteCreateNewListCommand()
        {
            TodoList list;
            if ((list = _todoService.AddList(new TodoList("List", _userService.DisplayName))) != null)
            {
                ListCollection.Add(list);
                SelectedItem = list;
            }
        }

        private DelegateCommand _deleteListCommand;

        public DelegateCommand DeleteListCommand =>
            _deleteListCommand ?? (_deleteListCommand =
                new DelegateCommand(ExecuteDeleteListCommand, () => SelectedItem != null)).ObservesProperty(() => SelectedItem);

        private void ExecuteDeleteListCommand()
        {
            if (_todoService.RemoveList(SelectedItem) != null)
            {
                ListCollection.Remove(SelectedItem);
                SelectedItem = null;
            }
        }

        private void CheckSelectedItem() => CheckSelectedItem(false);

        private void CheckSelectedItem(bool reset)
        {
            if (reset) SelectedItem = null;
            RaisePropertyChanged(nameof(SelectedItem));
        }

        protected override void ConfigureOnNavigatedTo(INavigationParameters parameters)
        {           
            Dispatcher.BeginInvoke(
                () =>
                {
                    return _todoService.GetAllLists();

                }, collection =>
                {
                    ListCollection = new ObservableCollection<TodoList>(collection);
                    return null;
                }, (o) =>
                {
                    CheckSelectedItem(true);
                });
        }

    }
}