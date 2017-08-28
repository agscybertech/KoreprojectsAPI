using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Warpfusion.A4PP.Objects;
using Warpfusion.A4PP.Services;
using Warpfusion.Shared.Utilities;

namespace koreprojectapi
{
    /// <summary>
    /// Summary description for Projects
    /// </summary>
    public class Projects : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            

            ManagementService managementservice = new ManagementService();
            ScopeServices scopeservices = new ScopeServices();

            managementservice.SQLConnection = ConnectDb.SQLConnection;
            UserProfile contactProfile = new UserProfile();
            Project project = new Project();
            User contactUser = new User();
            long userId;
            string FirstName; string LastName;
            string Email;
            string Contact1; string Contact2; string Contact3;
            string Address;
            string Suburb; int SuburbId; string City; int CityId;
            string PostCode; string Region; int RegionId;
            string Country; int CountryId;
            string ProjectName; string ClaimNumber; string EstimatedTime;
            string StartDate = ""; string ScopeDate="";
            string AssessmentDate=""; string QuotationDate="";
            string FinishDate=""; string ProjectGroupName;
            int ProjectGroupId; int Priority; string Hazard; int status;

            try { userId = Convert.ToInt32(context.Request.QueryString["userId"]); }
            catch { userId = 0; }
            try { FirstName = context.Request.QueryString["FirstName"]; }
            catch { FirstName = ""; }
            try { LastName = context.Request.QueryString["LastName"]; }
            catch { LastName = ""; }
            try { Email = context.Request.QueryString["Email"]; }
            catch { Email = ""; }
            try { Contact1 = context.Request.QueryString["Contact1"]; }
            catch { Contact1 = ""; }
            try { Contact2 = context.Request.QueryString["Contact2"]; }
            catch { Contact2 = ""; }
            try { Contact3 = context.Request.QueryString["Contact3"]; }
            catch { Contact3 = ""; }
            try { Address = context.Request.QueryString["Address"]; }
            catch { Address = ""; }
            try { Suburb = context.Request.QueryString["Suburb"]; }
            catch { Suburb = ""; }
            try { SuburbId = Convert.ToInt32(context.Request.QueryString["SuburbId"]); }
            catch { SuburbId = 0; }
            try { City = context.Request.QueryString["City"]; }
            catch { City = ""; }
            try { CityId = Convert.ToInt32(context.Request.QueryString["CityId"]); }
            catch { CityId = 0; }
            try { PostCode = context.Request.QueryString["PostCode"]; }
            catch { PostCode = ""; }
            try { Region = context.Request.QueryString["Region"]; }
            catch { Region = ""; }
            try { RegionId = Convert.ToInt32(context.Request.QueryString["RegionId"]); }
            catch { RegionId = 0; }
            try { Country = context.Request.QueryString["Country"]; }
            catch { Country = ""; }
            try { CountryId = Convert.ToInt32(context.Request.QueryString["CountryId"]); }
            catch { CountryId = 0; }
            try { ProjectName = context.Request.QueryString["ProjectName"]; }
            catch { ProjectName = ""; }
            try { ClaimNumber = context.Request.QueryString["ClaimNumber"]; }
            catch { ClaimNumber = ""; }
            try { EstimatedTime = context.Request.QueryString["EstimatedTime"]; }
            catch { EstimatedTime = ""; }
            try { StartDate =context.Request.QueryString["StartDate"]; }
            catch { }
            try { ScopeDate = context.Request.QueryString["ScopeDate"]; }
            catch { }
            try { AssessmentDate = context.Request.QueryString["AssessmentDate"]; }
            catch { }
            try { QuotationDate = context.Request.QueryString["QuotationDate"]; }
            catch { }
            try { FinishDate = context.Request.QueryString["FinishDate"]; }
            catch { }
            try { ProjectGroupName = context.Request.QueryString["ProjectGroupName"]; }
            catch { ProjectGroupName = ""; }
            try { ProjectGroupId = Convert.ToInt32(context.Request.QueryString["ProjectGroupId"]); }
            catch { ProjectGroupId = 0; }
            try { Priority = Convert.ToInt32(context.Request.QueryString["Priority"]); }
            catch { Priority = 0; }
            try { Hazard = context.Request.QueryString["Hazard"]; }
            catch { Hazard = ""; }
            try { status = Convert.ToInt32(context.Request.QueryString["status"]); }
            catch { status = 0; }

            contactUser.Email = Email.Trim();
            contactUser.Type = 0;
            userId = managementservice.CreateUser(contactUser, userId);

            long userProfileId;
            contactProfile.UserId = userId;
            contactProfile.FirstName = FirstName.Trim();
            contactProfile.LastName = LastName.Trim();
            contactProfile.Contact1 = Contact1.Trim();
            contactProfile.Contact2 = Contact2.Trim();
            contactProfile.Contact3 = Contact3.Trim();
            contactProfile.Email = Email.Trim();
            userProfileId = managementservice.CreateUserProfile(contactProfile);
            contactProfile.UserProfileId = userProfileId;

            VoucherCodeFunctions cVoucherCode = new VoucherCodeFunctions();
            String strIdentifier = String.Format("{0}{1}", userProfileId, cVoucherCode.GenerateVoucherCodeGuid(16));
            contactProfile.Identifier = strIdentifier;

            managementservice.UpdateUserProfileIdentifier(contactProfile);
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
                        string projectpath = "http://koreprojects.com";
                        string folderPath = context.Server.MapPath(projectpath+"/Images/" + contactProfile.Identifier);
                        if (!System.IO.Directory.Exists(folderPath))
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }
                        fname = context.Server.MapPath(projectpath+"/Images/" + contactProfile.Identifier + "/" + filename);

                        file.SaveAs(fname);
                        //context.Response.Write(filename);
                    }
                }

               
            }
            catch
            {
                //context.Response.Write("un");
                
            }
            if (filename != String.Empty)
            {
                contactProfile.PersonalPhoto = filename;
            }

            managementservice.UpdateUserProfile(contactProfile);

            project.ContactId = userId;
            project.ProjectOwnerId = managementservice.GetProjectOwnerByContactId(userId).ProjectOwnerId;
            project.Address = Address.Trim();
            project.Suburb = Suburb;
            if (Suburb != string.Empty)
            {
                project.SuburbID = SuburbId;
            }

            project.City = City;
            if (City != string.Empty)
            {
                project.CityID = CityId;
            }
            project.Region = Region;
            if (RegionId > 0)
            {
                project.RegionID = RegionId;
            }
            project.Country = Country;
            if (CountryId > 0)
            {
                project.CountryID = CountryId;
            }
            project.Name = ProjectName.Trim();
            project.EQCClaimNumber = ClaimNumber.Trim();
            project.EstimatedTime = EstimatedTime.Trim();

            project.StartDate = Convert.ToDateTime(StartDate);

            project.ScopeDate = Convert.ToDateTime(ScopeDate);

            project.ProjectStatusId = 0;
            if (ProjectGroupId > 0)
            {
                project.GroupID = ProjectGroupId;
                project.GroupName = ProjectGroupName;
            }
            else
            {
                project.GroupID = 0;
                project.GroupName = String.Empty;
            }

            project.AssessmentDate = Convert.ToDateTime(AssessmentDate);
            project.QuotationDate = Convert.ToDateTime(QuotationDate);
            project.FinishDate = Convert.ToDateTime(FinishDate);
            project.Priority = Priority;
            project.Hazard = Hazard.Trim();

            long newProjectId;
            newProjectId = managementservice.CreateProject(project);
            project.ProjectId = newProjectId;

            UserProjectStatusValue userProjectStatusValue = new UserProjectStatusValue();
            userProjectStatusValue.ProjectId = project.ProjectId;
            userProjectStatusValue.UserId = userId;
            userProjectStatusValue.UserProjectStatusValue = status;
            managementservice.CreateUserProjectStatusValue(userProjectStatusValue);
            int projectCredit = 0;

            try
            {
                DataSet dsUserAccount = new DataSet();
                dsUserAccount = managementservice.GetUserAccountByUserID(userId);
                if (dsUserAccount.Tables[0].Rows.Count > 0)
                {
                    projectCredit = int.Parse(dsUserAccount.Tables[0].Rows[0]["ProjectCredit"].ToString());
                }

                if (projectCredit > 0)
                {
                    managementservice.UpdateUserAccount(userId, projectCredit - 1);
                    managementservice.CreateUserTransaction(userId, String.Format("Create Project", project.Name), 0, 0, -1, projectCredit - 1);
                }

            }
            catch (Exception w)
            { }

            context.Response.ContentType = "image/jpg";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
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