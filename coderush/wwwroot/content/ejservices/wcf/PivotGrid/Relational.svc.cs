#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using EJServices.Models;
using Syncfusion.JavaScript;
using Syncfusion.PivotAnalysis.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using OLAPUTILS = Syncfusion.JavaScript.Olap;
namespace EJServices.Wcf.Pivotgrid
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RelationalGrid" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RelationalGrid.svc or RelationalGrid.svc.cs at the Solution Explorer and start debugging.
    public class Relational : IRelational
    {
        PivotGrid htmlHelper = new PivotGrid();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        static int cultureIDInfoval = 1033;
        static string connectionString = ConfigurationManager.ConnectionStrings["Adventure Works"].ConnectionString + "locale identifier=" + cultureIDInfoval + ";";
        string conStringforDB = "DataSource=" + HttpContext.Current.Server.MapPath("~/App_Data/ReportsTable.sdf") + "; Persist Security Info=False";
        public Dictionary<string, object> CalculatedField(string action, string headerTag, string currentReport)
        {
            htmlHelper.PopulateData(currentReport);
            dict = htmlHelper.GetJsonData(action, ProductSales.GetSalesData(), null, headerTag);
            return dict;
        }

        public Dictionary<string, object> CellEditing(string action, string index, string valueHeaders, string summaryValues, string currentReport)
        {
            htmlHelper.PopulateData(currentReport);
            dict = htmlHelper.GetJsonData(action, ProductSales.GetSalesData(), index, summaryValues, valueHeaders);
            return dict;
        }

        public Dictionary<string, object> DeferUpdate(string action, string filterParams, string sortedHeaders, string currentReport)
        {
            htmlHelper.PopulateData(currentReport);
            dict = htmlHelper.GetJsonData(action, ProductSales.GetSalesData(), null, null, null, sortedHeaders, filterParams);
            return dict;
        }

        public Dictionary<string, object> DropNode(string action, string dropAxis, string headerTag, string sortedHeaders, string filterParams, string currentReport)
        {
            htmlHelper.PopulateData(currentReport);
            dict = htmlHelper.GetJsonData(action, ProductSales.GetSalesData(), dropAxis, headerTag, filterParams, sortedHeaders);
            return dict;
        }

        public void Export(Stream stream)
        {
            System.IO.StreamReader sReader = new System.IO.StreamReader(stream);
            string args = System.Web.HttpContext.Current.Server.UrlDecode(sReader.ReadToEnd()).Remove(0, 5);
            Dictionary<string, string> gridParams = serializer.Deserialize<Dictionary<string, string>>(args);
            htmlHelper.PopulateData(gridParams["currentReport"]);
            string fileName = "Sample";
            htmlHelper.ExportPivotGrid(ProductSales.GetSalesData(), args, fileName, System.Web.HttpContext.Current.Response);
        }

        public Dictionary<string, object> FetchMembers(string action, string headerTag, string sortedHeaders, string currentReport)
        {
            htmlHelper.PopulateData(currentReport);
            dict = htmlHelper.GetJsonData(action, ProductSales.GetSalesData(), headerTag, sortedHeaders);
            return dict;
        }

        public Dictionary<string, object> Filtering(string action, string filterParams, string sortedHeaders, string currentReport)
        {
            htmlHelper.PopulateData(currentReport);
            dict = htmlHelper.GetJsonData(action, ProductSales.GetSalesData(), filterParams, sortedHeaders);
            return dict;
        }

        public Dictionary<string, object> Initialize(string action)
        {
            htmlHelper.PivotReport = BindDefaultData();
            dict = htmlHelper.GetJsonData(action, ProductSales.GetSalesData());
            return dict;
        }

        public Dictionary<string, object> LoadReportFromDB(string reportName, string operationalMode, string olapReport, string clientReports)
        {
            byte[] reportString = new byte[2 * 1024];
            PivotReport report = new PivotReport();
            var reports = "";
            string mode = operationalMode;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (mode == "serverMode" && !string.IsNullOrEmpty(clientReports))
            {
                reports = clientReports;
            }
            else
            {
                foreach (DataRow row in GetDataTable().Rows)
                {
                    if ((row.ItemArray[0] as string).Equals(reportName))
                    {
                        if (mode == "clientMode")
                        {
                            reportString = (row.ItemArray[1] as byte[]);
                            dictionary.Add("report", Encoding.UTF8.GetString(reportString));
                            break;
                        }
                        else if (mode == "serverMode")
                        {
                            reports = OLAPUTILS.Utils.CompressData(row.ItemArray[1] as byte[]);
                            break;
                        }
                    }
                }
            }
            if (reports != "")
            {
                report = htmlHelper.DeserializedReports(reports);
                htmlHelper.PivotReport = report;
                dictionary = htmlHelper.GetJsonData("loadOperation", ProductSales.GetSalesData(), "Load Report", reportName);
            }
            return dictionary;
        }

        public Dictionary<string, object> ModifyNodeState(string action, string headerTag, string dropAxis, string sortedHeaders, string filterParams, string currentReport)
        {
            htmlHelper.PopulateData(currentReport);
            dict = htmlHelper.GetJsonData(action, ProductSales.GetSalesData(), headerTag, dropAxis, filterParams, sortedHeaders);
            return dict;
        }

        public Dictionary<string, object> SaveReport(string reportName, string operationalMode, string olapReport, string clientReports)
        {
            string mode = operationalMode;
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
            //cmd1.Parameters.Add("@Reports", OLAPUTILS.Utils.GetReportStream(clientReports).ToArray());
            if (mode == "serverMode")
                cmd1.Parameters.Add("@Reports", OLAPUTILS.Utils.GetReportStream(clientReports).ToArray());
            else if (mode == "clientMode")
                cmd1.Parameters.Add("@Reports", Encoding.UTF8.GetBytes(clientReports).ToArray());
            cmd1.ExecuteNonQuery();
            con.Close();
            return null;
        }

        public Dictionary<string, object> Sorting(string action, string sortedHeaders, string currentReport)
        {
            htmlHelper.PopulateData(currentReport);
            dict = htmlHelper.GetJsonData(action, ProductSales.GetSalesData(), sortedHeaders);
            return dict;
        }

        private PivotReport BindDefaultData()
        {
            PivotReport pivotSetting = new PivotReport();
            pivotSetting.PivotRows.Add(new PivotItem { FieldMappingName = "Date", FieldHeader = "Date", TotalHeader = "Total" });
            pivotSetting.PivotRows.Add(new PivotItem { FieldMappingName = "Product", FieldHeader = "Product", TotalHeader = "Total" });
            pivotSetting.PivotColumns.Add(new PivotItem { FieldMappingName = "Country", FieldHeader = "Country", TotalHeader = "Total" });
            pivotSetting.PivotColumns.Add(new PivotItem { FieldMappingName = "State", FieldHeader = "State", TotalHeader = "Total" });
            pivotSetting.PivotCalculations.Add(new PivotComputationInfo { CalculationName = "Amount", Description = "Amount", FieldHeader = "Amount", FieldName = "Amount", Format = "C", SummaryType = Syncfusion.PivotAnalysis.Base.SummaryType.DoubleTotalSum });
            return pivotSetting;
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
