using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    internal class Customer
    {
        // ************************************************************ Fields ************************************************************

        private readonly string userID;
        private string email;
        private string name;
        private string phone;
        private readonly string password;
        private string address;
        private int age;
        public List<Flight> flightsRegisteredByUser;
        public List<int> numOfTicketsBookedByUser;
        public static readonly List<Customer> customerCollection = User.getCustomersCollection();

        // ************************************************************ Behaviours/Methods ************************************************************

        // Default constructor
        public Customer()
        {
            this.userID = null;
            this.name = null;
            this.email = null;
            this.password = null;
            this.phone = null;
            this.address = null;
            this.age = 0;
        }
        public Customer(string name, string email, string password, string phone, string address, int age)
        {
            RandomGenerator random = new RandomGenerator();
            random.RandomIDGen();
            this.name = name;
            this.userID = random.GetRandomNumber();
            this.email = email;
            this.password = password;
            this.phone = phone;
            this.address = address;
            this.age = age;
            this.flightsRegisteredByUser = new List<Flight>();
            this.numOfTicketsBookedByUser = new List<int>();
        }

        // Method to register a new customer
        public void AddNewCustomer()
        {
            Console.WriteLine($"\n\n\n{new string(' ', 60)} ++++++++++++++ Welcome to the Customer Registration Portal ++++++++++++++");
            Console.Write("Enter your name :\t");
            string name = Console.ReadLine();
            Console.Write("Enter your email address :\t");
            string email = Console.ReadLine();
            while (IsUniqueData(email))
            {
                Console.WriteLine("ERROR!!! User with the same email already exists... Use a new email or login using the previous credentials....");
                Console.Write("Enter your email address :\t");
                email = Console.ReadLine();
            }
            Console.Write("Enter your Password :\t");
            string password = Console.ReadLine();
            Console.Write("Enter your Phone number :\t");
            string phone = Console.ReadLine();
            Console.Write("Enter your address :\t");
            string address = Console.ReadLine();
            Console.Write("Enter your age :\t");
            int age = int.Parse(Console.ReadLine());
            customerCollection.Add(new Customer(name, email, password, phone, address, age));
        }
        private string ToString(int i)
        {
            return string.Format("{0,10}| {1,-10} | {2,-10} | {3,-32} | {4,-7} | {5,-27} | {6,-35} | {7,-23} |", "", i, RandomIDDisplay(userID), name, age, email, address, phone);
        }

        public void SearchUser(string ID)
        {
			//fix code
			bool isFound = false;
			if (customerCollection.Count > 0)
			{
				Customer customerWithTheID = customerCollection[0];
				foreach (Customer c in customerCollection)
				{
					if (ID.Equals(c.userID))
					{											
						Console.WriteLine($"{"Customer Found...!!! Here is the Full Record...!!!\n\n\n".PadLeft(50)}"); //FIX
						DisplayHeader();
						isFound = true;
						customerWithTheID = c;
						break;
					}
				}
				if (isFound)
				{
					Console.WriteLine(customerWithTheID.ToString(1));
					Console.WriteLine($"{new string(' ', 10)}+------------+------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+\n");
				}
				else
				{					
					Console.WriteLine($"{"No Customer with the ID {ID} Found...!!!".PadLeft(50)}"); //FIX
				}
			}
			else
			{				
				Console.WriteLine($"{"No Customer with the ID {ID} Found...!!!".PadLeft(50)}");//FIX
			}
		}
        public bool IsUniqueData(string emailID)
        {
            bool isUnique = false;

            foreach (Customer c in customerCollection)
            {
                if (emailID.Equals(c.email))
                {
                    isUnique = true;
                    break;
                }
            }

            return isUnique;
        }
        public void EditUserInfo(string ID)
        {
            bool isFound = false;
            Console.WriteLine();

            foreach (Customer c in customerCollection)
            {
                if (ID.Equals(c.userID))
                {
                    isFound = true;

                    Console.Write("Enter the new name of the Passenger:\t");
                    c.name = Console.ReadLine();

                    Console.Write($"Enter the new email address of Passenger {c.name}:\t");
                    c.email = Console.ReadLine();

                    Console.Write($"Enter the new Phone number of Passenger {c.name}:\t");
                    c.phone = Console.ReadLine();

                    Console.Write($"Enter the new address of Passenger {c.name}:\t");
                    c.address = Console.ReadLine();

                    Console.Write($"Enter the new age of Passenger {c.name}:\t");
                    c.age = Convert.ToInt32(Console.ReadLine());

                    DisplayCustomersData(false);
                    break;
                }
            }

            if (!isFound)
            {
                Console.WriteLine($"{"No Customer with the ID {ID} Found...!!!\n".PadLeft(50)}");//FIX
            }
		}

        public void DeleteUser(string ID)
        {
            bool isFound = false;

            foreach (Customer customer in customerCollection)
            {
                if (ID.Equals(customer.userID))
                {
                    isFound = true;
                    customerCollection.Remove(customer);
                    Console.WriteLine($"{"Printing all Customer's Data after deleting Customer with the ID {ID}.....!!!!\n".PadLeft(50)}");//fix code
                    DisplayCustomersData(false);
                    break;
                }
            }

            if (!isFound)
            {				
				Console.WriteLine($"{"No Customer with the ID {ID} Found...!!!\n".PadLeft(50)}");//fix code
			}
        }

        public void DisplayCustomersData(bool showHeader)
        {
            if (showHeader)
            {
                DisplayArtWork(3);
            }

            DisplayHeader();

            int i = 0;

            foreach (Customer c in customerCollection)
            {
                i++;
                Console.WriteLine(c.ToString(i));
                Console.WriteLine($"{new string(' ', 10)}+------------+------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+");
            }
        }
        void DisplayHeader()
        {
            Console.WriteLine();
            Console.WriteLine($"{new string(' ', 10)}+------------+------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+");
            Console.WriteLine($"{new string(' ', 10)}| SerialNum  |   UserID   | Passenger Names                  | Age     | EmailID\t\t       | Home Address\t\t\t     | Phone Number\t       |");
            Console.WriteLine($"{new string(' ', 10)}+------------+------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+");
        }

        public string RandomIDDisplay(string randomID)
        {
            StringBuilder newString = new StringBuilder();
            for (int i = 0; i <= randomID.Length; i++)
            {
                if (i == 3)
                {
                    newString.Append(" ").Append(randomID[i]);
                }
                else if (i < randomID.Length)
                {
                    newString.Append(randomID[i]);
                }
            }
            return newString.ToString();
        }

        public void AddNewFlightToCustomerList(Flight f)
        {
            this.flightsRegisteredByUser.Add(f);
            //numOfFlights++;
        }
        public void AddExistingFlightToCustomerList(int index, int numOfTickets)
        {
            int newNumOfTickets = numOfTicketsBookedByUser[index] + numOfTickets;
            numOfTicketsBookedByUser[index] = newNumOfTickets;
        }

        public void DisplayArtWork(int option)
        {
            string artWork = "";
            if (option == 1)
            {
                artWork = @"                                       
            ███    ██ ███████ ██     ██      ██████ ██    ██ ███████ ████████  ██████  ███    ███ ███████ ██████  
            ████   ██ ██      ██     ██     ██      ██    ██ ██         ██    ██    ██ ████  ████ ██      ██   ██ 
            ██ ██  ██ █████   ██  █  ██     ██      ██    ██ ███████    ██    ██    ██ ██ ████ ██ █████   ██████  
            ██  ██ ██ ██      ██ ███ ██     ██      ██    ██      ██    ██    ██    ██ ██  ██  ██ ██      ██   ██ 
            ██   ████ ███████  ███ ███       ██████  ██████  ███████    ██     ██████  ██      ██ ███████ ██   ██ 
                    ";
            }
            if (option == 2)
            {
                artWork = @"                                       
            ███████ ███████  █████  ██████   ██████ ██   ██      ██████ ██    ██ ███████ ████████  ██████  ███    ███ ███████ ██████  
            ██      ██      ██   ██ ██   ██ ██      ██   ██     ██      ██    ██ ██         ██    ██    ██ ████  ████ ██      ██   ██ 
            ███████ █████   ███████ ██████  ██      ███████     ██      ██    ██ ███████    ██    ██    ██ ██ ████ ██ █████   ██████  
                 ██ ██      ██   ██ ██   ██ ██      ██   ██     ██      ██    ██      ██    ██    ██    ██ ██  ██  ██ ██      ██   ██ 
            ███████ ███████ ██   ██ ██   ██  ██████ ██   ██      ██████  ██████  ███████    ██     ██████  ██      ██ ███████ ██   ██ 
                    ";
            }
            if (option == 3)
            {
                artWork = @"                                       
            ███████ ██   ██  ██████  ██     ██      █████  ██      ██                          
            ██      ██   ██ ██    ██ ██     ██     ██   ██ ██      ██                          
            ███████ ███████ ██    ██ ██  █  ██     ███████ ██      ██                          
                 ██ ██   ██ ██    ██ ██ ███ ██     ██   ██ ██      ██                          
            ███████ ██   ██  ██████   ███ ███      ██   ██ ███████ ███████                     
                                                                                   
                                                                                   
            ██████   █████  ███████ ███████ ███████ ███    ██  ██████  ███████ ██████  ███████ 
            ██   ██ ██   ██ ██      ██      ██      ████   ██ ██       ██      ██   ██ ██      
            ██████  ███████ ███████ ███████ █████   ██ ██  ██ ██   ███ █████   ██████  ███████ 
            ██      ██   ██      ██      ██ ██      ██  ██ ██ ██    ██ ██      ██   ██      ██ 
            ██      ██   ██ ███████ ███████ ███████ ██   ████  ██████  ███████ ██   ██ ███████ 
                    ";
            }
            if (option == 4)
            {
                artWork = @"                                       
            ██████  ███████  ██████  ██ ███████ ████████ ███████ ██████  ███████ ██████        
            ██   ██ ██      ██       ██ ██         ██    ██      ██   ██ ██      ██   ██       
            ██████  █████   ██   ███ ██ ███████    ██    █████   ██████  █████   ██   ██       
            ██   ██ ██      ██    ██ ██      ██    ██    ██      ██   ██ ██      ██   ██       
            ██   ██ ███████  ██████  ██ ███████    ██    ███████ ██   ██ ███████ ██████        
                                                                                   
                                                                                   
            ██████   █████  ███████ ███████ ███████ ███    ██  ██████  ███████ ██████  ███████ 
            ██   ██ ██   ██ ██      ██      ██      ████   ██ ██       ██      ██   ██ ██      
            ██████  ███████ ███████ ███████ █████   ██ ██  ██ ██   ███ █████   ██████  ███████ 
            ██      ██   ██      ██      ██ ██      ██  ██ ██ ██    ██ ██      ██   ██      ██ 
            ██      ██   ██ ███████ ███████ ███████ ██   ████  ██████  ███████ ██   ██ ███████ 
                                                                                   
                                                                                   
            ██ ███    ██     ███████ ██      ██  ██████  ██   ██ ████████                      
            ██ ████   ██     ██      ██      ██ ██       ██   ██    ██                         
            ██ ██ ██  ██     █████   ██      ██ ██   ███ ███████    ██                         
            ██ ██  ██ ██     ██      ██      ██ ██    ██ ██   ██    ██                         
            ██ ██   ████     ██      ███████ ██  ██████  ██   ██    ██                                                                                                                                                                         
                    ";
            }
            if (option == 5)
            {
                artWork = @"                                       
            ██████  ███████ ██      ███████ ████████ ███████     ███████ ██      ██  ██████  ██   ██ ████████ 
            ██   ██ ██      ██      ██         ██    ██          ██      ██      ██ ██       ██   ██    ██    
            ██   ██ █████   ██      █████      ██    █████       █████   ██      ██ ██   ███ ███████    ██    
            ██   ██ ██      ██      ██         ██    ██          ██      ██      ██ ██    ██ ██   ██    ██    
            ██████  ███████ ███████ ███████    ██    ███████     ██      ███████ ██  ██████  ██   ██    ██    
                                                                                                  
                                                                                                                                                                                             
                    ";
            }
            Console.WriteLine(artWork);
        }


        // ************************************************************ Setters & Getters ************************************************************
        public List<Flight> GetFlightsRegisteredByUser()
        {
            return flightsRegisteredByUser;
        }

        public string GetPassword()
        {
            return password;
        }

        public string GetPhone()
        {
            return phone;
        }

        public string GetAddress()
        {
            return address;
        }

        public string GetEmail()
        {
            return email;
        }

        public int GetAge()
        {
            return age;
        }

        public string GetUserID()
        {
            return userID;
        }

        public string GetName()
        {
            return name;
        }

        public List<int> GetNumOfTicketsBookedByUser()
        {
            return numOfTicketsBookedByUser;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetEmail(string email)
        {
            this.email = email;
        }

        public void SetPhone(string phone)
        {
            this.phone = phone;
        }

        public void SetAddress(string address)
        {
            this.address = address;
        }

        public void SetAge(int age)
        {
            this.age = age;
        }
    }
}
