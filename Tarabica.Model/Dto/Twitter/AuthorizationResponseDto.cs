using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Dto.Twitter
{
    public class AuthorizationResponseDto
    {
        public string Token_type { get; set; }
        public string Access_token { get; set; }

        public override string ToString()
        {
            return String.Format("{0}: {1}", Token_type, Access_token);
        }
    }
}
