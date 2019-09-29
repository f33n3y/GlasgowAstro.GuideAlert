namespace GlasgowAstro.GuideAlert.Models
{
    /// <summary>
    /// Represents an event notification message sent
    /// by PHD server
    /// </summary>
    public class PhdEvent
    {
        /// <summary>
        /// Gets or sets the name of a event
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of an event 
        /// in seconds from epoch, including fractional seconds
        /// </summary>
        public double Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the hostname of the machine running PHD
        /// </summary>
        public string Host { get; set; }        

        /// <summary>
        /// Gets or sets the error message returned when star is lost
        /// </summary>
        public string Status { get; set; }
    }
}
