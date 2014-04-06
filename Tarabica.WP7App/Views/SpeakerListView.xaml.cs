using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Tarabica.WP7App.Infrastructure;

namespace Tarabica.WP7App.Views
{
    public partial class SpeakerListView : PhoneApplicationPage
    {
        public SpeakerListView()
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