using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    internal class User
    {
        static protected string[,] adminUserNameAndPassword = new string[10, 2];
        private static List<Customer> customersCollection = new List<Customer>();
        static void Main()
        {
            DateTime now = DateTime.Now;
			Console.OutputEncoding = Encoding.Unicode;
			int countNumOfUsers = 1;
            RolesAndPermissions r1 = new RolesAndPermissions();

            Flight f1 = new Flight();
            FlightReservation bookingAndReserving = new FlightReservation();
            //f1.FlightScheduler();

            Customer c1 = new Customer();
            //c1.AddCustomerWithFile();       //..bin/Debug/datatxt/Customer.txt
            Console.WriteLine();
            WelcomeScreen(1);
            Console.WriteLine("\n\t\t\t\t\t+++++++++++++ Chào mừng bạn đến với Star AirLines +++++++++++++\n\nĐể tiếp tục, vui lòng nhập một giá trị.");
            Console.WriteLine("\n***** Tên người dùng && Mật khẩu mặc định là root-root ***** Sử dụng Thông tin đăng nhập mặc định sẽ giới hạn bạn chỉ có thể xem danh sách Hành khách....\n");
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
                    /*Default username and password....*/
                    adminUserNameAndPassword[0,0] = "root";
                    adminUserNameAndPassword[0,1] = "root";
                    PrintArtWork(1);
                    Console.Write("\nNhập tên người dùng để đăng nhập vào Hệ thống Quản lý :     ");
                    string username = Console.ReadLine();
                    Console.Write("\nNhập mật khẩu để đăng nhập vào Hệ thống Quản lý :     ");
                    string password = Console.ReadLine();
                    Console.WriteLine();

                    /*Kiểm tra RolesAndPermissions......*/
                    if (r1.IsPrivilegedUserOrNot(username, password) == -1)
                    {
                        Console.WriteLine($"\n{"",20}LỖI!!! Không thể đăng nhập. Không thể tìm thấy người dùng với thông tin đăng nhập đã nhập.... Hãy Tạo Thông Tin Mới hoặc đăng ký bằng cách nhấn 4....", "");
                    }
                    else if (r1.IsPrivilegedUserOrNot(username, password) == 0)
                    {
                        Console.WriteLine("Bạn có quyền truy cập dữ liệu thông thường/mặc định... Bạn chỉ có thể xem dữ liệu của khách hàng... Không thể thực hiện bất kỳ hành động nào trên họ....");
                        c1.DisplayCustomersData(true);
                    }
                    else
                    {
                        // In ra màn hình để kiểm tra						
                        Console.WriteLine($"{"",-20}Đăng nhập thành công với tên người dùng \"{username}\"..... Để tiếp tục, nhập một giá trị từ dưới đây....", "");

                        /* Sẽ hiển thị các hoạt động CRUD mà người dùng đặc quyền có thể thực hiện..... Bao gồm Tạo, Cập nhật,
                         * Đọc (Tìm kiếm) và xóa một khách hàng....
                         * */

                        do
                        {
                            Console.WriteLine($"\n\n+++++++++ 2nd Layer Menu +++++++++ Logged in as \"{username}\"\n", "", "");

                            Console.WriteLine("(a) Nhập 1 để thêm hành khách mới....");
                            Console.WriteLine("(b) Nhập 2 để tìm kiếm một hành khách....");
                            Console.WriteLine("(c) Nhập 3 để cập nhật dữ liệu của hành khách....");
                            Console.WriteLine("(d) Nhập 4 để xóa một hành khách....");
                            Console.WriteLine("(e) Nhập 5 để Hiển thị tất cả hành khách....");
                            Console.WriteLine("(f) Nhập 6 để Hiển thị tất cả các chuyến bay đã đăng ký bởi hành khách...");
                            Console.WriteLine("(g) Nhập 7 để Hiển thị tất cả hành khách đã đăng ký trên một chuyến bay....");
                            Console.WriteLine("(h) Nhập 8 để Xóa một chuyến bay....");
                            Console.WriteLine("(h) Nhập 9 để Thêm một chuyến bay....");
                            Console.WriteLine("(h) Nhập 10 để Chỉnh sửa một chuyến bay....");
                            Console.WriteLine("(h) Nhập 11 để Hiển thị toàn bộ các chuyến bay....");
                            Console.WriteLine("(i) Nhập 0 để Quay lại Menu Chính/Đăng xuất....");


                            Console.Write("\nNhập tùy chọn mong muốn:   ");
                            //fix code
                            while (!int.TryParse(Console.ReadLine(), out desiredOption))
							{
								Console.Write("Tuỳ chọn không hợp lệ, vui lòng nhập lại: ");
							}

							/*If 1 is entered by the privileged user, then add a new customer......*/
							if (desiredOption == 1)
                            {
                                c1.DisplayArtWork(1);
                                c1.AddNewCustomer();
                            }
                            else if (desiredOption == 2)
                            {
                                /*If 2 is entered by the privileged user, then call the search method of the Customer class*/
                                c1.DisplayArtWork(2);
                                c1.DisplayCustomersData(false);
                                Console.Write("Nhập CustomerID muốn tìm :\t");
                                string customerID = Console.ReadLine();
                                Console.WriteLine();
                                c1.SearchUser(customerID);
                            }
                            else if (desiredOption == 3)
                            {
                                /* Nếu người dùng nhập 3, sau đó gọi phương thức cập nhật của lớp Customer với các đối số cần thiết....
                                 * */

                                bookingAndReserving.DisplayArtWork(2);
                                c1.DisplayCustomersData(false);
                                Console.Write("Nhập CustomerID để Cập nhật Dữ liệu của khách hàng đó :\t");
                                string customerID = Console.ReadLine();
                                c1.EditUserInfo(customerID);
                            }
                            else if (desiredOption == 4)
                            {
                                /*Nếu nhập 4 thì yêu cầu người dùng nhập id khách hàng rồi xóa khách hàng đó.... */
                                bookingAndReserving.DisplayArtWork(3);
                                c1.DisplayCustomersData(false);
                                Console.Write("Nhập CustomerID của khách hàng muốn xóa :\t");
                                string customerID = Console.ReadLine();
                                c1.DeleteUser(customerID);
                            }
                            else if (desiredOption == 5)
                            {
                                /*Nếu nhập 5, Gọi phương thức Display của lớp Customer ....*/
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
                                f1.HiddenFlight(flightNum,username,now);

                            }
                            else if (desiredOption == 9)
                            {
                                f1.AddFlight(username,now);
                                f1.DisplayFlightSchedule();
                            }
                            else if (desiredOption == 10)
                            {
                                f1.DisplayFlightSchedule();
                                Console.WriteLine("Nhập vào Số hiệu chuyến bay muốn chỉnh sửa: ");
                                string id = Console.ReadLine().ToUpper();
                                f1.EditFlight(id,username,now);
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

                    }
                }
                else if (desiredOption == 2)
                {
                    PrintArtWork(2);
                    /*Nếu desiredOption là 2, hãy gọi phương thức đăng ký để đăng ký người dùng......*/
                    Console.Write("\nNhập tên người dùng để Đăng ký :    ");
                    string username = Console.ReadLine();
                    Console.Write("\nNhập mật khẩu để Đăng ký :    ");
                    string password = Console.ReadLine();
                    while (r1.IsPrivilegedUserOrNot(username, password) != -1)
                    {
                        Console.Write("LỖI!!! Quản trị viên với cùng tên người dùng đã tồn tại. Nhập tên người dùng mới:   ");
                        username = Console.ReadLine();
                        Console.Write("Nhập lại mật khẩu:   ");
                        password = Console.ReadLine();
                    }

                    /*Thiết lập thông tin đăng nhập do người dùng nhập......*/
                    adminUserNameAndPassword[countNumOfUsers, 0] = username;
                    adminUserNameAndPassword[countNumOfUsers, 1] = password;

                    /*Tăng số lượng người dùng */
                    countNumOfUsers++;
                }
                else if (desiredOption == 3)
                {
                    PrintArtWork(3);
                    Console.Write("\n\nNhập Email để đăng nhập: \t");
                    string userName = Console.ReadLine();
                    Console.Write("Nhập mật khẩu : \t");
                    string password = Console.ReadLine();
                    string[] result = r1.IsPassengerRegistered(userName, password).Split('-');

                    if (Convert.ToInt32(result[0]) == 1)
                    {
                        int desiredChoice;
                        Console.WriteLine($"\n\n{"",-20}Đăng nhập thành công với tên người dùng \"{userName}\"..... Để tiếp tục, nhập giá trị từ dưới đây....");
                        do
                        {
                            Console.WriteLine($"\n\n{"",-60}+++++++++ 3rd Layer Menu +++++++++{"",50}Logged in as \"{userName}\"\n");
                            Console.WriteLine("(a) Nhập 1 để Đặt chỗ trên chuyến bay....");
                            Console.WriteLine("(b) Nhập 2 để cập nhật dữ liệu của bạn....");
                            Console.WriteLine("(c) Nhập 3 để xóa tài khoản của bạn....");
                            Console.WriteLine("(d) Nhập 4 để Hiển thị Lịch trình chuyến bay....");
                            Console.WriteLine("(e) Nhập 5 để Hủy chuyến bay....");
                            Console.WriteLine($"(f) Nhập 6 để Hiển thị tất cả các chuyến bay đã đăng ký bởi \"{userName}\"....");
                            Console.WriteLine("(g) Nhập 0 để Quay lại Menu Chính/Đăng xuất....");
                            Console.Write("Nhập lựa chọn mong muốn :   ");
							while (!int.TryParse(Console.ReadLine(), out desiredChoice))
							{
								Console.Write("Lựa chọn không hợp lệ vui lòng nhập lại:  ");
							}							
                            if (desiredChoice == 1)
                            {
                                bookingAndReserving.DisplayArtWork(1);
                                f1.DisplayFlightSchedule();
                                Console.Write("\nNhập số chuyến bay mong muốn để đặt chỗ :\t ");
                                string flightToBeBooked = Console.ReadLine().ToUpper();
                                Console.Write($"Nhập số lượng vé cho chuyến bay {flightToBeBooked} :   ");
                                int numOfTickets;
								while (!int.TryParse(Console.ReadLine(), out numOfTickets) || numOfTickets > 10 || numOfTickets < 1)
								{
                                    Console.Write("LỖI!! Vui lòng nhập số lượng vé hợp lệ (ít hơn 10, nhiều hơn 0): ");
                                }                                
                                bookingAndReserving.BookFlight(flightToBeBooked, numOfTickets, result[1]);
                            }
                            else if (desiredChoice == 2)
                            {
                                bookingAndReserving.DisplayArtWork(2);
                                c1.EditUserInfo(result[1]);
                            }
                            else if (desiredChoice == 3)
                            {
                                bookingAndReserving.DisplayArtWork(3);
                                Console.Write("Bạn chắc chắn muốn xóa tài khoản của mình... Đây là một hành động không thể hoàn tác... Nhập Y/y để xác nhận...");
                                char confirmationChar = Console.ReadLine()[0];
                                if (confirmationChar == 'Y' || confirmationChar == 'y')
                                {
                                    c1.DeleteUser(result[1]);
                                    Console.WriteLine($"Tài khoản của người dùng {userName} đã bị xóa thành công...!!!");
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
                                bookingAndReserving.CancelFlight(result[1]);
                            }
                            else if (desiredChoice == 6)
                            {
                                bookingAndReserving.DisplayArtWork(6);
                                bookingAndReserving.DisplayFlightsRegisteredByOneUser(result[1]);
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
                    else
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
            Console.WriteLine("\n\n\t\t(a) Nhấn 0 để Thoát.");
            Console.WriteLine("\t\t(b) Nhấn 1 để Đăng nhập như quản trị viên.");
            Console.WriteLine("\t\t(c) Nhấn 2 để Đăng ký như quản trị viên.");
            Console.WriteLine("\t\t(d) Nhấn 3 để Đăng nhập như hành khách.");
            Console.WriteLine("\t\t(e) Nhấn 4 để Đăng ký như hành khách.");
            Console.WriteLine("\t\t(f) Nhấn 5 để Hiển thị Hướng dẫn sử dụng.");
            Console.Write("\t\tNhập tùy chọn mong muốn:    ");

        }

        static void ManualInstructions()
        {
			Console.OutputEncoding = Encoding.Unicode;
			Console.WriteLine($"\n\n{new string(' ', 50)} +++++++++++++++++ Chào mừng bạn đến với Hướng dẫn sử dụng của Star Airlines +++++++++++++++++");
            Console.WriteLine("\n\n\t\t(a) Nhấn phím 1 để hiển thị Hướng dẫn quản trị.");
            Console.WriteLine("\t\t(b) Nhấn 2 để hiển thị Hướng dẫn sử dụng.");
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

        public static List<Customer> getCustomersCollection()
        {
            return customersCollection;
        }

    }

    
}
