using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using HelloWorld.Contract;
using HelloWorld.Service;
using System.Data.SQLite;


namespace HostAnwendung
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(Service), new Uri("http://localhost:80/HelloWorld")))
            {
                host.AddServiceEndpoint(typeof(IService), new BasicHttpBinding(), "");
                host.Open();
                Console.WriteLine("Service running. Press ENTER to stop.");
                Console.ReadLine();
            }
        }

    }
}

