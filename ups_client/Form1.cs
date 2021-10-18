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

        public Form1()
        {
            InitializeComponent();
            game = new Game();
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

            gameFields[0, 7].HasStone = true;
            gameFields[0, 7].IsKing = true;
            gameFields[0, 7].IsWhite = true;
            game.Select(0, 7);

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
        }

        private void printGame()
        {
            GameField[,] gameFields = game.GameFields;

            for (int i = 0; i < gameFields.GetLength(0); i++)
            {
                for (int j = 0; j < gameFields.GetLength(1); j++)
                {
                    GameField gf = gameFields[i, j];
                    Panel p = new Panel();
                    p.Height = gameboardPanelSize;
                    p.Width = gameboardPanelSize;
                    p.Top = i * gameboardPanelSize;
                    p.Left = j * gameboardPanelSize;

                    if(gf.IsSelected)
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

                    if(gf.HasStone)
                    {                        
                        if(gf.IsKing)
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

                    gamePanel.Controls.Add(p);
                }
            }
        }
    }
}
