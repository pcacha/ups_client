using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ups_client
{
    public class SendMsgUtils
    {
        public static string Login(string nick)
        {
            return AddHashtags(Constants.msgStart + Constants.connect + Constants.msgSeparator + nick);
        }

        private static string AddHashtags(string msg)
        {
            for(int i = msg.Length; i < Constants.msgLength; i++)
            {
                msg += Constants.msgFill;
            }
            return msg;
        }
    }
}
