using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace koreprojectapi
{
    public class ConnectDb
    {
        public static string SQLConnection = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        //    if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
        //    {
        //        //These headers are handling the "pre-flight" OPTIONS call sent by the browser

        //        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
        //        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
        //        HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
        //        HttpContext.Current.Response.End();

        //    }
        //    String resultJSON = "";
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    Context.Response.Clear();
        //    Context.Response.ContentType = "application/json";
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
        //    Dictionary<String, Object> row;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        row = new Dictionary<string, object>();
        //        foreach (DataColumn col in dt.Columns)
        //        {
        //            row.Add(col.ColumnName, dr[col].ToString());
        //        }
        //        tableRows.Add(row);
        //    }
        //    resultJSON = serializer.Serialize(tableRows).ToString();
    }
   
}