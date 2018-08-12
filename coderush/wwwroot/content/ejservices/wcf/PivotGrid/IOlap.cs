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

namespace EJServices.Wcf.Pivotgrid
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOlapGrid" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IOlap
    {
        [OperationContract]
        Dictionary<string, object> Initialize(string action, string gridLayout, bool enablePivotFieldList, object customObject);

        [OperationContract]
        Dictionary<string, object> Drill(string action, string cellPosition, string currentReport,
                                                 string headerInfo, string layout, object customObject);
        [OperationContract]
        Dictionary<string, object> Paging(string action, string pagingInfo, string currentReport, string gridLayout, object customObject);

        [OperationContract]
        Dictionary<string, object> DropNode(string action, string dropType, string nodeInfo, string filterParams, string gridLayout, string currentReport, object customObject);

        [OperationContract]
        Dictionary<string, object> RemoveButton(string action, string headerInfo, string gridLayout, string currentReport, object customObject);

        [OperationContract]
        Dictionary<string, object> FetchMembers(string action, string headerTag, string currentReport);

        [OperationContract]
        Dictionary<string, object> Filtering(string action, string filterParams, string gridLayout, string currentReport, object customObject);

        [OperationContract]
        Dictionary<string, object> ExpandMember(string action, bool checkedStatus, string parentNode, string tag, string cubeName, string currentReport);

        [OperationContract]
        void Export(System.IO.Stream stream);

        [OperationContract]
        Dictionary<string, object> DeferUpdate(string action, string gridLayout, string filterParams, string currentReport, object customObject);
        [OperationContract]
        Dictionary<string, object> SaveReport(string reportName, string operationalMode, string olapReport, string clientReports);
        [OperationContract]
        Dictionary<string, object> LoadReportFromDB(string action, string gridLayout, bool enablePivotFieldList, object customObject, string reportName, string operationalMode, string olapReport, string clientReports);
    }
}
