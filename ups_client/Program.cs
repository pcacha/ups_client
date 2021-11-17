using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ups_client
{
    static class Program
    {
        // determines which form is oppened
        public static bool LoginFormOpened { get; set; } = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // program args
            string[] args = Environment.GetCommandLineArgs();           

            // test arguments validity
            int test;
            if (args.Length != 3 || !Int32.TryParse(args[2], out test))
            {
                Console.WriteLine("Start - wrong arguments count or port is not a number");
                return;
            }

            int port = Int32.Parse(args[2]);
            string ipAddress = args[1];

            if (port < 1024 || port > 65535)
            {
                Console.WriteLine("Start - port has bad range");
                return;
            }

            if(ipAddress == "localhost" || ipAddress == "Localhost" || ipAddress == "LOCALHOST") 
            {
                Console.WriteLine("Start - localhost transformed to 127.0.0.1");
                ipAddress = "127.0.0.1";
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // create main instances
            Game game = new Game();
            SocketManager socketManager = new SocketManager(ipAddress, port, game);

            if(!socketManager.BindOk)
            {
                return;
            }

            Form1 form = new Form1(socketManager, game);
            LoginForm loginForm = new LoginForm(socketManager, form, game);

            socketManager.Form = form;
            socketManager.LoginForm = loginForm;

            // start therad that listens to server
            Thread listenThread = new Thread(new ThreadStart(socketManager.Listen));
            listenThread.IsBackground = true;
            listenThread.Start();

            Application.Run(loginForm);
        }
    }
}
