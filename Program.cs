using Microsoft.Extensions.Hosting;
using rc_program;
using RingCentral;
using DotNetEnv;
using System.IO;
using ServiceStack.Text;
namespace Busy_Light
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
       public static (string clientId, string clientSecret, string serverUrl, string redirectUri) LoadEnvVariables()
        {
            string envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rccalllight.env");
            if (!File.Exists(envPath))
                throw new FileNotFoundException($".env file not found at {envPath}");
            DotNetEnv.Env.Load(envPath);
            Console.WriteLine("Looking for .env in: " + AppDomain.CurrentDomain.BaseDirectory);
            var clientId = DotNetEnv.Env.GetString("CLIENT_ID");
            var clientSecret = DotNetEnv.Env.GetString("CLIENT_SECRET");
            var serverUrl = DotNetEnv.Env.GetString("SERVER_URL");
            var redirectUri = DotNetEnv.Env.GetString("REDIRECT_URI");
            if (string.IsNullOrWhiteSpace(clientId) ||
        string.IsNullOrWhiteSpace(clientSecret) ||
        string.IsNullOrWhiteSpace(serverUrl))
            {
                throw new Exception(".env variables CLIENT_ID, CLIENT_SECRET, SERVER_URL are required!");
            }

            return (clientId, clientSecret, serverUrl, redirectUri);
        }
        [STAThread]
        static void Main()
        {
            
            ApplicationConfiguration.Initialize();
           
            // load .env variables
            var (clientId, clientSecret, serverUrl, redirectUri) = LoadEnvVariables();
            
            // initialize RestClient
            var restClient = new RestClient(clientId, clientSecret, serverUrl);
            
            var tokenService = new Busy_Light.TokenService();

            Application.Run(new Form1(restClient, tokenService, redirectUri));

        }
    }
}