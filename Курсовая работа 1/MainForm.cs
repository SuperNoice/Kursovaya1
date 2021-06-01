using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLib;

namespace Курсовая_работа_1
{
    public partial class MainForm : Form
    {
        private Session Server_Session;

        private List<Button> menuButtonsList;
        private Button lastPressedButton;

        private string menuName;
        private Dictionary<string, MenuData> menuData;
        private DataTable CurrentMenuTable => menuData[menuName].ActualTable;

        private List<ClassLib.Message[]> _messagesStek;

        private bool Filling;


        public MainForm(Session session)
        {
            menuData = new Dictionary<string, MenuData>();
            menuData.Add("users", new MenuData("users"));
            menuData.Add("admins", new MenuData("admins"));
            menuData.Add("directions", new MenuData("directions"));
            menuData.Add("groups", new MenuData("groups"));
            menuData.Add("teachers", new MenuData("teachers"));
            menuData.Add("subjects", new MenuData("subjects"));
            menuData.Add("students", new MenuData("students"));
            menuData.Add("semsubject", new MenuData("semsubject"));

            Filling = true;
            _messagesStek = new List<ClassLib.Message[]>();
            Server_Session = session;
            menuButtonsList = new List<Button>();
            InitializeComponent();
            InitializeMenu();
            Filling = false;
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
                    texsts = new string[] { "Пользователи", "Администраторы", "Направления", "Группы", "Преподаватели", "Предметы", "Студенты", "Семестры" };
                    events = new EventHandler[] { admin_Users, admin_Admins, admin_Direstions, admin_Groups, admin_Teachers, admin_Subjects, admin_Stusents, admin_Semsubject };
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
            Filling = true;
            DataRow _row = menuData[menuName].ActualTable.Rows.Add();
            menuData[menuName].addChangedRow("add", _row);

            SaveChangesButton.Enabled = !menuData[menuName].Saved;
            Filling = false;
        }

        private void DelButton_Click(object sender, EventArgs e)
        {
            Filling = true;
            foreach (DataGridViewRow row in dataGridViewMain.SelectedRows)
            {
                menuData[menuName].addChangedRow("del", menuData[menuName].ActualTable.Rows[row.Index]);
                menuData[menuName].ActualTable.Rows.RemoveAt(row.Index);
            }

            SaveChangesButton.Enabled = !menuData[menuName].Saved;
            Filling = false;
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            _messagesStek.Add(menuData[menuName].SaveChanges());
            ClassLib.Message[] answers = Server_Session.Send_Recieve(_messagesStek.Last<ClassLib.Message[]>());
            _messagesStek.Remove(_messagesStek.Last<ClassLib.Message[]>());

            SaveChangesButton.Enabled = !menuData[menuName].Saved;
            updButton.Enabled = true;
        }

        private void updButton_Click(object sender, EventArgs e)
        {
            menuData[menuName].IsLoaded = false;
            lastPressedButton.PerformClick();
            updButton.Enabled = false;
        }

        private void ChangeButtonColor(object sender, EventArgs e)
        {
            if (lastPressedButton != null)
                lastPressedButton.BackColor = SystemColors.ControlLight;
            ((Button)sender).BackColor = Color.FromArgb(165, 208, 255);
            lastPressedButton = (Button)sender;
        }

        // Функции кнопок меню
        private void admin_Users(object sender, EventArgs e)
        {
            menuName = "users";
            Filling = true;
            dataGridViewMain.DataSource = null;
            dataGridViewMain.Columns.Clear();
            if (!menuData[menuName].IsLoaded)
            {
                DataTable _table = Server_Session.Send_Recieve(new ClassLib.Message("users get")).Table;
                DataTable _tableRoles = Server_Session.Send_Recieve(new ClassLib.Message("users get role")).Table;
                menuData[menuName].LoadTable(_table, new DataTable[] { _tableRoles });
            }
            dataGridViewMain.DataSource = CurrentMenuTable;
            dataGridViewMain.Columns[0].Visible = false;
            string[] columnNames = new string[] { "role_id" };
            int ptr = 0;
            foreach (DataTable table in menuData[menuName].additionalColumns)
            {
                table.TableName = columnNames[ptr];
                table.Columns[1].ColumnName = "val";
                List<object> values = new List<object>();
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                foreach (DataRow row in table.Rows)
                    values.Add(row.ItemArray[1]);

                column.Name = columnNames[ptr] + "Comb";
                column.Items.AddRange(values.ToArray());

                dataGridViewMain.Columns.Insert(0, column);
                ptr++;
            }

            for (ptr = 0; ptr < columnNames.Count<string>(); ++ptr)
                foreach (DataGridViewRow row in dataGridViewMain.Rows)
                    row.Cells[columnNames[ptr] + "Comb"].Value = menuData[menuName].additionalColumns[ptr].Select($"id='{row.Cells[columnNames[ptr]].Value}'")[0].ItemArray[1];


            foreach (string item in columnNames)
                dataGridViewMain.Columns[item].Visible = false;
            SaveChangesButton.Enabled = !menuData[menuName].Saved;
            Filling = false;
        }

        private void admin_Admins(object sender, EventArgs e)
        {
            menuName = "admins";
            Filling = true;
            dataGridViewMain.DataSource = null;
            dataGridViewMain.Columns.Clear();
            if (!menuData[menuName].IsLoaded)
            {
                DataTable _table = Server_Session.Send_Recieve(new ClassLib.Message("admins get")).Table;
                DataTable _tableUsers = Server_Session.Send_Recieve(new ClassLib.Message("admins get user")).Table;

                menuData[menuName].LoadTable(_table, new DataTable[] { _tableUsers });
            }
            dataGridViewMain.DataSource = CurrentMenuTable;
            dataGridViewMain.Columns[0].Visible = false;
            string[] columnNames = new string[] { "fio" };
            int ptr = 0;
            foreach (DataTable table in menuData[menuName].additionalColumns)
            {
                table.TableName = columnNames[ptr];
                table.Columns[1].ColumnName = "val";
                List<object> values = new List<object>();
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                foreach (DataRow row in table.Rows)
                    values.Add(row.ItemArray[1]);

                column.Name = columnNames[ptr] + "Comb";
                column.Items.AddRange(values.ToArray());

                dataGridViewMain.Columns.Insert(0, column);
                ptr++;
            }
            for (ptr = 0; ptr < columnNames.Count<string>(); ++ptr)
                foreach (DataGridViewRow row in dataGridViewMain.Rows)
                    row.Cells[columnNames[ptr] + "Comb"].Value = row.Cells[columnNames[ptr]].Value;

            foreach (string item in columnNames)
                dataGridViewMain.Columns[item].Visible = false;
            SaveChangesButton.Enabled = !menuData[menuName].Saved;
            Filling = false;
        }

        private void admin_Direstions(object sender, EventArgs e)
        {
            menuName = "directions";
            Filling = true;
            dataGridViewMain.DataSource = null;
            dataGridViewMain.Columns.Clear();
            if (!menuData[menuName].IsLoaded)
            {
                DataTable _table = Server_Session.Send_Recieve(new ClassLib.Message("directions get")).Table;

                menuData[menuName].LoadTable(_table);
            }
            dataGridViewMain.DataSource = CurrentMenuTable;
            dataGridViewMain.Columns[0].Visible = false;

            SaveChangesButton.Enabled = !menuData[menuName].Saved;
            Filling = false;
        }

        private void admin_Groups(object sender, EventArgs e)
        {
            menuName = "groups";
            Filling = true;
            dataGridViewMain.DataSource = null;
            dataGridViewMain.Columns.Clear();
            if (!menuData[menuName].IsLoaded)
            {
                DataTable _table = Server_Session.Send_Recieve(new ClassLib.Message("groups get")).Table;
                DataTable _tabledDirections = Server_Session.Send_Recieve(new ClassLib.Message("groups get direction")).Table;

                menuData[menuName].LoadTable(_table, new DataTable[] { _tabledDirections });
            }
            dataGridViewMain.DataSource = CurrentMenuTable;
            dataGridViewMain.Columns[0].Visible = false;
            string[] columnNames = new string[] { "direction_id" };
            int ptr = 0;
            foreach (DataTable table in menuData[menuName].additionalColumns)
            {
                table.TableName = columnNames[ptr];
                table.Columns[1].ColumnName = "val";
                List<object> values = new List<object>();
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                foreach (DataRow row in table.Rows)
                    values.Add(row.ItemArray[1]);

                column.Name = columnNames[ptr] + "Comb";
                column.Items.AddRange(values.ToArray());

                dataGridViewMain.Columns.Insert(0, column);
                ptr++;
            }
            for (ptr = 0; ptr < columnNames.Count<string>(); ++ptr)
                foreach (DataGridViewRow row in dataGridViewMain.Rows)
                    row.Cells[columnNames[ptr] + "Comb"].Value = menuData[menuName].additionalColumns[ptr].Select($"id='{row.Cells[columnNames[ptr]].Value}'")[0].ItemArray[1];

            foreach (string item in columnNames)
                dataGridViewMain.Columns[item].Visible = false;
            SaveChangesButton.Enabled = !menuData[menuName].Saved;
            Filling = false;
        }

        private void admin_Teachers(object sender, EventArgs e)
        {
            menuName = "teachers";
            Filling = true;
            dataGridViewMain.DataSource = null;
            dataGridViewMain.Columns.Clear();
            if (!menuData[menuName].IsLoaded)
            {
                DataTable _table = Server_Session.Send_Recieve(new ClassLib.Message("teachers get")).Table;
                DataTable _tableUsers = Server_Session.Send_Recieve(new ClassLib.Message("teachers get user")).Table;

                menuData[menuName].LoadTable(_table, new DataTable[] { _tableUsers });
            }
            dataGridViewMain.DataSource = CurrentMenuTable;
            dataGridViewMain.Columns[0].Visible = false;
            string[] columnNames = new string[] { "user_id" };
            int ptr = 0;
            foreach (DataTable table in menuData[menuName].additionalColumns)
            {
                table.TableName = columnNames[ptr];
                table.Columns[1].ColumnName = "val";
                List<object> values = new List<object>();
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                foreach (DataRow row in table.Rows)
                    values.Add(row.ItemArray[1]);

                column.Name = columnNames[ptr] + "Comb";
                column.Items.AddRange(values.ToArray());

                dataGridViewMain.Columns.Insert(0, column);
                ptr++;
            }
            for (ptr = 0; ptr < columnNames.Count<string>(); ++ptr)
                foreach (DataGridViewRow row in dataGridViewMain.Rows)
                    row.Cells[columnNames[ptr] + "Comb"].Value = menuData[menuName].additionalColumns[ptr].Select($"id='{row.Cells[columnNames[ptr]].Value}'")[0].ItemArray[1];

            foreach (string item in columnNames)
                dataGridViewMain.Columns[item].Visible = false;
            SaveChangesButton.Enabled = !menuData[menuName].Saved;
            Filling = false;
        }

        private void admin_Subjects(object sender, EventArgs e)
        {
            menuName = "subjects";
            Filling = true;
            dataGridViewMain.DataSource = null;
            dataGridViewMain.Columns.Clear();
            if (!menuData[menuName].IsLoaded)
            {
                DataTable _table = Server_Session.Send_Recieve(new ClassLib.Message("subjects get")).Table;
                DataTable _tableTeachers = Server_Session.Send_Recieve(new ClassLib.Message("subjects get teacher")).Table;
                DataTable _tableDirections = Server_Session.Send_Recieve(new ClassLib.Message("subjects get direction")).Table;

                menuData[menuName].LoadTable(_table, new DataTable[] { _tableTeachers, _tableDirections });
            }
            dataGridViewMain.DataSource = CurrentMenuTable;
            dataGridViewMain.Columns[0].Visible = false;
            string[] columnNames = new string[] { "teacher_id", "direction_id" };
            int ptr = 0;
            foreach (DataTable table in menuData[menuName].additionalColumns)
            {
                table.TableName = columnNames[ptr];
                table.Columns[1].ColumnName = "val";
                List<object> values = new List<object>();
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                foreach (DataRow row in table.Rows)
                    values.Add(row.ItemArray[1]);

                column.Name = columnNames[ptr] + "Comb";
                column.Items.AddRange(values.ToArray());

                dataGridViewMain.Columns.Insert(0, column);
                ptr++;
            }
            for (ptr = 0; ptr < columnNames.Count<string>(); ++ptr)
                foreach (DataGridViewRow row in dataGridViewMain.Rows)
                    row.Cells[columnNames[ptr] + "Comb"].Value = menuData[menuName].additionalColumns[ptr].Select($"id='{row.Cells[columnNames[ptr]].Value}'")[0].ItemArray[1];

            foreach (string item in columnNames)
                dataGridViewMain.Columns[item].Visible = false;
            SaveChangesButton.Enabled = !menuData[menuName].Saved;
            Filling = false;
        }

        private void admin_Stusents(object sender, EventArgs e)
        {
            menuName = "students";
            Filling = true;
            dataGridViewMain.DataSource = null;
            dataGridViewMain.Columns.Clear();
            if (!menuData[menuName].IsLoaded)
            {
                DataTable _table = Server_Session.Send_Recieve(new ClassLib.Message("students get")).Table;
                DataTable _tableGroups = Server_Session.Send_Recieve(new ClassLib.Message("students get group")).Table;
                DataTable _tableUsers = Server_Session.Send_Recieve(new ClassLib.Message("students get user")).Table;

                menuData[menuName].LoadTable(_table, new DataTable[] { _tableGroups, _tableUsers });
            }
            dataGridViewMain.DataSource = CurrentMenuTable;
            dataGridViewMain.Columns[0].Visible = false;
            string[] columnNames = new string[] { "group_id", "user_id" };
            int ptr = 0;
            foreach (DataTable table in menuData[menuName].additionalColumns)
            {
                table.TableName = columnNames[ptr];
                table.Columns[1].ColumnName = "val";
                List<object> values = new List<object>();
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                foreach (DataRow row in table.Rows)
                    values.Add(row.ItemArray[1]);

                column.Name = columnNames[ptr] + "Comb";
                column.Items.AddRange(values.ToArray());

                dataGridViewMain.Columns.Insert(0, column);
                ptr++;
            }
            for (ptr = 0; ptr < columnNames.Count<string>(); ++ptr)
                foreach (DataGridViewRow row in dataGridViewMain.Rows)
                    row.Cells[columnNames[ptr] + "Comb"].Value = menuData[menuName].additionalColumns[ptr].Select($"id='{row.Cells[columnNames[ptr]].Value}'")[0].ItemArray[1];

            foreach (string item in columnNames)
                dataGridViewMain.Columns[item].Visible = false;
            SaveChangesButton.Enabled = !menuData[menuName].Saved;
            Filling = false;
        }

        private void admin_Semsubject(object sender, EventArgs e)
        {
            menuName = "semsubject";
            Filling = true;
            dataGridViewMain.DataSource = null;
            dataGridViewMain.Columns.Clear();
            if (!menuData[menuName].IsLoaded)
            {
                DataTable _table = Server_Session.Send_Recieve(new ClassLib.Message("semsubject get")).Table;
                DataTable _tableTeachers = Server_Session.Send_Recieve(new ClassLib.Message("semsubject get teacher")).Table;
                DataTable _tableGroups = Server_Session.Send_Recieve(new ClassLib.Message("semsubject get group")).Table;
                DataTable _tableDirections = Server_Session.Send_Recieve(new ClassLib.Message("subjects get direction")).Table;
                DataTable _tableSubjects = Server_Session.Send_Recieve(new ClassLib.Message("subjects get")).Table;

                menuData[menuName].LoadTable(_table, new DataTable[] { _tableTeachers, _tableGroups, _tableDirections, _tableSubjects });
            }
            dataGridViewMain.DataSource = CurrentMenuTable;
            dataGridViewMain.Columns[0].Visible = false;
            string[] columnNames = new string[] { "teacher_id", "group_id", "direction_id", "subject_id" };
            int ptr = 0;
            foreach (DataTable table in menuData[menuName].additionalColumns)
            {
                table.TableName = columnNames[ptr];
                table.Columns[1].ColumnName = "val";
                List<object> values = new List<object>();
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                foreach (DataRow row in table.Rows)
                    values.Add(row.ItemArray[1]);

                column.Name = columnNames[ptr] + "Comb";
                column.Items.AddRange(values.ToArray());

                dataGridViewMain.Columns.Insert(0, column);
                ptr++;
            }
            for (ptr = 0; ptr < columnNames.Count<string>(); ++ptr)
                foreach (DataGridViewRow row in dataGridViewMain.Rows)
                    row.Cells[columnNames[ptr] + "Comb"].Value = menuData[menuName].additionalColumns[ptr].Select($"id = '{row.Cells[columnNames[ptr]].Value}'")[0].ItemArray[1];

            foreach (string item in columnNames)
                dataGridViewMain.Columns[item].Visible = false;
            SaveChangesButton.Enabled = !menuData[menuName].Saved;
            Filling = false;
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
            if (!Filling)
            {
                if (dataGridViewMain.Columns[e.ColumnIndex].Name.Contains("Comb"))
                {
                    string colname = dataGridViewMain.Columns[e.ColumnIndex].Name.Replace("Comb", "");
                    DataTable additional = null;
                    foreach (DataTable _table in menuData[menuName].additionalColumns)
                        if (_table.TableName == colname)
                            additional = _table;

                    dataGridViewMain[colname, e.RowIndex].Value = additional.Select($"val = '{dataGridViewMain[e.ColumnIndex, e.RowIndex].Value}'")[0].ItemArray[0];
                }

                Log.Print($"RowIndex={e.RowIndex}; NewValue={dataGridViewMain[e.ColumnIndex, e.RowIndex].Value}");

                menuData[menuName].addChangedRow("upd", menuData[menuName].ActualTable.Rows[e.RowIndex]);

                SaveChangesButton.Enabled = !menuData[menuName].Saved;
            }
        }


    }
}
