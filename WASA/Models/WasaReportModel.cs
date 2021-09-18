using System;

namespace WASA.Models
{
    public class WasaReportModel
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Production { get; set; }
        public decimal Runtime { get; set; }
        public decimal KWH { get; set; }
        public decimal Flow { get; set; }
        public decimal Stoptime { get; set; }
        public string ProductionCum { get; set; }

        
    }
}
