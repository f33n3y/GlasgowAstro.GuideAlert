using System;
using System.Net.Sockets;
using System.Text;

namespace GlasgowAstro.GuideAlert
{
    class Program
    {
        const string Host = "localhost";
        const int Port = 4400;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("*** Guide Alert by GlasgowAstro *** \n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);

            try
            {
                TcpClient client = new TcpClient(Host, Port);
                NetworkStream stream = client.GetStream();
                byte[] readBuffer = new byte[256];

                while (true)
                {
                    var data = stream.Read(readBuffer, 0, readBuffer.Length);
                    string stringData = Encoding.ASCII.GetString(readBuffer, 0, data);
                    Console.WriteLine(stringData);
                }
            }
            catch (Exception e)
            {
                // TODO: Logging
            }
        }
    }
}
