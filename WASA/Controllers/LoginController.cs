using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WASA.Models;

namespace WASA.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(UserModel userModel)
        {
            if (userModel.UserName == "Afsana" && userModel.password == "1234")
            {
                return View("Dashboard", userModel);
            } else
            {
                return View("LoginFailure", userModel);
            }
           
        }

    }
}
