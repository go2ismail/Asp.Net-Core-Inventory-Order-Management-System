#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.JavaScript.Olap;
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
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using OLAPUTILS = Syncfusion.JavaScript.Olap;

namespace EJServices.Wcf.Pivotgrid
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OlapGrid" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OlapGrid.svc or OlapGrid.svc.cs at the Solution Explorer and start debugging.
    public class Olap : IOlap
    {
        Syncfusion.JavaScript.PivotGrid htmlHelper = new Syncfusion.JavaScript.PivotGrid();
        string connectionString = ConfigurationManager.ConnectionStrings["Adventure Works"].ConnectionString + "locale identifier=1033;";
        int cultureIDInfoval = 1033;
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string conStringforDB = "DataSource=" + HttpContext.Current.Server.MapPath("~/App_Data/ReportsTable.sdf") + "; Persist Security Info=False";

        public Dictionary<string, object> DeferUpdate(string action, string gridLayout, string filterParams, string currentReport, object customObject)
        {
            dynamic customData = serializer.Deserialize<dynamic>(customObject.ToString());
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(Utils.DeserializeOlapReport(currentReport));
            if (customData is Dictionary<string, object> && customData.ContainsKey("Language"))
            {
                DataManager.Culture = new System.Globalization.CultureInfo((customData["Language"]));
                DataManager.OverrideDefaultFormatStrings = true;
            }
            return htmlHelper.GetJsonData(action, connectionString, DataManager, null, filterParams, gridLayout);
        }

        public Dictionary<string, object> Drill(string action, string cellPosition, string currentReport, string headerInfo, string layout, object customObject)
        {
            dynamic customData = serializer.Deserialize<dynamic>(customObject.ToString());
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            if (customData is Dictionary<string, object> && customData.ContainsKey("Language"))
            {
                var cultureIDInfo = new System.Globalization.CultureInfo((customData["Language"])).LCID;
                connectionString = connectionString.Replace("" + cultureIDInfoval + "", "" + cultureIDInfo + "");
                cultureIDInfoval = cultureIDInfo;
                DataManager = new OlapDataManager(connectionString);
                DataManager.Culture = new System.Globalization.CultureInfo((customData["Language"]));
                DataManager.OverrideDefaultFormatStrings = true;
            }
            else
                DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(Utils.DeserializeOlapReport(currentReport));
            return htmlHelper.GetJsonData(action, connectionString, DataManager, cellPosition, headerInfo, layout);
        }

        public Dictionary<string, object> DropNode(string action, string dropType, string nodeInfo, string filterParams, string gridLayout, string currentReport, object customObject)
        {
            dynamic customData = serializer.Deserialize<dynamic>(customObject.ToString());
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(Utils.DeserializeOlapReport(currentReport));
            if (customData is Dictionary<string, object> && customData.ContainsKey("Language"))
            {
                DataManager.Culture = new System.Globalization.CultureInfo((customData["Language"]));
                DataManager.OverrideDefaultFormatStrings = true;
            }
            return htmlHelper.GetJsonData(action, DataManager, dropType, nodeInfo, filterParams, gridLayout, true);
        }

        public Dictionary<string, object> ExpandMember(string action, bool checkedStatus, string parentNode, string tag, string cubeName, string currentReport)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            if (!string.IsNullOrEmpty(currentReport))
                DataManager.SetCurrentReport(Utils.DeserializeOlapReport(currentReport));
            return htmlHelper.GetJsonData(action, DataManager, checkedStatus, parentNode, tag, cubeName);
        }

        public void Export(Stream stream)
        {
            System.IO.StreamReader sReader = new System.IO.StreamReader(stream);
            string args = System.Web.HttpContext.Current.Server.UrlDecode(sReader.ReadToEnd()).Remove(0, 5); ;
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            string fileName = "Sample";
            htmlHelper.ExportPivotGrid(DataManager, args, fileName, System.Web.HttpContext.Current.Response);
        }

        public Dictionary<string, object> FetchMembers(string action, string headerTag, string currentReport)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(Utils.DeserializeOlapReport(currentReport));
            return htmlHelper.GetJsonData(action, DataManager, null, headerTag);
        }

        public Dictionary<string, object> Filtering(string action, string filterParams, string gridLayout, string currentReport, object customObject)
        {
            dynamic customData = serializer.Deserialize<dynamic>(customObject.ToString());
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(Utils.DeserializeOlapReport(currentReport));
            if (customData is Dictionary<string, object> && customData.ContainsKey("Language"))
            {
                DataManager.Culture = new System.Globalization.CultureInfo((customData["Language"]));
                DataManager.OverrideDefaultFormatStrings = true;
            }
            return htmlHelper.GetJsonData(action, connectionString, DataManager, null, filterParams, gridLayout);
        }

        public Dictionary<string, object> Initialize(string action, string gridLayout, bool enablePivotFieldList, object customObject)
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
            DataManager.OverrideDefaultFormatStrings = true;
            DataManager.SetCurrentReport(CreateOlapReport((customData is Dictionary<string, object> && customData.ContainsKey("isPaging")) ? customData["isPaging"] : false));
            return htmlHelper.GetJsonData(action, DataManager, gridLayout, enablePivotFieldList);
        }

        public Dictionary<string, object> LoadReportFromDB(string action, string layout, bool enablePivotFieldList, object customObject, string reportName, string operationalMode, string olapReport, string clientReports)
        {
            string mode = operationalMode;
            var reports = "";
            byte[] reportString = new byte[4 * 1024];
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
                            reportString = row.ItemArray[1] as byte[];
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
                OlapDataManager DataManager = new OlapDataManager(connectionString);
                dynamic customData = serializer.Deserialize<dynamic>(customObject.ToString());
                var cultureIDInfo = new System.Globalization.CultureInfo(("en-US")).LCID;
                if (customData is Dictionary<string, object> && customData.ContainsKey("Language"))
                {
                    cultureIDInfo = new System.Globalization.CultureInfo((customData["Language"])).LCID;
                }
                connectionString = connectionString.Replace("" + cultureIDInfoval + "", "" + cultureIDInfo + "");
                cultureIDInfoval = cultureIDInfo;
                DataManager.Culture = new System.Globalization.CultureInfo((cultureIDInfo));
                DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(reports));
                DataManager.OverrideDefaultFormatStrings = true;
                dictionary = htmlHelper.GetJsonData(action, DataManager, layout, enablePivotFieldList);
            }
            return dictionary;
        }

        public Dictionary<string, object> Paging(string action, string pagingInfo, string currentReport, string gridLayout, object customObject)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(htmlHelper.SetPaging(currentReport, pagingInfo));
            return htmlHelper.GetJsonData(action, DataManager, gridLayout);
        }

        public Dictionary<string, object> RemoveButton(string action, string headerInfo, string gridLayout, string currentReport, object customObject)
        {
            dynamic customData = serializer.Deserialize<dynamic>(customObject.ToString());
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(Utils.DeserializeOlapReport(currentReport));
            if (customData is Dictionary<string, object> && customData.ContainsKey("Language"))
            {
                DataManager.Culture = new System.Globalization.CultureInfo((customData["Language"]));
                DataManager.OverrideDefaultFormatStrings = true;
            }
            return htmlHelper.GetJsonData(action, connectionString, DataManager, null, headerInfo, gridLayout);
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

        private DataTable GetDataTable()
        {
            SqlCeConnection con = new SqlCeConnection() { ConnectionString = conStringforDB };
            con.Open();
            DataSet dSet = new DataSet();
            new SqlCeDataAdapter("Select * from ReportsTable", con).Fill(dSet);
            con.Close();
            return dSet.Tables[0];
        }

        private OlapReport CreateOlapReport(bool isPaging)
        {
            OlapReport olapReport = new OlapReport();
            olapReport.CurrentCubeName = "Adventure Works";

            if (isPaging)
            {
                olapReport.EnablePaging = true;
                olapReport.PagerOptions.SeriesPageSize = 5;
                olapReport.PagerOptions.CategoricalPageSize = 5;

                DimensionElement dimensionElement = new DimensionElement() { Name = "Customer" };
                dimensionElement.AddLevel("Customer", "Customer");
                olapReport.CategoricalElements.Add(dimensionElement);

                DimensionElement dimensionElementRow = new DimensionElement() { Name = "Customer", HierarchyName = "Customer" };
                dimensionElementRow.AddLevel("Customer Geography", "Country");
                olapReport.SeriesElements.Add(dimensionElementRow);

                MeasureElements measureElementColumn = new MeasureElements();
                measureElementColumn.Elements.Add(new MeasureElement { Name = "Internet Sales Amount" });
                olapReport.CategoricalElements.Add(measureElementColumn);
            }
            else
            {
                MeasureElements measureElement = new MeasureElements();
                measureElement.Elements.Add(new MeasureElement { UniqueName = "[Measures].[Internet Sales Amount]" });

                DimensionElement dimensionElementRow = new DimensionElement();
                dimensionElementRow.Name = "Date";
                dimensionElementRow.AddLevel("Fiscal", "Fiscal Year");

                DimensionElement dimensionElementColumn = new DimensionElement();
                dimensionElementColumn.Name = "Customer";
                dimensionElementColumn.AddLevel("Customer Geography", "Country");

                olapReport.SeriesElements.Add(dimensionElementRow);
                olapReport.CategoricalElements.Add(dimensionElementColumn);
                olapReport.CategoricalElements.Add(measureElement);
            }
            return olapReport;
        }
    }
}
