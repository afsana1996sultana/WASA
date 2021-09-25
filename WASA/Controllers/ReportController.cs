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
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from AZAMPUR where ID < 101", cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {

                    WasaReportModel p = new WasaReportModel();

                    p.Id = Convert.ToInt64(rd["ID"]);

                    p.Date = Convert.ToDateTime(rd["Date"]);

                    p.Production = Convert.ToDecimal(rd["Production"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.Flow = Convert.ToDecimal(rd["Flow"]);

                    p.Stoptime = rd["Stoptime"] == DBNull.Value ? 00 : Convert.ToInt64(rd["Stoptime"]);

                    rportlist.Add(p);



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
            ViewData["WasaReportSum"] = null;
            return View();



        }

        [HttpGet]
        public IActionResult GetWasaReportFilter(DateTime? from, DateTime? to)
        {


            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                //string sqlquary = "select * from [dbo].[tbReports] where Date between'" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "'";
                string sqlquary = "SELECT ID, Date" +
                                         ", round(Production, 1) as 'Production(M3)'" +
                                        " ,round(Runtime, 1) as Runtime" +
                                        " ,round(KWH, 1) as KWH" +
                                        " ,round(Flow, 1) as Flow" +
                                        " , round(24 - Runtime, 1) as Downtime" +
                                        " FROM(SELECT *, ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE,[Date]) " +
                                        " ORDER BY DATE DESC) AS rn " +
                                         " FROM AZAMPUR) t WHERE t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "' and Runtime != 0";
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModel p = new WasaReportModel();

                    p.Id = Convert.ToInt64(rd["ID"]);

                    p.Date = Convert.ToDateTime(rd["Date"]);

                    p.Production = Convert.ToDecimal(rd["Production(M3)"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.Flow = Convert.ToDecimal(rd["Flow"]);

                    p.Stoptime = rd["Downtime"] == DBNull.Value ? 00 : Convert.ToInt64(rd["Downtime"]);

                    rportlist.Add(p);


                }
            }
            catch (Exception ex)
            {

                //throw ex;
            }
            finally
            {
                cnn.Close();
            }

            ViewData["WasaReport"] = rportlist;

            return View("GetWasaReport");



        }


        [HttpGet]
        public IActionResult GetWasaReportSum(DateTime? from, DateTime? to)
        {

            List<WasaReportModelSum> rportlist = new List<WasaReportModelSum>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {

                string sqlquary = "SELECT ID, sum" +
                                  ", round(Production, 1)) as 'Production(cubicmeter)'" +
                                  ",sum(round(Runtime, 1)) as Runtime(Hr)" +
                                  ",sum(round(KWH, 1)) as KWH" +
                                  ",avg(round(Flow, 2)) as AVGFlow(Ltr-min)" +
                                  ",SUM(round(24 - Runtime, 1)) as DownTime" +
                                  " FROM(SELECT *, ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE,[Date])" +  
                                  " ORDER BY DATE DESC)  AS rn " +
                                  " FROM[AZAMPUR] WHERE Runtime != 0) t WHERE t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd");

                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModelSum p = new WasaReportModelSum();

                    p.Id = Convert.ToInt64(rd["ID"]);

                    p.Production = Convert.ToDecimal(rd["Production(cubicmeter)"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime(Hr)"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.AVGFlow = Convert.ToDecimal(rd["AVGFlow(Ltr-min)"]);

                    p.DownTime = Convert.ToDecimal(rd["Downtime"]);


                    rportlist.Add(p);


                }
            }
            catch (Exception ex)
            {

                //throw ex;
            }
            finally
            {
                cnn.Close();
            }

            ViewData["WasaReportSum"] = rportlist;
            ViewData["WasaReport"] = null;

            return View("GetWasaReport");

        }



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


        [HttpGet]
        public IActionResult GoDashboard()
        {
           
            return RedirectToAction("Dashboard", "Login");
        }


    }
}












