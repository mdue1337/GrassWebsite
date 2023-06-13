using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassDatabase
{
    public class JobDetails
    {
        public int JobId { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public double JobTime { get; set; }
    }
}
