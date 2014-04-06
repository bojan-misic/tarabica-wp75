using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Dto.Conference
{
    public class ConferenceDataModelDto
    {
        public int Version { get; set; }
        public List<DayDto> Days { get; set; }
        public List<RoomDto> Rooms { get; set; }
        public List<SpeakerDto> Speakers { get; set; }
        public List<SessionDto> Sessions { get; set; }
        public List<SessionSpeakerRelationDto> SessionSpeakerRelations { get; set; }
        public List<SlotDto> Slots { get; set; }
        public List<TrackDto> Tracks { get; set; }
        //public List<EventDto> Events { get; set; }
        //public string Test { get; set; }
        public string PicturesLocation { get; set; }
    }
}
