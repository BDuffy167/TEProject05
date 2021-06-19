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
    }
}