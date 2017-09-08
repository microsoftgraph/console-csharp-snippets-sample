using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace console_csharp_snippets_sample
{
    class Program
    {
        static void Main(string[] args)
        {
            // record start DateTime of execution
            string currentDateTime = DateTime.Now.ToUniversalTime().ToString();

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            Console.WriteLine("Run operations for signed-in user, or in app-only mode.\n");
            Console.WriteLine("[a] - app-only\n[u] - as user\n[b] - both as user first, and then as app.\nPlease enter your choice:\n");

            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.KeyChar)
            {
                case 'a':
                    Console.WriteLine("\nRunning app-only mode.\n\n");
                    AppMode.AppModeRequests();
                    break;
                case 'u':
                    Console.WriteLine("\nRunning in user mode.\n\n");
                    UserMode.UserModeRequests();
                    break;
                case 'b':
                    Console.WriteLine("\nRunning user mode, followed by app-only mode.\n\n");
                    UserMode.UserModeRequests();
                    Console.WriteLine("\nFinished running user mode. Press any key to run app-only mode.\n\n");
                    Console.ReadKey();
                    AppMode.AppModeRequests();                   
                    break;
                default:
                    Console.WriteLine("\nSelection not recognized. Running in user mode.\n\n");
                    UserMode.UserModeRequests();
                    break;
            }

            //*********************************************************************************************
            // End of Demo Console App
            //*********************************************************************************************

            Console.WriteLine("\nCompleted at {0} \n Press Any Key to Exit.", currentDateTime);
            Console.ReadKey();
        }
    }
}
