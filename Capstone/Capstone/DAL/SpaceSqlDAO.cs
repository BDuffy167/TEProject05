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
        private const string SqlGetSpaceInfo = "SELECT id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy FROM space";
        private const string SqlInputSpaceMenuChoice = "SELECT id, name, open_from, open_to, daily_rate, max_occupancy FROM space";
        public SpaceSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Space> GetSpaceInfo(string name)
        {
            IList<Space> spaces = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy FROM space WHERE spaces = @spaces;", conn);
                    cmd.Parameters.AddWithValue("@spaces", spaces);

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

            public IList<Space> InputSpaceMenuChoice(Venue venue)
            {
                List<Space> spaces = new List<Space>();

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("SELECT id, name, open_from, open_to, daily_rate, max_occupancy FROM space", conn);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Space space = new Space();

                            space.VenueName = Convert.ToString(reader["name"]);
                            spaces.Add(space);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error retrieving venues.");
                    Console.WriteLine(ex.Message);

                    throw;
                }

                return spaces;
            }
        private Space ConvertReaderToSpace(SqlDataReader reader, IList<Space> spaces)
        {
            Space space = new Space();

            if (reader["open_from"] != DBNull.Value)
            {
                space.OpenFrom = Convert.ToDateTime(reader["open_from"]);
            }
            if (reader["open_to"] != DBNull.Value)
            {
                space.OpenTo = Convert.ToDateTime(reader["open_to"]);
            }

            space.SpaceId = Convert.ToInt32(reader["id"]);
            space.VenueID = Convert.ToInt32(reader["venue_id"]);
            space.VenueName = Convert.ToString(reader["name"]);
            space.IsAccessible = Convert.ToBoolean(reader["is_accessible"]);
            space.OpenFrom = Convert.ToDateTime(reader["open_from"]);
            space.OpenTo = Convert.ToDateTime(reader["open_to"]);
            space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
            space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);

            return space;
        }

    }
}
