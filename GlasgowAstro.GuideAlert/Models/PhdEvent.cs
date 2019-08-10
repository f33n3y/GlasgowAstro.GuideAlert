using System;
using System.Collections.Generic;
using System.Text;

namespace GlasgowAstro.GuideAlert.Models
{
    public class PhdEvent
    {
        public string Event { get; set; }

        public double Timestamp { get; set; }

        public string Host { get; set; }        

        public string Status { get; set; }
    }
}
