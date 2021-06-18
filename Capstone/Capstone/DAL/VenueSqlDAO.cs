using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueSqlDAO: IVenueDAO
    {
        private readonly string connectionString;
        private const string SqlListVenues =    "SELECT v.id, v.name, v.city_id, v.description, c.name AS 'city_name', s.abbreviation, s.name AS 'state_name' " +
                                                "FROM venue v INNER JOIN city c ON v.city_id = c.id INNER JOIN state s ON c.state_abbreviation = s.abbreviation " +
                                                "ORDER BY v.name";
        public VenueSqlDAO (string connectionString)
        {
            this.connectionString = connectionString;
        }
        public IEnumerable<Venue> ListVenues()
        {
            List<Venue> venues = new List<Venue>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SqlListVenues, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Venue venue = ConvertReaderToVenue(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occured communicating with the database");
                Console.WriteLine(ex.Message);
            }
            return venues;

        }
        private Venue ConvertReaderToVenue(SqlDataReader reader)
        {
            Venue venue = new Venue();

            venue.VenueId = Convert.ToInt32(reader["id"]);
            venue.VenueName = Convert.ToString(reader["name"]);
            venue.CityId = Convert.ToInt32(reader["city_id"]);
            venue.Description = Convert.ToString(reader["description"]);
            venue.CityName = Convert.ToString(reader["city_name"]);
            venue.StateAbbreviation = Convert.ToString(reader["abbreviation"]);
            venue.StateName = Convert.ToString(reader["state_name"]);

            return venue;
        }
    }
}
