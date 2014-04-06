using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Domain.Conference
{
    public class Slot
    {
        public int Id { get; set; }
        public int TimeLine { get; set; }
        public string Type { get; set; }
        public byte StartHour { get; set; }
        public byte StartMinute { get; set; }
        public byte EndHour { get; set; }
        public byte EndMinute { get; set; }

        public Day Day { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string ToTimeSpanString()
        {
            return Day.Id == -1 ? String.Empty : string.Format("{0:00}:{1:00} - {2:00}:{3:00}", StartHour, StartMinute, EndHour, EndMinute);
        }

        public override string ToString()
        {
            return string.Format("[{0}] [{1}] {2:00}:{3:00}-{4:00}:{5:00}",
                Id, Day.Id, StartHour, StartMinute, EndHour, EndMinute);
        }
    }
}
