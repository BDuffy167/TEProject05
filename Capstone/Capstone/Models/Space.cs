﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int SpaceId { get; set; }
        public int VenueID { get; set; }
        public string Name { get; set; }
        public bool IsAccessible { get; set; }
        public DateTime OpenFrom { get; set; }
        public DateTime OpenTo { get; set; }
        public decimal DailyRate { get; set; }
        public int MaxOccupancy { get; set; }
    }
}