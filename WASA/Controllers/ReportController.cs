using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Web;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Data;
using System.Configuration;
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
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                 //string sqlquary = "select * from [dbo].[tbReports] where Date between'" + from+ "'and'" + to + "'";
                //string sqlquary = $"SELECT * FROM[demo_databaseDB].[dbo].[tbReports] where Date between '{from}' and '{to}'";
                //SqlCommand sqlcomm = new SqlCommand(sqlquary, cnn);
                //cnn.Open();
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from AZAMPUR", cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModel p = new WasaReportModel();
                    p.Id = Convert.ToInt64(rd[0]);

                    p.Date = Convert.ToDateTime(rd[1]).Date;
                    rportlist.Add(p);

                    p.Production = Convert.ToInt64(rd[2]);

                    p.Runtime = Convert.ToInt64(rd[3]);

                    p.KWH = Convert.ToInt64(rd[4]);

                    p.Flow = Convert.ToInt64(rd[5]);

                    p.Stoptime = Convert.ToInt64(rd[6]);

                    p.ProductionCum = Convert.ToInt64(rd[7]);


                }
            }
            catch (Exception)
            {
                
                //throw ex;
            }
            finally
            {
                cnn.Close();
            }

            ViewData["WasaReport"] = rportlist;

            return View();

      

        }
        [HttpGet]
        //public IActionResult GetWasaReportFilter(DateTime? from, DateTime? to)
        //{


        //    List<WasaReportModel> rportlist = new List<WasaReportModel>();
        //    string connetionString;
        //    SqlConnection cnn;
        //    connetionString = @"Server=localhost;Database=demo_databaseDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        //    cnn = new SqlConnection(connetionString);
        //    try
        //    {
        //         string sqlquary = "select * from [dbo].[tbReports] where Date between'" + from.Value.Date+ "'and'" + to.Value.Date + "'";
        //        cnn.Open();
        //        SqlCommand cmd = new SqlCommand(sqlquary, cnn);

        //        SqlDataReader rd = cmd.ExecuteReader();

        //        while (rd.Read())

        //        {

        //            WasaReportModel p = new WasaReportModel();
        //            p.Id = Convert.ToInt64(rd[0]);

        //            p.Date = Convert.ToDateTime(rd[1]).Date;
        //            rportlist.Add(p);

        //            p.Production = Convert.ToInt64(rd[2]);

        //            p.Runtime = Convert.ToInt64(rd[3]);

        //            p.KWH = Convert.ToInt64(rd[4]);

        //            p.Flow = Convert.ToInt64(rd[5]);

        //            p.Stoptime = Convert.ToInt64(rd[6]);


        //        }
        //    }
        //    catch (Exception ex)
        //    {
                
        //        //throw ex;
        //    }
        //    finally
        //    {
        //        cnn.Close();
        //    }

        //    ViewData["WasaReport"] = rportlist;

        //    return View("GetWasaReport");



        //}


        [HttpGet]
        public ActionResult PrintInvoice()
        {
            using (PdfDocument document = new PdfDocument())
            {
                //Add a page to the document
                PdfPage page = document.Pages.Add();

                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;

                //Set the standard font
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                //Draw the text
                graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));

                // Open the document in browser after saving it

                return View();
            }

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
                

        

    

           
            

           
      
