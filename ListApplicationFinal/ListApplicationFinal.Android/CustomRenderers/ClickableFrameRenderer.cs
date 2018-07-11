using System;
using Android.Content;
using Android.Views;
using ListApplicationFinal.CustomControls;
using ListApplicationFinal.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FrameRenderer = Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(ClickableFrame), typeof(ClickableFrameRenderer))]
namespace ListApplicationFinal.Droid.CustomRenderers
{
    public class ClickableFrameRenderer : FrameRenderer
    {

        private bool _isDisposed;

        public ClickableFrameRenderer(Context context) : base(context)
        {
        }

        public static void Init()
        {

        }

        public Frame PElement => Element;

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    Control.SetOnClickListener(FrameOnClickListener.Instance.Value);
                    Control.SetOnTouchListener(FrameOnTouchListener.Instance.Value);
                    Control.Tag = this;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
            {
                if (Control != null)
                {
                    Control.SetOnClickListener(null);
                    Control.SetOnTouchListener(null);
                    Control.Tag = null;
                }
            }

            base.Dispose(disposing);
        }
    }

    internal class FrameOnClickListener : Java.Lang.Object, View.IOnClickListener
    {
        private FrameOnClickListener()
        { }

        public static readonly Lazy<FrameOnClickListener> Instance =
            new Lazy<FrameOnClickListener>(() => new FrameOnClickListener());

        public void OnClick(View v)
        {
            IButtonController frameBtn = null;

            if (v.Tag is ClickableFrameRenderer frame)
            {
                frameBtn = frame.PElement as IButtonController;
            }

            frameBtn?.SendClicked();
        }
    }

    internal class FrameOnTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        private FrameOnTouchListener() { }

        public static readonly Lazy<FrameOnTouchListener> Instance = new Lazy<FrameOnTouchListener>(() => new FrameOnTouchListener());

        public bool OnTouch(global::Android.Views.View v, MotionEvent e)
        {
            IButtonController frameBtn = null;

            if (v.Tag is ClickableFrameRenderer frame)
            {
                frameBtn = frame.PElement as IButtonController;
            }

            if (e.Action == MotionEventActions.Down)
            {
                frameBtn?.SendPressed();
            }
            else if (e.Action == MotionEventActions.Up)
            {
                frameBtn?.SendReleased();
            }

            return false;
        }
    }
}