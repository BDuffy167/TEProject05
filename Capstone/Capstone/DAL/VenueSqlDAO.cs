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
    public class VenueSqlDAO : IVenueDAO
    {
        private readonly string connectionString;
        private const string SqlVenueNames = "SELECT name FROM venue ORDER BY name";
                                                
        private const string SqlDetailedVenue = "SELECT v.id, v.name, v.city_id, v.description, c.name AS 'city_name', s.abbreviation, s.name AS 'state_name', cat.id AS 'category_id', cat.name AS 'category_name'" +
                                                "FROM venue v INNER JOIN city c ON v.city_id = c.id  INNER JOIN state s ON c.state_abbreviation = s.abbreviation INNER JOIN category_venue catv ON c.id = catv.venue_id INNER JOIN category cat ON catv.category_id = cat.id " +
                                                "WHERE v.name = @name ORDER BY v.name, cat.name;";

        public VenueSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public IList<Venue> GetVenueNames()
        {
            IList<Venue> venues = new List<Venue>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SqlVenueNames, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Venue venue = new Venue();

                        venue.VenueName = Convert.ToString(reader["name"]);

                        venues.Add(venue);
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

        public IList<Venue> GetDetailedVenueInfo(string s)
        {
            IList<Venue> venues = new List<Venue>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SqlDetailedVenue, conn);
                    cmd.Parameters.AddWithValue("@name", s);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Venue venue = ConvertReaderToVenue(reader);

                        venues.Add(venue);
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


            venue.CategoryId = Convert.ToInt32(reader["category_id"]);
            venue.CategoryName = Convert.ToString(reader["category_name"]);

            return venue;
        }






    }
}
