using GlasgowAstro.GuideAlert.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;

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

            //TODO: Move to classes
            try
            {
                TcpClient client = new TcpClient(Host, Port); 
                StreamReader streamReader = new StreamReader(client.GetStream());
               
                while (true)
                {                  
                    var eventJson = streamReader.ReadLine();

                    if (!string.IsNullOrEmpty(eventJson))
                    {
                        Console.WriteLine(eventJson);

                        try
                        {
                            var phdEvent = JsonConvert.DeserializeObject<PhdEvent>(eventJson);

                            if (phdEvent != null && phdEvent.Event.Equals("StarLost"))
                            {
                                Console.WriteLine("STAR LOST! Sending notification...");
                                // TODO: Fire off notification...
                                //(rate limit, wait for X seconds of star loss? - user chooses duration)
                            }
                        }
                        catch (Exception e)
                        {
                            // TODO: Logging
                        }
                    }                 
                }
            }
            catch (Exception e)
            {
                // TODO: Logging
            }
        }
    }
}
