using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DialogServices.Register;
using Prism;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DialogServices.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuestionDialogPage : PopupPageBase
	{
		public QuestionDialogPage ()
		{
			InitializeComponent ();

		    var config = (IDialogConfiguration)
		        PrismApplicationBase.Current.Container.Resolve(typeof(IDialogConfiguration), "iDialogConfiguration");

		    Frame.Padding = config.Padding;
		    Frame.BackgroundColor = config.QuestionDialogBackgroundColor;

		    CancelButton.FontSize = config.ButtonTextSize;
		    AcceptButton.FontSize = config.ButtonTextSize;

		    CancelButton.BackgroundColor = config.CancelButtonColor;
		    AcceptButton.BackgroundColor = config.AcceptButtonColor;

		    Label.FontSize = config.DialogTextSize;
		}
	}
}