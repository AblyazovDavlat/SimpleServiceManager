using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ManagerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(wpf_manager.ServiceManager)))
            {
                host.Open();
                Console.WriteLine("Host is ready");
                Console.ReadLine();
            }
        }
    }
}
