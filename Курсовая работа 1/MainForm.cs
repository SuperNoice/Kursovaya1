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
    public partial class MainForm : Form
    {
        private Session Server_Session;
        private List<Button> menuButtonsList;

        public MainForm(Session session)
        {
            Server_Session = session;
            menuButtonsList = new List<Button>();
            InitializeComponent();
            InitializeMenu();
        }

        private void InitializeMenu()
        {
            string[] texsts = null;
            EventHandler[] events = null;
            switch (UserInfo.UserRole)
            {
                case Role.None:
                    Close();
                    break;
                case Role.Admin:
                    texsts = new string[] { "Администраторы", "Направления", "Группы", "Преподаватели", "Предметы", "Студенты" };
                    events = new EventHandler[] { admin_Admins, admin_Direstions, admin_Groups, admin_Teachers, admin_Subjects, admin_Stusents };
                    break;
                case Role.Lecturer:
                    texsts = new string[] { "Задания" };
                    events = new EventHandler[] { lector_Tasks };
                    break;
                case Role.Teacher:
                    texsts = new string[] { "Студенты", "Задания" };
                    events = new EventHandler[] { teachers_Stusents, teachers_Tasks };
                    break;
                case Role.Student:
                    texsts = new string[] { "Задания", "Преподаватели" };
                    events = new EventHandler[] { student_Tasks, student_Teachers };
                    break;
                default:
                    break;
            }

            for (int ptr = 0; ptr < texsts.Length; ++ptr)
            {
                menuButtonsList.Add(MenuButton.getMenuButton(texsts[ptr], MenuPanel));
                menuButtonsList[ptr].Click += events[ptr];
                menuButtonsList[ptr].Click += new EventHandler(ChangeButtonColor);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {

        }

        private void DelButton_Click(object sender, EventArgs e)
        {

        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
 
        }

        private void ChangeButtonColor(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.FromArgb(165, 208, 255);
            foreach (Button button in menuButtonsList)
            {
                if ((object)button != sender)
                    button.BackColor = SystemColors.ControlLight;
            }
        }

        // Функции кнопок меню
        private void admin_Admins(object sender, EventArgs e)
        {

        }

        private void admin_Direstions(object sender, EventArgs e)
        {

        }

        private void admin_Groups(object sender, EventArgs e)
        {

        }

        private void admin_Teachers(object sender, EventArgs e)
        {

        }

        private void admin_Subjects(object sender, EventArgs e)
        {

        }

        private void admin_Stusents(object sender, EventArgs e)
        {

        }

        private void lector_Tasks(object sender, EventArgs e)
        {

        }

        private void teachers_Stusents(object sender, EventArgs e)
        {

        }

        private void teachers_Tasks(object sender, EventArgs e)
        {

        }

        private void student_Tasks(object sender, EventArgs e)
        {

        }

        private void student_Teachers(object sender, EventArgs e)
        {

        }

    }
}
