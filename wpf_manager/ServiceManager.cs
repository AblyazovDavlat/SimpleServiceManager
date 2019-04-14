using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wpf_manager
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceManager : IServiceManager
    {
        Dictionary<ServerUser, List<ServiceData>> users = new Dictionary<ServerUser, List<ServiceData>>();
        int nextID = 1;

        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                ID = nextID,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextID++;

            users.Add(user, new List<ServiceData>());
            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.Keys.FirstOrDefault(i => i.ID == id);
            if (user != null)
            {
                users.Remove(user);
            }
        }

        public void SendChangeServiceStatus(string nameService, string status, int id)
        {
            var user = users.Keys.FirstOrDefault(i => i.ID == id);
            List<ServiceData> services = new List<ServiceData>();
            ServiceData service;
            int index;

            if (user != null)
            {
                services = users[user];
                service = services.Find(x => x.name == nameService);
                index = users[user].IndexOf(service);
                users[user][index].status = status;
            }

            user.operationContext.GetCallbackChannel<IServiceManagerCallback>().MsgCallBack("Статус службы изменен на " + status);
            user.operationContext.GetCallbackChannel<IServiceManagerCallback>().ChangeStatusCallBack(nameService, status);
        }

        public void SendMsg(string msg, int id)
        {

            string answer = DateTime.Now.ToShortTimeString();

            var user = users.Keys.FirstOrDefault(i => i.ID == id);
            if (user != null)
            {
                answer += " " + user.Name + " ";
            }
            answer += msg;
            user.operationContext.GetCallbackChannel <IServiceManagerCallback>().MsgCallBack(answer);

        }

        public void SendServices(List<ServiceData> services, int id)
        {
            var user = users.Keys.FirstOrDefault(i => i.ID == id);
            if (user != null)
            {
                users[user] = services;
            }
            user.operationContext.GetCallbackChannel<IServiceManagerCallback>().MsgCallBack("Список сервисов отправлен.");
            foreach (ServiceData serviceData in services)
            {
                user.operationContext.GetCallbackChannel<IServiceManagerCallback>().MsgCallBack(serviceData.name + " " + serviceData.status);
            }
            
        }
    }
}
