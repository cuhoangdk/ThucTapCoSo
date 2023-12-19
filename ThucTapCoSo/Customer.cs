using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
        private DateTime birth;
        //public List<Flight> flightsRegisteredByUser;
        //public List<int> numOfTicketsBookedByUser;
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
            this.birth = DateTime.Now;
        }
        public Customer(string userID, string name, string email, string password, string phone, string address, DateTime birth)
        {
            this.name = name;
            this.userID = userID;
            this.email = email;
            this.password = password;
            this.phone = phone;
            this.address = address;
            this.birth = birth;
        }

        // Method to register a new 
        
        public void AddNewCustomer()
        {
            RandomGenerator random = new RandomGenerator();
            random.RandomIDGen();
            string userID = random.GetRandomNumber();

            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");

            string filePath = Path.Combine(datatxt, "Customer.txt");

            Console.WriteLine($"\n\n\n{new string(' ', 30)} ++++++++++++++ CHÀO MỪNG BẠN ĐẾN VỚI CỔNG ĐĂNG KÍ CỦA KHÁCH HÀNG ++++++++++++++");
            Console.Write("\tHỌ VÀ TÊN:\t");
            string name = Console.ReadLine();
            Console.Write("\tEMAIL :\t");
            string email = Console.ReadLine();
            while (IsUniqueData(email) || !IsValidEmail(email))
            {
                Console.WriteLine("ĐỊA CHỈ EMAIL ĐÃ TỒN TẠI HOẶC KHÔNG HỢP LỆ");
                Console.Write("EMAIL :\t");
                email = Console.ReadLine();
            }
            Console.Write("\tMẬT KHẨU:\t");
            string password = Console.ReadLine();
            Console.Write("\tSỐ ĐIỆN THOẠI:\t");
            string phone = Console.ReadLine();
            Console.Write("\tĐỊA CHỈ:\t");
            string address = Console.ReadLine();
            Console.Write("\tNGÀY THÁNG NĂM SINH:\t");
            DateTime birth;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out birth))
            {
                Console.Write("VUI LÒNG NHẬP NGÀY SINH ĐÚNG ĐỊNH DẠNG: \t");
			}

            //true là dùng để ghi tiếp theo vào file .txt, StreamWriter(filePath): là dùng để ghi đè lên file cũ 
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"1;{userID};{name};{email};{password};{phone};{address};{birth.ToString("dd/MM/yyyy")}");
            }
        }

        public void EditCustomerInfo(string ID)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            bool isFound = false;

            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");

            string filePath = Path.Combine(datatxt, "Customer.txt");

            Console.WriteLine();

            string[] line = File.ReadAllLines(filePath);

            for (int i = 0; i < line.Length; i++)
            {
                string[] data = line[i].Split(';');

                if (ID.Equals(data[1]) && data[0] == "1")
                {
                    isFound = true;

                    Console.WriteLine("\tNHẬP THÔNG TIN MỚI CỦA KHÁCH HÀNG!");
                    Console.WriteLine("\tBẤM ENTER ĐỂ BỎ QUA KHÔNG CHỈNH SỬA!");

                    Console.Write("\tHỌ VÀ TÊN:\t");
                    string name = Console.ReadLine();
                    Console.Write("\tEMAIL :\t");
                    string email = Console.ReadLine();
                    while (email != "" && (IsUniqueData(email) || !IsValidEmail(email)))
                    {
                        Console.WriteLine("\tĐỊA CHỈ EMAIL ĐÃ TỒN TẠI HOẶC KHÔNG HỢP LỆ");
                        Console.Write("\tEMAIL :\t");
                        email = Console.ReadLine();
                    }
                    Console.Write("\tSỐ ĐIỆN THOẠI:\t");
                    string phone = Console.ReadLine();
                    Console.Write("\tĐỊA CHỈ:\t");
                    string address = Console.ReadLine();
                    Console.Write("\tNGÀY THÁNG NĂM SINH:\t");
                    string input;
                    do
                    {
                        Console.Write("\tNGÀY THÁNG NĂM SINH (dd/MM/yyyy):\t");
                        input = Console.ReadLine();

                        if (input == "")
                        {
                            break; // Thoát khỏi vòng lặp nếu người dùng không nhập gì cả
                        }

                        if (!DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out birth))
                        {
                            Console.Write("\tVUI LÒNG NHẬP NGÀY SINH ĐÚNG ĐỊNH DẠNG: \t");
                        }
                    } while (input != "");
                    //data0: userID; data1: name; data2: email; data3: pass; data4: phone; data5: address; data6: age
                    // Ghi vô file
                    if (name != "")
                    {
                        data[2] = "";

                    }
                    if (email != "")
                    {
                        data[3] = email;

                    }
                    if (phone != "")
                    {
                        data[5] = phone;

                    }
                    if (address != "")
                    {
                        data[6] = address;

                    }
                    if (birth != null)
                    {
                        data[7] = birth.ToString("dd/MM/yyyy");

                    }

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

        public void DeleteCustomer(string ID)
        {
            Console.OutputEncoding = Encoding.Unicode;
            bool isFound = false;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");
            string filePath = Path.Combine(datatxt, "Customer.txt");

            string[] Customer = File.ReadAllLines(filePath);

            for (int i = 0; i < Customer.Length; i++)
            {
                // Phân tách dữ liệu trong dòng sử dụng dấu chấm phẩy
                string[] dataCustomer = Customer[i].Split(';');

                if (ID.Equals(dataCustomer[1]) && dataCustomer[0] == "1")
                {
                    dataCustomer[0] = "0";
                    Customer[i] = string.Join(";", dataCustomer);
                    isFound = true;
                    break; // Đã tìm thấy và xóa, không cần kiểm tra các dòng khác
                }
            }

            if (isFound)
            {
                // Ghi lại tất cả các dòng đã được cập nhật vào tệp
                File.WriteAllLines(filePath, Customer);
                Console.WriteLine($"{new string(' ', 10)}Đã xóa Khách hàng với ID {ID}."); // FIX
            }
            else
            {
                Console.WriteLine($"{new string(' ', 10)}Không tìm thấy Khách hàng với ID {ID}...!!!"); // FIX
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
                if (ID.Equals(data[1]) && data[0] == "1")
                {
                    Console.WriteLine($"{new string(' ', 10)}Khách hàng được tìm thấy...!!! Đây là Bản ghi đầy đủ...!!!\n\n\n"); //FIX
                    DisplayHeader();
                    isFound = true;
                    Console.WriteLine($"{new string(' ', 10)}| {i + 1,-5} | {data[1],-13} | {data[2],-32} | {data[7],-7} | {data[3],-27} | {data[6],-30} | {data[5],-14} |");
                    Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
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
        public bool IsValidEmail(string email)
        {
            // Biểu thức chính quy để kiểm tra định dạng email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Sử dụng Regex.IsMatch để kiểm tra tính hợp lệ của email
            return Regex.IsMatch(email, pattern);
        }
       
        public void DisplayCustomersData(bool showHeader)
        {
            Console.OutputEncoding = Encoding.Unicode;
            if (showHeader)
            {
                DisplayArtWork(3);
            }

            DisplayHeader();

            bool isFound = false;
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");

            string filePath = Path.Combine(datatxt, "Customer.txt");

            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(';');
                if (data[0] == "1")
                {
                    isFound = true;
                    Console.WriteLine($"{new string(' ', 10)}| {i + 1,-5} | {data[1],-13} | {data[2],-32} | {data[7],-10} | {data[3],-27} | {data[6],-30} | {data[5],-14} |");
                    Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
                }
            }
            if (!isFound)
            {
                Console.WriteLine("Chưa khách hàng nào đăng kí!!!!");

            }
        }
        void DisplayHeader()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine();
            Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
            Console.WriteLine($"{new string(' ', 10)}| STT   | Mã khách hàng | Tên khách hàng                   | Năm sinh   | Email                       | Địa chỉ                        | Số điện thoại  |");
            Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
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

        //public void AddNewFlightToCustomerList(Flight f)
        //{
        //    this.flightsRegisteredByUser.Add(f);
        //    //numOfFlights++;
        //}
        //public void AddExistingFlightToCustomerList(int index, int numOfTickets)
        //{
        //    int newNumOfTickets = numOfTicketsBookedByUser[index] + numOfTickets;
        //    numOfTicketsBookedByUser[index] = newNumOfTickets;
        //}

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
        //public List<Flight> GetFlightsRegisteredByUser()
        //{
        //    return flightsRegisteredByUser;
        //}

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

        public DateTime GetAge()
        {
            return birth;
        }

        public string GetUserID()
        {
            return userID;
        }

        public string GetName()
        {
            return name;
        }

        //public List<int> GetNumOfTicketsBookedByUser()
        //{
        //    return numOfTicketsBookedByUser;
        //}

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

        public void SetAge(DateTime age)
        {
            this.birth = age;
        }
    }
}
