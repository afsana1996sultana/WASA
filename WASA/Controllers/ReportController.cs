using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;

namespace WASA.Controllers
{
    public class ReportController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return View();
        } 

        [HttpGet]
        public IActionResult GetWasaReport()
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=demo_databaseDB;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();

            }
            catch (Exception ex )
            {

                throw ex;
            }

            cnn.Close();

            return View();
        }
       
    }
}
