using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SpaceSqlDAO : ISpaceDAO
    {
        private readonly string connectionString;
        private const string SqlGetOpenSpaces = "SELECT s.name, s.open_from, s.open_to, s.daily_rate, s.max_occupancy, s.id, s.venue_id, s.is_accessible FROM space s WHERE venue_id = @VenueId AND s.max_occupancy >= @ResAttendance AND s.id NOT IN (SELECT s.id from reservation r JOIN space s on r.space_id = s.id WHERE s.venue_id =  6 AND r.end_date >= @ResStartDate AND r.start_date <= @ResEndDate)";

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

                    SqlCommand cmd = new SqlCommand("SELECT v.name AS 'venue_name', s.name, s.open_from, s.open_to, s.daily_rate, s.max_occupancy, s.id, s.venue_id, s.is_accessible FROM space s INNER JOIN venue v ON s.venue_id = v.id WHERE v.name = @venueName", conn);
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

        public List<Space> GetOpenSpaces(int venueId, DateTime resStartDate, DateTime resEndDate, int resAttendance)
        {
            List<Space> openSpaces = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SqlGetOpenSpaces, conn);
                    cmd.Parameters.AddWithValue("@VenueId", venueId);
                    cmd.Parameters.AddWithValue("@ResStartDate", resStartDate);
                    cmd.Parameters.AddWithValue("@ResEndDate", resEndDate);
                    cmd.Parameters.AddWithValue("@ResAttendance", resAttendance);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = new Space();

                        space = ConvertReaderToSpace(reader);

                        openSpaces.Add(space);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occured communicating with the database");
                Console.WriteLine(ex.Message);
            }
            return openSpaces;
        }

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

            space.SpaceId = Convert.ToInt32(reader["id"]);
            space.VenueId = Convert.ToInt32(reader["venue_id"]);
            space.SpaceName = Convert.ToString(reader["name"]);
            space.IsAccessible = Convert.ToBoolean(reader["is_accessible"]);
            space.DailyRate = Convert.ToDouble(reader["daily_rate"]);
            space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            return space;
        }

    }
}
