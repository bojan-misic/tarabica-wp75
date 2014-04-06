using System;
//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================

using Tarabica.WP7App.Infrastructure;

namespace Tarabica.WP7App.ViewModels
{
    public class ViewModelLocator : IDisposable
    {
        private readonly ContainerLocator containerLocator;
        private bool disposed;

        public ViewModelLocator()
        {
            this.containerLocator = new ContainerLocator();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<MainViewModel>();
            }
        }

        public SessionListViewModel SessionListViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<SessionListViewModel>();
            }
        }

        public SessionDetailsViewModel SessionDetailsViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<SessionDetailsViewModel>();
            }
        }

        public SpeakerDetailsViewModel SpeakerDetailsViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<SpeakerDetailsViewModel>();
            }
        }

        public ConferenceInfoViewModel ConferenceInfoViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<ConferenceInfoViewModel>();
            }
        }

        public SpeakerListViewModel SpeakerListViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<SpeakerListViewModel>();
            }
        }

        //public EventInfoViewModel EventInfoViewModel
        //{
        //    get
        //    {
        //        return this.containerLocator.Container.Resolve<EventInfoViewModel>();
        //    }
        //}

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
                this.containerLocator.Dispose();
            }

            this.disposed = true;
        }
    }
}
