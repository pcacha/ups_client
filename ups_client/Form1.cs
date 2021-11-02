using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ups_client.Constants;

namespace ups_client
{
    public partial class Form1 : Form
    {
        private Panel gamePanel;
        private Game game;
        private Panel[,] panels;
        private SocketManager socketManager;

        public Form1(SocketManager socketManager, Game game)
        {
            InitializeComponent();
            this.game = game;
            panels = new Panel[gameboardLength, gameboardLength];
            this.socketManager = socketManager;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateGameboard();          
            PrintGame();              
        }        

        private void CreateGameboard()
        {
            gamePanel = new Panel();
            gamePanel.Height = gameboardPanelSize * gameboardLength;
            gamePanel.Width = gameboardPanelSize * gameboardLength;
            gamePanel.Top = topMargin;
            gamePanel.Left = leftMargin;           
            gamePanel.Cursor = Cursors.Hand;
            this.Controls.Add(gamePanel);
            CreatePanels();
        }

        private void CreatePanels()
        {
            for (int i = 0; i < gameboardLength; i++)
            {
                for (int j = 0; j < gameboardLength; j++)
                {                    
                    Panel p = new Panel();
                    p.Click += new EventHandler(gameboardPanel_Click);
                    p.Height = gameboardPanelSize;
                    p.Width = gameboardPanelSize;
                    p.Top = i * gameboardPanelSize;
                    p.Left = j * gameboardPanelSize;
                    panels[i, j] = p;

                    if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0))
                    {
                        p.BackColor = whiteGameboardPanelColor;
                    }
                    else
                    {
                        p.BackColor = blackGameboardPanelColor;
                    }                                       

                    gamePanel.Controls.Add(p);
                }
            }
        }

        public void PrintGame()
        {
            PrintGameboard();
            PrintSideInfo();
        }

        private void PrintGameboard()
        {           
            GameField[,] gameFields = game.GameFields;            

            for (int i = 0; i < gameboardLength; i++)
            {
                for (int j = 0; j < gameboardLength; j++)
                {
                    GameField gf = gameFields[i, j];
                    Panel p = panels[i, j];                   

                    if (gf.IsSelected)
                    {
                        p.BackColor = selectedGameboardPanelColor;
                    }
                    else if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0))
                    {
                        p.BackColor = whiteGameboardPanelColor;
                    }
                    else
                    {
                        p.BackColor = blackGameboardPanelColor;
                    }

                    if (gf.HasStone)
                    {
                        if (gf.IsKing)
                        {
                            if (gf.IsWhite)
                            {
                                p.BackgroundImage = Image.FromFile(whiteKingPath);
                            }
                            else
                            {
                                p.BackgroundImage = Image.FromFile(blackKingPath);
                            }
                        }
                        else
                        {
                            if (gf.IsWhite)
                            {
                                p.BackgroundImage = Image.FromFile(whiteStonePath);
                            }
                            else
                            {
                                p.BackgroundImage = Image.FromFile(blackStonePath);
                            }
                        }
                        p.BackgroundImageLayout = ImageLayout.Center;
                    }
                    else
                    {
                        p.BackgroundImage = null;
                    }
                }                
            }
        }
        
        private void PrintSideInfo()
        {
            playerNameLabel.Text = game.PlayerName;
            opponentNameLabel.Text = game.OpponentName;
            playingNameLabel.Text = game.PlayingName;
            winnerNameLabel.Text = game.WinnerName;

            if(game.IsPlayerWhite)
            {
                playerStonePanel.BackgroundImage = Image.FromFile(whiteStonePath);
                opponentStonePanel.BackgroundImage = Image.FromFile(blackStonePath);
            }
            else
            {
                playerStonePanel.BackgroundImage = Image.FromFile(blackStonePath);
                opponentStonePanel.BackgroundImage = Image.FromFile(whiteStonePath);
            }
           
        }        

        private void clearSelectionBtn_Click(object sender, EventArgs e)
        {
            game.Select(-1, -1);
            PrintGame();
        }

        private void gameboardPanel_Click(object sender, EventArgs e)
        {
            GameField[,] gameFields = game.GameFields;
            Panel panel = (Panel)sender;
            Point location = panel.Location;
            int x = location.X / gameboardPanelSize;
            int y = location.Y / gameboardPanelSize;           
            
            if(game.GameState == GameStateEnum.IN_GAME)
            {
                if (!game.IsSelected)
                {
                    game.Select(x, y);
                }
                else
                {
                    if (game.GameState == GameStateEnum.IN_GAME && game.PlayerPlaying == true)
                    {
                        socketManager.Send(SendMsgUtils.Move(game.SelectedY, game.SelectedX, y, x));
                    }

                    game.Select(-1, -1);
                }

                PrintGame();
            }
        }

        public void HandleGame(string[] msgParts)
        {
            if((game.GameState == GameStateEnum.IN_GAME || game.GameState == GameStateEnum.QUEUED) && msgParts.Length == 5)
            {
                if(game.GameState == GameStateEnum.QUEUED)
                {
                    game.GameState = GameStateEnum.IN_GAME;
                }

                int[] gameboardEncoded = GetEncodedGameboard(msgParts[4]);

                if(gameboardEncoded == null)
                {
                    socketManager.CloseSocket();
                }

                game.OpponentName = msgParts[1];

                string playing = msgParts[2];
                if(playing == Constants.msgNull)
                {
                    game.PlayerPlaying = null;
                }
                else if (playing == game.PlayerName)
                {
                    game.PlayerPlaying = true;
                } 
                else
                {
                    game.PlayerPlaying = false;
                }

                string winner = msgParts[3];
                if(winner == Constants.msgNull)
                {
                    game.WinnerName = Constants.fieldEmpty;
                }
                else
                {
                    if(winner != game.PlayerName && winner != game.OpponentName)
                    {
                        socketManager.CloseSocket();
                    }
                    else
                    {
                        game.WinnerName = winner;
                        game.GameState = GameStateEnum.FINISHED;
                    }
                }

                game.UpdateGameboard(gameboardEncoded);
                game.Select(-1, -1);
                PrintGame();
            }
            else
            {
                socketManager.CloseSocket();
            }
        }

        private int[] GetEncodedGameboard(string gameboardString)
        {
            if(gameboardString.Length != Constants.gameboardLength * Constants.gameboardLength)
            {
                return null;
            }

            int[] gameboardEncoded = new int[Constants.gameboardLength * Constants.gameboardLength];

            for(int i = 0; i < gameboardString.Length; i++)
            {
                if(gameboardString[i] != '1' && gameboardString[i] != '2' && gameboardString[i] != '3' && gameboardString[i] != '4' && gameboardString[i] != '5')
                {
                    return null;
                }

                gameboardEncoded[i] = Int32.Parse(Char.ToString(gameboardString[i]));
            }


            return gameboardEncoded;
        }

        public void HandleMoveFailed(string[] msgParts)
        {
            if(game.GameState == GameStateEnum.IN_GAME && msgParts.Length == 2 && msgParts[1] == Constants.failed)
            {
                MessageBox.Show(Constants.moveFailedMsg);
            }
            else
            {
                socketManager.CloseSocket();
            }
        }
    }
}
