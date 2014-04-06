//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace Tarabica.DataServices.Tasks
{
    public enum TaskSummaryResult
    {
        TwitterAuthorizationSuccess,
        TwitterAuthorizationFailure,
        NewVersionAvailable,
        AlreadyHaveTheCurrentVersion,
        Success,
        AccessDenied,
        UnreachableServer,
        UnknownError
    }
}