#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EJServices.Wcf.Pivotclient
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRelationalClient" in both code and config file together.
    [ServiceContract]
    public interface IRelational
    {
        [OperationContract]
        Dictionary<string, object> Initialize(string action, string clientParams);
        [OperationContract]
        Dictionary<string, object> FetchMembers(string action, string currentReport, string customObject, string headerTag);
        [OperationContract]
        Dictionary<string, object> DrillChart(string action, string drilledSeries, string currentReport);
        [OperationContract]
        Dictionary<string, object> Filtering(string action, string filterParams, string currentReport, string customObject);
        [OperationContract]
		Dictionary<string, object> ValueSorting(string action, string valueSorting, string currentReport, string customObject);
        [OperationContract]
        Dictionary<string, object> DropNode(string action, string args);
        [OperationContract]
        Dictionary<string, object> ToolbarOperations(string action, string args);
        [OperationContract]
        Dictionary<string, object> SaveReportToDB(string reportName, string operationalMode, string analysisMode, string olapReport, string clientReports);
        [OperationContract]
        Dictionary<string, object> RemoveReportFromDB(string reportName, string operationalMode, string analysisMode);
        [OperationContract]
        Dictionary<string, object> RenameReportInDB(string selectedReport, string renameReport, string operationalMode, string analysisMode);
        [OperationContract]
        Dictionary<string, object> LoadReportFromDB(string reportName, string operationalMode, string analysisMode, string olapReport, string clientReports);
        [OperationContract]
        Dictionary<string, object> FetchReportListFromDB(string action, string operationalMode, string analysisMode);
        [OperationContract]
        Dictionary<string, object> Sorting(string action, string currentReport, string sortedHeaders);
        [OperationContract]
        void Export(System.IO.Stream stream);
    }
}
