using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Domain.Twitter
{
    public class User
    {
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string ImageUrl { get; set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Name, ScreenName);
        }
    }
}
