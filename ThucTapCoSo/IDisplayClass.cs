using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    public interface IDisplayClass
    {
        void DisplayRegisteredUsersForAllFlight();

        void DisplayRegisteredUsersForASpecificFlight(string flightNum);

        //void DisplayHeaderForUsers(Flight flight, List<Customer> c);

        void DisplayFlightsRegisteredByOneUser(string userID);

        void DisplayArtWork(int options);
    }
}
