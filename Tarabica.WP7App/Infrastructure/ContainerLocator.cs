//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================

//===============================================================================
// Using IoC Container from Funq library for leveraging Dependency Injection (DI).
//
// DI: A type of Inversion of Control (IoC) where we move the creation and
// binding of a dependency outside of the class that depends on it. Dependency is 
// usually an Interface which has different implementations; different implementations
// can be injected in dependent class at design-time or run-time (for example: mock 
// data during testing or real data in production).
//
// This is where dependencies are configured (interfaces are associated with the 
// concrete implementations).
//===============================================================================

using System;
using Funq;
using Tarabica.WP7App.ViewModels;
using Tarabica.DataServices;
using Tarabica.DataServices.Http.Twitter;
using Tarabica.DataServices.Http.Conference;
using Tarabica.DataServices.Store.Conference;
using Tarabica.DataServices.Store.Twitter;
using Tarabica.WP7App.Services;
using System.Windows;
using Tarabica.DataServices.Store.App;

namespace Tarabica.WP7App.Infrastructure
{
    public class ContainerLocator : IDisposable
    {
        private bool disposed;

        public Container Container { get; private set; }

        public ContainerLocator()
        {
            this.Container = new Container();
            this.ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            this.Container.Register<IConferenceDataService>(c => new ConferenceDataService());
            this.Container.Register<ITwitterDataService>(c => new TwitterDataService());
            this.Container.Register<IConferenceDataStoreLocator>(c => new ConferenceDataStoreLocator(c.Resolve<IConferenceDataService>()));
            this.Container.Register<ITwitterDataStoreLocator>(c => new TwitterDataStoreLocator(c.Resolve<ITwitterDataService>()));
            this.Container.Register<IAppSettingsStoreLocator>(c => new AppSettingsStoreLocator());

            this.Container.Register<INavigationService>(_ =>
                new ApplicationFrameNavigationService(((App)Application.Current).RootFrame));

            this.Container.Register(
                c => new MainViewModel(
                    c.Resolve<IConferenceDataStoreLocator>(),
                    c.Resolve<ITwitterDataStoreLocator>(),
                    c.Resolve<IAppSettingsStoreLocator>(),
                    c.Resolve<INavigationService>()))
                .ReusedWithin(ReuseScope.None);

            this.Container.Register(
                c => new SessionListViewModel(
                    c.Resolve<IConferenceDataStoreLocator>(),
                    c.Resolve<INavigationService>()))
                .ReusedWithin(ReuseScope.None);

            this.Container.Register(
                c => new SessionDetailsViewModel(
                    c.Resolve<IConferenceDataStoreLocator>(),
                    c.Resolve<INavigationService>()))
                .ReusedWithin(ReuseScope.None);

            this.Container.Register(
                c => new SpeakerListViewModel(
                    c.Resolve<IConferenceDataStoreLocator>(),
                    c.Resolve<INavigationService>()))
                .ReusedWithin(ReuseScope.None);

            this.Container.Register(
                c => new SpeakerDetailsViewModel(
                    c.Resolve<IConferenceDataStoreLocator>(),
                    c.Resolve<INavigationService>()))
                .ReusedWithin(ReuseScope.None);

            this.Container.Register(
                c => new ConferenceInfoViewModel(
                    c.Resolve<INavigationService>()))
                .ReusedWithin(ReuseScope.None);
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
                this.Container.Dispose();
            }

            this.disposed = true;
        }
    }
}
