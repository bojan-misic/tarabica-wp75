using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Dto.Conference
{
    public class SpeakerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string PictureUrl { get; set; }
        public string Company { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1} {2}", Id, FirstName, LastName);
        }
    }
}
