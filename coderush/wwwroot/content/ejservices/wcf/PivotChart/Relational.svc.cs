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
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web.Script.Serialization;

namespace EJServices.Wcf.Pivotchart
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RelationalChart" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RelationalChart.svc or RelationalChart.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Relational : IRelational
    {
        PivotChart PivotChart = new PivotChart();
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        public Dictionary<string, object> Initialize(string action, string currentReport, string customObject)
        {
            BindData();
            return PivotChart.GetJsonData(action, ProductSales.GetSalesData());
        }

        public Dictionary<string, object> Drill(string action, string drilledSeries)
        {
            BindData();
            return PivotChart.GetJsonData(action, ProductSales.GetSalesData(), drilledSeries);
        }
               

        private void BindData()
        {
            this.PivotChart.PivotEngine.PivotRows.Add(new PivotItem { FieldMappingName = "Country", FieldHeader = "Country", TotalHeader = "Total", ShowSubTotal = false });
            this.PivotChart.PivotEngine.PivotRows.Add(new PivotItem { FieldMappingName = "State", FieldHeader = "State", TotalHeader = "Total" });
            this.PivotChart.PivotEngine.PivotRows.Add(new PivotItem { FieldMappingName = "Date", FieldHeader = "Date", TotalHeader = "Total" });
            this.PivotChart.PivotEngine.PivotColumns.Add(new PivotItem { FieldMappingName = "Product", FieldHeader = "Product", TotalHeader = "Total", ShowSubTotal = false });
            this.PivotChart.PivotEngine.PivotCalculations.Add(new PivotComputationInfo { CalculationName = "Amount", Description = "Amount", FieldHeader = "Amount", FieldName = "Amount", Format = "C", SummaryType = Syncfusion.PivotAnalysis.Base.SummaryType.DoubleTotalSum });
        }
    }
}
