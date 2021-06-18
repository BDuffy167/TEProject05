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
        private const string SqlListVenues = "SELECT name FROM venue ORDER BY name ASC";

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

        }
        private Venue ConvertReaderToVenue(SqlDataReader reader)
        {
            Venue venue = new Venue();
            venue.VenueId = Convert.ToInt32(reader["id"]);
            venue.Name = Convert.ToString(reader["name"]);
            venue.CityId = Convert.ToInt32(reader["city_id"]);
            venue.Description = Convert.ToString(reader["description"]);
            venue.CityName = Convert.ToString(reader["city_name"]);
            venue.VenueId = Convert.ToInt32(reader["id"]);
            venue.VenueId = Convert.ToInt32(reader["id"]);
        }
    }
}
