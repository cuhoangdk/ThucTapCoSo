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
    internal class FlightReservation : IDisplayClass
    {
        public void BookFlight(string flightNum, string userID, int numOfTickets, string ticketType)
        {
			Console.OutputEncoding = Encoding.Unicode;
            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePathCus = Path.Combine(datatxt, "Customer.txt");
            string filePathFl = Path.Combine(datatxt, "FlightScheduler.txt");
            string filePathTicketReceipt = Path.Combine(datatxt, "TicketReceipt.txt");

            string[] customer = File.ReadAllLines(filePathCus);
            string[] flight = File.ReadAllLines(filePathFl);
            //string[] TicketReceipt = File.ReadAllLines(filePathTicketReceipt);

            bool isFound = false;
            DateTime now = DateTime.Now;
            //
            for (int i=0; i<flight.Length; i++)
            {
                string[] dataFlight = flight[i].Split(';');
                if(dataFlight[1].Equals(flightNum) && dataFlight[0] == "1")
                {
                    isFound = true;
                    //
                    for (int j=0; j < customer.Length; j++)
                    {
                        string[] dataCustomer = customer[j].Split(';');
                        if(dataCustomer[1].Equals(userID))
                        {
                            int availableECOSeats = int.Parse(dataFlight[3]);
                            int availableBSNSeats = int.Parse(dataFlight[2]);

                            if (ticketType == "ECO")
                            {
                                if (availableECOSeats >= numOfTickets)
                                {
                                    // Giảm số ghế hạng Eco
                                    dataFlight[3] = Convert.ToString(availableECOSeats - numOfTickets);
                                    // Receipt
                                    using (StreamWriter write = new StreamWriter(filePathTicketReceipt, true))
                                    {
                                        write.WriteLine($"{now};{flightNum};{userID};{numOfTickets};{ticketType}");
                                    }

                                    flight[i] = string.Join(";", dataFlight);
                                    File.WriteAllLines(filePathFl, flight);

                                    Console.WriteLine($"\n {new string(' ', 30)} Bạn đã đặt {numOfTickets} vé {ticketType} cho Chuyến bay \"{flightNum.ToUpper()}\"...");
                                }
                                else
                                {
                                    Console.WriteLine($"Không đủ chỗ ngồi hạng {ticketType} cho Chuyến bay \"{flightNum.ToUpper()}\"...");
                                }
                            }
                            else if (ticketType == "BSN")
                            {
                                if (availableBSNSeats >= numOfTickets)
                                {
                                    dataFlight[2] = Convert.ToString(availableBSNSeats - numOfTickets);

                                    using (StreamWriter write = new StreamWriter(filePathTicketReceipt, true))
                                    {
                                        write.WriteLine($"{now};{flightNum};{userID};{numOfTickets};{ticketType}");
                                    }

                                    flight[i] = string.Join(";", dataFlight);
                                    File.WriteAllLines(filePathFl, flight);

                                    Console.WriteLine($"\n {new string(' ', 30)} Bạn đã đặt {numOfTickets} vé {ticketType} cho Chuyến bay \"{flightNum.ToUpper()}\"...");
                                }
                                else
                                {
                                    Console.WriteLine($"Không đủ chỗ ngồi hạng {ticketType} cho Chuyến bay \"{flightNum.ToUpper()}\"...");
                                }
                            }
                            break;
                        }
                    }
                }
            }
            if (!isFound)
            {
                Console.WriteLine($"Số hiệu không hợp lệ...! Không tìm thấy chuyến bay với ID \"{flightNum}\"...");
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

            string filePathTR = Path.Combine(datatxt, "TicketReceipt.txt");
            string filePathCustomer = Path.Combine(datatxt, "Customer.txt");
            string filePathFlight = Path.Combine(datatxt, "FlightScheduler.txt");

            string[] flight = File.ReadAllLines(filePathFlight);
            string[] TicketReceipt = File.ReadAllLines(filePathTR);
            string[] Customer = File.ReadAllLines(filePathCustomer);

            //in ra console user có những chuyến bay nào
            Console.WriteLine();
            Console.WriteLine("+------+-----------------------------+-----------+------------------+-----------------------+------------------------+-----------------------------+-------------+--------+------------------+");
            Console.WriteLine("| STT  | LỊCH BAY                    | MÃ CHUYẾN | SỐ VÉ            | KHỞI HÀNH             | ĐIỂM ĐẾN               | THƯỜI GIAN HẠ CÁNH          |THỜI GIAN BAY| CỔNG   | TRẠNG THÁI       |");
            Console.WriteLine("+------+-----------------------------+-----------+------------------+-----------------------+------------------------+-----------------------------+-------------+--------+------------------+");

            int stt = 0;

            for(int i = 0; i<TicketReceipt.Length; i++)
            {
                string[] dataTR = TicketReceipt[i].Split(';');
                //
                if (userID.Equals(dataTR[2]))
                {
                    for (int j = 0; j < Customer.Length; j++)
                    {
                        string[] dataC = Customer[j].Split(';');

                        if (userID.Equals(dataC[1]) && dataC[0] == "1")
                        {
                            for(int h = 0; h<flight.Length; h++)
                            {
                                string[] dataF = flight[h].Split(';');
                                if (dataF[1].Equals(dataTR[1]))
                                {
                                    Flight fl = new Flight();
                                    Console.WriteLine($"| {stt + 1,-5}| {dataF[4],-27} | {dataF[1],-9} | {dataTR[3],-1} {dataTR[4],-13}  | {dataF[5],-21} | {dataF[6],-22} | {fl.FetchArrivalTime(dataF[4], dataF[7]),-27} | {dataF[7],-11} | {dataF[8],-6} | {FlightStatus(dataF[1]),-17}|");
                                    Console.WriteLine($"+------+-----------------------------+-----------+------------------+-----------------------+------------------------+-----------------------------+-------------+--------+------------------+");
                                    stt++;
                                }
                            }
                        }
                    }
                }
            }

            if (stt == 0)
            {
                Console.WriteLine($"Không có chuyến bay được đăng ký cho người dùng với ID {userID}.");
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
                                        Console.WriteLine($"{new string(' ', 10)}| STT         | Mã khách hàng | Tên khách hàng                   | Năm sinh   | Email                       | Địa chỉ                        | Số điện thoại           | Số vé đã đặt | Tổng tiền vé $ |");
                                        Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+------------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                                        shouldDisplayHeader = false; // Đặt flag để không hiển thị header nữa
                                    }
                                    // In thông tin của mỗi khách hàng trong nhóm
                                    Console.WriteLine($"{new string(' ', 10)}| {stt + 1,-11} | {dataCustomer[1],-13} | {dataCustomer[2],-32} | {dataCustomer[7],-10} | {dataCustomer[3],-27} | {dataCustomer[6],-30} | {dataCustomer[5],-23} | {dataTR[3]} {dataTR[4]} | {TotalPrice(dataTR[3], fl.CalculatePrice(dataFlight[9])),-14} |");
                                    Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
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
                    Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
                    Console.WriteLine($"{new string(' ', 10)}| Mã chuyến bay  |Mã khách hàng| Tên khách hàng                   | Tuổi    | Email  \t\t\t    | Địa chỉ\t\t\t     | Số điện thoại\t       | Số vé đã đặt | Tổng tiền vé $ |");
                    Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
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
                                Console.WriteLine($"{new string(' ', 10)}| {flightNum,-14} | {dataCustomer[1],-11} | {dataCustomer[2],-32} | {dataCustomer[7],-7} | {dataCustomer[3],-27} | {dataCustomer[6],-30} | {dataCustomer[5],-23} | {dataTR[3]} {dataTR[4]} | {TotalPrice(dataTR[3], fl.CalculatePrice(dataF[9])),-14} |");
                                Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+----------------+");
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
