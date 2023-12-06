using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        public Customer(string userID, string name, string email, string password, string phone, string address, int age)
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
        
        public void AddNewCustomer()
        {
            RandomGenerator random = new RandomGenerator();
            random.RandomIDGen();
            string userID = random.GetRandomNumber();

            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            Console.WriteLine($"\n\n\n{new string(' ', 30)} ++++++++++++++ Chào mừng bạn đến với Cổng đăng ký của Khách hàng ++++++++++++++");
            Console.Write("Nhập tên của bạn:\t");
            string name = Console.ReadLine();
            Console.Write("Nhập địa chỉ Email của bạn:\t");
            string email = Console.ReadLine();
            while (IsUniqueData(email))
            {
                Console.WriteLine("LỖI!!! Người dùng có cùng địa chỉ email đã tồn tại... Sử dụng một địa chỉ email mới hoặc đăng nhập bằng thông tin đăng nhập trước đó....");
                Console.Write("Nhập địa chỉ Email của bạn :\t");
                email = Console.ReadLine();
            }
            Console.Write("Nhập mật khẩu của bạn:\t");
            string password = Console.ReadLine();
            Console.Write("Nhập số điện thoại của bạn:\t");
            string phone = Console.ReadLine();
            Console.Write("Nhập địa chỉ của bạn:\t");
            string address = Console.ReadLine();
            Console.Write("Nhập tuổi của bạn:\t");
            int age;
			while (!int.TryParse(Console.ReadLine(), out age) || age < 0)
			{
				Console.Write("Vui lòng nhập số tuổi đúng định dạng: \t");
			}

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");

            string filePath = Path.Combine(datatxt, "Customer.txt");

            //Mỗi khi thêm khách thì ghi thông tin khách vào file Customer.txt
            //true là dùng để ghi tiếp theo vào file .txt, StreamWriter(filePath): là dùng để ghi đè lên file cũ 
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{userID};{name};{email};{password};{phone};{address};{age}");
            }
        }
        public void SearchUser(string ID)
        {
            Console.OutputEncoding = Encoding.Unicode;
            //fix code
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");
            string filePath = Path.Combine(datatxt, "Customer.txt");

            string[] Customers = File.ReadAllLines(filePath);
            bool isFound = false;

            for (int i=0; i<Customers.Length; i++)
            {
                string[] data = Customers[i].Split(';');
                if (ID.Equals(data[0]))
                {
                    Console.WriteLine($"{new string(' ', 10)}Khách hàng được tìm thấy...!!! Đây là Bản ghi đầy đủ...!!!\n\n\n"); //FIX
                    DisplayHeader();
                    isFound = true;
                    Console.WriteLine($"{new string(' ', 10)}| {i + 1,-5} | {data[0],-13} | {data[1],-32} | {data[6],-7} | {data[2],-27} | {data[5],-30} | {data[4],-14} |");
                    Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+---------+-----------------------------+--------------------------------+----------------+");
                    break;
                }
            }
            if(!isFound)
            {
                Console.WriteLine($"{new string(' ', 10)}Không tìm thấy Khách hàng với ID {ID}...!!!"); //FIX
            }
        }
        public bool IsUniqueData(string emailID)
        {
            bool isUnique = false;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");

            string customerPath = Path.Combine(datatxt, "Customer.txt");

            string[] line = File.ReadAllLines(customerPath);

            for (int i = 0; i < line.Length; i++)
            {
                string[] data = line[i].Split(';');
                if (data[2].Equals(emailID))
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

            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");

            string filePath = Path.Combine(datatxt, "Customer.txt");

            Console.WriteLine();

            string[] line = File.ReadAllLines(filePath);

            for (int i = 0; i < line.Length; i++)
            {
                string[] data = line[i].Split(';');

                if (data.Length == 7 && ID.Equals(data[0]))
                {
                    isFound = true;

                    //data0: userID; data1: name; data2: email; data3: pass; data4: phone; data5: address; data6: age
                    Console.Write("Nhập tên mới của Hành khách:\t");
                    data[1] = Console.ReadLine();

                    Console.Write($"Nhập địa chỉ email mới của Hành khách {data[1]}:\t");
                    data[2] = Console.ReadLine();

                    Console.Write($"Nhập số điện thoại mới của Hành khách {data[1]}:\t");
                    data[4] = Console.ReadLine();

                    Console.Write($"Nhập địa chỉ mới của Hành khách {data[1]}:\t");
                    data[5] = Console.ReadLine();

                    Console.Write($"Nhập tuổi mới của Hành khách {data[1]}:\t");
                    int newAge;
                    while (!int.TryParse(Console.ReadLine(), out newAge) || newAge < 0)
                    {
                        Console.Write("Vui lòng nhập số tuổi đúng định dạng: \t");
                    }
                    data[6] = newAge.ToString();

                    line[i] = string.Join(";", data);
                }
            }
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

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");
            //tìm tới thư mục FlightScheduler.txt
            string filePath = Path.Combine(datatxt, "Customer.txt");

            //đọc dòng trong file txt và lưu vào list
            List<string> lines = File.ReadAllLines(filePath).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
				// Phân tách dữ liệu trong dòng sử dụng dấu chấm phẩy
				string[] data = lines[i].Split(';');

                if (data.Length == 7 && ID.Equals(data[0]))
                {
                    // Nếu ID khớp, xóa dòng từ danh sách
                    lines.RemoveAt(i);
					isFound = true;					
					break; // Đã tìm thấy và xóa, không cần kiểm tra các dòng khác
                }
            }

            if (isFound)
            {
                // Ghi lại tất cả các dòng đã được cập nhật vào tệp
                File.WriteAllLines(filePath, lines);
                Console.WriteLine($"{new string(' ', 10)}Đã xóa Khách hàng với ID {ID}."); // FIX
            }
            else
            {
                Console.WriteLine($"{new string(' ', 10)}Không tìm thấy Khách hàng với ID {ID}...!!!"); // FIX
            }
        }
        private string ToString(int i)
        {
            return string.Format("{0,10}| {1,-10} | {2,-11} | {3,-32} | {4,-7} | {5,-27} | {6,-35} | {7,-23} |", "", i, RandomIDDisplay(userID), name, age, email, address, phone);
        }
        public void DisplayCustomersData(bool showHeader)
        {
            Console.OutputEncoding = Encoding.Unicode;
            if (showHeader)
            {
                DisplayArtWork(3);
            }

            DisplayHeader();

            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");

            string filePath = Path.Combine(datatxt, "Customer.txt");

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(';');
                Console.WriteLine($"{new string(' ',10)}| {i+1,-5} | {data[0],-13} | {data[1],-32} | {data[6],-7} | {data[2],-27} | {data[5],-30} | {data[4],-14} |");
                Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+---------+-----------------------------+--------------------------------+----------------+");
            }

            /*int j = 0;
            foreach (Customer c in customerCollection)
            {
                i++;
                Console.WriteLine(c.ToString(i));
                Console.WriteLine($"{new string(' ', 10)}+------------+----------------+----------------------------------+---------+-----------------------------+-------------------------------------+-------------------------+");
            }*/
        }
        void DisplayHeader()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine();
            Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+---------+-----------------------------+--------------------------------+----------------+");
            Console.WriteLine($"{new string(' ', 10)}| STT   | Mã khách hàng | Tên khách hàng                   | Tuổi    | Email\t\t\t     | Địa chỉ\t\t\t      | Số điện thoại  |");
            Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+---------+-----------------------------+--------------------------------+----------------+");
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
