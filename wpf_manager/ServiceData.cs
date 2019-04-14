using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Runtime.Serialization;

namespace wpf_manager
{
    [DataContractAttribute]
    public class ServiceData
    {
        [DataMember]
        public string name;

        [DataMember]
        public string status;

        public ServiceData(string name, string status)
        {
            this.name = name;
            this.status = status;
        }

        public static List<ServiceData> ParseServiceControllerToMessage(ServiceController[] serviceControllers)
        {
            List<ServiceData> serviceMessage = new List<ServiceData>();

            foreach (ServiceController scTemp in serviceControllers)
            {
                ServiceData sm = new ServiceData(scTemp.DisplayName, scTemp.Status.ToString());
                serviceMessage.Add(sm);
            }

            return serviceMessage;
        }
    }
}
