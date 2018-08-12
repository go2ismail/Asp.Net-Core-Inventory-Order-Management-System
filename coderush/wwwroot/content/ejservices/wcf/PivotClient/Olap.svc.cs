#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using EJServices.Models;
using Syncfusion.JavaScript;
using Syncfusion.Olap.Manager;
using Syncfusion.Olap.Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using OLAPUTILS = Syncfusion.JavaScript.Olap;

namespace EJServices.Wcf.Pivotclient
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OlapClient" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OlapClient.svc or OlapClient.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Olap : IOlap
    {
        PivotClient olapClientHelper = new PivotClient();
        PivotTreeMap treemapHelper = new PivotTreeMap();
        PivotChart chartHelper = new PivotChart();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        static int cultureIDInfoval = 1033;
        static string connectionString = ConfigurationManager.ConnectionStrings["Adventure Works"].ConnectionString + "locale identifier=" + cultureIDInfoval + ";";
        string conStringforDB = "DataSource=" + HttpContext.Current.Server.MapPath("~/App_Data/ReportsTable.sdf") + "; Persist Security Info=False", reportTableName = "ReportsTable";
        public Dictionary<string, object> CubeChange(string action, string cubeName, string olapReport, string clientReports, string clientParams)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            if (olapReport != "")
                DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(olapReport));
            if (clientReports != "")
                DataManager.Reports = olapClientHelper.DeserializedReports(clientReports);
            return olapClientHelper.GetJsonData(action, DataManager, cubeName, clientParams);
        }

        public Dictionary<string, object> DrillChart(string action, string drilledSeries, string olapReport, string clientReports)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(olapReport));
            DataManager.Reports = olapClientHelper.DeserializedReports(clientReports);
            return chartHelper.GetJsonData(action, DataManager, drilledSeries);
        }

        public Dictionary<string, object> DrillGrid(string action, string cellPosition, string currentReport, string clientReports, string headerInfo, string layout)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            if (DataManager.ConnectionString.ToLower().Replace(" ", String.Empty).Split(';', '=').Contains("localeidentifier"))
            {
                DataManager.Culture = new System.Globalization.CultureInfo(cultureIDInfoval);
                DataManager.OverrideDefaultFormatStrings = true;
            }
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(currentReport));
            DataManager.Reports = olapClientHelper.DeserializedReports(clientReports);
            return olapClientHelper.GetJsonData(action, DataManager, cellPosition, headerInfo, layout);
        }

        public Dictionary<string, object> DrillTreeMap(string action, string drillInfo, string olapReport, string clientReports)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(olapReport));
            DataManager.Reports = olapClientHelper.DeserializedReports(clientReports);
            return treemapHelper.GetJsonData(action, DataManager, drillInfo);
        }

        public Dictionary<string, object> DropNode(string action, string dropType, string nodeInfo, string olapReport, string clientReports)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(olapReport));
            DataManager.Reports = olapClientHelper.DeserializedReports(clientReports);
            return olapClientHelper.GetJsonData(action, DataManager, dropType, nodeInfo);
        }

        public Dictionary<string, object> ExpandMember(string action, bool checkedStatus, string parentNode, string tag, string dimensionName, string cubeName, string olapReport, string clientReports)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            if (!string.IsNullOrEmpty(olapReport))
                DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(olapReport));
            if (!string.IsNullOrEmpty(clientReports))
                DataManager.Reports = olapClientHelper.DeserializedReports(clientReports);
            return olapClientHelper.GetJsonData(action, DataManager, checkedStatus, parentNode, tag, dimensionName, cubeName);
        }

        public void Export(Stream stream)
        {
            System.IO.StreamReader sReader = new System.IO.StreamReader(stream);
            string args = System.Web.HttpContext.Current.Server.UrlDecode(sReader.ReadToEnd()).Remove(0, 5);
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            string fileName = "Sample";
            olapClientHelper.ExportPivotClient(DataManager, args, fileName, System.Web.HttpContext.Current.Response);
        }

        public Dictionary<string, object> FetchMemberTreeNodes(string action, string dimensionName, string olapReport)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(olapReport));
            return olapClientHelper.GetJsonData(action, DataManager, dimensionName);
        }

        public Dictionary<string, object> RemoveReportFromDB(string reportName, string operationalMode, string analysisMode)
        {
            SqlCeConnection con = new SqlCeConnection() { ConnectionString = conStringforDB };
            con.Open();
            reportName = reportName + "##" + operationalMode.ToLower() + "#>>#" + analysisMode.ToLower();
            SqlCeCommand cmd1 = null;
            foreach (DataRow row in GetDataTable().Rows)
            {
                if ((row.ItemArray[0] as string).Equals(reportName))
                {
                    cmd1 = new SqlCeCommand("DELETE FROM ReportsTable WHERE ReportName LIKE '%" + reportName + "%'", con);
                }
            }
            cmd1.ExecuteNonQuery();
            con.Close();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("CurrentAction", "Remove");
            return dictionary;
        }

        public Dictionary<string, object> RenameReportInDB(string selectedReport, string renameReport, string operationalMode, string analysisMode)
        {
            SqlCeConnection con = new SqlCeConnection() { ConnectionString = conStringforDB };
            con.Open();
            selectedReport = selectedReport + "##" + operationalMode.ToLower() + "#>>#" + analysisMode.ToLower();
            renameReport = renameReport + "##" + operationalMode.ToLower() + "#>>#" + analysisMode.ToLower();
            SqlCeCommand cmd1 = null;
            foreach (DataRow row in GetDataTable().Rows)
            {
                if ((row.ItemArray[0] as string).Equals(selectedReport))
                {
                    cmd1 = new SqlCeCommand("update ReportsTable set ReportName=@RenameReport where ReportName like '%" + selectedReport + "%'", con);
                }
            }
            cmd1.Parameters.Add("@RenameReport", renameReport);
            cmd1.ExecuteNonQuery();
            con.Close();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("CurrentAction", "Rename");
            return dictionary;
        }

        public Dictionary<string, object> FetchReportListFromDB(string action, string operationalMode, string analysisMode)
        {
            string reportNames = string.Empty;
            string currentRptName = string.Empty;
            foreach (System.Data.DataRow row in GetDataTable().Rows)
            {
                currentRptName = (row.ItemArray[0] as string);
                if (currentRptName.IndexOf("##" + operationalMode + "#>>#" + analysisMode) >= 0)
                {
                    currentRptName = currentRptName.Replace("##" + operationalMode + "#>>#" + analysisMode, "");
                    reportNames = reportNames == "" ? currentRptName : reportNames + "__" + currentRptName;
                }
            }
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ReportNameList", reportNames);
            dictionary.Add("action", action);
            return dictionary;
        }

        public Dictionary<string, object> FilterElement(string action, string clientParams, string olapReport, string clientReports)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(olapReport));
            DataManager.Reports = olapClientHelper.DeserializedReports(clientReports);
            return olapClientHelper.GetJsonData(action, DataManager, clientParams);
        }

        public string GetMDXQuery(string olapReport)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(olapReport));
            return DataManager.GetMDXQuery();
        }

        public Dictionary<string, object> Initialize(string action, string customObject, string clientParams)
        {
            OlapDataManager DataManager = null;
            dynamic customData = serializer.Deserialize<dynamic>(customObject.ToString());
            var cultureIDInfo = new System.Globalization.CultureInfo("en-US").LCID;
            if (customData is Dictionary<string, object> && customData.ContainsKey("Language"))
            {
                cultureIDInfo = new System.Globalization.CultureInfo((customData["Language"])).LCID;
            }
            connectionString = connectionString.Replace("" + cultureIDInfoval + "", "" + cultureIDInfo + "");
            cultureIDInfoval = cultureIDInfo;
            DataManager = new OlapDataManager(connectionString);
            DataManager.Culture = new System.Globalization.CultureInfo(cultureIDInfo);
            DataManager.SetCurrentReport(CreateOlapReport());
            return olapClientHelper.GetJsonData(action, DataManager, clientParams);
        }

        public Dictionary<string, object> InitializeChart(string action, string currentReport, string customObject)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(currentReport));
            return chartHelper.GetJsonData(action, DataManager);
        }

        public Dictionary<string, object> InitializeGrid(string action, string currentReport, string gridLayout, string customObject)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            if (DataManager.ConnectionString.ToLower().Replace(" ", String.Empty).Split(';', '=').Contains("localeidentifier"))
            {
                DataManager.Culture = new System.Globalization.CultureInfo(cultureIDInfoval);
                DataManager.OverrideDefaultFormatStrings = true;
            }
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(currentReport));
            return olapClientHelper.GetJsonData(action, DataManager, gridLayout);
        }

        public Dictionary<string, object> InitializeTreeMap(string action, string currentReport, string customObject)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(currentReport));
            return treemapHelper.GetJsonData(action, DataManager);
        }

        public Dictionary<string, object> LoadReportFromDB(string reportName, string operationalMode, string analysisMode, string olapReport, string clientReports)
        {
            PivotReport report = new PivotReport();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string currentRptName = string.Empty;
            foreach (DataRow row in GetDataTable().Rows)
            {
                currentRptName = (row.ItemArray[0] as string).Replace("##" + operationalMode.ToLower() + "#>>#" + analysisMode.ToLower(), "");
                if (currentRptName.Equals(reportName))
                {
                    byte[] reportString = new byte[2 * 1024];
                    reportString = (row.ItemArray[1] as byte[]);
                    if (operationalMode.ToLower() == "servermode" && analysisMode == "olap")
                    {
                        var repCol = Encoding.UTF8.GetString(reportString);
                        OlapDataManager DataManager = new OlapDataManager(connectionString);
                        if (repCol.IndexOf("<?xml version") == 0)
                        {
                            var reportStr = "";
                            reportStr = Syncfusion.JavaScript.Olap.Utils.CompressData(row.ItemArray[1] as byte[]);
                            DataManager.Reports = olapClientHelper.DeserializedReports(reportStr);
                            DataManager.SetCurrentReport(DataManager.Reports[0]);
                            return olapClientHelper.GetJsonData("toolbarOperation", DataManager, "Load Report", reportName);
                        }
                        else
                        {
                            dynamic customData = serializer.Deserialize<dynamic>(repCol.ToString());
                            DataManager.Reports = olapClientHelper.DeserializedReports(customData[customData[customData.Length - 1]["cubeIndex"]]["Reports"]);
                            DataManager.SetCurrentReport(DataManager.Reports[customData[customData[customData.Length - 1]["cubeIndex"]]["ReportIndex"]]);
                            dictionary = olapClientHelper.GetJsonData("toolbarOperation", DataManager, "Load Report", reportName);
                            dictionary.Add("Collection", repCol);
                        }
                    }
                    else
                    {
                        if (analysisMode.ToLower() == "pivot" && operationalMode.ToLower() == "servermode")
                            dictionary = olapClientHelper.GetJsonData("LoadReport", ProductSales.GetSalesData(), Encoding.UTF8.GetString(reportString));
                        else
                            dictionary.Add("report", Encoding.UTF8.GetString(reportString));
                        break;
                    }
                }
            }
            return dictionary;
        }

        public Dictionary<string, object> MeasureGroup(string action, string measureGroupName, string olapReport, string clientReports)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(olapReport));
            DataManager.Reports = olapClientHelper.DeserializedReports(clientReports);
            return olapClientHelper.GetJsonData(action, DataManager, measureGroupName);
        }
        public Dictionary<string, object> CalculatedMember(string action, string olapReport, string clientReports, string caption, string expression, string memberType, string dimension, string formatString, string uniqueName)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(Syncfusion.JavaScript.Olap.Utils.DeserializeOlapReport(olapReport));
            DataManager.Reports = olapClientHelper.DeserializedReports(clientReports);
            return olapClientHelper.GetJsonData(action, DataManager, caption, expression, memberType, dimension, formatString,uniqueName);
        }
        public Dictionary<string, object> RemoveSplitButton(string action, string clientParams, string olapReport, string clientReports)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(olapReport));
            DataManager.Reports = olapClientHelper.DeserializedReports(clientReports);
            return olapClientHelper.GetJsonData(action, DataManager, clientParams);
        }

        public Dictionary<string, object> SaveReportToDB(string reportName, string operationalMode, string analysisMode, string olapReport, string clientReports)
        {
            reportName = reportName + "##" + operationalMode.ToLower() + "#>>#" + analysisMode.ToLower();
            bool isDuplicate = true;
            SqlCeConnection con = new SqlCeConnection() { ConnectionString = conStringforDB };
            con.Open();
            SqlCeCommand cmd1 = null;
            foreach (DataRow row in GetDataTable().Rows)
            {
                if ((row.ItemArray[0] as string).Equals(reportName))
                {
                    isDuplicate = false;
                    cmd1 = new SqlCeCommand("update ReportsTable set Report=@Reports where ReportName like @ReportName", con);
                }
            }
            if (isDuplicate)
            {
                cmd1 = new SqlCeCommand("insert into ReportsTable Values(@ReportName,@Reports)", con);
            }
            cmd1.Parameters.Add("@ReportName", reportName);
            cmd1.Parameters.Add("@Reports", Encoding.UTF8.GetBytes(clientReports).ToArray());
            cmd1.ExecuteNonQuery();
            con.Close();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("CurrentAction", "Save");
            return dictionary;
        }

        public Dictionary<string, object> ToggleAxis(string action, string currentReport, string clientReports)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(currentReport));
            DataManager.Reports = olapClientHelper.DeserializedReports(clientReports);
            DataManager.ToggleAxis(DataManager.CurrentReport);
            return olapClientHelper.GetJsonData(action, DataManager, clientReports);
        }

        public Dictionary<string, object> ToolbarOperations(string action, string toolbarOperation, string clientInfo, string olapReport, string clientReports)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            if (!string.IsNullOrEmpty(olapReport))
                DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(olapReport));
            if (!string.IsNullOrEmpty(clientReports))
                DataManager.Reports = olapClientHelper.DeserializedReports(clientReports);
            return olapClientHelper.GetJsonData(action, DataManager, toolbarOperation, clientInfo);
        }

        public Dictionary<string, object> UpdateReport(string action, string clientParams, string olapReport, string clientReports)
        {
            return olapClientHelper.GetJsonData(action, clientParams, olapReport, clientReports);
        }

        private DataTable GetDataTable()
        {
            SqlCeConnection con = new SqlCeConnection() { ConnectionString = conStringforDB };
            con.Open();
            DataSet dSet = new DataSet();
            new SqlCeDataAdapter("Select * from ReportsTable", con).Fill(dSet);
            con.Close();
            return dSet.Tables[0];
        }

        private OlapReport CreateOlapReport()
        {
            OlapReport olapReport = new OlapReport() { Name = "Default Report" };
            olapReport.CurrentCubeName = "Adventure Works";

            MeasureElements measureElement = new MeasureElements();
            measureElement.Elements.Add(new MeasureElement { UniqueName = "[Measures].[Customer Count]" });

            DimensionElement dimensionElementRow = new DimensionElement();
            dimensionElementRow.Name = "Date";
            dimensionElementRow.AddLevel("Fiscal", "Fiscal Year");

            olapReport.SeriesElements.Add(dimensionElementRow);
            olapReport.CategoricalElements.Add(measureElement);

            return olapReport;
        }
    }
}
