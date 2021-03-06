using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ups_client
{
    // enum definign state of game
    public enum GameStateEnum
    {
        INIT,
        QUEUED,
        IN_GAME,
        FINISHED,
    }

    /*
     * Class represents game state
     */
    public class Game
    {
        // name of player
        public string PlayerName { get; set; }
        // whether server is accessible
        public bool ServerOnline { get; set; }
        // last time server pinged
        public DateTime LastPingTimestamp { get; set; }
        // name of player's opponent
        public string OpponentName { get; set; }
        // whether is opponent online
        public bool OpponentOnline { get; set; }
        // color of player
        public bool IsPlayerWhite { get; set; }       
        // is logged in player on his turn
        public bool? PlayerPlaying { get; set; }
        // name of playing player
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

        // name of winner
        public string WinnerName { get; set; }

        // whether is field of game board selected
        public bool IsSelected { get; private set; }
        // selected field x coordinate
        public int SelectedX { get; private set; }
        // selected field y coordinate
        public int SelectedY { get; private set; }
        // 2D array of fields of game board
        public GameField[,] GameFields { get; set; }
        // game state
        public GameStateEnum GameState { get; set; }

        // constructor
        public Game()
        {
            PlayerName = Constants.fieldEmpty;
            ServerOnline = true;
            LastPingTimestamp = DateTime.Now;
            OpponentName = Constants.fieldEmpty;
            OpponentOnline = true;
            IsPlayerWhite = true;
            PlayerPlaying = null;
            
            // initialize game fields
            GameFields = new GameField[Constants.gameboardLength, Constants.gameboardLength];
            for (int i = 0; i < Constants.gameboardLength; i++)
            {
                for(int j = 0; j < Constants.gameboardLength; j++)
                {
                    GameFields[i, j] = new GameField();
                }
            }

            WinnerName = Constants.fieldEmpty;
            GameState = GameStateEnum.INIT;
        }

        // selects picked field of game board
        public void Select(int x, int y)
        {
            if(x < 0 || y < 0)
            {
                // delte selection
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
                // select field
                IsSelected = true;
                SelectedX = x;
                SelectedY = y;
                GameFields[SelectedY, SelectedX].IsSelected = true;                
            }
        }

        // update gameboard based on field codes
        public void UpdateGameboard(int[] gameboardEncoded)
        {
            int counter = 0;

            // foreach field of game board
            for (int i = 0; i < Constants.gameboardLength; i++)
            {
                for (int j = 0; j < Constants.gameboardLength; j++)
                {
                    GameFields[i, j].SetEncodingBased(gameboardEncoded[counter]);
                    counter++;
                }
            }            
        }

        // reset game to init state
        public void Reset()
        {            
            OpponentName = Constants.fieldEmpty;
            OpponentOnline = true;
            IsPlayerWhite = true;
            PlayerPlaying = null;
            WinnerName = Constants.fieldEmpty;
            GameState = GameStateEnum.INIT;
            Select(-1, -1);

            // reset gameboard
            for (int i = 0; i < Constants.gameboardLength; i++)
            {
                for (int j = 0; j < Constants.gameboardLength; j++)
                {
                    GameFields[i, j].Reset();
                }
            }
        }
    }
}
