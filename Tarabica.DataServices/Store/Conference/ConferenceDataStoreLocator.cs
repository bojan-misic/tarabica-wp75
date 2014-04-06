using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarabica.DataServices.Http.Conference;

namespace Tarabica.DataServices.Store.Conference
{
    public class ConferenceDataStoreLocator : IConferenceDataStoreLocator
    {
        private IConferenceDataStore conferenceDataStore;

        public ConferenceDataStoreLocator(IConferenceDataService conferenceDataService)
        {
            conferenceDataStore = new ConferenceDataStore(conferenceDataService);
        }

        public IConferenceDataStore GetStore()
        {
            return conferenceDataStore;
        }
    }
}
