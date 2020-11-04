using System;
using System.ServiceModel;

namespace Simple_Engine.Engine.WCF_System
{
    public static class Wcf_Engine
    {
        private static ServiceHost selfHost;

        public static void Start_Wcf_Engine()
        {
            Uri baseAddress = new Uri("http://localhost:8000/Simple_Engine/Engine/WCF_System/");
            // Step 2: Create a ServiceHost instance.
            selfHost = new ServiceHost(typeof(Wcf_EngineService));

            try
            {
                // Step 3: Add a service endpoint.
                selfHost.AddServiceEndpoint(typeof(IWcf_Engine), new WSHttpBinding(), nameof(Wcf_EngineService));

                // Step 5: Start the service.
                selfHost.Open();
                Console.WriteLine("The service is ready.");

                // Close the ServiceHost to stop the service.
                Console.WriteLine("Press <Enter> to terminate the service.");
                Console.WriteLine();
                Console.ReadLine();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
                selfHost.Abort();
                //   Debugger.Break();
            }
        }

        public static void Stop_Wcf_Engine()
        {
            selfHost?.Close();
        }
    }
}