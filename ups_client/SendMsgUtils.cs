using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ups_client
{
    /*
     * Class containting utility methods for sending to server
     */ 
    public class SendMsgUtils
    {
        // returns login message
        public static string Login(string nick)
        {
            return Constants.msgStart + Constants.connect + Constants.msgSeparator + nick + Constants.msgEnd;
        }

        // returns move message
        public static string Move(int sourceY, int sourceX, int goalY, int goalX)
        {
            return Constants.msgStart + Constants.move + Constants.msgSeparator + sourceY + Constants.msgSeparator + sourceX + Constants.msgSeparator + 
                goalY + Constants.msgSeparator + goalX + Constants.msgEnd;
        }
    }
}
