using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ups_client
{
    public class GameField
    {
        public bool HasStone { get; set; }
        public bool IsWhite { get; set; }
        public bool IsKing { get; set; }
        public bool IsSelected { get; set; }

        public GameField()
        {
            HasStone = false;
            IsWhite = false;
            IsKing = false;
            IsSelected = false;
        }       

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
