using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.IntegrationTests
{
   public class SpaceSqlDAOTests: IntegrationTestBase
    {
        public void GetSpaceInfoTest_Should_ReturnAllSpaces()
        {
            // Arrange
            SpaceSqlDAO dao = new SpaceSqlDAO(this.ConnectionString);
            int expectedResults = this.GetRowCount("space");

            // Act
            IList<Space> results = dao.GetSpaceInfo("name");

            // Assert
            Assert.IsTrue(results.Count > 0);
            Assert.AreEqual(expectedResults, results.Count);
        }
    }
}
