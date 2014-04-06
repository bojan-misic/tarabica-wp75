using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Dto.Twitter
{
    public class TweetDto
    {
        public Int64 Id { get; set; }
        public string Created_at { get; set; }
        public string Text { get; set; }
        public UserDto User { get; set; }
        public EntitiesDto Entities { get; set; }

        public override string ToString()
        {
            return String.Format("[{0}] {1}: {2}", Created_at, User.Screen_name, Text);
        }
    }
}
