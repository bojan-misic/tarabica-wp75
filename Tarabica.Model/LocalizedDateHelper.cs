using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Tarabica.Model
{
    public class LocalizedDateHelper
    {
        public static string GetDayNameForCulture(DateTime date, string cultureSpecifier)
        {
            string format = "dddd";
            CultureInfo culture = new CultureInfo(cultureSpecifier);
            return date.ToString(format, culture);
        }

        public static string GetFullDateForCulture(DateTime date, string cultureSpecifier)
        {
            string format = "D";
            CultureInfo culture = new CultureInfo(cultureSpecifier);
            return date.ToString(format, culture);
        }

        public static string GetDateTimeForCulture(DateTime date, string cultureSpecifier)
        {
            string format = "g";
            CultureInfo culture = new CultureInfo(cultureSpecifier);
            return date.ToString(format, culture);
        }

        public static string GetShortTimePatternForCulture(DateTime date, string cultureSpecifier)
        {
            string format = "t";
            CultureInfo culture = new CultureInfo(cultureSpecifier);
            return date.ToString(format, culture);
        }

        public static string GetPrettyDateSince(DateTime d)
        {
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(d);

            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return null;
            }

            // Handle same-day times.
            if (dayDiff == 0)
            {
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "malopre";
                }
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("pre {0}min",
                        Math.Floor((double)secDiff / 60));
                }
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("pre {0}h",
                        Math.Floor((double)secDiff / 3600));
                }
            }

            // Handle previous days.
            if (dayDiff == 1)
            {
                return "juče";
            }
            if (dayDiff < 14)
            {
                return string.Format("pre {0} dana",
                dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format("pre {0} nedelje",
                Math.Ceiling((double)dayDiff / 7));
            }

            return "pre više od mesec dana";
        }
    }
}
