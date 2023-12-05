using System;
using System.Collections.Generic;
using System.Data;
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
        // Fields
        Flight flight = new Flight();
        int flightIndexInFlightList;      

        public void BookFlight(string flightNo, int numOfTickets, string userID)
        {
			Console.OutputEncoding = Encoding.Unicode;
			bool isFound = false;
            bool checkFlightHasCustomer = false;
            
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
            string[] FlightHasCustomers = File.ReadAllLines(filePathFlightHasCustomers);
            //string[] UserHasFlights = File.ReadAllLines(filePathCBF);

            for (int i=0; i<flight.Length; i++)
            {
                string[] dataFlight = flight[i].Split(';');
                if ( dataFlight[1].Equals(flightNo))
                {
                    for(int j=0; j < customer.Length; j++)
                    {
                        string[] dataCustomer = customer[j].Split(';');
                        if ( dataCustomer.Length == 7 && dataCustomer[0].Equals(userID))
                        {
                            isFound = true;
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
                                        dataFlightHasCustomers[8] = Convert.ToString(int.Parse(dataFlightHasCustomers[8]) + numOfTickets);
                                        FlightHasCustomers[f] = string.Join(";", dataFlightHasCustomers);
                                        File.WriteAllLines(filePathFlightHasCustomers, FlightHasCustomers);
                                        break;
                                    }
                                }
                                if (!checkFlightHasCustomer)
                                {
                                    using (StreamWriter write = new StreamWriter(filePathFlightHasCustomers, true))
                                    {
                                        write.WriteLine($"{flightNo};{userID};{dataCustomer[1]};{dataCustomer[2]};{dataCustomer[3]};{dataCustomer[4]};{dataCustomer[5]};{dataCustomer[6]};{numOfTickets}");
                                    }
                                }
                                // User Has Flight
                                using (StreamWriter write = new StreamWriter(filePathCBF, true))
                                {
                                    write.WriteLine($"{dataCustomer[0]};{dataFlight[1]};{dataFlight[0]};{numOfTickets};{dataFlight[3]};{dataFlight[4]};{dataFlight[5]};{dataFlight[6]};{dataFlight[7]}");
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
                Console.WriteLine($"Flight Number không hợp lệ...! Không tìm thấy chuyến bay với ID \"{flightNo}\"...");

            }
        }

        public void CancelFlight(string userID)
        {
			Console.OutputEncoding = Encoding.Unicode;
			string flightNum = "";
            DisplayFlightsRegisteredByOneUser(userID);
            Console.WriteLine("Nhập Flight Number của chuyến bay bạn muốn hủy:");

            flightNum = Console.ReadLine();
            Console.WriteLine("Nhập số lượng vé muốn hủy:");
            int numOfTickets;
			while (!int.TryParse(Console.ReadLine(), out numOfTickets))
			{
				Console.Write("Vui lòng nhập số vé hợp lệ:  ");
			}
			bool isFound = false;
            int index = 0;
            foreach (Customer customer in Customer.customerCollection)
            {
                if (userID.Equals(customer.GetUserID()))
                {
                    if (customer.flightsRegisteredByUser.Count != 0)
                    {
                        Console.WriteLine($"{new string(' ', 30)}++++++++++++++ Đây là danh sách tất cả các chuyến bay bạn đã đăng ký ++++++++++++++");
                        DisplayFlightsRegisteredByOneUser(userID);

                        foreach (Flight flight in customer.flightsRegisteredByUser)
                        {
                            if (flightNum.Equals(flight.FlightNumber, StringComparison.OrdinalIgnoreCase))
                            {
                                isFound = true;
                                int numOfTicketsForFlight = customer.numOfTicketsBookedByUser[index];

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
                                    ticketsToBeReturned = flight.NoOfSeats + numOfTicketsForFlight;
                                    customer.numOfTicketsBookedByUser.RemoveAt(index);
                                    customer.flightsRegisteredByUser.RemoveAt(index);
                                }
                                else
                                {
                                    ticketsToBeReturned = numOfTickets + flight.NoOfSeats;
                                    customer.numOfTicketsBookedByUser[index] = numOfTicketsForFlight - numOfTickets;
                                }

                                flight.numOfSeatsInTheFlight = ticketsToBeReturned;
                                break;
                            }
                            index++;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Bạn chưa đăng ký chuyến bay nào với ID \"{flightNum.ToUpper()}\".....");
                    }

                    if (!isFound)
                    {
                        Console.WriteLine($"LỖI!!! Không thể tìm thấy chuyến bay với ID \"{flightNum.ToUpper()}\".....");
                    }
                }
            }
        }

        void AddNumberOfTicketsToAlreadyBookedFlight(Customer customer, int numOfTickets)
        {
            int newNumOfTickets = customer.GetNumOfTicketsBookedByUser()[flightIndexInFlightList] + numOfTickets;
            customer.GetNumOfTicketsBookedByUser()[flightIndexInFlightList] = newNumOfTickets;
        }

        void AddNumberOfTicketsForNewFlight(Customer customer, int numOfTickets)
        {
            customer.GetNumOfTicketsBookedByUser().Add(numOfTickets);
        }

        bool IsFlightAlreadyAddedToCustomerList(List<Flight> flightList, Flight flight)
        {
            bool addedOrNot = false;
            foreach (Flight flight1 in flightList)
            {
                if (flight1.FlightNumber.Equals(flight.FlightNumber, StringComparison.OrdinalIgnoreCase))
                {
                    this.flightIndexInFlightList = flightList.IndexOf(flight1);
                    addedOrNot = true;
                    break;
                }
            }
            return addedOrNot;
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
                if (dataFlight[1].Equals(fightNo))
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

            // Sử dụng Dictionary để lưu trữ số vé theo cặp userID và flightID
            Dictionary<string, int> userFlightTicketCounts = new Dictionary<string, int>();

            //tính tổng số vé nếu user đó có đặt nhiều lần cùng 1 chuyến bay
            for(int i=0; i < UserHasFlights.Length; i++)
            {
                string[] dataUserHasFlight = UserHasFlights[i].Split(';');
                if (userID.Equals(dataUserHasFlight[0]))
                {
                    int numberOfTickets = int.Parse(dataUserHasFlight[3]);

                    // Tạo key là sự kết hợp của userID và flightID
                    string key = $"{userID};{dataUserHasFlight[1]}";

                    // Kiểm tra xem đã có cặp userID và flightID trong Dictionary hay chưa
                    if (userFlightTicketCounts.ContainsKey(key))
                    {
                        userFlightTicketCounts[key] += numberOfTickets;
                    }
                    else
                    {
                        // Nếu chưa tồn tại, thêm mới vào Dictionary
                        userFlightTicketCounts[key] = numberOfTickets;
                    }
                }
            }
            //in ra console user có những chuyến bay nào
            Console.WriteLine();
            Console.Write("+------+-------------------------------------------+-----------+------------------+-----------------------+------------------------+-----------------------------+-------------+--------+------------------+\n");
            Console.WriteLine("| STT  | LỊCH BAY\t\t\t\t   | MÃ CHUYẾN |  Số vé đã đặt    | \tTừ ====>>         | \t====>> Đến\t   | \t  THỜI GIAN HẠ CÁNH      |THỜI GIAN BAY|  CỔNG  |  TRẠNG THÁI      |");
            Console.Write("+------+-------------------------------------------+-----------+------------------+-----------------------+------------------------+-----------------------------+-------------+--------+------------------+\n");

            int stt = 0;
            foreach (var entry in userFlightTicketCounts)
            {
                string[] ids = entry.Key.Split(';');
                for (int i = 0; i < UserHasFlights.Length; i++)
                {
                    string[] dataUserHasFlight = UserHasFlights[i].Split(';');
                        if (ids[1].Equals(dataUserHasFlight[1]))
                        {
                            Console.WriteLine($"|{stt + 1,-6}| {dataUserHasFlight[2],-41} | {ids[1],-9} | \t{entry.Value,-9} | {dataUserHasFlight[4],-21} | {dataUserHasFlight[5],-22} | {dataUserHasFlight[6],-27} | {dataUserHasFlight[7],-11} | {dataUserHasFlight[8],-6} | {FlightStatus(ids[1]),-17}|");
                            Console.Write("+------+-------------------------------------------+-----------+------------------+-----------------------+------------------------+-----------------------------+-------------+--------+------------------+\n");
                            break;
                        }
                }
                stt++;
            }
            
            if (stt == 0)
            {
                Console.WriteLine($"Không có chuyến bay được đăng ký cho người dùng với ID {userID}.");
            }

            /*
			foreach (Customer customer in Customer.customerCollection)
			{
				if (userID.Equals(customer.GetUserID()))
				{
					List<Flight> flights = customer.GetFlightsRegisteredByUser();
					int size = flights.Count;

					if (size > 0)
					{
						flightsFound = true;

						for (int i = 0; i < size; i++)
						{
							Console.WriteLine(ToString((i + 1), flights[i], customer));
							Console.Write("+------+-------------------------------------------+-----------+------------------+-----------------------+------------------------+-----------------------------+-------------+--------+-----------------+\n");
						}
					}
				}
			}
            if (!flightsFound)
			{
                Console.WriteLine($"Không có chuyến bay được đăng ký cho người dùng với ID {userID}.");
            }
            */

        }
        public string ToString(int serialNum, Customer customer, int index)
        {
            return string.Format("          | {0,-10} | {1,-10}  | {2,-32} | {3,-7} | {4,-27} | {5,-35} | {6,-23} |       {7,-7}  |", (serialNum + 1), customer.RandomIDDisplay(customer.GetUserID()), customer.GetName(),
                customer.GetAge(), customer.GetEmail(), customer.GetAddress(), customer.GetPhone(), customer.numOfTicketsBookedByUser[index]);
        }
        public void DisplayRegisteredUsersForAllFlight()
        {
            Console.WriteLine();
            Console.WriteLine($"\n{new string('+', 30)} Hiển thị tất cả chuyến bay đã được đăng ký\" {new string('+', 30)}\n");
            Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+");
            Console.WriteLine($"{new string(' ', 10)}| Mã chuyến bay  |Mã khách hàng| Tên khách hàng                   | Tuổi    | Email  \t\t\t    | Địa chỉ\t\t\t     | Số điện thoại\t       | Số vé đã đặt |");
            Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+");

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");
            string filePathCBF = Path.Combine(datatxt, "FlightHasCustomers.txt");
            string[] FlightHasCustomers = File.ReadAllLines(filePathCBF);

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

                // In thông tin từng nhóm
                foreach (var customerData in customerDataList)
                {
                    // In thông tin của mỗi khách hàng trong nhóm
                    Console.WriteLine($"{new string(' ', 10)}| {customerData[0],-14} | {customerData[1],-11} | {customerData[2],-32} | {customerData[7],-7} | {customerData[3],-27} | {customerData[6],-30} | {customerData[5],-23} | {customerData[8],-12} |");
                    Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+");
                }
            }
        }
        public void DisplayRegisteredUsersForASpecificFlight(string flightNum)
        {
            Console.WriteLine();
            Console.WriteLine($"\n{new string('+', 30)} Hiển thị Khách hàng đã đăng ký cho Chuyến bay số \"{flightNum,-6}\" {new string('+', 30)}\n");
            Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+");
            Console.WriteLine($"{new string(' ', 10)}| Mã chuyến bay  |Mã khách hàng| Tên khách hàng                   | Tuổi    | Email  \t\t\t    | Địa chỉ\t\t\t     | Số điện thoại\t       | Số vé đã đặt |");
            Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+");

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");
            string filePathCBF = Path.Combine(datatxt, "FlightHasCustomers.txt");
            string[] FlightHasCustomers = File.ReadAllLines(filePathCBF);
            for(int i = 0; i<FlightHasCustomers.Length; i++)
            {
                string[] data = FlightHasCustomers[i].Split(';');
                if (flightNum.Equals(data[0]))
                {
                    Console.WriteLine($"{new string(' ', 10)}| {data[0],-14} | {data[1],-11} | {data[2],-32} | {data[7],-7} | {data[3],-27} | {data[6],-30} | {data[5],-23} | {data[8],-12} |");
                    Console.WriteLine($"{new string(' ', 10)}+----------------+-------------+----------------------------------+---------+-----------------------------+--------------------------------+-------------------------+--------------+");
                }
            }
        }
        int FlightIndex(List<Flight> flightList, Flight flight)
        {
            int index = -1;
            for (int i = 0; i < flightList.Count; i++)
            {
                if (flightList[i].Equals(flight))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        public void DisplayArtWork(int option)
        {
			Console.OutputEncoding = Encoding.Unicode;
			string artWork = string.Empty;

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
