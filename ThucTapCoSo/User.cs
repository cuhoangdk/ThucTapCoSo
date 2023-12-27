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
            Console.InputEncoding = Encoding.Unicode;

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

                                Console.WriteLine("\t\t1  : THÊM TÀI KHOẢN NGƯỜI DÙNG MỚI");
                                Console.WriteLine("\t\t2  : TÌM KIẾM TÀI KHOẢN NGƯỜI DÙNG");
                                Console.WriteLine("\t\t3  : CẬP NHẬT TÀI KHOẢN NGƯỜI DÙNG");
                                Console.WriteLine("\t\t4  : XÓA TÀI KHOẢN NGƯỜI DÙNG");
                                Console.WriteLine("\t\t5  : HIỂN THỊ TẤT CẢ TÀI KHOẢN NGƯỜI DÙNG");
                                Console.WriteLine("\t\t6  : HIỂN THỊ TẤT CẢ CÁC VÉ ĐƯỢC ĐĂNG KÍ BỞI MỘT NGƯỜI DÙNG");
                                Console.WriteLine("\t\t7  : HIỂN THỊ HÀNH KHÁCH CỦA CHUYẾN BAY");
                                Console.WriteLine("\t\t8  : XÓA CHUYẾN BAY");
                                Console.WriteLine("\t\t9  : THÊM CHUYẾN BAY");
                                Console.WriteLine("\t\t10 : CHỈNH SỬA CHUYẾN BAY");
                                Console.WriteLine("\t\t11 : HIỂN THỊ TOÀN BỘ CHUYẾN BAY");
                                Console.WriteLine("\t\t0  : ĐĂNG XUẤT");


                                Console.Write("\n\tNhập tùy chọn:   ");
                                //fix code
                                while (!int.TryParse(Console.ReadLine(), out desiredOption))
                                {
                                    Console.Write("\tTuỳ chọn không hợp lệ, vui lòng nhập lại: ");
                                }

                                if (desiredOption == 1)
                                {
                                    c1.DisplayArtWork(1);
                                    c1.AddNewCustomer();
                                }
                                else if (desiredOption == 2)
                                {
                                    string name;
                                    string address;
                                    string phone;
                                    string customerID;
                                    string email;
                                    c1.DisplayArtWork(2);
                                    c1.DisplayCustomersData(false);
                                    Console.WriteLine("\tNhập thông tin khách hàng muốn tìm kiếm.");
                                    Console.WriteLine("\tBấm enter đê bỏ qua không nhập.");
                                    Console.Write("\tNhập mã khách hàng  :\t");
                                    customerID = Console.ReadLine();
                                    Console.Write("\tNhập tên khách hàng :\t");
                                    name = Console.ReadLine();
                                    Console.Write("\tNhập địa chỉ        :\t");
                                    address = Console.ReadLine();
                                    Console.Write("\tNhập số điện thoại  :\t");
                                    phone = Console.ReadLine();
                                    Console.Write("\tNhập email          :\t");
                                    email = Console.ReadLine();
                                    Console.WriteLine();
                                    c1.SearchUser(customerID,name,address,phone,email);
                                }
                                else if (desiredOption == 3)
                                {


                                    bookingAndReserving.DisplayArtWork(2);
                                    c1.DisplayCustomersData(false);
                                    Console.Write("\t NHẬP MÃ KHÁCH HÀNG MUỐN CẬP NHẬT DỮ LIỆU  :  ");
                                    string customerID = Console.ReadLine();
                                    c1.EditCustomerInfo(customerID);
                                }
                                else if (desiredOption == 4)
                                {
                                    bookingAndReserving.DisplayArtWork(3);
                                    c1.DisplayCustomersData(false);
                                    Console.Write("\tNHẬP MÃ KHÁCH HÀNG MUỐN XÓA     :  ");
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
                                    Console.Write("\n\n\tTẤT CẢ CÁC CHUYẾN BAY ĐƯỢC ĐẶT CHỖ BỞI NGƯỜI DÙNG CÓ ID : ");
                                    string id = Console.ReadLine();
                                    bookingAndReserving.DisplayFlightsRegisteredByOneUser(id);
                                    Console.WriteLine("\tNhập N/n để quay lại!");
                                    while (true)
                                    {
                                        Console.Write("\tNhập mã hóa đơn vé muốn xem chi tiết:");
                                        string trID = Console.ReadLine();
                                        bookingAndReserving.DisplayTicketRecept(id, trID);
                                        if (trID == "n" || trID == "N") break;
                                    }
                                }
                                else if (desiredOption == 7)
                                {
                                    c1.DisplayArtWork(4);
                                    Console.WriteLine("\t1  HIỂN THỊ HÀNH KHÁCH TẤT CẢ CHUYẾN BAY");
                                    Console.WriteLine("\t2  HIỂN THỊ HÀNH KHÁCH MỘT CHUYẾN BAY CỤ THỂ");
                                    char choice = Console.ReadLine()[0];
                                    if ('1' == choice)
                                    {
                                        bookingAndReserving.DisplayRegisteredUsersForAllFlight();
                                    }
                                    else if ('2' == choice)
                                    {
                                        f1.DisplayFlightSchedule();
                                        Console.Write("NHẬP MÃ CHUYẾN BAY MUỐN XEM TOÀN BỘ HÀNH KHÁCH:");
                                        string flightNum = Console.ReadLine().ToUpper();
                                        bookingAndReserving.DisplayRegisteredUsersForASpecificFlight(flightNum);
                                    }
                                    else
                                    {
                                        Console.WriteLine("\tLỰA CHỌN KHÔNG HỢP LỆ!");
                                    }
                                }
                                else if (desiredOption == 8)
                                {
                                    c1.DisplayArtWork(5);
                                    f1.DisplayFlightSchedule();
                                    Console.Write("\tNHẬP MÃ CHUYẾN BAY MUỐN XÓA : ");
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
                                    Console.WriteLine("\tNHẬP MÃ CHUYẾN BAY MUỐN CHỈNH SỬA: ");
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
                                    Console.WriteLine("\tCảm ơn bạn đã sử dụng Hệ thống Đặt vé của Star Airlines...!!!");
                                }
                                else
                                {
                                    Console.WriteLine("\tLựa chọn không hợp lệ...Có vẻ như bạn là Robot...Đang nhập giá trị ngẫu nhiên...Bạn phải đăng nhập lại...");
                                    bookingAndReserving.DisplayArtWork(22);
                                    desiredOption = 0;
                                }

                            } while (desiredOption != 0);
                            break;
                        }
                    }
                    if (!isFound)
                    {
                        Console.WriteLine($"\n\tLỖI!!! Không thể đăng nhập. Không thể tìm thấy người dùng với thông tin đăng nhập đã nhập.... Hãy Tạo Thông Tin Mới hoặc đăng ký bằng cách nhấn 2....", "");
                    }
                }
                else if (desiredOption == 2)
                {
                    string filePathAdmin = Path.Combine(datatxt, "Admin.txt");
                    string[] Admin = File.ReadAllLines(filePathAdmin);
                    PrintArtWork(2);
                    /*Nếu desiredOption là 2, hãy gọi phương thức đăng ký để đăng ký người dùng......*/
                    Console.Write("\n\tUSERNAME :    ");
                    string username = Console.ReadLine();
                    Console.Write("\n\tPASSWORD :    ");
                    string password = Console.ReadLine();
                    for(int i=0; i<Admin.Length; i++)
                    {
                        string[] dataAdmin = Admin[i].Split(';');

                        while ( dataAdmin[0].Equals(username) )
                        {
                            Console.Write("LỖI!!! Quản trị viên với cùng tên người dùng đã tồn tại. Hãy tạo tài khoản mới.");
                            Console.Write("\tUSERNAME :   ");
                            username = Console.ReadLine();
                            Console.Write("\tPASSWORD :   ");
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
                                Console.WriteLine("\t\t1 : ĐẶT CHỖ");
                                Console.WriteLine("\t\t2 : CẬP NHẬT THÔNG TIN TÀI KHOẢN");
                                Console.WriteLine("\t\t3 : XÓA TÀI KHOẢN");
                                Console.WriteLine("\t\t4 : XEM LỊCH TRÌNH TẤT CẢ CÁC CHUYẾN BAY");
                                Console.WriteLine("\t\t5 : HỦY VÉ BAY");
                                Console.WriteLine($"\t\t6 : XEM CÁC VÉ ĐÃ ĐẶT \"{userName}\"....");
                                Console.WriteLine("\t\t0 : ĐĂNG XUẤT ");
                                Console.Write("\t\tNHẬP TÙY CHỌN :   ");
                                while (!int.TryParse(Console.ReadLine(), out desiredChoice))
                                {
                                    Console.Write("LỰA CHỌN KHÔNG HỢP LỆ VUI LÒNG NHẬP LẠI:  ");
                                }
                                if (desiredChoice == 1)
                                {
                                    bookingAndReserving.DisplayArtWork(1);
                                    bookingAndReserving.BookFlight(dataC[1]);
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
                                    Console.WriteLine("\tNhập N/n để quay lại!");
                                    while (true)
                                    {
                                        Console.Write("\tNhập mã hóa đơn vé muốn xem chi tiết:");
                                        string trID = Console.ReadLine();
                                        bookingAndReserving.DisplayTicketRecept(dataC[1], trID);
                                        if (trID == "n" || trID == "N") break;
                                    }

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
        //Hàm hiển thị menu chính
        static void DisplayMainMenu()
        {
			Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("\n\n\t\t1 : ĐĂNG NHẬP QUẢN TRỊ VIÊN");
            Console.WriteLine("\t\t2 : ĐĂNG KÍ QUẢN TRỊ VIÊN");
            Console.WriteLine("\t\t3 : ĐĂNG NHẬP NGƯỜI DÙNG");
            Console.WriteLine("\t\t4 : ĐĂNG KÍ NGƯỜI DÙNG");
            Console.WriteLine("\t\t5 : HƯỚNG DẪN SỬ DỤNG");
            Console.WriteLine("\t\t0 : THOÁT");
            Console.Write("\t\tNHẬP TÙY CHỌN:    ");

        }
        //Hàm hiển thị hướng dẫn sử dụng
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
        //Hàm hiển thị banner chào mừng
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
        //Hàm hiển thị các banner
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
