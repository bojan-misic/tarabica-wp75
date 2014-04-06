//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================

using System;
using System.Windows;
using Microsoft.Phone.Shell;
using Tarabica.WP7App.Services;
using Microsoft.Practices.Prism.ViewModel;
using System.Runtime.Serialization;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace Tarabica.WP7App.ViewModels
{
    [DataContract]
    public abstract class ViewModel : NotificationObject, IDisposable
    {
        private readonly INavigationService navigationService;
        private bool disposed;

        protected ViewModel(INavigationService navigationService)
        {
            PhoneApplicationService.Current.Deactivated += this.OnDeactivated;
            PhoneApplicationService.Current.Activated += this.OnActivated;

            (Application.Current as App).RootFrame.Obscured += OnObscured;
            (Application.Current as App).RootFrame.Unobscured += OnUnobscured;

            this.navigationService = navigationService;
        }

        ~ViewModel()
        {
            this.Dispose();
        }

        public INavigationService NavigationService
        {
            get { return this.navigationService; }
        }

        public virtual void IsBeingDeactivated()
        {
        }

        public abstract void IsBeingActivated();

        public virtual void IsBeingUnobscured()
        {
        }

        public virtual void IsBeingObscured(bool isLocked)
        {
        }

        public virtual void HandleOnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                PhoneApplicationService.Current.Deactivated -= this.OnDeactivated;
                PhoneApplicationService.Current.Activated -= this.OnActivated;
            }

            this.disposed = true;
        }

        private void OnDeactivated(object s, DeactivatedEventArgs e)
        {
            this.IsBeingDeactivated();
        }

        private void OnActivated(object s, ActivatedEventArgs e)
        {
            this.IsBeingActivated();
        }

        private void OnObscured(object sender, ObscuredEventArgs e)
        {
            this.IsBeingObscured(e.IsLocked);
        }

        private void OnUnobscured(object sender, EventArgs e)
        {
            this.IsBeingUnobscured();
        }


    }
}
