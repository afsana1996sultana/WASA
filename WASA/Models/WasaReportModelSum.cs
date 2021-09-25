using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WASA.Models
{
    public class WasaReportModelSum
    {

        public long Id { get; set; }
        public decimal Production { get; set; }
        public decimal Runtime { get; set; }
        public decimal KWH { get; set; }
        public decimal AVGFlow { get; set; }
        public decimal DownTime { get; set; }
    }
}
