using Microsoft.Phone.Controls;
using Tarabica.WP7App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Navigation;

namespace Tarabica.WP7App.Infrastructure
{
    public static class PhoneApplicationPageExtensions
    {
        public static void HandleOnNavigatedTo(this PhoneApplicationPage page, NavigationEventArgs e)
        {
            var vm = page.DataContext as ViewModel;
            if (vm == null) return;
            vm.HandleOnNavigatedTo(e);
        }
    }
}
