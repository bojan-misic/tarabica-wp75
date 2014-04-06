using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.DataServices.Store.App
{
    public interface IAppSettingsStoreLocator
    {
        IAppSettingsStore GetStore();
    }
}
