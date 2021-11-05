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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // create main instances
            Game game = new Game();
            SocketManager socketManager = new SocketManager("127.0.0.1", 9999, game);
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
