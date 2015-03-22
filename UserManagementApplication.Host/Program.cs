using System;
using System.ServiceModel;
using System.ServiceModel.Description;
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
                    Console.WriteLine("Session services started! \n[TCP] >> " + sessionServiceHost.BaseAddresses[0].AbsoluteUri);

                    Console.WriteLine();

                    userServiceHost.Open();
                    Console.WriteLine("User services started! \n[TCP] >> " + sessionServiceHost.BaseAddresses[0].AbsoluteUri);

                    Console.WriteLine();
                    Console.WriteLine("Press enter to terminate service hosts...");
                    Console.ReadLine();
                }

                Console.WriteLine("Service hosts terminated.");

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
            serviceHost.AddServiceEndpoint(contractType, new NetTcpBinding()
                {
                    ReceiveTimeout = TimeSpan.FromMinutes(5),
                    SendTimeout = TimeSpan.FromMinutes(5),
                    MaxBufferSize = 655360,
                    MaxBufferPoolSize = 655360,
                    MaxReceivedMessageSize = 655360

                }, serviceAddress);
            serviceHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

            return serviceHost;
        }
    }
}
