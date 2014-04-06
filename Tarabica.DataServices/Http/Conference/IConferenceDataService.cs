using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarabica.Model.Dto.Conference;

namespace Tarabica.DataServices.Http.Conference
{
    /// <summary>
    /// Interface for providing conference data to the application.
    /// </summary>
    public interface IConferenceDataService
    {
        IObservable<ConferenceDataModelDto> GetConferenceData();
        IObservable<int> GetVersion();
    }
}
