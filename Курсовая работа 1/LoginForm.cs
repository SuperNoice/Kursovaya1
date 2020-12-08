using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсовая_работа_1
{
    public partial class LoginForm : Form
    {
        private Session Server_Session;
        private bool IsLogin = false;

        public LoginForm(Session session)
        {
            Server_Session = session;
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            //Message message = new Message($"login {loginTextBox.Text.Trim()};{PasswordTextBox.Text.Trim()}");
            //Message answer = Server_Session.Send_Recieve(message);

            //Log.Print($"Отправлено:{message.Text}\tПринято:{answer.Text}");

            //switch (answer.Text.ToLower())
            //{
            //    case "none":
            //        UserInfo.UserRole = Role.None;
            //        loginErrorLabel.Visible = true;
            //        break;
            //    case "lecturer":
            //        UserInfo.UserRole = Role.Lecturer;
            //        IsLogin = true;
            //        break;
            //    case "teacher":
            //        UserInfo.UserRole = Role.Teacher;
            //        IsLogin = true;
            //        break;
            //    case "admin":
            //        UserInfo.UserRole = Role.Admin;
            //        IsLogin = true;
            //        break;
            //    case "student":
            //        UserInfo.UserRole = Role.Student;
            //        IsLogin = true;
            //        break;
            //    default:
            //        throw new Exception($"Неизвестный ответ: \"{answer.Text.ToLower()}\"");
            //}
            //loginErrorLabel.Visible = false;
            
            IsLogin = true;
            UserInfo.UserRole = Role.Student;
            
            if (IsLogin)
                this.Close();
        }
    }
}
