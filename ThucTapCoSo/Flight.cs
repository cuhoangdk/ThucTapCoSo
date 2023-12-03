using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ThucTapCoSo
{
    internal class Flight : FlightDistance
    {
        // ************************************************************ Fields ************************************************************

        private readonly string flightSchedule;
        private readonly string flightNumber;
        private string fromWhichCity;
        private string gate;
        private string toWhichCity;
        private double distanceInMiles;
        private double distanceInKm;
        private string flightTime;
        public int numOfSeatsInTheFlight;
        private readonly List<Customer> listOfRegisteredCustomersInAFlight;
        private int customerIndex;
        private static int nextFlightDay = 0;
        private static readonly List<Flight> flightList = new List<Flight>();

        // ************************************************************ Behaviours/Methods ************************************************************

        public Flight()
        {
            this.flightSchedule = null;
            this.flightNumber = null;
            this.numOfSeatsInTheFlight = 0;
            this.toWhichCity = null;
            this.fromWhichCity = null;
            this.gate = null;
        }

        public Flight(string flightSchedule, string flightNumber, int numOfSeatsInTheFlight, string[][] chosenDestinations, string[] distanceBetweenTheCities, string gate)
        {
            this.flightSchedule = flightSchedule;
            this.flightNumber = flightNumber;
            this.numOfSeatsInTheFlight = numOfSeatsInTheFlight;
            this.fromWhichCity = chosenDestinations[0][0];
            this.toWhichCity = chosenDestinations[1][0];
            this.distanceInMiles = double.Parse(distanceBetweenTheCities[0]);
            this.distanceInKm = double.Parse(distanceBetweenTheCities[1]);
            this.flightTime = CalculateFlightTime(distanceInMiles);
            this.listOfRegisteredCustomersInAFlight = new List<Customer>();
            this.gate = gate;
        }
        public void FlightScheduler()
        {
            RandomGenerator r1 = new RandomGenerator();

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");
            //tìm tới thư mục FlightScheduler.txt
            string filePath = Path.Combine(datatxt, "FlightScheduler.txt");

            string[][] chosenDestinations = r1.RandomDestinations();
                
            double latitude1, longitude1, latitude2, longitude2;

            if (double.TryParse(chosenDestinations[0][1], out latitude1) && double.TryParse(chosenDestinations[0][2], out longitude1) && double.TryParse(chosenDestinations[1][1], out latitude2) && double.TryParse(chosenDestinations[1][2], out longitude2))
            {
                string[] distanceBetweenTheCities = CalculateDistance(latitude1, longitude1, latitude2, longitude2);
                double distanceInMiles = double.Parse(distanceBetweenTheCities[0]);
                double distanceInKm = double.Parse(distanceBetweenTheCities[1]);

                string flightSchedule = CreateNewFlightsAndTime();
                string flightNumber = r1.RandomFlightNumbGen(2, 1).ToUpper();
                int numOfSeatsInTheFlight = r1.RandomNumOfSeats();
                string gate = r1.RandomFlightNumbGen(1, 30);
                string flightTime = CalculateFlightTime(distanceInMiles);
            }
        }

        public void AddFlight()
        {
            RandomGenerator r1 = new RandomGenerator();

            string[][] chosenDestinations = r1.SpecificallyDestinations();
            int flag = 1;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");
            //tìm tới thư mục FlightScheduler.txt
            string filePath = Path.Combine(datatxt, "FlightScheduler.txt");

            double latitude1, longitude1, latitude2, longitude2;

                if (double.TryParse(chosenDestinations[0][1], out latitude1) &&
                    double.TryParse(chosenDestinations[0][2], out longitude1) &&
                    double.TryParse(chosenDestinations[1][1], out latitude2) &&
                    double.TryParse(chosenDestinations[1][2], out longitude2))
                {
                    string[] distanceBetweenTheCities = CalculateDistance(latitude1, longitude1, latitude2, longitude2);

                    string flightSchedule = CreateNewFlightsAndTime();
                    string flightNumber = r1.RandomFlightNumbGen(2, 1).ToUpper();
                    Console.Write("Nhập vào số ghế ngồi của chuyến bay: ");
                    int numOfSeatsInTheFlight = int.Parse(Console.ReadLine());
                    string gate = r1.RandomFlightNumbGen(1, 30);
                //
                double distanceInMiles = double.Parse(distanceBetweenTheCities[0]);
                double distanceInKm = double.Parse(distanceBetweenTheCities[1]);
                string flightTime = CalculateFlightTime(distanceInMiles);


                flightList.Add(new Flight(
                        flightSchedule,
                        flightNumber,
                        numOfSeatsInTheFlight,
                        chosenDestinations,
                        distanceBetweenTheCities,
                        gate.ToUpper()
                    ));
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{flightSchedule};{flightNumber};{numOfSeatsInTheFlight};{chosenDestinations[0][0]};{chosenDestinations[1][0]};{flightTime};{gate.ToUpper()};{distanceInMiles};{distanceInKm};{flag}");
                }
            }
                else
                {
                    // Xử lý trường hợp không thể chuyển đổi thành công
                    Console.WriteLine("Lỗi chuyển đổi tọa độ cho chuyến bay " + (flightNumber));
                }
        }
        public void EditFlight(string ID)
        {
            bool isFound = false;
            Console.WriteLine();

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");
            //tìm tới thư mục FlightScheduler.txt
            string filePath = Path.Combine(datatxt, "FlightScheduler.txt");

            string[] line = File.ReadAllLines(filePath);

            for (int i = 0; i < line.Length; i++)
            {
                string[] data = line[i].Split(';');

                if (data.Length == 10 && ID.Equals(data[1]))
                {
                    isFound = true;

                    RandomGenerator r1 = new RandomGenerator();
                    string[][] chosenDestinations = r1.SpecificallyDestinations();
                    double latitude1, longitude1, latitude2, longitude2;

                    //data0: userID; data1: name; data2: email; data3: pass; data4: phone; data5: address; data6: age
                    if (double.TryParse(chosenDestinations[0][1], out latitude1) && double.TryParse(chosenDestinations[0][2], out longitude1) && double.TryParse(chosenDestinations[1][1], out latitude2) && double.TryParse(chosenDestinations[1][2], out longitude2))
                    {
                        string[] distanceBetweenTheCities = CalculateDistance(latitude1, longitude1, latitude2, longitude2);
                        data[3] = chosenDestinations[0][0];
                        data[4] = chosenDestinations[1][0];
                        data[7] = distanceBetweenTheCities[0];
                        data[8] = distanceBetweenTheCities[1];
                        Console.Write("Nhập số ghế mới của chuyến bay:\t");
                        data[2] = Console.ReadLine();
                        Console.Write("Nhập cổng mới cho chuyến bay:\t");
                        data[6] = Console.ReadLine();
                        data[5] = CalculateFlightTime(double.Parse(data[7]));
                    }
                    line[i] = string.Join(";", data);
                }
            }
            /*foreach (Flight f in flightList)
            {
                if (ID.Equals(f.flightNumber))
                {
                    RandomGenerator r1 = new RandomGenerator();
                    string[][] chosenDestinations = r1.SpecificallyDestinations();
                    double latitude1, longitude1, latitude2, longitude2;

                    isFound = true;
                    if (double.TryParse(chosenDestinations[0][1], out latitude1) &&
                        double.TryParse(chosenDestinations[0][2], out longitude1) &&
                        double.TryParse(chosenDestinations[1][1], out latitude2) &&
                        double.TryParse(chosenDestinations[1][2], out longitude2))
                    {
                        string[] distanceBetweenTheCities = CalculateDistance(latitude1, longitude1, latitude2, longitude2);
                        f.fromWhichCity = chosenDestinations[0][0];
                        f.toWhichCity = chosenDestinations[1][0];
                        f.distanceInMiles = double.Parse(distanceBetweenTheCities[0]);
                        f.distanceInKm = double.Parse(distanceBetweenTheCities[1]); 
                        Console.Write("Nhập số ghế mới của chuyến bay:\t");
                        f.numOfSeatsInTheFlight = int.Parse(Console.ReadLine());
                        Console.Write("Nhập cổng mới cho chuyến bay:\t");
                        f.gate = Console.ReadLine();
                        f.flightTime = CalculateFlightTime(f.distanceInMiles);
                        break;
                    }
                }
            }*/

            if (!isFound)
            {
                Console.WriteLine($"{new string(' ', 10)}Không tìm thấy chuyến bay với ID {ID} ...!!!"); //FIX
            }
            else
            {
                File.WriteAllLines(filePath, line);
                Console.WriteLine("Cập nhật thông tin thành công!");
            }
        }
        public void AddNewCustomerToFlight(Customer customer)
        {
            this.listOfRegisteredCustomersInAFlight.Add(customer);
        }

        public void AddTicketsToExistingCustomer(Customer customer, int numOfTickets)
        {
            customer.AddExistingFlightToCustomerList(customerIndex, numOfTickets);
        }

        public bool IsCustomerAlreadyAdded(List<Customer> customersList, Customer customer)
        {
            bool isAdded = false;
            foreach (Customer customer1 in customersList)
            {
                if (customer1.GetUserID().Equals(customer.GetUserID()))
                {
                    isAdded = true;
                    customerIndex = customersList.IndexOf(customer1);
                    break;
                }
            }
            return isAdded;
        }

        public string CalculateFlightTime(double distanceBetweenTheCities)
        {
            double groundSpeed = 450;
            double time = (distanceBetweenTheCities / groundSpeed);
            string timeInString = $"{time:F4}";
            string[] timeArray = timeInString.Replace('.', ':').Split(':');
            int hours = int.Parse(timeArray[0]);
            int minutes = int.Parse(timeArray[1])%60;
            int modulus = minutes % 5;

            if (modulus < 3)
            {
                minutes -= modulus;
            }
            else
            {
                minutes += 5 - modulus;
            }

            if (minutes >= 60)
            {
                minutes -= 60;
                hours++;
            }
			return $"{hours:D2}:{minutes:D2}";
		}

        public string FetchArrivalTime(string flightSchedule, string flightTime)
        {

            if (flightSchedule != null)
            {
                DateTime departureDateTime = DateTime.ParseExact(flightSchedule, "dddd, dd MMMM yyyy, HH:mm tt", CultureInfo.InvariantCulture);

                string[] duration = flightTime.Split(':');
                int hours = int.Parse(duration[0]);
                int minutes = int.Parse(duration[1]);

                DateTime arrivalTime = departureDateTime.AddHours(hours).AddMinutes(minutes);

                return arrivalTime.ToString("ddd, dd-MM-yyyy HH:mm tt");
            }
            else
            {
                return "N/A";
            }
        }

        public void DeleteFlight(string flightNumber)
        {
			Console.OutputEncoding = Encoding.Unicode;
			bool isFound = false;
            int flag = 1;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");
            //tìm tới thư mục FlightScheduler.txt
            string filePath = Path.Combine(datatxt, "FlightScheduler.txt");

            //đọc dòng trong file txt và lưu vào list
            List<string> lines = File.ReadAllLines(filePath).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                // Phân tách dữ liệu trong dòng sử dụng dấu chấm phẩy
                string[] data = lines[i].Split(';');

                if (data.Length == 10 && flightNumber.Equals(data[1]))
                {
                    // Nếu ID khớp, gắn flag là 0 để ẩn chuyến bay
                    //lines.RemoveAt(i);
                    flag = 0;
                    data[9] = Convert.ToString(flag);
                    lines[i] = string.Join(";", data);
                    isFound = true;
                    break; // Đã tìm thấy và xóa, không cần kiểm tra các dòng khác
                }
            }

            if (isFound)
            {
                File.WriteAllLines(filePath, lines);
                Console.WriteLine($"{new string(' ', 10)}Đã xóa chuyến bay với Flight NO: {flightNumber}."); // FIX
            }
            else
            {
                Console.WriteLine($"{new string(' ', 10)}Không tìm thấy chuyến bay với Flight NO: {flightNumber}...!!!"); // FIX
            }
            DisplayFlightSchedule();
        }


        public override string[] CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double theta = lon1 - lon2;
            double distance = Math.Sin(DegreeToRadian(lat1)) * Math.Sin(DegreeToRadian(lat2)) + Math.Cos(DegreeToRadian(lat1)) * Math.Cos(DegreeToRadian(lat2)) * Math.Cos(DegreeToRadian(theta));
            distance = Math.Acos(distance);
            distance = RadianToDegree(distance);
            distance = distance * 60 * 1.1515;

            string[] distanceString = new string[3];
            distanceString[0] = $"{distance * 0.8684:F2}";
            distanceString[1] = $"{distance * 1.609344:F2}";
            distanceString[2] = $"{Math.Round(distance * 100.0) / 100.0:F2}";
            return distanceString;
        }

        private double DegreeToRadian(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double RadianToDegree(double rad)
        {
            return (rad * 180.0 / Math.PI);
        }

        public void DisplayFlightSchedule()
        {
			Console.OutputEncoding = Encoding.Unicode;
			Console.WriteLine();
            Console.Write("+------+-------------------------------------------+-------------+------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+\n");
            Console.Write("| STT  | Lịch chuyến bay\t\t\t   |Mã chuyến bay| Số ghế trống     | \tTỪ ====>>           | \t====>> ĐẾN\t     | \t   THỜI GIAN HẠ CÁNH     |THỜI GIAN BAY|  CỔNG  | QUÃNG ĐƯỜNG(MILES/KMS) |\n");
            Console.Write("+------+-------------------------------------------+-------------+------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+\n");

            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");
            //tìm tới thư mục FlightScheduler.txt
            string filePath = Path.Combine(datatxt, "FlightScheduler.txt");
            List<string> lines = File.ReadAllLines(filePath).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                string[] data = lines[i].Split(';');
                if(data.Length == 10 && data[9]!="0")
                {
                    string stt = (i + 1).ToString();
                    string flightSchedule = data[0];
                    string flightNumber = data[1];
                    string numOfSeats = data[2];
                    string fromCity = data[3];
                    string toCity = data[4];
                    string flightTime = data[5];
                    string gate = data[6];
                    string distanceMiles = data[7];
                    string distanceKm = data[8];

                    Console.WriteLine($"| {stt,-4} | {flightSchedule,-41} | {flightNumber,-11} | {numOfSeats,-16} | {fromCity,-21} | {toCity,-22} | {FetchArrivalTime(flightSchedule,flightTime),-25} | {flightTime,6}  Hrs | {gate,-6} | {distanceMiles,-9} / {distanceKm,-10} |");
                    Console.Write("+------+-------------------------------------------+-------------+------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+\n");
                }
            }

            /*int i = 0;

            foreach (Flight f1 in flightList)
            {
                i++;
                Console.WriteLine(f1.ToString(i));
                Console.Write("+------+-------------------------------------------+-------------+------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+\n");
            }*/
        }


        public override string ToString(int i)
        {
            return $"| {i,-5}| {flightSchedule,-41} | {flightNumber,-11} | \t{numOfSeatsInTheFlight,-11} | {fromWhichCity,-21} | {toWhichCity,-22} | {FetchArrivalTime(flightSchedule, flightTime),-10}  |   {flightTime,-6}Hrs |  {gate,-4}  |  {distanceInMiles,-8} / {distanceInKm,-11}|";
        }
		public string CreateNewFlightsAndTime()
        {
            Random random = new Random();
            DateTime currentDate = DateTime.Now;

            // Tăng giá trị của nextFlightDay, để chuyến bay được lên lịch tiếp theo sẽ ở tương lai, không phải trong hiện tại
            nextFlightDay += (int)(random.NextDouble()*7);
            DateTime newDate = currentDate.AddDays(nextFlightDay)
                                        .AddHours((int)nextFlightDay)
                                        .AddMinutes(random.Next(0, 45));

            // Làm tròn số phút đến phút gần nhất của một phần tư
            int mod = newDate.Minute % 15;
            if (mod < 8)
            {
                newDate = newDate.AddMinutes(-mod);
            }
            else
            {
                newDate = newDate.AddMinutes(15 - mod);
            }
            return newDate.ToString("dddd, dd MMMM yyyy, HH:mm tt");
        }
        public DateTime GetNearestHourQuarter(DateTime datetime)
        {
            int minutes = datetime.Minute;
            int mod = minutes % 15;
            DateTime newDatetime;
            if (mod < 8)
            {
                newDatetime = datetime.AddMinutes(-mod);
            }
            else
            {
                newDatetime = datetime.AddMinutes(15 - mod);
            }
            return newDatetime;
        }

        //************************************************************ Setters & Getters ************************************************************

        public int NoOfSeats => numOfSeatsInTheFlight;
        public string FlightNumber => flightNumber;
        public void SetNoOfSeatsInTheFlight(int numOfSeatsInTheFlight) => this.numOfSeatsInTheFlight = numOfSeatsInTheFlight;
        public string FlightTime => flightTime;
        public List<Flight> FlightList => flightList;
        public List<Customer> ListOfRegisteredCustomersInAFlight => listOfRegisteredCustomersInAFlight;
        public string FlightSchedule => flightSchedule;
        public string FromWhichCity => fromWhichCity;
        public string Gate => gate;
        public string ToWhichCity => toWhichCity;
    }
}

