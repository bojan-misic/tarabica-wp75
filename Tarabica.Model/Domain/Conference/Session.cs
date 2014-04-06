using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Domain.Conference
{
    public class Session
    {
        public Session()
        {
            Speakers = new List<Speaker>();
        }

        public int Id { get; set; }
        public string LanguageCode { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFavourite { get; set; }

        public List<Speaker> Speakers { get; set; }
        public Track Track { get; set; }
        public Slot Slot { get; set; }
        public Room Room { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] [{1}] [{2}] {3}", Id, Slot.Day.Id, Room.Id, Title);
        }
    }
}
