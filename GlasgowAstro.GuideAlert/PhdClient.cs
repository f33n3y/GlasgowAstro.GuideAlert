using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using GlasgowAstro.GuideAlert.Interfaces;
using GlasgowAstro.GuideAlert.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GlasgowAstro.GuideAlert
{
    /// <summary>
    /// Client object used to connect to PHD's TCP service
    /// and capture the event notification messages
    /// </summary>
    public class PhdClient : IPhdClient
    {
        private const string DefaultPhdHostname = "localhost";
        private const ushort DefaultPhdPort = 4400;
        private readonly ILogger<PhdClient> logger;
        private readonly GuideAlertSettings guideAlertSettings;

        private TcpClient tcpClient;
        private StreamReader streamReader;

        public PhdClient(GuideAlertSettings guideAlertSettings, ILogger<PhdClient> logger)
        {
            this.guideAlertSettings = guideAlertSettings;
            this.logger = logger;
        }

        /// <summary>
        /// Performs initial setup of TCP connection and
        /// reads first line from stream to test connection
        /// </summary>
        /// <returns>Boolean indicating successful TCP connnection</returns>
        public async Task<bool> ConnectAndTestAsync()
        {
            throw new NotImplementedException();

            try
            {
                //var hostname = guideAlertSettings.PhdHost;
                //var portNumber = guideAlertSettings.PhdPort;

                //if (string.IsNullOrWhiteSpace(hostname))
                //{
                //    logger.LogWarning("No PHD hostname found in config. Falling back to localhost");
                //    return false;
                //}

                //if (portNumber < ushort.MinValue && portNumber > ushort.MaxValue)
                //{
                //    logger.LogWarning("Invalid or missing port number in config. Falling back to port 4400");
                //    return false;
                //}

                //tcpClient = new TcpClient(hostname, portNumber);
                //streamReader = new StreamReader(tcpClient.GetStream());

                //var eventJson = await streamReader.ReadLineAsync();
                //if (!string.IsNullOrWhiteSpace(eventJson))
                //{
                //    return true;
                //}
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to connect to Phd server.");
            }

            return false;
        }

        /// <summary>
        /// Reads lines from event stream to look for star loss events
        /// </summary>
        /// <returns></returns>
        public bool WatchForStarLossEvents()
        {
            if (tcpClient == null || streamReader == null)
            {
                logger.LogError("TCPClient or StreamReader is null.");
                return false;
            }

            return true; // TODO

            //while (true)
            //{
            //    var eventJson = streamReader.ReadLine();

            //    if (!string.IsNullOrWhiteSpace(eventJson))
            //    {

            //        //Console.WriteLine(eventJson);

            //        try
            //        {
            //            var phdEvent = JsonConvert.DeserializeObject<PhdEvent>(eventJson);

            //            if (phdEvent != null && phdEvent.Event.Equals("StarLost"))
            //            {
            //                starLossCount++;
            //                ConsoleHelper.StarLostWarning();
            //                if (starLossCount == 5) // TODO: Read this value from config
            //                {
            //                    // TODO: Fire off notification
            //                    //var sc = new SlackClient();
            //                    //sc.SendAlert("Star lost!");
            //                }
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            logger.LogError(e, "Failed to send test Slack notification.");
            //        }
            //    }
            //}
        }
    }
}
