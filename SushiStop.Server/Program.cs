using System.Net;

namespace SushiStop.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter port: ");
            string? portString = Console.ReadLine();
            if (int.TryParse(portString, out int port))
                Console.WriteLine($"TCP server port: {port}");
            else
                return;

            Console.WriteLine();

            // Create a new TCP chat server
            var server = new SushiStopServer(IPAddress.Any, port);

            // Start the server
            Console.Write("Server starting... ");
            server.Start();
            Console.WriteLine("Done! Press return to end the server.");

            while (true)
            {
                string? line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;
            }

            // Stop the server
            Console.Write("Server stopping... ");
            server.Stop();
            Console.WriteLine("Done!");
        }
    }
}
