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

        public List<Space> GetSpaceInfo(string venueName)
        {
            List<Space> spaces = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT v.name AS 'venue_name', s.name, s.open_from, s.open_to, s.daily_rate, s.max_occupancy FROM space s INNER JOIN venue v ON s.venue_id = v.id WHERE v.name = @venueName", conn);
                    cmd.Parameters.AddWithValue("@venueName", venueName);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = new Space();

                        space = ConvertReaderToSpace(reader);

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

            //public IList<Space> InputSpaceMenuChoice(Venue venue)
            //{
            //    List<Space> spaces = new List<Space>();

            //    try
            //    {
            //        using (SqlConnection conn = new SqlConnection(connectionString))
            //        {
            //            conn.Open();

            //            SqlCommand cmd = new SqlCommand("SELECT id, name, open_from, open_to, daily_rate, max_occupancy FROM space", conn);
            //            SqlDataReader reader = cmd.ExecuteReader();

            //            while (reader.Read())
            //            {
            //                Space space = new Space();

            //                space.VenueName = Convert.ToString(reader["name"]);
            //                spaces.Add(space);
            //            }
            //        }
            //    }
            //    catch (SqlException ex)
            //    {
            //        Console.WriteLine("Error retrieving venues.");
            //        Console.WriteLine(ex.Message);

            //        throw;
            //    }

            //    return spaces;
            //}
        private Space ConvertReaderToSpace(SqlDataReader reader)
        {
            Space space = new Space();

            if (reader["open_from"] != DBNull.Value)
            {
                space.OpenFrom = Convert.ToInt32(reader["open_from"]);
            }
            if (reader["open_to"] != DBNull.Value)
            {
                space.OpenTo = Convert.ToInt32(reader["open_to"]);
            }

            //space.SpaceId = Convert.ToInt32(reader["id"]);
            //space.VenueID = Convert.ToInt32(reader["venue_id"]);
            space.SpaceName = Convert.ToString(reader["name"]);
            //space.IsAccessible = Convert.ToBoolean(reader["is_accessible"]);
            space.DailyRate = Convert.ToDouble(reader["daily_rate"]);
            space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            space.VenueName = Convert.ToString(reader["venue_name"]);
           // return month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthIndex);
            return space;
        }

    }
}
