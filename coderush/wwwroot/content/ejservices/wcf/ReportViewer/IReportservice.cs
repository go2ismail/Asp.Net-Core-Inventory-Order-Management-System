#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Runtime.Serialization;
namespace EJServices.Wcf.ReportViewer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Ireportservice" in both code and config file together.
    [ServiceContract]
    public interface IReportservice
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        List<OrderDetails> GetOrderDetails();
    }
    [DataContract]
    public class OrderDetails
    {
        double orderid = 0;
        string customerid = string.Empty;
        double employeeid = 0;
        double freight = 0;
        string shipcity = string.Empty;
        string shipcountry = string.Empty;

        [DataMember]
        public double OrderID { get; set; }
        [DataMember]
        public string CustomerID { get; set; }
        [DataMember]
        public double EmployeeID { get; set; }
        [DataMember]
        public double Freight { get; set; }
        [DataMember]
        public string ShipCity { get; set; }
        [DataMember]
        public string ShipCountry { get; set; }
    }
}
