using System;
using System.Collections.Generic;
using System.Globalization;
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

        // ************************************************************ Behaviours/Methods ************************************************************

        // phương thức khởi tạo
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
        //Hàm tạo một người dùng mới
        public void AddNewCustomer()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");
            string filePath = Path.Combine(datatxt, "Customer.txt");

            Generator G = new Generator();
            string userID = G.NewID("Customer.txt");

            Console.WriteLine($"\n\n\n{new string(' ', 30)} ++++++++++++++ CHÀO MỪNG BẠN ĐẾN VỚI CỔNG ĐĂNG KÍ CỦA KHÁCH HÀNG ++++++++++++++");
            Console.Write("\tHỌ VÀ TÊN:\t");
			string name = Console.ReadLine();
			while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("VUI LÒNG NHẬP HỌ VÀ TÊN: ");
				name = Console.ReadLine();
			}			
            Console.Write("\tEMAIL :\t");
            string email = Console.ReadLine();
            while (IsUniqueEmail(email) || !IsValidEmail(email))
            {
                Console.WriteLine("\tĐỊA CHỈ EMAIL ĐÃ TỒN TẠI HOẶC KHÔNG HỢP LỆ");
                Console.Write("\tEMAIL :\t");
                email = Console.ReadLine();
            }
            Console.Write("\tMẬT KHẨU:\t");
            string password = Console.ReadLine();
			while (string.IsNullOrWhiteSpace(password))
			{
				Console.Write("VUI LÒNG NHẬP MẬT KHẨU: ");
				password = Console.ReadLine();
			}
			Console.Write("\tSỐ ĐIỆN THOẠI:\t");
            string phone = Console.ReadLine();
			while (string.IsNullOrWhiteSpace(phone))
			{
				Console.Write("VUI LÒNG NHẬP SỐ ĐIỆN THOẠI: ");
				phone = Console.ReadLine();
			}
			Console.Write("\tĐỊA CHỈ:\t");
            string address = Console.ReadLine();
			while (string.IsNullOrWhiteSpace(address))
			{
				Console.Write("VUI LÒNG NHẬP ĐỊA CHỈ: ");
				address = Console.ReadLine();
			}
			Console.Write("\tNGÀY THÁNG NĂM SINH:\t");
            DateTime birth;
			DateTime currentDate = DateTime.Now;			

			while (!DateTime.TryParseExact(Console.ReadLine(), "d/M/yyyy", null, System.Globalization.DateTimeStyles.None, out birth) || birth > currentDate)
			{				
				Console.Write("\tVUI LÒNG NHẬP NGÀY SINH HỢP LỆ: \t");
				
			}
			//true là dùng để ghi tiếp theo vào file .txt, StreamWriter(filePath): là dùng để ghi đè lên file cũ 
			using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"1;{userID};{name};{email};{password};{phone};{address};{birth.ToString("dd/MM/yyyy")}");
            }
        }
        //Hàm chỉnh sửa thông tin người dùng
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

                    Console.Write("\tHỌ VÀ TÊN: \t");
                    string name = Console.ReadLine();
                    Console.Write("\tEMAIL: \t");
                    string email = Console.ReadLine();
                    while (email != "" && (IsUniqueEmail(email) || !IsValidEmail(email)))
                    {
                        Console.WriteLine("\tĐỊA CHỈ EMAIL ĐÃ TỒN TẠI HOẶC KHÔNG HỢP LỆ");
                        Console.Write("\tEMAIL: \t");
                        email = Console.ReadLine();
                    }
                    Console.Write("\tSỐ ĐIỆN THOẠI: \t");
                    string phone = Console.ReadLine();
                    Console.Write("\tĐỊA CHỈ: \t");
                    string address = Console.ReadLine();
                    Console.Write("\tNGÀY THÁNG NĂM SINH: \t");
                    string input = Console.ReadLine();
                    DateTime birth = DateTime.Now;
					DateTime currentDate = DateTime.Now;
					while (true)
                    {
                        if (string.IsNullOrWhiteSpace(input))
                        {
                            // Người dùng không nhập gì hoặc chỉ nhập khoảng trắng, bỏ qua và thoát khỏi vòng lặp.
                            break;
                        }

                        if (DateTime.TryParseExact(input, "d/M/yyyy", null, System.Globalization.DateTimeStyles.None, out birth)|| DateTime.Parse(input) > currentDate)
                        {
                            // Ngày sinh hợp lệ, thoát khỏi vòng lặp.
                            break;
                        }
                        else
                        {
                            Console.Write("\tVUI LÒNG NHẬP NGÀY SINH ĐÚNG HỢP LỆ: \t");
                            input = Console.ReadLine();
                        }
                    }


                    //data0: userID; data1: name; data2: email; data3: pass; data4: phone; data5: address; data6: age
                    // Ghi vô file
                    if (name != "")
                    {
                        data[2] = name;

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
                    if (input != "")
                    {
                        data[7] = birth.ToString("dd/MM/yyyy");

                    }

                    line[i] = string.Join(";", data);
                }
            }
            if (!isFound)
            {
                Console.WriteLine($"{new string(' ', 10)}Không tìm thấy Khách hàng với ID {ID}!!!");
            }
            else
            {
                File.WriteAllLines(filePath, line);
                Console.WriteLine("Cập nhật thông tin thành công!");
            }
        }
        //Hàm xóa tài khoản người dùng
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
                Console.WriteLine($"{new string(' ', 10)}Không tìm thấy Khách hàng với ID {ID}!!!"); // FIX
            }
        }
        //Hàm tìm kiếm người dùng
        public void SearchUser(string ID, string name, string address, string phone, string email)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            //fix code
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");
            string filePath = Path.Combine(datatxt, "Customer.txt");

            string[] Customers = File.ReadAllLines(filePath);

            bool isFound = false;
            DisplayHeader();
            int stt=1;
            for (int i = 0; i < Customers.Length; i++)
            {
                string[] data = Customers[i].Split(';');

                // Check for empty or whitespace values before applying the search criteria

                if (!string.IsNullOrWhiteSpace(ID) && ID.Equals(data[1]) && data[0] == "1")
                {
                    isFound = true;
                    Console.WriteLine($"{new string(' ', 10)}| {stt,-5} | {data[1],-13} | {data[2],-32} | {data[7],-7} | {data[3],-27} | {data[6],-30} | {data[5],-14} |");
                    Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
                    stt++;
                }
                else if (string.IsNullOrWhiteSpace(ID) && data[0] == "1")
                {
                    if (string.IsNullOrWhiteSpace(email) && phone.Equals(data[5])) 
                    { 
                    isFound = true;
                    Console.WriteLine($"{new string(' ', 10)}| {stt,-5} | {data[1],-13} | {data[2],-32} | {data[7],-7} | {data[3],-27} | {data[6],-30} | {data[5],-14} |");
                    Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
                    stt++;
                    }
                    else if (string.IsNullOrWhiteSpace(phone) && email.Equals(data[3]))
                    {
                    isFound = true;
                    Console.WriteLine($"{new string(' ', 10)}| {stt,-5} | {data[1],-13} | {data[2],-32} | {data[7],-7} | {data[3],-27} | {data[6],-30} | {data[5],-14} |");
                    Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
                    stt++;
                    }
					else if (phone.Equals(data[5]) && email.Equals(data[3]))
					{
						isFound = true;
						Console.WriteLine($"{new string(' ', 10)}| {stt,-5} | {data[1],-13} | {data[2],-32} | {data[7],-7} | {data[3],-27} | {data[6],-30} | {data[5],-14} |");
						Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
						stt++;
					}
                    else if (string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(email))
                    {
						if (string.IsNullOrWhiteSpace(address) && RemoveDiacritics(data[2]).Contains(RemoveDiacritics(name)))
						{
							isFound = true;
							Console.WriteLine($"{new string(' ', 10)}| {stt,-5} | {data[1],-13} | {data[2],-32} | {data[7],-7} | {data[3],-27} | {data[6],-30} | {data[5],-14} |");
							Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
							stt++;
						}
						else if (string.IsNullOrWhiteSpace(name) && RemoveDiacritics(data[6]).Contains(RemoveDiacritics(address)))
						{
							isFound = true;
							Console.WriteLine($"{new string(' ', 10)}| {stt,-5} | {data[1],-13} | {data[2],-32} | {data[7],-7} | {data[3],-27} | {data[6],-30} | {data[5],-14} |");
							Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
							stt++;
						}
						else if (RemoveDiacritics(data[2]).Contains(RemoveDiacritics(name)) && RemoveDiacritics(data[6]).Contains(RemoveDiacritics(address)))
						{
							isFound = true;
							Console.WriteLine($"{new string(' ', 10)}| {stt,-5} | {data[1],-13} | {data[2],-32} | {data[7],-7} | {data[3],-27} | {data[6],-30} | {data[5],-14} |");
							Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
							stt++;
						}
					}
				}
            }
            if (!isFound)
            {
                Console.WriteLine($"{new string(' ', 10)}KHÔNG TÌM THẤY KHÁCH HÀNG"); //FIX
            }
        }
        //Hàm bỏ dấu cho chuỗi (dùng cho việc so sánh chuỗi)
        static string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().ToLowerInvariant().Replace(" ", "").Replace("đ", "d");
        }
        //Hàm kiểm tra email có trùng không
        public bool IsUniqueEmail(string emailID)
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
                if (data[3].Equals(emailID))
                {
                    //trùng dữ liệu
                    isUnique = true;
                    break;
                }
            }
            return isUnique;
        }
        //Hàm kiểm tra email có hợp lệ không
        public bool IsValidEmail(string email)
        {
            // Biểu thức chính quy để kiểm tra định dạng email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Sử dụng Regex.IsMatch để kiểm tra tính hợp lệ của email
            return Regex.IsMatch(email, pattern);
        }
        //Hàm hiển thị toàn bộ thông tin khách hàng
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
                Console.WriteLine("CHUYẾN BAY KHÔNG TỒN TẠI KHÁCH HÀNG!");

            }
        }
        //Hàm hiển thị header của bảng
        void DisplayHeader()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine();
            Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
            Console.WriteLine($"{new string(' ', 10)}| STT   | MÃ KHÁCH HÀNG | HỌ VÀ TÊN                        | NĂM SINH   | EMAIL                       | ĐỊA CHỈ                        | SỐ ĐIỆN THOẠI  |");
            Console.WriteLine($"{new string(' ', 10)}+-------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+----------------+");
        }
        //Hàm hiển thị các banner
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
