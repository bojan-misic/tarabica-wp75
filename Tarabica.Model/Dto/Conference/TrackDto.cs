using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Dto.Conference
{
    public class TrackDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Abbeveration { get; set; }
        public string PicturePath { get; set; }

        public override string ToString()
        {
 	        return String.Format("[{0}]{1}", Id, Abbeveration);
        }
    }
}
