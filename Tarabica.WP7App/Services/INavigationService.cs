//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================

using System;

namespace Tarabica.WP7App.Services
{
    public interface INavigationService
    {
        bool CanGoBack { get; }
        Uri CurrentSource { get; }
        bool Navigate(Uri source);
        void GoBack();
    }
}