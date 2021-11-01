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
    public partial class LoginForm : Form
    {
        private SocketManager socketManager;
        private Form1 form;
        private Game game;
        private string nick;

        public LoginForm(SocketManager socketManager, Form1 form, Game game)
        {
            InitializeComponent();
            this.socketManager = socketManager;
            this.form = form;
            this.game = game;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            string nick = nameTextBox.Text;

            if(!ValidationUtils.ValidateLogin(nick))
            {
                MessageBox.Show(Constants.invalidNameMsg);                
                nameTextBox.Text = "";
            }
            else
            {
                connectButton.Enabled = false;
                this.nick = nick;
                socketManager.Send(SendMsgUtils.Login(nick));
            }
        }

        internal void HandleConnect(string[] msgParts)
        {
            if(game.GameState != GameStateEnum.INIT || msgParts.Length < 2)
            {
                socketManager.CloseSocket();
            }

            switch(msgParts[1])
            {
                case Constants.connectOk:
                    if(msgParts.Length !=3 || (msgParts[2] != Constants.white && msgParts[2] != Constants.black))
                    {
                        socketManager.CloseSocket();
                    }
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
                    Invoke(new Action(() => { Hide(); }));
                    Application.Run(form);
                    break;
                case Constants.connectInvalid:
                    if (msgParts.Length != 2)
                    {
                        socketManager.CloseSocket();
                    }
                    MessageBox.Show(Constants.takenNameMsg);
                    Invoke(new Action(() => 
                    {
                        nameTextBox.Text = "";
                        connectButton.Enabled = true;
                    }));                    
                    break;
                default:
                    socketManager.CloseSocket();
                    break;
            }           
        }
    }
}
