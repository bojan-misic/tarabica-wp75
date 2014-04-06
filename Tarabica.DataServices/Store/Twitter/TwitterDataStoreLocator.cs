using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarabica.DataServices.Http.Twitter;

namespace Tarabica.DataServices.Store.Twitter
{
    public class TwitterDataStoreLocator : ITwitterDataStoreLocator
    {
        ITwitterDataStore twitterDataStore;

        public TwitterDataStoreLocator(ITwitterDataService twitterService)
        {
            twitterDataStore = new TwitterDataStore(twitterService);
        }

        public ITwitterDataStore GetStore()
        {
            return twitterDataStore;
        }
    }
}
