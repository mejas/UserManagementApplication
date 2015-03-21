using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Engine.Services;
using UserManagementApplication.Remoting.Services;

namespace UserManagementApplication.Host
{
    class Program
    {
        private const string SESSION_SVC_ADDRESS = "net.tcp://localhost:8080/UserManagementApplication/Session";
        private const string USER_SVC_ADDRESS = "net.tcp://localhost:8080/UserManagementApplication/User";

        static void Main(string[] args)
        {
            try
            {

                Console.WriteLine("Initializing service hosts...\n");

                using(var sessionServiceHost = createServiceHost(SESSION_SVC_ADDRESS, typeof(ISessionServices),typeof(SessionServices)))
                using (var userServiceHost = createServiceHost(USER_SVC_ADDRESS, typeof(IUserServices), typeof(UserServices)))
                {
                    sessionServiceHost.Open();
                    Console.WriteLine("Session services started! \n[TCP] >> " + SESSION_SVC_ADDRESS);

                    Console.WriteLine();

                    userServiceHost.Open();
                    Console.WriteLine("User services started! \n[TCP] >> " + USER_SVC_ADDRESS);

                    Console.WriteLine();
                    Console.WriteLine("Press any key to terminate service hosts...");
                }

            }
            catch (Exception eX)
            {
                Console.WriteLine("Services terminated. \n\nError Message [" + eX.Message + "]");
            }

            Console.Read();
        }

        private static ServiceHost createServiceHost(string address, Type contractType, Type implementationType)
        {
            Uri serviceAddress = new Uri(address);

            ServiceHost serviceHost = new ServiceHost(implementationType, serviceAddress);

            serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior());
            serviceHost.AddServiceEndpoint(contractType, new NetTcpBinding(), serviceAddress);
            serviceHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

            return serviceHost;
        }
    }
}
