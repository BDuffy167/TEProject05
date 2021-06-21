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

        private const string SqlPrintNewReservation = "SELECT r.reservation_id, v.name, s.name, r.reserved_for, r.number_of_attendees, r.start_date, r.end_date, s.daily_rate" +
                                                         "FROM reservation r INNER JOIN space s ON r.space_id = s.id INNER JOIN venue v ON s.id = v.id" +
                                                         "WHERE r.reservation_id = @confirmation";


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

                    SqlCommand cmd = new SqlCommand(SqlReservation, conn);
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

        public Reservation GetResConfirmation(int confirmationNum)
        {
            Reservation reservation = new Reservation();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SqlPrintNewReservation, conn);
                    cmd.Parameters.AddWithValue("@confirmation", confirmationNum);
                    

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

        public void AddNewReservation(Reservation newReservation)
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

         

        }
    }
}