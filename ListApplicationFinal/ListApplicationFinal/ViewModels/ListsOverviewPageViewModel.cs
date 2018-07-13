using System.Collections.ObjectModel;
using System.Diagnostics;
using ListApplicationFinal.Domain;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class ListsOverviewPageViewModel : VmBase
    {
        public ListsOverviewPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            if(Debugger.IsAttached)
                TodoListsCollection = new ObservableCollection<TodoList>
                {
                    new TodoList("Indkøbdsliste", "Swagboy1337"),
                    new TodoList("En til", "Swagboy1337"),
                    new TodoList("Der var engang", "Swagboy1337"),
                    new TodoList("Tralala", "Swagboy1337"),
                    new TodoList("Bill me", "Swagboy1337"),
                    new TodoList("Familiy Guy", "Swagboy1337"),
                    new TodoList("Lol", "Swagboy1337"),
                    new TodoList("Nope", "Swagboy1337"),
                    new TodoList("My dinner", "Swagboy1337"),
                    new TodoList("Nu er det vidst nok", "Swagboy1337"),
                    new TodoList("Sidste", "Swagboy1337"),
                    new TodoList("Indkøbdsliste", "Swagboy1337"),
                    new TodoList("En til", "Swagboy1337"),
                    new TodoList("Der var engang", "Swagboy1337"),
                    new TodoList("Tralala", "Swagboy1337"),
                    new TodoList("Bill me", "Swagboy1337"),
                    new TodoList("Familiy Guy", "Swagboy1337"),
                    new TodoList("Lol", "Swagboy1337"),
                    new TodoList("Nope", "Swagboy1337"),
                    new TodoList("My dinner", "Swagboy1337"),
                    new TodoList("Nu er det vidst nok", "Swagboy1337"),
                    new TodoList("Sidste", "Swagboy1337"),
                    new TodoList("Indkøbdsliste", "Swagboy1337"),
                    new TodoList("En til", "Swagboy1337"),
                    new TodoList("Der var engang", "Swagboy1337"),
                    new TodoList("Tralala", "Swagboy1337"),
                    new TodoList("Bill me", "Swagboy1337"),
                    new TodoList("Familiy Guy", "Swagboy1337"),
                    new TodoList("Lol", "Swagboy1337"),
                    new TodoList("Nope", "Swagboy1337"),
                    new TodoList("My dinner", "Swagboy1337"),
                    new TodoList("Nu er det vidst nok", "Swagboy1337"),
                    new TodoList("Sidste", "Swagboy1337"),
                    new TodoList("Indkøbdsliste", "Swagboy1337"),
                    new TodoList("En til", "Swagboy1337"),
                    new TodoList("Der var engang", "Swagboy1337"),
                    new TodoList("Tralala", "Swagboy1337"),
                    new TodoList("Bill me", "Swagboy1337"),
                    new TodoList("Familiy Guy", "Swagboy1337"),
                    new TodoList("Lol", "Swagboy1337"),
                    new TodoList("Nope", "Swagboy1337"),
                    new TodoList("My dinner", "Swagboy1337"),
                    new TodoList("Nu er det vidst nok", "Swagboy1337"),
                    new TodoList("Sidste", "Swagboy1337")
                };
        }

        private ObservableCollection<TodoList> _todoListsCollection;
        public ObservableCollection<TodoList> TodoListsCollection
        {
            get => _todoListsCollection;
            set => SetProperty(ref _todoListsCollection, value);
        }
    }
}