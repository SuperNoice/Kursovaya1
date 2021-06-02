using System;
using System.Windows.Forms;

namespace Курсовая_работа_1
{
    public partial class LoginForm : Form
    {
        private bool IsLogin = false;

        public LoginForm()
        {
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
            UserInfo.UserRole = Role.Admin;

            if (IsLogin)
                this.Close();
        }
    }
}
