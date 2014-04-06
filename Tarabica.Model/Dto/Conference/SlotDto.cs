using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Dto.Conference
{
    public class SlotDto
    {
        public int Id { get; set; }
        public int TimeLine { get; set; }
        public int DayId { get; set; }
        public string Type { get; set; }
        public byte StartHour { get; set; }
        public byte StartMinute { get; set; }
        public byte EndHour { get; set; }
        public byte EndMinute { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] [{1}] {2:00}:{3:00}-{4:00}:{5:00}",
                Id, DayId, StartHour, StartMinute, EndHour, EndMinute);
        }
    }
}
