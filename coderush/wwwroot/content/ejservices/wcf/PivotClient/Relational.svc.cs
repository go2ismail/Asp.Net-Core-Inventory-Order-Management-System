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
using Syncfusion.PivotAnalysis.Base;
using System;
using System.Collections.Generic;
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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RelationalClient" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RelationalClient.svc or RelationalClient.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Relational : IRelational
    {
        PivotClient pivotClient = new PivotClient();
        PivotChart pivotChart = new PivotChart();
        PivotGrid pivotGrid = new PivotGrid();
        string conStringforDB = "DataSource=" + HttpContext.Current.Server.MapPath("~/App_Data/ReportsTable.sdf") + "; Persist Security Info=False", reportTableName = "ReportsTable";
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        public Dictionary<string, object> DrillChart(string action, string drilledSeries, string currentReport)
        {
            pivotClient.PopulateData(currentReport);
            pivotChart.PivotEngine.PivotRows = this.pivotClient.PivotReport.PivotRows;
            pivotChart.PivotEngine.PivotColumns = this.pivotClient.PivotReport.PivotColumns;
            pivotChart.PivotEngine.PivotCalculations = this.pivotClient.PivotReport.PivotCalculations;
            pivotChart.PivotEngine.Filters = this.pivotClient.PivotReport.Filters;
            pivotChart.PivotReport = this.pivotClient.PivotReport;
            return pivotChart.GetJsonData(action, ProductSales.GetSalesData(), drilledSeries);
        }

        public Dictionary<string, object> DropNode(string action, string args)
        {
            return pivotClient.GetJsonData(action, ProductSales.GetSalesData(), args);
        }

        public void Export(Stream stream)
        {
            System.IO.StreamReader sReader = new System.IO.StreamReader(stream);
            string args = System.Web.HttpContext.Current.Server.UrlDecode(sReader.ReadToEnd()).Remove(0, 5);
            Dictionary<string, string> gridParams = serializer.Deserialize<Dictionary<string, string>>(args);
            pivotClient.PopulateData(gridParams["currentReport"]);
            string fileName = "Sample";
            pivotClient.ExportPivotClient(ProductSales.GetSalesData(), args, fileName, System.Web.HttpContext.Current.Response);
        }

        public Dictionary<string, object> FetchMembers(string action, string currentReport, string customObject, string headerTag)
        {
            pivotClient.PopulateData(currentReport);
            return pivotClient.GetJsonData(action, ProductSales.GetSalesData(), headerTag);
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

        public Dictionary<string, object> Filtering(string action, string filterParams, string currentReport, string customObject)
        {
            pivotClient.PopulateData(currentReport);
            return pivotClient.GetJsonData(action, ProductSales.GetSalesData(), filterParams);
        }

        public Dictionary<string, object> Initialize(string action, string clientParams)
        {
            BindData();
            return pivotClient.GetJsonData(action, ProductSales.GetSalesData(), clientParams);
        }

        public Dictionary<string, object> Sorting(string action, string currentReport, string sortedHeaders)
        {
            pivotClient.PopulateData(currentReport);
            return pivotClient.GetJsonData(action, ProductSales.GetSalesData(), sortedHeaders);
        }
		
		public Dictionary<string, object> ValueSorting(string action, string valueSorting, string currentReport, string customObject)
        {
            pivotClient.PopulateData(currentReport);
            return pivotClient.GetJsonData(action, ProductSales.GetSalesData(), valueSorting);
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
                    if (operationalMode.ToLower() == "servermode" && analysisMode == "olap")
                    {
                        var reportString = "";
                        OlapDataManager DataManager = new OlapDataManager();
                        reportString = OLAPUTILS.Utils.CompressData(row.ItemArray[1] as byte[]);
                        DataManager.Reports = pivotClient.DeserializedReports(reportString);
                        DataManager.SetCurrentReport(DataManager.Reports[0]);
                        return pivotClient.GetJsonData("toolbarOperation", DataManager, "Load Report", reportName);
                    }
                    else
                    {
                        byte[] reportString = new byte[2 * 1024];
                        reportString = (row.ItemArray[1] as byte[]);
                        if (analysisMode.ToLower() == "pivot" && operationalMode.ToLower() == "servermode")
                            dictionary = pivotClient.GetJsonData("LoadReport", ProductSales.GetSalesData(), Encoding.UTF8.GetString(reportString));
                        else
                            dictionary.Add("report", Encoding.UTF8.GetString(reportString));
                        break;
                    }

                }
            }
            return dictionary;
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

        public Dictionary<string, object> ToolbarOperations(string action, string args)
        {
            return pivotClient.GetJsonData(action, ProductSales.GetSalesData(), args);
        }


        private void BindData()
        {
            pivotClient.PivotReport.PivotRows.Add(new PivotItem { FieldMappingName = "Country", FieldHeader = "Country", TotalHeader = "Total", ShowSubTotal = false });
            pivotClient.PivotReport.PivotRows.Add(new PivotItem { FieldMappingName = "State", FieldHeader = "State", TotalHeader = "Total" });
            pivotClient.PivotReport.PivotRows.Add(new PivotItem { FieldMappingName = "Date", FieldHeader = "Date", TotalHeader = "Total" });
            pivotClient.PivotReport.PivotColumns.Add(new PivotItem { FieldMappingName = "Product", FieldHeader = "Product", TotalHeader = "Total", ShowSubTotal = false });
            pivotClient.PivotReport.PivotCalculations.Add(new PivotComputationInfo { CalculationName = "Amount", Description = "Amount", FieldHeader = "Amount", FieldName = "Amount", Format = "C", SummaryType = Syncfusion.PivotAnalysis.Base.SummaryType.DoubleTotalSum });
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
    }
}
