using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ups_client
{
    public class SocketManager
    {
        private int port;
        private string ipAdress;
        private Game game;
        private Socket socket;

        public Form1 Form { get; set; }
        public LoginForm LoginForm { get; set; }

        public SocketManager(string ipAdress, int port, Game game)
        {
            this.ipAdress = ipAdress;
            this.port = port;
            this.game = game;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(IPAddress.Parse(ipAdress), port);
                Console.WriteLine("Connect - OK");
            }
            catch
            {
                Console.WriteLine("Connect - error");
                Application.Exit();
            }
        }

        public void Send(string msg)
        {
            byte[] byteMsg = Encoding.UTF8.GetBytes(msg);
            int charsSend = socket.Send(byteMsg, 0, byteMsg.Length, SocketFlags.None);
            Console.WriteLine("Send - {0} chars", charsSend);
        }

        public void Listen()
        {            
            while(socket.Connected)
            {
                if(socket.Available == Constants.msgLength)
                {
                    byte[] byteMsg = new byte[Constants.msgLength];
                    socket.Receive(byteMsg, byteMsg.Length, SocketFlags.None);
                    string msg = Encoding.UTF8.GetString(byteMsg, 0, byteMsg.Length);
                    Console.WriteLine("Received - " + msg);
                    HandleMessage(msg);
                }              
            }
        }

        public void CloseSocket()
        {
            socket.Close();
            Application.Exit();
        }

        private void HandleMessage(string msg)
        {
            if(!ValidationUtils.BasicMsgCheck(msg))
            {
                CloseSocket();
            }
            msg = RemoveStartStopChars(msg);

            string[] msgParts = msg.Split(Constants.msgSeparator[0]);

            switch(msgParts[0])
            {
                case Constants.connect:
                    if(msgParts.Length != 2)
                    {
                        CloseSocket();
                    }
                    LoginForm.HandleConnect(msgParts);
                    break;
                default:
                    CloseSocket();
                    break;
            }
        }

        private string RemoveStartStopChars(string msg)
        {
            int firstEndCharIdx = 1;

            for(int i = msg.Length - 1; i > 0; i--)
            {
                if(msg[i] != Constants.msgFill[0])
                {
                    firstEndCharIdx = i;
                    break;
                }
            }

            return msg.Substring(1, firstEndCharIdx);
        }
    }
}
