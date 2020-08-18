using System;
namespace SafeAuto.Models
{
    public class Trip
    {
        public string DriverName { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int MilesDriven { get; set; }
    }
}
