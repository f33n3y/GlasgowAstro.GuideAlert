using GlasgowAstro.GuideAlert.Helpers;
using GlasgowAstro.GuideAlert.Interfaces;
using GlasgowAstro.GuideAlert.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;

namespace GlasgowAstro.GuideAlert
{
    /// <summary>
    /// Client object used to connect to PHD's TCP service
    /// and capture the event notification messages
    /// </summary>
    public class PhdClient : IPhdClient
    {
        //private const string Host = "localhost"; // TODO: Read from config
        //private const int Port = 4400; // TODO: Read from config
        private readonly string hostname;
        private readonly int port;
        private TcpClient client;
        private StreamReader streamReader;
        private int starLossCount = 0;

        public PhdClient(/*string hostname, int port*/)
        {            
            //this.hostname = hostname;
            //this.port = port;
        }

        /// <summary>
        /// Peforms initial setup of TCP connection and
        /// reads first line from stream to test connection
        /// </summary>
        /// <returns>Boolean indicating successful TCP connnection</returns>
        public bool ConnectAndTest()
        {
            try
            {
                client = new TcpClient(hostname, port);
                streamReader = new StreamReader(client.GetStream());
                var eventJson = streamReader.ReadLine();
                if (!string.IsNullOrWhiteSpace(eventJson))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                // TODO: Logging
            }
            return false;
        }

        /// <summary>
        /// Reads lines from event stream to look for star loss events
        /// </summary>
        /// <returns></returns>
        public bool WatchForStarLossEvents()
        {
            if (client == null || streamReader == null)
            {
                return false;
            }

            while (true)
            {
                var eventJson = streamReader.ReadLine();

                if (!string.IsNullOrWhiteSpace(eventJson))
                {
                    //Console.WriteLine(eventJson);

                    try
                    {
                        var phdEvent = JsonConvert.DeserializeObject<PhdEvent>(eventJson);

                        if (phdEvent != null && phdEvent.Event.Equals("StarLost"))
                        {
                            starLossCount++;
                            ConsoleHelper.StarLostWarning();
                            if (starLossCount == 5) // TODO: Read this value from config
                            {
                                // TODO: Fire off notification
                                //var sc = new SlackClient();
                                //sc.SendAlert("Star lost!");
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
    }
}
