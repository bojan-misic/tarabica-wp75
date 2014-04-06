using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Dto.Conference
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Id, Code);
        }
    }
}
