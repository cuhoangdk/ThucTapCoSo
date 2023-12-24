﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    internal class Generator
    {
		// ************************************************************ Fields ************************************************************
		Random rand = new Random();
	    	private string randomNum;

        /* Tên thành phố nằm ở chỉ số 0, vĩ độ của thành phố ở chỉ số 1 và kinh độ ở chỉ số 2 */
        public static readonly string[][] destinations =
        {
        new[] {"TP Hồ Chí Minh"   , "10.816332", "106.664067", "Vietnam", "International"},
        new[] {"Hà Nội"           , "21.217854", "105.792948", "Vietnam", "International"},
        new[] {"Đà Nẵng"          , "16.055667", "108.202380", "Vietnam", "International"},
        new[] {"Quảng Ninh"       , "21.123035", "107.415795", "Vietnam", "International"},
        new[] {"Phú Quốc"         , "10.162943", "103.998084", "Vietnam", "International"},
        new[] {"Hải Phòng"        , "20.822658", "106.724718", "Vietnam", "International"},
        new[] {"Vinh"             , "18.727710", "105.668708", "Vietnam", "International"},
        new[] {"Thừa Thiên Huế"   , "16.397890", "107.700093", "Vietnam", "International"},
        new[] {"Cam Ranh"         , "11.998309", "109.219019", "Vietnam", "International"},
        new[] {"Đà Lạt"           , "11.748914", "108.368293", "Vietnam", "International"},
        new[] {"Bình Định"        , "13.953898", "109.048440", "Vietnam", "International"},
        new[] {"Cần Thơ"          , "10.080652", "105.712188", "Vietnam", "International" },
        new[] {"Điện Biên"        , "21.403374", "103.061597", "Vietnam", "Domestic" },
        new[] {"Thanh Hóa"        , "19.892754", "105.476546", "Vietnam", "Domestic" },
        new[] {"Quảng Bình"       , "17.513173", "106.589771", "Vietnam", "Domestic" },
        new[] {"Quảng Nam"        , "15.412313", "108.709657", "Vietnam", "Domestic" },
        new[] {"Phú Yên"          , "13.050594", "109.345897", "Vietnam", "Domestic" },
        new[] {"Pleiku"           , "14.006461", "108.006028", "Vietnam", "Domestic" },
        new[] {"Buôn Mê Thuật"    , "12.664323", "108.117821", "Vietnam", "Domestic" },
        new[] {"Kiên Giang"       , "9.959967",  "105.134935", "Vietnam", "Domestic" },
        new[] {"Cà Mau"           , "9.175938",  "105.175963", "Vietnam", "Domestic" },
        new[] {"Côn Đảo"          , "8.731477",  "106.629832", "Vietnam", "Domestic" },
    };
        // ************************************************************ Behaviours/Methods ************************************************************

        //Tạo ID ngẫu nhiên cho khách hàng
        public void RandomIDGen()
        {            
            string randomID = rand.Next(10000000, 99999999).ToString();
            SetRandomNum(randomID);
        }
        //Hàm chọn điểm đi và điểm đến để tạo chuyến bay
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
            Console.WriteLine("\n\tDanh sách thành phố:\n");
            int columns = 3;
            for (int i = 0; i < destinations.Length; i++)
            {
                Console.Write($"\t{i,-3} {destinations[i][0],-20}");

                if ((i + 1) % columns == 0)
                {
                    Console.WriteLine(); // Xuống dòng sau mỗi số cột
                }
            }
            Console.Write("\nNhập tên thành phố xuất phát: ");
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
        //Hàm tạo mã cho chuyến bay
        public string RandomFlightNumbGen(int uptoHowManyLettersRequired, int divisible)
        {            
            string randomAlphabets = new string(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz", uptoHowManyLettersRequired)
                .Select(s => s[rand.Next(s.Length)]).ToArray());

            randomAlphabets += "-" + (rand.Next(100, 500) / divisible).ToString();

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
