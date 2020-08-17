using System;
namespace SafeAuto.Models
{
    public class Trips
    {
        public string DriveName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int MilesDriven { get; set; }
    }
}
