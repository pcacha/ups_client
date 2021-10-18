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

        public Form1()
        {
            InitializeComponent();
            game = new Game();
            panels = new Panel[gameboardLength, gameboardLength];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            createGameboard();

            GameField[,] gameFields = game.GameFields;
            gameFields[0, 3].HasStone = true;
            
            gameFields[0, 4].HasStone = true;
            gameFields[0, 4].IsWhite = true;

            gameFields[0, 5].HasStone = true;
            gameFields[0, 5].IsKing = true;

            gameFields[0, 6].HasStone = true;
            gameFields[0, 6].IsKing = true;
            gameFields[0, 6].IsWhite = true;

            printGame();
        }

        private void createGameboard()
        {
            gamePanel = new Panel();
            gamePanel.Height = gameboardPanelSize * gameboardLength;
            gamePanel.Width = gameboardPanelSize * gameboardLength;
            gamePanel.Top = topMargin;
            gamePanel.Left = leftMargin;
            gamePanel.BackColor = Color.Black;
            gamePanel.Cursor = Cursors.Hand;
            this.Controls.Add(gamePanel);
            createPanels();
        }

        private void createPanels()
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

        private void printGame()
        {
            printGameboard();
            printSideInfo();
        }

        private void printGameboard()
        {           
            GameField[,] gameFields = game.GameFields;            

            for (int i = 0; i < gameFields.GetLength(0); i++)
            {
                for (int j = 0; j < gameFields.GetLength(1); j++)
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
        
        private void printSideInfo()
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
            printGame();
        }

        private void gameboardPanel_Click(object sender, EventArgs e)
        {
            GameField[,] gameFields = game.GameFields;
            Panel panel = (Panel)sender;
            Point location = panel.Location;
            int x = location.X / gameboardPanelSize;
            int y = location.Y / gameboardPanelSize;           
            
            if(!game.IsSelected)
            {                
                game.Select(x, y);              
            }
            else
            {
                GameField startField = gameFields[game.SelectedY, game.SelectedX];
                GameField toField = gameFields[y, x];

                toField.CreateFrom(startField);
                startField.RemoveStone();               

                game.Select(-1, -1);
            }

            printGame();
        }
    }
}
