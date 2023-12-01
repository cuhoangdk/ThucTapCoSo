using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    internal class RolesAndPermissions : User
    {
        // Có thể cần định nghĩa lại các trường được kế thừa từ lớp User ở đây.

        // ************************************************************ Behaviours/Methods ************************************************************

        /// <summary>
        /// Kiểm tra xem người quản trị có đăng ký với thông tin đăng nhập cụ thể không.
        /// </summary>
        /// <param name="username">Tên người quản trị ảo</param>
        /// <param name="password">Mật khẩu của người quản trị ảo</param>
        /// <returns>-1 nếu không tìm thấy người quản trị, ngược lại trả về chỉ mục của người quản trị trong mảng.</returns>
        public int IsPrivilegedUserOrNot(string username, string password)
        {
            int isFound = -1;
            for (int i = 0; i < adminUserNameAndPassword.GetLength(0); i++)
            {
                if (username.Equals(adminUserNameAndPassword[i,0]))
                {
                    if (password.Equals(adminUserNameAndPassword[i, 1]))
                    {
                        isFound = i;
                        break;
                    }
                }
            }
            return isFound;
        }

        /**
         * Checks if the passenger with specified credentials is registered or not.
         * @param email of the specified passenger
         * @param password of the specified passenger
         * @return "0" if the passenger is not registered, else "1-userID"
         */
        public string IsPassengerRegistered(string email, string password)
        {
            string isFound = "0";
            foreach (Customer c in Customer.customerCollection)
            {
                if (email.Equals(c.GetEmail()))
                {
                    if (password.Equals(c.GetPassword()))
                    {
                        isFound = "1-" + c.GetUserID();
                        break;
                    }
                }
            }
            return isFound;
        }
    }
 }
