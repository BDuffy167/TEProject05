using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class EmployeeSqlDAO : IEmployeeDAO
    {
        private readonly string connectionString;

        // Single Parameter Constructor
        public EmployeeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        private const string SqlSelectAll = "SELECT id, first_name, last_name, job_title, birth_date, hire_date, department_id FROM employee";
        private const string SqlSelectSearch = "SELECT first_name, last_name FROM employee WHERE first_name LIKE @first_name OR last_name LIKE @last_name";
        private const string SqlSelectNoProj = "SELECT id, first_name, last_name FROM employee WHERE project_id IS NULL";

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public ICollection<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAll, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        Employee employee = new Employee();

                        // Set the values on the new thing
                        employee.EmployeeId = Convert.ToInt32(reader["id"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        employee.HireDate = Convert.ToDateTime(reader["hire_date"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.DepartmentId = Convert.ToInt32(reader["department_id"]);


                        // Add it to our list of results
                        employees.Add(employee);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem getting employees: " + ex.Message);
            }

            return employees;
        }

        /// <summary>
        /// Find all employees whose names contain the search strings.
        /// Returned employees names must contain *both* first and last names.
        /// </summary>
        /// <remarks>Be sure to use LIKE for proper search matching.</remarks>
        /// <param name="firstname">The string to search for in the first_name field</param>
        /// <param name="lastname">The string to search for in the last_name field</param>
        /// <returns>A list of employees that matches the search.</returns>
        public ICollection<Employee> Search(string firstname, string lastname)
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectSearch, conn);
                    command.Parameters.AddWithValue("@first_name", firstname);
                    command.Parameters.AddWithValue("@last_name", lastname);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee();

                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);


                        employees.Add(employee);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem getting employee: " + ex.Message);
            }

            return employees;
        }

        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public ICollection<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectNoProj, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee();

                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);


                        employees.Add(employee);
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("Problem getting employees without projects: " + ex.Message);
            }

            return employees;
        }

    }
}
