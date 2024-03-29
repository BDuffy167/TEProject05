﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int SpaceId { get; set; }
        public int VenueId { get; set; }
        public string SpaceName { get; set; }
        public string VenueName { get; set; }
        public bool IsAccessible { get; set; }
        public int OpenFrom { get; set; } = 13; //If the DB is Null, when converting to a month name it returns an empty string.
        public int OpenTo { get; set; } = 13;
        public double DailyRate { get; set; } = 0.00d;
        public int MaxOccupancy { get; set; }

        public int SpaceVenueId
        {
            get
            {
                string strResult = this.VenueId.ToString() + this.SpaceId.ToString();

                int intResult = Convert.ToInt32(strResult);

                return intResult;
            }
        }
    }
}
