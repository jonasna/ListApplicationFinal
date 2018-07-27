using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace DialogServices
{
    public abstract class PopupBase<T> : PopupPage, INotifyResultSet
    {
        protected PopupBase()
        {
            BindingContext = this;          
        }

        #region Animations

        protected sealed override async void OnAppearingAnimationEnd()
        {
            await Content.FadeTo(1);
        }

        protected sealed override async void OnDisappearingAnimationBegin()
        {
            await Content.FadeTo(1);
        }

        #endregion

        #region PropertyChanged

        protected virtual bool SetProperty<TProp>(ref TProp field, TProp value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TProp>.Default.Equals(field, value))
                return false;

            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected virtual bool SetProperty<TProp>(ref TProp field, TProp value, Action onSet, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TProp>.Default.Equals(field, value))
                return false;

            field = value;
            onSet?.Invoke();
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        #endregion

        #region Completion

        public event EventHandler ResultSetEvent;

        public TaskCompletionSource<T> Completion { get; set; }

        protected void SetResult(T result)
        {
            Completion.SetResult(result);
            ResultSetEvent?.Invoke(this, null);
        }

        #endregion

        #region Back Buttons

        protected sealed override bool OnBackButtonPressed()
        {
            ConfigureBackButtonClicked();
            return true;

        }

        protected sealed override bool OnBackgroundClicked()
        {
            ConfigureBackgroundClicked();
            return false;
        }

        protected abstract void ConfigureBackButtonClicked();

        protected abstract void ConfigureBackgroundClicked();

        #endregion

        #region Initiation and loading

        protected sealed override void OnAppearing()
        {
            base.OnAppearing();
            if (Completion == null)
                throw new Exception("Completion is null");
            OnInitiated();
        }

        protected abstract void OnInitiated();

        #endregion

        protected static TType GetApplicationResource<TType>(string key)
        {
            return (TType)Application.Current.Resources[key];
        }

        protected static Color GetApplicationResourceColor(string key)
        {
            return GetApplicationResource<Color>(key);
        }
    }
}