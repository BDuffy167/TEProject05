using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

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

        private readonly IVenueDAO venueDAO;
        private readonly IReservationDAO reservationDAO;
        private readonly ISpaceDAO spaceDAO;
        private readonly ICategoryDAO categoryDAO;

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;

            this.venueDAO = new VenueSqlDAO(connectionString);
            this.reservationDAO = new ReservationSqlDAO(connectionString);
            this.spaceDAO = new SpaceSqlDAO(connectionString);
            this.categoryDAO = new CategorySqlDAO(connectionString);
        }

        const string Cmd_ListVenues = "1";

        const string Cmd_QuitProgram = "q";
        public void Run()
        {
            PrintMainMenu();
            while (true)
            {
                string command = Console.ReadLine();
                Console.Clear();

                switch (command.ToLower())
                {
                    case Cmd_ListVenues:
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

        private void ListVenues()
        {
            IList<Venue> venues = venueDAO.ListVenues();
            int listNum = 1;

            Console.WriteLine();
            Console.WriteLine("Which venue would you like to view?");

            foreach (Venue i in venues)
            {
                Console.WriteLine($"{listNum}) {i.VenueName}");
                listNum++;
            }

            Console.WriteLine("R) Return to previous screen.");

            GetUserVenueChoice(venues);
        }

        private void GetUserVenueChoice(IList<Venue> venues)
        {
            int userInput = int.Parse(Console.ReadLine()) - 1;

            for (int i = 0; i < venues.Count; i++)
            {
                if (userInput == i)
                {
                    Console.WriteLine(venues[i].VenueName);
                    Console.WriteLine($"Location: {venues[i].CityName}, {venues[i].StateAbbreviation}");
                    Console.WriteLine(categoryDAO.GetVenueCategories(venues[i]));
                    Console.WriteLine();
                    Console.WriteLine(venues[i].Description);
                    Console.WriteLine();
                    Console.WriteLine("What would you like to do next?");
                }
            }
        }
    }
}
