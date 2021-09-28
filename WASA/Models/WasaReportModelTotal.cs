using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WASA.Models
{
    public class WasaReportModelTotal : Controller
    {
        public decimal Production { get; set; }
        public decimal Runtime { get; set; }
        public decimal KWH { get; set; }
        public decimal AVGFlow { get; set; }
        public decimal DownTime { get; set; }
    }
}
