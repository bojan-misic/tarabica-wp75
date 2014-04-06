using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Tarabica.Model.Domain.Conference
{
    public class Track
    {
        public int Id { get; set; }
        public string Abbeveration { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        //public Color Color { get; set; }
        //public Color BackgroundColor { get; set; }
        public string PictureUrl { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Id, Title);
        }

        public bool HasThreeChars
        {
            get { return Abbeveration.Length == 3; }
        }
    }
}
