using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
   public  class SpaceSqlDAO: ISpaceDAO
    {
        private readonly string connectionString;
        private const string SqlGetSpaceInfo = "SELECT * FROM space";

        public SpaceSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Space> GetSpaceInfo()
        {
            IList<Space> spaces = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SqlGetSpaceInfo, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = new Space();

                        space.SpaceName = Convert.ToString(reader["name"]);

                        spaces.Add(space);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occured communicating with the database");
                Console.WriteLine(ex.Message);
            }
            return spaces;
        }
        private Space ConvertReaderToSpace(SqlDataReader reader, IList<Space> spaces)
        {
            Space space = new Space();

            space.SpaceId = Convert.ToInt32(reader["id"]);
            space.VenueID = Convert.ToInt32(reader["venue_id"]);
            space.SpaceName = Convert.ToString(reader["name"]);
            space.IsAccessible = Convert.ToBoolean(reader["is_accessible"]);
            space.OpenFrom = Convert.ToDateTime(reader["open_from"]);
            space.OpenTo = Convert.ToDateTime(reader["open_to"]);
            space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
            space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);

            return space;
        }

    }
}
