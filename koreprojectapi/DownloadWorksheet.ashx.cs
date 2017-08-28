using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spire;
using Spire.DataExport.XLS;
using Warpfusion.Shared.Utilities;
using Warpfusion.A4PP.Services;
using Warpfusion.A4PP.Objects;
using System.Data;
using Microsoft.VisualBasic;

namespace koreprojectapi
{
    /// <summary>
    /// Summary description for DownloadWorksheet
    /// </summary>
    public class DownloadWorksheet : IHttpHandler
    {
        CellExport cellExport = new CellExport();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
            string id = context.Request.QueryString["ScopeId"];
            Cryption objCryption= new Cryption();
            long lngScopeId=long.Parse(id);
            GenerateSpreadsheetforGroupedWorksheet(lngScopeId, context);
        }
        private string GenerateSpreadsheetforGroupedWorksheet(long ScopeId, HttpContext context)
        {
            String result = String.Empty;
            ScopeServices objScopeService= new ScopeServices();
            objScopeService.SQLConnection = ConnectDb.SQLConnection;
            ManagementService objManagementService = new ManagementService();
            objManagementService.SQLConnection = ConnectDb.SQLConnection;

            Scope objScope = objScopeService.GetScopeByScopeId(ScopeId);
            Project objProject = objManagementService.GetProjectByProjectId(objScope.ProjectId);
            DataSet dsQuery = new DataSet();
            int intFootTotalStartRow;
            int intFootTotalStartColum ;
            int intCompanyInfoStartRow;
            int intCompanyInfoStartColum;
            String strExcelFile;
            String strNewExcelFileName;
            DataSet dsProjectGroup  = new DataSet();
             String strClaimNo   = "";
            String strClaimant   = "";
            String strSiteLocation   = "";
            String strEQRSupervisor   = "";
            String strContractorEmail   = "";
            String strScopeDate   = "";
            String strContractor   = "";
            String strAddress   = "";
            String strAccreditationNo   = "";
            String strContractorPhone   = "";
            String strGSTNo   = "";
            String strTotalPrice   = "";
            DataSet dsProject=new DataSet();
            ProjectOwner projectOwner =new ProjectOwner();
            String strLogo = "";

             dsProjectGroup = objScopeService.GetProjectGroupsByProjectOwnerId(objProject.ProjectOwnerId);
             dsProject = objManagementService.GetProjectInfoByProjectId(objScope.ProjectId);

             //get project group email
            if (dsProjectGroup.Tables.Count > 0 ){
                    if (dsProjectGroup.Tables[0].Rows.Count > 0 ){
                    if (!Convert.IsDBNull(dsProjectGroup.Tables[0].Rows[0]["Email"])){
                        strContractorEmail = dsProjectGroup.Tables[0].Rows[0]["Email"].ToString();
                    }
                    }
            }
                  //get claimant detail
            if (! Convert.IsDBNull(dsProject.Tables[0].Rows[0]["EQCClaimNumber"])){
                strClaimNo = dsProject.Tables[0].Rows[0]["EQCClaimNumber"].ToString();
             }
            if(!Convert.IsDBNull(dsProject.Tables[0].Rows[0]["ScopeDate"])){
                strScopeDate = dsProject.Tables[0].Rows[0]["ScopeDate"].ToString();
            }
           if(!Convert.IsDBNull(dsProject.Tables[0].Rows[0]["ScopeDate"])){
               strScopeDate = Convert.ToDateTime(dsProject.Tables[0].Rows[0]["ScopeDate"]).ToString("dd/MM/yyyy");
             }

           if(! Convert.IsDBNull(dsProject.Tables[0].Rows[0]["Address"])){
                strSiteLocation = dsProject.Tables[0].Rows[0]["Address"].ToString();
            }

            if(! Convert.IsDBNull(dsProject.Tables[0].Rows[0]["ContactName"])){
                strClaimant = dsProject.Tables[0].Rows[0]["ContactName"].ToString();
            }
                 
            //get project owner detail
            projectOwner = objManagementService.GetProjectOwnerByProjectOwnerId(objProject.ProjectOwnerId);
            strEQRSupervisor = projectOwner.EQRSupervisor;
            strContractor = projectOwner.Name;
            strContractorPhone = projectOwner.Contact1;
            strAddress = projectOwner.Address;
            strGSTNo = projectOwner.GSTNumber;
            strAccreditationNo = projectOwner.AccreditationNumber;
            String ApprovedexcludeGstCost = objScope.Cost.ToString("c");
            String ApprovedGSTCost  = (objScope.Total - objScope.Cost).ToString("c");
            String ApprovedInGSTCost= objScope.Total.ToString("c");

            String GrandExGSTCost= (objScope.Cost1 + objScope.Cost).ToString("c");
            String GrandInGSTCost = (objScope.Total1 + objScope.Total).ToString("c");

            strTotalPrice = ApprovedInGSTCost;

            //Company Logo
            strLogo = String.Format("{0}/Images/{1}/{2}", "http://koreprojects.com", projectOwner.Identifier, projectOwner.Logo);

            strNewExcelFileName = String.Empty;

            String strExcelFileNameTemp  = Strings.StrConv(strSiteLocation, VbStrConv.ProperCase);
            int ascChar;
            for (int index = 0; index < strExcelFileNameTemp.Length - 1; index++)
			{
                ascChar = Strings.Asc(strExcelFileNameTemp[index]);
                if ((ascChar >= 65 && ascChar <= 90) || (ascChar >= 97 && ascChar <= 122) || (ascChar >= 48 && ascChar <= 57))
                {
                    strNewExcelFileName = strNewExcelFileName + strExcelFileNameTemp[index];
                }
                else
                {
                    strNewExcelFileName = strNewExcelFileName + "-";
                }
			}
            do
	        {
                strNewExcelFileName = Strings.Replace(strNewExcelFileName, "--", "-");
	        } while (strNewExcelFileName.Contains("--"));
            if (strNewExcelFileName.Substring(0, 1) == "-")
            {
                strNewExcelFileName = strNewExcelFileName.Substring(1);
            }
            if (strNewExcelFileName.Substring(strNewExcelFileName.Length - 1) == "-")
            {
                strNewExcelFileName = strNewExcelFileName.Substring(0, strNewExcelFileName.Length - 1);
            }
            if (strNewExcelFileName.Length > 20)
            {
                strNewExcelFileName = strNewExcelFileName.Substring(0, 20);
            }
            strExcelFile = String.Format("../Downloads/{0}/{1}.xls", "ExcelExport", strNewExcelFileName);

            if ( (!System.IO.Directory.Exists(String.Format("../Downloads/{0}", "ExcelExport")))){
                System.IO.Directory.CreateDirectory(String.Format("../Downloads/{0}",  "ExcelExport"));
            }

            WorkSheet workSheet = new WorkSheet();
             //initialzing the cellexport tool
            cellExport = new Spire.DataExport.XLS.CellExport();
            cellExport.ActionAfterExport = Spire.DataExport.Common.ActionType.OpenView;
            //culture format setting
            cellExport.DataFormats.CultureName = "en-NZ";
            cellExport.DataFormats.Currency = "#,###,##0.00";
            cellExport.DataFormats.DateTime = "dd/MM/yyyy H:mm";
            cellExport.DataFormats.Float = "#,###,##0.00";
            cellExport.DataFormats.Integer = "#,###,##0";
            cellExport.DataFormats.Time = "H:mm";
             //set up file name and location
            cellExport.FileName = strExcelFile;

            //outlook formating
            cellExport.SheetOptions.AggregateFormat.Font.Name = "Arial";
            cellExport.SheetOptions.CustomDataFormat.Font.Name = "Arial";
            cellExport.SheetOptions.DefaultFont.Name = "Arial";
            cellExport.SheetOptions.FooterFormat.Font.Name = "Arial";
            cellExport.SheetOptions.HeaderFormat.Font.Name = "Arial";
            cellExport.SheetOptions.HyperlinkFormat.Font.Color = Spire.DataExport.XLS.CellColor.Blue;
            cellExport.SheetOptions.HyperlinkFormat.Font.Name = "Arial";
            cellExport.SheetOptions.HyperlinkFormat.Font.Underline = Spire.DataExport.XLS.XlsFontUnderline.Single;
            cellExport.SheetOptions.NoteFormat.Alignment.Horizontal = Spire.DataExport.XLS.HorizontalAlignment.Left;
            cellExport.SheetOptions.NoteFormat.Alignment.Vertical = Spire.DataExport.XLS.VerticalAlignment.Top;
            cellExport.SheetOptions.NoteFormat.Font.Bold = true;
            cellExport.SheetOptions.NoteFormat.Font.Name = "Tahoma";
            cellExport.SheetOptions.NoteFormat.Font.Size = 8.0F;
            cellExport.SheetOptions.TitlesFormat.FillStyle.Background = Spire.DataExport.XLS.CellColor.Gray40Percent;
            cellExport.SheetOptions.TitlesFormat.Font.Bold = true;
            cellExport.SheetOptions.TitlesFormat.Font.Name = "Arial";

             //worksheet culture format
            workSheet.FormatsExport.CultureName = "en-NZ";
            workSheet.FormatsExport.Currency = "#,###,##0.00";
            workSheet.FormatsExport.DateTime = "dd/MM/yyyy H:mm";
            workSheet.FormatsExport.Float = "#,###,##0.00";
            workSheet.FormatsExport.Integer = "#,###,##0";
            workSheet.FormatsExport.Time = "H:mm";

             //worksheet outlook format;
            workSheet.Options.AggregateFormat.Font.Name = "Arial";
            workSheet.Options.CustomDataFormat.Font.Name = "Arial";
            workSheet.Options.DefaultFont.Name = "Arial";
            workSheet.Options.FooterFormat.Font.Name = "Arial";
            workSheet.Options.HeaderFormat.Font.Name = "Arial";
            workSheet.Options.HyperlinkFormat.Font.Color = Spire.DataExport.XLS.CellColor.Blue;
            workSheet.Options.HyperlinkFormat.Font.Name = "Arial";
            workSheet.Options.HyperlinkFormat.Font.Underline = Spire.DataExport.XLS.XlsFontUnderline.Single;
            workSheet.Options.NoteFormat.Alignment.Horizontal = Spire.DataExport.XLS.HorizontalAlignment.Left;
            workSheet.Options.NoteFormat.Alignment.Vertical = Spire.DataExport.XLS.VerticalAlignment.Top;
            workSheet.Options.NoteFormat.Font.Bold = true;
            workSheet.Options.NoteFormat.Font.Name = "Tahoma";
            workSheet.Options.NoteFormat.Font.Size = 8.0F;

            workSheet.Options.TitlesFormat.Font.Bold = true;
            workSheet.Options.TitlesFormat.Font.Name = "Arial";

            workSheet.Options.CustomDataFormat.Borders.Bottom.Color = Spire.DataExport.XLS.CellColor.Blue;
            workSheet.Options.CustomDataFormat.Borders.Left.Color = Spire.DataExport.XLS.CellColor.Blue;
            workSheet.Options.CustomDataFormat.Borders.Right.Color = Spire.DataExport.XLS.CellColor.Blue;
            workSheet.Options.CustomDataFormat.Borders.Top.Color = Spire.DataExport.XLS.CellColor.Blue;
            workSheet.SheetName = "Worksheet";
            workSheet.AutoFitColWidth = true;

            StripStyle stripStyle1  = new Spire.DataExport.XLS.StripStyle();

            Cell mcell = new Spire.DataExport.XLS.Cell();
            
            mcell.Column = 1;
            mcell.Row = 2;
            if( dsProjectGroup.Tables.Count > 0){
                if (dsProjectGroup.Tables[0].Rows.Count > 0 )
                {
                    mcell.Value = String.Format("{0} - Contractor's Quote", dsProjectGroup.Tables[0].Rows[0]["Name"]);
                }
                else
                    mcell.Value = String.Format("Contractor's Quote");
                
            }
            else
            {
                mcell.Value = String.Format("Contractor's Quote");
            }
            mcell.CellType = Spire.DataExport.XLS.CellType.String;
             workSheet.HeaderRows = 15;
            workSheet.Cells.Add(mcell);

            intCompanyInfoStartRow = 8;
            intCompanyInfoStartColum = 1;

            CellPicture pic1 = new Spire.DataExport.XLS.CellPicture();
            pic1.FileName = strLogo; //need to find out where the logo is
            pic1.Name = "Logo";

            cellExport.Pictures.Add(pic1);

            CellImage img1 = new Spire.DataExport.XLS.CellImage();
            img1.Column = 3;
            img1.PictureName = "Logo";
            img1.Row = 1;
            workSheet.Images.Add(img1);

            Cell mcell1 = new Spire.DataExport.XLS.Cell();
            mcell1.Column = intCompanyInfoStartColum;
            mcell1.Row = intCompanyInfoStartRow;
            mcell1.Value = "Claim No:";
            mcell1.CellType = CellType.String;
            workSheet.Cells.Add(mcell1);


            Cell mcell2 = new Spire.DataExport.XLS.Cell();
            mcell2.Column = intCompanyInfoStartColum;
            mcell2.Row = intCompanyInfoStartRow + 1;
            mcell2.Value = "Claimant:";
            mcell2.CellType = CellType.String;
            workSheet.Cells.Add(mcell2);


            Cell mcell3 = new Spire.DataExport.XLS.Cell();
            mcell3.Column = intCompanyInfoStartColum;
            mcell3.Row = intCompanyInfoStartRow + 2;
            mcell3.Value = "Site Location:";
            mcell3.CellType = CellType.String;
            workSheet.Cells.Add(mcell3);


            Cell mcell4 = new Spire.DataExport.XLS.Cell();
            mcell4.Column = intCompanyInfoStartColum;
            mcell4.Row = intCompanyInfoStartRow + 3;
            mcell4.Value = "EQR Supervisor:";
            mcell4.CellType = CellType.String;
            workSheet.Cells.Add(mcell4);


            Cell mcell5 = new Spire.DataExport.XLS.Cell();
            mcell5.Column = intCompanyInfoStartColum;
            mcell5.Row = intCompanyInfoStartRow + 4;
            mcell5.Value = "Contractor E-mail:";
            mcell5.CellType = CellType.String;
            workSheet.Cells.Add(mcell5);


            Cell mcell6 = new Spire.DataExport.XLS.Cell();
            mcell6.Column = intCompanyInfoStartColum;
            mcell6.Row = intCompanyInfoStartRow + 5;
            mcell6.Value = "Date:";
            mcell6.CellType = CellType.String;
            workSheet.Cells.Add(mcell6);

            //'add contact detail head field contet
            //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Cell mcell1_2 = new Spire.DataExport.XLS.Cell();
            mcell1_2.Column = intCompanyInfoStartColum + 1;
            mcell1_2.Row = intCompanyInfoStartRow;
            mcell1_2.Value = strClaimNo;
            mcell1_2.CellType = CellType.String;
            workSheet.Cells.Add(mcell1_2);

            Cell mcell2_2 = new Spire.DataExport.XLS.Cell();
            mcell2_2.Column = intCompanyInfoStartColum + 1;
            mcell2_2.Row = intCompanyInfoStartRow + 1;
            mcell2_2.Value = strClaimant;
            mcell2_2.CellType = CellType.String;
            workSheet.Cells.Add(mcell2_2);


            Cell mcell3_2 = new Spire.DataExport.XLS.Cell();
            mcell3_2.Column = intCompanyInfoStartColum + 1;
            mcell3_2.Row = intCompanyInfoStartRow + 2;
            mcell3_2.Value = strSiteLocation;
            mcell3_2.CellType = CellType.String;
            workSheet.Cells.Add(mcell3_2);


            Cell mcell4_2 = new Spire.DataExport.XLS.Cell();
            mcell4_2.Column = intCompanyInfoStartColum + 1;
            mcell4_2.Row = intCompanyInfoStartRow + 3;
            mcell4_2.Value = strEQRSupervisor;
            mcell4_2.CellType = CellType.String;
            workSheet.Cells.Add(mcell4_2);


            Cell mcell5_2 = new Spire.DataExport.XLS.Cell();
            mcell5_2.Column = intCompanyInfoStartColum + 1;
            mcell5_2.Row = intCompanyInfoStartRow + 4;
            mcell5_2.Value = strContractorEmail;
            mcell5_2.CellType = CellType.String;
            workSheet.Cells.Add(mcell5_2);


            Cell mcell6_2 = new Spire.DataExport.XLS.Cell();
            mcell6_2.Column = intCompanyInfoStartColum + 1;
            mcell6_2.Row = intCompanyInfoStartRow + 5;
           // 'mcell6_2.Value = IIf(strScopeDate.Trim() = String.Empty, Today.ToString("dd/MM/yyyy"), strScopeDate)
            mcell6_2.Value = DateTime.Now.ToString("dd/MM/yyyy");
            mcell6_2.CellType = CellType.String;
            workSheet.Cells.Add(mcell6_2);

        //'second part of heading
        //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Cell mcell1_3 = new Spire.DataExport.XLS.Cell();
            mcell1_3.Column = intCompanyInfoStartColum + 2;
            mcell1_3.Row = intCompanyInfoStartRow;
            mcell1_3.Value = "Contractor:";
            mcell1_3.CellType = CellType.String;

            workSheet.Cells.Add(mcell1_3);


            Cell mcell2_3 = new Spire.DataExport.XLS.Cell();
            mcell2_3.Column = intCompanyInfoStartColum + 2;
            mcell2_3.Row = intCompanyInfoStartRow + 1;
            mcell2_3.Value = "Address:";
            mcell2_3.CellType = CellType.String;
            workSheet.Cells.Add(mcell2_3);


            Cell mcell3_3 = new Spire.DataExport.XLS.Cell();
            mcell3_3.Column = intCompanyInfoStartColum + 2;
            mcell3_3.Row = intCompanyInfoStartRow + 2;
            mcell3_3.Value = "Accreditation No:";
            mcell3_3.CellType = CellType.String;
            workSheet.Cells.Add(mcell3_3);


           Cell mcell4_3 = new Spire.DataExport.XLS.Cell();
            mcell4_3.Column = intCompanyInfoStartColum + 2;
            mcell4_3.Row = intCompanyInfoStartRow + 3;
            mcell4_3.Value = "Contractor Phone:";
            mcell4_3.CellType = CellType.String;
            workSheet.Cells.Add(mcell4_3);

            Cell mcell5_3 = new Spire.DataExport.XLS.Cell();
            mcell5_3.Column = intCompanyInfoStartColum + 2;
            mcell5_3.Row = intCompanyInfoStartRow + 4;
            mcell5_3.Value = "GST No:";
            mcell5_3.CellType = CellType.String;
            workSheet.Cells.Add(mcell5_3);


            Cell mcell6_3 = new Spire.DataExport.XLS.Cell();
            mcell6_3.Column = intCompanyInfoStartColum + 2;
            mcell6_3.Row = intCompanyInfoStartRow + 5;
            mcell6_3.Value = "Total Price (incl. GST):";
            mcell6_3.CellType = CellType.String;
            workSheet.Cells.Add(mcell6_3);

            //'second part of heading
           // '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Cell mcell1_4 = new Spire.DataExport.XLS.Cell();
            mcell1_4.Column = intCompanyInfoStartColum + 3;
            mcell1_4.Row = intCompanyInfoStartRow;
            mcell1_4.Value = strContractor;
            mcell1_4.CellType = CellType.String;
            workSheet.Cells.Add(mcell1_4);


            Cell mcell2_4 = new Spire.DataExport.XLS.Cell();
            mcell2_4.Column = intCompanyInfoStartColum + 3;
            mcell2_4.Row = intCompanyInfoStartRow + 1;
            mcell2_4.Value = strAddress;
            mcell2_4.CellType = CellType.String;
            workSheet.Cells.Add(mcell2_4);


            Cell mcell3_4 = new Spire.DataExport.XLS.Cell();
            mcell3_4.Column = intCompanyInfoStartColum + 3;
            mcell3_4.Row = intCompanyInfoStartRow + 2;
            mcell3_4.Value = strAccreditationNo;
            mcell3_4.CellType = CellType.String;
            workSheet.Cells.Add(mcell3_4);

            Cell mcell4_4 = new Spire.DataExport.XLS.Cell();
            mcell4_4.Column = intCompanyInfoStartColum + 3;
            mcell4_4.Row = intCompanyInfoStartRow + 3;
            mcell4_4.Value = strContractorPhone;
            mcell4_4.CellType = CellType.String;
            workSheet.Cells.Add(mcell4_4);

            Cell mcell5_4 = new Spire.DataExport.XLS.Cell();
            mcell5_4.Column = intCompanyInfoStartColum + 3;
            mcell5_4.Row = intCompanyInfoStartRow + 4;
            mcell5_4.Value = strGSTNo;
            mcell5_4.CellType = CellType.String;
            workSheet.Cells.Add(mcell5_4);

            Cell mcell6_4 = new Spire.DataExport.XLS.Cell();
            mcell6_4.Column = intCompanyInfoStartColum + 3;
            mcell6_4.Row = intCompanyInfoStartRow + 5;
            mcell6_4.Value = strTotalPrice;
            mcell6_4.CellType = CellType.String;
            workSheet.Cells.Add(mcell6_4);

            dsQuery = objScopeService.GetScopeItemsByScopeIdScopeItemStatus(ScopeId, 2);
            dsQuery.Tables[0].DefaultView.Sort = "ScopeGroup";
            //DataRowView rowView= new DataRowView();
           
            int intRowIndex;
            int intColumIndex;
            intRowIndex = 15;
            intColumIndex = 0;

            String strPrevScopeGroup = "****";
            String strScopeGroup = String.Empty;
            String strPrevArea  = "*****";
            String strArea  = String.Empty;
            String strAreaMeasurement  = String.Empty;
            Boolean blnAreaMeasurementFilled  = false;

            Cell dynamiccell_1 ;

            intRowIndex = intRowIndex + 1;
            int Range1  = intRowIndex;

            foreach (DataRowView rowView in dsQuery.Tables[0].DefaultView)
	        {
                DataRow row = rowView.Row;
                 strScopeGroup = String.Format("{0}", row["ScopeGroup"]);
                 if (strScopeGroup != strPrevScopeGroup)
                 {
                    intRowIndex = intRowIndex + 1;
                    dynamiccell_1 = new Spire.DataExport.XLS.Cell();
                    dynamiccell_1.Column = 1;
                    dynamiccell_1.Row = intRowIndex;
                    dynamiccell_1.Value = String.Format("{0}", row["ScopeGroup"]);
                    dynamiccell_1.CellType = CellType.String;
                    workSheet.Cells.Add(dynamiccell_1);

                    dynamiccell_1 = new Spire.DataExport.XLS.Cell();
                    dynamiccell_1.Column = 2;
                    dynamiccell_1.Row = intRowIndex;
                    dynamiccell_1.Value = "Description of Works";
                    dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String;
                    workSheet.Cells.Add(dynamiccell_1);

                    dynamiccell_1 = new Spire.DataExport.XLS.Cell();
                    dynamiccell_1.Column = 3;
                    dynamiccell_1.Row = intRowIndex;
                    dynamiccell_1.Value = "Dimension";
                    dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String;
                    workSheet.Cells.Add(dynamiccell_1);

                    dynamiccell_1 = new Spire.DataExport.XLS.Cell();
                    dynamiccell_1.Column = 4;
                    dynamiccell_1.Row = intRowIndex;
                    dynamiccell_1.Value = "$ Rate";
                    dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String;
                    workSheet.Cells.Add(dynamiccell_1);

                    dynamiccell_1 = new Spire.DataExport.XLS.Cell();
                    dynamiccell_1.Column = 5;
                    dynamiccell_1.Row = intRowIndex;
                    dynamiccell_1.Value = "Contractors Quote";
                    dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String;
                    workSheet.Cells.Add(dynamiccell_1);

                    intRowIndex = intRowIndex + 1;

                 }
                 strPrevScopeGroup = strScopeGroup;

                strArea = String.Format("{0}", row["Area"]);
                strAreaMeasurement = String.Format("{0}", row["AreaMeasurement"]);

                dynamiccell_1 = new Spire.DataExport.XLS.Cell();
                dynamiccell_1.Column = 1;
                dynamiccell_1.Row = intRowIndex;
                if (strArea != strPrevArea) {
                dynamiccell_1.Value = strArea;
                blnAreaMeasurementFilled = false;
                } 
                else
                if (blnAreaMeasurementFilled ){
                    dynamiccell_1.Value = "-";}
                else
                {
                    if (strAreaMeasurement.Trim() == String.Empty) {
                        dynamiccell_1.Value = "-";}
                    else{
                        dynamiccell_1.Value = strAreaMeasurement;}
                                    
                    blnAreaMeasurementFilled =true;
                            
                }
                strPrevArea = strArea ;           
                dynamiccell_1.CellType = CellType.String;
                workSheet.Cells.Add(dynamiccell_1);

                 //'Note
                dynamiccell_1 = new Spire.DataExport.XLS.Cell();
                dynamiccell_1.Column = 2;
                dynamiccell_1.Row = intRowIndex;

                if (!Convert.IsDBNull(row["Description"]))
                {
                    dynamiccell_1.Value = row["Description"];
                }
                else
                {
                    dynamiccell_1.Value = "";
                }
                if (!Convert.IsDBNull(row["Item"]))
                {
                    if (String.Format("{0}", row["Item"]).Trim() != String.Empty)
                    {
                        dynamiccell_1.Value = String.Format("{0}: {1}", row["Item"], dynamiccell_1.Value);
                    }
                }
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String;
                workSheet.Cells.Add(dynamiccell_1);

                //'QTY
                dynamiccell_1 = new Spire.DataExport.XLS.Cell();
                dynamiccell_1.Column = 3;
                dynamiccell_1.Row = intRowIndex;

                if (Convert.IsDBNull(row["Quantity"]))
                {
                    dynamiccell_1.Value = Convert.ToDouble(row["Quantity"]).ToString();
                }
                else
                    dynamiccell_1.Value = 0;

                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String;
                dynamiccell_1.NumericFormat = "#,###,##0.00";
                workSheet.Cells.Add(dynamiccell_1);

                //'Rate
                dynamiccell_1 = new Spire.DataExport.XLS.Cell();
                dynamiccell_1.Column = 4;
                dynamiccell_1.Row = intRowIndex;

                if (Convert.IsDBNull(row["Rate"]))
                {
                    dynamiccell_1.Value = Convert.ToDouble(row["Rate"]).ToString();
                }
                else
                    dynamiccell_1.Value = 0;

                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String;
                dynamiccell_1.NumericFormat = "#,###,##0.00";
                workSheet.Cells.Add(dynamiccell_1);

                 //'Cost
                dynamiccell_1 = new Spire.DataExport.XLS.Cell();
                dynamiccell_1.Column = 5;
                dynamiccell_1.Row = intRowIndex;

                if (Convert.IsDBNull(row["Cost"]))
                {
                    dynamiccell_1.Value = Convert.ToDouble(row["Cost"]).ToString("c");
                }
                else
                    dynamiccell_1.Value = Convert.ToDouble(0).ToString("c");

                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String;
                dynamiccell_1.NumericFormat = "#,###,##0.00";
                workSheet.Cells.Add(dynamiccell_1);

                intRowIndex = intRowIndex + 1;
	        }
             int Range2 = intRowIndex - 1;

            intFootTotalStartRow = intRowIndex;
            intFootTotalStartColum = 4;

            //'heading
            Cell mcell1_5  = new Spire.DataExport.XLS.Cell();
            mcell1_5.Column = intFootTotalStartColum;
            mcell1_5.Row = intFootTotalStartRow + 2;
            mcell1_5.Value = "Subtotal (excl. GST)";
            mcell1_5.CellType = Spire.DataExport.XLS.CellType.String;
            workSheet.Cells.Add(mcell1_5);

            Cell mcell2_5 = new Spire.DataExport.XLS.Cell();
            mcell2_5.Column = intFootTotalStartColum;
            mcell2_5.Row = intFootTotalStartRow + 3;
            mcell2_5.Value = "Add 15% GST";
            mcell2_5.CellType = Spire.DataExport.XLS.CellType.String;
            workSheet.Cells.Add(mcell2_5);

            Cell mcell3_5 = new Spire.DataExport.XLS.Cell();
            mcell3_5.Column = intFootTotalStartColum;
            mcell3_5.Row = intFootTotalStartRow + 4;
            mcell3_5.Value = "Total Incl. GST";
            mcell3_5.CellType = Spire.DataExport.XLS.CellType.String;
            workSheet.Cells.Add(mcell3_5);


            Cell mcell1_7 = new Spire.DataExport.XLS.Cell();
            mcell1_7.Column = intFootTotalStartColum+1;
            mcell1_7.Row = intFootTotalStartRow +2;
            mcell1_7.Value = ApprovedexcludeGstCost;
            mcell1_7.CellType = Spire.DataExport.XLS.CellType.String;
            workSheet.Cells.Add(mcell1_7);

            Cell mcell2_7 = new Spire.DataExport.XLS.Cell();
            mcell2_7.Column = intFootTotalStartColum + 2;
            mcell2_7.Row = intFootTotalStartRow + 3;
            mcell2_7.Value = ApprovedGSTCost;
            mcell2_7.CellType = Spire.DataExport.XLS.CellType.String;
            workSheet.Cells.Add(mcell2_7);

            Cell mcell3_7 = new Spire.DataExport.XLS.Cell();
            mcell3_7.Column = intFootTotalStartColum + 3;
            mcell3_7.Row = intFootTotalStartRow + 4;
            mcell3_7.Value = ApprovedInGSTCost;
            mcell3_7.CellType = Spire.DataExport.XLS.CellType.String;
            workSheet.Cells.Add(mcell3_7);


            DataTable InitialDataTable =new DataTable();
            //'connect to dataset
            workSheet.DataSource = Spire.DataExport.Common.ExportSource.DataTable;
            //'workSheet.DataTable = dsQuery.Tables(0)
            workSheet.DataTable = InitialDataTable;
            //'workSheet.SQLCommand = oleDbCommand
            workSheet.StartDataCol = Convert.ToByte(1);
            cellExport.Sheets.Add(workSheet);

            try
            {
                cellExport.SaveToHttpResponse(String.Format("{0}.xls", strNewExcelFileName), context.Response);
                result = strExcelFile;
            }
            catch (Exception)
            {
                
            }
            return result;
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