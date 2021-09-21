using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WASA.Models;

namespace WASA.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Dashboard()
        { 
            return View();

        }

        public IActionResult ProcessLogin(UserModel userModel)
        {
            List<UserModel> userModels = new List<UserModel>();
            string connectionString;
            SqlConnection cnn;
           connectionString = @"Server=localhost;Database=demo_databaseDB;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connectionString);
            try
            {

                cnn.Open();
                string query = $"Select * from SystemUsers Where UserName='{userModel.UserName }' and Password='{userModel.password}'";
                SqlCommand cmd = new SqlCommand(query, cnn);
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    ViewData["Dashboard"] = userModels;
                    return View("Dashboard", userModel);
                }
                else
                {
                    return View("LoginFailure", userModel);
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

          


        }
    }
}








