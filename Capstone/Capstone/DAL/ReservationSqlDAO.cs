using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationDAO
    {
        private readonly string connectionString;
        private const string SqlGetAvailableReservations = "SELECT * FROM reservation WHERE(start_date) between GETDATE() and(GETDATE() + 30) ORDER BY end_date";
        private const string SqlReservation = "SELECT * FROM reservation join space on space.id = reservation.space_id join venue on venue.id = space.venue_id" +
                                                                "WHERE (CAST('@arrival' AS date) BETWEEN start_date and end_date or" +
                                                                "CAST('@depart' AS date) BETWEEN start_date and end_date or " +
                                                                 "CAST('@arrival' AS date) < start_date and cast('@depart' AS date) > end_date) and venue.id = '@venue_id'";
        public ReservationSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Reservation> GetAvailableReservations()
        {
            IList<Reservation> reservations = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SqlGetAvailableReservations, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation();

                        reservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);

                        reservations.Add(reservation);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occured communicating with the database");
                Console.WriteLine(ex.Message);
            }
            return reservations;
        }

        private Reservation GetResConfirmation(int confirmationNum)
        {
            Reservation reservation = new Reservation();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("select * from reservation join space on space.id = reservation.space_id join venue on venue.id = space.venue_id" +
                                                                "where(CAST('@arrival' AS date) BETWEEN start_date and end_date or" +
                                                                "CAST('@depart' AS date) BETWEEN start_date and end_date or " +
                                                                 "CAST('@arrival' AS date) < start_date and cast('@depart' AS date) > end_date) and venue.id = '@venue_id'", conn);
                    cmd.Parameters.AddWithValue("@arrive", reservation.StartDate);
                    cmd.Parameters.AddWithValue("@depart", reservation.EndDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        reservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);
                        reservation.VenueName = Convert.ToString(reader["venue_name"]);
                        reservation.SpaceName = Convert.ToString(reader["space_name"]);
                        reservation.ReservedFor = Convert.ToString(reader["reserved_for"]);
                        reservation.NumberOfAttendees = Convert.ToInt32(reader["number_of_attendees"]);
                        reservation.StartDate = Convert.ToDateTime(reader["start_date"]);
                        reservation.EndDate = Convert.ToDateTime(reader["end_date"]);
                        reservation.DailyRate = Convert.ToDouble(reader["daily_rate"]);
                        reservation.CreatedBooking = Convert.ToDateTime(reader["create_date"]);

                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occured communicating with the database");
                Console.WriteLine(ex.Message);
            }
            return reservation;

        }


        private Reservation ConvertReaderToReservation(SqlDataReader reader, IList<Reservation> reservations)
        {
            Reservation reservation = new Reservation();

            reservation.ReservationId = Convert.ToInt32(reader["id"]);
            reservation.SpaceId = Convert.ToInt32(reader["space_id"]);
            reservation.NumberOfAttendees = Convert.ToInt32(reader["number_of_attendees"]);
            reservation.StartDate = Convert.ToDateTime(reader["start_date"]);
            reservation.EndDate = Convert.ToDateTime(reader["end_date"]);
            reservation.ReservedFor = Convert.ToString(reader["reserved_for"]);

            return reservation;
        }

        public bool AddNewReservation(Reservation newReservation)
        {
            try
            {
                // Create a connection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Open the connection
                    conn.Open();

                    // Create the command
                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation VALUES (@SpaceId, @NumberOfAttendees, @StartDate, @EndDate, @ReservedFor)", conn);
                    cmd.Parameters.AddWithValue("@SpaceId", newReservation.SpaceId);
                    cmd.Parameters.AddWithValue("@NumberOfAttendees", newReservation.NumberOfAttendees);
                    cmd.Parameters.AddWithValue("@StartDate", newReservation.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", newReservation.EndDate);
                    cmd.Parameters.AddWithValue("@ReservedFor", newReservation.ReservedFor);

                    // Execute the command
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred saving the new language.");
                Console.WriteLine(ex.Message);

                throw;
            }

            return true;

        }
    }
}