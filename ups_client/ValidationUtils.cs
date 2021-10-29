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

            if(login.Contains(Constants.msgStart) || login.Contains(Constants.msgFill) || login.Contains(Constants.msgSeparator))
            {
                return false;
            }

            return true;
        }

        internal static bool BasicMsgCheck(string msg)
        {
            if(msg[0] == Constants.msgStart[0] && msg[msg.Length - 1] == Constants.msgFill[0])
            {
                return true;
            }
            return false;
        }
    }
}
