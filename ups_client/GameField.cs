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
    }
}
