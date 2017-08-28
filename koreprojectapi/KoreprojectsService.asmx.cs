using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Warpfusion.A4PP.Services;
using Warpfusion.Shared.Utilities;
using Warpfusion.A4PP.Objects;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data;
using System.IO;
using System.Drawing;
using System.Data.SqlClient;

namespace koreprojectapi
{
    /// <summary>
    /// Summary description for KoreprojectsService
    /// </summary>
    [WebService(Namespace = "http://and.koreprojects.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class KoreprojectsService : System.Web.Services.WebService
    {
        private ManagementService managementservice = new ManagementService();
        private ScopeServices scopeservices = new ScopeServices();

        private User user = new User();
        private WorksheetGroup worksheetgroup;
        private Area area;
        private Item item;
        private WorksheetService worksheetService;
        private ServiceGroup serviceGroup;
        private ProjectGroup projectgroup;
        private VoucherCodeFunctions vouchercodefunctions;
        private UserProjectStatusSetting userProjectStatusSetting;
        private UserProjectJobSetting userProjectJobSetting;
        private UserProfile userProfile;
        private ProjectOwner projectOwner;

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void Loginuser(string username, string pwd)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            user = managementservice.Login(username, pwd);

            //String resultJSON = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (user != null)
            {
                row.Add("user", true);
                row.Add("UserId", user.UserId);
                row.Add("Email", user.Email);
                row.Add("Password", user.Password);
                row.Add("Type", user.Type);
                row.Add("Mailbox", user.Mailbox);
                row.Add("BranchID", user.BranchId);
                row.Add("CompanyID", user.CompanyId);
                row.Add("CreatedDate", user.CreatedDate);
                row.Add("ModifiedDate", user.ModifiedDate);
                row.Add("DeactivatedDate", user.DeactivatedDate);
                row.Add("ActionToken", user.ActionToken);
                row.Add("AccessLevel", user.AccessLevel);
            }
            else
            {
                row.Add("user", false);
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
            //return resultJSON;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadLoginuser(string username, string pwd)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            user = new User();
            user = managementservice.Login(username, pwd);

            //String resultJSON = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (user != null)
            {
                row.Add("UserId", user.UserId);
                row.Add("Email", user.Email);
                row.Add("Password", user.Password);
                row.Add("Type", user.Type);
                row.Add("Mailbox", user.Mailbox);
                row.Add("BranchID", user.BranchId);
                row.Add("CompanyID", user.CompanyId);
                row.Add("CreatedDate", user.CreatedDate);
                row.Add("ModifiedDate", user.ModifiedDate);
                row.Add("DeactivatedDate", user.DeactivatedDate);
                row.Add("ActionToken", user.ActionToken);
                row.Add("AccessLevel", user.AccessLevel);

            }
            else
            {
                row.Add("Result", "No Data found");
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
            //return resultJSON;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectOwnerData(long ProjectOwnerId, long userid)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            projectOwner = new ProjectOwner();
            projectOwner = managementservice.GetProjectOwnerByProjectOwnerId(ProjectOwnerId);

            userProfile = new UserProfile();
            userProfile = managementservice.GetUserProfileByUserID(userid);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (projectOwner != null)
            {
                row.Add("Name", projectOwner.Name);
                row.Add("Contact1", projectOwner.Contact1);
                row.Add("Contact2", projectOwner.Contact2);
                row.Add("Contact3", projectOwner.Contact3);
                row.Add("Accreditation", projectOwner.Accreditation);
                row.Add("EQRSupervisor", projectOwner.EQRSupervisor);
                row.Add("Address", projectOwner.Address);
                row.Add("Suburb", projectOwner.Suburb);
                row.Add("City", projectOwner.City);
                row.Add("PostCode", projectOwner.PostCode);
                row.Add("Region", projectOwner.Region);
                row.Add("Country", projectOwner.Country);
                row.Add("AccreditationNumber", projectOwner.AccreditationNumber);
                row.Add("GSTNumber", projectOwner.GSTNumber);


                row.Add("FirstName", userProfile.FirstName);
                row.Add("LastName", userProfile.LastName);
                row.Add("Email", userProfile.Email);

            }
            else
            {
                row.Add("Result", "No Data found");
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateProjectOwnerData(long userid, long ProjectOwnerId, string BusinessName, string FirstName, string LastName, string Email, string Contact1, string Contact2, string Contact3, string Accreditation, string AccreditationNumber, string GSTNumber, string EQRSupervisor, string Address, string Suburb, string City, string PostCode, string Region, string Country)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;

            projectOwner = new ProjectOwner();
            projectOwner.ProjectOwnerId = ProjectOwnerId;
            projectOwner.Name = BusinessName;
            projectOwner.Contact1 = Contact1;
            projectOwner.Contact2 = Contact2;
            projectOwner.Contact3 = Contact3;
            projectOwner.Accreditation = Accreditation;
            projectOwner.AccreditationNumber = AccreditationNumber;
            projectOwner.GSTNumber = GSTNumber;
            projectOwner.EQRSupervisor = EQRSupervisor;
            projectOwner.Address = Address;
            projectOwner.Suburb = Suburb;
            projectOwner.City = City;
            projectOwner.PostCode = PostCode;
            projectOwner.Region = Region;
            projectOwner.Country = Country;
            projectOwner.ContactId = userid;
            managementservice.UpdateProjectOwner(projectOwner);

            userProfile = new UserProfile();
            userProfile = managementservice.GetUserProfileByUserID(userid);
            userProfile.FirstName = FirstName;
            userProfile.LastName = LastName;
            userProfile.Contact1 = Contact1;
            userProfile.Contact2 = Contact2;
            userProfile.Contact3 = Contact3;
            userProfile.Email = Email;
            managementservice.UpdateUserProfile(userProfile);

            user = new User();
            user.UserId = userid;
            user.Email = Email;
            managementservice.UpdateUserEmail(user);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void Signup(string Email, string Password)
        {
            int result = 0;
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            if (managementservice.GetUserCountByEmail(Email) == 0)
            {
                User signUser = new User();
                vouchercodefunctions = new VoucherCodeFunctions();
                string strIdentifier = "";
                signUser.Email = Email;
                signUser.Password = Password;
                signUser.Type = 2;

                long signUserID = managementservice.CreateUser(signUser, 0);
                userProfile = new UserProfile();
                userProfile.UserId = signUserID;
                userProfile.Email = Email;
                result = 1;

                long userProfileId = managementservice.CreateUserProfile(userProfile);
                userProfile.UserProfileId = userProfileId;
                strIdentifier = String.Format("{0}{1}", userProfileId, vouchercodefunctions.GenerateVoucherCodeGuid(16));
                userProfile.Identifier = strIdentifier;
                managementservice.UpdateUserProfileIdentifier(userProfile);

                projectOwner = new ProjectOwner();
                projectOwner.ContactId = signUserID;
                long intCompanyID = managementservice.CreateProjectOwner(projectOwner);
                projectOwner.ProjectOwnerId = intCompanyID;
                strIdentifier = String.Format("{0}{1}", vouchercodefunctions.GenerateVoucherCodeGuid(16), intCompanyID);
                projectOwner.Identifier = strIdentifier;
                managementservice.UpdateProjectOwnerIdentifier(projectOwner);
                user = managementservice.Login(Email, Password);
                if (user != null)
                {
                    int numberOfProjects = 0;
                    ServiceGroup objServiceGroup = new ServiceGroup();
                    objServiceGroup.UserId = user.UserId;
                    objServiceGroup.Name = "Rate Sheet 1";
                    int intDisplayOrder = 0;

                    objServiceGroup.DisplayOrder = intDisplayOrder;
                    objServiceGroup.IsPrivate = 2;
                    scopeservices.CreateServiceGroup(objServiceGroup);
                    objServiceGroup = new ServiceGroup();
                    objServiceGroup.UserId = user.UserId;
                    objServiceGroup.Name = "Rate Sheet 2";
                    objServiceGroup.DisplayOrder = intDisplayOrder;
                    objServiceGroup.IsPrivate = 2;
                    scopeservices.CreateServiceGroup(objServiceGroup);

                }
            }


            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (result > 0)
            {
                row.Add("Result", result);
            }
            else
            {
                row.Add("Result", 0);
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        //Start Group Data----------------------------------------------------------------------
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetGroupData(long companyid)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = scopeservices.GetWorksheetGroupsByProjectOwnerId(companyid);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadGroupData(long GroupId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            worksheetgroup = new WorksheetGroup();
            worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (worksheetgroup != null)
            {
                row.Add("WorksheetGroupId", worksheetgroup.WorksheetGroupId);
                row.Add("ProjectOwnerId", worksheetgroup.ProjectOwnerId);
                row.Add("Name", worksheetgroup.Name);
                row.Add("Description", worksheetgroup.Description);
                row.Add("Disabled", worksheetgroup.Disabled);
                row.Add("DisplayOrder", worksheetgroup.DisplayOrder);

            }
            else
            {
                row.Add("Result", "No Data found");
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveGroupData(long ProjectOwnerId, string Name, int DisplayOrder)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            worksheetgroup = new WorksheetGroup();
            worksheetgroup.ProjectOwnerId = ProjectOwnerId;
            worksheetgroup.Name = Name;
            worksheetgroup.DisplayOrder = DisplayOrder;
            long result = scopeservices.CreateWorksheetGroup(worksheetgroup);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (result > 0)
            {
                row.Add("Result", result);
            }
            else
            {
                row.Add("Result", 0);
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateGroupData(long GroupId, long ProjectOwnerId, string Name, int DisplayOrder)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            worksheetgroup = new WorksheetGroup();
            worksheetgroup.WorksheetGroupId = GroupId;
            worksheetgroup.ProjectOwnerId = ProjectOwnerId;
            worksheetgroup.Name = Name;
            worksheetgroup.DisplayOrder = DisplayOrder;
            scopeservices.UpdateWorksheetGroup(worksheetgroup);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteGroupData(long GroupId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);

            scopeservices.DeleteWorksheetGroupByWorksheetGroupId(GroupId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }
        //End Group Data-----------------------------------------------------------------------




        //Start Area Data----------------------------------------------------------------------
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetAreaData(long companyid)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = scopeservices.GetAreasByProjectOwnerId(companyid);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadAreaData(long AreaId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            area = new Area();
            area = scopeservices.GetAreaByAreaId(AreaId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (area != null)
            {
                row.Add("AreaId", area.AreaId);
                row.Add("ProjectOwnerId", area.ProjectOwnerId);
                row.Add("Name", area.Name);
                row.Add("Description", area.Description);
                row.Add("Disabled", area.Disabled);
                row.Add("DisplayOrder", area.DisplayOrder);

            }
            else
            {
                row.Add("Result", "No Data found");
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveAreaData(long ProjectOwnerId, string Name, int DisplayOrder)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            area = new Area();
            area.ProjectOwnerId = ProjectOwnerId;
            area.Name = Name;
            area.DisplayOrder = DisplayOrder;
            long result = scopeservices.CreateArea(area);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (result > 0)
            {
                row.Add("Result", result);
            }
            else
            {
                row.Add("Result", 0);
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateAreaData(long AreaId, long ProjectOwnerId, string Name, int DisplayOrder)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            area = new Area();
            area.AreaId = AreaId;
            area.ProjectOwnerId = ProjectOwnerId;
            area.Name = Name;
            area.DisplayOrder = DisplayOrder;
            scopeservices.UpdateArea(area);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteAreaData(long AreaId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);

            scopeservices.DeleteAreaByAreaId(AreaId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        //End Area Data----------------------------------------------------------------------




        //Start Item Data----------------------------------------------------------------------
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetItemData(long companyid)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = scopeservices.GetItemsByProjectOwnerId(companyid);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadItemData(long itemId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            item = new Item();
            item = scopeservices.GetItemByItemId(itemId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (item != null)
            {
                row.Add("ItemId", item.ItemId);
                row.Add("ProjectOwnerId", item.ProjectOwnerId);
                row.Add("Name", item.Name);
                row.Add("Description", item.Description);
                row.Add("Disabled", item.Disabled);
                row.Add("DisplayOrder", item.DisplayOrder);

            }
            else
            {
                row.Add("Result", "No Data found");
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveItemData(long ProjectOwnerId, string Name, int DisplayOrder)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            item = new Item();
            item.ProjectOwnerId = ProjectOwnerId;
            item.Name = Name;
            item.DisplayOrder = DisplayOrder;
            long result = scopeservices.CreateItem(item);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (result > 0)
            {
                row.Add("Result", result);
            }
            else
            {
                row.Add("Result", 0);
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateItemData(long ItemId, long ProjectOwnerId, string Name, int DisplayOrder)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            item = new Item();
            item.ItemId = ItemId;
            item.ProjectOwnerId = ProjectOwnerId;
            item.Name = Name;
            item.DisplayOrder = DisplayOrder;
            scopeservices.UpdateItem(item);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteItemData(long ItemId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);

            scopeservices.DeleteItemByItemId(ItemId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        //End Item Data----------------------------------------------------------------------



        //Start Service Data----------------------------------------------------------------------
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetServiceData(long companyid)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = scopeservices.GetWorksheetServicesByProjectOwnerId(companyid);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadServiceData(long serviceId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            worksheetService = new WorksheetService();
            worksheetService = scopeservices.GetWorksheetServiceByWorksheetServiceId(serviceId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (worksheetService != null)
            {
                row.Add("WorksheetServiceId", worksheetService.WorksheetServiceId);
                row.Add("ProjectOwnerId", worksheetService.ProjectOwnerId);
                row.Add("Name", worksheetService.Name);
                row.Add("Description", worksheetService.Description);
                row.Add("Disabled", worksheetService.Disabled);
                row.Add("DisplayOrder", worksheetService.DisplayOrder);

            }
            else
            {
                row.Add("Result", "No Data found");
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveServiceData(long ProjectOwnerId, string Name, int DisplayOrder)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            worksheetService = new WorksheetService();
            worksheetService.ProjectOwnerId = ProjectOwnerId;
            worksheetService.Name = Name;
            worksheetService.DisplayOrder = DisplayOrder;
            long result = scopeservices.CreateWorksheetService(worksheetService);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (result > 0)
            {
                row.Add("Result", result);
            }
            else
            {
                row.Add("Result", 0);
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateServiceData(long ServiceId, long ProjectOwnerId, string Name, int DisplayOrder)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            worksheetService = new WorksheetService();
            worksheetService.WorksheetServiceId = ServiceId;
            worksheetService.ProjectOwnerId = ProjectOwnerId;
            worksheetService.Name = Name;
            worksheetService.DisplayOrder = DisplayOrder;
            scopeservices.UpdateWorksheetService(worksheetService);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteServiceData(long ServiceId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);

            scopeservices.DeleteWorksheetServiceByWorksheetServiceId(ServiceId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        //End Service Data----------------------------------------------------------------------



        //Start Rate Sheet Data----------------------------------------------------------------------

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetRateSheetData(long userid)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = scopeservices.GetServiceGroupsByUserId(userid);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadRateSheetData(long serviceId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            serviceGroup = new ServiceGroup();
            serviceGroup = scopeservices.GetServiceGroupByServiceGroupId(serviceId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (serviceGroup != null)
            {
                row.Add("ServiceGroupId", serviceGroup.ServiceGroupId);
                row.Add("UserId", serviceGroup.UserId);
                row.Add("Name", serviceGroup.Name);
                row.Add("IsPrivate", serviceGroup.IsPrivate);
                row.Add("DisplayOrder", serviceGroup.DisplayOrder);

            }
            else
            {
                row.Add("Result", "No Data found");
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveRateSheetData(long UserId, string Name, int DisplayOrder, int IsPrivate)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            serviceGroup = new ServiceGroup();
            serviceGroup.UserId = UserId;
            serviceGroup.Name = Name;
            serviceGroup.DisplayOrder = DisplayOrder;
            serviceGroup.IsPrivate = IsPrivate;
            long result = scopeservices.CreateServiceGroup(serviceGroup);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (result > 0)
            {
                row.Add("Result", result);
            }
            else
            {
                row.Add("Result", 0);
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateRateSheetData(long ServiceGroupId, long UserId, string Name, int DisplayOrder, int IsPrivate)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            serviceGroup = new ServiceGroup();
            serviceGroup.ServiceGroupId = ServiceGroupId;
            serviceGroup.UserId = UserId;
            serviceGroup.Name = Name;
            serviceGroup.DisplayOrder = DisplayOrder;
            serviceGroup.IsPrivate = IsPrivate;
            scopeservices.UpdateServiceGroup(serviceGroup);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteRateSheetData(long ServiceGroupId, bool IsPhysicalDelete)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);

            scopeservices.DeleteServiceGroupByServiceGroupId(ServiceGroupId, IsPhysicalDelete);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        //End Rate Sheet Data----------------------------------------------------------------------



        //Start Service Rates Data ----------------------------------------------------------------------

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetServiceRateGroupData(long userid)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = scopeservices.GetServiceGroupsByUserId(userid);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetServiceRatesByGroupId(long servicegroupId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = scopeservices.GetAllServiceRatesByServiceGroupId(servicegroupId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void IsUserAccountOverDue(long UserId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            bool val = managementservice.IsUserAccountOverDue(UserId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            //foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                //foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add("Result", val);
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadServiceRateGroupData(long ServiceRateID)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            DataSet dsServiceRate = new DataSet();
            dsServiceRate = scopeservices.GetServiceRateByServiceRateId(ServiceRateID);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (dsServiceRate.Tables[0].Rows.Count > 0)
            {
                row.Add("ServiceRateId", dsServiceRate.Tables[0].Rows[0]["ServiceRateId"].ToString());
                row.Add("ServiceID", dsServiceRate.Tables[0].Rows[0]["ServiceID"].ToString());
                row.Add("ServiceName", dsServiceRate.Tables[0].Rows[0]["Name"].ToString());
                row.Add("Note", dsServiceRate.Tables[0].Rows[0]["Description"].ToString());
                row.Add("Cost", dsServiceRate.Tables[0].Rows[0]["CostRate"].ToString());
                row.Add("Charge", dsServiceRate.Tables[0].Rows[0]["ChargeRate"].ToString());
                row.Add("Unit", dsServiceRate.Tables[0].Rows[0]["Unit"].ToString());
                row.Add("DisplayOrder", dsServiceRate.Tables[0].Rows[0]["DisplayOrder"].ToString());
                row.Add("Status", dsServiceRate.Tables[0].Rows[0]["Status"].ToString());

            }
            else
            {
                row.Add("Result", "No Data found");
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveServiceRateGroupData(long UserId, long CompanyId, string ServiceName, string Note, long Cost, string Charge, string Unit, int DisplayOrder, string Status, string arrchecked, string arrunchecked)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            Service objService = new Service();
            objService.Name = ServiceName;
            objService.ProjectOwnerId = CompanyId;
            objService.UserId = UserId;
            objService.Description = Note;
            objService.DisplayOrder = DisplayOrder;
            long m_ServiceID = scopeservices.CreateService(objService);

            ServiceRate objServiceRate = new ServiceRate();
            objServiceRate.ServiceId = m_ServiceID;
            if (Status == "ON")
                objServiceRate.Disabled = false;
            else
                objServiceRate.Disabled = true;

            objServiceRate.CostRate = Cost;
            objServiceRate.ChargeRate = Charge;
            objServiceRate.Unit = Unit;
            scopeservices.CreateServiceRate(objServiceRate);

            string[] arrayun = arrunchecked.Trim('(', ')').Split(',');
            string[] arrayc = arrchecked.Trim('(', ')').Split(',');
            for (int i = 0; i < arrayun.Length; i++)
            {
                scopeservices.DeleteServiceGroupItemByServiceGroupIdServiceId(long.Parse(arrayun[i]), m_ServiceID);
            }
            for (int i = 0; i < arrayc.Length; i++)
            {
                scopeservices.CreateServiceGroupItem(long.Parse(arrayc[i]), m_ServiceID);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (m_ServiceID > 0)
            {
                row.Add("Result", m_ServiceID);
            }
            else
            {
                row.Add("Result", 0);
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateServiceRateGroupData(long ServiceRateID, long ServiceID, long UserId, long CompanyId, string ServiceName, string Note, long Cost, string Charge, string Unit, int DisplayOrder, string Status, string arrchecked, string arrunchecked)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            Service objService = new Service();
            objService.ServiceId = ServiceID;
            objService.Name = ServiceName;
            objService.ProjectOwnerId = CompanyId;
            objService.UserId = UserId;
            objService.Description = Note;
            objService.DisplayOrder = DisplayOrder;
            scopeservices.UpdateService(objService);

            ServiceRate objServiceRate = new ServiceRate();
            objServiceRate.ServiceId = ServiceID;
            objServiceRate.ServiceRateId = ServiceRateID;
            if (Status == "ON")
                objServiceRate.Disabled = false;
            else
                objServiceRate.Disabled = true;

            objServiceRate.CostRate = Cost;
            objServiceRate.ChargeRate = Charge;
            objServiceRate.Unit = Unit;
            scopeservices.UpdateServiceRate(objServiceRate);

            string[] arrayun = arrunchecked.Trim('(', ')').Split(',');
            string[] arrayc = arrchecked.Trim('(', ')').Split(',');
            for (int i = 0; i < arrayun.Length; i++)
            {
                scopeservices.DeleteServiceGroupItemByServiceGroupIdServiceId(long.Parse(arrayun[i]), ServiceID);
            }
            for (int i = 0; i < arrayc.Length; i++)
            {
                scopeservices.CreateServiceGroupItem(long.Parse(arrayc[i]), ServiceID);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (ServiceID > 0)
            {
                row.Add("Result", ServiceID);
            }
            else
            {
                row.Add("Result", 0);
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteServiceRateGroupData(long ServiceRateID, long ServiceID, string arrchecked)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);

            scopeservices.DeleteServiceRateByServiceRateId(ServiceRateID);
            scopeservices.DeleteServiceByServiceId(ServiceID);
            string[] arrayc = arrchecked.Trim('(', ')').Split(',');
            for (int i = 0; i < arrayc.Length; i++)
            {
                scopeservices.DeleteServiceGroupItemByServiceGroupIdServiceId(long.Parse(arrayc[i]), ServiceID);
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }
        //End Service Rates Data ----------------------------------------------------------------------


        //Start Project SetUp Data ----------------------------------------------------------------------

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectGroupData(long companyid)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = scopeservices.GetProjectGroupsByProjectOwnerId(companyid);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadProjectGroupData(long ProjectGroupId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            projectgroup = new ProjectGroup();
            projectgroup = managementservice.GetProjectGroupByProjectGroupId(ProjectGroupId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (projectgroup != null)
            {
                row.Add("ProjectGroupId", projectgroup.ProjectGroupId);
                row.Add("BusinessName", projectgroup.Name);
                row.Add("Email", projectgroup.Email);
                row.Add("Phone", projectgroup.Contact1);
                row.Add("Fax", projectgroup.Contact2);
                row.Add("Mobile", projectgroup.Contact3);
                row.Add("Address", projectgroup.Address);
                row.Add("Suburb", projectgroup.Suburb);
                row.Add("City", projectgroup.City);
                row.Add("PostCode", projectgroup.PostCode);
                row.Add("Region", projectgroup.Region);
                row.Add("Country", projectgroup.Country);
                row.Add("DisplayOrder", projectgroup.DisplayOrder);

            }
            else
            {
                row.Add("Result", "No Data found");
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveProjectGroupData(long ProjectOwnerId, string Name, string Email, string Contact1, string Contact2, string Contact3, string Address, string Suburb, string City, string PostCode, string Region, string Country, int DisplayOrder)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            projectgroup = new ProjectGroup();
            projectgroup.Name = Name;
            projectgroup.Email = Email;
            projectgroup.Contact1 = Contact1;
            projectgroup.Contact2 = Contact2;
            projectgroup.Contact3 = Contact3;
            projectgroup.Address = Address;
            projectgroup.Suburb = Suburb;
            projectgroup.City = City;
            projectgroup.PostCode = PostCode;
            projectgroup.Region = Region;
            projectgroup.Country = Country;
            projectgroup.ProjectOwnerId = ProjectOwnerId;
            long result = managementservice.CreateProjectGroup(projectgroup);
            projectgroup.ProjectGroupId = result;
            vouchercodefunctions = new VoucherCodeFunctions();
            string strIdentifier = String.Format("{0}{1}", vouchercodefunctions.GenerateVoucherCodeGuid(16), result);
            projectgroup.Identifier = strIdentifier;
            managementservice.UpdateProjectGroupIdentifier(projectgroup);

            //string NewFile = UploadImages(result, strIdentifier);
            //if (NewFile != string.Empty)
            //  projectgroup.Logo = NewFile;

            managementservice.UpdateProjectGroup(projectgroup);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (result > 0)
            {
                row.Add("Result", result);
            }
            else
            {
                row.Add("Result", 0);
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateProjectGroupData(long ProjectGroupId, long ProjectOwnerId, string Name, string Email, string Contact1, string Contact2, string Contact3, string Address, string Suburb, string City, string PostCode, string Region, string Country, int DisplayOrder)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            projectgroup = new ProjectGroup();
            projectgroup.ProjectGroupId = ProjectGroupId;
            projectgroup.Name = Name;
            projectgroup.Email = Email;
            projectgroup.Contact1 = Contact1;
            projectgroup.Contact2 = Contact2;
            projectgroup.Contact3 = Contact3;
            projectgroup.Address = Address;
            projectgroup.Suburb = Suburb;
            projectgroup.City = City;
            projectgroup.PostCode = PostCode;
            projectgroup.Region = Region;
            projectgroup.Country = Country;
            projectgroup.ProjectOwnerId = ProjectOwnerId;
            managementservice.UpdateProjectGroup(projectgroup);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteProjectGroupData(long ProjectGroupID)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);

            managementservice.GetProjectGroupByProjectGroupId(ProjectGroupID);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }
        //End Project SetUp Data ----------------------------------------------------------------------


        //Start Project Status Data ----------------------------------------------------------------------

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectStatusData(long Projectid, long userid)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = managementservice.GetUserProjectStatusesByProjectIdUserId(Projectid, userid);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadProjectStatusData(long ProjectStatusSettingId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            userProjectStatusSetting = new UserProjectStatusSetting();
            userProjectStatusSetting = managementservice.GetUserProjectStatusSettingByUserProjectStatusSettingId(ProjectStatusSettingId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (userProjectStatusSetting != null)
            {
                row.Add("UserProjectStatusSettingId", userProjectStatusSetting.UserProjectStatusSettingId);
                row.Add("Name", userProjectStatusSetting.Name);
                row.Add("DisplayOrder", userProjectStatusSetting.DisplayOrder);

            }
            else
            {
                row.Add("Result", "No Data found");
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveProjectStatusData(long UserId, string Name, int DisplayOrder)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            userProjectStatusSetting = new UserProjectStatusSetting();
            userProjectStatusSetting.UserId = UserId;
            userProjectStatusSetting.Name = Name;
            userProjectStatusSetting.ProjectId = 0;
            userProjectStatusSetting.DisplayOrder = DisplayOrder;
            long result = managementservice.CreateUserProjectStatusSetting(userProjectStatusSetting);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (result > 0)
            {
                row.Add("Result", result);
            }
            else
            {
                row.Add("Result", 0);
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateProjectStatusData(long ProjectStatusSettingId, string Name, int DisplayOrder)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            userProjectStatusSetting = new UserProjectStatusSetting();
            userProjectStatusSetting.UserProjectStatusSettingId = ProjectStatusSettingId;
            userProjectStatusSetting.Name = Name;
            userProjectStatusSetting.ProjectId = 0;
            userProjectStatusSetting.DisplayOrder = DisplayOrder;
            managementservice.UpdateUserProjectStatusSetting(userProjectStatusSetting);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteProjectStatusData(long ProjectStatusSettingId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);

            managementservice.DeleteUserProjectStatusSettingByUserProjectStatusSettingId(ProjectStatusSettingId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }
        //End Project Status Data ------------------------------------------------------------------------


        //Start Project Job Data ----------------------------------------------------------------------

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectJobData(long ProjectId, long CompanyId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = managementservice.GetUserProjectJobsByProjectIdUserId(ProjectId, CompanyId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadProjectJobData(long ProjectJobSettingId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            userProjectJobSetting = new UserProjectJobSetting();
            userProjectJobSetting = managementservice.GetUserProjectJobSettingByUserProjectJobSettingId(ProjectJobSettingId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (userProjectJobSetting != null)
            {
                row.Add("UserProjectJobSettingId", userProjectJobSetting.UserProjectJobSettingId);
                row.Add("Name", userProjectJobSetting.Name);
                row.Add("DisplayOrder", userProjectJobSetting.DisplayOrder);

            }
            else
            {
                row.Add("Result", "No Data found");
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveProjectJobData(long CompanyId, string Name, int DisplayOrder)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            userProjectJobSetting = new UserProjectJobSetting();
            userProjectJobSetting.UserId = CompanyId;
            userProjectJobSetting.Name = Name;
            userProjectJobSetting.ProjectId = 0;
            userProjectJobSetting.DisplayOrder = DisplayOrder;
            long result = managementservice.CreateUserProjectJobSetting(userProjectJobSetting);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (result > 0)
            {
                row.Add("Result", result);
            }
            else
            {
                row.Add("Result", 0);
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateProjectJobData(long ProjectJobSettingId, string Name, int DisplayOrder)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);
            userProjectJobSetting = new UserProjectJobSetting();
            userProjectJobSetting.UserProjectJobSettingId = ProjectJobSettingId;
            userProjectJobSetting.Name = Name;
            userProjectJobSetting.ProjectId = 0;
            userProjectJobSetting.DisplayOrder = DisplayOrder;
            managementservice.UpdateUserProjectJobSetting(userProjectJobSetting);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteProjectJobData(long ProjectJobSettingId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            //worksheetgroup = scopeservices.GetWorksheetGroupByWorksheetGroupId(GroupId);

            managementservice.DeleteUserProjectJobSettingByUserProjectJobSettingId(ProjectJobSettingId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }
        //End Project Job Data ------------------------------------------------------------------------


        //Start Contacts Data ----------------------------------------------------------------------

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetContactsDataByUserId(long UserId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = managementservice.GetUserProfilesRelatedByUserId(UserId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetContactsDataByUserIdByUsertype(long UserId, int UserType)
        {

            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = managementservice.GetUserProfilesByPartyAType(UserId, UserType);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        //Staff   =101
        //Contractors=102
        //Suppliers=103

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadContactsData(long UserId, long ContactId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            userProfile = new UserProfile();
            userProfile = managementservice.GetUserProfileByUserID(UserId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            if (userProfile != null)
            {
                row.Add("UserProfileId", userProfile.UserProfileId);
                row.Add("FirstName", userProfile.FirstName);
                row.Add("LastName", userProfile.LastName);
                row.Add("Contact1", userProfile.Contact1);
                row.Add("Contact2", userProfile.Contact2);
                row.Add("Contact3", userProfile.Contact3);
                row.Add("Email", userProfile.Email);
                row.Add("Notes", userProfile.Notes);

                DataSet dt = new DataSet();
                dt = managementservice.GetUserRelationshipByPartyAPartyB(UserId, ContactId);
                string type = "";
                try { type = dt.Tables[0].Rows[0]["Type"].ToString(); }
                catch { };

                string status = "";
                try { status = dt.Tables[0].Rows[0]["Status"].ToString(); }
                catch { };
                row.Add("Type", type);
                row.Add("Status", status);

                projectOwner = new ProjectOwner();
                projectOwner = managementservice.GetProjectOwnerByContactId(ContactId);
                projectOwner = managementservice.GetProjectOwnerByProjectOwnerId(projectOwner.ProjectOwnerId);
                row.Add("BusinessName", projectOwner.Name);
                row.Add("Address", projectOwner.Address);
                row.Add("Suburb", projectOwner.Suburb);
                row.Add("City", projectOwner.City);
                row.Add("PostCode", projectOwner.PostCode);
                row.Add("Region", projectOwner.Region);
                row.Add("Country", projectOwner.Country);
            }
            else
            {
                row.Add("Result", "No Data found");
            }
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateContactsData(long ContactId, long userid, string BusinessName, string FirstName, string LastName, string Email, string Contact1, string Contact2, string Contact3, string Address, string Suburb, string City, string PostCode, string Region, string Country, string Notes, int type, int status)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            //DataSet dsUser = new DataSet();
            //dsUser = managementservice.GetUserByEmail(Email);

            //bool userChanged = false;
            //bool passwordExisted = false;
            //if (dsUser.Tables.Count > 0)
            //{
            //    if (dsUser.Tables[0].Rows.Count > 0)
            //    {
            //        if(ContactId != long.Parse(dsUser.Tables[0].Rows[0]["UserId"].ToString()))
            //           userChanged = true;

            //        if (dsUser.Tables[0].Rows[0]["Password"] != null)
            //            passwordExisted = true;
            //    }
            //}
            //long userId;
            //if (ContactId > 0)
            //{
            //    if (passwordExisted)
            //    { }

            //}
            DataSet dsCurrentUser = new DataSet();
            dsCurrentUser = managementservice.GetUserByEmail(Email);
            projectOwner = new ProjectOwner();
            projectOwner.Name = BusinessName;
            projectOwner.Contact1 = Contact1;
            projectOwner.Contact2 = Contact2;
            projectOwner.Contact3 = Contact3;
            projectOwner.Address = Address;
            projectOwner.Suburb = Suburb;
            projectOwner.City = City;
            projectOwner.PostCode = PostCode;
            projectOwner.Region = Region;
            projectOwner.Country = Country;

            managementservice.UpdateProjectOwner(projectOwner);
            userProfile = new UserProfile();
            userProfile = managementservice.GetUserProfileByUserID(ContactId);
            userProfile.FirstName = FirstName;
            userProfile.LastName = LastName;
            userProfile.Contact1 = Contact1;
            userProfile.Contact2 = Contact2;
            userProfile.Contact3 = Contact3;
            userProfile.Email = Email;
            userProfile.Notes = Notes;
            managementservice.UpdateUserProfile(userProfile);
            if (dsCurrentUser.Tables.Count > 0)
            {
                user = new User();
                if (dsCurrentUser.Tables[0].Rows.Count > 0)
                { user.UserId = long.Parse(dsCurrentUser.Tables[0].Rows[0]["UserId"].ToString()); }
                else { user.UserId = userid; }

                user.Email = Email;
                managementservice.UpdateUserEmail(user);
            }
            managementservice.UpdateUserRelationshipType(userid, ContactId, type);
            managementservice.UpdateUserRelationshipStatus(userid, ContactId, status);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveContactsData(long userid, string BusinessName, string FirstName, string LastName, string Email, string Contact1, string Contact2, string Contact3, string Address, string Suburb, string City, string PostCode, string Region, string Country, string Notes, int type, int status)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet dsUser = new DataSet();
            dsUser = managementservice.GetUserByEmail(Email);

            long ContactId = 0;
            bool passwordExisted = false;
            if (dsUser.Tables.Count > 0)
            {
                if (dsUser.Tables[0].Rows.Count > 0)
                {
                    ContactId = long.Parse(dsUser.Tables[0].Rows[0]["UserId"].ToString());


                    if (dsUser.Tables[0].Rows[0]["Password"] != null)
                        passwordExisted = true;
                }
            }
            //long userId;
            //if (ContactId > 0)
            //{
            projectOwner = new ProjectOwner();
            projectOwner.Name = BusinessName;
            projectOwner.Contact1 = Contact1;
            projectOwner.Contact2 = Contact2;
            projectOwner.Contact3 = Contact3;
            projectOwner.Address = Address;
            projectOwner.Suburb = Suburb;
            projectOwner.City = City;
            projectOwner.PostCode = PostCode;
            projectOwner.Region = Region;
            projectOwner.Country = Country;
            long intCompanyID = managementservice.CreateProjectOwner(projectOwner);
            projectOwner.ProjectOwnerId = intCompanyID;
            vouchercodefunctions = new VoucherCodeFunctions();
            string strIdentifier = string.Format("{0}{1}", vouchercodefunctions.GenerateVoucherCodeGuid(16), intCompanyID);
            projectOwner.Identifier = strIdentifier;
            managementservice.UpdateProjectOwnerIdentifier(projectOwner);
            string NewFile;
            if (intCompanyID > 0)
            {
                //NewFile = UploadImages(intCompanyID, strIdentifier);
                //if(NewFile != string.Empty) {
                //    projectOwner.Logo = NewFile;
                //}
            }
            userProfile = new UserProfile();
            user = new User();
            long userId;
            user.Email = Email;
            user.Type = type;
            user.CompanyId = intCompanyID;

            userId = managementservice.CreateUser(user, userid);
            projectOwner.ContactId = userId;
            managementservice.UpdateProjectOwner(projectOwner);

            long userProfileId;
            userProfile.UserId = userId;
            userProfile.FirstName = FirstName;
            userProfile.LastName = LastName;
            userProfile.Contact1 = Contact1;
            userProfile.Contact2 = Contact2;
            userProfile.Contact3 = Contact3;
            userProfile.Email = Email;
            userProfile.Notes = Notes;

            userProfileId = managementservice.CreateUserProfile(userProfile);
            userProfile.UserProfileId = userProfileId;

            strIdentifier = String.Format("{0}{1}", userProfileId, vouchercodefunctions.GenerateVoucherCodeGuid(16));
            userProfile.Identifier = strIdentifier;
            managementservice.UpdateUserProfileIdentifier(userProfile);

            //NewFile = UploadImages(userId, strIdentifier);
            //if (NewFile != string.Empty) {
            //    userProfile.PersonalPhoto = NewFile; }

            managementservice.UpdateUserProfile(userProfile);
            managementservice.UpdateUserRelationshipStatus(userid, userId, status);
            //}


            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", userId);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteContactsData(long ContactId, long userid)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;

            managementservice.DeleteUserRelationshipByPartyAPartyB(userid, ContactId, false);
            managementservice.UpdateUserRelationshipInvitationAcceptDate(ContactId, userid, DateTime.Now);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }
        //End Contacts Data ------------------------------------------------------------------------


        //Start Non Archived Projects Data ------------------------------------------------------------------------

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectsGroupNonArchivedDataByUserId(long UserId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = managementservice.GetProjectGroupsNotUserArchivedByUserId(UserId, "", 2);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectsNonArchivedDataByUserId(long UserId, long ProjectGroupId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = managementservice.GetProjectsNotUserArchivedByUserIdGroupId(UserId, ProjectGroupId, "", 2);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                long CurrentProjectId = 0;
                DataSet dsUserProjectStatusValue = new DataSet();
                try
                {
                    CurrentProjectId = long.Parse(dr["ProjectId"].ToString());
                    dsUserProjectStatusValue = managementservice.GetUserProjectStatusValueByProjectIdUserId(CurrentProjectId, UserId);
                }
                catch { }
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                try
                {
                    row.Add("UserProjectStatusValue", dsUserProjectStatusValue.Tables[0].Rows[0]["UserProjectStatusValue"].ToString());
                    row.Add("UserProjectStatusName", dsUserProjectStatusValue.Tables[0].Rows[0]["Name"].ToString());
                }
                catch { row.Add("UserProjectStatusValue", 0); }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        //End Non Archived Projects Data ------------------------------------------------------------------------

        //Start Archived Projects Data --------------------------------------------------------------------------
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectsGroupArchivedDataByUserId(long UserId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = managementservice.GetProjectGroupsUserArchivedByUserId(UserId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectsArchivedDataByUserId(long UserId, long ProjectGroupId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = managementservice.GetProjectsUserArchivedByUserIdGroupId(UserId, ProjectGroupId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                long CurrentProjectId = 0;
                DataSet dsUserProjectStatusValue = new DataSet();
                try
                {
                    CurrentProjectId = long.Parse(dr["ProjectId"].ToString());

                    dsUserProjectStatusValue = managementservice.GetUserProjectStatusValueByProjectIdUserId(CurrentProjectId, UserId);
                }
                catch { }
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                try
                {

                    row.Add("UserProjectStatusValue", dsUserProjectStatusValue.Tables[0].Rows[0]["UserProjectStatusValue"].ToString());
                }
                catch { row.Add("UserProjectStatusValue", 0); }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }


        //End Archived Projects Data ------------------------------------------------------------------------

        //Start Scope Projects Data --------------------------------------------------------------------------

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectsScopeDataByUserId(long UserId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();
            //DataTable dt = new DataTable();
            gd = managementservice.GetProjectsByUserIdUserProjectStatusValue(UserId, "", -1000);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        //End Scope Projects Data ---------------------------------------------------------------------------


        //Start Projects Updates Data --------------------------------------------------------------------------

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectsStatus(long ProjectId, long UserId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();

            gd = managementservice.GetProjectStatusesByProjectIdUserId(ProjectId, UserId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateProjectPriority(long ProjectID, int rate)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            try
            {
                managementservice.UpdateProjectPriority(ProjectID, rate); row.Add("Result", 1);
            }
            catch (Exception ex)
            {
                row.Add("Result", 0);
            }

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateProjectArchivedDate(long ProjectID, long userid)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            try
            {
                Project project = new Project();
                project.ProjectId = ProjectID;
                project.ArchivedDate = DateTime.Now;

                managementservice.UpdateProjectArchivedDateByUserId(project, userid);
                row.Add("Result", 1);
            }
            catch (Exception ex)
            {
                row.Add("Result", 0);
            }

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateProjectNonArchivedDate(long ProjectID, long userid)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            try
            {
                Project project = new Project();
                project.ProjectId = ProjectID;
                //project.ArchivedDate = DateTime.Now;

                managementservice.UpdateProjectArchivedDateByUserId(project, userid);
                row.Add("Result", 1);
            }
            catch (Exception ex)
            {
                row.Add("Result", 0);
            }

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateProjectStatus(long ProjectID, long userid, int statusid)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            try
            {
                bool isExisted_ProjectStatus = false;
                UserProjectStatusValue userProjectStatusValue = new UserProjectStatusValue();
                userProjectStatusValue.UserId = userid;
                userProjectStatusValue.ProjectId = ProjectID;
                userProjectStatusValue.UserProjectStatusValue = statusid;
                DataSet dsUserProjectStatusValue = new DataSet();
                dsUserProjectStatusValue = managementservice.GetUserProjectStatusValueByProjectIdUserId(ProjectID, userid);
                if (dsUserProjectStatusValue.Tables[0].Rows.Count > 0)
                {
                    isExisted_ProjectStatus = true;
                    userProjectStatusValue.UserProjectStatusValueId = Convert.ToInt32(dsUserProjectStatusValue.Tables[0].Rows[0]["UserProjectStatusValueId"]);

                }
                if (isExisted_ProjectStatus)
                {
                    managementservice.UpdateUserProjectStatusValueByProjectIdUserId(userProjectStatusValue);
                }
                else
                {
                    managementservice.CreateUserProjectStatusValue(userProjectStatusValue);
                }

                row.Add("Result", 1);
            }
            catch (Exception ex)
            {
                row.Add("Result", 0);
            }

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DisplayNotes(long ProjectId, long UserId, DateTime ScopeDate, DateTime StartDate, DateTime AssessmentDate, string Hazard)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            string result = string.Empty;
            string strNotes = string.Empty;
            int intNotesLength = 65;
            DataSet dsNotes = new DataSet();
            DataSet dsScopeItem = new DataSet();
            long UserProjectStatusValue = 0;
            DataSet dsUserProjectStatus = new DataSet();
            dsUserProjectStatus = managementservice.GetUserProjectStatusValueByProjectIdUserId(ProjectId, UserId);
            if (dsUserProjectStatus.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsUserProjectStatus.Tables[0].Rows[0]["UserProjectStatusValue"].ToString()))
                {
                    UserProjectStatusValue = long.Parse(dsUserProjectStatus.Tables[0].Rows[0]["UserProjectStatusValue"].ToString());
                }
            }
            dsNotes = managementservice.GetUserNotesByUserIDProjectStatusID(ProjectId, UserProjectStatusValue);
            if (dsNotes.Tables.Count > 0)
            {
                if (dsNotes.Tables[0].Rows.Count > 0)
                {
                    strNotes = String.Format("{0}", dsNotes.Tables[0].Rows[0]["NoteContent"]);
                    strNotes = strNotes.Trim();
                }
            }

            switch (UserProjectStatusValue)
            {
                case -1000:
                case -100:
                    if (strNotes.Length > intNotesLength - 24)
                    {
                        result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength - 24));
                    }
                    else
                        result = String.Format("{0} {1}", result, strNotes);


                    if (StartDate.ToString() != String.Empty)
                    {
                        if (result.Trim() == String.Empty)
                        {
                            result = String.Format("<b>Start Date: {0}</b>", StartDate.ToString("dd/MM/yyyy"));
                        }
                        else
                            result = String.Format("<b>Start Date: {0}</b>, {1}", StartDate.ToString("dd/MM/yyyy"), result);
                    }
                    break;
                case 1000:
                    if (strNotes.Length > intNotesLength - 29)
                    {
                        result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength - 29));
                    }
                    else
                        result = String.Format("{0} {1}", result, strNotes);

                    if (AssessmentDate.ToString() != String.Empty)
                    {
                        if (result.Trim() == String.Empty)
                        {
                            result = String.Format("<b>Assessment Date: {0}</b>", AssessmentDate.ToString("dd/MM/yyyy"));
                        }
                        else
                            result = String.Format("<b>Assessment Date: {0}</b>, {1}", AssessmentDate.ToString("dd/MM/yyyy"), result);
                    }
                    break;
                default:
                    if (strNotes.Length > intNotesLength)
                    {
                        result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength));
                    }
                    else
                        result = String.Format("{0} {1}", result, strNotes);
                    break;

            }
            string strImgLinksHTML = string.Empty;
            if (Hazard.Trim().Length > 0)
            {
                //strImgLinksHTML = String.Format("<a class='form_popup' href='Hazard.aspx?id={0}{1}'><img src='../images/hazard.png' alt='View Hazard' width='26px' height='24px' border='0' /></a>", ProjectId, String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")));
                strImgLinksHTML = "http://koreprojects.com/images/hazard.png";
            }
            int intNotesCount = 0;
            if (dsNotes.Tables.Count > 0)
            {
                if (dsNotes.Tables[0].Rows.Count > 0)
                {
                    intNotesCount = dsNotes.Tables[0].Rows.Count;
                }
            }
            if (UserProjectStatusValue == -1000)
            {
                if (dsScopeItem.Tables.Count > 0)
                {
                    if (dsScopeItem.Tables[0].Rows.Count > 0)
                    {
                        intNotesCount = dsScopeItem.Tables[0].Rows.Count;
                    }
                }
            }
            if (intNotesCount > 0)
            {
                //strImgLinksHTML = strImgLinksHTML + "&nbsp;&nbsp;" + String.Format("<a class='form_popup' href='Notes.aspx?id={0}&sid={1}{2}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a>", ProjectId, UserProjectStatusValue, String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")));
                strImgLinksHTML = strImgLinksHTML + "&nbsp;&nbsp;" + "http://koreprojects.com/images/info.png";
            }

            DataSet dsTradeNotes = new DataSet();
            dsTradeNotes = managementservice.GetTradeNotesByUserIDProjectStatusID(ProjectId, UserProjectStatusValue);
            if (dsTradeNotes.Tables.Count > 0)
            {
                if (dsTradeNotes.Tables[0].Rows.Count > 0)
                {
                    //strImgLinksHTML = strImgLinksHTML + "&nbsp;&nbsp;" + String.Format("<a class='form_popup' href='TradeNotes.aspx?id={0}&sid={1}{2}'><img src='../images/trade-notes.png' alt='View Trade Notes' width='26px' height='24px' border='0' /></a>", ProjectId, UserProjectStatusValue, String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")));
                    strImgLinksHTML = strImgLinksHTML + "&nbsp;&nbsp;" + "http://koreprojects.com/images/trade-notes.png";
                }
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            strImgLinksHTML = strImgLinksHTML.Trim();
            if (strImgLinksHTML == String.Empty)
            {
                //result = String.Format("<table width='100%'><tr><td width='180px'>{0}</td><td align='right'></td></tr></table>", result);
                row.Add("Result", result);
                row.Add("Image", "");
                //result = result;
            }
            else
            {
                row.Add("Result", result);
                row.Add("Image", strImgLinksHTML);
                //result = String.Format("<table width='100%'><tr><td width='180px'>{0}</td><td align='right'>{1}</td></tr></table>", result, strImgLinksHTML);

            }

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectsHazard(long ProjectId, long UserId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();

            string strContentHtml = string.Empty;
            DataSet dsProject = new DataSet();
            dsProject = managementservice.GetProjectInfoByProjectId(ProjectId);
            if (dsProject.Tables.Count > 0)
            {
                if (dsProject.Tables[0].Rows.Count > 0)
                {
                    strContentHtml = String.Format("{0}", dsProject.Tables[0].Rows[0]["Hazard"]);
                }
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            row.Add("Result", strContentHtml);
            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void ShowProjectImage(string Identifier, string PersonalPhoto)
        {
            //managementservice.SQLConnection = ConnectDb.SQLConnection;
            //DataSet gd = new DataSet();

            //gd = managementservice.GetProjectStatusesByProjectIdUserId(ProjectId, UserId);
            string result = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();
            //if (File.Exists(String.Format("{0}\\images\\{1}\\{2}", "http://koreprojects.com", Identifier, PersonalPhoto)))
            if (Identifier != string.Empty && PersonalPhoto != string.Empty)
            {
                result = String.Format("http://koreprojects.com/images/{0}/{1}", Identifier, PersonalPhoto);
            }
            else
                result = String.Format("http://koreprojects.com/images/house.jpg");

            row.Add("Result", result);

            tableRows.Add(row);


            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void SaveProjectsData(long userid, string FirstName, string LastName, string Email, string Contact1, string Contact2, string Contact3, string Address, string Suburb, int SuburbId, string City, int CityId, string PostCode, string Region, int RegionId, string Country, int CountryId, string ProjectName, string ClaimNumber, string EstimatedTime, DateTime StartDate, DateTime ScopeDate, DateTime AssessmentDate, DateTime QuotationDate, DateTime FinishDate, string ProjectGroupName, int ProjectGroupId, int Priority, string Hazard, int status, string imagestring)
        {
            long userProfileId;

            try
            {
                managementservice.SQLConnection = ConnectDb.SQLConnection;
                UserProfile contactProfile = new UserProfile();
                Project project = new Project();
                User contactUser = new User();
                long userId;
                contactUser.Email = Email.Trim();
                contactUser.Type = 0;
                userId = managementservice.CreateUser(contactUser, userid);


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


                String NewFile = UploadImages(userId, strIdentifier, imagestring);

                contactProfile.PersonalPhoto = NewFile;

                managementservice.UpdateUserProfile(contactProfile);

                project.ContactId = userId;
                project.ProjectOwnerId = managementservice.GetProjectOwnerByContactId(userid).ProjectOwnerId;
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
                userProjectStatusValue.UserId = userid;
                userProjectStatusValue.UserProjectStatusValue = status;
                managementservice.CreateUserProjectStatusValue(userProjectStatusValue);
                int projectCredit = 0;

                try
                {
                    DataSet dsUserAccount = new DataSet();
                    dsUserAccount = managementservice.GetUserAccountByUserID(userid);
                    if (dsUserAccount.Tables[0].Rows.Count > 0)
                    {
                        projectCredit = int.Parse(dsUserAccount.Tables[0].Rows[0]["ProjectCredit"].ToString());
                    }

                    if (projectCredit > 0)
                    {
                        managementservice.UpdateUserAccount(userid, projectCredit - 1);
                        managementservice.CreateUserTransaction(userid, String.Format("Create Project", project.Name), 0, 0, -1, projectCredit - 1);
                    }

                }
                catch (Exception w)
                { }

                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Clear();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                row = new Dictionary<string, object>();
                row.Add("Result", userProfileId);
                tableRows.Add(row);

                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
            }
            catch (Exception ex)
            {
                SqlCommand m_SqlCommand;
                DataSet result = new DataSet();
                SqlConnection conn = new SqlConnection("Data Source=184.168.194.78;Initial Catalog=A4PP_Phase_Dev2;User ID=A4PPUser;Password=Ktex758@");
                m_SqlCommand = new SqlCommand();
                m_SqlCommand.Connection = conn;
                m_SqlCommand.CommandText = "USP_ErrorLog_Insert";
                m_SqlCommand.CommandType = CommandType.StoredProcedure;
                m_SqlCommand.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                m_SqlCommand.Parameters.AddWithValue("@StackTrace", ex.StackTrace);
                conn.Open();
                m_SqlCommand.ExecuteNonQuery();
                conn.Close();

                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Clear();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                row = new Dictionary<string, object>();
                row.Add("Result", "Error:" + ex.StackTrace);
                tableRows.Add(row);

                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));

            }
        }

        private string UploadImages(long userid, string Email, string base64String)
        {
            string NewFileName = string.Empty;
            try
            {

                string strUserFileDescription = string.Empty;
                // Convert Base64 String to byte[]
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    // Convert byte[] to Image
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    Image imgFile = Image.FromStream(ms, true);
                    int x = 0;
                    int y = 0;
                    int newX = 0;
                    int newY = 0;
                    x = imgFile.Width;
                    y = imgFile.Height;

                    newX = x;
                    newY = y;

                    byte[] uploadedFile = new byte[ms.Length];
                    uploadedFile = ms.ToArray();
                    ms.Close();

                    NewFileName = String.Format("{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    if (userid > 0)
                    {
                        strUserFileDescription = String.Format("{0}\\images\\{1}\\{2}.jpg", System.Configuration.ConfigurationManager.AppSettings["ProjectPath"], Email, NewFileName);
                        if (!System.IO.Directory.Exists(String.Format("{0}\\images\\{1}", System.Configuration.ConfigurationManager.AppSettings["ProjectPath"], Email)))
                        {
                            System.IO.Directory.CreateDirectory(String.Format("{0}\\images\\{1}", System.Configuration.ConfigurationManager.AppSettings["ProjectPath"], Email));
                        }

                        FileStream wFile;
                        wFile = new FileStream(strUserFileDescription, FileMode.Create);
                        wFile.Write(uploadedFile, 0, uploadedFile.Length);
                        wFile.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return NewFileName + ".jpg";
        }

        private void UploadFiles(long userid, long ProjectID, string details, string FileType, byte[] imageBytes)
        {
            string NewFileName = string.Empty;
            string strUserFileDescription = string.Empty;
            string projectpath = "http://koreprojects.com";
            UserProfile ContactUserProfile = new UserProfile();
            Project CurrentProject = new Project();
            if (ProjectID > 0)
            {
                CurrentProject = managementservice.GetProjectByProjectId(ProjectID);
                ContactUserProfile = managementservice.GetUserProfileByUserID(CurrentProject.ContactId);

                if (FileType == "gif" || FileType == "jpeg" || FileType == "pjpeg" || FileType == "png" || FileType == "bmp")
                {
                    int limitX = 800;
                    int limitY = 600;
                    int x = 0;
                    int y = 0;
                    int newX = 0;
                    int newY = 0;
                    var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);

                    System.Drawing.Image imgFile;
                    imgFile = Image.FromStream(ms, true);

                    x = imgFile.Width;
                    y = imgFile.Height;

                    if (x > limitX || y > limitY)
                    {
                        if (x * 1.0 / y >= limitX * 1.0 / limitY)
                        {
                            newX = limitX;
                            newY = Convert.ToInt32((y * (limitX * 1.0 / x)));
                        }
                        else
                        {
                            newY = limitY;
                            newX = Convert.ToInt32((x * (limitY * 1.0 / y)));
                        }
                    }
                    else
                    {
                        newX = x;
                        newY = y;
                    }
                    byte[] uploadedFile = new byte[ms.Length];
                    uploadedFile = ms.ToArray();
                    ms.Close();
                    NewFileName = String.Format("{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    if (ProjectID > 0)
                    {
                        strUserFileDescription = String.Format("{0}\\images\\{1}\\{2}.jpg", projectpath, ContactUserProfile.Identifier, NewFileName);
                        if (!System.IO.Directory.Exists(String.Format("{0}\\images\\{1}", projectpath, ContactUserProfile.Identifier)))
                        {
                            System.IO.Directory.CreateDirectory(String.Format("{0}\\images\\{1}", projectpath, ContactUserProfile.Identifier));
                        }
                    }
                    FileStream wFile;
                    wFile = new FileStream(strUserFileDescription, FileMode.Create);
                    wFile.Write(uploadedFile, 0, uploadedFile.Length);
                    wFile.Close();
                    UserFile CurrentFile = new UserFile();
                    if (ProjectID > 0)
                    {
                        CurrentFile.Owner = ProjectID;
                    }
                    CurrentFile.FileName = NewFileName;
                    CurrentFile.FileExtension = "jpg";
                    CurrentFile.Description = details.Trim();
                    managementservice.CreateUserFile(CurrentFile);


                }
                else if (FileType == "pdf")
                {


                    NewFileName = String.Format("{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));

                    //BinaryReader binReader = new BinaryReader(File.Open(Server.MapPath(filename), FileMode.Open, FileAccess.Read));
                    //binReader.BaseStream.Position = 0;
                    byte[] binFile = imageBytes; //binReader.ReadBytes(Convert.ToInt32(binReader.BaseStream.Length));
                    //binReader.Close();

                    if (ProjectID > 0)
                    {
                        strUserFileDescription = String.Format("{0}\\images\\{1}\\{2}.pdf", projectpath, ContactUserProfile.Identifier, NewFileName);
                        if (!System.IO.Directory.Exists(String.Format("{0}\\images\\{1}", projectpath, ContactUserProfile.Identifier))) ;
                        {
                            System.IO.Directory.CreateDirectory(String.Format("{0}\\images\\{1}", projectpath, ContactUserProfile.Identifier));
                        }
                    }

                    //Txt_FileUpload.PostedFile.SaveAs(strUserFileDescription)

                    UserFile CurrentFile = new UserFile();
                    if (ProjectID > 0)
                    {
                        CurrentFile.Owner = ProjectID;
                    }
                    CurrentFile.FileName = NewFileName;
                    CurrentFile.FileExtension = "pdf";
                    CurrentFile.Description = details;
                    managementservice.CreateUserFile(CurrentFile);
                }
            }


        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SubmitProjectsData(long userid, string FirstName, string LastName, string Email, string Contact1, string Contact2, string Contact3, string Address, string Suburb, int SuburbId, string City, int CityId, string PostCode, string Region, int RegionId, string Country, int CountryId, string ProjectName, string ClaimNumber, string EstimatedTime, DateTime StartDate, DateTime ScopeDate, DateTime AssessmentDate, DateTime QuotationDate, DateTime FinishDate, string ProjectGroupName, int ProjectGroupId, int Priority, string Hazard, int status, string FileType, string imageFile)
        {
            long userProfileId;

            try
            {
                HttpContext postedContext = HttpContext.Current;
                HttpFileCollection Request = postedContext.Request.Files;

                managementservice.SQLConnection = ConnectDb.SQLConnection;
                UserProfile contactProfile = new UserProfile();
                Project project = new Project();
                User contactUser = new User();
                long userId;
                contactUser.Email = Email.Trim();
                contactUser.Type = 0;
                userId = managementservice.CreateUser(contactUser, userid);

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
                string fname = "";

                if (FileType == "gif" || FileType == "jpeg" || FileType == "pjpeg" || FileType == "png" || FileType == "bmp")
                {
                    int limitX = 800;
                    int limitY = 600;
                    int x = 0;
                    int y = 0;
                    int newX = 0;
                    int newY = 0;
                    byte[] imageBytes = Convert.FromBase64String(imageFile);
                    var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);

                    System.Drawing.Image imgFile;
                    imgFile = Image.FromStream(ms, true);

                    x = imgFile.Width;
                    y = imgFile.Height;

                    if (x > limitX || y > limitY)
                    {
                        if (x * 1.0 / y >= limitX * 1.0 / limitY)
                        {
                            newX = limitX;
                            newY = Convert.ToInt32((y * (limitX * 1.0 / x)));
                        }
                        else
                        {
                            newY = limitY;
                            newX = Convert.ToInt32((x * (limitY * 1.0 / y)));
                        }
                    }
                    else
                    {
                        newX = x;
                        newY = y;
                    }
                    byte[] uploadedFile = new byte[ms.Length];
                    uploadedFile = ms.ToArray();
                    ms.Close();
                    fname = String.Format("{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    string strUserFileDescription = String.Format("{0}\\images\\{1}\\{2}.jpg", System.Configuration.ConfigurationManager.AppSettings["ProjectPath"], contactProfile.Identifier, fname);
                    if (!System.IO.Directory.Exists(String.Format("{0}\\images\\{1}", System.Configuration.ConfigurationManager.AppSettings["ProjectPath"], contactProfile.Identifier)))
                    {
                        System.IO.Directory.CreateDirectory(String.Format("{0}\\images\\{1}", System.Configuration.ConfigurationManager.AppSettings["ProjectPath"], contactProfile.Identifier));
                    }
                    FileStream wFile;
                    wFile = new FileStream(strUserFileDescription, FileMode.Create);
                    wFile.Write(uploadedFile, 0, uploadedFile.Length);
                    wFile.Close();
                }

                //foreach (HttpPostedFile item in Request)
                //{
                //    string filename = item.FileName;
                //    NewFile = filename;
                //    byte[] fileBytes = new byte[item.ContentLength];
                //    item.InputStream.Read(fileBytes, 0, item.ContentLength);

                //    string projectpath = "http://koreprojects.com";
                //    string folderPath = postedContext.Server.MapPath(projectpath + "/Images/" + contactProfile.Identifier);
                //    if (!System.IO.Directory.Exists(folderPath))
                //    {
                //        System.IO.Directory.CreateDirectory(folderPath);
                //    }
                //    fname = postedContext.Server.MapPath(projectpath + "/Images/" + contactProfile.Identifier + "/" + filename);
                //    item.SaveAs(fname);
                //}
                contactProfile.PersonalPhoto = fname + ".jpg";

                managementservice.UpdateUserProfile(contactProfile);

                project.ContactId = userId;
                project.ProjectOwnerId = managementservice.GetProjectOwnerByContactId(userid).ProjectOwnerId;
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
                userProjectStatusValue.UserId = userid;
                userProjectStatusValue.UserProjectStatusValue = status;
                managementservice.CreateUserProjectStatusValue(userProjectStatusValue);
                int projectCredit = 0;

                try
                {
                    DataSet dsUserAccount = new DataSet();
                    dsUserAccount = managementservice.GetUserAccountByUserID(userid);
                    if (dsUserAccount.Tables[0].Rows.Count > 0)
                    {
                        projectCredit = int.Parse(dsUserAccount.Tables[0].Rows[0]["ProjectCredit"].ToString());
                    }

                    if (projectCredit > 0)
                    {
                        managementservice.UpdateUserAccount(userid, projectCredit - 1);
                        managementservice.CreateUserTransaction(userid, String.Format("Create Project", project.Name), 0, 0, -1, projectCredit - 1);
                    }

                }
                catch (Exception w)
                { }

                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Clear();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                row = new Dictionary<string, object>();

                row.Add("Result", userProfileId);

                tableRows.Add(row);

                return serializer.Serialize(new { Response = tableRows });
            }
            catch (Exception ex)
            {
                SQLConn m_SQLConn = new SQLConn();
                SqlConnection m_SqlConnection = m_SQLConn.conn();
                SqlCommand m_SqlCommand;
                DataSet result = new DataSet();
                SqlConnection conn = m_SQLConn.conn();
                String m_SQL = "Data Source=184.168.194.78;Initial Catalog=A4PP_Phase_Dev2;User ID=A4PPUser;Password=Ktex758@";
                m_SqlCommand = new SqlCommand(m_SQL, conn);
                m_SqlCommand.CommandText = "USP_ErrorLog_Insert";
                m_SqlCommand.CommandType = CommandType.StoredProcedure;
                m_SqlCommand.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                m_SqlCommand.Parameters.AddWithValue("@StackTrace", ex.StackTrace);
                conn.Open();
                m_SqlCommand.ExecuteNonQuery();
                conn.Close();

                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Clear();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                row = new Dictionary<string, object>();
                row.Add("Result", ex.StackTrace);
                tableRows.Add(row);

                return serializer.Serialize(new { Response = tableRows });

            }
        }




        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UploadPdf(long userid, long ProjectID, string details, byte[] fileBytes)
        {
            string NewFileName = string.Empty;
            try
            {
                UserProfile ContactUserProfile = new UserProfile();
                Project CurrentProject = new Project();
                NewFileName = String.Format("{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                string projectpath = "http://koreprojects.com";

                CurrentProject = managementservice.GetProjectByProjectId(ProjectID);
                ContactUserProfile = managementservice.GetUserProfileByUserID(CurrentProject.ContactId);
                string strUserFileDescription = String.Format("{0}\\images\\{1}\\{2}.pdf", projectpath, ContactUserProfile.Identifier, NewFileName);
                if (!System.IO.Directory.Exists(String.Format("{0}\\images\\{1}", projectpath, ContactUserProfile.Identifier)))
                {
                    System.IO.Directory.CreateDirectory(String.Format("{0}\\images\\{1}", projectpath, ContactUserProfile.Identifier));
                }

                //if (Offset == 0) // new file, create an empty file
                // File.Create(FilePath).Close();
                // open a file stream and write the buffer. 
                // Don't open with FileMode.Append because the transfer may wish to 
                // start a different point
                using (FileStream fs = new FileStream(strUserFileDescription, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Write(fileBytes, 0, fileBytes.Length);
                }
                UserFile CurrentFile = new UserFile();
                if (ProjectID > 0)
                {
                    CurrentFile.Owner = ProjectID;
                }
                CurrentFile.FileName = NewFileName;
                CurrentFile.FileExtension = "pdf";
                CurrentFile.Description = details;
                managementservice.CreateUserFile(CurrentFile);
                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Clear();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                row = new Dictionary<string, object>();

                row.Add("Result", 1);

                tableRows.Add(row);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
            }
            catch (Exception ex)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Clear();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                row = new Dictionary<string, object>();

                row.Add("Result", 0);

                tableRows.Add(row);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UploadImg(long userid, long ProjectID, string details, byte[] fileBytes)
        {
            string NewFileName = string.Empty;
            try
            {
                UserProfile ContactUserProfile = new UserProfile();
                Project CurrentProject = new Project();
                NewFileName = String.Format("{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                string projectpath = "http://koreprojects.com";

                CurrentProject = managementservice.GetProjectByProjectId(ProjectID);
                ContactUserProfile = managementservice.GetUserProfileByUserID(CurrentProject.ContactId);
                int limitX = 800;
                int limitY = 600;
                int x = 0;
                int y = 0;
                int newX = 0;
                int newY = 0;
                var ms = new MemoryStream(fileBytes, 0, fileBytes.Length);
                ms.Write(fileBytes, 0, fileBytes.Length);

                System.Drawing.Image imgFile;
                imgFile = Image.FromStream(ms, true);

                x = imgFile.Width;
                y = imgFile.Height;

                if (x > limitX || y > limitY)
                {
                    if (x * 1.0 / y >= limitX * 1.0 / limitY)
                    {
                        newX = limitX;
                        newY = Convert.ToInt32((y * (limitX * 1.0 / x)));
                    }
                    else
                    {
                        newY = limitY;
                        newX = Convert.ToInt32((x * (limitY * 1.0 / y)));
                    }
                }
                else
                {
                    newX = x;
                    newY = y;
                }
                byte[] uploadedFile = new byte[ms.Length];
                uploadedFile = ms.ToArray();
                ms.Close();
                string strUserFileDescription = string.Empty;
                if (ProjectID > 0)
                {
                    strUserFileDescription = String.Format("{0}\\images\\{1}\\{2}.jpg", projectpath, ContactUserProfile.Identifier, NewFileName);
                    if (!System.IO.Directory.Exists(String.Format("{0}\\images\\{1}", projectpath, ContactUserProfile.Identifier)))
                    {
                        System.IO.Directory.CreateDirectory(String.Format("{0}\\images\\{1}", projectpath, ContactUserProfile.Identifier));
                    }
                }

                FileStream wFile;
                wFile = new FileStream(strUserFileDescription, FileMode.Create);
                wFile.Write(uploadedFile, 0, uploadedFile.Length);
                wFile.Close();

                UserFile CurrentFile = new UserFile();
                if (ProjectID > 0)
                {
                    CurrentFile.Owner = ProjectID;
                }
                CurrentFile.FileName = NewFileName;
                CurrentFile.FileExtension = "pdf";
                CurrentFile.Description = details;
                managementservice.CreateUserFile(CurrentFile);

                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Clear();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                row = new Dictionary<string, object>();

                row.Add("Result", 1);

                tableRows.Add(row);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
            }
            catch (Exception ex)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Clear();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                row = new Dictionary<string, object>();

                row.Add("Result", 0);

                tableRows.Add(row);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetFiles(long ProjectId)
        {
            string projectpath = "http://koreprojects.com/images/";
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();

            gd = managementservice.GetUserFileByUserID(ProjectId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                Project CurrentProject = new Project();
                CurrentProject = managementservice.GetProjectByProjectId(long.Parse(dr["Owner"].ToString()));
                UserProfile CurrentContact = managementservice.GetUserProfileByUserID(CurrentProject.ContactId);
                //if (dr["FileExtension"].ToString().ToUpper()=="PDF")
                {
                    row.Add("Description", dr["Description"]);
                    row.Add("File", projectpath + CurrentContact.Identifier + "/" + dr["FileName"]);//Description
                }

                tableRows.Add(row);
            }

            //foreach (DataRow dr in gd.Tables[0].Rows)
            //{
            //    row = new Dictionary<string, object>();
            //    foreach (DataColumn col in gd.Tables[0].Columns)
            //    {
            //        row.Add(col.ColumnName, dr[col].ToString());
            //    }
            //    tableRows.Add(row);
            //}

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }


        //private void resizeimage(Image objImage, int width, int height, MemoryStream imgStream)
        //{
        //    System.Drawing.Image.GetThumbnailImageAbort dummyCallBack;
        //    dummyCallBack=new Image.GetThumbnailImageAbort(
        //}

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectDetails(long UserId, long ProjectId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet dsProject = new DataSet();
            //DataTable dt = new DataTable();
            dsProject = managementservice.GetProjectInfoByProjectId(ProjectId);
            UserProfile ContactUserProfile = new UserProfile();

            ContactUserProfile = managementservice.GetUserProfileByUserID(long.Parse(dsProject.Tables[0].Rows[0]["ContactId"].ToString()));

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();


            row.Add("FirstName", ContactUserProfile.FirstName);
            row.Add("LastName", ContactUserProfile.LastName);
            row.Add("Contact1", ContactUserProfile.Contact1);
            row.Add("Contact2", ContactUserProfile.Contact2);
            row.Add("Contact3", ContactUserProfile.Contact3);
            row.Add("Email", ContactUserProfile.Email);
            if (ContactUserProfile.PersonalPhoto != String.Empty)
            {
                row.Add("PersonalPhoto", String.Format("http://koreprojects.com/images/{0}/{1}", ContactUserProfile.Identifier, ContactUserProfile.PersonalPhoto));
                row.Add("ScopePhoto", String.Format("http://koreprojects.com/images/{0}/{1}", ContactUserProfile.Identifier, ContactUserProfile.PersonalPhoto));
            }
            else
            {
                row.Add("PersonalPhoto", "http://koreprojects.com/images/house.jpg");
                row.Add("ScopePhoto", "http://koreprojects.com/images/house.jpg");
            }
            row.Add("Address", dsProject.Tables[0].Rows[0]["Address"].ToString());
            row.Add("Suburb", dsProject.Tables[0].Rows[0]["Suburb"].ToString());
            row.Add("City", dsProject.Tables[0].Rows[0]["City"].ToString());
            row.Add("Postcode", dsProject.Tables[0].Rows[0]["Postcode"].ToString());
            row.Add("Region", dsProject.Tables[0].Rows[0]["Region"].ToString());
            row.Add("Country", dsProject.Tables[0].Rows[0]["Country"].ToString());

            row.Add("projectname", dsProject.Tables[0].Rows[0]["Name"].ToString());
            row.Add("projectnametitle", dsProject.Tables[0].Rows[0]["Name"].ToString());
            row.Add("scopenametitle", dsProject.Tables[0].Rows[0]["Name"].ToString());

            row.Add("EQCClaimNumber", dsProject.Tables[0].Rows[0]["EQCClaimNumber"].ToString());
            row.Add("EstimatedTime", dsProject.Tables[0].Rows[0]["EstimatedTime"].ToString());
            row.Add("ScopeDate", dsProject.Tables[0].Rows[0]["ScopeDate"].ToString());
            row.Add("StartDate", dsProject.Tables[0].Rows[0]["StartDate"].ToString());
            row.Add("AssessmentDate", dsProject.Tables[0].Rows[0]["AssessmentDate"].ToString());
            row.Add("QuotationDate", dsProject.Tables[0].Rows[0]["QuotationDate"].ToString());

            row.Add("GroupName", dsProject.Tables[0].Rows[0]["GroupName"].ToString());
            row.Add("GroupID", dsProject.Tables[0].Rows[0]["GroupID"].ToString());
            row.Add("FinishDate", dsProject.Tables[0].Rows[0]["FinishDate"].ToString());

            DataSet dsUserProjectStatus = new DataSet();
            dsUserProjectStatus = managementservice.GetUserProjectStatusValueByProjectIdUserId(ProjectId, UserId);
            if (dsUserProjectStatus.Tables[0].Rows.Count > 0)
            {
                if (dsUserProjectStatus.Tables[0].Rows[0]["UserProjectStatusValue"] != null)
                {
                    row.Add("projectstatus", dsUserProjectStatus.Tables[0].Rows[0]["Name"].ToString());
                    try
                    {
                        row.Add("UserProjectStatusValue", dsUserProjectStatus.Tables[0].Rows[0]["UserProjectStatusValue"].ToString());
                    }
                    catch { }
                    row.Add("scopestatus", dsUserProjectStatus.Tables[0].Rows[0]["Name"].ToString());
                }
            }
            else
            {
                row.Add("projectstatus", "");
                row.Add("scopestatus", "");
            }

            row.Add("Priority", dsProject.Tables[0].Rows[0]["Priority"].ToString());
            row.Add("Hazard", dsProject.Tables[0].Rows[0]["Hazard"].ToString());

            tableRows.Add(row);

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateProjectsData(long ProjectId, long userid, string FirstName, string LastName, string Email, string Contact1, string Contact2, string Contact3, string Address, string Suburb, int SuburbId, string City, int CityId, string PostCode, string Region, int RegionId, string Country, int CountryId, string ProjectName, string ClaimNumber, string EstimatedTime, DateTime StartDate, DateTime ScopeDate, DateTime AssessmentDate, DateTime QuotationDate, DateTime FinishDate, string ProjectGroupName, int ProjectGroupId, int Priority, string Hazard, int status)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            UserProfile contactProfile = new UserProfile();
            Project project = new Project();
            project = managementservice.GetProjectByProjectId(ProjectId);
            contactProfile = managementservice.GetUserProfileByUserID(project.ContactId);

            contactProfile.FirstName = FirstName.Trim();
            contactProfile.LastName = LastName.Trim();
            contactProfile.Contact1 = Contact1.Trim();
            contactProfile.Contact2 = Contact2.Trim();
            contactProfile.Contact3 = Contact3.Trim();
            contactProfile.Email = Email.Trim();

            managementservice.UpdateUserProfile(contactProfile);

            //project.ProjectOwnerId = managementservice.GetProjectOwnerByContactId(userid).ProjectOwnerId;
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

            managementservice.UpdateProject(project);

            UserProjectStatusValue userProjectStatusValue = new UserProjectStatusValue();
            userProjectStatusValue.ProjectId = project.ProjectId;
            userProjectStatusValue.UserId = userid;
            userProjectStatusValue.UserProjectStatusValue = status;
            managementservice.UpdateUserProjectStatusValueByProjectIdUserId(userProjectStatusValue);


            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteProjectsData(long ProjectId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            UserProfile contactProfile = new UserProfile();
            Project project = new Project();
            project.ProjectId = ProjectId;
            project.DeactivatedDate = DateTime.Now;
            managementservice.UpdateProjectDeactivated(project);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        //End Projects Updates Data --------------------------------------------------------------------------

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectStatusesByProjectIdUserId(long ProjectID, long UserId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();

            gd = managementservice.GetProjectStatusesByProjectIdUserId(ProjectID, UserId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectsJob(long UserId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();

            gd = managementservice.GetJobsByProjectId(UserId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveProjectJob(long userid, long ProjectID, int ProjectJobSettings, string JobName, string Description, string DueDate, bool chk, long CompanyId, string Title, int Status)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            Job CurrentJob = new Job();
            CurrentJob.JobName = JobName;
            CurrentJob.Description = Description;
            //DateTime.TryParse(DueDate, CurrentJob.DueDate);
            if (ProjectJobSettings == 0)
            {
                if (chk == true)
                {
                    UserProjectJobSetting objUserProjectJobSetting = new UserProjectJobSetting();
                    long intProjectJobSettingID = 0;
                    objUserProjectJobSetting.UserId = CompanyId;
                    objUserProjectJobSetting.ProjectId = 0;
                    objUserProjectJobSetting.Name = Title.Trim();
                    int intDisplayOrder = 0;
                    objUserProjectJobSetting.DisplayOrder = intDisplayOrder;
                    intProjectJobSettingID = managementservice.CreateUserProjectJobSetting(objUserProjectJobSetting);
                    if (intProjectJobSettingID > 0)
                    {
                        objUserProjectJobSetting = new UserProjectJobSetting();
                        objUserProjectJobSetting = managementservice.GetUserProjectJobSettingByUserProjectJobSettingId(intProjectJobSettingID);
                        CurrentJob.JobValue = objUserProjectJobSetting.JobValue;
                    }
                }
            }
            else
            {
                CurrentJob.JobValue = ProjectJobSettings;
            }

            CurrentJob.ProjectId = ProjectID;
            CurrentJob.ProjectOwnerId = CompanyId;
            CurrentJob.Status = Status;
            JobStatus jobstat = (JobStatus)Status;
            long m_CurrentJobID = managementservice.CreateJob(CurrentJob);

            managementservice.UpdateJobStatus(m_CurrentJobID, jobstat);
            JobAssignment objJobAssignment = new JobAssignment();
            objJobAssignment.JobId = m_CurrentJobID;
            objJobAssignment.ProjectId = ProjectID;
            objJobAssignment.ProjectOwnerId = CompanyId;
            objJobAssignment.UserId = userid;
            managementservice.CreateJobAssignment(objJobAssignment);


            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateProjectJob(long userid, long ProjectID, int JobID, int ProjectJobSettings, string JobName, string Description, string DueDate, bool chk, long CompanyId, string Title, int Status)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            Job CurrentJob = new Job();
            CurrentJob.JobId = JobID;
            CurrentJob.JobName = JobName;
            CurrentJob.Description = Description;
            //DateTime.TryParse(DueDate, CurrentJob.DueDate);
            if (ProjectJobSettings == 0)
            {
                if (chk == true)
                {
                    UserProjectJobSetting objUserProjectJobSetting = new UserProjectJobSetting();
                    long intProjectJobSettingID = 0;
                    objUserProjectJobSetting.UserId = CompanyId;
                    objUserProjectJobSetting.ProjectId = 0;
                    objUserProjectJobSetting.Name = Title.Trim();
                    int intDisplayOrder = 0;
                    objUserProjectJobSetting.DisplayOrder = intDisplayOrder;
                    intProjectJobSettingID = managementservice.CreateUserProjectJobSetting(objUserProjectJobSetting);
                    if (intProjectJobSettingID > 0)
                    {
                        objUserProjectJobSetting = new UserProjectJobSetting();
                        objUserProjectJobSetting = managementservice.GetUserProjectJobSettingByUserProjectJobSettingId(intProjectJobSettingID);
                        CurrentJob.JobValue = objUserProjectJobSetting.JobValue;
                    }
                }
            }
            else
            {
                CurrentJob.JobValue = ProjectJobSettings;
            }

            CurrentJob.ProjectId = ProjectID;
            CurrentJob.ProjectOwnerId = CompanyId;
            CurrentJob.Status = Status;
            JobStatus jobstat = (JobStatus)Status;
            managementservice.UpdateJob(CurrentJob);
            managementservice.UpdateJobStatus(JobID, jobstat);




            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteProjectJob(int JobID)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            managementservice.DeleteJobByJobId(JobID, false);


            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }





        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectsNotesData(long UserId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();

            gd = managementservice.GetUserNoteByUserID(UserId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadProjectsNotesData(long NoteID)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet dsUserFile = new DataSet();

            dsUserFile = managementservice.GetUserNoteByUserNoteID(NoteID);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("NoteID", NoteID);
            try
            {
                row.Add("ProjectStatusId", dsUserFile.Tables[0].Rows[0]["ProjectStatusId"]);
            }
            catch
            {
                row.Add("ProjectStatusId", "-1");
            }
            row.Add("Message", dsUserFile.Tables[0].Rows[0]["NoteContent"]);
            row.Add("Title", dsUserFile.Tables[0].Rows[0]["Description"]);

            tableRows.Add(row);

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveProjectNotesData(long userid, long ProjectID, string Title, string Message, int ProjectStatus)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            UserNote CurrentNote = new UserNote();
            CurrentNote.Author = userid;
            CurrentNote.Owner = ProjectID;
            CurrentNote.Description = Title.Trim();
            CurrentNote.NoteContent = Message.Trim();
            CurrentNote.ProjectStatusId = ProjectStatus;

            managementservice.CreateUserNote(CurrentNote);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateProjectNotesData(long userid, int NoteID, long ProjectID, string Title, string Message, int ProjectStatus)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            UserNote CurrentNote = new UserNote();
            CurrentNote.UserNoteId = NoteID;
            CurrentNote.Owner = ProjectID;
            CurrentNote.Description = Title.Trim();
            CurrentNote.NoteContent = Message.Trim();
            CurrentNote.ProjectStatusId = ProjectStatus;
            managementservice.UpdateUserNote(CurrentNote);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteProjectNotesData(long userid, int NoteID, long ProjectID)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;

            managementservice.DeleteUserNoteByUserNoteIDOwner(NoteID, ProjectID);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }




        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetProjectsTradesData(long ProjectId, long UserId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet gd = new DataSet();

            gd = managementservice.GetTradeNoteByUserID(ProjectId);
            DataSet gdscope = new DataSet();

            gdscope = managementservice.GetProjectStatusesByProjectIdUserId(ProjectId, UserId);
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                string statusname = "General";
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                foreach (DataRow drs in gdscope.Tables[0].Rows)
                {
                    if (Convert.ToInt32(drs["StatusValue"].ToString()) == Convert.ToInt32(dr["ProjectStatusId"].ToString()))
                    {
                        statusname = drs["Name"].ToString();
                    }
                }
                row.Add("ProjectStatusNameNew", statusname);
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadProjectsTradesData(long TradeNoteID)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            DataSet dsUserFile = new DataSet();

            dsUserFile = managementservice.GetTradeNoteByUserNoteID(TradeNoteID);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("TradeNoteID", TradeNoteID);
            try
            {
                row.Add("ProjectStatusId", dsUserFile.Tables[0].Rows[0]["ProjectStatusId"]);
            }
            catch
            {
                row.Add("ProjectStatusId", "-1");
            }
            row.Add("Message", dsUserFile.Tables[0].Rows[0]["NoteContent"]);
            row.Add("Title", dsUserFile.Tables[0].Rows[0]["Description"]);

            tableRows.Add(row);

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveProjectTradesData(long userid, long ProjectID, string Title, string Message, int ProjectStatus)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            UserNote CurrentNote = new UserNote();
            CurrentNote.Owner = ProjectID;
            CurrentNote.Description = Title.Trim();
            CurrentNote.NoteContent = Message.Trim();
            CurrentNote.ProjectStatusId = ProjectStatus;

            managementservice.CreateTradeNote(CurrentNote);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateProjectTradesData(long userid, int TradesNoteID, long ProjectID, string Title, string Message, int ProjectStatus)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            UserNote CurrentNote = new UserNote();
            CurrentNote.UserNoteId = TradesNoteID;
            CurrentNote.Owner = ProjectID;
            CurrentNote.Description = Title.Trim();
            CurrentNote.NoteContent = Message.Trim();
            CurrentNote.ProjectStatusId = ProjectStatus;
            managementservice.UpdateTradeNote(CurrentNote);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteProjectTradesData(long userid, int TradesNoteID, long ProjectID)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;

            managementservice.DeleteTradeNoteByUserNoteIDOwner(TradesNoteID, ProjectID);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }





        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWorkPendingData(long ProjectID, long UserId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            Scope objScope = new Scope();
            objScope.ProjectId = ProjectID;
            objScope.GSTRate = 0;
            long ScopeID = scopeservices.CreateScope(objScope);

            DataSet gd = new DataSet();

            gd = scopeservices.GetWorksheetGroupsByScopeIdScopeItemStatusUserId(ScopeID, 1, UserId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWorkPendingDetailData(long ProjectID, long UserId, long WorksheetGroupId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            Scope objScope = new Scope();
            objScope.ProjectId = ProjectID;
            objScope.GSTRate = 0;
            long ScopeID = scopeservices.CreateScope(objScope);

            DataSet gd = new DataSet();
            gd = scopeservices.GetScopeItemsByScopeIdScopeItemStatusUserIdGroupId(ScopeID, 1, UserId, WorksheetGroupId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                string result = string.Empty;
                string strNotes = string.Empty;
                int intNotesLength = 40;
                string ScopeItemId = string.Empty;
                string AssignTo = string.Empty;
                ScopeItem scopeItem = new ScopeItem();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    if (col.ColumnName == "ScopeItemId")
                    {
                        ScopeItemId = dr[col].ToString();
                    }
                    if (col.ColumnName == "AssignTo")
                    {
                        AssignTo = dr[col].ToString();
                    }
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                scopeItem = scopeservices.GetScopeItemByScopeItemId(Convert.ToInt32(ScopeItemId));
                strNotes = String.Format("{0}", scopeItem.Description);
                if (AssignTo.Trim() != String.Empty)
                {
                    strNotes = String.Format("{0} - {1}", AssignTo, strNotes);
                }
                if (strNotes.Length > intNotesLength)
                {
                    result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength));
                }
                else
                {
                    result = String.Format("{0} {1}", result, strNotes);
                }
                row.Add("Notes", result);
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWorkApprovedData(long ProjectID, long UserId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            Scope objScope = new Scope();
            objScope.ProjectId = ProjectID;
            objScope.GSTRate = 0;
            long ScopeID = scopeservices.CreateScope(objScope);

            DataSet gd = new DataSet();
            gd = scopeservices.GetWorksheetGroupsByScopeIdScopeItemStatusUserId(ScopeID, 2, UserId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWorkApprovedDetailData(long ProjectID, long UserId, long WorksheetGroupId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            Scope objScope = new Scope();
            objScope.ProjectId = ProjectID;
            objScope.GSTRate = 0;
            long ScopeID = scopeservices.CreateScope(objScope);

            DataSet gd = new DataSet();
            gd = scopeservices.GetScopeItemsByScopeIdScopeItemStatusUserIdGroupId(ScopeID, 2, UserId, WorksheetGroupId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                string result = string.Empty;
                string strNotes = string.Empty;
                int intNotesLength = 40;
                string ScopeItemId = string.Empty;
                string AssignTo = string.Empty;
                ScopeItem scopeItem = new ScopeItem();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    if (col.ColumnName == "ScopeItemId")
                    {
                        ScopeItemId = dr[col].ToString();
                    }
                    if (col.ColumnName == "AssignTo")
                    {
                        AssignTo = dr[col].ToString();
                    }
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                scopeItem = scopeservices.GetScopeItemByScopeItemId(Convert.ToInt32(ScopeItemId));
                strNotes = String.Format("{0}", scopeItem.Description);
                if (AssignTo.Trim() != String.Empty)
                {
                    strNotes = String.Format("{0} - {1}", AssignTo, strNotes);
                }
                if (strNotes.Length > intNotesLength)
                {
                    result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength));
                }
                else
                {
                    result = String.Format("{0} {1}", result, strNotes);
                }
                row.Add("Notes", result);
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void ApproveWorkSheetData(long ScopeID, long ScopeItemId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;

            scopeservices.UpdateScopeItemApproveByScopeItemId(ScopeItemId);
            scopeservices.UpdateTotal(ScopeID);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DisApproveWorkSheetData(long ScopeID, long ScopeItemId)
        {
            scopeservices.SQLConnection = ConnectDb.SQLConnection;

            scopeservices.UpdateScopeItemDisapproveByScopeItemId(ScopeItemId);
            scopeservices.UpdateTotal(ScopeID);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWorkGroupData(long ProjectID)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            Scope objScope = new Scope();
            objScope.ProjectId = ProjectID;
            objScope.GSTRate = 0;
            long ScopeID = scopeservices.CreateScope(objScope);

            long m_ProjectId = scopeservices.GetScopeByScopeId(ScopeID).ProjectId;


            long m_ProjectOwnerId = managementservice.GetProjectByProjectId(m_ProjectId).ProjectOwnerId;
            long m_ProjectOwnerUserId = managementservice.GetProjectOwnerByProjectOwnerId(m_ProjectOwnerId).ContactId;

            DataSet gd = new DataSet();
            gd = scopeservices.GetWorksheetGroupsByProjectOwnerId(m_ProjectOwnerId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWorkAreaData(long ProjectID)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            Scope objScope = new Scope();
            objScope.ProjectId = ProjectID;
            objScope.GSTRate = 0;
            long ScopeID = scopeservices.CreateScope(objScope);
            long m_ProjectId = scopeservices.GetScopeByScopeId(ScopeID).ProjectId;


            long m_ProjectOwnerId = managementservice.GetProjectByProjectId(m_ProjectId).ProjectOwnerId;
            long m_ProjectOwnerUserId = managementservice.GetProjectOwnerByProjectOwnerId(m_ProjectOwnerId).ContactId;

            DataSet gd = new DataSet();
            gd = scopeservices.GetAreasByProjectOwnerId(m_ProjectOwnerId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWorkItemData(long ProjectID)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            Scope objScope = new Scope();
            objScope.ProjectId = ProjectID;
            objScope.GSTRate = 0;
            long ScopeID = scopeservices.CreateScope(objScope);
            long m_ProjectId = scopeservices.GetScopeByScopeId(ScopeID).ProjectId;


            long m_ProjectOwnerId = managementservice.GetProjectByProjectId(m_ProjectId).ProjectOwnerId;
            long m_ProjectOwnerUserId = managementservice.GetProjectOwnerByProjectOwnerId(m_ProjectOwnerId).ContactId;

            DataSet gd = new DataSet();
            gd = scopeservices.GetItemsByProjectOwnerId(m_ProjectOwnerId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWorkServiceData(long ProjectID)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            Scope objScope = new Scope();
            objScope.ProjectId = ProjectID;
            objScope.GSTRate = 0;
            long ScopeID = scopeservices.CreateScope(objScope);

            long m_ProjectId = scopeservices.GetScopeByScopeId(ScopeID).ProjectId;


            long m_ProjectOwnerId = managementservice.GetProjectByProjectId(m_ProjectId).ProjectOwnerId;
            long m_ProjectOwnerUserId = managementservice.GetProjectOwnerByProjectOwnerId(m_ProjectOwnerId).ContactId;

            DataSet gd = new DataSet();
            gd = scopeservices.GetWorksheetServicesByProjectOwnerId(m_ProjectOwnerId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWorkStatusData()
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            scopeservices.SQLConnection = ConnectDb.SQLConnection;

            DataSet gd = new DataSet();
            gd = scopeservices.GetScopeItemStatuses();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void GetWorkMeasurementData()
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            scopeservices.SQLConnection = ConnectDb.SQLConnection;

            DataSet gd = new DataSet();
            gd = scopeservices.GetUnits();

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;

            foreach (DataRow dr in gd.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in gd.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                tableRows.Add(row);
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void SaveWorkSheetData(long ProjectID, string ScopeGroup, long ScopeGroupId, string Area, string AreaMeasurement, string Item, string WorksheetService, string AssignTo, int AssignToId, string service, string Description, int ScopeItemStatusId, decimal Quantity, string Unit, decimal Rate, long ScopeId, decimal Cost, long ServiceGroupId, int DisplayOrder)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            Scope objScope = new Scope();
            objScope.ProjectId = ProjectID;
            objScope.GSTRate = 0;
            long ScopeID = scopeservices.CreateScope(objScope);

            ScopeItem objScopeItem = new ScopeItem();
            objScopeItem.ProjectId = ProjectID;
            objScopeItem.ScopeId = ScopeID;
            objScopeItem.ScopeGroup = ScopeGroup;
            objScopeItem.ScopeGroupId = ScopeGroupId;
            objScopeItem.Area = Area;
            objScopeItem.AreaMeasurement = AreaMeasurement;
            objScopeItem.Item = Item;
            objScopeItem.WorksheetService = WorksheetService;
            objScopeItem.AssignTo = AssignTo;
            objScopeItem.AssignToId = AssignToId;
            objScopeItem.Service = service;
            objScopeItem.Description = Description;
            objScopeItem.ScopeItemStatusId = ScopeItemStatusId;
            objScopeItem.Quantity = Quantity;
            objScopeItem.Unit = Unit;
            objScopeItem.Rate = Rate;
            //objScopeItem.ScopeId = ScopeId;
            objScopeItem.Cost = Cost;
            objScopeItem.ServiceGroupId = ServiceGroupId;
            objScopeItem.DisplayOrder = DisplayOrder;
            scopeservices.CreateScopeItem(objScopeItem);
            scopeservices.UpdateTotal(objScopeItem.ScopeId);


            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void LoadWorkSheetData(long ScopeItemId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            ScopeItem objScopeItem = new ScopeItem();
            objScopeItem = scopeservices.GetScopeItemByScopeItemId(ScopeItemId);


            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            //Context.Response.ContentType = "application/json; charset=utf-8";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();


            row.Add("ScopeGroup", objScopeItem.ScopeGroup);
            row.Add("Area", objScopeItem.Area);
            row.Add("Item", objScopeItem.Item);
            row.Add("WorksheetService", objScopeItem.WorksheetService);
            row.Add("AssignTo", objScopeItem.AssignTo);
            row.Add("Service", objScopeItem.Service);
            row.Add("ServiceGroupId", objScopeItem.ServiceGroupId);
            row.Add("AreaMeasurement", objScopeItem.AreaMeasurement);
            row.Add("AssignToId", objScopeItem.AssignToId);
            row.Add("Description", objScopeItem.Description);
            row.Add("ScopeItemStatusId", objScopeItem.ScopeItemStatusId);
            row.Add("Quantity", objScopeItem.Quantity);
            row.Add("Unit", objScopeItem.Unit);
            row.Add("Rate", objScopeItem.Rate);
            row.Add("DisplayOrder", objScopeItem.DisplayOrder);

            tableRows.Add(row);

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateWorkSheetData(long ProjectID, long ScopeItemId, string ScopeGroup, long ScopeGroupId, string Area, string AreaMeasurement, string Item, string WorksheetService, string AssignTo, int AssignToId, string service, string Description, int ScopeItemStatusId, decimal Quantity, string Unit, decimal Rate, long ScopeId, decimal Cost, long ServiceGroupId, int DisplayOrder)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            scopeservices.SQLConnection = ConnectDb.SQLConnection;
            Scope objScope = new Scope();
            objScope.ProjectId = ProjectID;
            objScope.GSTRate = 0;
            long ScopeID = scopeservices.CreateScope(objScope);

            ScopeItem objScopeItem = new ScopeItem();
            objScopeItem.ScopeItemId = ScopeItemId;
            objScopeItem.ProjectId = ProjectID;
            objScopeItem.ScopeId = ScopeID;
            objScopeItem.ScopeGroup = ScopeGroup;
            objScopeItem.ScopeGroupId = ScopeGroupId;
            objScopeItem.Area = Area;
            objScopeItem.AreaMeasurement = AreaMeasurement;
            objScopeItem.Item = Item;
            objScopeItem.WorksheetService = WorksheetService;
            objScopeItem.AssignTo = AssignTo;
            objScopeItem.AssignToId = AssignToId;
            objScopeItem.Service = service;
            objScopeItem.Description = Description;
            objScopeItem.ScopeItemStatusId = ScopeItemStatusId;
            objScopeItem.Quantity = Quantity;
            objScopeItem.Unit = Unit;
            objScopeItem.Rate = Rate;
            //objScopeItem.ScopeId = ScopeId;
            objScopeItem.Cost = Cost;
            objScopeItem.ServiceGroupId = ServiceGroupId;
            objScopeItem.DisplayOrder = DisplayOrder;
            scopeservices.UpdateScopeItem(objScopeItem);
            scopeservices.UpdateTotal(objScopeItem.ScopeId);


            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteWorkSheetData(long ProjectID, long ScopeItemId)
        {
            managementservice.SQLConnection = ConnectDb.SQLConnection;
            scopeservices.SQLConnection = ConnectDb.SQLConnection;

            ScopeItem objScopeItem = new ScopeItem();
            objScopeItem = scopeservices.GetScopeItemByScopeItemId(ScopeItemId);
            scopeservices.DeleteScopeItemByScopeItemId(ScopeItemId);
            scopeservices.UpdateTotal(objScopeItem.ScopeId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
            Dictionary<String, Object> row;
            row = new Dictionary<string, object>();

            row.Add("Result", 1);

            tableRows.Add(row);
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(serializer.Serialize(new { Response = tableRows }));
        }
    }
}
