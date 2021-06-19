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

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;

            this.venueDAO = new VenueSqlDAO(connectionString);
            this.reservationDAO = new ReservationSqlDAO(connectionString);
            this.spaceDAO = new SpaceSqlDAO(connectionString);
        }

        public void Run()
        {
            ShowMainMenu();
        }

        // This displays the starting menu
        private void ShowMainMenu()
        {
            bool keepGoing = true;

            while (keepGoing)
            {
                string userInput = InputMainMenuChoice();

                switch (userInput.ToLower())
                {
                    case "1":
                        ShowVenueNames();
                        break;
                    case "q":
                        keepGoing = false;
                        break;
                    default:
                        Console.WriteLine("Command provided was not a valid please try again");
                        break;
                }
            }
        }

        // User selects and option from the main menu
        private string InputMainMenuChoice()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("\t1) List Venues");
            Console.WriteLine("\tQ) Quit");

            string userInput = Console.ReadLine();

            return userInput;
        }
        // Displays full list of venue names
        public void ShowVenueNames()
        {
            IList<Venue> venues = venueDAO.GetVenueNames();
            int listNum = 1;
            List<string> venueList = new List<string>();

            Console.WriteLine();
            Console.WriteLine("Which venue would you like to view?");

            foreach (Venue i in venues)
            {
                venueList.Add($"{listNum}) {i.VenueName}");
                Console.WriteLine($"\t{listNum}) {i.VenueName}");
                listNum++;
            }

            Console.WriteLine("\tR) Return to previous screen.");

            InputVenueMenuChoice(venueList);

        }
        // User selects which venue they would like to know more about
        private void InputVenueMenuChoice(List<string> venueList)
        {
            bool keepGoing = true;

            while (keepGoing)
            {
                string userInput = Console.ReadLine();

                foreach (string i in venueList)
                {
                    if (userInput.ToLower() == i.Substring(0, 1))
                    {
                        IList<Venue> venue = venueDAO.GetDetailedVenueInfo(i[3..]);
                        ShowDetailedVenueInfo(venue);
                    }
                }
                if (userInput.ToLower() == "r")
                {
                    keepGoing = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Please input a valid choice.");
                    break;
                }
            }
        }
        // Displays detailed venue info to the user
        public void ShowDetailedVenueInfo(IList<Venue> venue)
        {
            Console.WriteLine(venue[0].VenueName);
            Console.WriteLine($"Location: {venue[0].CityName}, {venue[0].StateAbbreviation}");
            Console.Write("Category: ");
            string categoryString = "";
            foreach (Venue v in venue)
            {
                categoryString += ($"{v.CategoryName}, ");
            }
            Console.WriteLine(categoryString.Substring(0, categoryString.Length - 2));
            Console.WriteLine();
            Console.WriteLine(venue[0].Description);
            Console.WriteLine();

            bool keepGoing = true;

            while (keepGoing)
            {
                string userInput = InputDetailedVenueChoice();

                switch (userInput.ToLower())
                {
                    case "1":
                        ShowSpaceMenu(venue[0].VenueName);
                        break;
                    case "2":
                        MakeReservation();
                        break;
                    case "r":
                        keepGoing = false;
                        break;
                    default:
                        Console.WriteLine("Command provided was not a valid please try again");
                        break;
                }
            }
        }
        // User selects to see venue spaces or tries to make a reservation
        public string InputDetailedVenueChoice()
        {
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("\t1) View Spaces");
            Console.WriteLine("\t2) Search for Reservation");
            Console.WriteLine("\tR) Return to previous screen");

            string userInput = Console.ReadLine();

            return userInput;
        }
        // Lists detailed info for all spaces in a venue
        public void ShowSpaceMenu(string name)
        {
            List<Space> spaces = spaceDAO.GetSpaceInfo(name);

            bool keepGoing = true;

            while (keepGoing)
            {
                string userInput = InputSpaceMenuChoice(spaces);
            }
        }
        // User may search for reservations
        public string InputSpaceMenuChoice(List<Space> spaces)
        {
            Console.WriteLine($"{spaces[0].VenueName} Spaces");
            Console.WriteLine();
            string header = string.Format($"{"",-6}{"Name",-15}{"Open",-8}{"Close",-8}{"Daily Rate",-14}{"Max. Occupancy",-14}");
            Console.WriteLine(header);

            int indexNum = 1;

            foreach (Space space in spaces)
            {
                string spaceItem = string.Format($"{0,-6}{1,-15}{2,-8}{3,-8}{4,-14}{5,-14}", "#" + indexNum, space.SpaceName, space.OpenFrom, space.OpenTo, space.DailyRate, space.MaxOccupancy);
                Console.WriteLine(spaceItem);
            }

            string userInput = Console.ReadLine();
            return userInput;

        }
        // Walks a user through searching for a reservation
        public void MakeReservation()
        {

        }
        // Displays details of a successful reservation
        public void PrintReservationConfirmation()
        {

        }
    }
}
