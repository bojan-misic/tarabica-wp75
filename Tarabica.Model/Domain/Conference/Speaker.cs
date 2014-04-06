using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Domain.Conference
{
    public class Speaker
    {
        public Speaker()
        {
            Sessions = new List<Session>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string PictureUrl { get; set; }
        public string Company { get; set; }

        public List<Session> Sessions { get; set; }
        public List<Track> Tracks { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1} {2}", Id, FirstName, LastName);
        }
    }
}
