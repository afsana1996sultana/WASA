using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WASA.Models
{
    public class WasaReportModelStatic : Controller
    {
        public DateTime Date { get; set; }
        public string PumpBrand { get; set; }
        public string VFDBrand { get; set; }
        public string TXBrand { get; set; }
        public decimal PumpSerial { get; set; }
    }
}
