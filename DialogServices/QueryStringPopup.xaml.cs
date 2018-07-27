using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DialogServices
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QueryStringPopup : PopupBase<string>
	{
		public QueryStringPopup()
		{
			InitializeComponent ();
		    Input = string.Empty;
		}

	    private string _queryQuestion;
	    public string QueryQuestion
	    {
	        get => _queryQuestion;
	        set => SetProperty(ref _queryQuestion, value);
	    }

	    private string _input;
	    public string Input
	    {
	        get => _input;
	        set => SetProperty(ref _input, value, InputOnSet);
	    }

	    private void InputOnSet()
	    {
	        CanExecute = !string.IsNullOrWhiteSpace(Input);
	    }

	    private bool _canExecute;
	    public bool CanExecute
	    {
	        get => _canExecute;
	        set => SetProperty(ref _canExecute, value);
	    }

	    private void HandleConfirmBtnClicked(object sender, EventArgs args)
	    {
            SetResult(Input);
	    }

	    private void HandleCancelBtnClicked(object sender, EventArgs args)
	    {
            SetResult(null);
	    }

        protected override void ConfigureBackButtonClicked()
	    {
	        SetResult(null);
	    }

	    protected override void ConfigureBackgroundClicked()
	    {
	        SetResult(null);
	    }

	    protected override void OnInitiated()
	    {
	        
	    }
	}
}