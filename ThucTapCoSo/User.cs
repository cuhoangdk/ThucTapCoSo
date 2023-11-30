using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    internal class User
    {
        static protected string[,] adminUserNameAndPassword = new string[10, 2];
        private static List<Customer> customersCollection = new List<Customer>();

        static void Main()
        {
            int countNumOfUsers = 1;
            RolesAndPermissions r1 = new RolesAndPermissions();
            Flight f1 = new Flight();
            FlightReservation bookingAndReserving = new FlightReservation();
            Customer c1 = new Customer();
            f1.FlightScheduler();
            Console.WriteLine();
            WelcomeScreen(1);
            Console.WriteLine("\n\t\t\t\t\t+++++++++++++ Welcome to BAV AirLines +++++++++++++\n\nTo Further Proceed, Please enter a value.");
            Console.WriteLine("\n***** Default Username && Password is root-root ***** Using Default Credentials will restrict you to just view the list of Passengers....\n");
            DisplayMainMenu();
            int desiredOption;
            //fix code
			while (!int.TryParse(Console.ReadLine(), out desiredOption))
			{
				Console.Write("Invalid input. Please enter a valid number: ");
			}			         
            while (desiredOption < 0 || desiredOption > 8)
            {
				while (!int.TryParse(Console.ReadLine(), out desiredOption))
				{
					Console.Write("Invalid input. Please enter a valid number: ");
				}
				Console.Write("ERROR!! Please enter value between 0 - 4. Enter the value again :\t");
                desiredOption = Convert.ToInt32(Console.ReadLine());
            }

            do
            {
                /* If desiredOption is 1 then call the login method.... if default credentials are used then set the permission
                 * level to standard/default where the user can just view the customer's data...if not found, then return -1, and if
                 * data is found then show the user display menu for adding, updating, deleting and searching users/customers...
                 * */
                if (desiredOption == 1)
                {
                    /*Default username and password....*/
                    adminUserNameAndPassword[0,0] = "root";
                    adminUserNameAndPassword[0,1] = "root";
                    PrintArtWork(1);
                    Console.Write("\nEnter the UserName to login to the Management System :     ");
                    string username = Console.ReadLine();
                    Console.Write("Enter the Password to login to the Management System :    ");
                    string password = Console.ReadLine();
                    Console.WriteLine();

                    /*Checking the RolesAndPermissions......*/
                    if (r1.IsPrivilegedUserOrNot(username, password) == -1)
                    {
                        Console.WriteLine($"\n{"", 20}ERROR!!! Unable to login Cannot find user with the entered credentials.... Try Creating New Credentials or get yourself register by pressing 4....", "");
                    }
                    else if (r1.IsPrivilegedUserOrNot(username, password) == 0)
                    {
                        Console.WriteLine("You've standard/default privileges to access the data... You can just view customers data... Can't perform any actions on them....");
                        c1.DisplayCustomersData(true);
                    }
                    else
                    {						
                        // In ra màn hình để kiểm tra						
						Console.WriteLine($"{"",-20}Logged in Successfully as \"{username}\"..... For further Proceedings, enter a value from below....", "");

                        /*Going to Display the CRUD operations to be performed by the privileged user.....Which includes Creating, Updating
                         * Reading(Searching) and deleting a customer....
                         * */
                        do
                        {
                            Console.WriteLine($"\n\n+++++++++ 2nd Layer Menu +++++++++ Logged in as \"{username}\"\n", "", "");

                            Console.WriteLine("(a) Enter 1 to add new Passenger....");
                            Console.WriteLine("(b) Enter 2 to search a Passenger....");
                            Console.WriteLine("(c) Enter 3 to update the Data of the Passenger....");
                            Console.WriteLine("(d) Enter 4 to delete a Passenger....");
                            Console.WriteLine("(e) Enter 5 to Display all Passengers....");
                            Console.WriteLine("(f) Enter 6 to Display all flights registered by a Passenger...");
                            Console.WriteLine("(g) Enter 7 to Display all registered Passengers in a Flight....");
                            Console.WriteLine("(h) Enter 8 to Delete a Flight....");
                            Console.WriteLine("(i) Enter 0 to Go back to the Main Menu/Logout....");

                            Console.Write("Enter the desired Choice :   ");
                            //fix code
							while (!int.TryParse(Console.ReadLine(), out desiredOption))
							{
								Console.Write("Invalid input. Please enter a valid number: ");
							}

							/*If 1 is entered by the privileged user, then add a new customer......*/
							if (desiredOption == 1)
                            {
                                c1.DisplayArtWork(1);
                                c1.AddNewCustomer();
                            }
                            else if (desiredOption == 2)
                            {
                                /*If 2 is entered by the privileged user, then call the search method of the Customer class*/
                                c1.DisplayArtWork(2);
                                c1.DisplayCustomersData(false);
                                Console.Write("Enter the CustomerID to Search :\t");
                                string customerID = Console.ReadLine();
                                Console.WriteLine();
                                c1.SearchUser(customerID);
                            }
                            else if (desiredOption == 3)
                            {
                                /*If 3 is entered by the user, then call the update method of the Customer Class with required
                                 * arguments.....
                                 * */
                                bookingAndReserving.DisplayArtWork(2);
                                c1.DisplayCustomersData(false);
                                Console.Write("Enter the CustomerID to Update its Data :\t");
                                string customerID = Console.ReadLine();
                                if (customersCollection.Count > 0)
                                {
                                    c1.EditUserInfo(customerID);
                                }
                                else
                                {
                                    Console.WriteLine($"No Customer with the ID {customerID} Found...!!!", " ");
                                }
                            }
                            else if (desiredOption == 4)
                            {
                                /*If 4 is entered, then ask the user to enter the customer id, and then delete
                                 * that customer....
                                 * */
                                bookingAndReserving.DisplayArtWork(3);
                                c1.DisplayCustomersData(false);
                                Console.Write("Enter the CustomerID to Delete its Data :\t");
                                string customerID = Console.ReadLine();
                                if (customersCollection.Count > 0)
                                {
                                    c1.DeleteUser(customerID);
                                }
                                else
                                {
                                    Console.WriteLine($"{"",-50}No Customer with the ID {customerID} Found...!!!", " ");
                                }
                            }
                            else if (desiredOption == 5)
                            {
                                /*Call the Display Method of Customer Class....*/
                                c1.DisplayArtWork(3);
                                c1.DisplayCustomersData(false);
                            }
                            else if (desiredOption == 6)
                            {
                                bookingAndReserving.DisplayArtWork(6);
                                c1.DisplayCustomersData(false);
                                Console.Write("\n\nEnter the ID of the user to display all flights registered by that user...");
                                string id = Console.ReadLine();
                                bookingAndReserving.DisplayFlightsRegisteredByOneUser(id);
                            }
                            else if (desiredOption == 7)
                            {
                                c1.DisplayArtWork(4);
                                Console.Write("Do you want to display Passengers of all flights or a specific flight.... 'Y/y' for displaying all flights and 'N/n' to look for a" +
                                        " specific flight.... ");
                                char choice = Console.ReadLine()[0];
                                if ('y' == choice || 'Y' == choice)
                                {
                                    bookingAndReserving.DisplayRegisteredUsersForAllFlight();
                                }
                                else if ('n' == choice || 'N' == choice)
                                {
                                    f1.DisplayFlightSchedule();
                                    Console.Write("Enter the Flight Number to display the list of passengers registered in that flight... ");
                                    string flightNum = Console.ReadLine();
                                    bookingAndReserving.DisplayRegisteredUsersForASpecificFlight(flightNum);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Choice...No Response...!");
                                }
                            }
                            else if (desiredOption == 8)
                            {
                                c1.DisplayArtWork(5);
                                f1.DisplayFlightSchedule();
                                Console.Write("Enter the Flight Number to delete the flight : ");
                                string flightNum = Console.ReadLine();
                                f1.DeleteFlight(flightNum);

                            }
                            else if (desiredOption == 0)
                            {
                                bookingAndReserving.DisplayArtWork(22);
                                Console.WriteLine("Thanks for Using BAV Airlines Ticketing System...!!!");
                            }
                            else
                            {
                                Console.WriteLine("Invalid Choice...Looks like you're Robot...Entering values randomly...You've Have to login again...");
                                bookingAndReserving.DisplayArtWork(22);
                                desiredOption = 0;
                            }

                        } while (desiredOption != 0);

                    }
                }
                else if (desiredOption == 2)
                {
                    PrintArtWork(2);
                    /*If desiredOption is 2, then call the registration method to register a user......*/
                    Console.Write("\nEnter the UserName to Register :    ");
                    string username = Console.ReadLine();
                    Console.Write("Enter the Password to Register :     ");
                    string password = Console.ReadLine();
                    while (r1.IsPrivilegedUserOrNot(username, password) != -1)
                    {
                        Console.Write("ERROR!!! Admin with same UserName already exist. Enter new UserName:   ");
                        username = Console.ReadLine();
                        Console.Write("Enter the Password Again:   ");
                        password = Console.ReadLine();
                    }

                    /*Setting the credentials entered by the user.....*/
                    adminUserNameAndPassword[countNumOfUsers, 0] = username;
                    adminUserNameAndPassword[countNumOfUsers, 1] = password;

                    /*Incrementing the numOfUsers */
                    countNumOfUsers++;
                }
                else if (desiredOption == 3)
                {
                    PrintArtWork(3);
                    Console.Write("\n\nEnter the Email to Login : \t");
                    string userName = Console.ReadLine();
                    Console.Write("Enter the Password : \t");
                    string password = Console.ReadLine();
                    string[] result = r1.IsPassengerRegistered(userName, password).Split('-');

                    if (Convert.ToInt32(result[0]) == 1)
                    {
                        int desiredChoice;
                        Console.WriteLine($"\n\n{"",-20}Logged in Successfully as \"{userName}\"..... For further Proceedings, enter a value from below....");
                        do
                        {
                            Console.WriteLine($"\n\n{"",-60}+++++++++ 3rd Layer Menu +++++++++{"",50}Logged in as \"{userName}\"\n");
                            Console.WriteLine("(a) Enter 1 to Book a flight....");
                            Console.WriteLine("(b) Enter 2 to update your Data....");
                            Console.WriteLine("(c) Enter 3 to delete your account....");
                            Console.WriteLine("(d) Enter 4 to Display Flight Schedule....");
                            Console.WriteLine("(e) Enter 5 to Cancel a Flight....");
                            Console.WriteLine($"(f) Enter 6 to Display all flights registered by \"{userName}\"....");
                            Console.WriteLine("(g) Enter 0 to Go back to the Main Menu/Logout....");
                            Console.Write("Enter the desired Choice :   ");
                            desiredChoice = Convert.ToInt32(Console.ReadLine());
                            if (desiredChoice == 1)
                            {
                                bookingAndReserving.DisplayArtWork(1);
                                f1.DisplayFlightSchedule();
                                Console.Write("\nEnter the desired flight number to book :\t ");
                                string flightToBeBooked = Console.ReadLine();
                                Console.Write($"Enter the Number of tickets for {flightToBeBooked} flight :   ");
                                int numOfTickets;
								while (!int.TryParse(Console.ReadLine(), out numOfTickets))
								{
									Console.Write("Invalid input. Please enter a valid number: ");
								}
								while (numOfTickets > 10)
                                {
									while (!int.TryParse(Console.ReadLine(), out numOfTickets))
									{
										Console.Write("Invalid input. Please enter a valid number: ");
									}
									Console.Write("ERROR!! You can't book more than 10 tickets at a time for single flight....Enter number of tickets again : ");
                                    numOfTickets = int.Parse(Console.ReadLine());
                                }
                                bookingAndReserving.BookFlight(flightToBeBooked, numOfTickets, result[1]);
                            }
                            else if (desiredChoice == 2)
                            {
                                bookingAndReserving.DisplayArtWork(2);
                                c1.EditUserInfo(result[1]);
                            }
                            else if (desiredChoice == 3)
                            {
                                bookingAndReserving.DisplayArtWork(3);
                                Console.Write("Are you sure to delete your account...It's an irreversible action...Enter Y/y to confirm...");
                                char confirmationChar = Console.ReadLine()[0];
                                if (confirmationChar == 'Y' || confirmationChar == 'y')
                                {
                                    c1.DeleteUser(result[1]);
                                    Console.WriteLine($"User {userName}'s account deleted Successfully...!!!");
                                    desiredChoice = 0;
                                }
                                else
                                {
                                    Console.WriteLine("Action has been cancelled...");
                                }
                            }
                            else if (desiredChoice == 4)
                            {
                                bookingAndReserving.DisplayArtWork(4);
                                f1.DisplayFlightSchedule();
                                f1.DisplayMeasurementInstructions();
                            }
                            else if (desiredChoice == 5)
                            {
                                bookingAndReserving.DisplayArtWork(5);
                                bookingAndReserving.CancelFlight(result[1]);
                            }
                            else if (desiredChoice == 6)
                            {
                                bookingAndReserving.DisplayArtWork(6);
                                bookingAndReserving.DisplayFlightsRegisteredByOneUser(result[1]);
                            }
                            else
                            {
                                bookingAndReserving.DisplayArtWork(7);
                                if (desiredChoice != 0)
                                {
                                    Console.WriteLine("Invalid Choice...Looks like you're Robot...Entering values randomly...You've Have to login again...");
                                }
                                desiredChoice = 0;
                            }
                        } while (desiredChoice != 0);

                    }
                    else
                    {
                        Console.WriteLine($"\n{" ",20}ERROR!!! Unable to login Cannot find user with the entered credentials.... Try Creating New Credentials or get yourself register by pressing 4....", "");
                    }
                }
                else if (desiredOption == 4)
                {
                    PrintArtWork(4);
                    c1.AddNewCustomer();
                }
                else if (desiredOption == 5)
                {
                    ManualInstructions();
                }

                DisplayMainMenu();
				while (!int.TryParse(Console.ReadLine(), out desiredOption))
				{
					Console.Write("Invalid input. Please enter a valid number: ");
				}
				while (desiredOption < 0 || desiredOption > 8)
                {
					while (!int.TryParse(Console.ReadLine(), out desiredOption))
					{
						Console.Write("Invalid input. Please enter a valid number: ");
					}
					Console.Write("ERROR!! Please enter value between 0 - 4. Enter the value again :\t");
                    desiredOption = Convert.ToInt32(Console.ReadLine());
                }
            } while (desiredOption != 0);

            WelcomeScreen(-1);
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("\n\n\t\t(a) Press 0 to Exit.");
            Console.WriteLine("\t\t(b) Press 1 to Login as admin.");
            Console.WriteLine("\t\t(c) Press 2 to Register as admin.");
            Console.WriteLine("\t\t(d) Press 3 to Login as Passenger.");
            Console.WriteLine("\t\t(e) Press 4 to Register as Passenger.");
            Console.WriteLine("\t\t(f) Press 5 to Display the User Manual.");
            Console.Write("\t\tEnter the desired option:    ");
        }

        static void ManualInstructions()
        {
            Console.WriteLine($"\n\n{new string(' ', 50)} +++++++++++++++++ Welcome to BAV Airlines User Manual +++++++++++++++++");
            Console.WriteLine("\n\n\t\t(a) Press 1 to display Admin Manual.");
            Console.WriteLine("\t\t(b) Press 2 to display User Manual.");
            Console.Write("\nEnter the desired option :    ");
            //fix code
            int choice;
			while (!int.TryParse(Console.ReadLine(), out choice))
			{
				Console.Write("Invalid input. Please enter a valid number: ");
			}
			while (choice < 1 || choice > 2)
            {
				while (!int.TryParse(Console.ReadLine(), out choice))
				{
					Console.Write("Invalid input. Please enter a valid number: ");
				}
				Console.Write("ERROR!!! Invalid entry...Please enter a value either 1 or 2....Enter again....");
                choice = Convert.ToInt32(Console.ReadLine());
            }
			if (choice == 1)
			{
				Console.WriteLine("\n\n(1) Admin have the access to all users data...Admin can delete, update, add and can perform search for any customer...\n");
				Console.WriteLine("(2) In order to access the admin module, you've to get yourself register by pressing 2, when the main menu gets displayed...\n");
				Console.WriteLine("(3) Provide the required details i.e., name, email, id...Once you've registered yourself, press 1 to login as an admin... \n");
				Console.WriteLine("(4) Once you've logged in, 2nd layer menu will be displayed on the screen...From here on, you can select from a variety of options...\n");
				Console.WriteLine("(5) Pressing \"1\" will add a new Passenger, provide the program with required details to add the passenger...\n");
				Console.WriteLine("(6) Pressing \"2\" will search for any passenger, given the admin(you) provides the ID from the table printing above....  \n");
				Console.WriteLine("(7) Pressing \"3\" will let you update any passenger's data given the user ID provided to the program...\n");
				Console.WriteLine("(8) Pressing \"4\" will let you delete any passenger given its ID provided...\n");
				Console.WriteLine("(9) Pressing \"5\" will let you display all registered passengers...\n");
				Console.WriteLine("(10) Pressing \"6\" will let you display all registered passengers...After selecting, the program will ask if you want to display passengers for all flights(Y/y) or a specific flight(N/n)\n");
				Console.WriteLine("(11) Pressing \"7\" will let you delete any flight given its flight number provided...\n");
				Console.WriteLine("(11) Pressing \"0\" will make you log out of the program...You can log in again anytime you want during the program execution....\n");
			}
			else
			{
				Console.WriteLine("\n\n(1) Local user has the access to its data only...He/She won't be able to change/update other users' data...\n");
				Console.WriteLine("(2) In order to access local users benefits, you've to get yourself registered by pressing 4 when the main menu gets displayed...\n");
				Console.WriteLine("(3) Provide the details asked by the program to add you to the users list...Once you've registered yourself, press \"3\" to log in as a passenger...\n");
				Console.WriteLine("(4) Once you've logged in, the 3rd layer menu will be displayed...From here on, you embarked on the journey to fly with us...\n");
				Console.WriteLine("(5) Pressing \"1\" will display available/scheduled list of flights...To get yourself booked for a flight, enter the flight number and number of tickets for the flight...Max num of tickets at a time is 10 ...\n");
				Console.WriteLine("(7) Pressing \"2\" will let you update your own data...You won't be able to update others' data... \n");
				Console.WriteLine("(8) Pressing \"3\" will delete your account... \n");
				Console.WriteLine("(9) Pressing \"4\" will display a randomly designed flight schedule for this runtime...\n");
				Console.WriteLine("(10) Pressing \"5\" will let you cancel any flight registered by you...\n");
				Console.WriteLine("(11) Pressing \"6\" will display all flights registered by you...\n");
				Console.WriteLine("(12) Pressing \"0\" will make you log out of the program...You can log in back at any time with your credentials...for this particular runtime... \n");
			}

		}
		static void WelcomeScreen(int option)
    {
        string artWork;

        if (option == 1)
        {
            artWork = @"
 ▄█     █▄     ▄████████  ▄█        ▄████████  ▄██████▄     ▄▄▄▄███▄▄▄▄      ▄████████          ███      ▄██████▄          ▄████████     ███        ▄████████    ▄████████    ▄████████  ▄█     ▄████████  ▄█        ▄█  ███▄▄▄▄      ▄████████    ▄████████ 
███     ███   ███    ███ ███       ███    ███ ███    ███  ▄██▀▀▀███▀▀▀██▄   ███    ███      ▀█████████▄ ███    ███        ███    ███ ▀█████████▄   ███    ███   ███    ███   ███    ███ ███    ███    ███ ███       ███  ███▀▀▀██▄   ███    ███   ███    ███ 
███     ███   ███    █▀  ███       ███    █▀  ███    ███  ███   ███   ███   ███    █▀          ▀███▀▀██ ███    ███        ███    █▀     ▀███▀▀██   ███    ███   ███    ███   ███    ███ ███▌   ███    ███ ███       ███▌ ███   ███   ███    █▀    ███    █▀  
███     ███  ▄███▄▄▄     ███       ███        ███    ███  ███   ███   ███  ▄███▄▄▄              ███   ▀ ███    ███        ███            ███   ▀   ███    ███  ▄███▄▄▄▄██▀   ███    ███ ███▌  ▄███▄▄▄▄██▀ ███       ███▌ ███   ███  ▄███▄▄▄       ███        
███     ███ ▀▀███▀▀▀     ███       ███        ███    ███  ███   ███   ███ ▀▀███▀▀▀              ███     ███    ███      ▀███████████     ███     ▀███████████ ▀▀███▀▀▀▀▀   ▀███████████ ███▌ ▀▀███▀▀▀▀▀   ███       ███▌ ███   ███ ▀▀███▀▀▀     ▀███████████ 
███     ███   ███    █▄  ███       ███    █▄  ███    ███  ███   ███   ███   ███    █▄           ███     ███    ███               ███     ███       ███    ███ ▀███████████   ███    ███ ███  ▀███████████ ███       ███  ███   ███   ███    █▄           ███ 
███ ▄█▄ ███   ███    ███ ███▌    ▄ ███    ███ ███    ███  ███   ███   ███   ███    ███          ███     ███    ███         ▄█    ███     ███       ███    ███   ███    ███   ███    ███ ███    ███    ███ ███▌    ▄ ███  ███   ███   ███    ███    ▄█    ███ 
 ▀███▀███▀    ██████████ █████▄▄██ ████████▀   ▀██████▀    ▀█   ███   █▀    ██████████         ▄████▀    ▀██████▀        ▄████████▀     ▄████▀     ███    █▀    ███    ███   ███    █▀  █▀     ███    ███ █████▄▄██ █▀    ▀█   █▀    ██████████  ▄████████▀  
                         ▀                                                                                                                                      ███    ███                     ███    ███ ▀                                                  
";

        }
        else
        {
            artWork = @"
            ███████╗██╗     ██╗   ██╗██╗███╗   ██╗ ██████╗     ████████╗ ██████╗     ███████╗████████╗ █████╗ ██████╗ 
            ██╔════╝██║     ╚██╗ ██╔╝██║████╗  ██║██╔════╝     ╚══██╔══╝██╔═══██╗    ██╔════╝╚══██╔══╝██╔══██╗██╔══██╗
            █████╗  ██║      ╚████╔╝ ██║██╔██╗ ██║██║  ███╗       ██║   ██║   ██║    ███████╗   ██║   ███████║██████╔╝
            ██╔══╝  ██║       ╚██╔╝  ██║██║╚██╗██║██║   ██║       ██║   ██║   ██║    ╚════██║   ██║   ██╔══██║██╔══██╗
            ██║     ███████╗   ██║   ██║██║ ╚████║╚██████╔╝       ██║   ╚██████╔╝    ███████║   ██║   ██║  ██║██║  ██║
            ╚═╝     ╚══════╝   ╚═╝   ╚═╝╚═╝  ╚═══╝ ╚═════╝        ╚═╝    ╚═════╝     ╚══════╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
                    ";
        }

        Console.WriteLine(artWork);
    }
        static void PrintArtWork(int option)
        {
            string artWork;

            if (option == 4)
            {
                artWork = @"
               _____                 _                                               _                                 
              / ____|               | |                                             (_)                                
             | |       _   _   ___  | |_    ___    _ __ ___     ___   _ __     ___   _    __ _   _ __    _   _   _ __  
             | |      | | | | / __| | __|  / _ \  | '_ ` _ \   / _ \ | '__|   / __| | |  / _` | | '_ \  | | | | | '_ \ 
             | |____  | |_| | \__ \ | |_  | (_) | | | | | | | |  __/ | |      \__ \ | | | (_| | | | | | | |_| | | |_) |
              \_____|  \__,_| |___/  \__|  \___/  |_| |_| |_|  \___| |_|      |___/ |_|  \__, | |_| |_|  \__,_| | .__/ 
                                                                                          __/ |                 | |    
                                                                                         |___/                  |_|                                                                                                    
                    ";
            }
            else if (option == 3)
            {
                artWork = @"
               _____                 _                                         _                   _         
              / ____|               | |                                       | |                 (_)        
             | |       _   _   ___  | |_    ___    _ __ ___     ___   _ __    | |   ___     __ _   _   _ __  
             | |      | | | | / __| | __|  / _ \  | '_ ` _ \   / _ \ | '__|   | |  / _ \   / _` | | | | '_ \ 
             | |____  | |_| | \__ \ | |_  | (_) | | | | | | | |  __/ | |      | | | (_) | | (_| | | | | | | |
              \_____|  \__,_| |___/  \__|  \___/  |_| |_| |_|  \___| |_|      |_|  \___/   \__, | |_| |_| |_|
                                                                                            __/ |            
                                                                                           |___/             
                                                                                                                      
                    ";
            }
            else if (option == 2)
            {
                artWork = @"
                             _               _                   _                                 
                 /\         | |             (_)                 (_)                                
                /  \      __| |  _ __ ___    _   _ __      ___   _    __ _   _ __    _   _   _ __  
               / /\ \    / _` | | '_ ` _ \  | | | '_ \    / __| | |  / _` | | '_ \  | | | | | '_ \ 
              / ____ \  | (_| | | | | | | | | | | | | |   \__ \ | | | (_| | | | | | | |_| | | |_) |
             /_/    \_\  \__,_| |_| |_| |_| |_| |_| |_|   |___/ |_|  \__, | |_| |_|  \__,_| | .__/ 
                                                                      __/ |                 | |    
                                                                     |___/                  |_|                                                                                 
                        ";
            }
            else
            {
                artWork = @"
                             _               _             _                   _         
                 /\         | |             (_)           | |                 (_)        
                /  \      __| |  _ __ ___    _   _ __     | |   ___     __ _   _   _ __  
               / /\ \    / _` | | '_ ` _ \  | | | '_ \    | |  / _ \   / _` | | | | '_ \ 
              / ____ \  | (_| | | | | | | | | | | | | |   | | | (_) | | (_| | | | | | | |
             /_/    \_\  \__,_| |_| |_| |_| |_| |_| |_|   |_|  \___/   \__, | |_| |_| |_|
                                                                        __/ |            
                                                                       |___/                                                                                          
                    ";
            }

            Console.WriteLine(artWork);
        }

        public static List<Customer> getCustomersCollection()
        {
            return customersCollection;
        }

    }

    
}
