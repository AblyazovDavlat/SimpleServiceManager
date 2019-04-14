using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wpf_manager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceManager" in both code and config file together.
    [ServiceContract(CallbackContract =typeof(IServiceManagerCallback))]
    public interface IServiceManager
    {
        [OperationContract]
        int Connect(string name);

        [OperationContract]
        void Disconnect(int id);

        [OperationContract(IsOneWay = true)]
        void SendMsg(string msg, int id);
    }
    
    public interface IServiceManagerCallback
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallBack(string msg);
    }
}
