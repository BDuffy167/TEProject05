using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;

namespace Capstone.IntegrationTests
{
    public class ReservationSqlDAOTests: IntegrationTestBase
    {
        [TestMethod]
        public void InsertNewBookingTest()
        {
            DateTime dateStart = new DateTime(2021, 08, 17);
            DateTime dateEnd = new DateTime(2021, 08, 21);

            Reservation reservation = new Reservation()
            {

                StartDate = dateStart,
                EndDate = dateEnd,
                ReservedFor = "Test",
                SpaceId = 1
            };

            int id = reservation.ReservationId;
            Assert.AreNotEqual(0, id);

            Reservation getReservation = reservation;
            Assert.AreEqual(reservation.StartDate, getReservation.StartDate);
            Assert.AreEqual(reservation.EndDate, getReservation.EndDate);
            Assert.AreEqual(reservation.ReservedFor, getReservation.ReservedFor);
            Assert.AreEqual(reservation.SpaceId, getReservation.SpaceId);
        }
    }
}
