using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    internal class Flight : FlightDistance
    {
        // ************************************************************ Fields ************************************************************

        private readonly string flightSchedule;
        private readonly string flightNumber;
        private readonly string fromWhichCity;
        private readonly string gate;
        private readonly string toWhichCity;
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
            int numOfFlights = 15;
            RandomGenerator r1 = new RandomGenerator();

            for (int i = 0; i < numOfFlights; i++)
            {
                string[][] chosenDestinations = r1.RandomDestinations();

                double latitude1, longitude1, latitude2, longitude2;

                if (double.TryParse(chosenDestinations[0][1], out latitude1) &&
                    double.TryParse(chosenDestinations[0][2], out longitude1) &&
                    double.TryParse(chosenDestinations[1][1], out latitude2) &&
                    double.TryParse(chosenDestinations[1][2], out longitude2))
                {
                    string[] distanceBetweenTheCities = CalculateDistance(latitude1, longitude1, latitude2, longitude2);

                    string flightSchedule = CreateNewFlightsAndTime();
                    string flightNumber = r1.RandomFlightNumbGen(2, 1).ToUpper();
                    int numOfSeatsInTheFlight = r1.RandomNumOfSeats();
                    string gate = r1.RandomFlightNumbGen(1, 30);

                    flightList.Add(new Flight(
                        flightSchedule,
                        flightNumber,
                        numOfSeatsInTheFlight,
                        chosenDestinations,
                        distanceBetweenTheCities,
                        gate.ToUpper()
                    ));
                }
                else
                {
                    // Xử lý trường hợp không thể chuyển đổi thành công
                    Console.WriteLine("Lỗi chuyển đổi tọa độ cho chuyến bay " + (i + 1));
                }
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

        public string FetchArrivalTime()
        {
            DateTime departureDateTime = DateTime.ParseExact(flightSchedule, "dddd, dd MMMM yyyy, HH:mm tt", CultureInfo.InvariantCulture);

            string[] flightTime = FlightTime.Split(':');
            int hours = int.Parse(flightTime[0]);
            int minutes = int.Parse(flightTime[1]);

            DateTime arrivalTime = departureDateTime.AddHours(hours).AddMinutes(minutes);

            return arrivalTime.ToString("ddd, dd-MM-yyyy HH:mm tt");
        }


        public void DeleteFlight(string flightNumber)
        {
			Console.OutputEncoding = Encoding.Unicode;
			Flight foundFlight = null;

            foreach (Flight flight in flightList)
            {
                if (flight.FlightNumber.Equals(flightNumber, StringComparison.OrdinalIgnoreCase))
                {
                    foundFlight = flight;
                    break;
                }
            }

            if (foundFlight != null)
            {
                flightList.Remove(foundFlight);
            }
            else
            {
                Console.WriteLine("Không tìm thấy Chuyến bay với số hiệu đã cho...");
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

            int i = 0;

            foreach (Flight f1 in flightList)
            {
                i++;
                Console.WriteLine(f1.ToString(i));
                Console.Write("+------+-------------------------------------------+-------------+------------------+-----------------------+------------------------+---------------------------+-------------+--------+------------------------+\n");
            }
        }


        public override string ToString(int i)
        {
            return $"| {i,-5}| {flightSchedule,-41} | {flightNumber,-11} | \t{numOfSeatsInTheFlight,-11} | {fromWhichCity,-21} | {toWhichCity,-22} | {FetchArrivalTime(),-10}  |   {flightTime,-6}Hrs |  {gate,-4}  |  {distanceInMiles,-8} / {distanceInKm,-11}|";
        }
		Random random = new Random();
		public string CreateNewFlightsAndTime()
        {            
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

