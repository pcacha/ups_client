using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ups_client
{
    public class ValidationUtils
    {
        public static bool ValidateLogin(string login)
        {
            if(login.Length == 0 || login.Length > Constants.maxNameLen)
            {
                return false;
            }

            if(login.Contains(Constants.msgStart) || login.Contains(Constants.msgEnd) || login.Contains(Constants.msgSeparator) || login.Contains(Constants.msgNull))
            {
                return false;
            }

            return true;
        }        
    }
}
