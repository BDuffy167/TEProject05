using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
   public class Venue
    {
        public int VenueId { get; set; }
        public string VenueName { get; set; }
        public int CityId { get; set; } //Might need to remove//
        public string Description { get; set; }
        public string CityName { get; set; }
        public string StateAbbreviation { get; set; } //Might need to remove//
        public string StateName { get; set; }
        public IList<int> CategoryId { get; set; } = new List<int>();
        public IList<string> CategoryName { get; set; } = new List<string>();
    }
}
