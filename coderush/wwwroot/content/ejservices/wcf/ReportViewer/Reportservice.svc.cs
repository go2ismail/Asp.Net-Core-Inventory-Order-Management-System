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
using System.ServiceModel.Activation;
using System.Text;

namespace EJServices.Wcf.ReportViewer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "reportservice" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select reportservice.svc or reportservice.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Reportservice : IReportservice
    {
        public List<OrderDetails> GetOrderDetails()
        {
            List<OrderDetails> datas = new List<OrderDetails>();
            OrderDetails data = null;
            data = new OrderDetails()
            {
                OrderID = 10248,
                CustomerID = "VINET",
                EmployeeID = 5,
                Freight = 32.38,
                ShipCity = "Reims",
                ShipCountry = "France"
            };
            datas.Add(data);
            data = new OrderDetails()
            {
                OrderID = 10249,
                CustomerID = "TOMSP",
                EmployeeID = 6,
                Freight = 11.61,
                ShipCity = "Munster",
                ShipCountry = "Germany"
            };
            datas.Add(data);

            data = new OrderDetails()
            {
                OrderID = 10250,
                CustomerID = "HANAR",
                EmployeeID = 4,
                Freight = 65.83,
                ShipCity = "Rio de Janeiro",
                ShipCountry = "Brazil"
            };
            datas.Add(data);

            data = new OrderDetails()
            {
                OrderID = 10251,
                CustomerID = "VICTE",
                EmployeeID = 3,
                Freight = 41.34,
                ShipCity = "Lyon",
                ShipCountry = "France"
            };
            datas.Add(data);

            data = new OrderDetails()
            {
                OrderID = 10252,
                CustomerID = "SUPRD",
                EmployeeID = 4,
                Freight = 51.30,
                ShipCity = "Charleroi",
                ShipCountry = "Belgium"
            };
            datas.Add(data);

            data = new OrderDetails()
            {
                OrderID = 10253,
                CustomerID = "HANAR",
                EmployeeID = 3,
                Freight = 58.17,
                ShipCity = "Rio de Janeiro",
                ShipCountry = "Brazil"
            };
            datas.Add(data);

            data = new OrderDetails()
            {
                OrderID = 10254,
                CustomerID = "CHOPS",
                EmployeeID = 5,
                Freight = 22.98,
                ShipCity = "Bern",
                ShipCountry = "Switzerland"
            };
            datas.Add(data);

            data = new OrderDetails()
            {
                OrderID = 10255,
                CustomerID = "RICSU",
                EmployeeID = 9,
                Freight = 148.33,
                ShipCity = "Geneve",
                ShipCountry = "Switzerland"
            };
            datas.Add(data);
            data = new OrderDetails()
            {
                OrderID = 10256,
                CustomerID = "WELLI",
                EmployeeID = 3,
                Freight = 13.97,
                ShipCity = "Resende",
                ShipCountry = "Brazil"
            };
            datas.Add(data);

            data = new OrderDetails()
            {
                OrderID = 10257,
                CustomerID = "HILAA",
                EmployeeID = 4,
                Freight = 81.91,
                ShipCity = "San Christobal",
                ShipCountry = "Venezuela"
            };
            datas.Add(data);
            data = new OrderDetails()
            {
                OrderID = 10258,
                CustomerID = "ERNSH",
                EmployeeID = 1,
                Freight = 140.51,
                ShipCity = "Graz",
                ShipCountry = "Austria"
            };
            datas.Add(data);
            data = new OrderDetails()
            {
                OrderID = 10259,
                CustomerID = "CENTC",
                EmployeeID = 4,
                Freight = 3.25,
                ShipCity = "Mwxico D.F",
                ShipCountry = "Mexico"
            };
            datas.Add(data);

            data = new OrderDetails()
            {
                OrderID = 10260,
                CustomerID = "OTTIk",
                EmployeeID = 4,
                Freight = 55.09,
                ShipCity = "Koln",
                ShipCountry = "Germany"
            };
            datas.Add(data);

            data = new OrderDetails()
            {
                OrderID = 10261,
                CustomerID = "QUEDE",
                EmployeeID = 4,
                Freight = 3.05,
                ShipCity = "Rio de Janeiro",
                ShipCountry = "Brazil"
            };
            datas.Add(data);

            return datas;
        }
    }
}
