using MySql.Data.MySqlClient;
using SportNews.Models;
using SportNews.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportNews.Controllers
{
    public class ConteController : Controller
    {
        MySqlConnection connection = DbUtil.GetDBConnection();
        MySqlCommand cmd = new MySqlCommand();
        List<Category> lstCa = new List<Category>();
        //
        // GET: /Conte/
        public ActionResult Index()
        {
            ContentModel cm = new ContentModel();
            getCate();
            cm.catList = new List<Category>();
            cm.catList = lstCa;
            return View(cm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ContentModel model)
        {
            if (ModelState.IsValid)
            {                
                MySqlConnection connection = DbUtil.GetDBConnection();
                connection.Open();
                string sql = "Insert into news (title, description, cat_id, created_date) values ( ";

                try
                {

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    DateTime theDate = DateTime.Now;
                    theDate.ToString("dd/MM/yyyy");
                    sql = sql + "'" + model.title.ToString() + "'";
                    sql = sql + ", '" + model.descp.ToString() + "'";
                    sql = sql + ", " + model.category_id + "";
                    sql = sql + ", '" + theDate + "'";
                    sql = sql + ");";

                    cmd.CommandText = sql;
                    int rowCount = cmd.ExecuteNonQuery();

                    Console.WriteLine("Row Count affected = " + rowCount);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + sql);
                    Console.WriteLine(e.StackTrace);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                    connection = null;
                }
            }

            return RedirectToAction("Index","Conte");
        }

        [HttpGet]
        public ActionResult Search()
        {
            newslist nl = new newslist();
            nl.ctLst = new List<ContentModel>();
            string sql = @"select a.title, a.description, b.category_name 
                           from news a 
                           join category b on a.cat_id = b.category_id";
            connection.Open();

            try
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ContentModel cm = new ContentModel();
                            cm.title = reader.GetString(0);
                            cm.descp = reader.GetString(1);
                            cm.cat_name = reader.GetString(2);
                            //cm.category_id = Convert.ToInt64(reader.GetValue(0));
                            nl.ctLst.Add(cm);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + sql);
                Console.WriteLine(e.StackTrace);
            }
            return PartialView("_NewsLst", nl);
        }

        public List<Category> getCate()
        {
            string sql = "select category_id, category_name from category ";
            connection.Open();

            try
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while(reader.Read()){
                        Category um = new Category();
                        um.category_name = reader.GetString(1);
                        um.category_id = Convert.ToInt64(reader.GetValue(0));
                        lstCa.Add(um);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + sql);
                Console.WriteLine(e.StackTrace);
            }
            return lstCa;
        }

    }
}