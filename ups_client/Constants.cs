using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ups_client
{
    /*
     * Class containing all program constants
     */
    public static class Constants
    {
        // top margin of game board [pixels]
        public const int topMargin = 12;
        // left margin of game board [pixels]
        public const int leftMargin = 12;
        // length of one side of square game board in game fields
        public const int gameboardLength = 8;
        // game board panel (field) size [pixels]
        public const int gameboardPanelSize = 70;
        // game board stone size [pixels]
        public const int gameboardStoneSize = 50;
        // color for black field of game board
        public static Color blackGameboardPanelColor = Color.FromArgb(182, 135, 107);
        // color for white field of game board
        public static Color whiteGameboardPanelColor = Color.FromArgb(246, 223, 192);
        // color for selected field of game board
        public static Color selectedGameboardPanelColor = Color.FromArgb(252, 243, 127);        
        // path to black stone igm
        public const string blackStonePath = "img/black.png";
        // path to black king img
        public const string blackKingPath = "img/black_king.png";
        // path to white stone img
        public const string whiteStonePath = "img/white.png";
        // path to white king img
        public const string whiteKingPath = "img/white_king.png";
        // max length of player's name
        public const int maxNameLen = 10;
        // max length of data in file descriptor
        public const int maxMsgBatchLength = 500;
        // max length of protocol message
        public const int maxMsgLength = 110;
        // min length of protocol message
        public const int minMsgLength = 4;
        // indicator of message start
        public const string msgStart = "$";
        // message parts separator
        public const string msgSeparator = "|";
        // indicator of message end
        public const string msgEnd = "#";
        // indicator of null in message
        public const string msgNull = "@";
        // connect key word
        public const string connect = "CONNECT";
        // game key word
        public const string game = "GAME";
        // ok key word
        public const string connectOk = "OK";
        // move key word
        public const string move = "MOVE";
        // invalid name key word
        public const string connectInvalid = "INVALID_NAME";
        // white key word
        public const string white = "WHITE";
        // black key word
        public const string black = "BLACK";
        // failed key word
        public const string failed = "FAILED";
        // leave key word
        public const string leave = "LEAVE";
        // empty field representation
        public const string fieldEmpty = "-";
        // message regex structure
        public const string msgRegex = @"\" + msgStart + @"[^\" + msgStart + @"\" + msgEnd + @"]*\" + msgEnd;
        // invalid name notification
        public const string invalidNameMsg = "Přezdívka není validní";
        // taken name notification
        public const string takenNameMsg = "Přezdívka je již zabraná"; 
        // move failed notification
        public const string moveFailedMsg = "Byl zaznamenán neplatný tah!";
        // winner notification
        public const string winnerPopupMsg = "Zvítězil hráč ";
        // draw notification
        public const string drawPopupMsg = "Hra skončila remízou";
    }
}
