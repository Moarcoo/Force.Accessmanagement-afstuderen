using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WCFOpenIdConnectClient
{
    [ServiceContract]
    public interface ITestService
    {

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        Task<string> GetData(int value);

        // The FaultContract solution for throwing errors comes from:
        // http://stackoverflow.com/questions/12647615/wcf-service-exception-good-practices
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        Task<MortgageFile> GetMortgageFileForUser(string dossierId);

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

    [DataContract]
    public class ServiceFault
    {
        private string _message;

        public ServiceFault(string message)
        {
            _message = message;
        }

        [DataMember]
        public string Message { get { return _message; } set { _message = value; } }
    }
}
