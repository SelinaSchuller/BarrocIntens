using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrocIntens.Data
{
    internal class CalenderInfo
    {
        public int id { get; set; }
        public DayOfWeek Day { get; set; }
        public Appointment Appointment { get; set; }
        public int Row { get; set; }
        public DateTime Date { get; set; }
    }
}
