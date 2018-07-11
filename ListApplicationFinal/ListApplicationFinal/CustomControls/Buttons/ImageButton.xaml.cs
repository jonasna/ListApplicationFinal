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

	    public static readonly BindableProperty IsDisabledProperty = BindableProperty.Create(nameof(IsDisabled), typeof(bool), typeof(ImageButton), false, propertyChanged: OnDisabledChanged);

	    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ImageButton), null);

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

	    public bool IsDisabled
	    {
	        get => (bool)GetValue(IsDisabledProperty);
	        set => SetValue(IsDisabledProperty, value);
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

	    private static void OnDisabledChanged(BindableObject bindable, object oldValue, object newValue)
	    {
	        var btn = (bindable as ImageButton);

	        try
	        {
	            var img = btn.Image;
	            img.Transformations = new List<ITransformation> { };
	            if (btn.IsDisabled)
	            {
	                img.Transformations = new List<ITransformation> { new ColorSpaceTransformation(FFColorMatrix.GrayscaleColorMatrix) };
	            }
	        }
	        catch (System.Exception ex)
	        {
	            System.Diagnostics.Debug.WriteLine(ex);
	        }
	    }

	    private void TransitionDown()
	    {
	        try
	        {
	            Image.Transformations = new List<ITransformation> { new ColorSpaceTransformation(FFColorMatrix.PolaroidColorMatrix) };
	        }
	        catch
	        {
	            // nothing to do
	        }
        }
        private void TransitionUp()
        {
            try
            {
                Image.Transformations = new List<ITransformation> { };
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
            if (IsDisabled) return;
            Clicked?.Invoke(this, EventArgs.Empty);
            Command?.Execute(CommandParameter);
        }

        private void ClickableFrame_Pressed(object sender, EventArgs e)
        {
            if (IsDisabled) return;
            TransitionDown();
            Pressed?.Invoke(this, EventArgs.Empty);
        }

        private void ClickableFrame_Released(object sender, EventArgs e)
        {
            if (IsDisabled) return;
            TransitionUp();
            Released?.Invoke(this, EventArgs.Empty);
        }

    }
}