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
			Console.OutputEncoding = Encoding.Unicode;
			int countNumOfUsers = 1;
            RolesAndPermissions r1 = new RolesAndPermissions();
            Flight f1 = new Flight();
            FlightReservation bookingAndReserving = new FlightReservation();
            Customer c1 = new Customer();
            f1.FlightScheduler();
            Console.WriteLine();
            WelcomeScreen(1);
            Console.WriteLine("\n\t\t\t\t\t+++++++++++++ Welcome to Star AirLines +++++++++++++\n\nTo Further Proceed, Please enter a value.");
            Console.WriteLine("\n***** Default Username && Password is root-root ***** Using Default Credentials will restrict you to just view the list of Passengers....\n");
            DisplayMainMenu();
            int desiredOption;
            //fix code
			while (!int.TryParse(Console.ReadLine(), out desiredOption))
			{
				Console.Write("Invalid input. Please enter a valid number: ");
			}			         
            while (desiredOption < 0 || desiredOption > 8)
            {
				while (!int.TryParse(Console.ReadLine(), out desiredOption))
				{
					Console.Write("Invalid input. Please enter a valid number: ");
				}
				Console.Write("ERROR!! Please enter value between 0 - 4. Enter the value again :\t");
                desiredOption = Convert.ToInt32(Console.ReadLine());
            }

            do
            {
                /* If desiredOption is 1 then call the login method.... if default credentials are used then set the permission
                 * level to standard/default where the user can just view the customer's data...if not found, then return -1, and if
                 * data is found then show the user display menu for adding, updating, deleting and searching users/customers...
                 * */
                if (desiredOption == 1)
                {
                    /*Default username and password....*/
                    adminUserNameAndPassword[0,0] = "root";
                    adminUserNameAndPassword[0,1] = "root";
                    PrintArtWork(1);
                    Console.Write("\nEnter the UserName to login to the Management System :     ");
                    string username = Console.ReadLine();
                    Console.Write("Enter the Password to login to the Management System :    ");
                    string password = Console.ReadLine();
                    Console.WriteLine();

                    /*Checking the RolesAndPermissions......*/
                    if (r1.IsPrivilegedUserOrNot(username, password) == -1)
                    {
                        Console.WriteLine($"\n{"", 20}ERROR!!! Unable to login Cannot find user with the entered credentials.... Try Creating New Credentials or get yourself register by pressing 4....", "");
                    }
                    else if (r1.IsPrivilegedUserOrNot(username, password) == 0)
                    {
                        Console.WriteLine("You've standard/default privileges to access the data... You can just view customers data... Can't perform any actions on them....");
                        c1.DisplayCustomersData(true);
                    }
                    else
                    {						
                        // In ra màn hình để kiểm tra						
						Console.WriteLine($"{"",-20}Logged in Successfully as \"{username}\"..... For further Proceedings, enter a value from below....", "");

                        /*Going to Display the CRUD operations to be performed by the privileged user.....Which includes Creating, Updating
                         * Reading(Searching) and deleting a customer....
                         * */
                        do
                        {
                            Console.WriteLine($"\n\n+++++++++ 2nd Layer Menu +++++++++ Logged in as \"{username}\"\n", "", "");

                            Console.WriteLine("(a) Enter 1 to add new Passenger....");
                            Console.WriteLine("(b) Enter 2 to search a Passenger....");
                            Console.WriteLine("(c) Enter 3 to update the Data of the Passenger....");
                            Console.WriteLine("(d) Enter 4 to delete a Passenger....");
                            Console.WriteLine("(e) Enter 5 to Display all Passengers....");
                            Console.WriteLine("(f) Enter 6 to Display all flights registered by a Passenger...");
                            Console.WriteLine("(g) Enter 7 to Display all registered Passengers in a Flight....");
                            Console.WriteLine("(h) Enter 8 to Delete a Flight....");
                            Console.WriteLine("(i) Enter 0 to Go back to the Main Menu/Logout....");

                            Console.Write("Enter the desired Choice :   ");
                            //fix code
							while (!int.TryParse(Console.ReadLine(), out desiredOption))
							{
								Console.Write("Invalid input. Please enter a valid number: ");
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
                                Console.Write("Enter the CustomerID to Search :\t");
                                string customerID = Console.ReadLine();
                                Console.WriteLine();
                                c1.SearchUser(customerID);
                            }
                            else if (desiredOption == 3)
                            {
                                /*If 3 is entered by the user, then call the update method of the Customer Class with required
                                 * arguments.....
                                 * */
                                bookingAndReserving.DisplayArtWork(2);
                                c1.DisplayCustomersData(false);
                                Console.Write("Enter the CustomerID to Update its Data :\t");
                                string customerID = Console.ReadLine();
                                if (customersCollection.Count > 0)
                                {
                                    c1.EditUserInfo(customerID);
                                }
                                else
                                {
                                    Console.WriteLine($"No Customer with the ID {customerID} Found...!!!", " ");
                                }
                            }
                            else if (desiredOption == 4)
                            {
                                /*If 4 is entered, then ask the user to enter the customer id, and then delete
                                 * that customer....
                                 * */
                                bookingAndReserving.DisplayArtWork(3);
                                c1.DisplayCustomersData(false);
                                Console.Write("Enter the CustomerID to Delete its Data :\t");
                                string customerID = Console.ReadLine();
                                if (customersCollection.Count > 0)
                                {
                                    c1.DeleteUser(customerID);
                                }
                                else
                                {
                                    Console.WriteLine($"{"",-50}No Customer with the ID {customerID} Found...!!!", " ");
                                }
                            }
                            else if (desiredOption == 5)
                            {
                                /*Call the Display Method of Customer Class....*/
                                c1.DisplayArtWork(3);
                                c1.DisplayCustomersData(false);
                            }
                            else if (desiredOption == 6)
                            {
                                bookingAndReserving.DisplayArtWork(6);
                                c1.DisplayCustomersData(false);
                                Console.Write("\n\nEnter the ID of the user to display all flights registered by that user...");
                                string id = Console.ReadLine();
                                bookingAndReserving.DisplayFlightsRegisteredByOneUser(id);
                            }
                            else if (desiredOption == 7)
                            {
                                c1.DisplayArtWork(4);
                                Console.Write("Do you want to display Passengers of all flights or a specific flight.... 'Y/y' for displaying all flights and 'N/n' to look for a" +
                                        " specific flight.... ");
                                char choice = Console.ReadLine()[0];
                                if ('y' == choice || 'Y' == choice)
                                {
                                    bookingAndReserving.DisplayRegisteredUsersForAllFlight();
                                }
                                else if ('n' == choice || 'N' == choice)
                                {
                                    f1.DisplayFlightSchedule();
                                    Console.Write("Enter the Flight Number to display the list of passengers registered in that flight... ");
                                    string flightNum = Console.ReadLine();
                                    bookingAndReserving.DisplayRegisteredUsersForASpecificFlight(flightNum);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Choice...No Response...!");
                                }
                            }
                            else if (desiredOption == 8)
                            {
                                c1.DisplayArtWork(5);
                                f1.DisplayFlightSchedule();
                                Console.Write("Enter the Flight Number to delete the flight : ");
                                string flightNum = Console.ReadLine();
                                f1.DeleteFlight(flightNum);

                            }
                            else if (desiredOption == 0)
                            {
                                bookingAndReserving.DisplayArtWork(22);
                                Console.WriteLine("Thanks for Using BAV Airlines Ticketing System...!!!");
                            }
                            else
                            {
                                Console.WriteLine("Invalid Choice...Looks like you're Robot...Entering values randomly...You've Have to login again...");
                                bookingAndReserving.DisplayArtWork(22);
                                desiredOption = 0;
                            }

                        } while (desiredOption != 0);

                    }
                }
                else if (desiredOption == 2)
                {
                    PrintArtWork(2);
                    /*If desiredOption is 2, then call the registration method to register a user......*/
                    Console.Write("\nEnter the UserName to Register :    ");
                    string username = Console.ReadLine();
                    Console.Write("Enter the Password to Register :     ");
                    string password = Console.ReadLine();
                    while (r1.IsPrivilegedUserOrNot(username, password) != -1)
                    {
                        Console.Write("ERROR!!! Admin with same UserName already exist. Enter new UserName:   ");
                        username = Console.ReadLine();
                        Console.Write("Enter the Password Again:   ");
                        password = Console.ReadLine();
                    }

                    /*Setting the credentials entered by the user.....*/
                    adminUserNameAndPassword[countNumOfUsers, 0] = username;
                    adminUserNameAndPassword[countNumOfUsers, 1] = password;

                    /*Incrementing the numOfUsers */
                    countNumOfUsers++;
                }
                else if (desiredOption == 3)
                {
                    PrintArtWork(3);
                    Console.Write("\n\nEnter the Email to Login : \t");
                    string userName = Console.ReadLine();
                    Console.Write("Enter the Password : \t");
                    string password = Console.ReadLine();
                    string[] result = r1.IsPassengerRegistered(userName, password).Split('-');

                    if (Convert.ToInt32(result[0]) == 1)
                    {
                        int desiredChoice;
                        Console.WriteLine($"\n\n{"",-20}Logged in Successfully as \"{userName}\"..... For further Proceedings, enter a value from below....");
                        do
                        {
                            Console.WriteLine($"\n\n{"",-60}+++++++++ 3rd Layer Menu +++++++++{"",50}Logged in as \"{userName}\"\n");
                            Console.WriteLine("(a) Enter 1 to Book a flight....");
                            Console.WriteLine("(b) Enter 2 to update your Data....");
                            Console.WriteLine("(c) Enter 3 to delete your account....");
                            Console.WriteLine("(d) Enter 4 to Display Flight Schedule....");
                            Console.WriteLine("(e) Enter 5 to Cancel a Flight....");
                            Console.WriteLine($"(f) Enter 6 to Display all flights registered by \"{userName}\"....");
                            Console.WriteLine("(g) Enter 0 to Go back to the Main Menu/Logout....");
                            Console.Write("Enter the desired Choice :   ");
                            desiredChoice = Convert.ToInt32(Console.ReadLine());
                            if (desiredChoice == 1)
                            {
                                bookingAndReserving.DisplayArtWork(1);
                                f1.DisplayFlightSchedule();
                                Console.Write("\nEnter the desired flight number to book :\t ");
                                string flightToBeBooked = Console.ReadLine();
                                Console.Write($"Enter the Number of tickets for {flightToBeBooked} flight :   ");
                                int numOfTickets;
								while (!int.TryParse(Console.ReadLine(), out numOfTickets))
								{
									Console.Write("Invalid input. Please enter a valid number: ");
								}
								while (numOfTickets > 10)
                                {
									while (!int.TryParse(Console.ReadLine(), out numOfTickets))
									{
										Console.Write("Invalid input. Please enter a valid number: ");
									}
									Console.Write("ERROR!! You can't book more than 10 tickets at a time for single flight....Enter number of tickets again : ");
                                    numOfTickets = int.Parse(Console.ReadLine());
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
                                Console.Write("Are you sure to delete your account...It's an irreversible action...Enter Y/y to confirm...");
                                char confirmationChar = Console.ReadLine()[0];
                                if (confirmationChar == 'Y' || confirmationChar == 'y')
                                {
                                    c1.DeleteUser(result[1]);
                                    Console.WriteLine($"User {userName}'s account deleted Successfully...!!!");
                                    desiredChoice = 0;
                                }
                                else
                                {
                                    Console.WriteLine("Action has been cancelled...");
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
				while (!int.TryParse(Console.ReadLine(), out desiredOption))
				{
					Console.Write("Invalid input. Please enter a valid number: ");
				}
				while (desiredOption < 0 || desiredOption > 8)
                {
					while (!int.TryParse(Console.ReadLine(), out desiredOption))
					{
						Console.Write("Invalid input. Please enter a valid number: ");
					}
					Console.Write("ERROR!! Please enter value between 0 - 4. Enter the value again :\t");
                    desiredOption = Convert.ToInt32(Console.ReadLine());
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
			while (!int.TryParse(Console.ReadLine(), out choice))
			{
				Console.Write("Đầu vào không hợp lệ. Vui lòng nhập một số hợp lệ:  ");
			}
			while (choice < 1 || choice > 2)
            {
				while (!int.TryParse(Console.ReadLine(), out choice))
				{
					Console.Write("Đầu vào không hợp lệ. Vui lòng nhập một số hợp lệ:  ");
				}
				Console.Write("LỖI!!! Mục nhập không hợp lệ...Vui lòng nhập giá trị 1 hoặc 2....Nhập lại....  ");
                choice = Convert.ToInt32(Console.ReadLine());
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
                Console.WriteLine("(10) Nhấn \"6\" sẽ cho phép bạn hiển thị tất cả hành khách đã đăng ký... Sau khi chọn, chương trình sẽ hỏi bạn có muốn hiển thị hành khách cho tất cả các chuyến bay (Y/y) hay một chuyến bay cụ thể (N/n)\n");
                Console.WriteLine("(11) Nhấn \"7\" sẽ cho phép bạn xóa bất kỳ chuyến bay nào, miễn là số hiệu chuyến bay được cung cấp...\n");
                Console.WriteLine("(11) Nhấn \"0\" sẽ khiến bạn đăng xuất khỏi chương trình... Bạn có thể đăng nhập lại bất cứ lúc nào trong quá trình thực hiện chương trình....\n");

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
                Console.WriteLine("(9) Nhấn \"4\" sẽ hiển thị một lịch trình chuyến bay được thiết kế ngẫu nhiên cho chạy...\n");
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
