using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Domain.Twitter
{
    public class Tweet
    {
        public Int64 Id { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public string Since { get; set; }
        public List<string> Urls { get; set; }

        public override string ToString()
        {
            return String.Format("[{0}] @{1}: {2} ", LocalizedDateHelper.GetDateTimeForCulture(Time, "sr-Latn-CS"), User.ScreenName, Text);
        }
    }
}
