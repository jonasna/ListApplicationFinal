using System;
using Xamarin.Forms.Xaml;

namespace DialogServices
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuestionPopup : PopupBase<bool>
	{
		public QuestionPopup()
		{
			InitializeComponent ();
		}

	    private string _question;
        public string Question
	    {
	        get => _question;
	        set => SetProperty(ref _question, value);
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

        protected override void ConfigureBackButtonClicked()
        {
            SetResult(false);
        }

        protected override void ConfigureBackgroundClicked()
        {
            SetResult(false);
        }

	    protected override void OnInitiated()
	    {
	        
	    }

	    private void HandleCancelBtnClicked(object sender, EventArgs args)
	    {
	        SetResult(false);
	    }

	    private void HandleOkBtnClicked(object sender, EventArgs args)
	    {
	        SetResult(true);
	    }
    }
}