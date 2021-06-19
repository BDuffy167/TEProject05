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
        // DELETE LATER
        private void GetUserVenueChoice(IList<Venue> venues)
        {
            int userInput = int.Parse(Console.ReadLine()) - 1;

            for (int i = 0; i < venues.Count; i++)
            {
                if (userInput == i)
                {
                    Console.WriteLine(venues[i].VenueName);
                    Console.WriteLine($"Location: {venues[i].CityName}, {venues[i].StateAbbreviation}");
                    Console.Write("Category: ");
                    string categoryString = "";
                    foreach (string s in venues[i].CategoryName)
                    {
                        categoryString += ($"{s}, ");
                    }
                    Console.WriteLine(categoryString.Substring(0, categoryString.Length - 2));
                    Console.WriteLine();
                    Console.WriteLine(venues[i].Description);
                    Console.WriteLine();
                    Console.WriteLine("What would you like to do next?");
                    Console.WriteLine("\t1) View Spaces");
                    Console.WriteLine("\t2) Search for Reservation");
                    Console.WriteLine("\tR) Return to previous screen");
                }
                else
                {
                    Console.WriteLine("Please enter a valid selection or (R)eturn");
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
                    if (userInput == i.Substring(0, 0))
                    {

                        //PrintDetailedVenueInfo(i.Substring(3));
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
        public void PrintDetailedVenueInfo(IList<Venue> venue)
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
            Console.WriteLine("\t2) Search for Reservation");
            Console.WriteLine("\tR) Return to previous screen");
        }
        // User selects to see venue spaces or tries to make a reservation
        public int InputDetailedVenueChoice()
        {
            return 1;
        }
        // Lists detailed info for all spaces in a venue
        public void PrintSpaceMenu()
        {

        }
        // User may search for reservations
        public int InputSpaceMenuChoice()
        {
            return 1;
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
