using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOrganizerTests.DAL
{
    [TestClass]
    public class DepartmentSqlDAOTests : CorporateDAOTestsBase
    {
        [TestMethod]
        [DataRow("Store Support", 4)]
        public void GetDepartmentByID_Should_ReturnRightDepartment(string expectedDepartment, int department_id)
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(this.ConnectionString);

            // Act
            IList<Department> departments = (IList<Department>)dao.GetDepartments();

            // Assert
            Assert.AreEqual(expectedDepartment, department_id);
        }
    }
}