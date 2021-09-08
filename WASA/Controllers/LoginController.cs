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
            List<UserModel> userModels = new List<UserModel>();
            string connectionString;
            SqlConnection cnn;
            connectionString = @"Server=localhost;Database=demo_databaseDB;Trusted_Connection=True;MultipleActiveResultSets=true";
            cnn = new SqlConnection(connectionString);
            try
            {

                cnn.Open();
                SqlCommand cmd = new SqlCommand("Select * from SystemUsers", cnn);
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {




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

            ViewData["Dashboard"] = userModels;

            return View();

        }
    }
}

            

        

               


        //public IActionResult ProcessLogin(UserModel userModel)
        //{
        //    if (userModel.UserName == "Afsana" && userModel.password == "1234")
        //    {
        //        return View("Dashboard", userModel);
        //    } else
        //    {
        //        return View("LoginFailure", userModel);
        //    }
           
        //}
