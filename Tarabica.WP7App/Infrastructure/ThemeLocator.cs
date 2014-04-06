using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Tarabica.WP7App.Infrastructure
{
    public class ThemeLocator
    {
        public static bool LightThemeEnabled
        {
            get
            {
                return (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == Visibility.Visible;
            }
        }

        public static bool DarkThemeEnabled
        {
            get
            {
                return (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible;
            }
        }
    }
}
