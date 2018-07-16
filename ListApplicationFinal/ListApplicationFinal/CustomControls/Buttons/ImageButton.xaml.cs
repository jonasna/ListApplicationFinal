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

	    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ImageButton), null, BindingMode.Default, null,
	        ((b, o, n) => ((ImageButton)b).CommandPropertyChanged((ICommand)o, (ICommand)n)), (b, o, n) => ((ImageButton)b).CommandPropertyChanging((ICommand)o, (ICommand)n));

	    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ImageButton));

        public new static readonly BindableProperty IsEnabledProperty = BindableProperty.Create("IsEnabled", typeof(bool), typeof(ImageButton), true, BindingMode.Default, null,
            (b, o, n) => ((ImageButton)b).IsEnabledPropertyChanged((bool)o, (bool)n));

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

	    public new bool IsEnabled
	    {
	        set => SetValue(IsEnabledProperty, value);
	        get => (bool) GetValue(IsEnabledProperty);
	    }

	    private void CommandPropertyChanging(ICommand oldValue, ICommand newValue)
	    {
	        if (oldValue != null)
	            oldValue.CanExecuteChanged -= CommandOnCanExecuteChanged;
	    }

	    private void CommandPropertyChanged(ICommand oldvalue, ICommand newValue)
	    {
	        if (newValue != null)
	        {
	            newValue.CanExecuteChanged += CommandOnCanExecuteChanged;
	            IsEnabled = newValue.CanExecute(null);
	        }
	    }

	    private void CommandOnCanExecuteChanged(object sender, EventArgs e)
	    {
	        var cmd = (ICommand)sender;
	        if (cmd != null)
	        {
	            IsEnabled = cmd.CanExecute(null);
            }
	    }

	    private void IsEnabledPropertyChanged(bool oldValue, bool newValue)
	    {
	        Image.Transformations = newValue ? NoTransformation : DisabledTransformation;
	        base.IsEnabled = newValue;
	    }

        private static readonly List<ITransformation> NoTransformation;
	    private static readonly List<ITransformation> DisabledTransformation;
	    private static readonly List<ITransformation> ClickedTransformation;

        static ImageButton()
	    {
            DisabledTransformation = new List<ITransformation>{ new ColorSpaceTransformation(FFColorMatrix.GrayscaleColorMatrix) };
            ClickedTransformation = new List<ITransformation> { new ColorSpaceTransformation(FFColorMatrix.PolaroidColorMatrix) };
            NoTransformation = new List<ITransformation>();
	    }

	    private Task TransitionDownAsync()
	    {
	        if (Image == null) return Task.CompletedTask;
	        Image.Transformations = ClickedTransformation;
	        return Frame.ScaleTo(0.9, 50, Easing.Linear);
	    }

	    private Task TransitionUpAsync()
	    {
	        if (Image == null) return Task.CompletedTask;
	        Image.Transformations = NoTransformation;
	        return Frame.ScaleTo(1, 50, Easing.Linear);
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
	        await TransitionDownAsync();
	        Pressed?.Invoke(this, EventArgs.Empty);
	    }

	    private async void ClickableFrame_Released(object sender, EventArgs e)
	    {
	        await TransitionUpAsync();
	        Released?.Invoke(this, EventArgs.Empty);
	    }
    }
}