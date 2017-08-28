using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Warpfusion.A4PP.Objects;
using Warpfusion.A4PP.Services;

namespace koreprojectapi
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            UserProfile ContactUserProfile = new UserProfile();
            Project CurrentProject = new Project();
            ManagementService managementservice = new ManagementService();
            managementservice.SQLConnection = ConnectDb.SQLConnection;

            string projectpath = "http://koreprojects.com";
            string ProjectID = "0";
            string details = "";
            try { ProjectID = context.Request.QueryString["ProjectID"]; }
            catch { ProjectID = "0"; }
            try { details = context.Request.QueryString["Details"]; }
            catch { details = ""; }
            if (ProjectID == "0")
            {
                context.Response.Write("un");
            }
            else
            {
                CurrentProject = managementservice.GetProjectByProjectId(long.Parse(ProjectID));
                ContactUserProfile = managementservice.GetUserProfileByUserID(CurrentProject.ContactId);
                string filename = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                string fname = "", virtualpath = "";
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                try
                {
                    if (context.Request.Files.Count > 0)
                    {
                        HttpFileCollection files = context.Request.Files;

                        string myactualfilename = "";
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFile file = files[i];
                            myactualfilename = file.FileName;
                            var p = file.FileName.Split('.');

                            var extention = myactualfilename.Split('.');
                            string ext = extention[extention.Length - 1];
                            filename = filename + '.' + ext;
                            //filename = "demo1" + '.' + ext;
                            // 
                            //myactualfilename = RemoveSpecialChar(p[0]) + '.' + ext;
                            //filename = RemoveSpecialChar(p[0]) + "_" + filename;

                            //string folderPath = context.Server.MapPath("../Images/" + ContactUserProfile.Identifier);
                            //if (!System.IO.Directory.Exists(folderPath))
                            //{
                            //    System.IO.Directory.CreateDirectory(folderPath);
                            //}
                            //fname = context.Server.MapPath("../Images/" + ContactUserProfile.Identifier + "/" + filename);

                            string folderPath = context.Server.MapPath(projectpath+"/Images/" + ContactUserProfile.Identifier);
                            if (!System.IO.Directory.Exists(folderPath))
                            {
                                System.IO.Directory.CreateDirectory(folderPath);
                            }
                            fname = context.Server.MapPath(projectpath+"/Images/" + ContactUserProfile.Identifier + "/" + filename);
                            
                            
                            file.SaveAs(fname);

                            try
                            {
                                string fn = filename;
                                string[] fext = fn.Split(new char[] { '.' });
                                string Extentestion = fext[1];
                                virtualpath = fname;

                            }
                            catch { }
                            UserFile CurrentFile = new UserFile();
                            if (long.Parse(ProjectID) > 0)
                            {
                                CurrentFile.Owner = long.Parse(ProjectID);
                            }
                            CurrentFile.FileName = filename;
                            CurrentFile.FileExtension = ext;
                            CurrentFile.Description = details;
                            managementservice.CreateUserFile(CurrentFile);
                        }
                    }
                    bool IsJson = true;
                    //try
                    //{
                    //    //?android=1
                    //    IsJson = context.Request.QueryString["android"].Contains("1");
                    //}
                    //catch { IsJson = false; }
                    if (IsJson)
                    {
                        context.Response.Write(virtualpath);
                    }
                    //else
                    //{
                    //    context.Response.Write(str);
                    //}
                    return;
                }
                catch
                {
                    context.Response.Write("un");
                    return;
                }//TemporaryInboxfileSaving
            }
            

        }
        public string RemoveSpecialChar(string str)
        {
            try
            {
                str = System.Text.RegularExpressions.Regex.Replace(str, "[^a-zA-Z0-9_]+", "");
            }
            catch (Exception)
            { }
            return str;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}