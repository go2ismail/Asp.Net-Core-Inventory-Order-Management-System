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

namespace EJServices.Wcf.Pivotchart
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRelationalChart" in both code and config file together.
    [ServiceContract]
    public interface IRelational
    {
        [OperationContract]
        Dictionary<string, object> Initialize(string action, string currentReport, string customObject);
        [OperationContract]
        Dictionary<string, object> Drill(string action, string drilledSeries);
    }
}
