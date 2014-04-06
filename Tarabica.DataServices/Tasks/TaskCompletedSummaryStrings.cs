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
    using System;

    public static class TaskCompletedSummaryStrings
    {
        public static string GetDescriptionForResult(TaskSummaryResult result)
        {
            switch (result)
            {
                case TaskSummaryResult.TwitterAuthorizationSuccess:
                    return "Twitter autorizacija uspela.";
                case TaskSummaryResult.TwitterAuthorizationFailure:
                    return "Twitter autorizacija nije uspela.";
                case TaskSummaryResult.AlreadyHaveTheCurrentVersion:
                    return "Ne postoji novija verzija na serveru.";
                case TaskSummaryResult.NewVersionAvailable:
                    return "Postoji novija verzija na serveru.";
                case TaskSummaryResult.Success:
                    return "Operacija je uspešno završena.";
                case TaskSummaryResult.AccessDenied:
                    return "Pristup servisu nije dozvoljen.";
                case TaskSummaryResult.UnreachableServer:
                    return "Server trenutno nije dostupan. Molimo Vas da kasnije pokušate ponovo.";
                case TaskSummaryResult.UnknownError:
                    return "Došlo je do greške. Molimo Vas da kasnije pokušate ponovo.";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}