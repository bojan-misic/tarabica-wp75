using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Dto.Conference
{
    public class SessionSpeakerRelationDto
    {
        public int SpeakerId { get; set; }
        public int SessionId { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] : [{1}]", SpeakerId, SessionId);
        }
    }
}
