using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.IntegrationTests
{
   public class VenueSqlDAOTests : IntegrationTestBase
    {

        [TestMethod]
        public void GetVenuesTest_Should_ReturnAllVenues()
        {
            // Arrange
            VenueSqlDAO dao = new VenueSqlDAO(this.ConnectionString);
            int expectedResults = this.GetRowCount("venue");

            // Act
            IList<Venue> results = dao.ListVenues();

            // Assert
            Assert.IsTrue(results.Count > 0);
            Assert.AreEqual(expectedResults, results.Count);
        }
    }
}
