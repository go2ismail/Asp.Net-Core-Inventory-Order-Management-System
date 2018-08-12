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
using System.ServiceModel.Activation;
using System.Text;
using System.Web.Script.Serialization;

namespace EJServices.Wcf.Pivotgauge
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OlapGauge" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OlapGauge.svc or OlapGauge.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Olap : IOlap
    {
        PivotGauge htmlHelper = new PivotGauge();
        int cultureIDInfoval = 1033;
        string connectionString = ConfigurationManager.ConnectionStrings["Adventure Works"].ConnectionString + "locale identifier=1033;";
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        public Dictionary<string, object> Initialize(string action, string customObject)
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
            return htmlHelper.GetJsonData(action, DataManager);
        }

        private OlapReport CreateOlapReport()
        {
            OlapReport report = new OlapReport();
            report.CurrentCubeName = "Adventure Works";

            //Specifying the KPI name
            KpiElements kpiElement = new KpiElements();
            kpiElement.Elements.Add(new KpiElement { Name = "Internet Revenue", ShowKPIGoal = true, ShowKPIStatus = true, ShowKPIValue = true, ShowKPITrend = true });

            DimensionElement dimensionElementColumn = new DimensionElement();
            //Specifying the dimension name
            dimensionElementColumn.Name = "Customer";
            //Adding the level with the hierarchy Name
            dimensionElementColumn.AddLevel("Customer Geography", "Country");

            //Specifying the measure name
            MeasureElements measureElementColumn = new MeasureElements();
            measureElementColumn.Elements.Add(new MeasureElement { Name = "Internet Sales Amount" });

            DimensionElement dimensionElementRow = new DimensionElement();
            //Specifying the dimension name
            dimensionElementRow.Name = "Date";
            //Adding the level with the hierarchy Name
            dimensionElementRow.AddLevel("Fiscal", "Fiscal Year");
            dimensionElementRow.Hierarchy.LevelElements["Fiscal Year"].Add("FY 2004");
            dimensionElementRow.Hierarchy.LevelElements["Fiscal Year"].IncludeAvailableMembers = true;

            report.CategoricalElements.Add(dimensionElementColumn);
            report.CategoricalElements.Add(kpiElement);
            report.CategoricalElements.Add(measureElementColumn);
            report.SeriesElements.Add(dimensionElementRow);
            return report;
        }
    }
}
