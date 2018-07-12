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

		    var config = (IDialogConfigurationBuilder)
		        PrismApplicationBase.Current.Container.Resolve(typeof(IDialogConfigurationBuilder), "iDialogConfiguration");

		    if (config.GetResource<Color>(DialogSetting.QuestionDialogBackgroundColor, out var result))
		    {
		        Frame.BackgroundColor = result;
		    }

		    if (config.GetResource<Color>(DialogSetting.CancelButtonColor, out result))
		    {
		        CancelButton.BackgroundColor = result;
		    }

		    if (config.GetResource<Color>(DialogSetting.AcceptButtonColor, out result))
		    {
		        AcceptButton.BackgroundColor = result;
		    }
		}
	}
}