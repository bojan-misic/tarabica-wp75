using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Dto.Twitter
{
    public class UrlDto
    {
        public string Url { get; set; }

        public override string ToString()
        {
            return String.Format("{0}", Url);
        }
    }
}
