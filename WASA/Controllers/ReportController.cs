using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WASA.Models;

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
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=demo_databaseDB;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from tbReports", cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModel p = new WasaReportModel();
                    p.Serial = Convert.ToInt64(rd[0]);

                    p.Date = Convert.ToDateTime(rd[1]).Date;
                    rportlist.Add(p);

                    p.Flow = Convert.ToInt64(rd[2]);

                    p.Level = Convert.ToInt64(rd[3]);

                    p.Runtime = Convert.ToInt64(rd[4]);

                    p.Stoptime = Convert.ToInt64(rd[5]);

                    p.KWH = Convert.ToInt64(rd[6]);

                    p.Production = Convert.ToInt64(rd[7]);

                    p.ProductionCum = Convert.ToInt64(rd[8]);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                cnn.Close();
            }

            ViewData["WasaReport"] = rportlist;

            return View();

        }

    }
}
           
            

           
      
