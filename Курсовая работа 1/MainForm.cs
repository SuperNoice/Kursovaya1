using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Курсовая_работа_1
{
    public partial class MainForm : Form
    {
        private List<Button> _menuButtonsList;
        private Button _lastPressedButton;

        private string _menuName;
        private Dictionary<string, MenuData> _menuData;
        private DataTable CurrentMenuTable => _menuData[_menuName].ActualTable;

        private List<string[]> _messagesStek;
        private bool _filling;

        public MainForm()
        {
            _menuData = new Dictionary<string, MenuData>();
            _menuData.Add("users", new MenuData("users"));
            _menuData.Add("admins", new MenuData("admins"));
            _menuData.Add("directions", new MenuData("directions"));
            _menuData.Add("groups", new MenuData("groups"));
            _menuData.Add("teachers", new MenuData("teachers"));
            _menuData.Add("subjects", new MenuData("subjects"));
            _menuData.Add("students", new MenuData("students"));
            _menuData.Add("semsubject", new MenuData("semsubject"));

            _messagesStek = new List<string[]>();
            _menuButtonsList = new List<Button>();

            _filling = true;
            InitializeComponent();
            InitializeMenu();
            _filling = false;
        }

        private void InitializeMenu()
        {
            string[] _menuButtonNames = null;
            EventHandler[] _menuButtonEvents = null;

            switch (UserInfo.UserRole)
            {
                case Role.None:
                    Close();
                    break;
                case Role.Admin:
                    _menuButtonNames = new string[] { "Пользователи", "Администраторы", "Направления", "Группы", "Преподаватели", "Предметы", "Студенты", "Семестры" };
                    _menuButtonEvents = new EventHandler[] { admin_Users, admin_Admins, admin_Direstions, admin_Groups, admin_Teachers, admin_Subjects, admin_Stusents, admin_Semsubject };
                    break;
                case Role.Lecturer:
                    _menuButtonNames = new string[] { "Задания" };
                    _menuButtonEvents = new EventHandler[] { lector_Tasks };
                    break;
                case Role.Teacher:
                    _menuButtonNames = new string[] { "Студенты", "Задания" };
                    _menuButtonEvents = new EventHandler[] { teachers_Stusents, teachers_Tasks };
                    break;
                case Role.Student:
                    _menuButtonNames = new string[] { "Задания", "Преподаватели" };
                    _menuButtonEvents = new EventHandler[] { student_Tasks, student_Teachers };
                    break;
                default:
                    break;
            }

            for (int ptr = 0; ptr < _menuButtonNames.Length; ++ptr)
            {
                _menuButtonsList.Add(MenuButton.getMenuButton(_menuButtonNames[ptr], MenuPanel));
                _menuButtonsList[ptr].Click += _menuButtonEvents[ptr];
                _menuButtonsList[ptr].Click += new EventHandler(ChangeButtonColor);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            _filling = true;
            DataRow _row = _menuData[_menuName].ActualTable.Rows.Add();
            _menuData[_menuName].addChangedRow("add", _row);

            SaveChangesButton.Enabled = !_menuData[_menuName].Saved;
            _filling = false;
        }

        private void DelButton_Click(object sender, EventArgs e)
        {
            _filling = true;
            foreach (DataGridViewRow row in dataGridViewMain.SelectedRows)
            {
                _menuData[_menuName].addChangedRow("del", _menuData[_menuName].ActualTable.Rows[row.Index]);
                _menuData[_menuName].ActualTable.Rows.RemoveAt(row.Index);
            }

            SaveChangesButton.Enabled = !_menuData[_menuName].Saved;
            _filling = false;
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            _messagesStek.Add(_menuData[_menuName].SaveChanges());
            //string[] answers = Session.GetSession().Send_Recieve(_messagesStek.Last<string[]>());
            _messagesStek.Remove(_messagesStek.Last<string[]>());

            SaveChangesButton.Enabled = !_menuData[_menuName].Saved;
            updButton.Enabled = true;
        }

        private void updButton_Click(object sender, EventArgs e)
        {
            _menuData[_menuName].IsLoaded = false;
            _lastPressedButton.PerformClick();
            updButton.Enabled = false;
        }

        private void ChangeButtonColor(object sender, EventArgs e)
        {
            if (_lastPressedButton != null)
                _lastPressedButton.BackColor = SystemColors.ControlLight;
            ((Button)sender).BackColor = Color.FromArgb(165, 208, 255);
            _lastPressedButton = (Button)sender;
        }

        private void SetTable(DataTable mainTable)
        {
            _filling = true;
            dataGridViewMain.DataSource = null;
            dataGridViewMain.Columns.Clear();
            if (!_menuData[_menuName].IsLoaded)
            {
                _menuData[_menuName].LoadTable(mainTable);
            }
            dataGridViewMain.DataSource = CurrentMenuTable;
            dataGridViewMain.Columns[0].Visible = false;

            SaveChangesButton.Enabled = !_menuData[_menuName].Saved;
            _filling = false;
        }

        private void SetTable(DataTable mainTable, string[] columnNamesForReplaceByAdditionals, DataTable[] additionals)
        {
            _filling = true;
            dataGridViewMain.DataSource = null;
            dataGridViewMain.Columns.Clear();
            if (!_menuData[_menuName].IsLoaded)
            {
                _menuData[_menuName].LoadTable(mainTable, additionals);
            }
            dataGridViewMain.DataSource = CurrentMenuTable;
            dataGridViewMain.Columns[0].Visible = false;

            int ptr = 0;
            foreach (DataTable table in _menuData[_menuName].additionalColumns)
            {
                table.TableName = columnNamesForReplaceByAdditionals[ptr];
                table.Columns[1].ColumnName = "val";
                List<object> values = new List<object>();
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                foreach (DataRow row in table.Rows)
                    values.Add(row.ItemArray[1]);

                column.Name = columnNamesForReplaceByAdditionals[ptr].Replace("id", "List");
                column.Items.AddRange(values.ToArray());

                dataGridViewMain.Columns.Insert(0, column);
                ptr++;
            }

            for (ptr = 0; ptr < columnNamesForReplaceByAdditionals.Count<string>(); ++ptr)
                foreach (DataGridViewRow row in dataGridViewMain.Rows)
                    row.Cells[columnNamesForReplaceByAdditionals[ptr].Replace("id", "List")].Value = _menuData[_menuName].additionalColumns[ptr].Select($"id='{row.Cells[columnNamesForReplaceByAdditionals[ptr]].Value}'")[0].ItemArray[1];


            foreach (string item in columnNamesForReplaceByAdditionals)
                dataGridViewMain.Columns[item].Visible = false;
            SaveChangesButton.Enabled = !_menuData[_menuName].Saved;
            _filling = false;
        }

        // Функции кнопок меню
        private void admin_Users(object sender, EventArgs e)
        {
            _menuName = "users";
            DataTable _table = Session.GetSession().Send_Recieve("SELECT * FROM `user`");
            DataTable _tableRoles = Session.GetSession().Send_Recieve("SELECT * FROM `role`");
            string[] columnNames = new string[] { "role_id" };
            SetTable(_table, columnNames, new DataTable[] { _tableRoles });

        }

        private void admin_Admins(object sender, EventArgs e)
        {
            _menuName = "admins";
            DataTable _table = Session.GetSession().Send_Recieve("SELECT id, login FROM `user` WHERE role_id = (SELECT id FROM `role` WHERE name = 'admin')");
            DataTable _tableUsers = Session.GetSession().Send_Recieve("SELECT id, login FROM `user`");
            string[] columnNames = new string[] { "login" };
            SetTable(_table, columnNames, new DataTable[] { _tableUsers });
        }

        private void admin_Direstions(object sender, EventArgs e)
        {
            _menuName = "directions";
            DataTable _table = Session.GetSession().Send_Recieve("SELECT * FROM `direction`");
            SetTable(_table);
        }

        private void admin_Groups(object sender, EventArgs e)
        {
            _menuName = "groups";
            DataTable _table = Session.GetSession().Send_Recieve("SELECT * FROM `group`");
            DataTable _tabledDirections = Session.GetSession().Send_Recieve("SELECT id, code FROM `direction`");
            string[] columnNames = new string[] { "direction_id" };
            SetTable(_table, columnNames, new DataTable[] { _tabledDirections });
        }

        private void admin_Teachers(object sender, EventArgs e)
        {
            _menuName = "teachers";
            DataTable _table = Session.GetSession().Send_Recieve("SELECT * FROM `teacher`");
            DataTable _tableUsers = Session.GetSession().Send_Recieve("SELECT id, login FROM `user`");
            string[] columnNames = new string[] { "user_id" };
            SetTable(_table, columnNames, new DataTable[] { _tableUsers });
        }

        private void admin_Subjects(object sender, EventArgs e)
        {
            _menuName = "subjects";
            DataTable _table = Session.GetSession().Send_Recieve("SELECT * FROM `subject`");
            DataTable _tableTeachers = Session.GetSession().Send_Recieve("SELECT `teacher`.id, `user`.fio FROM `teacher` INNER JOIN `user` ON `teacher`.user_id = `user`.id");
            DataTable _tableDirections = Session.GetSession().Send_Recieve("SELECT id, code FROM `direction`");
            string[] columnNames = new string[] { "teacher_id", "direction_id" };
            SetTable(_table, columnNames, new DataTable[] { _tableTeachers, _tableDirections });
        }

        private void admin_Stusents(object sender, EventArgs e)
        {
            _menuName = "students";
            DataTable _table = Session.GetSession().Send_Recieve("SELECT * FROM `student`");
            DataTable _tableGroups = Session.GetSession().Send_Recieve("SELECT id, code FROM `group`");
            DataTable _tableUsers = Session.GetSession().Send_Recieve("SELECT id, login FROM `user`");
            string[] columnNames = new string[] { "group_id", "user_id" };
            SetTable(_table, columnNames, new DataTable[] { _tableGroups, _tableUsers });
        }

        private void admin_Semsubject(object sender, EventArgs e)
        {
            _menuName = "semsubject";
            DataTable _table = Session.GetSession().Send_Recieve("SELECT * FROM `semestr_subject`");
            DataTable _tableSubjects = Session.GetSession().Send_Recieve("SELECT id, name FROM `subject`");
            DataTable _tableTeachers = Session.GetSession().Send_Recieve("SELECT `teacher`.id, `user`.fio FROM `teacher` INNER JOIN `user` ON `teacher`.user_id = `user`.id");
            DataTable _tableGroups = Session.GetSession().Send_Recieve("SELECT id, code FROM `group`");
            string[] columnNames = new string[] { "subject_id", "teacher_id", "group_id" };
            SetTable(_table, columnNames, new DataTable[] { _tableSubjects, _tableTeachers, _tableGroups });
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
            if (!_filling)
            {
                if (dataGridViewMain.Columns[e.ColumnIndex].Name.Contains("Comb"))
                {
                    string colname = dataGridViewMain.Columns[e.ColumnIndex].Name.Replace("Comb", "");
                    DataTable additional = null;
                    foreach (DataTable _table in _menuData[_menuName].additionalColumns)
                        if (_table.TableName == colname)
                            additional = _table;

                    dataGridViewMain[colname, e.RowIndex].Value = additional.Select($"val = '{dataGridViewMain[e.ColumnIndex, e.RowIndex].Value}'")[0].ItemArray[0];
                }

                Log.Print($"RowIndex={e.RowIndex}; NewValue={dataGridViewMain[e.ColumnIndex, e.RowIndex].Value}");

                _menuData[_menuName].addChangedRow("upd", _menuData[_menuName].ActualTable.Rows[e.RowIndex]);

                SaveChangesButton.Enabled = !_menuData[_menuName].Saved;
            }
        }


    }
}
