﻿using System;
using System.Collections.Generic;
using System.IO;
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
        public Customer(string userID,string name, string email, string password, string phone, string address, int age)
        { 
            this.name = name;
            this.userID = userID;
            this.email = email;
            this.password = password;
            this.phone = phone;
            this.address = address;
            this.age = age;
            this.flightsRegisteredByUser = new List<Flight>();
            this.numOfTicketsBookedByUser = new List<int>();
        }

        // Method to register a new 
        public void AddCustomerWithFile()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            //Lấy vị trí hiện tại
            string datatxtFolder = Path.Combine(Directory.GetCurrentDirectory(), "datatxt");

            string filePath = Path.Combine(datatxtFolder, "Customer.txt");

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while( (line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split(';');
                    if(data.Length == 7)
                    {
                        string userID = data[0];
                        string name = data[1];
                        string email = data[2];
                        string password = data[3];
                        string phone = data[4];
                        string address = data[5];
                        int age = int.Parse(data[6]);
                        customerCollection.Add(new Customer(userID, name, email, password, phone, address, age));
                    }
                }
            }
        }
        public void AddNewCustomer()
        {
            RandomGenerator random = new RandomGenerator();
            random.RandomIDGen();
            string userID = random.GetRandomNumber();

            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            Console.WriteLine($"\n\n\n{new string(' ', 30)} ++++++++++++++ Chào mừng bạn đến với Cổng đăng ký của Khách hàng ++++++++++++++");
            Console.Write("Nhập tên của bạn :\t");
            string name = Console.ReadLine();
            Console.Write("Nhập địa chỉ Email của bạn :\t");
            string email = Console.ReadLine();
            while (IsUniqueData(email))
            {
                Console.WriteLine("LỖI!!! Người dùng có cùng địa chỉ email đã tồn tại... Sử dụng một địa chỉ email mới hoặc đăng nhập bằng thông tin đăng nhập trước đó....");
                Console.Write("Nhập địa chỉ Email của bạn :\t");
                email = Console.ReadLine();
            }
            Console.Write("Nhập mật khẩu của bạn :\t");
            string password = Console.ReadLine();
            Console.Write("Nhập số điện thoại của bạn :\t");
            string phone = Console.ReadLine();
            Console.Write("Nhập địa chỉ của bạn :\t");
            string address = Console.ReadLine();
            Console.Write("Nhập tuổi của bạn :\t");
            int age = int.Parse(Console.ReadLine());
            customerCollection.Add(new Customer(name, email, password, phone, address, age));
        }
        private string ToString(int i)
        {
            return string.Format("{0,10}| {1,-10} | {2,-11} | {3,-32} | {4,-7} | {5,-27} | {6,-35} | {7,-23} |", "", i, RandomIDDisplay(userID), name, age, email, address, phone);
        }

        public void SearchUser(string ID)
        {
			Console.OutputEncoding = Encoding.Unicode;
			//fix code
			bool isFound = false;
			if (customerCollection.Count > 0)
			{
				Customer customerWithTheID = customerCollection[0];
				foreach (Customer c in customerCollection)
				{
					if (ID.Equals(c.userID))
					{
                        Console.WriteLine($"{new string(' ', 10)}Khách hàng được tìm thấy...!!! Đây là Bản ghi đầy đủ...!!!\n\n\n"); //FIX
                        DisplayHeader();
						isFound = true;
						customerWithTheID = c;
						break;
					}
				}
				if (isFound)
				{
					Console.WriteLine(customerWithTheID.ToString(1));
					Console.WriteLine($"{new string(' ', 10)}+------------+-------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+\n");
				}
				else
				{
                    Console.WriteLine($"{new string(' ', 10)}Không tìm thấy Khách hàng với ID {ID}...!!!"); //FIX
                }
            }
			else
			{
                Console.WriteLine($"{new string(' ', 10)}Không tìm thấy Khách hàng với ID {ID}...!!!"); //FIX
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
			Console.OutputEncoding = Encoding.Unicode;
			bool isFound = false;

            string datatxtFolder = Path.Combine(Directory.GetCurrentDirectory(), "datatxt");
            string filePath = Path.Combine(datatxtFolder, "Customer.txt");

            Console.WriteLine();

                string[] line= File.ReadAllLines(filePath);

                for(int i=0; i< line.Length; i++)
                {
                    string[] data = line[i].Split(';');
                    
                    if (data.Length == 7 && ID.Equals(data[0]))
                    {
                            isFound = true;
                            Console.Write("Nhập tên mới của Hành khách:\t");
                            data[1] = Console.ReadLine();

                            Console.Write($"Nhập địa chỉ email mới của Hành khách {data[1]}:\t");
                            data[2] = Console.ReadLine();

                            Console.Write($"Nhập số điện thoại mới của Hành khách {data[1]}:\t");
                            data[3] = Console.ReadLine();

                            Console.Write($"Nhập địa chỉ mới của Hành khách {data[1]}:\t");
                            data[4] = Console.ReadLine();

                            Console.Write($"Nhập tuổi mới của Hành khách {data[1]}:\t");
                            data[5] = Console.ReadLine();

                        line[i] = string.Join(";", data);

                    }
                }
            

            /*foreach (Customer c in customerCollection)
            {
                if (ID.Equals(c.userID))
                {
                    isFound = true;

                    Console.Write("Nhập tên mới của Hành khách:\t");
                    c.name = Console.ReadLine();

                    Console.Write($"Nhập địa chỉ email mới của Hành khách {c.name}:\t");
                    c.email = Console.ReadLine();

                    Console.Write($"Nhập số điện thoại mới của Hành khách {c.name}:\t");
                    c.phone = Console.ReadLine();

                    Console.Write($"Nhập địa chỉ mới của Hành khách {c.name}:\t");
                    c.address = Console.ReadLine();

                    Console.Write($"Nhập tuổi mới của Hành khách {c.name}:\t");
					while (!int.TryParse(Console.ReadLine(), out c.age)||c.age<0)
					{
						Console.Write("Số tuổi không hợp lệ vui lòng nhập lại: ");
					}				

                    DisplayCustomersData(false);
                    break;
                }
            }*/

            if (!isFound)
            {
                Console.WriteLine($"{new string(' ', 10)}Không tìm thấy Khách hàng với ID  {ID} ...!!!"); //FIX
            }
            else
            {
                File.WriteAllLines(filePath, line);
                Console.WriteLine("Cập nhật thông tin thành công!");
            }
        }

        public void DeleteUser(string ID)
        {
			Console.OutputEncoding = Encoding.Unicode;
			bool isFound = false;

            foreach (Customer customer in customerCollection)
            {
                if (ID.Equals(customer.userID))
                {
                    isFound = true;
                    customerCollection.Remove(customer);
                    Console.WriteLine($"{new string(' ', 10)}In toàn bộ dữ liệu Khách hàng sau khi xóa Khách hàng với ID {ID}.....!!!!\n");//fix code
                    DisplayCustomersData(false);
                    break;
                }
            }

            if (!isFound)
            {
                Console.WriteLine($"{new string(' ', 10)}Không tìm thấy Khách hàng với ID {ID}...!!!"); //FIX
            }
        }

        public void DisplayCustomersData(bool showHeader)
        {
			Console.OutputEncoding = Encoding.Unicode;
			if (showHeader)
            {
                DisplayArtWork(3);
            }

            DisplayHeader();

            string datatxtFolder = Path.Combine(Directory.GetCurrentDirectory(), "datatxt");
            string filePath = Path.Combine(datatxtFolder, "Customer.txt");

            string[] lines = File.ReadAllLines(filePath);
            for(int i = 0; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(';');
                if(data.Length == 7)
                {
                    Customer customer = new Customer(data[0], data[1], data[2], data[3], data[4], data[5], int.Parse(data[6]));
                    Console.WriteLine(customer.ToString(i + 1));
                    Console.WriteLine($"{new string(' ', 10)}+------------+------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+");
                }
            }

            /*int j = 0;
            foreach (Customer c in customerCollection)
            {
                i++;
                Console.WriteLine(c.ToString(i));
                Console.WriteLine($"{new string(' ', 10)}+------------+------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+");
            }
        }
        void DisplayHeader()
        {
			Console.OutputEncoding = Encoding.Unicode;
			Console.WriteLine();
            Console.WriteLine($"{new string(' ', 10)}+------------+-------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+");
            Console.WriteLine($"{new string(' ', 10)}| STT        |Mã khách hàng| Tên khách hàng                   | Tuổi    | Email\t\t\t| Địa chỉ     \t\t\t      | Số điện thoại           |");
            Console.WriteLine($"{new string(' ', 10)}+------------+-------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+");
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
			Console.OutputEncoding = Encoding.Unicode;
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
