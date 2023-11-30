using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    public abstract class FlightDistance
    {
        public abstract string ToString(int i);

        public abstract string[] CalculateDistance(double lat1, double lon1, double lat2, double lon2);

        public void DisplayMeasurementInstructions()
        {
            string symbols = "+---------------------------+";
            Console.WriteLine($"\n\n {symbols,-100}\n {symbols,-100}");
            Console.WriteLine($"\n {symbols,-100}\n");
            Console.WriteLine("\n\t\t1. Distance between the destinations are based upon the Airports Coordinates(Latitudes && Longitudes) based in those cities\n");
            Console.WriteLine("\t\t2. Actual Distance of the flight may vary from this approximation as Airlines may define their on Travel Policy that may restrict the planes to fly through specific regions...\n");
            Console.WriteLine("\t\t3. Flight Time depends upon several factors such as Ground Speed(GS), AirCraft Design, Flight Altitude and Weather. Ground Speed for these calculations is 450 Knots...\n");
            Console.WriteLine("\t\t4. Expect reaching your destination early or late from the Arrival Time. So, please keep a margin of ±1 hour...\n");
            Console.WriteLine("\t\t5. The departure time is the moment that your plane pushes back from the gate, not the time it takes off. The arrival time is the moment that your plane pulls into the gate, not the time\n\t\t   it touches down on the runway...\n");
        }
    }
}
