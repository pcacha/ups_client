using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ups_client
{
    /*
     * Class represents one field of game board 
     */
    public class GameField
    {
        // whteher field contains stone
        public bool HasStone { get; set; }
        // is stone white
        public bool IsWhite { get; set; }
        // is stone king
        public bool IsKing { get; set; }
        // is field selected
        public bool IsSelected { get; set; }

        // constructor
        public GameField()
        {
            HasStone = false;
            IsWhite = false;
            IsKing = false;
            IsSelected = false;
        }       

        // sets field based on code
        public void SetEncodingBased(int code)
        {
            switch(code)
            {
                case 1:
                    HasStone = false;
                    IsWhite = false;
                    IsKing = false;
                    break;
                case 2:
                    HasStone = true;
                    IsWhite = true;
                    IsKing = false;
                    break;
                case 3:
                    HasStone = true;
                    IsWhite = false;
                    IsKing = false;
                    break;
                case 4:
                    HasStone = true;
                    IsWhite = true;
                    IsKing = true;
                    break;
                case 5:
                    HasStone = true;
                    IsWhite = false;
                    IsKing = true;
                    break;
            }
        }       
    }
}
