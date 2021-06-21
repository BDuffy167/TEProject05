using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
   public interface ISpaceDAO
    {
        List<Space> GetSpaceInfo(string name);
        List<Space> GetOpenSpaces(string venueName, DateTime resStartDate, DateTime resEndDate, int resAttendance);
    }
}
