using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Dto.Twitter
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Screen_name { get; set; }
        public string Profile_image_url_https { get; set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Name, Screen_name);
        }
    }
}
