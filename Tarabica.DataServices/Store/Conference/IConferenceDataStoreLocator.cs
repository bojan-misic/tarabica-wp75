﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.DataServices.Store.Conference
{
    public interface IConferenceDataStoreLocator
    {
        IConferenceDataStore GetStore();
    }
}
