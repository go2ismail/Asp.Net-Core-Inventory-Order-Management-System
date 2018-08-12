#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.JavaScript;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using Syncfusion.Olap.Manager;
using Syncfusion.Olap.Reports;

namespace EJServices.Wcf.Pivotchart
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OlapChart" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OlapChart.svc or OlapChart.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Olap : IOlap
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        PivotChart htmlHelper = new PivotChart();
        int cultureIDInfovalval = 1033;
        string connectionString = ConfigurationManager.ConnectionStrings["Adventure Works"].ConnectionString + "locale identifier=1033;";

        public Dictionary<string, object> Initialize(string action, string customObject)
        {
            OlapDataManager DataManager = null;
            dynamic customData = serializer.Deserialize<dynamic>(customObject.ToString());
            var cultureIDInfo = new System.Globalization.CultureInfo(("en-US")).LCID;
            if (customData is Dictionary<string, object> && customData.ContainsKey("Language"))
            {
                cultureIDInfo = new System.Globalization.CultureInfo((customData["Language"])).LCID;
            }
            connectionString = connectionString.Replace("" + cultureIDInfovalval + "", "" + cultureIDInfo + "");
            cultureIDInfovalval = cultureIDInfo;
            DataManager = new OlapDataManager(connectionString);
            DataManager.Culture = new System.Globalization.CultureInfo((cultureIDInfo));
            DataManager.SetCurrentReport(CreateOlapReport());
            return htmlHelper.GetJsonData(action, DataManager);
        }

        public Dictionary<string, object> Drill(string action, string drilledSeries, string olapReport, string customObject)
        {
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            dynamic customData = serializer.Deserialize<dynamic>(customObject.ToString());
            if (customData is Dictionary<string, object> && customData.ContainsKey("Language"))
            {
                var cultureIDInfo = new System.Globalization.CultureInfo((customData["Language"])).LCID;
                connectionString = connectionString.Replace("" + cultureIDInfovalval + "", "" + cultureIDInfo + "");
                cultureIDInfovalval = cultureIDInfo;
                DataManager = new OlapDataManager(connectionString);
                DataManager.Culture = new System.Globalization.CultureInfo((customData["Language"]));
            }
            else
                DataManager = new OlapDataManager(connectionString);
            DataManager.SetCurrentReport(Syncfusion.JavaScript.Olap.Utils.DeserializeOlapReport(olapReport));
            return htmlHelper.GetJsonData(action, DataManager, drilledSeries);
        }

        public void Export(Stream stream)
        {
            System.IO.StreamReader sReader = new System.IO.StreamReader(stream);
            string args = System.Web.HttpContext.Current.Server.UrlDecode(sReader.ReadToEnd()).Remove(0, 5);
            OlapDataManager DataManager = new OlapDataManager(connectionString);
            string fileName = "Sample";
            htmlHelper.ExportPivotChart(args, fileName, System.Web.HttpContext.Current.Response);
        }


        private OlapReport CreateOlapReport()
        {
            OlapReport olapReport = new OlapReport();
            olapReport.Name = "Default Report";
            olapReport.CurrentCubeName = "Adventure Works";

            DimensionElement dimensionElementColumn = new DimensionElement();
            //Specifying the Name for the Dimension Element
            dimensionElementColumn.Name = "Customer";
            dimensionElementColumn.AddLevel("Customer Geography", "Country");

            MeasureElements measureElementColumn = new MeasureElements();
            //Specifying the Name for the Measure Element
            measureElementColumn.Elements.Add(new MeasureElement { Name = "Customer Count" });

            DimensionElement dimensionElementRow = new DimensionElement();
            //Specifying the Dimension Name
            dimensionElementRow.Name = "Date";
            dimensionElementRow.AddLevel("Fiscal", "Fiscal Year");

            ///Adding Row Members
            olapReport.SeriesElements.Add(dimensionElementRow);
            ///Adding Column Members
            olapReport.CategoricalElements.Add(dimensionElementColumn);
            ///Adding Measure Element
            olapReport.CategoricalElements.Add(measureElementColumn);
            return olapReport;
        }
    }
}
