using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.DataServices.Store.Twitter
{
    public interface ITwitterDataStoreLocator
    {
        ITwitterDataStore GetStore();
    }
}
