using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ImageSource = Xamarin.Forms.ImageSource;

namespace ListApplicationFinal.CustomControls.Buttons
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImageButton : ContentView
	{
	    private const int DefaultCornerRadius = -1;

        public ImageButton ()
		{
			InitializeComponent ();
	        Image.DownsampleToViewSize = true;
        }

	    public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(ImageButton));

	    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(ImageButton), DefaultCornerRadius);

	    public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(Frame), true);

	    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ImageButton), null, BindingMode.Default, null, CommandPropertyChanged);

	    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ImageButton), null);

	    public ImageSource Source
	    {
	        get => (ImageSource)GetValue(SourceProperty);
	        set => SetValue(SourceProperty, value);
	    }

        public int CornerRadius
	    {
	        get => (int)GetValue(CornerRadiusProperty);
	        set => SetValue(CornerRadiusProperty, value);
	    }

	    public bool HasShadow
	    {
	        get => (bool)GetValue(HasShadowProperty);
	        set => SetValue(HasShadowProperty, value);
	    }

	    public ICommand Command
	    {
	        get => (ICommand)GetValue(CommandProperty);
	        set => SetValue(CommandProperty, value);
	    }

	    public object CommandParameter
	    {
	        get => GetValue(CommandParameterProperty);
	        set => SetValue(CommandParameterProperty, value);
	    }

	    private static async Task TransitionDown(ImageButton button)
	    {
	        try
	        {
	            button.Image.Transformations = new List<ITransformation> { new ColorSpaceTransformation(FFColorMatrix.PolaroidColorMatrix) };
	            await button.Frame.ScaleTo(0.9, 50, Easing.Linear);
	        }
	        catch
	        {
	            // nothing to do
	        }
        }
        private static async Task TransitionUp(ImageButton button)
        {
            try
            {
                button.Image.Transformations = new List<ITransformation> { };
                await button.Frame.ScaleTo(1, 50, Easing.Linear);
            }
            catch
            {
                // nothing to do
            }
        }

        public event EventHandler Clicked;
	    public event EventHandler Pressed;
	    public event EventHandler Released;

        private void ClickableFrame_Clicked(object sender, EventArgs e)
        {
            Command?.Execute(CommandParameter);
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        private async void ClickableFrame_Pressed(object sender, EventArgs e)
        {
            await TransitionDown(this);
            Pressed?.Invoke(this, EventArgs.Empty);
        }

        private async void ClickableFrame_Released(object sender, EventArgs e)
        {
            await TransitionUp(this);
            Released?.Invoke(this, EventArgs.Empty);
        }

	    private static void CommandPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
	    {
	        var btn = (ImageButton)bindable;
	        if (btn == null)
	            return;

	        if (oldvalue != null)
	        {
	            var cmd = (ICommand)oldvalue;
	            cmd.CanExecuteChanged -= btn.CmdOnCanExecuteChanged;
	        }

	        if (newvalue != null)
	        {
	            var cmd = (ICommand)newvalue;
	            cmd.CanExecuteChanged += btn.CmdOnCanExecuteChanged;
	            SetButton(btn, cmd);
	        }
	    }

	    private void CmdOnCanExecuteChanged(object sender, EventArgs e)
	    {
	        var cmd = (ICommand)sender;
	        if (cmd != null)
	        {
	            SetButton(this, cmd);
	        }
	    }

	    private static void SetButton(ImageButton imageButton, ICommand command)
	    {
	        try
	        {
	            imageButton.IsEnabled = command.CanExecute(null);
	            var image = imageButton.Image;
	            image.Transformations = NoTransformation;
	            if (!imageButton.IsEnabled)
	            {
	                image.Transformations = Transformation;
	            }
	        }
	        catch (Exception e)
	        {
	            System.Diagnostics.Debug.WriteLine(e);
	        }
	    }

	    private static readonly List<ITransformation> NoTransformation;
	    private static readonly List<ITransformation> Transformation;

	    static ImageButton()
	    {
            Transformation = new List<ITransformation>{ new ColorSpaceTransformation(FFColorMatrix.GrayscaleColorMatrix) };
            NoTransformation = new List<ITransformation>();
	    }

	}
}