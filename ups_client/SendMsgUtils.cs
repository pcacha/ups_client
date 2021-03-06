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

        // returns leave message
        public static string Leave()
        {
            return Constants.msgStart + Constants.leave + Constants.msgEnd;
        }

        // returns play again message
        public static string PlayAgain()
        {
            return Constants.msgStart + Constants.playAgain + Constants.msgEnd;
        }

        // returns pong message
        public static string Pong()
        {
            return Constants.msgStart + Constants.pong + Constants.msgEnd;
        }
    }
}
