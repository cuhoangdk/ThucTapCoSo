using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    internal class User
    {
        static void Main()
        {
			Console.OutputEncoding = Encoding.Unicode;

            //Lấy vị trí hiện tại
            string current = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            string datatxt = Path.Combine(current, "datatxt");

            DateTime now = DateTime.Now;

            Flight f1 = new Flight();
            FlightReservation bookingAndReserving = new FlightReservation();

            Customer c1 = new Customer();

            Console.WriteLine();
            WelcomeScreen(1);
            Console.WriteLine("\n\t\t\t\t\t+++++++++++++ CHÀO MỪNG BẠN ĐẾN VỚI STAR AIRLINES +++++++++++++\n\n");
            DisplayMainMenu();
            int desiredOption;
            //fix code
			while (!int.TryParse(Console.ReadLine(), out desiredOption) || desiredOption < 0 || desiredOption > 5)
			{
                Console.Write("LỖI!! Vui lòng nhập giá trị giữa 0 - 5. Nhập giá trị lại :\t");
            }            

            do
            {
                /* Nếu desiredOption là 1 thì gọi phương thức đăng nhập.... nếu sử dụng thông tin đăng nhập mặc định thì đặt quyền
                 * mức về mức chuẩn/mặc định, nơi người dùng chỉ có thể xem dữ liệu của khách hàng... nếu không tìm thấy, trả về -1, và nếu
                 * dữ liệu được tìm thấy thì hiển thị menu hiển thị người dùng cho việc thêm, cập nhật, xóa và tìm kiếm người dùng/khách hàng...
                 * */
                if (desiredOption == 1)
                {
                    string filePathAdmin = Path.Combine(datatxt, "Admin.txt");
                    string[] Admin = File.ReadAllLines(filePathAdmin);
                    /*Default username and password....*/
                    PrintArtWork(1);
                    Console.Write("\n\tUSERNAME   :   ");
                    string username = Console.ReadLine();
                    Console.Write("\n\tPASSWORD   :   ");
                    string password = Console.ReadLine();
                    Console.WriteLine();
                    bool isFound = false;
                    for(int i = 0; i<Admin.Length; i++)
                    {
                        string[] dataAdmin = Admin[i].Split(';');

                        if(dataAdmin[0].Equals(username) && dataAdmin[1].Equals(password))
                        {
                            Console.WriteLine($"{"",-20}Đăng nhập thành công với tên người dùng \"{username}\"");
                            isFound = true;
                            do
                            {
                                Console.WriteLine($"\n\n\t+++++++++ 2ND LAYER MENU +++++++++ LOGGED IN AS \"{username}\"\n", "", "");

                                Console.WriteLine("\t\t1  : Thêm hành khách mới");
                                Console.WriteLine("\t\t2  : Tìm kiếm một hành khách");
                                Console.WriteLine("\t\t3  : Cập nhật dữ liệu hành khách");
                                Console.WriteLine("\t\t4  : Xóa hành khách");
                                Console.WriteLine("\t\t5  : Hiển thị tất cả hành khách");
                                Console.WriteLine("\t\t6  : Hiển thị tất cả các chuyến bay đã đăng ký bởi hành khách");
                                Console.WriteLine("\t\t7  : Hiển thị tất cả hành khách đã đăng ký trên một chuyến bay");
                                Console.WriteLine("\t\t8  : Xóa chuyến bay");
                                Console.WriteLine("\t\t9  : Thêm chuyến bay");
                                Console.WriteLine("\t\t10 : Chỉnh sửa chuyến bay");
                                Console.WriteLine("\t\t11 : Hiển thị toàn bộ các chuyến bay");
                                Console.WriteLine("\t\t0  : Đăng xuất");


                                Console.Write("\nNhập tùy chọn:   ");
                                //fix code
                                while (!int.TryParse(Console.ReadLine(), out desiredOption))
                                {
                                    Console.Write("Tuỳ chọn không hợp lệ, vui lòng nhập lại: ");
                                }

                                if (desiredOption == 1)
                                {
                                    c1.DisplayArtWork(1);
                                    c1.AddNewCustomer();
                                }
                                else if (desiredOption == 2)
                                {
                                    c1.DisplayArtWork(2);
                                    c1.DisplayCustomersData(false);
                                    Console.Write("\tNhập CustomerID muốn tìm :\t");
                                    string customerID = Console.ReadLine();
                                    Console.WriteLine();
                                    c1.SearchUser(customerID);
                                }
                                else if (desiredOption == 3)
                                {


                                    bookingAndReserving.DisplayArtWork(2);
                                    c1.DisplayCustomersData(false);
                                    Console.Write("Nhập CustomerID để Cập nhật Dữ liệu của khách hàng đó :\t");
                                    string customerID = Console.ReadLine();
                                    c1.EditCustomerInfo(customerID);
                                }
                                else if (desiredOption == 4)
                                {
                                    bookingAndReserving.DisplayArtWork(3);
                                    c1.DisplayCustomersData(false);
                                    Console.Write("Nhập CustomerID của khách hàng muốn xóa :\t");
                                    string customerID = Console.ReadLine();
                                    c1.DeleteCustomer(customerID);
                                }
                                else if (desiredOption == 5)
                                {
                                    c1.DisplayArtWork(3);
                                    c1.DisplayCustomersData(false);
                                }
                                else if (desiredOption == 6)
                                {
                                    bookingAndReserving.DisplayArtWork(6);
                                    c1.DisplayCustomersData(false);
                                    Console.Write("\n\nNhập ID của người dùng để hiển thị tất cả các chuyến bay đã đăng ký bởi người đó...");
                                    string id = Console.ReadLine();
                                    bookingAndReserving.DisplayFlightsRegisteredByOneUser(id);
                                }
                                else if (desiredOption == 7)
                                {
                                    c1.DisplayArtWork(4);
                                    Console.Write("Bạn muốn hiển thị Hành khách của tất cả các chuyến bay hay một chuyến bay cụ thể.... 'Y/y' để hiển thị tất cả các chuyến bay và 'N/n' để tìm kiếm một" +
                                                    " chuyến bay cụ thể.... ");

                                    char choice = Console.ReadLine()[0];
                                    if ('y' == choice || 'Y' == choice)
                                    {
                                        bookingAndReserving.DisplayRegisteredUsersForAllFlight();
                                    }
                                    else if ('n' == choice || 'N' == choice)
                                    {
                                        f1.DisplayFlightSchedule();
                                        Console.Write("Nhập Flight Number để hiển thị danh sách hành khách đã đăng ký trong chuyến bay đó... ");
                                        string flightNum = Console.ReadLine().ToUpper();
                                        bookingAndReserving.DisplayRegisteredUsersForASpecificFlight(flightNum);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Lựa chọn không hợp lệ...Không có phản hồi...!");
                                    }
                                }
                                else if (desiredOption == 8)
                                {
                                    c1.DisplayArtWork(5);
                                    f1.DisplayFlightSchedule();
                                    Console.Write("Nhập Flight Number để xóa chuyến bay : ");
                                    string flightNum = Console.ReadLine().ToUpper();
                                    f1.HiddenFlight(flightNum, username, now);

                                }
                                else if (desiredOption == 9)
                                {
                                    f1.AddFlight(username, now);
                                    f1.DisplayFlightSchedule();
                                }
                                else if (desiredOption == 10)
                                {
                                    f1.DisplayFlightSchedule();
                                    Console.WriteLine("Nhập vào Số hiệu chuyến bay muốn chỉnh sửa: ");
                                    string id = Console.ReadLine().ToUpper();
                                    f1.EditFlight(id, username, now);
                                }
                                else if (desiredOption == 11)
                                {
                                    f1.DisplayFlightSchedule();
                                }
                                else if (desiredOption == 0)
                                {
                                    bookingAndReserving.DisplayArtWork(22);
                                    Console.WriteLine("Cảm ơn bạn đã sử dụng Hệ thống Đặt vé của Star Airlines...!!!");
                                }
                                else
                                {
                                    Console.WriteLine("Lựa chọn không hợp lệ...Có vẻ như bạn là Robot...Đang nhập giá trị ngẫu nhiên...Bạn phải đăng nhập lại...");
                                    bookingAndReserving.DisplayArtWork(22);
                                    desiredOption = 0;
                                }

                            } while (desiredOption != 0);
                            break;
                        }
                    }
                    if (!isFound)
                    {
                        Console.WriteLine($"\n{"",20}LỖI!!! Không thể đăng nhập. Không thể tìm thấy người dùng với thông tin đăng nhập đã nhập.... Hãy Tạo Thông Tin Mới hoặc đăng ký bằng cách nhấn 2....", "");
                    }
                }
                else if (desiredOption == 2)
                {
                    string filePathAdmin = Path.Combine(datatxt, "Admin.txt");
                    string[] Admin = File.ReadAllLines(filePathAdmin);
                    PrintArtWork(2);
                    /*Nếu desiredOption là 2, hãy gọi phương thức đăng ký để đăng ký người dùng......*/
                    Console.Write("\nNhập tên người dùng để Đăng ký :    ");
                    string username = Console.ReadLine();
                    Console.Write("\nNhập mật khẩu để Đăng ký :    ");
                    string password = Console.ReadLine();
                    for(int i=0; i<Admin.Length; i++)
                    {
                        string[] dataAdmin = Admin[i].Split(';');

                        while ( dataAdmin[0].Equals(username) )
                        {
                            Console.Write("LỖI!!! Quản trị viên với cùng tên người dùng đã tồn tại. Nhập tên người dùng mới:   ");
                            username = Console.ReadLine();
                            Console.Write("Nhập lại mật khẩu:   ");
                            password = Console.ReadLine();
                        }                
                    }
                    using( StreamWriter writer = new StreamWriter(filePathAdmin, true))
                    {
                        writer.WriteLine($"{username};{password}");
                    }
                }
                else if (desiredOption == 3)
                {
                    string filePathCustomer = Path.Combine(datatxt, "Customer.txt");
                    string[] Customer = File.ReadAllLines(filePathCustomer);

                    PrintArtWork(3);
                    Console.Write("\n\tEMAIL     :   ");
                    string userName = Console.ReadLine();
                    Console.Write("\n\tPASSWORD  :   ");
                    string password = Console.ReadLine();

                    bool isFound = false;

                    for(int i=0; i<Customer.Length; i++)
                    {
                        string[] dataC = Customer[i].Split(';');
                        if (dataC[3].Equals(userName) && dataC[4].Equals(password) && dataC[0] == "1")
                        {
                            isFound = true;
                            int desiredChoice;
                            Console.WriteLine($"\n\n{"",-20}Đăng nhập thành công với tên người dùng \"{userName}\"");
                            do
                            {
                                Console.WriteLine($"\n\n{"",-60}+++++++++ 3RD LAYER MENU +++++++++{"",50}LOGGED IN AS \"{userName}\"\n");
                                Console.WriteLine("\t\t1 : Đặt chỗ");
                                Console.WriteLine("\t\t2 : Cập nhật thông tin tài khoản");
                                Console.WriteLine("\t\t3 : Xóa tài khoản");
                                Console.WriteLine("\t\t4 : Lịch trình chuyến bay");
                                Console.WriteLine("\t\t5 : Hủy vé bay");
                                Console.WriteLine($"\t\t6 : Các vé được đặt bởi tài khoản \"{userName}\"....");
                                Console.WriteLine("\t\t0 : Đăng xuất ");
                                Console.Write("\t\tNhập tùy chọn :   ");
                                while (!int.TryParse(Console.ReadLine(), out desiredChoice))
                                {
                                    Console.Write("Lựa chọn không hợp lệ vui lòng nhập lại:  ");
                                }
                                if (desiredChoice == 1)
                                {
                                    bookingAndReserving.DisplayArtWork(1);
                                    f1.DisplayFlightSchedule();
                                    Console.Write("\nNhập mã chuyến bay mong muốn để đặt chỗ :\t ");
                                    string flightToBeBooked = Console.ReadLine().ToUpper();

                                    string ticketType;
                                    while (true)
                                    {
                                        Console.WriteLine("\nNhập loại vé bạn muốn đặt (1. Business  / 2. Economy ):\t");
                                        int choose;

                                        if (int.TryParse(Console.ReadLine(), out choose) && (choose == 1 || choose == 2))
                                        {
                                            ticketType = (choose == 1) ? "BSN" : "ECO";
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                                        }
                                    }
                                    Console.Write($"Nhập số lượng vé cho chuyến bay {flightToBeBooked} :   ");
                                    int numOfTickets;
                                    while (!int.TryParse(Console.ReadLine(), out numOfTickets) || numOfTickets > 10 || numOfTickets < 1)
                                    {
                                        Console.Write("LỖI!! Vui lòng nhập số lượng vé hợp lệ (ít hơn 10, nhiều hơn 0): ");
                                    }
                                    bookingAndReserving.BookFlight(flightToBeBooked, dataC[1], numOfTickets, ticketType);
                                }
                                else if (desiredChoice == 2)
                                {
                                    bookingAndReserving.DisplayArtWork(2);
                                    c1.EditCustomerInfo(dataC[1]);
                                }
                                else if (desiredChoice == 3)
                                {
                                    bookingAndReserving.DisplayArtWork(3);
                                    Console.Write("\tBẠN CÓ CHẮC CHẮN MUỐN XÓA TÀI KHOẢN CỦA MÌNH. HÀNH ĐỘNG NÀY KHÔNG THỂ HOÀN TÁC. Nhập Y/y để xác nhận...");
                                    char confirmationChar = Console.ReadLine()[0];
                                    if (confirmationChar == 'Y' || confirmationChar == 'y')
                                    {
                                        c1.DeleteCustomer(dataC[1]);
                                        Console.WriteLine($"Tài khoản của người dùng {userName} đã bị xóa!");
                                        desiredChoice = 0;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Hành động đã bị hủy...");
                                    }
                                }
                                else if (desiredChoice == 4)
                                {
                                    bookingAndReserving.DisplayArtWork(4);
                                    f1.DisplayFlightSchedule();
                                    f1.DisplayMeasurementInstructions();
                                }
                                else if (desiredChoice == 5)
                                {
                                    bookingAndReserving.DisplayArtWork(5);
                                    bookingAndReserving.CancelFlight(dataC[1]);
                                }
                                else if (desiredChoice == 6)
                                {
                                    bookingAndReserving.DisplayArtWork(6);
                                    bookingAndReserving.DisplayFlightsRegisteredByOneUser(dataC[1]);
                                }
                                else
                                {
                                    bookingAndReserving.DisplayArtWork(7);
                                    if (desiredChoice != 0)
                                    {
                                        Console.WriteLine("Lựa chọn không hợp lệ... Dường như bạn là Robot... Nhập giá trị ngẫu nhiên... Bạn phải đăng nhập lại...");
                                    }
                                    desiredChoice = 0;
                                }
                            } while (desiredChoice != 0);
                        }
                    }
                    if(!isFound)
                    {
                        Console.WriteLine($"\n{" ",20}LỖI!!! Không thể đăng nhập. Không thể tìm thấy người dùng với thông tin đăng nhập đã nhập.... Hãy Tạo Thông Tin Mới hoặc đăng ký bằng cách nhấn 4....", "");
                    }
                }
                else if (desiredOption == 4)
                {
                    PrintArtWork(4);
                    c1.AddNewCustomer();
                }
                else if (desiredOption == 5)
                {
                    ManualInstructions();
                }

                DisplayMainMenu();
				while (!int.TryParse(Console.ReadLine(), out desiredOption)|| desiredOption < 0 || desiredOption > 5)
				{
                    Console.Write("LỖI!! Vui lòng nhập giá trị từ 0 - 5. Nhập lại giá trị :\t");
                }                
            } while (desiredOption != 0);

            WelcomeScreen(-1);
        }

        static void DisplayMainMenu()
        {
			Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("\n\n\t\t1 : Đăng nhập quản trị viên.");
            Console.WriteLine("\t\t2 : Đăng ký quản trị viên.");
            Console.WriteLine("\t\t3 : Đăng nhập hành khách.");
            Console.WriteLine("\t\t4 : Đăng ký hành khách.");
            Console.WriteLine("\t\t5 : Hiển thị Hướng dẫn sử dụng.");
            Console.WriteLine("\t\t0 : Thoát.");
            Console.Write("\t\tNhập tùy chọn mong muốn:    ");

        }

        static void ManualInstructions()
        {
			Console.OutputEncoding = Encoding.Unicode;
			Console.WriteLine($"\n\n{new string(' ', 50)} +++++++++++++++++ Chào mừng bạn đến với Hướng dẫn sử dụng của Star Airlines +++++++++++++++++");
            Console.WriteLine("\n\t\t1 : Hướng dẫn quản trị.");
            Console.WriteLine("\t\t2 : Hướng dẫn khách hàng.");
            Console.Write("\nNhập tùy chọn mong muốn:   ");
            //fix code
            int choice;
			while (!int.TryParse(Console.ReadLine(), out choice)|| choice < 1 || choice > 2)
			{
				Console.Write("LỖI!!! Mục nhập không hợp lệ...Vui lòng nhập giá trị 1 hoặc 2....Nhập lại....  ");
			}
			if (choice == 1)
			{
                Console.WriteLine("\n\n(1) Quản trị viên có quyền truy cập vào dữ liệu của tất cả người dùng... Quản trị viên có thể xóa, cập nhật, thêm và thực hiện tìm kiếm cho bất kỳ khách hàng nào...\n");
                Console.WriteLine("(2) Để truy cập vào mô-đun quản trị, bạn phải đăng ký bằng cách nhấn 2, khi menu chính hiển thị...\n");
                Console.WriteLine("(3) Cung cấp các thông tin cần thiết như tên, email, id... Sau khi bạn đã đăng ký, nhấn 1 để đăng nhập như một quản trị viên... \n");
                Console.WriteLine("(4) Khi bạn đã đăng nhập, menu lớp 2 sẽ được hiển thị trên màn hình... Từ đây, bạn có thể chọn từ nhiều tùy chọn...\n");
                Console.WriteLine("(5) Nhấn \"1\" sẽ thêm một hành khách mới, cung cấp chi tiết cần thiết để thêm hành khách...\n");
                Console.WriteLine("(6) Nhấn \"2\" sẽ tìm kiếm bất kỳ hành khách nào, miễn là quản trị viên (bạn) cung cấp ID từ bảng in ở trên....  \n");
                Console.WriteLine("(7) Nhấn \"3\" sẽ cho phép bạn cập nhật bất kỳ dữ liệu hành khách nào, miễn là ID người dùng được cung cấp cho chương trình...\n");
                Console.WriteLine("(8) Nhấn \"4\" sẽ cho phép bạn xóa bất kỳ hành khách nào, miễn là ID được cung cấp...\n");
                Console.WriteLine("(9) Nhấn \"5\" sẽ cho phép bạn hiển thị tất cả hành khách đã đăng ký...\n");
				Console.WriteLine("(10) Nhấn \"6\" sẽ cho phép bạn hiển thị tất cả các chuyến bay đã đăng ký bởi hành khách, cung cấp ID hành khách để xem chi tiết...\n");
				Console.WriteLine("(10) Nhấn \"7\" sẽ cho phép bạn hiển thị tất cả hành khách đã đăng ký... Sau khi chọn, chương trình sẽ hỏi bạn có muốn hiển thị hành khách cho tất cả các chuyến bay (Y/y) hay một chuyến bay cụ thể (N/n)\n");
                Console.WriteLine("(11) Nhấn \"8\" sẽ cho phép bạn xóa bất kỳ chuyến bay nào, miễn là số hiệu chuyến bay được cung cấp...\n");
				Console.WriteLine("(12) Nhấn \"9\" sẽ thêm một chuyến bay mới, cung cấp chi tiết cần thiết để thêm chuyến bay...\n");
				Console.WriteLine("(13) Nhấn \"10\" sẽ cho phép bạn cập nhật bất kỳ dữ liệu chuyến bay nào, miễn là số hiệu chuyến bay được cung cấp cho chương trình...\n");
				Console.WriteLine("(14) Nhấn \"11\" sẽ hiển thị lịch trình chuyến bay...\n");
				Console.WriteLine("() Nhấn \"0\" sẽ khiến bạn đăng xuất khỏi chương trình... Bạn có thể đăng nhập lại bất cứ lúc nào trong quá trình thực hiện chương trình....\n");

            }
            else
			{
                Console.WriteLine("\n\n(1) Người dùng cục bộ chỉ có quyền truy cập vào dữ liệu của mình... Anh/Chị sẽ không thể thay đổi/cập nhật dữ liệu của người dùng khác...\n");
                Console.WriteLine("(2) Để truy cập vào các ưu đãi của người dùng cục bộ, bạn phải đăng ký bằng cách nhấn 4 khi menu chính hiển thị...\n");
                Console.WriteLine("(3) Cung cấp thông tin được yêu cầu bởi chương trình để thêm bạn vào danh sách người dùng... Sau khi bạn đã đăng ký, nhấn \"3\" để đăng nhập như một hành khách...\n");
                Console.WriteLine("(4) Sau khi bạn đã đăng nhập, menu lớp 3 sẽ được hiển thị... Từ đây, bạn bắt đầu cuộc hành trình để bay với chúng tôi...\n");
                Console.WriteLine("(5) Nhấn \"1\" sẽ hiển thị danh sách các chuyến bay có sẵn/lên lịch... Để đặt chỗ cho mình trên một chuyến bay, nhập số hiệu chuyến bay và số vé cho chuyến bay... Số vé tối đa mỗi lần là 10 ...\n");
                Console.WriteLine("(7) Nhấn \"2\" sẽ cho phép bạn cập nhật dữ liệu của chính bạn... Bạn sẽ không thể cập nhật dữ liệu của người khác... \n");
                Console.WriteLine("(8) Nhấn \"3\" sẽ xóa tài khoản của bạn... \n");
                Console.WriteLine("(9) Nhấn \"4\" sẽ hiển thị lịch trình chuyến bay...\n");
                Console.WriteLine("(10) Nhấn \"5\" sẽ cho phép bạn hủy bỏ bất kỳ chuyến bay nào đã đăng ký bởi bạn...\n");
                Console.WriteLine("(11) Nhấn \"6\" sẽ hiển thị tất cả chuyến bay đã đăng ký bởi bạn...\n");
                Console.WriteLine("(12) Nhấn \"0\" sẽ khiến bạn đăng xuất khỏi chương trình... Bạn có thể đăng nhập lại bất cứ lúc nào với thông tin đăng nhập của mình... cho chạy này cụ thể... \n");

            }

        }
		static void WelcomeScreen(int option)
        {
			Console.OutputEncoding = Encoding.Unicode;
			string artWork;

        if (option == 1)
        {
            artWork = @"
            ███████╗████████╗ █████╗ ██████╗      █████╗ ██╗██████╗ ██╗     ██╗███╗   ██╗███████╗███████╗
            ██╔════╝╚══██╔══╝██╔══██╗██╔══██╗    ██╔══██╗██║██╔══██╗██║     ██║████╗  ██║██╔════╝██╔════╝
            ███████╗   ██║   ███████║██████╔╝    ███████║██║██████╔╝██║     ██║██╔██╗ ██║█████╗  ███████╗
            ╚════██║   ██║   ██╔══██║██╔══██╗    ██╔══██║██║██╔══██╗██║     ██║██║╚██╗██║██╔══╝  ╚════██║
            ███████║   ██║   ██║  ██║██║  ██║    ██║  ██║██║██║  ██║███████╗██║██║ ╚████║███████╗███████║
            ╚══════╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝    ╚═╝  ╚═╝╚═╝╚═╝  ╚═╝╚══════╝╚═╝╚═╝  ╚═══╝╚══════╝╚══════╝
                                                                                             
            ";

        }
        else
        {
            artWork = @"
            ███████╗██╗     ██╗   ██╗██╗███╗   ██╗ ██████╗     ████████╗ ██████╗     ███████╗████████╗ █████╗ ██████╗ 
            ██╔════╝██║     ╚██╗ ██╔╝██║████╗  ██║██╔════╝     ╚══██╔══╝██╔═══██╗    ██╔════╝╚══██╔══╝██╔══██╗██╔══██╗
            █████╗  ██║      ╚████╔╝ ██║██╔██╗ ██║██║  ███╗       ██║   ██║   ██║    ███████╗   ██║   ███████║██████╔╝
            ██╔══╝  ██║       ╚██╔╝  ██║██║╚██╗██║██║   ██║       ██║   ██║   ██║    ╚════██║   ██║   ██╔══██║██╔══██╗
            ██║     ███████╗   ██║   ██║██║ ╚████║╚██████╔╝       ██║   ╚██████╔╝    ███████║   ██║   ██║  ██║██║  ██║
            ╚═╝     ╚══════╝   ╚═╝   ╚═╝╚═╝  ╚═══╝ ╚═════╝        ╚═╝    ╚═════╝     ╚══════╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
                    ";
        }

        Console.WriteLine(artWork);
    }
        static void PrintArtWork(int option)
        {
			Console.OutputEncoding = Encoding.Unicode;
			string artWork;

            if (option == 4)
            {
                artWork = @"
                 ██████ ██    ██ ███████ ████████  ██████  ███    ███ ███████ ██████      ███████ ██  ██████  ███    ██ ██    ██ ██████  
                ██      ██    ██ ██         ██    ██    ██ ████  ████ ██      ██   ██     ██      ██ ██       ████   ██ ██    ██ ██   ██ 
                ██      ██    ██ ███████    ██    ██    ██ ██ ████ ██ █████   ██████      ███████ ██ ██   ███ ██ ██  ██ ██    ██ ██████  
                ██      ██    ██      ██    ██    ██    ██ ██  ██  ██ ██      ██   ██          ██ ██ ██    ██ ██  ██ ██ ██    ██ ██      
                 ██████  ██████  ███████    ██     ██████  ██      ██ ███████ ██   ██     ███████ ██  ██████  ██   ████  ██████  ██      

                    ";
            }
            else if (option == 3)
            {
                artWork = @"
                 ██████ ██    ██ ███████ ████████  ██████  ███    ███ ███████ ██████      ██       ██████   ██████  ██ ███    ██ 
                ██      ██    ██ ██         ██    ██    ██ ████  ████ ██      ██   ██     ██      ██    ██ ██       ██ ████   ██ 
                ██      ██    ██ ███████    ██    ██    ██ ██ ████ ██ █████   ██████      ██      ██    ██ ██   ███ ██ ██ ██  ██ 
                ██      ██    ██      ██    ██    ██    ██ ██  ██  ██ ██      ██   ██     ██      ██    ██ ██    ██ ██ ██  ██ ██ 
                 ██████  ██████  ███████    ██     ██████  ██      ██ ███████ ██   ██     ███████  ██████   ██████  ██ ██   ████ 
                                                                                                                 
                    ";
            }
            else if (option == 2)
            {
                artWork = @"
                 █████  ██████  ███    ███ ██ ███    ██     ███████ ██  ██████  ███    ██ ██    ██ ██████  
                ██   ██ ██   ██ ████  ████ ██ ████   ██     ██      ██ ██       ████   ██ ██    ██ ██   ██ 
                ███████ ██   ██ ██ ████ ██ ██ ██ ██  ██     ███████ ██ ██   ███ ██ ██  ██ ██    ██ ██████  
                ██   ██ ██   ██ ██  ██  ██ ██ ██  ██ ██          ██ ██ ██    ██ ██  ██ ██ ██    ██ ██      
                ██   ██ ██████  ██      ██ ██ ██   ████     ███████ ██  ██████  ██   ████  ██████  ██      
                        ";
            }
            else
            {
                artWork = @"
                 █████  ██████  ███    ███ ██ ███    ██     ██       ██████   ██████  ██ ███    ██ 
                ██   ██ ██   ██ ████  ████ ██ ████   ██     ██      ██    ██ ██       ██ ████   ██ 
                ███████ ██   ██ ██ ████ ██ ██ ██ ██  ██     ██      ██    ██ ██   ███ ██ ██ ██  ██ 
                ██   ██ ██   ██ ██  ██  ██ ██ ██  ██ ██     ██      ██    ██ ██    ██ ██ ██  ██ ██ 
                ██   ██ ██████  ██      ██ ██ ██   ████     ███████  ██████   ██████  ██ ██   ████                                                                                                                                 
                    ";
            }

            Console.WriteLine(artWork);
        }


    }

    
}
