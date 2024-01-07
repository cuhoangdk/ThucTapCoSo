using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    public interface IDisplayClass
    {
        void DisplayRegisteredPassengersForAllFlight();

        void DisplayRegisteredPassengersForASpecificFlight(string flightNum);

        void DisplayReceptTicketsRegisteredByOneUser(string userID);

        void DisplayArtWork(int options);
    }
}
