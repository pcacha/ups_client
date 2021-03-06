using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace ups_client
{
    /*
     * Class representing 
     */
    public class SocketManager
    {
        // port
        private int port;
        // ip adress
        private string ipAdress;
        // game state
        private Game game;
        // socket
        private Socket socket;

        // game board form
        public Form1 Form { get; set; }
        // login form
        public LoginForm LoginForm { get; set; }
        // determines if bind succeeded
        public bool BindOk { get; set; }

        // statistical data
        private int bytesCount = 0;
        private int messagesCount = 0;
        private DateTime programStartTime = DateTime.Now;

        // constructor
        public SocketManager(string ipAdress, int port, Game game)
        {
            this.ipAdress = ipAdress;
            this.port = port;
            this.game = game;
            BindOk = true;
            
            try
            {
                // try connect
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // check if first argument is hostname or ip address
                MatchCollection matches = Regex.Matches(ipAdress, Constants.ipAddressRegex);

                if (matches.Count == 0)
                {
                    // first parameter is not ip address but hostname
                    // get ip addresses from hostname
                    IPAddress[] addressList;
                    try
                    {
                        addressList = Dns.GetHostAddresses(ipAdress);
                    }
                    catch
                    {
                        // no ip address available
                        Console.WriteLine("Connect - error - bad hostname");
                        BindOk = false;
                        return;
                    }
                    

                    if(addressList.Length == 0)
                    {
                        // no ip address available
                        Console.WriteLine("Connect - error - bad hostname");
                        BindOk = false;
                        return;
                    }

                    // get frist ipv4 address
                    IPAddress ipFromDns = null;
                    foreach(IPAddress address in addressList)
                    {
                        if(address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipFromDns = address;
                            break;
                        }
                    }

                    socket.Connect(ipFromDns, port);
                }
                else
                {
                    // first parameter is ip address
                    socket.Connect(IPAddress.Parse(ipAdress), port);
                }
                
                Console.WriteLine("Connect - OK");
            }
            catch
            {
                // if connection failed
                Console.WriteLine("Connect - error");
                BindOk = false;
                return;
            }

            // start timer for evaluating server accessiblity
            Timer serverAccessiblityTimer = new Timer();
            serverAccessiblityTimer.Elapsed += new ElapsedEventHandler(CheckServerAccessibility);
            serverAccessiblityTimer.Interval = Constants.serverAccessibilityTimerRepeatTime;
            serverAccessiblityTimer.Enabled = true;
        }

        // sends message to server
        public void Send(string msg)
        {           
            if(game.ServerOnline)
            {
                try
                {
                    // server is accessible
                    byte[] byteMsg = Encoding.UTF8.GetBytes(msg);
                    socket.Send(byteMsg, 0, byteMsg.Length, SocketFlags.None);
                    if (!msg.Contains(Constants.pong))
                    {
                        Console.WriteLine("Send - msg: " + msg);
                    }
                }
                catch
                {
                    Console.WriteLine("Cannot send - msg: " + msg);
                }               
            }
            else
            {
                // server not accessible
                Console.WriteLine("Can not send - server not accessible - msg: " + msg);
            }
            
        }

        // listening loop
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
               
                // only if something is available
                if (socket.Available > 0)
                {
                    bytesCount += socket.Available;

                    if (socket.Available > Constants.maxMsgBatchLength)
                    {
                        Console.WriteLine("Socket - max batch length exceeded");
                        CloseSocket();
                    }

                    // read data from file descriptor
                    byte[] dataChars = new byte[socket.Available];
                    socket.Receive(dataChars, dataChars.Length, SocketFlags.None);
                    string dataString = Encoding.UTF8.GetString(dataChars, 0, dataChars.Length);
                    if (!dataString.Contains(Constants.ping))
                    {
                        // TODO
                        // Console.WriteLine("Socket - data on socket: " + dataString);
                    }
                        

                    // split data based on message regex
                    MatchCollection matches = Regex.Matches(dataString, Constants.msgRegex);

                    if(matches.Count == 0)
                    {
                        Console.WriteLine("Regex - no message available");
                        CloseSocket();
                    }                                        

                    foreach (Match match in matches)
                    {
                        string msg = match.Value;

                        // if message has the righr length, process it
                        if (msg.Length >= Constants.minMsgLength && msg.Length <= Constants.maxMsgLength)
                        {
                            if (!msg.Contains(Constants.ping))
                            {
                                Console.WriteLine("Received - msg: " + msg);
                            }
                            messagesCount++;
                            HandleMessage(msg);
                        }
                        else
                        {
                            Console.WriteLine("Message - invalid message length");
                            CloseSocket();
                        }
                    }
                }
            }
        }

        // prints statistics to file
        public void PrintStatisticalData()
        {
            // measure uptime
            DateTime current = DateTime.Now;
            TimeSpan uptime = current - programStartTime;
            double uptimeSeconds = uptime.TotalSeconds;

            // print data
            using (StreamWriter writer = new StreamWriter(new FileStream(Constants.statDataFileName, FileMode.Create)))
            {
                writer.WriteLine("Transported bytes count: " + bytesCount);
                writer.WriteLine("Transported messages count: " + messagesCount);
                writer.WriteLine("Uptime in seconds: " + uptimeSeconds);
            }            
        }

        // closes socket and application
        public void CloseSocket()
        {
            Console.WriteLine("Socket - error");
            Console.WriteLine("Application - close");
            socket.Close();                       

            PrintStatisticalData();
            Environment.Exit(0);
        }

        // handles incoming message
        private void HandleMessage(string msg)
        {      
            // remove start stop chars
            msg = msg.Substring(1, msg.Length - 2);

            string[] msgParts = msg.Split(Constants.msgSeparator[0]);

            if(msgParts.Length < 1)
            {
                Console.WriteLine("Message - no message parts");
                CloseSocket();
            }

            // call method based on message type
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
                case Constants.playAgain:
                    Form.HandlePlayAgain(msgParts);
                    break;
                case Constants.ping:
                    HandlePing(msgParts);
                    break;
                case Constants.opponentOffline:
                    Form.HandleOpponentOffline(msgParts);
                    break;
                case Constants.opponentOnline:
                    Form.HandleOpponentOnline(msgParts);
                    break;
                default:
                    Console.WriteLine("Handle message - invalid keyword");
                    CloseSocket();
                    break;
            }
        }

        // handles ping message from server
        private void HandlePing(string[] msgParts)
        {
            // check of valid count of message parts
            if (msgParts.Length != 1)
            {
                Console.WriteLine("Ping handling - bad message parts count");
                CloseSocket();
            }

            game.LastPingTimestamp = DateTime.Now;
            if (!game.ServerOnline)
            {
                // update server accessibility in gui               
                game.ServerOnline = true;
                UpdateGuiOnAccessibilityChange();
            }  

            // send pong
            Send(SendMsgUtils.Pong());
        }

        // updates gui when server accessiblity has changed
        private void UpdateGuiOnAccessibilityChange()
        {
            Console.WriteLine("Server accessiblity - value changed");                         

            if (Program.LoginFormOpened)
            {
                // for login form
                LoginForm.UpdateServerOnline();
            }
            else
            {
                // for game form
                Form.PrintGame();
            }
        }

        // check if server pings the client
        private void CheckServerAccessibility(object source, ElapsedEventArgs e)
        {
            // measure distinction from last ping
            DateTime current = DateTime.Now;
            TimeSpan distinction = current - game.LastPingTimestamp;

            if (game.ServerOnline)
            {             
                // if distinction exceeded max allowed value
                if(distinction.TotalMilliseconds > Constants.maxPingDelay)
                {
                    game.ServerOnline = false;                   
                    UpdateGuiOnAccessibilityChange();
                }               
            }

            if (distinction.TotalMilliseconds > Constants.maxOfflineTime)
            {
                Console.WriteLine("Offline - offline too long");
                Console.WriteLine("Application - close");
                socket.Close();                

                PrintStatisticalData();
                Application.Exit();
            }
        }        
    }
}
