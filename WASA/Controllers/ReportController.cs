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
        public IActionResult GetAllPump()
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
            return View();

        }



        [HttpGet]
        public IActionResult GetWasaReportTotal()
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

            ViewData["WasaReportTotal"] = rportlist;
            ViewData["WasaReportTotalFilter"] = null;

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
        public IActionResult GetWasaReportFilterTotal(DateTime? from, DateTime? to)
        {


            List<WasaReportModelSum> rportlist = new List<WasaReportModelSum>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = " select sum(round(production,1))as production,"
                                  + " sum(round(runtime, 1)) as runtime,"
                                  + "sum(round(kwh, 1)) as kwh,"
                                  + "avg(round(flow, 2)) as avgflow,"
                                  + "sum(round(24 - runtime, 1)) as downtime"
                               + " from("
                                   + "select *,"
                                      + "row_number() over(partition by convert(date,[date])  order by date desc)  as rn"
                                    + " from azampur where runtime != 0"
                                  + ") t where t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "'";
               
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModelSum p = new WasaReportModelSum();

                    p.Production = Convert.ToDecimal(rd["Production"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.AVGFlow = Convert.ToDecimal(rd["AVGFlow"]);

                    p.DownTime = Convert.ToDecimal(rd["DownTime"]);

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

            ViewData["WasaReportTotalFilter"] = rportlist;
            ViewData["WasaReportTotal"] = null;

            return View("GetWasaReportTotal");



        }



        public IActionResult GetWasaReportPump2()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from CHALABAN where ID < 101", cnn);

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

            ViewData["WasaReportPump2"] = rportlist;
            return View();

        }



        public IActionResult GetWasaReportTotalPump2()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from CHALABAN where ID < 101", cnn);

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

            ViewData["WasaReportTotalPump2"] = rportlist;
            ViewData["WasaReportTotalFilterPump2"] = null;

            return View();

        }



        public IActionResult GetWasaReportFilterPump2(DateTime? from, DateTime? to)
        {

            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = "SELECT ID, Date" +
                                         ", round(Production, 1) as 'Production(M3)'" +
                                        " ,round(Runtime, 1) as Runtime" +
                                        " ,round(KWH, 1) as KWH" +
                                        " ,round(Flow, 1) as Flow" +
                                        " , round(24 - Runtime, 1) as Downtime" +
                                        " FROM(SELECT *, ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE,[Date]) " +
                                        " ORDER BY DATE DESC) AS rn " +
                                         " FROM CHALABAN) t WHERE t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "' and Runtime != 0";

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

            ViewData["WasaReportPump2"] = rportlist;

            return View("GetWasaReportPump2");

        }



        public IActionResult GetWasaReportFilterTotalPump2(DateTime? from, DateTime? to)
        {


            List<WasaReportModelSum> rportlist = new List<WasaReportModelSum>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = " select sum(round(production,1))as production,"
                                  + " sum(round(runtime, 1)) as runtime,"
                                  + "sum(round(kwh, 1)) as kwh,"
                                  + "avg(round(flow, 2)) as avgflow,"
                                  + "sum(round(24 - runtime, 1)) as downtime"
                               + " from("
                                   + "select *,"
                                      + "row_number() over(partition by convert(date,[date])  order by date desc)  as rn"
                                    + " from chalaban where runtime != 0"
                                  + ") t where t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "'";

                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModelSum p = new WasaReportModelSum();

                    p.Production = Convert.ToDecimal(rd["Production"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.AVGFlow = Convert.ToDecimal(rd["AVGFlow"]);

                    p.DownTime = Convert.ToDecimal(rd["DownTime"]);

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

            ViewData["WasaReportTotalFilterPump2"] = rportlist;
            ViewData["WasaReportTotalPump2"] = null;

            return View("GetWasaReportTotalPump2");



        }



        public IActionResult GetWasaReportPump3()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from CIVILAV where ID < 101", cnn);

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

            ViewData["WasaReportPump3"] = rportlist;
            return View();

        }



        public IActionResult GetWasaReportTotalPump3()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from CIVILAV where ID < 101", cnn);

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

            ViewData["WasaReportTotalPump3"] = rportlist;
            ViewData["WasaReportTotalFilterPump3"] = null;

            return View();

        }



        public IActionResult GetWasaReportFilterPump3(DateTime? from, DateTime? to)
        {

            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = "SELECT ID, Date" +
                                         ", round(Production, 1) as 'Production(M3)'" +
                                        " ,round(Runtime, 1) as Runtime" +
                                        " ,round(KWH, 1) as KWH" +
                                        " ,round(Flow, 1) as Flow" +
                                        " , round(24 - Runtime, 1) as Downtime" +
                                        " FROM(SELECT *, ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE,[Date]) " +
                                        " ORDER BY DATE DESC) AS rn " +
                                         " FROM CIVILAV) t WHERE t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "' and Runtime != 0";

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

            ViewData["WasaReportPump3"] = rportlist;

            return View("GetWasaReportPump3");

        }




        public IActionResult GetWasaReportFilterTotalPump3(DateTime? from, DateTime? to)
        {


            List<WasaReportModelSum> rportlist = new List<WasaReportModelSum>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = " select sum(round(production,1))as production,"
                                  + " sum(round(runtime, 1)) as runtime,"
                                  + "sum(round(kwh, 1)) as kwh,"
                                  + "avg(round(flow, 2)) as avgflow,"
                                  + "sum(round(24 - runtime, 1)) as downtime"
                               + " from("
                                   + "select *,"
                                      + "row_number() over(partition by convert(date,[date])  order by date desc)  as rn"
                                    + " from civilav where runtime != 0"
                                  + ") t where t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "'";

                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModelSum p = new WasaReportModelSum();

                    p.Production = Convert.ToDecimal(rd["Production"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.AVGFlow = Convert.ToDecimal(rd["AVGFlow"]);

                    p.DownTime = Convert.ToDecimal(rd["DownTime"]);

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

            ViewData["WasaReportTotalFilterPump3"] = rportlist;
            ViewData["WasaReportTotalPump3"] = null;

            return View("GetWasaReportTotalPump3");



        }




        public IActionResult GetWasaReportPump4()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from KAWLER where ID < 101", cnn);

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

            ViewData["WasaReportPump4"] = rportlist;
            return View();

        }



        public IActionResult GetWasaReportTotalPump4()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from KAWLER where ID < 101", cnn);

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

            ViewData["WasaReportTotalPump4"] = rportlist;
            ViewData["WasaReportTotalFilterPump4"] = null;

            return View();

        }




        public IActionResult GetWasaReportFilterPump4(DateTime? from, DateTime? to)
        {

            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = "SELECT ID, Date" +
                                         ", round(Production, 1) as 'Production(M3)'" +
                                        " ,round(Runtime, 1) as Runtime" +
                                        " ,round(KWH, 1) as KWH" +
                                        " ,round(Flow, 1) as Flow" +
                                        " , round(24 - Runtime, 1) as Downtime" +
                                        " FROM(SELECT *, ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE,[Date]) " +
                                        " ORDER BY DATE DESC) AS rn " +
                                         " FROM KAWLER) t WHERE t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "' and Runtime != 0";

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

            ViewData["WasaReportPump4"] = rportlist;

            return View("GetWasaReportPump4");

        }





        public IActionResult GetWasaReportFilterTotalPump4(DateTime? from, DateTime? to)
        {


            List<WasaReportModelSum> rportlist = new List<WasaReportModelSum>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = " select sum(round(production,1))as production,"
                                  + " sum(round(runtime, 1)) as runtime,"
                                  + "sum(round(kwh, 1)) as kwh,"
                                  + "avg(round(flow, 2)) as avgflow,"
                                  + "sum(round(24 - runtime, 1)) as downtime"
                               + " from("
                                   + "select *,"
                                      + "row_number() over(partition by convert(date,[date])  order by date desc)  as rn"
                                    + " from kawler where runtime != 0"
                                  + ") t where t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "'";

                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModelSum p = new WasaReportModelSum();

                    p.Production = Convert.ToDecimal(rd["Production"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.AVGFlow = Convert.ToDecimal(rd["AVGFlow"]);

                    p.DownTime = Convert.ToDecimal(rd["DownTime"]);

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

            ViewData["WasaReportTotalFilterPump4"] = rportlist;
            ViewData["WasaReportTotalPump4"] = null;

            return View("GetWasaReportTotalPump4");



        }




        public IActionResult GetWasaReportPump5()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from NIKUNJO where ID < 101", cnn);

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

            ViewData["WasaReportPump5"] = rportlist;
            return View();

        }




        public IActionResult GetWasaReportTotalPump5()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from NIKUNJO where ID < 101", cnn);

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

            ViewData["WasaReportTotalPump5"] = rportlist;
            ViewData["WasaReportTotalFilterPump5"] = null;

            return View();

        }




        public IActionResult GetWasaReportFilterPump5(DateTime? from, DateTime? to)
        {

            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = "SELECT ID, Date" +
                                         ", round(Production, 1) as 'Production(M3)'" +
                                        " ,round(Runtime, 1) as Runtime" +
                                        " ,round(KWH, 1) as KWH" +
                                        " ,round(Flow, 1) as Flow" +
                                        " , round(24 - Runtime, 1) as Downtime" +
                                        " FROM(SELECT *, ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE,[Date]) " +
                                        " ORDER BY DATE DESC) AS rn " +
                                         " FROM NIKUNJO) t WHERE t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "' and Runtime != 0";

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

            ViewData["WasaReportPump5"] = rportlist;

            return View("GetWasaReportPump5");

        }




        public IActionResult GetWasaReportFilterTotalPump5(DateTime? from, DateTime? to)
        {


            List<WasaReportModelSum> rportlist = new List<WasaReportModelSum>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = " select sum(round(production,1))as production,"
                                  + " sum(round(runtime, 1)) as runtime,"
                                  + "sum(round(kwh, 1)) as kwh,"
                                  + "avg(round(flow, 2)) as avgflow,"
                                  + "sum(round(24 - runtime, 1)) as downtime"
                               + " from("
                                   + "select *,"
                                      + "row_number() over(partition by convert(date,[date])  order by date desc)  as rn"
                                    + " from nikunjo where runtime != 0"
                                  + ") t where t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "'";

                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModelSum p = new WasaReportModelSum();

                    p.Production = Convert.ToDecimal(rd["Production"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.AVGFlow = Convert.ToDecimal(rd["AVGFlow"]);

                    p.DownTime = Convert.ToDecimal(rd["DownTime"]);

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

            ViewData["WasaReportTotalFilterPump5"] = rportlist;
            ViewData["WasaReportTotalPump5"] = null;

            return View("GetWasaReportTotalPump5");



        }





        public IActionResult GetWasaReportPump6()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from PRIYANKA where ID < 101", cnn);

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

            ViewData["WasaReportPump6"] = rportlist;
            return View();

        }




        public IActionResult GetWasaReportTotalPump6()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from PRIYANKA where ID < 101", cnn);

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

            ViewData["WasaReportTotalPump6"] = rportlist;
            ViewData["WasaReportTotalFilterPump6"] = null;

            return View();

        }




        public IActionResult GetWasaReportFilterPump6(DateTime? from, DateTime? to)
        {

            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = "SELECT ID, Date" +
                                         ", round(Production, 1) as 'Production(M3)'" +
                                        " ,round(Runtime, 1) as Runtime" +
                                        " ,round(KWH, 1) as KWH" +
                                        " ,round(Flow, 1) as Flow" +
                                        " , round(24 - Runtime, 1) as Downtime" +
                                        " FROM(SELECT *, ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE,[Date]) " +
                                        " ORDER BY DATE DESC) AS rn " +
                                         " FROM PRIYANKA) t WHERE t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "' and Runtime != 0";

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

            ViewData["WasaReportPump6"] = rportlist;

            return View("GetWasaReportPump6");

        }




        public IActionResult GetWasaReportFilterTotalPump6(DateTime? from, DateTime? to)
        {


            List<WasaReportModelSum> rportlist = new List<WasaReportModelSum>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = " select sum(round(production,1))as production,"
                                  + " sum(round(runtime, 1)) as runtime,"
                                  + "sum(round(kwh, 1)) as kwh,"
                                  + "avg(round(flow, 2)) as avgflow,"
                                  + "sum(round(24 - runtime, 1)) as downtime"
                               + " from("
                                   + "select *,"
                                      + "row_number() over(partition by convert(date,[date])  order by date desc)  as rn"
                                    + " from priyanka where runtime != 0"
                                  + ") t where t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "'";

                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModelSum p = new WasaReportModelSum();

                    p.Production = Convert.ToDecimal(rd["Production"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.AVGFlow = Convert.ToDecimal(rd["AVGFlow"]);

                    p.DownTime = Convert.ToDecimal(rd["DownTime"]);

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

            ViewData["WasaReportTotalFilterPump6"] = rportlist;
            ViewData["WasaReportTotalPump6"] = null;

            return View("GetWasaReportTotalPump6");



        }




        public IActionResult GetWasaReportPump7()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from SECTOR12 where ID < 101", cnn);

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

            ViewData["WasaReportPump7"] = rportlist;
            return View();

        }




        public IActionResult GetWasaReportTotalPump7()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from SECTOR12 where ID < 101", cnn);

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

            ViewData["WasaReportTotalPump7"] = rportlist;
            ViewData["WasaReportTotalFilterPump7"] = null;

            return View();

        }




        public IActionResult GetWasaReportFilterPump7(DateTime? from, DateTime? to)
        {

            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = "SELECT ID, Date" +
                                         ", round(Production, 1) as 'Production(M3)'" +
                                        " ,round(Runtime, 1) as Runtime" +
                                        " ,round(KWH, 1) as KWH" +
                                        " ,round(Flow, 1) as Flow" +
                                        " , round(24 - Runtime, 1) as Downtime" +
                                        " FROM(SELECT *, ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE,[Date]) " +
                                        " ORDER BY DATE DESC) AS rn " +
                                         " FROM SECTOR12) t WHERE t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "' and Runtime != 0";

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

            ViewData["WasaReportPump7"] = rportlist;

            return View("GetWasaReportPump7");

        }




        public IActionResult GetWasaReportFilterTotalPump7(DateTime? from, DateTime? to)
        {


            List<WasaReportModelSum> rportlist = new List<WasaReportModelSum>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = " select sum(round(production,1))as production,"
                                  + " sum(round(runtime, 1)) as runtime,"
                                  + "sum(round(kwh, 1)) as kwh,"
                                  + "avg(round(flow, 2)) as avgflow,"
                                  + "sum(round(24 - runtime, 1)) as downtime"
                               + " from("
                                   + "select *,"
                                      + "row_number() over(partition by convert(date,[date])  order by date desc)  as rn"
                                    + " from sector12 where runtime != 0"
                                  + ") t where t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "'";

                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModelSum p = new WasaReportModelSum();

                    p.Production = Convert.ToDecimal(rd["Production"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.AVGFlow = Convert.ToDecimal(rd["AVGFlow"]);

                    p.DownTime = Convert.ToDecimal(rd["DownTime"]);

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

            ViewData["WasaReportTotalFilterPump7"] = rportlist;
            ViewData["WasaReportTotalPump7"] = null;

            return View("GetWasaReportTotalPump7");



        }





        public IActionResult GetWasaReportPump8()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from SECTOR13 where ID < 101", cnn);

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

            ViewData["WasaReportPump8"] = rportlist;
            return View();

        }




        public IActionResult GetWasaReportTotalPump8()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from SECTOR13 where ID < 101", cnn);

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

            ViewData["WasaReportTotalPump8"] = rportlist;
            ViewData["WasaReportTotalFilterPump8"] = null;

            return View();

        }




        public IActionResult GetWasaReportFilterPump8(DateTime? from, DateTime? to)
        {

            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = "SELECT ID, Date" +
                                         ", round(Production, 1) as 'Production(M3)'" +
                                        " ,round(Runtime, 1) as Runtime" +
                                        " ,round(KWH, 1) as KWH" +
                                        " ,round(Flow, 1) as Flow" +
                                        " , round(24 - Runtime, 1) as Downtime" +
                                        " FROM(SELECT *, ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE,[Date]) " +
                                        " ORDER BY DATE DESC) AS rn " +
                                         " FROM SECTOR13) t WHERE t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "' and Runtime != 0";

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

            ViewData["WasaReportPump8"] = rportlist;

            return View("GetWasaReportPump8");

        }




        public IActionResult GetWasaReportFilterTotalPump8(DateTime? from, DateTime? to)
        {


            List<WasaReportModelSum> rportlist = new List<WasaReportModelSum>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = " select sum(round(production,1))as production,"
                                  + " sum(round(runtime, 1)) as runtime,"
                                  + "sum(round(kwh, 1)) as kwh,"
                                  + "avg(round(flow, 2)) as avgflow,"
                                  + "sum(round(24 - runtime, 1)) as downtime"
                               + " from("
                                   + "select *,"
                                      + "row_number() over(partition by convert(date,[date])  order by date desc)  as rn"
                                    + " from sector13 where runtime != 0"
                                  + ") t where t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "'";

                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModelSum p = new WasaReportModelSum();

                    p.Production = Convert.ToDecimal(rd["Production"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.AVGFlow = Convert.ToDecimal(rd["AVGFlow"]);

                    p.DownTime = Convert.ToDecimal(rd["DownTime"]);

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

            ViewData["WasaReportTotalFilterPump8"] = rportlist;
            ViewData["WasaReportTotalPump8"] = null;

            return View("GetWasaReportTotalPump8");



        }



        public IActionResult GetWasaReportPump9()
        {
            List<WasaReportModelPump> rportlist = new List<WasaReportModelPump>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                //SqlCommand cmd = new SqlCommand("Select * from SECTOR3 where ID < 101", cnn);
                SqlCommand cmd = new SqlCommand("Select * from SECTOR3 where Runtime < 18", cnn);
                

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {

                    WasaReportModelPump p = new WasaReportModelPump();

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

            ViewData["WasaReportPump9"] = rportlist;
            return View();

        }



        public IActionResult GetWasaReportTotalPump9()
        {
            List<WasaReportModelPump> rportlist = new List<WasaReportModelPump>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from SECTOR3 where ID < 101", cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {

                    WasaReportModelPump p = new WasaReportModelPump();

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

            ViewData["WasaReportTotalPump9"] = rportlist;
            ViewData["WasaReportTotalFilterPump9"] = null;

            return View();

        }




        public IActionResult GetWasaReportFilterPump9(DateTime? from, DateTime? to)
        {

            List<WasaReportModelPump> rportlist = new List<WasaReportModelPump>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = "SELECT ID, Date" +
                                         ", round(Production, 1) as 'Production(M3)'" +
                                        " ,round(Runtime, 1) as Runtime" +
                                        " ,round(KWH, 1) as KWH" +
                                        " ,round(Flow, 1) as Flow" +
                                        " , round(24 - Runtime, 1) as Downtime" +
                                        " FROM(SELECT *, ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE,[Date]) " +
                                        " ORDER BY DATE DESC) AS rn " +
                                         " FROM SECTOR3) t WHERE t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "' and Runtime != 0";

                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModelPump p = new WasaReportModelPump();

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

            ViewData["WasaReportPump9"] = rportlist;

            return View("GetWasaReportPump9");

        }




        public IActionResult GetWasaReportFilterTotalPump9(DateTime? from, DateTime? to)
        {


            List<WasaReportModelSum> rportlist = new List<WasaReportModelSum>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = " select sum(round(production,1))as production,"
                                  + " sum(round(runtime, 1)) as runtime,"
                                  + "sum(round(kwh, 1)) as kwh,"
                                  + "avg(round(flow, 2)) as avgflow,"
                                  + "sum(round(24 - runtime, 1)) as downtime"
                               + " from("
                                   + "select *,"
                                      + "row_number() over(partition by convert(date,[date])  order by date desc)  as rn"
                                    + " from sector3 where runtime != 0"
                                  + ") t where t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "'";

                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModelSum p = new WasaReportModelSum();

                    p.Production = Convert.ToDecimal(rd["Production"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.AVGFlow = Convert.ToDecimal(rd["AVGFlow"]);

                    p.DownTime = Convert.ToDecimal(rd["DownTime"]);

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

            ViewData["WasaReportTotalFilterPump9"] = rportlist;
            ViewData["WasaReportTotalPump9"] = null;

            return View("GetWasaReportTotalPump9");



        }





        public IActionResult GetWasaReportPump10()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from SECTOR9 where ID < 101", cnn);

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

            ViewData["WasaReportPump10"] = rportlist;
            return View();

        }




        public IActionResult GetWasaReportTotalPump10()
        {
            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from SECTOR9 where ID < 101", cnn);

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

            ViewData["WasaReportTotalPump10"] = rportlist;
            ViewData["WasaReportTotalFilterPump10"] = null;

            return View();

        }





        public IActionResult GetWasaReportFilterPump10(DateTime? from, DateTime? to)
        {

            List<WasaReportModel> rportlist = new List<WasaReportModel>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = "SELECT ID, Date" +
                                         ", round(Production, 1) as 'Production(M3)'" +
                                        " ,round(Runtime, 1) as Runtime" +
                                        " ,round(KWH, 1) as KWH" +
                                        " ,round(Flow, 1) as Flow" +
                                        " , round(24 - Runtime, 1) as Downtime" +
                                        " FROM(SELECT *, ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE,[Date]) " +
                                        " ORDER BY DATE DESC) AS rn " +
                                         " FROM SECTOR9) t WHERE t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "' and Runtime != 0";

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

            ViewData["WasaReportPump10"] = rportlist;

            return View("GetWasaReportPump10");

        }





        public IActionResult GetWasaReportFilterTotalPump10(DateTime? from, DateTime? to)
        {


            List<WasaReportModelSum> rportlist = new List<WasaReportModelSum>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                string sqlquary = " select sum(round(production,1))as production,"
                                  + " sum(round(runtime, 1)) as runtime,"
                                  + "sum(round(kwh, 1)) as kwh,"
                                  + "avg(round(flow, 2)) as avgflow,"
                                  + "sum(round(24 - runtime, 1)) as downtime"
                               + " from("
                                   + "select *,"
                                      + "row_number() over(partition by convert(date,[date])  order by date desc)  as rn"
                                    + " from sector9 where runtime != 0"
                                  + ") t where t.rn = 1 and date between '" + from.Value.Date.ToString("yyyy-MM-dd") + "'and'" + to.Value.Date.ToString("yyyy-MM-dd") + "'";

                cnn.Open();
                SqlCommand cmd = new SqlCommand(sqlquary, cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    WasaReportModelSum p = new WasaReportModelSum();

                    p.Production = Convert.ToDecimal(rd["Production"]);

                    p.Runtime = Convert.ToDecimal(rd["Runtime"]);

                    p.KWH = Convert.ToDecimal(rd["KWH"]);

                    p.AVGFlow = Convert.ToDecimal(rd["AVGFlow"]);

                    p.DownTime = Convert.ToDecimal(rd["DownTime"]);

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

            ViewData["WasaReportTotalFilterPump10"] = rportlist;
            ViewData["WasaReportTotalPump10"] = null;

            return View("GetWasaReportTotalPump10");



        }





        public IActionResult GetWasaReportPump11()
        {
            List<WasaReportModelStatic> rportlist = new List<WasaReportModelStatic>();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Server=localhost;Database=MZ-9;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from STATICAZAMPUR where ID < 101", cnn);

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {

                    WasaReportModelStatic p = new WasaReportModelStatic();

                    p.Date = Convert.ToDateTime(rd["Date"]);

                    p.PumpBrand = Convert.ToString(rd["PumpBrand"]);

                    p.VFDBrand = Convert.ToString(rd["VFDBrand"]);

                    p.TXBrand = Convert.ToString(rd["TXBrand"]);

                    p.PumpSerial = rd["PumpSerial"] == DBNull.Value ? 00 : Convert.ToInt64(rd["PumpSerial"]);

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

            ViewData["WasaReportPump11"] = rportlist;
            return View();

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

        [HttpGet]
        public IActionResult GoLogout()
        {
            return RedirectToAction("Index", "Login");
        }

    }
}












