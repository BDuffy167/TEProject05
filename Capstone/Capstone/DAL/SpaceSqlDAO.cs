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
        private const string SqlGetSpaceInfo = "SELECT id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy FROM space";
        private const string SqlInputSpaceMenuChoice = "SELECT id, name, open_from, open_to, daily_rate, max_occupancy FROM space";
        private const string SqlGetOpenSpaces = "SELECT DISTINCT v.id, s.id, s.name, s.daily_rate, s.max_occupancy" +
                                                "FROM reservation r INNER JOIN space s ON s.id = r.space_id  INNER JOIN venue v ON v.id = s.venue_id" +
                                                "WHERE v.name = @VenueName" +
                                                    "AND (r.start_date NOT BETWEEN '@ResStartDate' AND '@ResEndDate' OR r.start_date NOT BETWEEN '@ResStartDat'e AND '@ResEndDate')" +
                                                    "AND @ResAttendance <= s.max_occupancy;";

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

        public List<Space> GetOpenSpaces(string venueName, DateTime resStartDate1, DateTime resEndDate1, int resAttendance)
        {
            List<Space> openSpaces = new List<Space>();

            string resStartDate = resStartDate1.ToString("yyyy-MM-dd");
            string resEndDate = resEndDate1.ToString("yyyy-MM-dd");

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SqlGetOpenSpaces, conn);
                    cmd.Parameters.AddWithValue("@VenueName", venueName);
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
