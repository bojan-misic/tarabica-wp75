using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Dto.Conference
{
    public class SessionDto
    {
        public int Id { get; set; }
        public string Track { get; set; }
        public int DayId { get; set; }
        public int TimeLine { get; set; }
        public string Lang { get; set; }
        public int Level { get; set; }
        public int RoomId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] [{1}] [{2}] {3}", Id, DayId, RoomId, Title);
        }
    }
}
