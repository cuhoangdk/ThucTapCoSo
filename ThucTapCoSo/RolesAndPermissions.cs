using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapCoSo
{
    internal class RolesAndPermissions : User
    {
        // Fields inherited from the User class may need to be defined here.

        // ************************************************************ Behaviours/Methods ************************************************************

        /**
         * Checks if the admin with specified credentials is registered or not.
         * @param username of the imaginary admin
         * @param password of the imaginary admin
         * @return -1 if admin not found, else index of the admin in the array.
         */
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
