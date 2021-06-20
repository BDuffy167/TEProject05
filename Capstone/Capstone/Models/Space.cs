using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int SpaceId { get; set; }
        public int VenueID { get; set; }
        public string SpaceName { get; set; }
        public string VenueName { get; set; }
        public bool IsAccessible { get; set; }
        public int? OpenFrom { get; set; }
        public int? OpenTo { get; set; }
        public double DailyRate { get; set; } = 0.00d;
        public int MaxOccupancy { get; set; }

       
    }
}
