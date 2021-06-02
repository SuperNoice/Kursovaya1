using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Курсовая_работа_1
{
    static class Program
    {
        static bool DebugMode = true;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (DebugMode)
            {
                LogForm log = new LogForm();
                log.Show();
            }

            LoginForm login = new LoginForm();
            login.ShowDialog();

            if (UserInfo.UserRole != Role.None)
                Application.Run(new MainForm());

        }
    }
}
