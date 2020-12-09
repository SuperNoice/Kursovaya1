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
        private Button lastPressedButton;
        private string menuName;
        private DataTable cashedTable;
        private DataTable addedRowsTable;
        private DataTable changedRowsTable;
        private DataTable deletedRowsTable;
        private List<Message> messagesStek = new List<Message>();

        public MainForm(Session session)
        {
            cashedTable = new DataTable();
            addedRowsTable = new DataTable();
            changedRowsTable = new DataTable();
            deletedRowsTable = new DataTable();
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
                    texsts = new string[] { "Администраторы", "Направления", "Группы", "Преподаватели", "Предметы", "Студенты", "Преподаватель-Предмет", "Преподаватель-Группа" };
                    events = new EventHandler[] { admin_Admins, admin_Direstions, admin_Groups, admin_Teachers, admin_Subjects, admin_Stusents, admin_Linkteachers, admin_Linkgroups };
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
            dataGridViewMain.Rows.Add();
        }

        private void DelButton_Click(object sender, EventArgs e)
        {

        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            Message[] answers = Server_Session.Send_Recieve(messagesStek.ToArray());
            
        }

        private void ChangeButtonColor(object sender, EventArgs e)
        {
            if (lastPressedButton != null)
                lastPressedButton.BackColor = SystemColors.ControlLight;
            ((Button)sender).BackColor = Color.FromArgb(165, 208, 255);
            lastPressedButton = (Button)sender;
        }

        // Функции кнопок меню
        private void admin_Admins(object sender, EventArgs e)
        {
            menuName = "admins";
        }

        private void admin_Direstions(object sender, EventArgs e)
        {
            menuName = "directions";
        }

        private void admin_Groups(object sender, EventArgs e)
        {
            menuName = "groups";
        }

        private void admin_Teachers(object sender, EventArgs e)
        {
            menuName = "teachers";
        }

        private void admin_Subjects(object sender, EventArgs e)
        {
            menuName = "subjects";
        }

        private void admin_Stusents(object sender, EventArgs e)
        {
            menuName = "students";
        }

        private void admin_Linkteachers(object sender, EventArgs e)
        {
            menuName = "linkteachers";
        }

        private void admin_Linkgroups(object sender, EventArgs e)
        {
            menuName = "linkgroups";
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

        // DataGridView события изменения строк
        private void dataGridViewMain_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {         

        }

        private void dataGridViewMain_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        private void dataGridViewMain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
