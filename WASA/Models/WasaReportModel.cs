using System;

namespace WASA.Models
{
    public class WasaReportModel
    {
        public long Serial { get; set;}
        public DateTime Date { get; set;}
        public long Flow { get; set; }
        public long Level { get; set; }
        public long Runtime { get; set; }
        public long Stoptime { get; set; }
        public long KWH { get; set; }
        public long Production { get; set; }
        public long ProductionCum { get; set; }
        
    }
}
