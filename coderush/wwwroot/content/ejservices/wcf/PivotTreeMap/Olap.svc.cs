#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.JavaScript;
using Syncfusion.Olap.Manager;
using Syncfusion.Olap.Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Script.Serialization;
using OLAPUTILS = Syncfusion.JavaScript.Olap;

namespace EJServices.Wcf.Pivottreemap
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OlapTreeMap" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OlapTreeMap.svc or OlapTreeMap.svc.cs at the Solution Explorer and start debugging.
    public class Olap : IOlap
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        PivotTreeMap htmlHelper = new PivotTreeMap();
        int cultureIDInfovalval = 1033;
        string connectionString = ConfigurationManager.ConnectionStrings["Adventure Works"].ConnectionString + "locale identifier=1033;";
        public Dictionary<string, object> Initialize(string action, string currentReport, string customObject)
        {
            OlapDataManager DataManager = null;
            dynamic customData = serializer.Deserialize<dynamic>(customObject.ToString());
            var cultureIDInfo = new System.Globalization.CultureInfo(("en-US")).LCID;
            if (customData is Dictionary<string, object> && customData.ContainsKey("Language"))
                cultureIDInfo = new System.Globalization.CultureInfo((customData["Language"])).LCID;
            connectionString = connectionString.Replace("" + cultureIDInfovalval + "", "" + cultureIDInfo + "");
            cultureIDInfovalval = cultureIDInfo;
            DataManager = new OlapDataManager(connectionString);
            DataManager.Culture = new System.Globalization.CultureInfo((cultureIDInfo));
            DataManager.SetCurrentReport(CreateOlapReport());
            return htmlHelper.GetJsonData(action, DataManager);
        }

        public Dictionary<string, object> Drill(string action, string drillInfo, string olapReport, string customObject)
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
            DataManager.SetCurrentReport(OLAPUTILS.Utils.DeserializeOlapReport(olapReport));
            return htmlHelper.GetJsonData(action, DataManager, drillInfo);
        }

        private OlapReport CreateOlapReport()
        {
            OlapReport olapReport = new OlapReport();
            olapReport.Name = "Default Report";
            olapReport.CurrentCubeName = "Adventure Works";

            DimensionElement dimensionElementColumn = new DimensionElement();
            //Specifying the Name for the Dimension Element
            dimensionElementColumn.Name = "Date";
            dimensionElementColumn.AddLevel("Fiscal", "Fiscal Year");

            MeasureElements measureElementColumn = new MeasureElements();
            //Specifying the Name for the Measure Element
            measureElementColumn.Elements.Add(new MeasureElement { Name = "Customer Count" });

            DimensionElement dimensionElementRow = new DimensionElement();
            //Specifying the Dimension Name
            dimensionElementRow.Name = "Customer";
            dimensionElementRow.AddLevel("Customer Geography", "Country");

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
