using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Engine.Engine.WCF_System
{
    [ServiceContract(Namespace = "http://Simple_Engine")]
    interface IWcf_Engine
    {
        

        [OperationContract]
         void DisplayMessage(string message);
    }
}
