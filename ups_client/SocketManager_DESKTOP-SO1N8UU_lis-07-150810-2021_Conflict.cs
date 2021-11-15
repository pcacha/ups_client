using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
            socket.Send(byteMsg, 0, byteMsg.Length, SocketFlags.None);
            Console.WriteLine("Send - msg: " + msg);
        }

        public void Listen()
        {            
            while(socket.Connected)
            {
                /*
                if(socket.Available > 0)
                {
                    byte[] dataChars = new byte[socket.Available];
                    socket.Receive(dataChars, dataChars.Length, SocketFlags.None);
                    string dataString = Encoding.UTF8.GetString(dataChars, 0, dataChars.Length);

                    while(dataString.Length > 0)
                    {
                        int endIdx = dataString.IndexOf(Constants.msgEnd[0]);

                        if(endIdx == -1)
                        {
                            CloseSocket();                            
                        }

                        string msg = dataString.Substring(0, endIdx + 1);
                        if(endIdx + 1 == dataString.Length)
                        {
                            dataString = "";
                        }
                        else
                        {
                            dataString = dataString.Substring(endIdx + 1);
                        }
                        

                        int lastStartIdx = msg.LastIndexOf(Constants.msgStart[0]);

                        if(lastStartIdx != 0)
                        {
                            CloseSocket();
                        }

                        if(msg.Length <= Constants.maxMsgLength && msg.Length >= Constants.minMsgLength)
                        {
                            Console.WriteLine("Received - msg: " + msg);
                            HandleMessage(msg);
                        }
                        else
                        {
                            CloseSocket();
                        }
                    }
                }
                */

                Thread.Sleep(250);

                if (socket.Available > 0)
                {
                    if(socket.Available > Constants.maxMsgBatchLength)
                    {
                        CloseSocket();
                    }

                    byte[] dataChars = new byte[socket.Available];
                    socket.Receive(dataChars, dataChars.Length, SocketFlags.None);
                    string dataString = Encoding.UTF8.GetString(dataChars, 0, dataChars.Length);
                    Console.WriteLine("Socket - data on socket: " + dataString);

                    MatchCollection matches = Regex.Matches(dataString, Constants.msgRegex);

                    if(matches.Count == 0)
                    {
                        CloseSocket();
                    }                                        

                    foreach (Match match in matches)
                    {
                        string msg = match.Value;

                        if (msg.Length >= Constants.minMsgLength && msg.Length <= Constants.maxMsgLength)
                        {
                            Console.WriteLine("Received - msg: " + msg);
                            HandleMessage(msg);
                        }
                        else
                        {
                            CloseSocket();
                        }
                    }
                }
            }
        }

        public void CloseSocket()
        {
            Console.WriteLine("Socket - error");
            Console.WriteLine("Application - close");
            socket.Close();
            //Application.Exit();
        }

        private void HandleMessage(string msg)
        {            
            msg = msg.Substring(1, msg.Length - 2);

            string[] msgParts = msg.Split(Constants.msgSeparator[0]);

            if(msgParts.Length < 1)
            {
                CloseSocket();
            }

            switch(msgParts[0])
            {
                case Constants.connect:                    
                    LoginForm.HandleConnect(msgParts);
                    break;
                case Constants.game:
                    Form.HandleGame(msgParts);
                    break;
                case Constants.move:
                    Form.HandleMoveFailed(msgParts);
                    break;
                default:
                    CloseSocket();
                    break;
            }
        }       
    }
}
