﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using Tarabica.WP7App.Infrastructure;

namespace Tarabica.WP7App.Views
{
    public partial class SessionListView : PhoneApplicationPage
    {
        public SessionListView()
        {
            InitializeComponent();
        }

        // ADDED:
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.HandleOnNavigatedTo(e);
        }
    }
}