using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    internal class RandomGenerator
    {
		Random rand = new Random();
		// ************************************************************ Fields ************************************************************
		private string randomNum;

        /* City name is at the 0-index, its latitude is on the 1-index, and longitude on the 2-index */
        private static readonly string[][] destinations =
        {
        new[] {"Karachi", "24.871940", "66.988060"},
        new[] {"Bangkok", "13.921430", "100.595337"},
        new[] {"Jakarta", "-6.174760", "106.827072"},
        new[] {"Islamabad", "33.607587", "73.100316"},
        new[] {"New York City", "40.642422", "-73.781749"},
        new[] {"Lahore", "31.521139", "74.406519"},
        new[] {"Gilgit Baltistan", "35.919108", "74.332838"},
        new[] {"Jeddah", "21.683647", "39.152862"},
        new[] {"Riyadh", "24.977080", "46.688942"},
        new[] {"New Delhi", "28.555764", "77.096520"},
        new[] {"Hong Kong", "22.285005", "114.158339"},
        new[] {"Beijing", "40.052121", "116.609609"},
        new[] {"Tokyo", "35.550899", "139.780683"},
        new[] {"Kuala Lumpur", "2.749914", "101.707160"},
        new[] {"Sydney", "-33.942028", "151.174304"},
        new[] {"Melbourne", "-37.671812", "144.846079"},
        new[] {"Cape Town", "-33.968879", "18.596982"},
        new[] {"Madrid", "40.476938", "-3.569428"},
        new[] {"Dublin", "53.424077", "-6.256792"},
        new[] {"Johannesburg", "25.936834", "27.925890"},
        new[] {"London", "51.504473", "0.052271"},
        new[] {"Los Angeles", "33.942912", "-118.406829"},
        new[] {"Brisbane", "-27.388925", "153.116751"},
        new[] {"Amsterdam", "52.308100", "4.764170"},
        new[] {"Stockholm", "59.651236", "17.924793"},
        new[] {"Frankfurt", "50.050085", "8.571911"},
        new[] {"New Taipei City", "25.066471", "121.551638"},
        new[] {"Rio de Janeiro", "-22.812160", "-43.248636"},
        new[] {"Seoul", "37.558773", "126.802822"},
        new[] {"Yokohama", "35.462819", "139.637008"},
        new[] {"Ankara", "39.951898", "32.688792"},
        new[] {"Casablanca", "33.368202", "-7.580998"},
        new[] {"Shenzhen", "22.633977", "113.809360"},
        new[] {"Baghdad", "33.264824", "44.232014"},
        new[] {"Alexandria", "40.232302", "-85.637150"},
        new[] {"Pune", "18.579019", "73.908572"},
        new[] {"Shanghai", "31.145326", "121.804512"},
        new[] {"Istanbul", "41.289143", "41.261401", "28.742376"},
        new[] {"Bhutan", "22.648322", "88.443152"},
        new[] {"Dhaka", "23.847177", "90.404133"},
        new[] {"Munich", "48.354327", "11.788680"},
        new[] {"Perth", "56.435749", "-3.371675"},
        new[] {"Mexico", "21.038103", "-86.875259"},
        new[] {"California", "32.733089", "-117.194514"},
        new[] {"Kabul", "34.564296", "69.211574"},
        new[] {"Yangon", "47.604505", "-122.330604"},
        new[] {"Lagos", "17.981829", "102.565684"},
        new[] {"Santiago", "-33.394795", "-70.790183"},
        new[] {"Kuwait", "29.239250", "47.971575"},
        new[] {"Nairobi", "39.958361", "41.174310"},
        new[] {"Tehran", "35.696000", "51.401000"},
        new[] {"Saint Petersburg", "60.013492", "29.722189"},
        new[] {"Hanoi", "21.219185", "105.803967"},
        new[] {"Sialkot", "32.328361", "74.215310"},
        new[] {"Berlin", "52.554316", "13.291213"},
        new[] {"Paris", "48.999560", "2.539274"},
        new[] {"Dubai", "25.249869", "55.366483"}
    };

        // ************************************************************ Behaviours/Methods ************************************************************

        /* Generates Random ID for the Customers.... */
        public void RandomIDGen()
        {            
            string randomID = rand.Next(20000, 1000000).ToString();
            SetRandomNum(randomID);
        }

        /* This method sets the destinations for each of the flights from the above destinations randomly..... */
        public string[][] RandomDestinations()
        {            
            int randomCity1 = rand.Next(destinations.Length);
            int randomCity2 = rand.Next(destinations.Length);

            string fromWhichCity = destinations[randomCity1][0];
            string fromWhichCityLat = destinations[randomCity1][1];
            string fromWhichCityLong = destinations[randomCity1][2];

            while (randomCity2 == randomCity1)
            {
                randomCity2 = rand.Next(destinations.Length);
            }

            string toWhichCity = destinations[randomCity2][0];
            string toWhichCityLat = destinations[randomCity2][1];
            string toWhichCityLong = destinations[randomCity2][2];

            string[][] chosenDestinations = new string[2][];
            chosenDestinations[0] = new string[] { fromWhichCity, fromWhichCityLat, fromWhichCityLong };
            chosenDestinations[1] = new string[] { toWhichCity, toWhichCityLat, toWhichCityLong };

            return chosenDestinations;
        }

        /* Generates the Random Number of Seats for each flight */
        public int RandomNumOfSeats()
        {            
            int numOfSeats = rand.Next(75, 500);
            return numOfSeats;
        }

        /* Generates the Unique Flight Number.... */

        public string RandomFlightNumbGen(int uptoHowManyLettersRequired, int divisible)
        {            
            string randomAlphabets = new string(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz", uptoHowManyLettersRequired)
                .Select(s => s[rand.Next(s.Length)]).ToArray());

            randomAlphabets += "-" + (RandomNumOfSeats() / divisible).ToString();

            return randomAlphabets;
        }

        // ************************************************************ Setters & Getters ************************************************************

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
