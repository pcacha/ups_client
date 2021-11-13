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
    /*
     * Class represents main game board form
     */
    public partial class Form1 : Form
    {
        // panel with game board
        private Panel gamePanel;
        // representation of game
        private Game game;
        // fields of game board
        private Panel[,] panels;
        // socket manager
        private SocketManager socketManager;
        // if leave was send
        private bool leaveSend;

        private Image whiteStoneImg;
        private Image blackStoneImg;
        private Image whiteKingImg;
        private Image blackKingImg;

        // constructor
        public Form1(SocketManager socketManager, Game game)
        {
            InitializeComponent();
            this.game = game;
            panels = new Panel[gameboardLength, gameboardLength];
            this.socketManager = socketManager;
            leaveSend = false;

            whiteStoneImg = Image.FromFile(whiteStonePath);
            blackStoneImg = Image.FromFile(blackStonePath);
            whiteKingImg = Image.FromFile(whiteKingPath);
            blackKingImg = Image.FromFile(blackKingPath);
        }

        // on load event
        private void Form1_Load(object sender, EventArgs e)
        {
            // create gameboard and print actual state
            CreateGameboard();          
            PrintGame();              
        }        

        // prepare game board for playing
        private void CreateGameboard()
        {
            // create main panel, set its properties and add it to form
            Invoke(new Action(() => 
            {
                playAgainBtn.Visible = false;
                gamePanel = new Panel();
                gamePanel.Height = gameboardPanelSize * gameboardLength;
                gamePanel.Width = gameboardPanelSize * gameboardLength;
                gamePanel.Top = topMargin;
                gamePanel.Left = leftMargin;
                gamePanel.Cursor = Cursors.Hand;
                this.Controls.Add(gamePanel);
            }));            
            CreatePanels();
        }

        // creates fields of game board
        private void CreatePanels()
        {
            Invoke(new Action(() =>
            {
                // for all fields in game board
                for (int i = 0; i < gameboardLength; i++)
                {
                    for (int j = 0; j < gameboardLength; j++)
                    {
                        // create panel
                        Panel p = new Panel();
                        p.Click += new EventHandler(gameboardPanel_Click);
                        p.Height = gameboardPanelSize;
                        p.Width = gameboardPanelSize;
                        p.Top = i * gameboardPanelSize;
                        p.Left = j * gameboardPanelSize;
                        panels[i, j] = p;

                        // set its color and add it to form
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
            }));            
        }

        // prints the game based on actual state
        public void PrintGame()
        {
            PrintGameboard();
            PrintSideInfo();
        }

        // prints gameboard based on current game state
        private void PrintGameboard()
        {
            Invoke(new Action(() =>
            {
                GameField[,] gameFields = game.GameFields;

                // for each field of game board
                for (int i = 0; i < gameboardLength; i++)
                {
                    for (int j = 0; j < gameboardLength; j++)
                    {
                        GameField gf = gameFields[i, j];
                        Panel p = panels[i, j];

                        // set field color
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

                        // set background image if field contains stone
                        if (gf.HasStone)
                        {
                            if (gf.IsKing)
                            {
                                if (gf.IsWhite)
                                {
                                    p.BackgroundImage = whiteKingImg;
                                }
                                else
                                {
                                    p.BackgroundImage = blackKingImg;
                                }
                            }
                            else
                            {
                                if (gf.IsWhite)
                                {
                                    p.BackgroundImage = whiteStoneImg;
                                }
                                else
                                {
                                    p.BackgroundImage = blackStoneImg;
                                }
                            }
                            p.BackgroundImageLayout = ImageLayout.Center;
                        }
                        else
                        {
                            // delete image if there is no stone
                            p.BackgroundImage = null;
                        }
                    }
                }
            }));            
        }
        
        // prints side info based on current state
        private void PrintSideInfo()
        {
            Invoke(new Action(() =>
            {
                playerNameLabel.Text = game.PlayerName;
                opponentNameLabel.Text = game.OpponentName;
                playingNameLabel.Text = game.PlayingName;
                winnerNameLabel.Text = game.WinnerName;

                // set color of label based on whether server is accessible or not
                if (game.ServerOnline)
                {
                    playerNameLabel.ForeColor = Color.Green;
                }
                else
                {
                    playerNameLabel.ForeColor = Color.Red;
                }

                // set color of label based on whether opponent is online or not
                if (game.OpponentOnline)
                {
                    opponentNameLabel.ForeColor = Color.Green;
                }
                else
                {
                    opponentNameLabel.ForeColor = Color.Red;
                }

                if (game.IsPlayerWhite)
                {
                    // for white player
                    playerStonePanel.BackgroundImage = Image.FromFile(whiteStonePath);
                    opponentStonePanel.BackgroundImage = Image.FromFile(blackStonePath);
                }
                else
                {
                    // for black player
                    playerStonePanel.BackgroundImage = Image.FromFile(blackStonePath);
                    opponentStonePanel.BackgroundImage = Image.FromFile(whiteStonePath);
                }
            }));          
        }        

        // delte selection btn event
        private void clearSelectionBtn_Click(object sender, EventArgs e)
        {
            // select no field and print the game
            game.Select(-1, -1);
            PrintGame();
        }

        // filed of game board click event
        private void gameboardPanel_Click(object sender, EventArgs e)
        {
            GameField[,] gameFields = game.GameFields;
            Panel panel = (Panel)sender;
            Point location = panel.Location;
            int x = location.X / gameboardPanelSize;
            int y = location.Y / gameboardPanelSize;           
            
            // only in game and if player is playing
            if(game.GameState == GameStateEnum.IN_GAME && game.PlayerPlaying == true)
            {
                // select field
                if (!game.IsSelected)
                {
                    game.Select(x, y);
                }
                else
                {
                    // send move to server
                    socketManager.Send(SendMsgUtils.Move(game.SelectedY, game.SelectedX, y, x));
                    game.Select(-1, -1);
                }

                PrintGame();
            }
        }

        // handles game state send by server
        public void HandleGame(string[] msgParts)
        {            
            if((game.GameState == GameStateEnum.IN_GAME || game.GameState == GameStateEnum.QUEUED) && msgParts.Length == 5)
            {
                // update game state
                if(game.GameState == GameStateEnum.QUEUED)
                {
                    game.GameState = GameStateEnum.IN_GAME;
                }

                int[] gameboardEncoded = GetEncodedGameboard(msgParts[4]);

                if(gameboardEncoded == null)
                {
                    Console.WriteLine("Game handle - invalid encoded game board");
                    socketManager.CloseSocket();
                }

                game.OpponentName = msgParts[1];

                // set playing player
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

                // set winner
                string winner = msgParts[3];
                if(winner == Constants.msgNull)
                {
                    game.WinnerName = Constants.fieldEmpty;
                }
                else if (winner == Constants.msgNull + Constants.msgNull)
                {
                    game.WinnerName = Constants.fieldEmpty;
                    game.GameState = GameStateEnum.FINISHED;
                    playAgainBtn.Visible = true;
                    MessageBox.Show(Constants.drawPopupMsg);
                }
                else
                {
                    if(winner != game.PlayerName && winner != game.OpponentName)
                    {
                        Console.WriteLine("Handle game - winner is none of players");
                        socketManager.CloseSocket();
                    }
                    else
                    {
                        game.WinnerName = winner;
                        game.GameState = GameStateEnum.FINISHED;
                        playAgainBtn.Visible = true;
                        MessageBox.Show(Constants.winnerPopupMsg + winner);
                    }
                }

                // update game and print it
                game.UpdateGameboard(gameboardEncoded);
                game.Select(-1, -1);
                PrintGame();
            }
            else
            {
                Console.WriteLine("Handle game - bad game state or invalid message parts count");
                socketManager.CloseSocket();
            }
        }

        // convert game board encoded string ot int array
        private int[] GetEncodedGameboard(string gameboardString)
        {
            // validate
            if(gameboardString.Length != Constants.gameboardLength * Constants.gameboardLength)
            {
                return null;
            }

            int[] gameboardEncoded = new int[Constants.gameboardLength * Constants.gameboardLength];

            // foreach string char
            for(int i = 0; i < gameboardString.Length; i++)
            {
                // validate permitted chars
                if(gameboardString[i] != '1' && gameboardString[i] != '2' && gameboardString[i] != '3' && gameboardString[i] != '4' && gameboardString[i] != '5')
                {
                    return null;
                }

                gameboardEncoded[i] = Int32.Parse(Char.ToString(gameboardString[i]));
            }


            return gameboardEncoded;
        }

        // handle move failed message send by server
        public void HandleMoveFailed(string[] msgParts)
        {
            // validte game state and message
            if(game.GameState == GameStateEnum.IN_GAME && msgParts.Length == 2 && msgParts[1] == Constants.failed)
            {
                MessageBox.Show(Constants.moveFailedMsg);
            }
            else
            {
                Console.WriteLine("Move failed - invalid message parts count or bad game state or wrong keyword");
                socketManager.CloseSocket();
            }
        }

        // inform server that client leaves game
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // send leave if was not send before
            if(!leaveSend)
            {
                socketManager.Send(SendMsgUtils.Leave());
                leaveSend = true;
            }           
            Application.Exit();
        }

        // click to start new game
        private void playAgainBtn_Click(object sender, EventArgs e)
        {
            playAgainBtn.Visible = false;
            game.Reset();
            socketManager.Send(SendMsgUtils.PlayAgain());
        }

        // handles play again response
        public void HandlePlayAgain(string[] msgParts)
        {
            // check of valid game state and mesage validity
            if (game.GameState != GameStateEnum.INIT || msgParts.Length != 3 || msgParts[1] != Constants.connectOk || 
                (msgParts[2] != Constants.white && msgParts[2] != Constants.black))
            {
                Console.WriteLine("PlayAgain handling - bad game state, message parts count or keywords");
                socketManager.CloseSocket();
            }
                        
            // update game state
            game.GameState = GameStateEnum.QUEUED;           
            if (msgParts[2] == Constants.white)
            {
                game.IsPlayerWhite = true;
            }
            else
            {
                game.IsPlayerWhite = false;
            }

            PrintGame();
        }

        // handles opponent online message
        public void HandleOpponentOnline(string[] msgParts)
        {
            // check of valid game state and mesage validity
            if (game.GameState != GameStateEnum.IN_GAME || msgParts.Length != 1)
            {
                Console.WriteLine("Opponent online handling - bad game state or message parts count");
                socketManager.CloseSocket();
            }

            // set online and print game
            game.OpponentOnline = true;
            PrintGame();
        }

        // handles opponent offline message
        public void HandleOpponentOffline(string[] msgParts)
        {
            // check of valid game state and mesage validity
            if (game.GameState != GameStateEnum.IN_GAME || msgParts.Length != 1)
            {
                Console.WriteLine("Opponent offline handling - bad game state or message parts count");
                socketManager.CloseSocket();
            }

            // set offline and print game
            game.OpponentOnline = false;
            PrintGame();
        }
    }
}
