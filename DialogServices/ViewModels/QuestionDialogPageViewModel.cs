using Prism.Commands;
using Prism.Navigation;

namespace DialogServices.ViewModels
{
    public class QuestionDialogPageViewModel : DialogPageVmBase<bool>
    {
        private string _question;

        public QuestionDialogPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public string Question
        {
            get => _question;
            set => SetProperty(ref _question, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _notOkBtnText;
        public string NotOkBtnText
        {
            get => _notOkBtnText;
            set => SetProperty(ref _notOkBtnText, value);
        }

        private string _okBtnText;
        public string OkBtnText
        {
            get => _okBtnText;
            set => SetProperty(ref _okBtnText, value);
        }

        private DelegateCommand _acceptCommand;
        public DelegateCommand AcceptCommand =>
            _acceptCommand ?? (_acceptCommand = new DelegateCommand(ExecuteAcceptCommand));

        protected virtual void ExecuteAcceptCommand()
        {
            SetCompleted(true);
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(ExecuteCancelCommand));

        protected virtual void ExecuteCancelCommand()
        {
            SetCompleted(false);
        }

        protected override void ConfigureOnLoad(INavigationParameters parameters)
        {
            Question = (string) parameters["Question"];
            Title = (string) parameters["Title"];
            OkBtnText = (string) parameters["OkBtnText"];
            NotOkBtnText = (string) parameters["NotOkBtnText"];
        }
    }
}