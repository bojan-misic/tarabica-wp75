using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Domain.Conference
{
    public class Day
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Id, Date);
        }

        public string DayName
        {
            get
            {
                return LocalizedDateHelper.GetDayNameForCulture(Date, "sr-Latn-CS");
            }
        }

        public string FullDateString
        {
            get
            {
                return LocalizedDateHelper.GetFullDateForCulture(Date, "sr-Latn-CS");
            }
        }
    }
}
