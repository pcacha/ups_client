using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ups_client
{
    /*
     * Class containing utility methods for validation
     */
    public class ValidationUtils
    {
        // validates user login
        public static bool ValidateLogin(string login)
        {
            // check reight length
            if(login.Length == 0 || login.Length > Constants.maxNameLen)
            {
                return false;
            }

            // check restricted chars
            if(login.Contains(Constants.msgStart) || login.Contains(Constants.msgEnd) || login.Contains(Constants.msgSeparator) || login.Contains(Constants.msgNull))
            {
                return false;
            }

            return true;
        }        
    }
}
