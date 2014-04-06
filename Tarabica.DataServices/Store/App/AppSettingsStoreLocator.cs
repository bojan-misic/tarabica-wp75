using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.DataServices.Store.App
{
    public class AppSettingsStoreLocator : IAppSettingsStoreLocator
    {
        private IAppSettingsStore appSettingsStore;

        public AppSettingsStoreLocator()
        {
            appSettingsStore = new AppSettingsStore();
        }

        public IAppSettingsStore GetStore()
        {
            return appSettingsStore;
        }
    }
}
