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

        public void BookFlight(string flightNo, int numOfTickets, string userID)
        {
			Console.OutputEncoding = Encoding.Unicode;
            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");
            //tìm tới thư mục 3 thư mục txt
            string filePathCus = Path.Combine(datatxt, "Customer.txt");
            string filePathFl = Path.Combine(datatxt, "FlightScheduler.txt");
            string filePathCBF = Path.Combine(datatxt, "CustomerBOOKFLIGHT.txt");
            string filePathFlightHasCustomers = Path.Combine(datatxt, "FlightHasCustomers.txt");
            //tạo biến lưu 3 file
            string[] customer = File.ReadAllLines(filePathCus);
            string[] flight = File.ReadAllLines(filePathFl);
            string[] CustomerBF = File.ReadAllLines(filePathCBF);
            string[] FlightHasCustomers = File.ReadAllLines(filePathFlightHasCustomers);

            bool isFound = false;

            for (int i=0; i<flight.Length; i++)
            {
                string[] dataFlight = flight[i].Split(';');
                if(dataFlight[1].Equals(flightNo) && dataFlight[9] == "1")
                {
                    bool checkFlightHasCustomer = false;
                    isFound = true;

                    for (int j=0; j < customer.Length; j++)
                    {
                        string[] dataCustomer = customer[j].Split(';');
                        if(dataCustomer[0].Equals(userID))
                        {

                            int availableSeats = int.Parse(dataFlight[2]);

                            if (availableSeats >= numOfTickets)
                            {
                                // Giảm số ghế của chuyến bay
                                dataFlight[2] = Convert.ToString(availableSeats - numOfTickets);
                                // Flight Has Customers
                                for (int f = 0; f < FlightHasCustomers.Length; f++)
                                {
                                    string[] dataFlightHasCustomers = FlightHasCustomers[f].Split(';');
                                    if (userID.Equals(dataFlightHasCustomers[1]) && flightNo.Equals(dataFlightHasCustomers[0]))
                                    {
                                        checkFlightHasCustomer = true;
                                        dataFlightHasCustomers[2] = Convert.ToString(int.Parse(dataFlightHasCustomers[2]) + numOfTickets);
                                        FlightHasCustomers[f] = string.Join(";", dataFlightHasCustomers);
                                        File.WriteAllLines(filePathFlightHasCustomers, FlightHasCustomers);
                                        break;
                                    }
                                }
                                if (!checkFlightHasCustomer)
                                {
                                    using (StreamWriter write = new StreamWriter(filePathFlightHasCustomers, true))
                                    {
                                        write.WriteLine($"{flightNo};{userID};{numOfTickets}");
                                    }
                                }
                                // User Has Flight
                                bool check = false;
                                for (int h = 0; h < CustomerBF.Length; h++)
                                {
                                    string[] dataCBF = CustomerBF[h].Split(';');
                                    if (userID.Equals(dataCBF[0]) && flightNo.Equals(dataCBF[1]))
                                    {
                                        check = true;
                                        dataCBF[3] = Convert.ToString(int.Parse(dataCBF[3]) + numOfTickets);
                                        CustomerBF[h] = string.Join(";", dataCBF);
                                        File.WriteAllLines(filePathCBF, CustomerBF);
                                        break;
                                    }
                                }
                                if (!check)
                                {
                                    using (StreamWriter write = new StreamWriter(filePathCBF, true))
                                    {
                                        write.WriteLine($"{dataCustomer[0]};{dataFlight[1]};{dataFlight[0]};{numOfTickets};{dataFlight[3]};{dataFlight[4]};{dataFlight[5]};{dataFlight[6]}");
                                    }
                                }
                                // Cập nhật số ghế trong danh sách chuyến bay
                                flight[i] = string.Join(";", dataFlight);
                                File.WriteAllLines(filePathFl, flight);

                                Console.WriteLine($"\n {new string(' ', 30)} Bạn đã đặt {numOfTickets} vé cho Chuyến bay \"{flightNo.ToUpper()}\"...");
                            }
                            else
                            {
                                Console.WriteLine($"Không đủ ghế trống cho Chuyến bay \"{flightNo.ToUpper()}\"...");
                            }
                            break; // Thoát khỏi vòng lặp sau khi xử lý xong
                        }
                    }
                }

            }
            if (!isFound)
            {
                Console.WriteLine($"Số hiệu không hợp lệ...! Không tìm thấy chuyến bay với ID \"{flightNo}\"...");
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
            string filePathCBF = Path.Combine(datatxt, "CustomerBOOKFLIGHT.txt");
            string filePathFHC = Path.Combine(datatxt, "FlightHasCustomers.txt");

            string[] flight = File.ReadAllLines(filePathFl);
            List<string> customerBF = File.ReadAllLines(filePathCBF).ToList();
            List<string> FlightHC = File.ReadAllLines(filePathFHC).ToList();

            string flightNum;
            bool isFound = false;

            Console.WriteLine($"{new string(' ', 30)}++++++++++++++ Đây là danh sách tất cả các chuyến bay bạn đã đăng ký ++++++++++++++");
            DisplayFlightsRegisteredByOneUser(userID);
            Console.WriteLine("Nhập sô hiệu của chuyến bay bạn muốn hủy:");
            flightNum = Console.ReadLine().ToUpper();
            Console.WriteLine("Nhập số lượng vé muốn hủy:");
            int numOfTickets;
			while (!int.TryParse(Console.ReadLine(), out numOfTickets))
			{
				Console.Write("Vui lòng nhập số vé hợp lệ:  ");
			}
			//Customer.txt
            for(int i=0; i<customerBF.Count; i++)
            {
                string[] dataCBF = customerBF[i].Split(';');
                if (dataCBF[0].Equals(userID))
                {
                    if (dataCBF[1].Equals(flightNum))
                    {
                        isFound = true;
                        //FlightScheduler.txt
                        //chỉnh sửa số vé được hoàn trả
                        for (int j = 0; j < flight.Length; j++)
                        {
                            string[] dataFlight = flight[j].Split(';');
                            int numOfTicketsForFlight = int.Parse(dataCBF[3]);

                            if (dataCBF[1].Equals(dataFlight[1]))
                            {
                                while (numOfTickets > numOfTicketsForFlight)
                                {
                                    Console.Write($"LỖI!!! Số vé không thể lớn hơn {numOfTicketsForFlight} cho chuyến bay này. Vui lòng nhập lại số lượng vé:");
                                    while (!int.TryParse(Console.ReadLine(), out numOfTickets))
                                    {
                                        Console.Write("Vui lòng nhập số vé hợp lệ:  ");
                                    }
                                }
                                int ticketsToBeReturned;

                                for(int h = 0; h<FlightHC.Count; h++)
                                {
                                    string[] dataFHC = FlightHC[h].Split(';');
                                    if (userID.Equals(dataFHC[1]) && flightNum.Equals(dataFHC[0]))
                                    {
                                        if (numOfTicketsForFlight == numOfTickets)
                                        {
                                            ticketsToBeReturned = int.Parse(dataFlight[2]) + numOfTicketsForFlight;
                                            dataFlight[2] = Convert.ToString(ticketsToBeReturned);
                                            //nếu số vé cần hủy bằng với số vé hiện có thì khách hàng đó ra khỏi 2 file
                                            customerBF.RemoveAt(i);
                                            FlightHC.RemoveAt(h);
                                            Console.Write($"Đã hủy hết vé của chuyến bay {flightNum} thành công.");
                                        }
                                        else
                                        {
                                            ticketsToBeReturned = numOfTickets + int.Parse(dataFlight[2]);
                                            //cập nhật số vé ở FHC và CBF
                                            dataCBF[3] = Convert.ToString(numOfTicketsForFlight - numOfTickets);
                                            dataFHC[2] = dataCBF[3];
                                            FlightHC[h] = string.Join(";", dataFHC);
                                            customerBF[i] = string.Join(";", dataCBF);

                                            Console.Write($"Đã hủy {numOfTickets} vé của chuyến bay {flightNum} thành công.");
                                        }
                                        dataFlight[2] = Convert.ToString(ticketsToBeReturned);
                                        //cập nhật số vé có trong FlightScheduler.txt
                                        flight[j] = string.Join(";", dataFlight);
                                        break;
                                    }
                                }
                                break;
                            }
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
                File.WriteAllLines(filePathCBF, customerBF);
                File.WriteAllLines(filePathFHC, FlightHC);
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
                if (dataFlight[1].Equals(fightNo) && dataFlight[9]=="1")
                {
                    isFlightAvailable = true;
                    break;
                }
            }
            return isFlightAvailable ? "Theo Lịch Trình" : "   Hủy Bỏ      ";
        }

        public void DisplayFlightsRegisteredByOneUser(string userID)
        {
			Console.OutputEncoding = Encoding.Unicode;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePathCBF = Path.Combine(datatxt, "CustomerBOOKFLIGHT.txt");

            string[] UserHasFlights = File.ReadAllLines(filePathCBF);

            //in ra console user có những chuyến bay nào
            Console.WriteLine();
            Console.Write("+------+-------------------------------------------+-----------+------------------+-----------------------+------------------------+-----------------------------+-------------+--------+------------------+\n");
            Console.WriteLine("| STT  | LỊCH BAY\t\t\t\t   | MÃ CHUYẾN |  Số vé đã đặt    | \tTừ ====>>         | \t====>> Đến\t   | \t  THỜI GIAN HẠ CÁNH      |THỜI GIAN BAY|  CỔNG  |  TRẠNG THÁI      |");
            Console.Write("+------+-------------------------------------------+-----------+------------------+-----------------------+------------------------+-----------------------------+-------------+--------+------------------+\n");

            int stt = 0;

            for (int i = 0; i < UserHasFlights.Length; i++)
            {
                string[] dataUserHasFlight = UserHasFlights[i].Split(';');
                if (userID.Equals(dataUserHasFlight[0]))
                {
                    Flight fl = new Flight();
                    Console.WriteLine($"| {stt + 1,-5}| {dataUserHasFlight[2],-41} | {dataUserHasFlight[1],-9} | \t{dataUserHasFlight[3],-9} | {dataUserHasFlight[4],-21} | {dataUserHasFlight[5],-22} | {fl.FetchArrivalTime(dataUserHasFlight[2], dataUserHasFlight[6]),-27} | {dataUserHasFlight[6],-11} | {dataUserHasFlight[7],-6} | {FlightStatus(dataUserHasFlight[1]),-17}|");
                    Console.Write("+------+-------------------------------------------+-----------+------------------+-----------------------+------------------------+-----------------------------+-------------+--------+------------------+\n");
                    stt++;
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

            string filePathFHC = Path.Combine(datatxt, "FlightHasCustomers.txt");
            string filePathCustomer = Path.Combine(datatxt, "Customer.txt");
            string filePathFlight = Path.Combine(datatxt, "FlightScheduler.txt");

            string[] flight = File.ReadAllLines(filePathFlight);
            string[] FlightHasCustomers = File.ReadAllLines(filePathFHC);
            string[] Customer = File.ReadAllLines(filePathCustomer);

            // Tạo Dictionary để nhóm theo tên chuyến bay
            Dictionary<string, List<string[]>> flightGroups = new Dictionary<string, List<string[]>>();

            foreach (string line in FlightHasCustomers)
            {
                string[] data = line.Split(';');
                string flightName = data[0]; // Sử dụng tên chuyến bay làm key

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
                foreach (var dataFHC in customerDataList)
                {
                    for(int j=0; j<Customer.Length; j++)
                    {
                        string[] dataCustomer = Customer[j].Split(';');
                        if ( dataFHC[1].Equals(dataCustomer[0]))
                        {
                            for (int i = 0; i < flight.Length; i++)
                            {
                                string[] dataFlight = flight[i].Split(';');

                                if (dataFHC[0].Equals(dataFlight[1]) && dataFlight[9] == "1")
                                {
                                    if (shouldDisplayHeader)
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine($" Mã chuyến bay: {flightName}");
                                        Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+");
                                        Console.WriteLine($"{new string(' ', 10)}| STT         | Mã khách hàng | Tên khách hàng                   | Tuổi    | Email  \t\t\t   | Địa chỉ\t\t\t    | Số điện thoại\t      | Số vé đã đặt |");
                                        Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+");
                                        shouldDisplayHeader = false; // Đặt flag để không hiển thị header nữa
                                    }
                                    // In thông tin của mỗi khách hàng trong nhóm
                                    Console.WriteLine($"{new string(' ', 10)}| {stt + 1,-11} | {dataCustomer[0],-13} | {dataCustomer[1],-32} | {dataCustomer[6],-7} | {dataCustomer[2],-27} | {dataCustomer[5],-30} | {dataCustomer[4],-23} | {dataFHC[2],-12} |");
                                    Console.WriteLine($"{new string(' ', 10)}+-------------+---------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+");
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
            string filePathFHC = Path.Combine(datatxt, "FlightHasCustomers.txt");
            string filePathFlight = Path.Combine(datatxt, "FlightScheduler.txt");
            string filePathCustomer = Path.Combine(datatxt, "Customer.txt");

            string[] Customer = File.ReadAllLines(filePathCustomer);
            string[] Flight = File.ReadAllLines(filePathFlight);
            string[] FlightHasCustomers = File.ReadAllLines(filePathFHC);

            for (int j = 0; j < Flight.Length; j++)
            {
                string[] dataF = Flight[j].Split(';');
                if (dataF[9] == "1" && flightNum.Equals(dataF[1]))
                {
                    Console.WriteLine();
                    Console.WriteLine($"\n{new string('+', 30)} Hiển thị Khách hàng đã đăng ký cho Chuyến bay số \"{flightNum,-6}\" {new string('+', 30)}\n");
                    Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+-----------+");
                    Console.WriteLine($"{new string(' ', 10)}| Mã chuyến bay  |Mã khách hàng| Tên khách hàng                   | Tuổi    | Email  \t\t\t    | Địa chỉ\t\t\t     | Số điện thoại\t       | Số vé đã đặt | Tổng tiền |");
                    Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+-----------+");
                    break;
                }
            }
            bool isFound = false;
            //
            for (int i = 0; i < FlightHasCustomers.Length; i++)
            {
                string[] dataFHC = FlightHasCustomers[i].Split(';');
                //
                for (int j = 0; j < Flight.Length; j++)
                {
                    string[] dataF = Flight[j].Split(';');

                    if (dataF[1].Equals(flightNum) && dataF[9] == "1" && flightNum.Equals(dataFHC[0]))
                    {
                        isFound = true;
                        //
                        for (int k = 0; k < Customer.Length; k++)
                        {
                            string[] dataCustomer = Customer[k].Split(';');

                            if (dataFHC[1].Equals(dataCustomer[0]) && dataCustomer[7] == "1")
                            {
                                Console.WriteLine($"{new string(' ', 10)}| {flightNum,-14} | {dataCustomer[0],-11} | {dataCustomer[1],-32} | {dataCustomer[6],-7} | {dataCustomer[2],-27} | {dataCustomer[5],-30} | {dataCustomer[4],-23} | {dataFHC[2],-12} |");
                                Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+");
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
