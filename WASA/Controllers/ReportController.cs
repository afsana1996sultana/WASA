using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WASA.Models;

namespace WASA.Controllers
{
    public class ReportController : Controller
    {

        //private string id;
        //private object lr;

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

        //    string reportType = id;
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;

        //    string deviceInfo =
        //        "<DeviceInfo>" +
        //        "<OutputFormat>" + id + "</OutputFormat>" +
        //        "<PageWidth> 8.15in</PageWidth>" +
        //        "<PageHeight>11in</PageHeight>" +
        //        "<MarginTop>0.5in</MarginTop>" +
        //        "<MarginLeft>1in</MarginLeft>" +
        //        "<MarginRight>1in</MarginRight>" +
        //        "<MarginBottom>0.5in</MarginBottom>" +
        //        "</DeviceInfo>";
        //    WasaReportModel[] wasa;
        //    string[] streams;
        //    byte[] renderedBytes;
        //    renderedBytes = lr.Render(
        //        formet: reportType,
        //        deviceInfo,
        //        out mimeType,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out wasa);//byte[]
        //    return File(renderedBytes, mimeType);

        //}   
                

        

    

           
            

           
      
