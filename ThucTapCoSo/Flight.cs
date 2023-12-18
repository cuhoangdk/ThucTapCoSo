﻿using System;
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
        private readonly string fromWhichCity;
        private string gate;
        private readonly string toWhichCity;
        private readonly double distanceInMiles;
        private readonly double distanceInKm;
        private readonly string flightTime;
        public int numOfSeatsInTheFlight;
        //private readonly List<Customer> listOfRegisteredCustomersInAFlight;
        //private int customerIndex;
        private static int nextFlightDay = 0;

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
            //this.listOfRegisteredCustomersInAFlight = new List<Customer>();
            this.gate = gate;
        }
        public void FlightHistory()
        {
            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");
            string flightAddPath = Path.Combine(datatxt, "FlightHistory.txt");
        }
        public void AddFlight(string idAdmin, DateTime date)
        {
            RandomGenerator r1 = new RandomGenerator();

            string[][] chosenDestinations = r1.SpecificallyDestinations();
            int flag = 1;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string fileHistory = Path.Combine(datatxt, "FlightHistory.txt");
            string filePath = Path.Combine(datatxt, "FlightScheduler.txt");


            if(double.TryParse(chosenDestinations[0][1], out double latitude1) &&
               double.TryParse(chosenDestinations[0][2], out double longitude1) &&
               double.TryParse(chosenDestinations[1][1], out double latitude2) &&
               double.TryParse(chosenDestinations[1][2], out double longitude2))
            {
                string[] distanceBetweenTheCities = CalculateDistance(latitude1, longitude1, latitude2, longitude2);

                string flightSchedule = CreateNewFlightsAndTime();
                string flightNumber = r1.RandomFlightNumbGen(2, 1).ToUpper();
                Console.Write("SỐ GHẾ HẠNG PHỔ THÔNG    : ");
                int numOfSeatsInTheFlightECO;
                while (!int.TryParse(Console.ReadLine(), out numOfSeatsInTheFlightECO) || numOfSeatsInTheFlightECO < 75 || numOfSeatsInTheFlightECO > 500)
                {
                    Console.Write("LỖI!! Vui lòng nhập số ghế đúng định dạng (ít nhất 75 ghế và nhiều nhất 500 ghế). Nhập giá trị lại :\t");
                }
                Console.Write("SỐ GHẾ HẠNG THƯƠNG GIA   :  ");
                int numOfSeatsInTheFlightBSN;
                while (!int.TryParse(Console.ReadLine(), out numOfSeatsInTheFlightBSN) || numOfSeatsInTheFlightBSN < 25 || numOfSeatsInTheFlightBSN > 500)
                {
                    Console.Write("LỖI!! Vui lòng nhập số ghế đúng định dạng (ít nhất 75 ghế và nhiều nhất 500 ghế). Nhập giá trị lại :\t");
                }
                string gate = r1.RandomFlightNumbGen(1, 30);
                //
                double distanceInMiles = double.Parse(distanceBetweenTheCities[0]);
                double distanceInKm = double.Parse(distanceBetweenTheCities[1]);
                string flightTime = CalculateFlightTime(distanceInMiles);

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{flag};{flightNumber};{numOfSeatsInTheFlightBSN};{numOfSeatsInTheFlightECO};{flightSchedule};{chosenDestinations[0][0]};{chosenDestinations[1][0]};{flightTime};{gate.ToUpper()};{distanceInMiles};{distanceInKm}");
                }
                using (StreamWriter writer = new StreamWriter(fileHistory, true))
                {
                    writer.WriteLine($"{date};ADD;{idAdmin};{flightNumber};{flightSchedule};{numOfSeatsInTheFlightECO};{chosenDestinations[0][0]};{chosenDestinations[1][0]};{flightTime};{gate.ToUpper()};{distanceInMiles};{distanceInKm}");
                }
            }
            else
            {
                // Xử lý trường hợp không thể chuyển đổi thành công
                Console.WriteLine("Lỗi chuyển đổi tọa độ cho chuyến bay " + (flightNumber));
            }
        }
        public void EditFlight(string ID, string idAdmin, DateTime date)
        {
            bool isFound = false;
            Console.WriteLine();

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string flightEditPath = Path.Combine(datatxt, "FlightHistory.txt");
            string filePath = Path.Combine(datatxt, "FlightScheduler.txt");

            string[] line = File.ReadAllLines(filePath);

            for (int i = 0; i < line.Length; i++)
            {                
                string[] data = line[i].Split(';');

                if (ID.Equals(data[1]))
                {
                    isFound = true;

                    using (StreamWriter writer = new StreamWriter(flightEditPath, true))
                    {
                        writer.WriteLine($"{date};EDIT;{idAdmin};{ID}");
                    }

                    RandomGenerator r1 = new RandomGenerator();
                    string[][] chosenDestinations = r1.SpecificallyDestinations();

                    if (double.TryParse(chosenDestinations[0][1], out double latitude1) && 
                        double.TryParse(chosenDestinations[0][2], out double longitude1) && 
                        double.TryParse(chosenDestinations[1][1], out double latitude2) && 
                        double.TryParse(chosenDestinations[1][2], out double longitude2))
                    {
                        string[] distanceBetweenTheCities = CalculateDistance(latitude1, longitude1, latitude2, longitude2);
                        data[5] = chosenDestinations[0][0];
                        data[6] = chosenDestinations[1][0];
                        data[9] = distanceBetweenTheCities[0];
                        data[10] = distanceBetweenTheCities[1];
                        //
                        Console.Write("Nhập số ghế thương gia mới của chuyến bay:\t");
                        int BSNSeats;
                        while (!int.TryParse(Console.ReadLine(), out BSNSeats) || BSNSeats < 10 || BSNSeats > 50)
                        {
                            Console.Write("LỖI!! Vui lòng nhập số ghế đúng định dạng (ít nhất 10 ghế và nhiều nhất 50 ghế). Nhập giá trị lại :\t");
                        }
                        data[2] = BSNSeats.ToString();
                        //
                        int ECOSeats;
                        Console.Write("Nhập số ghế phổ thông mới của chuyến bay:\t");
                        while (!int.TryParse(Console.ReadLine(), out ECOSeats) || ECOSeats < 75 || ECOSeats > 500)
						{
							Console.Write("LỖI!! Vui lòng nhập số ghế đúng định dạng (ít nhất 75 ghế và nhiều nhất 500 ghế). Nhập giá trị lại :\t");
						}
						data[3] = ECOSeats.ToString();

                        Console.Write("Nhập cổng mới cho chuyến bay:\t");
                        data[8] = Console.ReadLine();
                        data[7] = CalculateFlightTime(double.Parse(data[9]));
                    }
                    line[i] = string.Join(";", data);
                }
            }
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
        //public void AddNewCustomerToFlight(Customer customer)
        //{
        //    this.listOfRegisteredCustomersInAFlight.Add(customer);
        //}
        //public void AddTicketsToExistingCustomer(Customer customer, int numOfTickets)
        //{
        //    customer.AddExistingFlightToCustomerList(customerIndex, numOfTickets);
        //}
        //public bool IsCustomerAlreadyAdded(List<Customer> customersList, Customer customer)
        //{
        //    bool isAdded = false;
        //    foreach (Customer customer1 in customersList)
        //    {
        //        if (customer1.GetUserID().Equals(customer.GetUserID()))
        //        {
        //            isAdded = true;
        //            customerIndex = customersList.IndexOf(customer1);
        //            break;
        //        }
        //    }
        //    return isAdded;
        //}

        
        public void HiddenFlight(string flightNumber, string idAdmin, DateTime date)
        {
			Console.OutputEncoding = Encoding.Unicode;
			bool isFound = false;
            int flag;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");
            string filePath = Path.Combine(datatxt, "FlightScheduler.txt");
            string fileDeletePath = Path.Combine(datatxt, "FlightHistory.txt");

            List<string> lines = File.ReadAllLines(filePath).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                string[] data = lines[i].Split(';');

                if (flightNumber.Equals(data[1]))
                {
                    // Nếu ID khớp, gắn flag là 0 để ẩn chuyến bay
                    flag = 0;
                    data[0] = Convert.ToString(flag);
                    lines[i] = string.Join(";", data);
                    isFound = true;
                    break;
                }
            }
            if (isFound)
            {
                File.WriteAllLines(filePath, lines);
                using (StreamWriter writer = new StreamWriter(fileDeletePath, true))
                {
                    writer.WriteLine($"{date};REMOVE;{idAdmin};{flightNumber}");
                }
                Console.WriteLine($"{new string(' ', 10)}Đã xóa chuyến bay với Flight NO: {flightNumber}."); // FIX
            }
            else
            {
                Console.WriteLine($"{new string(' ', 10)}Không tìm thấy chuyến bay với Flight NO: {flightNumber}...!!!"); // FIX
            }
            DisplayFlightSchedule();
        }

        public float CalculatePrice(string mile)
        {
            //giá trung bình 0.1$/mile
            float price = (float)Math.Round((double.Parse(mile) * 0.10),2);
            return price;
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
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

			Console.WriteLine();
            Console.Write("+------+---------------------------+-------------+------------------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+----------+\n");
            Console.Write("| STT  | THỜI GIAN CẤT CÁNH        | MÃ CHUYẾN   | SỐ GHẾ TRỐNG                 | KHỞI HÀNH             | ĐIẾM ĐẾN               | THỜI GIAN HẠ CÁNH         |THỜI GIAN BAY|  CỔNG  | QUÃNG ĐƯỜNG(MILES/KMS) | GIÁ VÉ $ |\n");
            Console.Write("+------+---------------------------+-------------+------------------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+----------+\n");

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            //tìm folder datatxt: nơi lưu dữ liệu
            string datatxt = Path.Combine(current, "datatxt");

            string filePath = Path.Combine(datatxt, "FlightScheduler.txt");
            List<string> lines = File.ReadAllLines(filePath).ToList();
            int stt = 1;
            for (int i = 0; i < lines.Count; i++)
            {
                string[] data = lines[i].Split(';');
                if(data[0] == "0")
                {
                    continue;
                }
                Console.WriteLine($"| {stt,-4} | {data[4],-25} | {data[1],-11} | BSN: {data[2],-3} / ECO: {data[3],-3}          | {data[5],-21} | {data[6],-22} | {FetchArrivalTime(data[4],data[7]),-25} | {data[7],6}  Hrs | {data[8],-6} | {data[9],-9} / {data[10],-10} | {CalculatePrice(data[9]),-8} |");
                Console.Write("+------+---------------------------+-------------+------------------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+----------+\n");
                stt++;
            }
        }
		public string CreateNewFlightsAndTime()
        {
            Random random = new Random();
            DateTime currentDate = DateTime.Now;

            // Tăng giá trị của nextFlightDay, để chuyến bay được lên lịch tiếp theo sẽ ở tương lai, không phải trong hiện tại
            nextFlightDay += (int)(random.NextDouble()*7);
            DateTime newDate = currentDate.AddDays(nextFlightDay).AddHours((int)nextFlightDay).AddMinutes(random.Next(0, 45));

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
            return newDate.ToString("ddd, dd/MM/yyyy HH:mm tt");
        }
        public string CalculateFlightTime(double distanceBetweenTheCities)
        {
            double groundSpeed = 450;
            double time = (distanceBetweenTheCities / groundSpeed);

            // Sử dụng Math.Round để làm tròn thời gian
            int hours = (int)Math.Round(time);
            int minutes = (int)((time - hours) * 60 + 20);

            // Làm tròn đến đơn vị thời gian gần nhất chia hết cho 5
            int remainder = minutes % 5;
            if (remainder > 0)
            {
                minutes += 5 - remainder;
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
                DateTime departureDateTime = DateTime.ParseExact(flightSchedule, "ddd, dd/MM/yyyy HH:mm tt", CultureInfo.InvariantCulture);

                string[] duration = flightTime.Split(':');
                int hours = int.Parse(duration[0]);
                int minutes = int.Parse(duration[1]);

                DateTime arrivalTime = departureDateTime.AddHours(hours).AddMinutes(minutes);

                return arrivalTime.ToString("ddd, dd/MM/yyyy HH:mm tt");
            }
            else
            {
                return "N/A";
            }
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
        //public List<Flight> FlightList => flightList;
        //public List<Customer> ListOfRegisteredCustomersInAFlight => listOfRegisteredCustomersInAFlight;
        public string FlightSchedule => flightSchedule;
        public string FromWhichCity => fromWhichCity;
        public string Gate => gate;
        public string ToWhichCity => toWhichCity;
    }
}
