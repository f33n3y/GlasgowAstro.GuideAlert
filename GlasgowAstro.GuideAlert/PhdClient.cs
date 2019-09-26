using GlasgowAstro.GuideAlert.Helpers;
using GlasgowAstro.GuideAlert.Interfaces;
using GlasgowAstro.GuideAlert.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
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
        private TcpClient client;
        private StreamReader streamReader;
        private readonly ILogger<PhdClient> logger;

        public PhdClient(ILogger<PhdClient> logger)
        {
            this.logger = logger;
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
                // TODO

                //client = new TcpClient(hostname, port);
                //streamReader = new StreamReader(client.GetStream());
                //var eventJson = streamReader.ReadLine();
                //if (!string.IsNullOrWhiteSpace(eventJson))
                //{
                //    return true;
                //}
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to connect to Phd server");
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

                //if (!string.IsNullOrWhiteSpace(eventJson))
                //{
                //    //Console.WriteLine(eventJson);

                //    try
                //    {
                //        var phdEvent = JsonConvert.DeserializeObject<PhdEvent>(eventJson);

                //        if (phdEvent != null && phdEvent.Event.Equals("StarLost"))
                //        {
                //            starLossCount++;
                //            ConsoleHelper.StarLostWarning();
                //            if (starLossCount == 5) // TODO: Read this value from config
                //            {
                //                // TODO: Fire off notification
                //                //var sc = new SlackClient();
                //                //sc.SendAlert("Star lost!");
                //            }
                //        }
                //    }
                //    catch (Exception e)
                //    {
                //        // TODO: Logging
                //    }
                //}
            }
        }
    }
}
