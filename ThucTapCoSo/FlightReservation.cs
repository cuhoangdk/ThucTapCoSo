using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Xml.Linq;

namespace ThucTapCoSo
{
    internal class FlightReservation : Generator,IDisplayClass
    {
        public string TicketID(string plane, string seatsEmpty, string ticketType)
        {
            string[][] planeType = Flight.PlaneTypes;
            int id;

            for( int i = 0; i<planeType.Length; i++)
            {
                if (planeType[i][0].Equals(plane) && ticketType == "BSN")
                {
                    id = int.Parse(planeType[i][1]) - int.Parse(seatsEmpty) + 1;
                    return $"{ticketType}-{id.ToString().PadLeft(3, '0')}";
                }
                else if(planeType[i][0].Equals(plane) && ticketType == "ECO")
                {
                    id = int.Parse(planeType[i][2]) - int.Parse(seatsEmpty) + 1;
                    return $"{ticketType}-{id.ToString().PadLeft(3, '0')}";
                }
            }
            return "";
        }
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
                string ticketType;
                while (true)
                {
                    Console.Write("\nNhập loại vé bạn muốn đặt (1. Business  / 2. Economy ):\t");

                    if (int.TryParse(Console.ReadLine(), out int choose) && (choose == 1 || choose == 2))
                    {
                        ticketType = (choose == 1) ? "BSN" : "ECO";
                        break;
                    }
                    else
                    {
                        Console.Write("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                    }
                }
                Console.Write($"Nhập số lượng vé cho chuyến bay {flightToBeBooked} :   ");
                while (!int.TryParse(Console.ReadLine(), out numOfTickets) || numOfTickets > 10 || numOfTickets < 1)
                {
                    Console.Write("LỖI!! Vui lòng nhập số lượng vé hợp lệ (ít hơn 10, nhiều hơn 0): ");
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
                                    string tiketID = TicketID(dataFlight[11], dataFlight[3], ticketType);
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
                                        write.WriteLine($"{now};{tiketID};{userID};{flightToBeBooked};{ticketType};{name};{birth:dd/MM/yyyy};{email};{phone};{address}");
                                    }
                                    dataFlight[3] = Convert.ToString(availableECOSeats - 1);
                                }
                                else
                                {
                                    if(availableECOSeats == 0)
                                    {
                                        Console.WriteLine($"\tĐã hết vé {ticketType} cho chuyến bay {flightToBeBooked}.");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"\tKhông đủ số lượng vé {ticketType}. Số lượng vé còn lại {availableECOSeats}.");
                                        break;
                                    }
                                }
                            }
                            else if( ticketType == "BSN")
                            {
                                int availableBSNSeats = int.Parse(dataFlight[2]);

                                if (availableBSNSeats >= numOfTickets)
                                {
                                    string tiketID = TicketID(dataFlight[11], dataFlight[2], ticketType);
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
                                        write.WriteLine($"{now};{tiketID};{userID};{flightToBeBooked};{ticketType};{name};{birth:dd/MM/yyyy};{email};{phone};{address}");
                                    }

                                    dataFlight[2] = Convert.ToString(availableBSNSeats - 1);
                                }
                                else
                                {
                                    if (availableBSNSeats == 0)
                                    {
                                        Console.WriteLine($"\tĐã hết vé {ticketType} cho chuyến bay {flightToBeBooked}.");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"\tKhông đủ số lượng vé {ticketType}. Số lượng vé còn lại {availableBSNSeats}.");
                                        break;
                                    }
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
            }
        }
        public void CancelFlight(string userID)
        {
			Console.OutputEncoding = Encoding.Unicode;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");

            string filePathFl = Path.Combine(datatxt, "FlightScheduler.txt");
            string filePathTR = Path.Combine(datatxt, "TicketReceipt.txt");
            string[] flight = File.ReadAllLines(filePathFl);
            List<string> TicketReceipt = File.ReadAllLines(filePathTR).ToList();

            Console.WriteLine($"{new string(' ', 30)}++++++++++++++ Đây là danh sách tất cả các chuyến bay bạn đã đăng ký ++++++++++++++");
            DisplayFlightsRegisteredByOneUser(userID);

            Console.WriteLine("Nhập sô hiệu của chuyến bay bạn muốn hủy:");
            string flightNum = Console.ReadLine().ToUpper();

            DisplayTicketNumberBookedByOneCustomer(userID,flightNum);

            bool isFound = false;
            bool ticketCheck = false;
            int countBSN = 0, countECO = 0;

            Console.Write("Nhập mã vé muốn hủy (chọn n/N để thoát):\t");
            string ticketIdCancel = Console.ReadLine();

            while (ticketIdCancel.ToLower() != "n")
            {
                for (int j = 0; j < flight.Length; j++)
                {
                    string[] dataFlight = flight[j].Split(';');

                    if (dataFlight[1].Equals(flightNum) && dataFlight[0] == "1")
                    {
                        isFound = true;

                        for (int i = 0; i < TicketReceipt.Count; i++)
                        {
                            string[] dataTR = TicketReceipt[i].Split(';');

                            if (ticketIdCancel.Equals(dataTR[1]) && userID.Equals(dataTR[2]) && flightNum.Equals(dataTR[3]))
                            {
                                ticketCheck = true;
                                string ticketType = dataTR[4];
                                int ticketsToBeReturned = 1;

                                if (ticketType == "BSN")
                                {
                                    dataFlight[2] = Convert.ToString(int.Parse(dataFlight[2]) + ticketsToBeReturned);
                                    //xóa khách hàng đó ra khỏi file
                                    TicketReceipt.RemoveAt(i);
                                    countBSN++;
                                    Console.WriteLine($"\tBạn đã hủy vé {ticketIdCancel}");
                                    File.WriteAllLines(filePathTR, TicketReceipt);
                                    break;
                                }
                                else if (ticketType == "ECO")
                                {
                                    dataFlight[3] = Convert.ToString(int.Parse(dataFlight[3]) + ticketsToBeReturned);
                                    //xóa khách hàng đó ra khỏi file
                                    TicketReceipt.RemoveAt(i);
                                    countECO++;
                                    Console.WriteLine($"Bạn đã hủy vé {ticketIdCancel}");
                                    File.WriteAllLines(filePathTR, TicketReceipt);
                                    break;
                                }
                            }
                        }
                        //cập nhật số vé có trong FlightScheduler.txt
                        flight[j] = string.Join(";", dataFlight);
                        File.WriteAllLines(filePathFl, flight);
                        break;
                    }
                }
                DisplayTicketNumberBookedByOneCustomer(userID, flightNum);
                Console.Write("Nhập mã vé muốn hủy (chọn n/N để thoát):\t");
                ticketIdCancel = Console.ReadLine();
            }
            if (!isFound)
            {
                Console.WriteLine($"\nLỖI!!! Không thể tìm thấy chuyến bay với ID \"{flightNum.ToUpper()}\".....");
            }
            else
            {
                Console.WriteLine($"\nBạn đã hủy {countBSN} vé Business, {countECO} vé Economy trong chuyến bay {flightNum}");
            }
            if (!ticketCheck)
            {
                Console.WriteLine($"\nKhông tìm thấy mã vé {ticketIdCancel}");
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
            Flight fl = new Flight();

            //test
            fl.DisplayFlightSchedule();

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
            Console.Write("+------+---------------------------+-------------+------------------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+----------------------+\n");
            Console.Write("| STT  | THỜI GIAN CẤT CÁNH        | MÃ CHUYẾN   | SỐ GHẾ TRỐNG                 | KHỞI HÀNH             | ĐIẾM ĐẾN               | THỜI GIAN HẠ CÁNH         |THỜI GIAN BAY|  CỔNG  | QUÃNG ĐƯỜNG(MILES/KMS) | GIÁ VÉ $ (BSN / ECO) |\n");
            Console.Write("+------+---------------------------+-------------+------------------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+----------------------+\n");

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
                    Console.WriteLine($"| {stt,-4} | {data[4],-25} | {data[1],-11} | BSN: {data[2],-3} / ECO: {data[3],-3}          | {data[5],-21} | {data[6],-22} | {fl.FetchArrivalTime(data[4], data[7]),-25} | {data[7],6}  Hrs | {data[8],-6} | {data[9],-9} / {data[10],-10} | {fl.CalculatePrice("BSN", data[9]),-8} /  {fl.CalculatePrice("ECO", data[9]),-8} |");
                    Console.Write("+------+---------------------------+-------------+------------------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+----------------------+\n");
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
                return date.ToString("dd/MM/yyyy HH:mm");
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
        public float TotalPrice(int numOfTicket, float price)
        {
            float tolal = (float)Math.Round(price * numOfTicket ,2);
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

            Dictionary<string, int> user_receipt = new Dictionary<string, int>();

            foreach (string line in TicketReceipt)
            {
                string[] data = line.Split(';');
                string key = $"{data[2]}_{data[3]}_{data[4]}";  //  userID_flightNum_ticketType

                if (user_receipt.ContainsKey(key))
                {
                    user_receipt[key]++;
                }
                else
                {
                    user_receipt[key] = 1;
                }
            }

            bool isFound = false;

            foreach (var key in user_receipt)
            {
                string[] dataTR = key.Key.Split('_');   //  userID_flightNum_ticketType
                bool shouldDisplayHeader = true;

                for (int j = 0; j < Customer.Length; j++)
                {
                    string[] dataCustomer = Customer[j].Split(';');
                    if (userID.Equals(dataTR[0]) && dataTR[0].Equals(dataCustomer[1]))
                    {
                        for (int i = 0; i < flight.Length; i++)
                        {
                            string[] dataFlight = flight[i].Split(';');
                            if (dataTR[1].Equals(dataFlight[1]) && dataFlight[0] == "1")
                            {
                                isFound = true;
                                Flight fl = new Flight();
                                if (shouldDisplayHeader)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine($" Chuyến bay {dataTR[1]} khởi hành từ {dataFlight[5]} đến {dataFlight[6]} vào ngày {ConvertToDate(dataFlight[4])}");
                                    Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                                    Console.WriteLine($"{new string(' ', 10)}|             | Mã khách hàng | Tên khách hàng                   | Ngày sinh  | Email                       | Địa chỉ                        | Số điện thoại           | Số vé đã đặt | Tổng tiền vé $ |");
                                    Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                                    shouldDisplayHeader = false; // Đặt flag để không hiển thị header nữa
                                }
                                Console.WriteLine($"{new string(' ', 10)}| {"",-11} | {dataCustomer[1],-13} | {dataCustomer[2],-32} | {dataCustomer[7],-10} | {dataCustomer[3],-27} | {dataCustomer[6],-30} | {dataCustomer[5],-23} | {key.Value} {dataTR[2],-10} | {fl.CalculatePrice(dataTR[2],dataFlight[9]),-14} |");
                                Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                            }
                        }
                        break;
                    }
                }
            }
            if (!isFound)
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
                string flightName = data[3]; // Sử dụng tên chuyến bay làm key
                //string flightName = $"{data[3]}_{data[4]}";

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

                                if (dataTR[3].Equals(dataFlight[1]) && dataFlight[0] == "1" && dataCustomer[0] == "1")
                                {
                                    if (shouldDisplayHeader)
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine($" Chuyến bay {dataFlight[1]} khởi hành từ {dataFlight[5]} đến {dataFlight[6]} vào ngày {ConvertToDate(dataFlight[4])}");
                                        Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+");
                                        Console.WriteLine($"{new string(' ', 10)}| STT         | Mã vé         | Tên khách hàng                   | Ngày sinh  | Email                       | Địa chỉ                        | Số điện thoại           | Loại vé      |");
                                        Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+");
                                        shouldDisplayHeader = false; // Đặt flag để không hiển thị header nữa
                                    }
                                    // In thông tin của mỗi khách hàng trong nhóm
                                    Console.WriteLine($"{new string(' ', 10)}| {stt + 1,-11} | {dataTR[1],-13} | {dataTR[5],-32} | {dataTR[6],-10} | {dataTR[7],-27} | {dataTR[9],-30} | {dataTR[8],-23} | {dataTR[4],-12} |");
                                    Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+");
                                    stt++;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void DisplayTicketNumberBookedByOneCustomer(string userID, string flightNum)
        {
            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePathTR = Path.Combine(datatxt, "TicketReceipt.txt");
            string filePathFlight = Path.Combine(datatxt, "FlightScheduler.txt");

            string[] Flight = File.ReadAllLines(filePathFlight);
            string[] TicketReceipt = File.ReadAllLines(filePathTR);

            for (int j = 0; j < Flight.Length; j++)
            {
                string[] dataF = Flight[j].Split(';');
                if (dataF[0] == "1" && flightNum.Equals(dataF[1]))
                {
                    Console.WriteLine();
                    Console.WriteLine($"{new string('+', 30)} Hiển thị tất cả vé của chuyến bay {flightNum} được đặt bởi {userID} {new string('+', 30)}");
                    Console.WriteLine($"{new string(' ', 10)}+-----+-------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+---------+");
                    Console.WriteLine($"{new string(' ', 10)}| STT | Mã vé       | Tên khách hàng                   | Ngày sinh  | Email  \t\t            | Địa chỉ\t\t             | Số điện thoại\t       | Loại vé |");
                    Console.WriteLine($"{new string(' ', 10)}+-----+-------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+---------+");
                    break;
                }
            }

            bool isFound = false;
            int stt = 0;
            //
            for (int i = 0; i < TicketReceipt.Length; i++)
            {
                string[] dataTR = TicketReceipt[i].Split(';');
                //
                for (int j = 0; j < Flight.Length; j++)
                {
                    string[] dataF = Flight[j].Split(';');

                    if (userID.Equals(dataTR[2]) && dataF[1].Equals(flightNum) && dataF[0] == "1" && flightNum.Equals(dataTR[3]))
                    {
                        isFound = true;
                        //
                        Console.WriteLine($"{new string(' ', 10)}| {stt + 1,-3} | {dataTR[1],-11} | {dataTR[5],-32} | {dataTR[6],-10} | {dataTR[7],-27} | {dataTR[9],-30} | {dataTR[8],-23} | {dataTR[4],-7} |");
                        Console.WriteLine($"{new string(' ', 10)}+-----+-------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+---------+");
                        stt++;
                    }
                }
            }
            if (!isFound)
            {
                Console.WriteLine($"\nKhông tìm thấy chuyến bay {flightNum}");
            }
            if(stt == 0)
            {
                Console.WriteLine($"\nBạn chưa đăng kí chuyến bay này.");
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
                    Console.WriteLine($"{new string(' ', 10)}+-----+-------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+---------+");
                    Console.WriteLine($"{new string(' ', 10)}| STT | Mã vé       | Tên khách hàng                   | Ngày sinh  | Email  \t\t            | Địa chỉ\t\t             | Số điện thoại\t       | Loại vé |");
                    Console.WriteLine($"{new string(' ', 10)}+-----+-------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+---------+");
                    break;
                }
            }
            bool isFound = false;
            int stt = 0;
            //
            for (int i = 0; i < TicketReceipt.Length; i++)
            {
                string[] dataTR = TicketReceipt[i].Split(';');
                //
                for (int j = 0; j < Flight.Length; j++)
                {
                    string[] dataF = Flight[j].Split(';');

                    if (dataF[1].Equals(flightNum) && dataF[0] == "1" && flightNum.Equals(dataTR[3]))
                    {
                        isFound = true;
                        //
                        for (int k = 0; k < Customer.Length; k++)
                        {
                            string[] dataCustomer = Customer[k].Split(';');

                            if (dataTR[2].Equals(dataCustomer[1]) && dataCustomer[0] == "1")
                            {
                                Console.WriteLine($"{new string(' ', 10)}| {stt + 1,-3} | {dataTR[1],-11} | {dataTR[5],-32} | {dataTR[6],-10} | {dataTR[7],-27} | {dataTR[9],-30} | {dataTR[8],-23} | {dataTR[4],-7} |");
                                Console.WriteLine($"{new string(' ', 10)}+-----+-------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+---------+");
                                stt++;
                            }
                        }
                    }
                }
            }
            if (!isFound)
            {
                Console.WriteLine($"\nKhông tìm thấy chuyến bay {flightNum}");
            }
            if(stt == 0)
            {
                Console.WriteLine($"\nKhông có hành khách trong chuyến bay {flightNum}");
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
