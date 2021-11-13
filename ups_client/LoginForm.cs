using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ups_client
{
    /*
     * Class represents game login form
     */ 
    public partial class LoginForm : Form
    {
        // socket manager
        private SocketManager socketManager;
        // gmae board form
        private Form1 form;
        // game state
        private Game game;
        // player's nick
        private string nick;

        // constructor
        public LoginForm(SocketManager socketManager, Form1 form, Game game)
        {
            InitializeComponent();
            this.socketManager = socketManager;
            this.form = form;
            this.game = game;
            onlineLabel.ForeColor = Color.Green;
        }

        // connect btn click event
        private void connectButton_Click(object sender, EventArgs e)
        {
            string nick = nameTextBox.Text;

            if(!ValidationUtils.ValidateLogin(nick))
            {
                // show message when name is not valid
                MessageBox.Show(Constants.invalidNameMsg);                
                nameTextBox.Text = "";
            }
            else
            {
                // send mesage to server when name is valid
                connectButton.Enabled = false;
                this.nick = nick;
                socketManager.Send(SendMsgUtils.Login(nick));
            }
        }

        // handles connect message from server
        internal void HandleConnect(string[] msgParts)
        {
            // check of valid game state and mesage validity
            if(game.GameState != GameStateEnum.INIT || msgParts.Length < 2)
            {
                Console.WriteLine("Connect handling - bad game state or message parts count");
                socketManager.CloseSocket();
            }

            switch(msgParts[1])
            {
                case Constants.connectOk:
                    // for ok message
                    // check validity
                    if(msgParts.Length !=3 || (msgParts[2] != Constants.white && msgParts[2] != Constants.black))
                    {
                        Console.WriteLine("Connect Ok - bad message parts count or invalid keywords");
                        socketManager.CloseSocket();
                    }
                    // set name and update game state
                    game.GameState = GameStateEnum.QUEUED;
                    game.PlayerName = nick;
                    if(msgParts[2] == Constants.white)
                    {
                        game.IsPlayerWhite = true;
                    }
                    else
                    {
                        game.IsPlayerWhite = false;
                    }
                    // start game board form
                    Invoke(new Action(() => { Hide(); }));
                    Invoke(new Action(() => { form.Show(); Program.LoginFormOpened = false; }));
                    break;
                case Constants.connectInvalid:
                    // for failed connection
                    if (msgParts.Length != 2)
                    {
                        Console.WriteLine("Connect invalid - bad message parts count");
                        socketManager.CloseSocket();
                    }
                    // inform user that name is invalid
                    MessageBox.Show(Constants.takenNameMsg);
                    Invoke(new Action(() => 
                    {
                        nameTextBox.Text = "";
                        connectButton.Enabled = true;
                    }));                    
                    break;
                default:
                    Console.WriteLine("Handle connect - no matching keyword");
                    socketManager.CloseSocket();
                    break;
            }           
        }

        // changes the color of online label based on server accessibility
        internal void UpdateServerOnline()
        {
            Invoke(new Action(() => 
            { 
                // if server is online
                if(game.ServerOnline)
                {
                    onlineLabel.ForeColor = Color.Green;
                }
                else
                {
                    // if server is not online
                    onlineLabel.ForeColor = Color.Red;
                }               
            }));
        }
    }
}
