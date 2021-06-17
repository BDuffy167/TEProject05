using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class ProjectSqlDAO : IProjectDAO
    {
        private readonly string connectionString;

        // Single Parameter Constructor
        public ProjectSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        private const string SqlSelectAll = "SELECT project_id, name, from_date, to_date FROM project";
        private const string SqlInsertEmp = "INSERT INTO project_employee (project_id, employee_id) VALUES (@project_id, @employee_id)";
        private const string SqlDeleteEmp = "DELETE FROM project_employee WHERE project_id = @project_id AND employee_id = @employee_id";
        private const string SqCreateProj = "INSERT INTO project VALUES (@name, @from_date, @to_date)";

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns></returns>
        public ICollection<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAll, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        Project project = ConvertReaderToProject(reader);

                        
                        projects.Add(project);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem getting projects: " + ex.Message);
            }

            return projects;
        }


        /// <summary>
        /// Assigns an employee to a project using their IDs.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    // Create our insert command
                    SqlCommand command = new SqlCommand(SqlInsertEmp, conn);
                    command.Parameters.AddWithValue("@project_id", projectId);
                    command.Parameters.AddWithValue("@employee_id", employeeId);


                    // Run our insert command
                    command.ExecuteNonQuery();

                    // If we got here, it must have worked
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not insert employee: " + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    // Create our insert command
                    SqlCommand command = new SqlCommand(SqlDeleteEmp, conn);
                    command.Parameters.AddWithValue("@project_id", projectId);
                    command.Parameters.AddWithValue("@employee_id", employeeId);


                    // Run our insert command
                    command.ExecuteNonQuery();

                    // If we got here, it must have worked
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not delete employee: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="newProject">The new project object.</param>
        /// <returns>The new id of the project.</returns>
        public int CreateProject(Project newProject)
        { 

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    // Create our insert command
                    SqlCommand command = new SqlCommand(SqCreateProj, conn);
                    
                    command.Parameters.AddWithValue("@from_date", newProject.StartDate);
                    command.Parameters.AddWithValue("@to_date", newProject.EndDate);
                    command.Parameters.AddWithValue("@name", newProject.Name);


                    // Run our insert command
                    command.ExecuteNonQuery();

                    // If we got here, it must have worked
                    return newProject.ProjectId;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not insert new project: " + ex.Message);
                return 0;
            }
        }

        private Project ConvertReaderToProject(SqlDataReader reader)
        {
            Project project = new Project();

            project.ProjectId = Convert.ToInt32(reader["project_id"]);
            project.Name = Convert.ToString(reader["name"]);
            project.StartDate = Convert.ToDateTime(reader["from_date"]);
            project.EndDate = Convert.ToDateTime(reader["to_date"]);

            return project;
        }
    }
}
