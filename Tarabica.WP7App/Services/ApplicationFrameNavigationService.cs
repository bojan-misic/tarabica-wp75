//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================

using System;
using Microsoft.Phone.Controls;

namespace Tarabica.WP7App.Services
{
    public class ApplicationFrameNavigationService : INavigationService
    {
        private readonly PhoneApplicationFrame frame;

        public ApplicationFrameNavigationService(PhoneApplicationFrame frame)
        {
            this.frame = frame;
        }

        public bool CanGoBack
        {
            get { return this.frame.CanGoBack; }
        }

        public Uri CurrentSource
        {
            get { return this.frame.CurrentSource; }
        }

        public bool Navigate(Uri source)
        {
            return this.frame.Navigate(source);
        }

        public void GoBack()
        {
            this.frame.GoBack();
        }
    }
}