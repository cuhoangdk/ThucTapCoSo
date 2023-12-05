using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    public abstract class FlightDistance
    {
        //private readonly string arrivalTime;
        public abstract string ToString(int i);

        public abstract string[] CalculateDistance(double lat1, double lon1, double lat2, double lon2);

        public void DisplayMeasurementInstructions()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine($"{new string(' ', 40)}+-----------------------------+");
            Console.WriteLine($"{new string(' ', 40)}| MỘT SỐ HƯỚNG DẪN QUAN TRỌNG |");
            Console.WriteLine($"{new string(' ', 40)}+-----------------------------+");
            Console.WriteLine("\t\t1. Khoảng cách giữa các điểm đến được xác định dựa trên tọa độ của sân bay (Vĩ độ và Kinh độ) nằm ở những thành phố đó.\n");
            Console.WriteLine("\t\t2. Khoảng cách thực tế của chuyến bay có thể khác so với ước lượng này do các hãng hàng không có thể đặt ra Chính sách Du lịch của riêng họ, có thể hạn chế máy bay bay qua các khu vực cụ thể...\n");
            Console.WriteLine("\t\t3. Thời gian bay phụ thuộc vào nhiều yếu tố như Tốc độ trên mặt đất (GS), Thiết kế máy bay, Độ cao bay và Thời tiết. Tốc độ trên mặt đất cho các tính toán này là 450 Knots...\n");
            Console.WriteLine("\t\t4. Dự kiến bạn sẽ đến đích sớm hơn hoặc muộn hơn so với thời gian Đến. Vì vậy, vui lòng giữ một khoảng cách là ±1 giờ...\n");
            Console.WriteLine("\t\t5. Thời gian khởi hành là thời điểm máy bay của bạn bắt đầu rời khỏi cổng, không phải là thời điểm máy bay cất cánh. Thời gian đến là thời điểm máy bay của bạn đến cổng,\n\t\t  không phải là thời điểm máy bay chạm xuống đường băng...\n");
        }
    }
}
