using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WASA.Controllers
{
    public class ReportController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return View();
        }
    }
}
