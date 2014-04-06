//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================

using Tarabica.DataServices.Http.Twitter;
namespace Tarabica.DataServices.Store.App
{
    /// <summary>
    /// Interface for implementing the persistent Application Settings.
    /// </summary>
    public interface IAppSettingsStore
    {
        TweetType TweetType { get; set; }
    }
}