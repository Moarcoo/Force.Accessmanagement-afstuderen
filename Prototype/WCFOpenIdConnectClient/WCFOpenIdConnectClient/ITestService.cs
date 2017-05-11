using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFOpenIdConnectClient
{
    [ServiceContract]
    public interface ITestService
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        MortgageFile GetMortgageFileForUser(string dossierId);

    }

    [DataContract]
    public class MortgageFile
    {
        [DataMember]
        public Guid Guid { get; set; }
        [DataMember]
        public bool Bouwdepot { get; set; }
        [DataMember]
        public double RentePercentage { get; set; }
        [DataMember]
        public string Whiteblabel { get; set; }
    }
}
