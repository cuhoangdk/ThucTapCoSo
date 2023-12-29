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
        //Hàm tạo số ghế (mã ghế)
        static string SeatID(string flightNum, string ticketType)
        {
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");

            string filePathTicketReceipt = Path.Combine(datatxt, "TicketReceipt.txt");
            string[] TicketReceipt = File.ReadAllLines(filePathTicketReceipt);

            var matchingTickets = TicketReceipt
                .Where(line => line.Contains(flightNum) && line.Contains(ticketType))
                .ToList();

            int newSeatNumber;
            if (matchingTickets.Count > 0)
            {
                var usedSeatNumbers = matchingTickets
                    .Select(line => int.Parse(line.Split(';')[2].Split('-')[1]))
                    .ToList();

                // Tìm số vé chưa được sử dụng
                newSeatNumber = 1;
                while (usedSeatNumbers.Contains(newSeatNumber))
                {
                    newSeatNumber++;
                }
                string newSeatID = $"{ticketType}-{newSeatNumber:000}";
                return newSeatID;
            }
            else
            {
                // Nếu chuyến bay không có trong TicketReceipt, bắt đầu từ số 1
                newSeatNumber = 1;
            }
            return "";
        }
        //Hàm đặt chỗ ngồi cho chuyến bay, mỗi chỗ ngồi là một hành khách
        public void BookFlight(string userID)
        {
			Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            Random rand = new Random();

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
                string tiketID = SeatID(flightToBeBooked, ticketType);

                Console.Write($"Nhập số lượng vé cho chuyến bay {flightToBeBooked} :   ");
                while (!int.TryParse(Console.ReadLine(), out numOfTickets) || numOfTickets > 10 || numOfTickets < 1)
                {
                    Console.Write("LỖI!! Vui lòng nhập số lượng vé hợp lệ (ít hơn 10, nhiều hơn 0): ");
                }
                for (int i = 0; i < flight.Length; i++)
                {
                    string[] dataFlight = flight[i].Split(';');
                    int availableECOSeats = int.Parse(dataFlight[3]);
                    int availableBSNSeats = int.Parse(dataFlight[2]);

                    if (dataFlight[1].Equals(flightToBeBooked) && dataFlight[0] == "1")
                    {
                        int rtID = rand.Next(10000000, 99999999);
                        isFound = true;
                        for (int count = 1; count <= numOfTickets; count++)
                        {
                            string name, email, phone, address;
                            DateTime birth;
                            if (ticketType == "ECO")
                            {

                                if (availableECOSeats >= numOfTickets)
                                {
                                    checkTicket = true;
                                    Console.WriteLine($"\n\tNHẬP THÔNG TIN CỦA HÀNH KHÁCH THỨ {count}:\t");

									Console.Write("\tHỌ VÀ TÊN:\t");
									name = Console.ReadLine();
									while (string.IsNullOrWhiteSpace(name))
									{
										Console.Write("VUI LÒNG NHẬP HỌ VÀ TÊN: ");
										name = Console.ReadLine();
									}
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
									while (string.IsNullOrWhiteSpace(phone))
									{
										Console.Write("VUI LÒNG NHẬP SỐ ĐIỆN THOẠI: ");
										phone = Console.ReadLine();
									}
									Console.Write("\tĐỊA CHỈ:\t");
                                    address = Console.ReadLine();
									while (string.IsNullOrWhiteSpace(address))
									{
										Console.Write("VUI LÒNG NHẬP ĐỊA CHỈ: ");
										address = Console.ReadLine();
									}
									Console.Write("\tNGÀY THÁNG NĂM SINH:\t");
									DateTime currentDate = DateTime.Now;
									while (!DateTime.TryParseExact(Console.ReadLine(), "d/M/yyyy", null, System.Globalization.DateTimeStyles.None, out birth) || birth > currentDate)
									{
										Console.Write("\tVUI LÒNG NHẬP NGÀY SINH HỢP LỆ: \t");
									}
									using (StreamWriter write = new StreamWriter(filePathTicketReceipt, true))
                                    {
                                        write.WriteLine($"{now.ToString("dd/MM/yyyy HH:mm:ss")};{rtID};{tiketID};{userID};{flightToBeBooked};{ticketType};{name};{birth:dd/MM/yyyy};{email};{phone};{address}");
                                    }
                                    dataFlight[3] = Convert.ToString((int.Parse(dataFlight[3])-1)); 
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

                                if (availableBSNSeats >= numOfTickets)
                                {
                                    checkTicket = true;
                                    Console.WriteLine($"\n\tNHẬP THÔNG TIN CỦA HÀNH KHÁCH THỨ {count}:\t");

									Console.Write("\tHỌ VÀ TÊN:\t");
									name = Console.ReadLine();
									while (string.IsNullOrWhiteSpace(name))
									{
										Console.Write("VUI LÒNG NHẬP HỌ VÀ TÊN: ");
										name = Console.ReadLine();
									}
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
									while (string.IsNullOrWhiteSpace(phone))
									{
										Console.Write("VUI LÒNG NHẬP SỐ ĐIỆN THOẠI: ");
										phone = Console.ReadLine();
									}
									Console.Write("\tĐỊA CHỈ:\t");
									address = Console.ReadLine();
									while (string.IsNullOrWhiteSpace(address))
									{
										Console.Write("VUI LÒNG NHẬP ĐỊA CHỈ: ");
										address = Console.ReadLine();
									}
									Console.Write("\tNGÀY THÁNG NĂM SINH:\t");
									DateTime currentDate = DateTime.Now;
									while (!DateTime.TryParseExact(Console.ReadLine(), "d/M/yyyy", null, System.Globalization.DateTimeStyles.None, out birth) || birth > currentDate)
									{
										Console.Write("\tVUI LÒNG NHẬP NGÀY SINH HỢP LỆ: \t");
									}

									using (StreamWriter write = new StreamWriter(filePathTicketReceipt, true))
                                    {
                                        write.WriteLine($"{now.ToString("dd/MM/yyyy HH:mm:ss")};{rtID};{tiketID};{userID};{flightToBeBooked};{ticketType};{name};{birth:dd/MM/yyyy};{email};{phone};{address}");
                                    }

                                    dataFlight[2] = Convert.ToString((int.Parse(dataFlight[2]) - 1));
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
        //Hàm hủy đặt chỗ, có thể hủy toàn bộ hoặc hủy từng chỗ ngồi của vé
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

            Console.WriteLine("Chọn mã vé muốn hủy:");
            string trID = Console.ReadLine().ToUpper();

            if(DisplayTicketNumberBookedByOneCustomer(userID, trID))
            {

                bool isFound = false;
                bool ticketCheck = false;
                int countBSN = 0, countECO = 0;

                Console.Write("Nhập mã ghế muốn hủy \n(chọn n/N để thoát):\t");
                string SeatIDCancel = Console.ReadLine().ToUpper();

                while (SeatIDCancel.ToLower() != "n")
                {
                    for (int i = 0; i < TicketReceipt.Count; i++)
                    {
                        string[] dataTR = TicketReceipt[i].Split(';');
                        if (dataTR[1].Equals(trID))
                        {
                            ticketCheck = true;
                            for (int j = 0; j < flight.Length; j++)
                            {
                                string[] dataFlight = flight[j].Split(';');

                                if (dataFlight[1].Equals(dataTR[4]) && dataFlight[0] == "1")
                                {
                                    isFound = true;

                                    if (SeatIDCancel.Equals(dataTR[2]) && userID.Equals(dataTR[3]))
                                    {
                                        string ticketType = dataTR[5];
                                        int ticketsToBeReturned = 1;
                                        if (ticketType == "BSN")
                                        {
                                            dataFlight[2] = Convert.ToString(int.Parse(dataFlight[2]) + ticketsToBeReturned);
                                            //xóa khách hàng đó ra khỏi file
                                            TicketReceipt.RemoveAt(i);
                                            countBSN++;
                                            Console.WriteLine($"\tBạn đã hủy ghế {SeatIDCancel}");
                                            File.WriteAllLines(filePathTR, TicketReceipt);
                                            //cập nhật số vé có trong FlightScheduler.txt
                                            flight[j] = string.Join(";", dataFlight);
                                            File.WriteAllLines(filePathFl, flight);
                                            break;
                                        }
                                        else if (ticketType == "ECO")
                                        {
                                            dataFlight[3] = Convert.ToString(int.Parse(dataFlight[3]) + ticketsToBeReturned);
                                            //xóa khách hàng đó ra khỏi file
                                            TicketReceipt.RemoveAt(i);
                                            countECO++;
                                            Console.WriteLine($"Bạn đã hủy ghế {SeatIDCancel}");
                                            File.WriteAllLines(filePathTR, TicketReceipt);
                                            //cập nhật số vé có trong FlightScheduler.txt
                                            flight[j] = string.Join(";", dataFlight);
                                            File.WriteAllLines(filePathFl, flight);
                                            break;
                                        }
                                    }

                                    break;
                                }
                            }
                            if (!ticketCheck)
                            {
                                Console.WriteLine($"\nKhông tìm thấy mã ghế {SeatIDCancel}");
                                break;
                            }
                        }
                    }
                    DisplayTicketNumberBookedByOneCustomer(userID, trID);
                    Console.Write("Nhập mã vé muốn hủy (chọn n/N để thoát):\t");
                    SeatIDCancel = Console.ReadLine().ToUpper();
                }
                if (!isFound)
                {
                    Console.WriteLine($"\nKHÔNG TÌM THẤY VÉ \"{trID.ToUpper()}\".....");
                }
                else
                {
                    Console.WriteLine($"\nBạn đã hủy {countBSN} vé Business, {countECO} vé Economy trong vé {trID}");
                }
            }
        }
        //Hàm tìm kiếm chuyến bay theo 3 tiêu chí: Điểm đi, điểm đến, ngày khởi hành
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
            DateTime datesearch;
            while (!DateTime.TryParseExact(Console.ReadLine(), "d/M/yyyy", null, System.Globalization.DateTimeStyles.None, out datesearch))
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
                DateTime datefile = DateTime.ParseExact(data[4], "ddd, dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                if (datesearch.ToString("dd/MM/yyyy")==datefile.ToString("dd/MM/yyyy") && normalFrom.Equals(normaldataFrom, StringComparison.OrdinalIgnoreCase) && normalTo.Equals(normaldataTo, StringComparison.OrdinalIgnoreCase) && data[0] == "1")
                {
                    isFound = true;
                    Console.WriteLine($"| {stt,-4} | {data[4],-25} | {data[1],-11} | BSN: {data[2],-3} / ECO: {data[3],-3}          | {data[5],-21} | {data[6],-22} | {fl.FetchArrivalTime(data[4], data[7]),-25} | {data[7],6}  Hrs | {data[8],-6} | {data[9],-9} / {data[10],-10} | {fl.CalculatePrice("BSN", data[9], 18),-8} /  {fl.CalculatePrice("ECO", data[9], 18),-8} |");
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
        //Hàm chuyến string thành date
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
        //Hàm xét trạng thái chuyến bay (đã xóa, đang theo lịch trình, đã cất cánh)
        private string FlightStatus(string flag, string date)
        {
            if (flag == "0") return "ĐÃ XÓA";
            DateTime datefile = DateTime.ParseExact(date, "ddd, dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            if (datefile < DateTime.Now) return "ĐÃ CẤT CÁNH";
            else return "THEO LỊCH TRÌNH";
        }
        //Hàm tính tổng tiền
        public float TotalPrice(int numOfTicket, float price)
        {
            float tolal = (float)Math.Round(price * numOfTicket ,2);
            return tolal; 
        }
        //Hàm hiển thị tất cả các chuyến bay được đăng kí bởi một người dùng
        public void DisplayFlightsRegisteredByOneUser(string userID)
        {
            Console.OutputEncoding = Encoding.Unicode;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePathFlight = Path.Combine(datatxt, "FlightScheduler.txt");
            string filePathTR = Path.Combine(datatxt, "TicketReceipt.txt");

            string[] flight = File.ReadAllLines(filePathFlight);
            string[] TicketReceipt = File.ReadAllLines(filePathTR);

            Dictionary<string, int> user_receipt = new Dictionary<string, int>();

            foreach (string line in TicketReceipt)
            {
                string[] data = line.Split(';');
                string key = $"{data[1]}_{data[3]}_{data[4]}_{data[5]}";  //  ma hoa don_userID_flightNum_ticketType

                if (user_receipt.ContainsKey(key))
                {
                    user_receipt[key]++;
                }
                else
                {
                    user_receipt[key] = 1;
                }
            }

            int stt = 1;
            bool isFound = false;
            bool shouldDisplayHeader = true;
            foreach (var key in user_receipt)
            {
                string[] dataTR = key.Key.Split('_');   //   ma hoa don_userID_flightNum_ticketType

                for(int h=0; h<TicketReceipt.Length; h++)
                {
                    string[] dataPS = TicketReceipt[h].Split(';');

                    if (dataTR[1].Equals(dataPS[3]) && userID.Equals(dataTR[1]))
                    {
                        for (int i = 0; i < flight.Length; i++)
                        {
                            string[] dataFlight = flight[i].Split(';');
                            if (dataTR[2].Equals(dataFlight[1]) && dataFlight[0] == "1")
                            {
                                isFound = true;
                                Flight fl = new Flight();
                                if (shouldDisplayHeader)
                                {
                                    Console.WriteLine();
                                    Console.Write("\t+------+-------------+-------------+---------------------------+------------------------+-----------------------+---------------+--------+--------------+----------------+-----------------+\n");
                                    Console.Write("\t| STT  | MÃ ĐẶT CHỖ  | MÃ CHUYẾN   | THỜI GIAN CẤT CÁNH        | KHỞI HÀNH              | ĐIẾM ĐẾN              | THỜI GIAN BAY |  CỔNG  | TỔNG SỐ VÉ   | TỔNG TIỀN $    | TRẠNG THÁI      |\n");
                                    Console.Write("\t+------+-------------+-------------+---------------------------+------------------------+-----------------------+---------------+--------+--------------+----------------+-----------------+\n");
                                    shouldDisplayHeader = false; // Đặt flag để không hiển thị header nữa
                                }

                                DateTime birth = DateTime.ParseExact(dataPS[7],"dd/MM/yyyy", CultureInfo.InvariantCulture);

                                int age = DateTime.Now.Year - birth.Year;
                                if (DateTime.Now.Month < birth.Month || (DateTime.Now.Month == birth.Month && DateTime.Now.Day < birth.Day))
                                {
                                    age--;
                                }
                                Console.Write($"\t| {stt,-4} | {dataTR[0],-11} | {dataFlight[1],-11} | {dataFlight[4],-25} | {dataFlight[5],-22} | {dataFlight[6],-21} | {dataFlight[7],-8}  Hrs | {dataFlight[8],-6} | {key.Value} {dataTR[3],-10} | {TotalPrice(key.Value, fl.CalculatePrice(dataTR[3], dataFlight[9], age)),-14} | {FlightStatus(dataFlight[0], dataFlight[4]),-15} |\n");
                                Console.Write("\t+------+-------------+-------------+---------------------------+------------------------+-----------------------+---------------+--------+--------------+----------------+-----------------+\n");
                                stt++;
                            }
                        }
                        break;
                    }
                }
            }
            if (!isFound)
            {
                Console.WriteLine($"\tUSERID {userID} CHƯA ĐĂNG KÍ CHUYẾN BAY NÀO");
            }
        }
        //Hàm hiển thị tất cả hành khách của tất cả chuyến bay
        public void DisplayRegisteredUsersForAllFlight()
        {
            Console.WriteLine();
            Console.WriteLine($"\n{new string('+', 30)} TẤT CẢ CÁC CHUYẾN BAY ĐÃ ĐƯỢC ĐĂNG KÍ\" {new string('+', 30)}\n");

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
                string flightName = data[4]; // Sử dụng tên chuyến bay làm key
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
                        if ( dataTR[3].Equals(dataCustomer[1]))
                        {
                            for (int i = 0; i < flight.Length; i++)
                            {
                                string[] dataFlight = flight[i].Split(';');

                                if (dataTR[4].Equals(dataFlight[1]) && dataFlight[0] == "1" && dataCustomer[0] == "1")
                                {
                                    if (shouldDisplayHeader)
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine($"\tChuyến bay {dataFlight[1]} khởi hành từ {dataFlight[5]} đến {dataFlight[6]} vào {dataFlight[4]}");
                                        Console.WriteLine($"{new string(' ', 10)}+-----+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+");
                                        Console.WriteLine($"{new string(' ', 10)}| STT | SỐ GHẾ        | TÊN HÀNH KHÁCH                   | NGÀY SINH  | EMAIL                       | ĐỊA CHỈ                        | SỐ ĐIỆN THOẠI           | LOẠI VÉ      |");
                                        Console.WriteLine($"{new string(' ', 10)}+-----+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+");
                                        shouldDisplayHeader = false; // Đặt flag để không hiển thị header nữa
                                    }
                                    // In thông tin của mỗi khách hàng trong nhóm
                                    Console.WriteLine($"{new string(' ', 10)}| {stt + 1,-3} | {dataTR[2],-13} | {dataTR[6],-32} | {dataTR[7],-10} | {dataTR[8],-27} | {dataTR[10],-30} | {dataTR[9],-23} | {dataTR[5],-12} |");
                                    Console.WriteLine($"{new string(' ', 10)}+-----+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+");
                                    stt++;
                                }
                            }
                        }
                    }
                }
            }
        }
        //Hàm hiển thị tất cả hành khách của một chuyến bay cụ thể
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


            bool flag = false;
            for (int j = 0; j < Flight.Length; j++)
            {
                string[] dataF = Flight[j].Split(';');
                if (dataF[0] == "1" && flightNum.Equals(dataF[1]))
                {
                    Console.WriteLine();
                    Console.WriteLine($"\n{new string('+', 30)} Hiển thị Khách hàng đã đăng ký cho Chuyến bay số \"{flightNum,-6}\" {new string('+', 30)}\n");
                    Console.WriteLine($"{new string(' ', 10)}+-----+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+");
                    Console.WriteLine($"{new string(' ', 10)}| STT | SỐ GHẾ        | TÊN HÀNH KHÁCH                   | NGÀY SINH  | EMAIL                       | ĐỊA CHỈ                        | SỐ ĐIỆN THOẠI           | LOẠI VÉ      |");
                    Console.WriteLine($"{new string(' ', 10)}+-----+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+");
                    flag = true;
                    break;
                }
            }
            if (flag == false)
            {
                Console.WriteLine($"\nCHUYẾN BAY {flightNum} KHÔNG TỒN TẠI");
                return;
            }
            int stt = 0;
            //
            for (int i = 0; i < TicketReceipt.Length; i++)
            {
                string[] dataTR = TicketReceipt[i].Split(';');
                //
                for (int j = 0; j < Flight.Length; j++)
                {
                    string[] dataF = Flight[j].Split(';');
                    if (dataF[1].Equals(flightNum) && dataF[0] == "1" && flightNum.Equals(dataTR[4]))
                    {
                        for (int k = 0; k < Customer.Length; k++)
                        {
                            string[] dataCustomer = Customer[k].Split(';');

                            if (dataTR[3].Equals(dataCustomer[1]) && dataCustomer[0] == "1")
                            {
                                Console.WriteLine($"{new string(' ', 10)}| {stt + 1,-3} | {dataTR[2],-13} | {dataTR[6],-32} | {dataTR[7],-10} | {dataTR[8],-27} | {dataTR[10],-30} | {dataTR[9],-23} | {dataTR[5],-12} |");
                                Console.WriteLine($"{new string(' ', 10)}+-----+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+");
                                stt++;
                            }
                        }
                    }
                    else if (dataF[1].Equals(flightNum) && dataF[0] == "0")
                    {
                        Console.WriteLine($"\nCHUYẾN BAY {flightNum} ĐÃ BỊ XÓA");
                        return;
                    }

                }
            }

            if (stt == 0)
            {
                Console.WriteLine($"\nCHUYẾN BAY {flightNum} KHÔNG CÓ HÀNH KHÁCH");
                return;
            }

        }
        //Hàm hiển thị tất cả hành khách được đăng kí ở trong một vé cụ thể (sử dụng ở trong hàm hủy vé)
        public bool DisplayTicketNumberBookedByOneCustomer(string userID, string trID)
        {
            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePathTR = Path.Combine(datatxt, "TicketReceipt.txt");
            string filePathFlight = Path.Combine(datatxt, "FlightScheduler.txt");

            string[] Flight = File.ReadAllLines(filePathFlight);
            string[] TicketReceipt = File.ReadAllLines(filePathTR);

            for (int j = 0; j < TicketReceipt.Length; j++)
            {
                string[] dataTR = TicketReceipt[j].Split(';');
                if (trID.Equals(dataTR[1]))
                {
                    Console.WriteLine();
                    Console.WriteLine($"{new string('+', 30)} Các chỗ ngồi của vé {trID} được đặt bởi {userID} {new string('+', 30)}");
                    Console.WriteLine($"{new string(' ', 10)}+-----+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+");
                    Console.WriteLine($"{new string(' ', 10)}| STT | SỐ GHẾ        | TÊN HÀNH KHÁCH                   | NGÀY SINH  | EMAIL                       | ĐỊA CHỈ                        | SỐ ĐIỆN THOẠI           | LOẠI VÉ      |");
                    Console.WriteLine($"{new string(' ', 10)}+-----+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+");
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

                    if (userID.Equals(dataTR[3]) && dataF[1].Equals(dataTR[4]) && dataF[0] == "1" && trID.Equals(dataTR[1]))
                    {
                        isFound = true;
                        //
                        Console.WriteLine($"{new string(' ', 10)}| {stt + 1,-3} | {dataTR[2],-13} | {dataTR[6],-32} | {dataTR[7],-10} | {dataTR[8],-27} | {dataTR[10],-30} | {dataTR[9],-23} | {dataTR[5],-12} |");
                        Console.WriteLine($"{new string(' ', 10)}+-----+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+");
                        stt++;
                    }
                }
            }
            if (!isFound)
            {
                Console.WriteLine($"\n\tCHƯA CÓ VÉ NÀO ĐƯỢC ĐĂNG KÍ");
                return isFound;
            }
            return true;
        }
        public bool DisplayTicketRecept(string userID, string trID)
        {
            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePathTR = Path.Combine(datatxt, "TicketReceipt.txt");
            string filePathFlight = Path.Combine(datatxt, "FlightScheduler.txt");
            string filePathC = Path.Combine(datatxt, "Customer.txt");



            string[] Flight = File.ReadAllLines(filePathFlight);
            string[] TicketReceipt = File.ReadAllLines(filePathTR);
            string[] Customer = File.ReadAllLines(filePathC);


            string datebook = "";
            string nameCustomer = "";
            string emailCustomer = "";
            string phoneCustomer = "";

            string idFlight = "";
            string typeTicket = "";
            string fromWhich = "";
            DateTime timefromWich = DateTime.Now;
            string toWhich = "";
            DateTime timetoWich = DateTime.Now;
            TimeSpan timeFlight = TimeSpan.Zero;
            float totalPrice = 0;
            
            
            bool isFound = false;

            for (int i = 0; i < Customer.Length; i++)
            {
                string[] data = Customer[i].Split(';');
                if (data[0] == "1" && data[1] == userID)
                {
                    nameCustomer = data[2];
                    emailCustomer = data[3];
                    phoneCustomer = data[5];
                }
            }
            Console.WriteLine($"+-------------------------------------------------------------------------------------------------+");
            Console.WriteLine($"|   STAR AIRLINES                                                           Hotline: 0999999999   |");
            Console.WriteLine($"+-------------------------------------------------------------------------------------------------+");
            Console.WriteLine($"|                                           VÉ ĐIỆN TỬ                                            |");
            Console.WriteLine($"+-------------------------------------------------------------------------------------------------+");
            Console.WriteLine($"| Mã đặt chỗ (số vé): {trID,-8}                                                                    |");
            Console.WriteLine($"+-------------------------------------------------------------------------------------------------+");
            Console.WriteLine($"| 1. THÔNG TIN NGƯỜI ĐẶT CHỖ                                                                      |");
            Console.WriteLine($"+-------------------------------------------------------------------------------------------------+");
            Console.WriteLine($"| Mã khách hàng: {userID,-10}                   Liên lạc  : {phoneCustomer,-10}                              |");
            Console.WriteLine($"| Họ tên       : {nameCustomer,-20}         Email     : {emailCustomer,-20}                    |");
            Console.WriteLine($"+-------------------------------------------------------------------------------------------------+");
            Console.WriteLine($"| 2. THÔNG TIN HÀNH KHÁCH                                                                         |");
            Console.WriteLine($"+---------------------------------------------------------------------+-------------+-------------+");
            Console.WriteLine($"| Tên hành khách                                                      | Số ghế      | Mã chuyến   |");
            Console.WriteLine($"+---------------------------------------------------------------------+-------------+-------------+");
            //


            for (int i = 0; i < TicketReceipt.Length; i++)
            {
                string[] dataTR = TicketReceipt[i].Split(';');

                //
                for (int j = 0; j < Flight.Length; j++)
                {
                    string[] dataF = Flight[j].Split(';');

                    if (userID.Equals(dataTR[3]) && dataF[1].Equals(dataTR[4]) && dataF[0] == "1" && trID.Equals(dataTR[1]))
                    {
                        isFound = true;
                        //
                        Flight fl = new Flight();

                        Console.WriteLine($"| {dataTR[6],-20}                                                | {dataTR[2],-6}     | {dataTR[4],-6}      |");
                        idFlight = dataF[1];
                        timefromWich = DateTime.ParseExact(dataF[4], "ddd, dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                        fromWhich = dataF[5];
                        toWhich= dataF[6];
                        timeFlight = TimeSpan.Parse(dataF[7]);
                        typeTicket = dataTR[5];
                        datebook = dataTR[0];
                        float price = fl.CalculatePrice(typeTicket, dataF[9], 18);
                        totalPrice += price;
                    } 
                }
            }
            if (!isFound)
            {
                Console.WriteLine($"\n\tKHÔNG TÌM THẤY VÉ");
                return isFound;
            }
            Console.WriteLine($"+---------------------------------------------------------------------+-------------+-------------+");
            Console.WriteLine($"| 3. THÔNG TIN CHUYẾN BAY                                                                         |");
            Console.WriteLine($"+------------+------+--------------------------------------+--------------------------------------+");
            Console.WriteLine($"| Chuyến bay | Loại | Khởi hành                            | Đến                                  |");
            Console.WriteLine($"+------------+------+--------------------------------------+--------------------------------------+");
            Console.WriteLine($"| {idFlight,-10} | {typeTicket,-4} | {fromWhich,-19} {timefromWich.ToString("HH:mm dd/MM/yyyy"),-10} | {toWhich,-19} {timefromWich.Add(timeFlight).ToString("HH:mm dd/MM/yyyy"),-10} |");
            Console.WriteLine($"+------------+------+--------------------------------------+--------------------------------------+");
            Console.WriteLine($"| TỔNG TIỀN    : {totalPrice,-10} $                              | Ngày đặt   :     {datebook,-13} |");
            Console.WriteLine($"+----------------------------------------------------------+--------------------------------------+");

            return true;
        }
        //Hàm hiển thị các banner
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
