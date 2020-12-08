using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;

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
                logWindow log = new logWindow();
                log.Show();
            }

            Session session = null;
            //Session session = new Session(IPAddress.Parse(""), 18888);
            //session.Open();

            LoginForm login = new LoginForm(session);
            login.ShowDialog();

            if (UserInfo.UserRole != Role.None)
                Application.Run(new MainForm(session));

        }
    }
}
