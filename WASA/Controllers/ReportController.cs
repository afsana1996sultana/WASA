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
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=Wasa_Dev_Db;Trusted_Connection=True;MultipleActiveResultSets=true";
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
