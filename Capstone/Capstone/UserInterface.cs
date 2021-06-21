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
            while (true)
            {
                string userInput = InputMainMenuChoice();

                switch (userInput.ToLower())
                {
                    case "1":
                        ShowVenueMenuNames();
                        continue;
                    case "q":

                        return;
                    default:
                        Console.WriteLine("Command provided was not a valid please try again");
                        Console.WriteLine();
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
        public void ShowVenueMenuNames()
        {
            IList<Venue> venues = venueDAO.GetVenueNames();
            int listNum = 1;
            List<string> venueList = new List<string>();
            List<string> indexList = new List<string>();

            foreach (Venue i in venues)
            {
                venueList.Add($"{listNum}) {i.VenueName}");
                indexList.Add($"{listNum}");
                listNum++;
            }

            while (true)
            {
                string userInput = InputVenueMenuChoice();

                if (indexList.Contains(userInput))
                {
                    IList<Venue> venue = venueDAO.GetDetailedVenueInfo(venueList[int.Parse(userInput) - 1].Substring(userInput.Length + 2));
                    ShowDetailedVenueInfo(venue);
                    continue;
                }

                if (userInput.ToLower() == "r")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Please input a valid choice.");
                    continue;
                }
            }

        }

        // User selects which venue they would like to know more about
        private string InputVenueMenuChoice()
        {
            IList<Venue> venues = venueDAO.GetVenueNames();
            int listNum = 1;


            Console.WriteLine();
            Console.WriteLine("Which venue would you like to view?");

            foreach (Venue i in venues)
            {
                Console.WriteLine($"\t{listNum}) {i.VenueName}");
                listNum++;
            }

            Console.WriteLine("\tR) Return to previous screen.");

            string userInput = Console.ReadLine();
            return userInput;
        }
        // Displays detailed venue info to the user
        public void ShowDetailedVenueInfo(IList<Venue> venue) //I only add this arg to pass to the next method. Probably bad idea?
        {
            while (true)
            {
                string userInput = InputDetailedVenueChoice(venue);

                switch (userInput.ToLower())
                {
                    case "1":
                        ShowSpaceMenu(venue[0].VenueName);
                        continue;
                    //case "2":
                    //    MakeReservation();
                    //    continue;
                    case "r":
                        return;
                    default:
                        Console.WriteLine("Command provided was not a valid please try again");
                        Console.WriteLine();
                        continue;
                }
            }
        }
        // User selects to see venue spaces or tries to make a reservation
        public string InputDetailedVenueChoice(IList<Venue> venue)
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

            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("\t1) View Spaces");
            // Console.WriteLine("\t2) Search for Reservation");  BONUS
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

                switch (userInput.ToLower())
                {
                    case "1":
                        MakeReservation(spaces[0].VenueName);
                        continue;
                    case "r":
                        return;
                    default:
                        Console.WriteLine("Command provided was not a valid please try again");
                        Console.WriteLine();
                        continue;
                }
            }
        }
        // User may search for reservations
        public string InputSpaceMenuChoice(List<Space> spaces)
        {
            Console.WriteLine($"{spaces[0].VenueName} Spaces");
            Console.WriteLine();
            string header = string.Format($"{"",-4}{"Name",-25}{"Open",-8}{"Close",-8}{"Daily Rate",-14}{"Max. Occupancy",-14}");
            Console.WriteLine(header);

            int indexNum = 1;

            foreach (Space space in spaces)
            {
                string spaceItem = string.Format($"#{indexNum,-3}{space.SpaceName,-25}{space.OpenFrom,-8}{space.OpenTo,-8}${space.DailyRate,-13}{space.MaxOccupancy,-14}");
                Console.WriteLine(spaceItem);
                indexNum++;
            }
            Console.WriteLine();
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("\t1) Search for Reservation");
            Console.WriteLine("\tR) Return to previous screen");

            string userInput = Console.ReadLine();
            return userInput;

        }
        // Walks a user through searching for a reservation
        public void MakeReservation(string venueName)
        {
            Console.Write("What is the start date of your reservation (MM/DD/YYYY)? ");
            string resStartDate = Console.ReadLine();
            Console.Write("How many days will you need the space? ");
            int resLength = int.Parse(Console.ReadLine());
            Console.Write("How many people will be in attendance? ");
            string resAttendance = Console.ReadLine();
            Console.WriteLine();

            List<Space> openSpaces = new List<Space>();
            // Sql method query for THIS venue

            Console.WriteLine("The following spaces are available based on your needs:");
            Console.WriteLine();

            string header = string.Format($"{"Space #",-10}{"Name",-25}{"Daily Rate",-14}{"Max. Occupancy",-14}{"Accessible?",-14}{"Total Cost",-14}");
            Console.WriteLine(header);

            foreach (Space s in openSpaces)
            {
                string resItem = string.Format($"{s.VenueID + s.SpaceId,-10}{s.SpaceName,-25}${s.DailyRate,-13}{s.MaxOccupancy,-14}{s.IsAccessible,-14}${s.DailyRate * resLength,-13}");
                Console.WriteLine(resItem);
            }

            Console.Write("Which space would you like to reserve (enter 0 to cancel)? ");
            string spaceVenueId = Console.ReadLine();
            Console.Write("Who is this reservation for? ");
            string resHolder = Console.ReadLine();

           
        }
      





        // Displays details of a successful reservation
        public void PrintReservationConfirmation(int confirmationNum)
        {
            List<Reservation> reservations = new List<Reservation>();

            
            //Sql method to call the reservation.

            Console.WriteLine($"Confirmation #: ");
            Console.WriteLine($"Venue: ");
            Console.WriteLine($"Space: ");
            Console.WriteLine($"Reserved For: ");
            Console.WriteLine($"Attendees: ");
            Console.WriteLine($"Arrival Date: ");
            Console.WriteLine($"Depart Date: ");
            Console.WriteLine($"Total Cost: ");
        }
    }
}
