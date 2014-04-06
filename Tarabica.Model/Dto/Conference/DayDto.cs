using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarabica.Model.Dto.Conference
{
    public class DayDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Id, Date);
        }
    }
}
