using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace Курсовая_работа_1
{
    struct Pair<T1, T2>
    {
        public T1 Command { set; get; }
        public T2 Row { set; get; }
    }

    class MenuData
    {
        public bool IsLoaded { get; set; }
        public string MenuName { get; private set; }
        public bool Saved => (ChangesRowsStek.Count == 0);
        public DataTable ActualTable { get; set; }
        public DataTable CashedTable { get; set; }
        public List<DataTable> additionalColumns { get; set; }

        private List<string> messages;
        private List<Pair<string, DataRow>> ChangesRowsStek;
        private DataTable TmpTable;

        public MenuData(string _name)
        {
            ActualTable = new DataTable();
            CashedTable = new DataTable();
            TmpTable = new DataTable();
            additionalColumns = new List<DataTable>();
            ChangesRowsStek = new List<Pair<string, DataRow>>();
            IsLoaded = false;
            MenuName = _name;
        }

        public void addChangedRow(string ChangeType, DataRow row)
        {
            Log.Print($"Строка изменена. Type={ChangeType}; RowIndex={ActualTable.Rows.IndexOf(row)};");

            bool _finded = false;
            foreach (Pair<string, DataRow> item in ChangesRowsStek)
            {
                if (item.Row == row)
                {
                    _finded = true;
                    if (item.Command != ChangeType && ChangeType != "upd")
                        ChangesRowsStek.Remove(item);
                }
            }
            if (!_finded)
            {
                if (ChangeType == "del")
                {
                    DataRow _newrow = TmpTable.NewRow();
                    _newrow.ItemArray = row.ItemArray;
                    ChangesRowsStek.Add(new Pair<string, DataRow> { Command = ChangeType, Row = _newrow });
                }
                else
                {
                    ChangesRowsStek.Add(new Pair<string, DataRow> { Command = ChangeType, Row = row });
                }
            }
        }

        public string[] GetMessages()   // todo: переделать на json
        {
            List<string> _messages = new List<string>();
            string _command = "";
            DataTable sendTable = null;

            foreach (Pair<string, DataRow> item in ChangesRowsStek)
            {
                if (item.Command != _command)
                {
                    sendTable = ActualTable.Clone();
                    _command = item.Command;
                    _messages.Add($"{MenuName} {_command}"); // Здесь
                }
                DataRow row = sendTable.NewRow();
                row.ItemArray = item.Row.ItemArray;
                sendTable.Rows.Add(row);
            }

            return _messages.ToArray();
        }

        public void LoadTable(DataTable _table, DataTable[] _additionalColubnsTables)
        {
            ActualTable = _table;
            CashedTable = ActualTable.Copy();
            TmpTable = ActualTable.Copy();
            additionalColumns.Clear();
            additionalColumns.AddRange(_additionalColubnsTables);
            IsLoaded = true;
        }

        public void LoadTable(DataTable _table)
        {
            ActualTable = _table;
            CashedTable = ActualTable.Copy();
            TmpTable = ActualTable.Copy();
            IsLoaded = true;
        }

        public string[] SaveChanges()
        {
            CashedTable = ActualTable.Copy();
            string[] _messages = GetMessages();

            foreach (string _message in _messages)
                Log.Print($"[{DateTime.Now}] Отправлена команда: {_message}");

            ChangesRowsStek.Clear();
            TmpTable.Rows.Clear();

            return _messages;
        }
    }
}
