using GlasgowAstro.GuideAlert.Helpers;
using GlasgowAstro.GuideAlert.Interfaces;
using GlasgowAstro.GuideAlert.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace GlasgowAstro.GuideAlert
{
    /// <summary>
    /// Client object used to connect to PHD's TCP service
    /// and capture the event notification messages
    /// </summary>
    public class PhdClient : IPhdClient
    {
        private const string DefaultPhdHostname = "localhost";
        private const ushort DefaultPhdPortNum = 4400;
        private const int LinesToReadForTest = 5;
        private readonly ILogger<PhdClient> _logger;
        private readonly GuideAlertSettings _guideAlertSettings;
        private TcpClient _tcpClient;
        private StreamReader _streamReader;

        public PhdClient(GuideAlertSettings guideAlertSettings, ILogger<PhdClient> logger)
        {
            _guideAlertSettings = guideAlertSettings;
            _logger = logger;
        }

        /// <summary>
        /// Performs initial setup of TCP connection and
        /// reads first few lines from stream to test connection
        /// </summary>
        /// <returns>Task with boolean indicating successful TCP connnection</returns>
        public async Task<bool> ConnectAndTestAsync()
        {
            var hostname = _guideAlertSettings?.PhdHost;
            var portNumber = _guideAlertSettings.PhdPort;

            if (string.IsNullOrWhiteSpace(_guideAlertSettings?.PhdHost))
            {
                _logger.LogWarning("No PHD hostname found in config. Falling back to localhost.");
                hostname = DefaultPhdHostname;
            }

            if (portNumber < ushort.MinValue && portNumber > ushort.MaxValue)
            {
                _logger.LogWarning("Invalid or missing port number in config. Falling back to port 4400.");
                portNumber = DefaultPhdPortNum;
            }

            try
            {
                _tcpClient = new TcpClient(hostname, portNumber);
                _streamReader = new StreamReader(_tcpClient.GetStream());
                var linesRead = 0;

                for (var i = 0; i < LinesToReadForTest; i++)
                {
                    var eventJson = await _streamReader?.ReadLineAsync();
                    if (!string.IsNullOrWhiteSpace(eventJson) && JsonConvert.DeserializeObject<PhdEvent>(eventJson) != null)
                    {
                        linesRead++;
                    }
                }

                if (linesRead == LinesToReadForTest)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to connect to Phd server.");
            }

            return false;
        }

        /// <summary>
        /// Reads lines from event stream to look for star loss events
        /// </summary>
        /// <returns></returns>
        public async Task<bool> WatchForStarLossEvents()
        {
            if (_tcpClient == null || _streamReader == null)
            {
                _logger.LogCritical("TCPClient or StreamReader is null.");
                return false;
            }

            bool starLost = false;

            do
            {
                var eventJson = await _streamReader.ReadLineAsync();

                if (!string.IsNullOrWhiteSpace(eventJson))
                {
                    if (_guideAlertSettings.LogPhdEventsToConsole)
                    {
                        Console.WriteLine(eventJson);
                    }

                    PhdEvent phdEvent = JsonConvert.DeserializeObject<PhdEvent>(eventJson);

                    if (phdEvent != null && phdEvent.Event.ToLower().Equals("starlost"))
                    {
                        ConsoleHelper.StarLostWarning();
                        _logger.LogInformation("Star lost, sending alert.");
                        starLost = true;
                    }
                }
            } while (!starLost);

            return starLost;
        }
    }
}
