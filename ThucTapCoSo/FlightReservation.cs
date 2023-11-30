using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    internal class FlightReservation : IDisplayClass
    {
        // Fields
        Flight flight = new Flight();
        int flightIndexInFlightList;      

        public void BookFlight(string flightNo, int numOfTickets, string userID)
        {
			Console.OutputEncoding = Encoding.Unicode;
			bool isFound = false;
            foreach (Flight f1 in flight.FlightList)
            {
                if (flightNo.Equals(f1.FlightNumber, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (Customer customer in Customer.customerCollection)
                    {
                        if (userID.Equals(customer.GetUserID()))
                        {
                            isFound = true;
                            f1.SetNoOfSeatsInTheFlight(f1.NoOfSeats - numOfTickets);
                            if (!f1.IsCustomerAlreadyAdded(f1.ListOfRegisteredCustomersInAFlight, customer))
                            {
                                f1.AddNewCustomerToFlight(customer);
                            }
                            if (IsFlightAlreadyAddedToCustomerList(customer.GetFlightsRegisteredByUser(), f1))
                            {
                                AddNumberOfTicketsToAlreadyBookedFlight(customer, numOfTickets);
                                if (FlightIndex(flight.FlightList, flight) != -1)
                                {
                                    customer.AddExistingFlightToCustomerList(FlightIndex(flight.FlightList, flight), numOfTickets);
                                }
                            }
                            else
                            {
                                customer.AddNewFlightToCustomerList(f1);
                                AddNumberOfTicketsForNewFlight(customer, numOfTickets);
                            }
                            break;
                        }
                    }
                }
            }
            if (!isFound)
            {
                Console.WriteLine($"Invalid Flight Number...! No flight with the ID \"{flightNo}\" was found...");
            }
            else
            {
                Console.WriteLine($"\n {new string(' ', 50)} You've booked {numOfTickets} tickets for Flight \"{flightNo.ToUpper()}\"...");
            }
        }

        public void CancelFlight(string userID)
        {
			Console.OutputEncoding = Encoding.Unicode;
			string flightNum = "";
            DisplayFlightsRegisteredByOneUser(userID);
            Console.WriteLine("Enter the Flight Number of the Flight you want to cancel:");
            flightNum = Console.ReadLine();
            Console.WriteLine("Enter the number of tickets to cancel:");
            int numOfTickets = Convert.ToInt32(Console.ReadLine());

            bool isFound = false;
            int index = 0;
            foreach (Customer customer in Customer.customerCollection)
            {
                if (userID.Equals(customer.GetUserID()))
                {
                    if (customer.flightsRegisteredByUser.Count != 0)
                    {
						Console.WriteLine($"++++++++++++++ Here is the list of all the Flights registered by you ++++++++++++++".PadLeft(50));
						DisplayFlightsRegisteredByOneUser(userID);

                        foreach (Flight flight in customer.flightsRegisteredByUser)
                        {
                            if (flightNum.Equals(flight.FlightNumber, StringComparison.OrdinalIgnoreCase))
                            {
                                isFound = true;
                                int numOfTicketsForFlight = customer.numOfTicketsBookedByUser[index];

                                while (numOfTickets > numOfTicketsForFlight)
                                {
                                    Console.Write($"ERROR!!! Number of tickets cannot be greater than {numOfTicketsForFlight} for this flight. Please enter the number of tickets again:");
                                    numOfTickets = Convert.ToInt32(Console.ReadLine());
                                }

                                int ticketsToBeReturned;

                                if (numOfTicketsForFlight == numOfTickets)
                                {
                                    ticketsToBeReturned = flight.NoOfSeats + numOfTicketsForFlight;
                                    customer.numOfTicketsBookedByUser.RemoveAt(index);
                                    customer.flightsRegisteredByUser.RemoveAt(index);
                                }
                                else
                                {
                                    ticketsToBeReturned = numOfTickets + flight.NoOfSeats;
                                    customer.numOfTicketsBookedByUser[index] = numOfTicketsForFlight - numOfTickets;
                                }

                                flight.numOfSeatsInTheFlight = ticketsToBeReturned;
                                break;
                            }
                            index++;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No Flight Has been Registered by you with ID \"{flightNum.ToUpper()}\".....");
                    }

                    if (!isFound)
                    {
                        Console.WriteLine($"ERROR!!! Couldn't find Flight with ID \"{flightNum.ToUpper()}\".....");
                    }
                }
            }
        }

        void AddNumberOfTicketsToAlreadyBookedFlight(Customer customer, int numOfTickets)
        {
            int newNumOfTickets = customer.GetNumOfTicketsBookedByUser()[flightIndexInFlightList] + numOfTickets;
            customer.GetNumOfTicketsBookedByUser()[flightIndexInFlightList] = newNumOfTickets;
        }

        void AddNumberOfTicketsForNewFlight(Customer customer, int numOfTickets)
        {
            customer.GetNumOfTicketsBookedByUser().Add(numOfTickets);
        }

        bool IsFlightAlreadyAddedToCustomerList(List<Flight> flightList, Flight flight)
        {
            bool addedOrNot = false;
            foreach (Flight flight1 in flightList)
            {
                if (flight1.FlightNumber.Equals(flight.FlightNumber, StringComparison.OrdinalIgnoreCase))
                {
                    this.flightIndexInFlightList = flightList.IndexOf(flight1);
                    addedOrNot = true;
                    break;
                }
            }
            return addedOrNot;
        }

        string FlightStatus(Flight flight)
        {
			Console.OutputEncoding = Encoding.Unicode;
			bool isFlightAvailable = false;
            foreach (Flight list in flight.FlightList)
            {
                if (list.FlightNumber.Equals(flight.FlightNumber, StringComparison.OrdinalIgnoreCase))
                {
                    isFlightAvailable = true;
                    break;
                }
            }
            return isFlightAvailable ? "As Per Schedule" : "   Cancelled   ";
        }
        public string ToString(int serialNum, Flight flight, Customer customer)
        {
            return string.Format("| {0,-4} | {1,-41} | {2,-9} | \t{3,-9} | {4,-21} | {5,-22} | {6,-10}  |   {7,-6}Hrs |  {8,-4}  | {9,-10} |",
                                 serialNum, flight.FlightSchedule, flight.FlightNumber, customer.numOfTicketsBookedByUser[serialNum - 1],
                                 flight.FromWhichCity, flight.ToWhichCity, flight.FetchArrivalTime(), flight.FlightTime, flight.Gate, FlightStatus(flight));
        }
        public void DisplayFlightsRegisteredByOneUser(string userID)
        {
			Console.OutputEncoding = Encoding.Unicode;
			bool flightsFound = false; // Flag to check if any flights are found for the given user            
			Console.WriteLine();
			Console.Write("+------+-------------------------------------------+-----------+------------------+-----------------------+------------------------+---------------------------+-------------+--------+-----------------+\n");
			Console.WriteLine("| Num  | FLIGHT SCHEDULE\t\t\t   | FLIGHT NO |  Booked Tickets  | \tFROM ====>>       | \t====>> TO\t   | \t    ARRIVAL TIME       | FLIGHT TIME |  GATE  |  FLIGHT STATUS  |");
			Console.Write("+------+-------------------------------------------+-----------+------------------+-----------------------+------------------------+---------------------------+-------------+--------+-----------------+\n");
            

			foreach (Customer customer in Customer.customerCollection)
			{
				if (userID.Equals(customer.GetUserID()))
				{
					List<Flight> flights = customer.GetFlightsRegisteredByUser();
					int size = flights.Count;

					if (size > 0)
					{
						flightsFound = true;

						for (int i = 0; i < size; i++)
						{
							Console.WriteLine(ToString((i + 1), flights[i], customer));
							Console.Write("+------+-------------------------------------------+-----------+------------------+-----------------------+------------------------+---------------------------+-------------+--------+-----------------+\n");
						}
					}
				}
			}

			if (!flightsFound)
			{
				Console.WriteLine($"No flights registered for the user with ID {userID}.");
			}
		}
        public string ToString(int serialNum, Customer customer, int index)
        {
            return string.Format("          | {0,-10} | {1,-10} | {2,-32} | {3,-7} | {4,-27} | {5,-35} | {6,-23} |       {7,-7}  |", (serialNum + 1), customer.RandomIDDisplay(customer.GetUserID()), customer.GetName(),
                customer.GetAge(), customer.GetEmail(), customer.GetAddress(), customer.GetPhone(), customer.numOfTicketsBookedByUser[index]);
        }


        public void DisplayHeaderForUsers(Flight flight, List<Customer> customers)
        {
			Console.OutputEncoding = Encoding.Unicode;
			Console.WriteLine($"\n{new string('+', 65)} Displaying Registered Customers for Flight No. \"{flight.FlightNumber,-6}\" {new string('+', 10)}\n");
            Console.WriteLine($"{new string(' ', 10)}+------------+------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+----------------+");
            Console.WriteLine($"{new string(' ', 10)}| SerialNum  |   UserID   | Passenger Names                  | Age     | EmailID\t\t       | Home Address\t\t\t     | Phone Number\t       | Booked Tickets |");
            Console.WriteLine($"{new string(' ', 10)}+------------+------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+----------------+");

            int size = flight.ListOfRegisteredCustomersInAFlight.Count;
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(ToString(i, customers[i], FlightIndex(customers[i].flightsRegisteredByUser, flight)));
                Console.WriteLine($"{new string(' ', 10)}+------------+------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+----------------+");
            }
        }
        public void DisplayRegisteredUsersForAllFlight()
        {
            Console.WriteLine();
			foreach (Flight flight in flight.FlightList)
			{
                List<Customer> customers = flight.ListOfRegisteredCustomersInAFlight;
                int size = customers.Count;
                if (size != 0)
                {
                    DisplayHeaderForUsers(flight, customers);
                }
            }
        }        

        int FlightIndex(List<Flight> flightList, Flight flight)
        {
            int index = -1;
            for (int i = 0; i < flightList.Count; i++)
            {
                if (flightList[i].Equals(flight))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public void DisplayRegisteredUsersForASpecificFlight(string flightNum)
        {
            Console.WriteLine();
			foreach (Flight flight in flight.FlightList)
			{
                List<Customer> customers = flight.ListOfRegisteredCustomersInAFlight;
                if (flight.FlightNumber.Equals(flightNum, StringComparison.OrdinalIgnoreCase))
                {
                    DisplayHeaderForUsers(flight, customers);
                }
            }
        }
        public void DisplayArtWork(int option)
        {
			Console.OutputEncoding = Encoding.Unicode;
			string artWork = string.Empty;

            if (option == 1)
            {
                artWork = @"
                    ██████   ██████   ██████  ██   ██     ███████ ██      ██  ██████  ██   ██ ████████ 
                    ██   ██ ██    ██ ██    ██ ██  ██      ██      ██      ██ ██       ██   ██    ██    
                    ██████  ██    ██ ██    ██ █████       █████   ██      ██ ██   ███ ███████    ██    
                    ██   ██ ██    ██ ██    ██ ██  ██      ██      ██      ██ ██    ██ ██   ██    ██    
                    ██████   ██████   ██████  ██   ██     ██      ███████ ██  ██████  ██   ██    ██    
                                                                                   
                                                                                   
                    ";
            }
            else if (option == 2)
            {
                artWork = @"
                    ███████ ██████  ██ ████████     ██ ███    ██ ███████  ██████  
                    ██      ██   ██ ██    ██        ██ ████   ██ ██      ██    ██ 
                    █████   ██   ██ ██    ██        ██ ██ ██  ██ █████   ██    ██ 
                    ██      ██   ██ ██    ██        ██ ██  ██ ██ ██      ██    ██ 
                    ███████ ██████  ██    ██        ██ ██   ████ ██       ██████  
                                                              
                                                              
                    ";
            }
            else if (option == 3)
            {
                artWork = @"
                    ██████  ███████ ██      ███████ ████████ ███████      █████   ██████  ██████  ██████  ██    ██ ███    ██ ████████ 
                    ██   ██ ██      ██      ██         ██    ██          ██   ██ ██      ██      ██    ██ ██    ██ ████   ██    ██    
                    ██   ██ █████   ██      █████      ██    █████       ███████ ██      ██      ██    ██ ██    ██ ██ ██  ██    ██    
                    ██   ██ ██      ██      ██         ██    ██          ██   ██ ██      ██      ██    ██ ██    ██ ██  ██ ██    ██    
                    ██████  ███████ ███████ ███████    ██    ███████     ██   ██  ██████  ██████  ██████   ██████  ██   ████    ██    
                                                                                                                  
                                                                                                                  
                    ";
            }
            else if (option == 4)
            {
                artWork = @"
                    ██████   █████  ███    ██ ██████   ██████  ███    ███     ███████ ██      ██  ██████  ██   ██ ████████ 
                    ██   ██ ██   ██ ████   ██ ██   ██ ██    ██ ████  ████     ██      ██      ██ ██       ██   ██    ██    
                    ██████  ███████ ██ ██  ██ ██   ██ ██    ██ ██ ████ ██     █████   ██      ██ ██   ███ ███████    ██    
                    ██   ██ ██   ██ ██  ██ ██ ██   ██ ██    ██ ██  ██  ██     ██      ██      ██ ██    ██ ██   ██    ██    
                    ██   ██ ██   ██ ██   ████ ██████   ██████  ██      ██     ██      ███████ ██  ██████  ██   ██    ██    
                                                                                                       
                                                                                                       
                    ███████  ██████ ██   ██ ███████ ██████  ██    ██ ██      ███████                                       
                    ██      ██      ██   ██ ██      ██   ██ ██    ██ ██      ██                                            
                    ███████ ██      ███████ █████   ██   ██ ██    ██ ██      █████                                         
                         ██ ██      ██   ██ ██      ██   ██ ██    ██ ██      ██                                            
                    ███████  ██████ ██   ██ ███████ ██████   ██████  ███████ ███████                                       
                                                                                                       
                                                                                                       
                    ";
            }
            else if (option == 5)
            {
                artWork = @"
                     ██████  █████  ███    ██  ██████ ███████ ██          ███████ ██      ██  ██████  ██   ██ ████████ 
                    ██      ██   ██ ████   ██ ██      ██      ██          ██      ██      ██ ██       ██   ██    ██    
                    ██      ███████ ██ ██  ██ ██      █████   ██          █████   ██      ██ ██   ███ ███████    ██    
                    ██      ██   ██ ██  ██ ██ ██      ██      ██          ██      ██      ██ ██    ██ ██   ██    ██    
                     ██████ ██   ██ ██   ████  ██████ ███████ ███████     ██      ███████ ██  ██████  ██   ██    ██    
                                                                                                   
                                                                                                   
                    ";
            }
            else if (option == 6)
            {
                artWork = @"
                    ██████  ███████  ██████  ██ ███████ ████████ ███████ ██████  ███████ ██████      ███████ ██      ██  ██████  ██   ██ ████████ 
                    ██   ██ ██      ██       ██ ██         ██    ██      ██   ██ ██      ██   ██     ██      ██      ██ ██       ██   ██    ██    
                    ██████  █████   ██   ███ ██ ███████    ██    █████   ██████  █████   ██   ██     █████   ██      ██ ██   ███ ███████    ██    
                    ██   ██ ██      ██    ██ ██      ██    ██    ██      ██   ██ ██      ██   ██     ██      ██      ██ ██    ██ ██   ██    ██    
                    ██   ██ ███████  ██████  ██ ███████    ██    ███████ ██   ██ ███████ ██████      ██      ███████ ██  ██████  ██   ██    ██    
                                                                                                                              
                                                                                                                              
                    ██████  ██    ██     ██████   █████  ███████ ███████ ███████ ███    ██  ██████  ███████ ██████                                
                    ██   ██  ██  ██      ██   ██ ██   ██ ██      ██      ██      ████   ██ ██       ██      ██   ██                               
                    ██████    ████       ██████  ███████ ███████ ███████ █████   ██ ██  ██ ██   ███ █████   ██████                                
                    ██   ██    ██        ██      ██   ██      ██      ██ ██      ██  ██ ██ ██    ██ ██      ██   ██                               
                    ██████     ██        ██      ██   ██ ███████ ███████ ███████ ██   ████  ██████  ███████ ██   ██                               
                                                                                                                              
                                                                                                                              
                    ";
            }
            else
            {
                artWork = @"
                    ██       ██████   ██████       ██████  ██    ██ ████████ 
                    ██      ██    ██ ██           ██    ██ ██    ██    ██    
                    ██      ██    ██ ██   ███     ██    ██ ██    ██    ██    
                    ██      ██    ██ ██    ██     ██    ██ ██    ██    ██    
                    ███████  ██████   ██████       ██████   ██████     ██    
                                                         
                                                         
                    ";
            }

            Console.WriteLine(artWork);
        }

    }
}
