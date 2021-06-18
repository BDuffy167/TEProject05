using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    /// <summary>
    /// This class is responsible for representing the main user interface to the user.
    /// </summary>
    /// <remarks>
    /// ALL Console.ReadLine and WriteLine in this class
    /// NONE in any other class. 
    ///  
    /// The only exceptions to this are:
    /// 1. Error handling in catch blocks
    /// 2. Input helper methods in the CLIHelper.cs file
    /// 3. Things your instructor explicitly says are fine
    /// 
    /// No database calls should exist in classes outside of DAO objects
    /// </remarks>
    public class UserInterface
    {
        private readonly string connectionString;


        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            this.venueDAO = new VenueSqlDAO(connectionString);
            this.reservationDAO = new ReservationSqlDAO(connectionString);
            this.spaceDAO = new SpaceSqlDAO(connectionString);
            this.categoryDAO = new CategorySqlDAO(connectionString);
        }

        const string Command_ListVenues = "1";
        public void Run()
        {
            PrintMainMenu();
            while (true)
            {
                string command = Console.ReadLine();
                Console.Clear();

                switch (command.ToLower())
                {
                    case Command_ListVenues:
                        ListVenues();
                        break;

                    default:
                        Console.WriteLine("Command provided was not a valid command, please try again");
                        break;
                }
            }
        }
        private void PrintMainMenu()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine();
            Console.WriteLine("1) List Venues");

            Console.WriteLine("Q) Quit"); //Fill list as we complete items//

        }
    }
}
