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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOlapClient" in both code and config file together.
    [ServiceContract]
    public interface IOlap
    {
        [OperationContract]
        Dictionary<string, object> Initialize(string action, string customObject, string clientParams);
        [OperationContract]
        Dictionary<string, object> FetchMemberTreeNodes(string action, string dimensionName, string olapReport);
        [OperationContract]
        Dictionary<string, object> InitializeChart(string action, string currentReport, string customObject);
        [OperationContract]
        Dictionary<string, object> InitializeTreeMap(string action, string currentReport, string customObject);
        [OperationContract]
        Dictionary<string, object> DrillChart(string action, string drilledSeries, string olapReport, string clientReports);
        [OperationContract]
        Dictionary<string, object> DrillTreeMap(string action, string drillInfo, string olapReport, string clientReports);
        [OperationContract]
        Dictionary<string, object> InitializeGrid(string action, string currentReport, string gridLayout, string customObject);
        [OperationContract]
        Dictionary<string, object> DrillGrid(string action, string cellPosition, string currentReport, string clientReports, string headerInfo, string layout);
        [OperationContract]
        Dictionary<string, object> FilterElement(string action, string clientParams, string olapReport, string clientReports);
        [OperationContract]
        Dictionary<string, object> RemoveSplitButton(string action, string clientParams, string olapReport, string clientReports);
        [OperationContract]
        Dictionary<string, object> DropNode(string action, string dropType, string nodeInfo, string olapReport, string clientReports);
        [OperationContract]
        Dictionary<string, object> CubeChange(string action, string cubeName, string olapReport, string clientReports, string clientParams);
        [OperationContract]
        Dictionary<string, object> MeasureGroup(string action, string measureGroupName, string olapReport, string clientReports);
        [OperationContract]
        Dictionary<string, object> CalculatedMember(string action, string olapReport, string clientReports, string caption, string expression, string memberType, string dimension, string formatString,string uniqueName);
        [OperationContract]
        Dictionary<string, object> ToolbarOperations(string action, string toolbarOperation, string clientInfo, string olapReport, string clientReports);
        [OperationContract]
        Dictionary<string, object> UpdateReport(string action, string clientParams, string olapReport, string clientReports);
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
        Dictionary<string, object> ExpandMember(string action, bool checkedStatus, string parentNode, string tag, string dimensionName, string cubeName, string olapReport, string clientReports);
        [OperationContract]
        void Export(System.IO.Stream stream);
        [OperationContract]
        string GetMDXQuery(string olapReport);
        [OperationContract]
        Dictionary<string, object> ToggleAxis(string action, string currentReport, string clientReports);
    }
}
