using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    internal class RandomGenerator
    {
		// ************************************************************ Fields ************************************************************
		Random rand = new Random();
	    	private string randomNum;

        /* Tên thành phố nằm ở chỉ số 0, vĩ độ của thành phố ở chỉ số 1 và kinh độ ở chỉ số 2 */
        private static readonly string[][] destinations =
        {
        new[] {"Karachi", "24.871940", "66.988060","Pakistan","International"},
        new[] {"Bangkok", "13.921430", "100.595337", "Thailand", "International"},
        new[] {"Jakarta", "-6.174760", "106.827072", "Indonesia", "International"},
        new[] {"Islamabad", "33.607587", "73.100316", "Pakistan", "International"},
        new[] {"New York City", "40.642422", "-73.781749", "United States", "International"},
        new[] {"Lahore", "31.521139", "74.406519", "Pakistan", "International"},
        new[] {"Gilgit Baltistan", "35.919108", "74.332838", "Pakistan", "International"},
        new[] {"Jeddah", "21.683647", "39.152862", "Saudi Arabia", "International"},
        new[] {"Riyadh", "24.977080", "46.688942", "Saudi Arabia", "International"},
        new[] {"New Delhi", "28.555764", "77.096520", "India", "International"},
        new[] {"Hong Kong", "22.285005", "114.158339", "China", "International"},
        new[] {"Beijing", "40.052121", "116.609609", "China", "International"},
        new[] {"Tokyo", "35.550899", "139.780683", "Japan", "International"},
        new[] {"Kuala Lumpur", "2.749914", "101.707160", "Malaysia", "International"},
        new[] {"Sydney", "-33.942028", "151.174304", "Australia", "International"},
        new[] {"Melbourne", "-37.671812", "144.846079", "Australia", "International"},
        new[] {"Cape Town", "-33.968879", "18.596982", "South Africa", "International"},
        new[] {"Madrid", "40.476938", "-3.569428", "Spain", "International"},
        new[] {"Dublin", "53.424077", "-6.256792", "Ireland", "International"},
        new[] {"Johannesburg", "25.936834", "27.925890", "South Africa", "International"},
        new[] {"London", "51.504473", "0.052271", "United Kingdom", "International"},
        new[] {"Los Angeles", "33.942912", "-118.406829", "United States", "International"},
        new[] {"Brisbane", "-27.388925", "153.116751", "Australia", "International"},
        new[] {"Amsterdam", "52.308100", "4.764170", "Netherlands", "International"},
        new[] {"Stockholm", "59.651236", "17.924793", "Sweden", "International"},
        new[] {"Frankfurt", "50.050085", "8.571911", "Germany", "International"},
        new[] {"New Taipei City", "25.066471", "121.551638", "Taiwan", "International"},
        new[] {"Rio de Janeiro", "-22.812160", "-43.248636", "Brazil", "International"},
        new[] {"Seoul", "37.558773", "126.802822", "South Korea", "International"},
        new[] {"Yokohama", "35.462819", "139.637008", "Japan", "International"},
        new[] {"Ankara", "39.951898", "32.688792", "Turkey", "International"},
        new[] {"Casablanca", "33.368202", "-7.580998", "Morocco", "International"},
        new[] {"Shenzhen", "22.633977", "113.809360", "China", "International"},
        new[] {"Baghdad", "33.264824", "44.232014", "Iraq", "International"},
        new[] {"Alexandria", "40.232302", "-85.637150", "United States", "International"},
        new[] {"Pune", "18.579019", "73.908572", "India", "International"},
        new[] {"Shanghai", "31.145326", "121.804512", "China", "International"},
        new[] {"Istanbul", "41.289143", "41.261401", "28.742376", "Turkey", "International"},
        new[] {"Bhutan", "22.648322", "88.443152", "Bhutan", "International"},
        new[] {"Dhaka", "23.847177", "90.404133", "Bangladesh", "International"},
        new[] {"Munich", "48.354327", "11.788680", "Germany", "International"},
        new[] {"Perth", "56.435749", "-3.371675", "Australia", "International"},
        new[] {"Mexico", "21.038103", "-86.875259", "Mexico", "International"},
        new[] {"California", "32.733089", "-117.194514", "United States", "International"},
        new[] {"Kabul", "34.564296", "69.211574", "Afghanistan", "International"},
        new[] {"Yangon", "47.604505", "-122.330604", "Myanmar", "International"},
        new[] {"Lagos", "17.981829", "102.565684", "Nigeria", "International"},
        new[] {"Santiago", "-33.394795", "-70.790183", "Chile", "International"},
        new[] {"Kuwait", "29.239250", "47.971575", "Kuwait", "International"},
        new[] {"Nairobi", "39.958361", "41.174310", "Kenya", "International"},
        new[] {"Tehran", "35.696000", "51.401000", "Iran", "International"},
        new[] {"Saint Petersburg", "60.013492", "29.722189", "Russia", "International"},
        new[] {"Sialkot", "32.328361", "74.215310", "Pakistan", "International"},
        new[] {"Berlin", "52.554316", "13.291213", "Germany", "International"},
        new[] {"Paris", "48.999560", "2.539274", "France", "International"},
        new[] {"Dubai", "25.249869", "55.366483", "United Arab Emirates", "International"},
        new[] {"Ho Chi Minh City", "10.816332", "106.664067", "Vietnam", "International"},
        new[] {"Ha Noi", "21.217854", "105.792948", "Vietnam", "International"},
        new[] {"Da Nang", "16.055667", "108.202380", "Vietnam", "International"},
        new[] {"Quang Ninh", "21.123035", "107.415795", "Vietnam", "International"},
        new[] {"Phu Quoc", "10.162943", "103.998084", "Vietnam", "International"},
        new[] {"Hai Phong", "20.822658", "106.724718", "Vietnam", "International"},
        new[] {"Vinh", "18.727710", "105.668708", "Vietnam", "International"},
        new[] {"Thua Thien Hue", "16.397890", "107.700093", "Vietnam", "International"},
        new[] {"Cam Ranh", "11.998309", "109.219019", "Vietnam", "International"},
        new[] {"Da Lat", "11.748914", "108.368293", "Vietnam", "International"},
        new[] {"Binh Dinh", "13.953898", "109.048440", "Vietnam", "International"},
        new[] {"Can Tho", "10.080652", "105.712188", "Vietnam", "International" },
        new[] {"Dien Bien", "21.403374", "103.061597", "Vietnam", "Domestic" },
        new[] {"Thanh Hoa", "19.892754" , "105.4765462", "Vietnam", "Domestic" },
        new[] {"Quang Binh", "17.513173", "106.589771", "Vietnam", "Domestic" },
        new[] {"Quang Nam", "15.412313", "108.709657", "Vietnam", "Domestic" },
        new[] {"Phu Yen", "13.050594", "109.345897", "Vietnam", "Domestic" },
        new[] {"Pleiku", "14.006461", "108.006028", "Vietnam", "Domestic" },
        new[] {"Buon Me Thuot", "12.664323", "108.117821", "Vietnam", "Domestic" },
        new[] {"Kien Giang", "9.959967", "105.134935", "Vietnam", "Domestic" },
        new[] {"Ca Mau", "9.175938", "105.175963", "Vietnam", "Domestic" },
        new[] {"Con Dao", "8.731477", "106.629832", "Vietnam", "Domestic" },
        new[] {"Hiệu Đẹp Trai", "12.791860", "108.276721", "Vietnam", "Domestic" },

    };

        // ************************************************************ Behaviours/Methods ************************************************************

        /* Tạo ID ngẫu nhiên cho khách hàng (Customer).... */
        public void RandomIDGen()
        {            
            string randomID = rand.Next(20000, 1000000).ToString();
            SetRandomNum(randomID);
        }

        /* Phương thức này cho phép chọn điểm đi và điểm đến (thành phố) để tạo chuyến bay..... */

        public string[][] SpecificallyDestinations()
        {
            int specCity1;
            int specCity2;
            string fromWhichCity;
            string fromWhichCityLat;
            string fromWhichCityLong;
            string toWhichCity;
            string toWhichCityLat;
            string toWhichCityLong;
            string[][] chosenDestinations;
            Console.WriteLine("Danh sách thành phố:");
            int columns = 3;
            for (int i = 0; i < destinations.Length; i++)
            {
                Console.Write($"     {i}    {destinations[i][0],-20}");

                if ((i + 1) % columns == 0)
                {
                    Console.WriteLine(); // Xuống dòng sau mỗi số cột
                }
            }
            Console.WriteLine();

            Console.Write("Nhập tên thành phố xuất phát: ");
            while (!int.TryParse(Console.ReadLine(), out specCity1) || specCity1 < 0 || specCity1 > destinations.Length - 1)
            {
                Console.Write($"LỖI!! Vui lòng nhập giá trị giữa 0 - {destinations.Length - 1}. Nhập giá trị lại :\t");
            }

            Console.Write("Nhập tên thành phố bay đến: ");
            while (!int.TryParse(Console.ReadLine(), out specCity2) || specCity2 < 0 || specCity2 > destinations.Length - 1)
            {
                Console.Write($"LỖI!! Vui lòng nhập giá trị giữa 0 - {destinations.Length - 1}. Nhập giá trị lại :\t");
            }

            while (specCity2 == specCity1)
            {
                Console.Write("Bạn không thể nhập 2 thành phố giống nhau. ");
                Console.Write("Nhập lại tên thành phố bay đến: ");
                while (!int.TryParse(Console.ReadLine(), out specCity2) || specCity2 < 0 || specCity2 > destinations.Length - 1)
                {
                    Console.Write($"LỖI!! Vui lòng nhập giá trị giữa 0 - {destinations.Length - 1}. Nhập giá trị lại :\t");
                }
            }

            while (destinations[specCity1][3] != destinations[specCity2][3] && destinations[specCity1][4] != destinations[specCity2][4])
            {
                Console.Write("Các thành phố phải ở trong cùng 1 nước hoặc đều có các cảng hàng không quốc tế!");
                Console.Write("Nhập lại tên thành phố bay đi: ");
                while (!int.TryParse(Console.ReadLine(), out specCity1) || specCity1 < 0 || specCity1 > destinations.Length - 1)
                {
                    Console.Write($"LỖI!! Vui lòng nhập giá trị giữa 0 - {destinations.Length-1}. Nhập giá trị lại :\t");
                }
                Console.Write("Nhập lại tên thành phố bay đến: ");
                while (!int.TryParse(Console.ReadLine(), out specCity2) || specCity2 < 0 || specCity2 > destinations.Length - 1)
                {
                    Console.Write($"LỖI!! Vui lòng nhập giá trị giữa 0 - {destinations.Length-1}. Nhập giá trị lại :\t");
                }
            }

            // gán tên thành phố đi, kinh độ, vĩ độ
            fromWhichCity = destinations[specCity1][0];
            fromWhichCityLat = destinations[specCity1][1];
            fromWhichCityLong = destinations[specCity1][2];

            // gán tên thành phố đến, kinh độ, vĩ độ
            toWhichCity = destinations[specCity2][0];
            toWhichCityLat = destinations[specCity2][1];
            toWhichCityLong = destinations[specCity2][2];

            // gán điểm đi và điểm đến
            chosenDestinations = new string[2][];
            chosenDestinations[0] = new string[] { fromWhichCity, fromWhichCityLat, fromWhichCityLong };
            chosenDestinations[1] = new string[] { toWhichCity, toWhichCityLat, toWhichCityLong };

            return chosenDestinations;
        }

        /* Tạo số ghế ngẫu nhiên cho mỗi chuyến bay */
        public int RandomNumOfSeats()
        {            
            int numOfSeats = rand.Next(100, 500);
            return numOfSeats;
        }

        /* Tạo Số hiệu (Flight Number) chuyến bay duy nhất.... */

        public string RandomFlightNumbGen(int uptoHowManyLettersRequired, int divisible)
        {            
            string randomAlphabets = new string(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz", uptoHowManyLettersRequired)
                .Select(s => s[rand.Next(s.Length)]).ToArray());

            randomAlphabets += "-" + (RandomNumOfSeats() / divisible).ToString();

            return randomAlphabets;
        }

        //************************************************************ Setters & Getters ************************************************************

        public void SetRandomNum(string randomNum)
        {
            this.randomNum = randomNum;
        }

        public string GetRandomNumber()
        {
            return randomNum;
        }
    }
}
