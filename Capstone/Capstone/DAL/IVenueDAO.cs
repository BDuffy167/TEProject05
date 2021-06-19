using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
   public interface IVenueDAO
    {
        IList<Venue> GetVenueNames();

        //IList<Venue> GetDetailedVenueInfo(string s);
    }
}
