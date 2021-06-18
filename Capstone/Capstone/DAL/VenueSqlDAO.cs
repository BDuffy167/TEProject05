using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueSqlDAO: IVenueDAO
    {
        private readonly string connectionString;

        public VenueSqlDAO (string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
