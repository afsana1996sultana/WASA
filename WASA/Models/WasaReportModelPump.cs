using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WASA.Models
{
    public class WasaReportModelPump : Controller
    {
        public DateTime Date { get; set; }
        public decimal Production { get; set; }
        public decimal Runtime { get; set; }
        public decimal KWH { get; set; }
        public decimal Flow { get; set; }
        public decimal Stoptime { get; set; }





    }
}
