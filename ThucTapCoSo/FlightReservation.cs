using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Xml.Linq;

namespace ThucTapCoSo
{
    internal class FlightReservation : Generator,IDisplayClass
    {
        public void BookFlight(string userID)
        {
			Console.OutputEncoding = Encoding.Unicode;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePathFl = Path.Combine(datatxt, "FlightScheduler.txt");
            string filePathTicketReceipt = Path.Combine(datatxt, "TicketReceipt.txt");

            string[] flight = File.ReadAllLines(filePathFl);

            bool isFound = false;
            bool checkTicket = false;
            DateTime now = DateTime.Now;
            int numOfTickets;
            string flightToBeBooked;

            if(SearchFlight() == true)
            { 
                Console.Write("\nNhập mã chuyến bay mong muốn để đặt chỗ :\t ");
                flightToBeBooked = Console.ReadLine().ToUpper();
                Console.Write($"Nhập số lượng vé cho chuyến bay {flightToBeBooked} :   ");
                while (!int.TryParse(Console.ReadLine(), out numOfTickets) || numOfTickets > 10 || numOfTickets < 1)
                {
                    Console.Write("LỖI!! Vui lòng nhập số lượng vé hợp lệ (ít hơn 10, nhiều hơn 0): ");
                }

                string ticketType;
                while (true)
                {
                    Console.Write("\nNhập loại vé bạn muốn đặt (1. Business  / 2. Economy ):\t");
                    int choose;

                    if (int.TryParse(Console.ReadLine(), out choose) && (choose == 1 || choose == 2))
                    {
                        ticketType = (choose == 1) ? "BSN" : "ECO";
                        break;
                    }
                    else
                    {
                        Console.Write("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                    }
                }

                for (int i = 0; i < flight.Length; i++)
                {
                    string[] dataFlight = flight[i].Split(';');

                    if (dataFlight[1].Equals(flightToBeBooked) && dataFlight[0] == "1")
                    {
                        isFound = true;

                        for (int count = 1; count <= numOfTickets; count++)
                        {
                            string name, email, phone, address;
                            DateTime birth;

                            if(ticketType == "ECO")
                            {
                                int availableECOSeats = int.Parse(dataFlight[3]);

                                if (availableECOSeats >= numOfTickets)
                                {
                                    checkTicket = true;
                                    Console.WriteLine($"\n\tNHẬP THÔNG TIN CỦA HÀNH KHÁCH THỨ {count}:\t");

                                    Console.Write("\tHỌ VÀ TÊN:\t");
                                    name = Console.ReadLine();
                                    Console.Write("\tEMAIL :\t");
                                    email = Console.ReadLine();
                                    Customer c = new Customer();
                                    while (!c.IsValidEmail(email))
                                    {
                                        Console.WriteLine("ĐỊA CHỈ EMAIL KHÔNG HỢP LỆ");
                                        Console.Write("EMAIL :\t");
                                        email = Console.ReadLine();
                                    }
                                    Console.Write("\tSỐ ĐIỆN THOẠI:\t");
                                    phone = Console.ReadLine();
                                    Console.Write("\tĐỊA CHỈ:\t");
                                    address = Console.ReadLine();
                                    Console.Write("\tNGÀY THÁNG NĂM SINH:\t");
                                    while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out birth))
                                    {
                                        Console.Write("VUI LÒNG NHẬP NGÀY SINH ĐÚNG ĐỊNH DẠNG: \t");
                                    }
                                    using (StreamWriter write = new StreamWriter(filePathTicketReceipt, true))
                                    {
                                        write.WriteLine($"{now};Mã chỗ;{userID};{flightToBeBooked};{ticketType};{name};{birth:dd/MM/yyyy};{email};{phone};{address}");
                                    }
                                    dataFlight[3] = Convert.ToString(availableECOSeats - 1);
                                }
                            }
                            else if( ticketType == "BSN")
                            {
                                int availableBSNSeats = int.Parse(dataFlight[2]);

                                if (availableBSNSeats >= numOfTickets)
                                {
                                    checkTicket = true;
                                    Console.WriteLine($"\n\tNHẬP THÔNG TIN CỦA HÀNH KHÁCH THỨ {count}:\t");

                                    Console.Write("\tHỌ VÀ TÊN:\t");
                                    name = Console.ReadLine();
                                    Console.Write("\tEMAIL :\t");
                                    email = Console.ReadLine();
                                    Customer c = new Customer();
                                    while (!c.IsValidEmail(email))
                                    {
                                        Console.WriteLine("ĐỊA CHỈ EMAIL ĐÃ KHÔNG HỢP LỆ");
                                        Console.Write("EMAIL :\t");
                                        email = Console.ReadLine();
                                    }
                                    Console.Write("\tSỐ ĐIỆN THOẠI:\t");
                                    phone = Console.ReadLine();
                                    Console.Write("\tĐỊA CHỈ:\t");
                                    address = Console.ReadLine();
                                    Console.Write("\tNGÀY THÁNG NĂM SINH:\t");
                                    while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out birth))
                                    {
                                        Console.Write("VUI LÒNG NHẬP NGÀY SINH ĐÚNG ĐỊNH DẠNG: \t");
                                    }
                                    
                                    using (StreamWriter write = new StreamWriter(filePathTicketReceipt, true))
                                    {
                                        write.WriteLine($"{now};Mã chỗ;{userID};{flightToBeBooked};{ticketType};{name};{birth:dd/MM/yyyy};{email};{phone};{address}");
                                    }

                                    dataFlight[2] = Convert.ToString(availableBSNSeats - 1);
                                }
                            }
                            flight[i] = string.Join(";", dataFlight);
                        }
                        break;
                    }
                }
                if (!isFound)
                {
                    Console.WriteLine($"Số hiệu không hợp lệ...! Không tìm thấy chuyến bay với ID \"{flightToBeBooked}\"...");
                }
                if (checkTicket)
                {
                    File.WriteAllLines(filePathFl, flight);
                    Console.WriteLine($"\n {new string(' ', 30)} Bạn đã đặt {numOfTickets} vé {ticketType} cho Chuyến bay \"{flightToBeBooked.ToUpper()}\"...");
                }
                else
                {
                    Console.WriteLine($"Không đủ chỗ ngồi hạng {ticketType} cho Chuyến bay \"{flightToBeBooked.ToUpper()}\"...");
                }
            }
        }
        public void CancelFlight(string userID)
        {
			Console.OutputEncoding = Encoding.Unicode;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePathFl = Path.Combine(datatxt, "FlightScheduler.txt");
            string filePathTR = Path.Combine(datatxt, "TicketReceipt.txt");
            string filePathC = Path.Combine(datatxt, "Customer.txt");

            string[] flight = File.ReadAllLines(filePathFl);
            string[] customer = File.ReadAllLines(filePathC);
            List<string> TicketReceipt = File.ReadAllLines(filePathTR).ToList();

            string flightNum;
            bool isFound = false;

            Console.WriteLine($"{new string(' ', 30)}++++++++++++++ Đây là danh sách tất cả các chuyến bay bạn đã đăng ký ++++++++++++++");
            DisplayFlightsRegisteredByOneUser(userID);
            Console.WriteLine("Nhập sô hiệu của chuyến bay bạn muốn hủy:");
            flightNum = Console.ReadLine().ToUpper();

            string ticketType;
            while (true)
            {
                Console.WriteLine("\nNhập loại vé bạn muốn hủy (1. BSN / 2. ECO):\t");
                int choose;

                if (int.TryParse(Console.ReadLine(), out choose) && (choose == 1 || choose == 2))
                {
                    ticketType = (choose == 1) ? "BSN" : "ECO";
                    break;
                }
                else
                {
                    Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                }
            }

            Console.WriteLine("Nhập số lượng vé muốn hủy:");
            int numOfTickets;
			while (!int.TryParse(Console.ReadLine(), out numOfTickets))
			{
				Console.Write("Vui lòng nhập số vé hợp lệ:  ");
			}
			//FightHasCustomers
            for(int i=0; i<TicketReceipt.Count; i++)
            {
                string[] dataTR = TicketReceipt[i].Split(';');
                for (int j = 0; j < customer.Length; j++)
                {
                    string[] dataC = customer[j].Split(';');

                    for (int h = 0; h < flight.Length; h++)
                    {
                        string[] dataF = flight[h].Split(';');

                        if (flightNum.Equals(dataTR[1]) && flightNum.Equals(dataF[1]) && userID.Equals(dataTR[2]) && userID.Equals(dataC[1]) && dataC[0] == "1" && dataF[0] == "1")
                        {
                            int numOfTicketsForFlight = int.Parse(dataTR[3]);
                            // BSN
                            if ( ticketType == "BSN" && dataTR[4] == "BSN" )
                            {
                                isFound = true;

                                while (numOfTickets > numOfTicketsForFlight)
                                {
                                    Console.Write($"LỖI!!! Số vé không thể lớn hơn {numOfTicketsForFlight} cho chuyến bay này. Vui lòng nhập lại số lượng vé:");
                                    while (!int.TryParse(Console.ReadLine(), out numOfTickets))
                                    {
                                        Console.Write("Vui lòng nhập số vé hợp lệ:  ");
                                    }
                                }
                                int ticketsToBeReturned;

                                if (numOfTicketsForFlight == numOfTickets)
                                {
                                    ticketsToBeReturned = int.Parse(dataF[2]) + numOfTicketsForFlight;
                                    dataF[2] = Convert.ToString(ticketsToBeReturned);
                                    //nếu số vé cần hủy bằng với số vé hiện có thì khách hàng đó ra khỏi file
                                    TicketReceipt.RemoveAt(i);
                                    Console.Write($"Đã hủy hết vé của chuyến bay {flightNum} thành công.");
                                }
                                else
                                {
                                    ticketsToBeReturned = numOfTickets + int.Parse(dataF[2]);
                                    //cập nhật số vé ở TR
                                    dataTR[3] = Convert.ToString(numOfTicketsForFlight - numOfTickets);
                                    TicketReceipt[i] = string.Join(";", dataTR);
                                    Console.Write($"Đã hủy {numOfTickets} vé {ticketType} của chuyến bay {flightNum} thành công.");
                                }
                                dataF[2] = Convert.ToString(ticketsToBeReturned);
                            }
                            //ECO
                            else if ((ticketType == "ECO" && dataTR[4] == "ECO"))
                            {
                                isFound = true;

                                while (numOfTickets > numOfTicketsForFlight)
                                {
                                    Console.Write($"LỖI!!! Số vé không thể lớn hơn {numOfTicketsForFlight} cho chuyến bay này. Vui lòng nhập lại số lượng vé:");
                                    while (!int.TryParse(Console.ReadLine(), out numOfTickets))
                                    {
                                        Console.Write("Vui lòng nhập số vé hợp lệ:  ");
                                    }
                                }
                                int ticketsToBeReturned;

                                if (numOfTicketsForFlight == numOfTickets)
                                {
                                    ticketsToBeReturned = int.Parse(dataF[3]) + numOfTicketsForFlight;
                                    dataF[3] = Convert.ToString(ticketsToBeReturned);
                                    //nếu số vé cần hủy bằng với số vé hiện có thì khách hàng đó ra khỏi file
                                    TicketReceipt.RemoveAt(i);
                                    Console.Write($"Đã hủy hết vé của chuyến bay {flightNum} thành công.");
                                }
                                else
                                {
                                    ticketsToBeReturned = numOfTickets + int.Parse(dataF[3]);
                                    //cập nhật số vé ở TR
                                    dataTR[3] = Convert.ToString(numOfTicketsForFlight - numOfTickets);
                                    TicketReceipt[i] = string.Join(";", dataTR);
                                    Console.Write($"Đã hủy {numOfTickets} vé {ticketType} của chuyến bay {flightNum} thành công.");
                                }
                                dataF[3] = Convert.ToString(ticketsToBeReturned);
                            }
                            //cập nhật số vé có trong FlightScheduler.txt
                            flight[h] = string.Join(";", dataF);
                            break;
                        }
                    }
                }
            }
            if (!isFound)
            {
                Console.WriteLine($"LỖI!!! Không thể tìm thấy chuyến bay với ID \"{flightNum.ToUpper()}\".....");
            }
            else
            {
                File.WriteAllLines(filePathTR, TicketReceipt);
                File.WriteAllLines(filePathFl, flight);
            }
            
        }
        public bool SearchFlight()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePath = Path.Combine(datatxt, "FlightScheduler.txt");
            List<string> lines = File.ReadAllLines(filePath).ToList();

            int columns = 3;
            for (int i = 0; i < destinations.Length; i++)
            {
                Console.Write($"\t{i,-3} {destinations[i][0],-20}");

                if ((i + 1) % columns == 0)
                {
                    Console.WriteLine(); // Xuống dòng sau mỗi số cột
                }
            }

            Console.Write("\n\n\tNHẬP ĐIỂM ĐI   :  ");
            string from = Console.ReadLine();
            string normalFrom = RemoveDiacritics(from);
            Console.Write("\tNHẬP ĐIỂM ĐẾN  :  ");
            string to = Console.ReadLine();
            string normalTo = RemoveDiacritics(to);
            Console.Write("\tNGÀY KHỞI HÀNH : ");
            DateTime date;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date))
            {
                Console.Write("\tVUI LÒNG NHẬP NGÀY KHỞI HÀNH ĐÚNG ĐỊNH DẠNG: \t");
            }
            Console.WriteLine();
            Console.Write("+------+---------------------------+-------------+------------------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+----------+\n");
            Console.Write("| STT  | THỜI GIAN CẤT CÁNH        | MÃ CHUYẾN   | SỐ GHẾ TRỐNG                 | KHỞI HÀNH             | ĐIẾM ĐẾN               | THỜI GIAN HẠ CÁNH         |THỜI GIAN BAY|  CỔNG  | QUÃNG ĐƯỜNG(MILES/KMS) | GIÁ VÉ $ |\n");
            Console.Write("+------+---------------------------+-------------+------------------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+----------+\n");

            bool isFound = false;
            int stt = 1;
            for (int i = 0; i < lines.Count; i++)
            {
                string[] data = lines[i].Split(';');
                string normaldataFrom = RemoveDiacritics(data[5]);
                string normaldataTo = RemoveDiacritics(data[6]);

                // Chuyển đổi chuỗi ngày về đối tượng DateTime
                DateTime dateTime = DateTime.ParseExact(data[4], "ddd, dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                if (normalFrom.Equals(normaldataFrom, StringComparison.OrdinalIgnoreCase) && normalTo.Equals(normaldataTo, StringComparison.OrdinalIgnoreCase) && data[0] == "1")
                {
                    isFound = true;
                    Flight fl = new Flight();
                    Console.WriteLine($"| {stt,-4} | {data[4],-25} | {data[1],-11} | BSN: {data[2],-3} / ECO: {data[3],-3}          | {data[5],-21} | {data[6],-22} | {fl.FetchArrivalTime(data[4], data[7]),-25} | {data[7],6}  Hrs | {data[8],-6} | {data[9],-9} / {data[10],-10} | {fl.CalculatePrice(data[9]),-8} |");
                    Console.Write("+------+---------------------------+-------------+------------------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+----------+\n");
                    stt++;
                }
            }
            if(!isFound)
            {
                Console.WriteLine($"Không tìm thấy chuyến bay.");
                return false;
            }
            return true;

        }
        //bỏ dấu
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
        public string ConvertToDate(string dateString)
        {
            dateString.Trim();

            // Chuyển đổi chuỗi ngày thành kiểu DateTime
            if (DateTime.TryParse(dateString, out DateTime date))
            {
                return date.ToString("dd/MM/yyyy");
            }
            else
            {
                return "Không thể chuyển đổi";
            }
        }
        string FlightStatus(string fightNo)
        {
			Console.OutputEncoding = Encoding.Unicode;
            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string fileFlight = Path.Combine(datatxt, "FlightScheduler.txt");
            string[] Flight = File.ReadAllLines(fileFlight);

            bool isFlightAvailable = false;
            for(int i=0; i<Flight.Length; i++)
            {
                string[] dataFlight = Flight[i].Split(';');
                if (dataFlight[1].Equals(fightNo) && dataFlight[0]=="1")
                {
                    isFlightAvailable = true;
                    break;
                }
            }
            return isFlightAvailable ? "Theo Lịch Trình" : "   Hủy Bỏ      ";
        }
        public float TotalPrice(string numOfTicket, float price)
        {
            float tolal = float.Parse(numOfTicket) * price;
            return tolal; 
        }
        public void DisplayFlightsRegisteredByOneUser(string userID)
        {
			Console.OutputEncoding = Encoding.Unicode;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePathCustomer = Path.Combine(datatxt, "Customer.txt");
            string filePathFlight = Path.Combine(datatxt, "FlightScheduler.txt");
            string filePathTR = Path.Combine(datatxt, "TicketReceipt.txt");

            string[] Customer = File.ReadAllLines(filePathCustomer);
            string[] flight = File.ReadAllLines(filePathFlight);
            string[] TicketReceipt = File.ReadAllLines(filePathTR);

            Dictionary<string, List<string[]>> user_receipt = new Dictionary<string, List<string[]>>();

            foreach (string line in TicketReceipt)
            {
                string[] data = line.Split(';');
                string key = data[3]; // Sử dụng tên chuyến bay làm key

                if (!user_receipt.ContainsKey(key))
                {
                    user_receipt[key] = new List<string[]>();
                }

                user_receipt[key].Add(data);
            }

            int stt = 0;
            foreach (var idFL in user_receipt)
            {
                string id = idFL.Key;
                List<string[]> customerDataList = idFL.Value;
                bool shouldDisplayHeader = true;

                foreach (var dataTR in customerDataList)
                {
                    for (int j = 0; j < Customer.Length; j++)
                    {
                        string[] dataCustomer = Customer[j].Split(';');
                        for (int i = 0; i < flight.Length; i++)
                        {
                            string[] dataFlight = flight[i].Split(';');

                            if (userID.Equals(dataCustomer[1]) && dataTR[2].Equals(dataCustomer[1]) && id.Equals(dataFlight[1]) && dataFlight[0] == "1")
                            {
                                Flight fl = new Flight();
                                if (shouldDisplayHeader)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine($" Chuyến bay đi từ {dataFlight[5]} đến {dataFlight[6]} vào ngày {ConvertToDate(dataFlight[4])}");
                                    Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                                    Console.WriteLine($"{new string(' ', 10)}| STT         | Mã khách hàng | Tên khách hàng                   | Ngày sinh  | Email                       | Địa chỉ                        | Số điện thoại           | Số vé đã đặt | Tổng tiền vé $ |");
                                    Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                                    shouldDisplayHeader = false; // Đặt flag để không hiển thị header nữa
                                }
                                // In thông tin của mỗi khách hàng trong nhóm
                                Console.WriteLine($"{new string(' ', 10)}| {stt + 1,-11} | {dataCustomer[1],-13} | {dataCustomer[2],-32} | {dataCustomer[7],-10} | {dataCustomer[3],-27} | {dataCustomer[6],-30} | {dataCustomer[5],-23} | Chưa làm | Chưa làm |");
                                Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                                stt++;
                            }
                        }
                    }
                }
            }
            if (stt == 0)
            {
                Console.WriteLine($"Không có chuyến bay được đăng kí bởi {userID}");
            }
        }

        public void DisplayRegisteredUsersForAllFlight()
        {
            Console.WriteLine();
            Console.WriteLine($"\n{new string('+', 30)} Hiển thị tất cả chuyến bay đã được đăng ký\" {new string('+', 30)}\n");

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePathTR = Path.Combine(datatxt, "TicketReceipt.txt");
            string filePathCustomer = Path.Combine(datatxt, "Customer.txt");
            string filePathFlight = Path.Combine(datatxt, "FlightScheduler.txt");

            string[] flight = File.ReadAllLines(filePathFlight);
            string[] TicketReceipt = File.ReadAllLines(filePathTR);
            string[] Customer = File.ReadAllLines(filePathCustomer);

            // Tạo Dictionary để nhóm theo tên chuyến bay
            Dictionary<string, List<string[]>> flightGroups = new Dictionary<string, List<string[]>>();

            foreach (string line in TicketReceipt)
            {
                string[] data = line.Split(';');
                string flightName = data[1]; // Sử dụng tên chuyến bay làm key

                if (!flightGroups.ContainsKey(flightName))
                {
                    flightGroups[flightName] = new List<string[]>();
                }

                flightGroups[flightName].Add(data);
            }
            //sắp xếp lại theo nhóm ID chuyến bay
            foreach (var flightGroup in flightGroups)
            {
                string flightName = flightGroup.Key;
                List<string[]> customerDataList = flightGroup.Value;
                bool shouldDisplayHeader = true;
                int stt = 0;

                // In thông tin từng nhóm
                foreach (var dataTR in customerDataList)
                {
                    for(int j=0; j<Customer.Length; j++)
                    {
                        string[] dataCustomer = Customer[j].Split(';');
                        if ( dataTR[2].Equals(dataCustomer[1]))
                        {
                            for (int i = 0; i < flight.Length; i++)
                            {
                                string[] dataFlight = flight[i].Split(';');

                                if (dataTR[1].Equals(dataFlight[1]) && dataFlight[0] == "1" && dataCustomer[0] == "1")
                                {
                                    Flight fl = new Flight();
                                    if (shouldDisplayHeader)
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine($" Mã chuyến bay: {flightName}");
                                        Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                                        Console.WriteLine($"{new string(' ', 10)}| STT         | Mã khách hàng | Tên khách hàng                   | Ngày sinh  | Email                       | Địa chỉ                        | Số điện thoại           | Số vé đã đặt | Tổng tiền vé $ |");
                                        Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                                        shouldDisplayHeader = false; // Đặt flag để không hiển thị header nữa
                                    }
                                    // In thông tin của mỗi khách hàng trong nhóm
                                    Console.WriteLine($"{new string(' ', 10)}| {stt + 1,-11} | {dataCustomer[1],-13} | {dataCustomer[2],-32} | {dataCustomer[7],-10} | {dataCustomer[3],-27} | {dataCustomer[6],-30} | {dataCustomer[5],-23} | {dataTR[3]} {dataTR[4],-10} | {TotalPrice(dataTR[3], fl.CalculatePrice(dataFlight[9])),-14} |");
                                    Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                                    stt++;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void DisplayRegisteredUsersForASpecificFlight(string flightNum)
        {
            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePathTR = Path.Combine(datatxt, "TicketReceipt.txt");
            string filePathFlight = Path.Combine(datatxt, "FlightScheduler.txt");
            string filePathCustomer = Path.Combine(datatxt, "Customer.txt");

            string[] Customer = File.ReadAllLines(filePathCustomer);
            string[] Flight = File.ReadAllLines(filePathFlight);
            string[] TicketReceipt = File.ReadAllLines(filePathTR);

            for (int j = 0; j < Flight.Length; j++)
            {
                string[] dataF = Flight[j].Split(';');
                if (dataF[0] == "1" && flightNum.Equals(dataF[1]))
                {
                    Console.WriteLine();
                    Console.WriteLine($"\n{new string('+', 30)} Hiển thị Khách hàng đã đăng ký cho Chuyến bay số \"{flightNum,-6}\" {new string('+', 30)}\n");
                    Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                    Console.WriteLine($"{new string(' ', 10)}| Mã chuyến bay  |Mã khách hàng| Tên khách hàng                   | Ngày sinh  | Email  \t\t       | Địa chỉ\t\t        | Số điện thoại\t          | Số vé đã đặt | Tổng tiền vé $ |");
                    Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                    break;
                }
            }
            bool isFound = false;
            //
            for (int i = 0; i < TicketReceipt.Length; i++)
            {
                string[] dataTR = TicketReceipt[i].Split(';');
                //
                for (int j = 0; j < Flight.Length; j++)
                {
                    string[] dataF = Flight[j].Split(';');

                    if (dataF[1].Equals(flightNum) && dataF[0] == "1" && flightNum.Equals(dataTR[1]))
                    {
                        isFound = true;
                        //
                        for (int k = 0; k < Customer.Length; k++)
                        {
                            string[] dataCustomer = Customer[k].Split(';');

                            if (dataTR[2].Equals(dataCustomer[1]) && dataCustomer[0] == "1")
                            {
                                Flight fl = new Flight();
                                Console.WriteLine($"{new string(' ', 10)}| {flightNum,-14} | {dataCustomer[1],-11} | {dataCustomer[2],-32} | {dataCustomer[7],-7} | {dataCustomer[3],-27} | {dataCustomer[6],-30} | {dataCustomer[5],-23} | {dataTR[3]} {dataTR[4],-10} | {TotalPrice(dataTR[3], fl.CalculatePrice(dataF[9])),-14} |");
                                Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                            }
                        }
                    }
                }
            }
            if (!isFound)
            {
                Console.WriteLine($"\nKhông tìm thấy chuyến bay {flightNum}");
            }
        }

        public void DisplayArtWork(int option)
        {
			Console.OutputEncoding = Encoding.Unicode;
			string artWork;

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
