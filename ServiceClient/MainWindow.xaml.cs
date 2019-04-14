using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpf_manager;

namespace ServiceClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ServiceManager.IServiceManagerCallback
    {
        bool isConnected = false;
        ServiceManager.ServiceManagerClient client;
        int ID;

        ServiceController[] scServices;
        List<ServiceData> serviceMessages;

        public MainWindow()
        {
            InitializeComponent();
        }

        void ConnectUser()
        {
            if (!isConnected)
            {
                client = new ServiceManager.ServiceManagerClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(tbPCName.Text);
                tbPCName.IsEnabled = false;
                btnConnect.Content = "Disconnect";
                isConnected = true;

                serviceMessages = new List<ServiceData>();
                scServices = ServiceController.GetServices();
                client.SendMsg("Подключен к серверу ", ID);
                client.SendServices(ServiceData.ParseServiceControllerToMessage(scServices), ID);
            }
        }
        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);
                client = null;
                tbPCName.IsEnabled = true;
                btnConnect.Content = "Connect";
                isConnected = false;
            }
        }

        public void ChangeStatusCallBack(string nameService, string status)
        {
            ServiceController serviceController = scServices.FirstOrDefault(service => service.ServiceName == nameService);

            if (serviceController != null)
            {
                if (status == "Running")
                {
                    serviceController.Start();
                }
                if (status == "Stopped")
                {
                    serviceController.Stop();
                }
            }
            else
            {
                lbLog.Items.Add("Сервиса " + nameService + " не существует");
            }

        }


        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void MsgCallBack(string msg)
        {
            lbLog.Items.Add(msg);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        private void BtnEnableService_Click(object sender, RoutedEventArgs e)
        {
            client.SendChangeServiceStatus(tbServiceName.Text, "Running", ID);
        }

        private void BtnDisableService_Click(object sender, RoutedEventArgs e)
        {
            client.SendChangeServiceStatus(tbServiceName.Text, "Stopped", ID);
        }
    }
}
