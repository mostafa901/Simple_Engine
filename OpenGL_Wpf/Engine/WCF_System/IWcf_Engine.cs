using System.ServiceModel;

namespace Simple_Engine.Engine.WCF_System
{
    [ServiceContract(Namespace = "http://Simple_Engine",ProtectionLevel = System.Net.Security.ProtectionLevel.None)]
    internal interface IWcf_Engine
    {
        [OperationContract]
        void DisplayMessage(string message);

        [OperationContract]
        string ScopeToModel(string jsGeometry);
    }
}