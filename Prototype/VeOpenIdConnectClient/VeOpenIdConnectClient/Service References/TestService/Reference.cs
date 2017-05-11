﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VeOpenIdConnectClient.TestService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceFault", Namespace="http://schemas.datacontract.org/2004/07/WCFOpenIdConnectClient")]
    [System.SerializableAttribute()]
    public partial class ServiceFault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MortgageFile", Namespace="http://schemas.datacontract.org/2004/07/WCFOpenIdConnectClient")]
    [System.SerializableAttribute()]
    public partial class MortgageFile : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool BouwdepotField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid GuidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double RentePercentageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WhiteblabelField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Bouwdepot {
            get {
                return this.BouwdepotField;
            }
            set {
                if ((this.BouwdepotField.Equals(value) != true)) {
                    this.BouwdepotField = value;
                    this.RaisePropertyChanged("Bouwdepot");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Guid {
            get {
                return this.GuidField;
            }
            set {
                if ((this.GuidField.Equals(value) != true)) {
                    this.GuidField = value;
                    this.RaisePropertyChanged("Guid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double RentePercentage {
            get {
                return this.RentePercentageField;
            }
            set {
                if ((this.RentePercentageField.Equals(value) != true)) {
                    this.RentePercentageField = value;
                    this.RaisePropertyChanged("RentePercentage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Whiteblabel {
            get {
                return this.WhiteblabelField;
            }
            set {
                if ((object.ReferenceEquals(this.WhiteblabelField, value) != true)) {
                    this.WhiteblabelField = value;
                    this.RaisePropertyChanged("Whiteblabel");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TestService.ITestService")]
    public interface ITestService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITestService/GetData", ReplyAction="http://tempuri.org/ITestService/GetDataResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(VeOpenIdConnectClient.TestService.ServiceFault), Action="http://tempuri.org/ITestService/GetDataServiceFaultFault", Name="ServiceFault", Namespace="http://schemas.datacontract.org/2004/07/WCFOpenIdConnectClient")]
        string GetData(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITestService/GetData", ReplyAction="http://tempuri.org/ITestService/GetDataResponse")]
        System.Threading.Tasks.Task<string> GetDataAsync(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITestService/GetMortgageFileForUser", ReplyAction="http://tempuri.org/ITestService/GetMortgageFileForUserResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(VeOpenIdConnectClient.TestService.ServiceFault), Action="http://tempuri.org/ITestService/GetMortgageFileForUserServiceFaultFault", Name="ServiceFault", Namespace="http://schemas.datacontract.org/2004/07/WCFOpenIdConnectClient")]
        VeOpenIdConnectClient.TestService.MortgageFile GetMortgageFileForUser(string dossierId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITestService/GetMortgageFileForUser", ReplyAction="http://tempuri.org/ITestService/GetMortgageFileForUserResponse")]
        System.Threading.Tasks.Task<VeOpenIdConnectClient.TestService.MortgageFile> GetMortgageFileForUserAsync(string dossierId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITestServiceChannel : VeOpenIdConnectClient.TestService.ITestService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TestServiceClient : System.ServiceModel.ClientBase<VeOpenIdConnectClient.TestService.ITestService>, VeOpenIdConnectClient.TestService.ITestService {
        
        public TestServiceClient() {
        }
        
        public TestServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TestServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TestServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TestServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetData(int value) {
            return base.Channel.GetData(value);
        }
        
        public System.Threading.Tasks.Task<string> GetDataAsync(int value) {
            return base.Channel.GetDataAsync(value);
        }
        
        public VeOpenIdConnectClient.TestService.MortgageFile GetMortgageFileForUser(string dossierId) {
            return base.Channel.GetMortgageFileForUser(dossierId);
        }
        
        public System.Threading.Tasks.Task<VeOpenIdConnectClient.TestService.MortgageFile> GetMortgageFileForUserAsync(string dossierId) {
            return base.Channel.GetMortgageFileForUserAsync(dossierId);
        }
    }
}
