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
                socketManager.Send(SendMsgUtils.Login(nick));
                connectButton.Enabled = false;
            }
        }

        internal void HandleConnect(string[] msgParts)
        {
            if(game.GameState != GameStateEnum.INIT)
            {
                socketManager.CloseSocket();
            }

            if(msgParts[1] == Constants.connectOk)
            {
                game.GameState = GameStateEnum.IN_GAME;
                Close();
                form.Show();
            }
            if (msgParts[1] == Constants.connectInvalid)
            {
                MessageBox.Show(Constants.invalidNameMsg);
                nameTextBox.Text = "";
                connectButton.Enabled = true;
            }
            else
            {
                socketManager.CloseSocket();
            }
        }
    }
}
