using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ups_client.Constants;

namespace ups_client
{
    public enum GameStateEnum
    {
        INIT,
        QUEUED,
        IN_GAME,
        FINISHED,
    }

    public class Game
    {
        public string PlayerName { get; set; }
        public string OpponentName { get; set; }

        public bool IsPlayerWhite { get; set; }

        public bool? PlayerPlaying { get; set; }
        public string PlayingName {
            get
            {
                if(GameState != GameStateEnum.IN_GAME)
                {
                    return Constants.fieldEmpty;
                }
                else if (PlayerPlaying == true)
                {
                    return PlayerName;
                }
                else
                {
                    return OpponentName;
                }
            }
        }

        public string WinnerName { get; set; }

        public bool IsSelected { get; private set; }
        public int SelectedX { get; private set; }
        public int SelectedY { get; private set; }

        public GameField[,] GameFields { get; set; }

        public GameStateEnum GameState { get; set; }

        public Game()
        {
            PlayerName = Constants.fieldEmpty;
            OpponentName = Constants.fieldEmpty;
            IsPlayerWhite = true;
            PlayerPlaying = null;
            
            GameFields = new GameField[gameboardLength, gameboardLength];
            for (int i = 0; i < gameboardLength; i++)
            {
                for(int j = 0; j < gameboardLength; j++)
                {
                    GameFields[i, j] = new GameField();
                }
            }

            WinnerName = Constants.fieldEmpty;
            GameState = GameStateEnum.INIT;
        }

        public void Select(int x, int y)
        {
            if(x < 0 || y < 0)
            {
                if(SelectedX > -1 && SelectedY > -1)
                {
                    GameFields[SelectedY, SelectedX].IsSelected = false;
                }
                IsSelected = false;
                SelectedX = -1;
                SelectedY = -1;
            }
            else
            {
                IsSelected = true;
                SelectedX = x;
                SelectedY = y;
                GameFields[SelectedY, SelectedX].IsSelected = true;                
            }
        }

        public void UpdateGameboard(int[] gameboardEncoded)
        {
            int counter = 0;

            for (int i = 0; i < gameboardLength; i++)
            {
                for (int j = 0; j < gameboardLength; j++)
                {
                    GameFields[i, j].SetEncodingBased(gameboardEncoded[counter]);
                    counter++;
                }
            }            
        }
    }
}
