using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ups_client
{
    public static class Constants
    {
        public const int topMargin = 12;
        public const int leftMargin = 12;
        public const int gameboardLength = 8;
        public const int gameboardPanelSize = 70;
        public const int gameboardStoneSize = 50;
        public static Color blackGameboardPanelColor = Color.FromArgb(182, 135, 107);
        public static Color whiteGameboardPanelColor = Color.FromArgb(246, 223, 192);
        public static Color selectedGameboardPanelColor = Color.FromArgb(252, 243, 127);        
        public const string blackStonePath = "img/black.png";
        public const string blackKingPath = "img/black_king.png";
        public const string whiteStonePath = "img/white.png";
        public const string whiteKingPath = "img/white_king.png";
        public const int maxNameLen = 10;

        public const int maxMsgLength = 250;
        public const int minMsgLength = 4;
        public const string msgStart = "$";
        public const string msgSeparator = "|";
        public const string msgEnd = "#";
        public const string msgNull = "@";
        public const string connect = "CONNECT";
        public const string game = "GAME";
        public const string connectOk = "OK";
        public const string move = "MOVE";
        public const string connectInvalid = "INVALID_NAME";
        public const string white = "WHITE";
        public const string black = "BLACK";
        public const string failed = "FAILED";

        public const string invalidNameMsg = "Přezdívka není validní";
        public const string takenNameMsg = "Přezdívka je již zabraná"; 
        public const string moveFailedMsg = "Byl zaznamenán neplatný tah!";
    }
}
