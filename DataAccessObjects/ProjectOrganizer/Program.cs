using Microsoft.Extensions.Configuration;
using ProjectOrganizer.DAL;
using System;
using System.IO;

namespace ProjectOrganizer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("Project");

            IProjectDAO projectDAO = new ProjectSqlDAO(connectionString);
            IEmployeeDAO employeeDAO = new EmployeeSqlDAO(connectionString);
            IDepartmentDAO departmentDAO = new DepartmentSqlDAO(connectionString);

            ProjectCLI projectCLI = new ProjectCLI(employeeDAO, projectDAO, departmentDAO);

            string header = string.Format($"{"",-6}{"Name",-20}{"Open",-8}{"Close",-8}{"Daily Rate",-14}{"Max. Occupancy",-14}");
            Console.WriteLine(header);

            //projectCLI.RunCLI();
        }
    }
}
