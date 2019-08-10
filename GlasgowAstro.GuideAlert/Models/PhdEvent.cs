namespace GlasgowAstro.GuideAlert.Models
{
    /// <summary>
    /// Represents an event notification message sent
    /// by PHD server.
    /// </summary>
    public class PhdEvent
    {
        /// <summary>
        /// The name of the event
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// Timestamp of event in seconds from
        /// epoch, including fractional seconds
        /// </summary>
        public double Timestamp { get; set; }

        /// <summary>
        /// Hostname of the machine running PHD
        /// </summary>
        public string Host { get; set; }        

        /// <summary>
        /// Error message returned when star is lost
        /// </summary>
        public string Status { get; set; }
    }
}
