using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Курсовая_работа_1
{
    public enum Role
    {
        None, Admin, Lecturer, Teacher, Student
    }

    public static class UserInfo
    {
        public static Role UserRole = Role.None;
    }

    public class Session
    {
        public string ServerAddress => _serverIp;
        public bool IsOpen { get; private set; }

        private TcpClient _client;
        private NetworkStream _stream;

        private static Session _session = null;
        private static string _serverIp = "localhost";
        private static int _serverPort = 2000;

        private Session()
        {
            IsOpen = false;
            _client = new TcpClient(_serverIp, _serverPort);
            _stream = _client.GetStream();
            IsOpen = true;
        }

        public static Session GetSession()
        {
            if (_session==null)
            {
                _session = new Session();
                return _session;
            }
            else
            {
                return _session;
            }
        }

        public DataTable Send_Recieve(string msg)
        {
            if (!IsOpen)
                throw new Exception("Соединение не установлено. Сначала используйте Open()");

            var message = Encoding.UTF8.GetBytes(msg);

            try
            {
                _stream.Write(message, 0, message.Length);
            }
            catch (Exception)
            {
                IsOpen = false;
                Log.Print($"Потеряно соединение с сервером {ServerAddress}");
                MessageBox.Show("Потеряно соединение с сервером");
                throw new Exception("Потеряно соединение с сервером");
            }

            int waitingTime = 0;
            int sleepTime = 5;
            int timeout = 5000;
            while (_client.Available == 0)
            {
                if (waitingTime >= timeout)
                {
                    IsOpen = false;
                    Log.Print($"Потеряно соединение с сервером {ServerAddress}");
                    MessageBox.Show("Потеряно соединение с сервером");
                    throw new Exception("Потеряно соединение с сервером");
                }
                Thread.Sleep(sleepTime);
                waitingTime += sleepTime;
            }

            var buff = new byte[512];
            var response = new StringBuilder();
            var receivedBytes = 0;

            do
            {
                receivedBytes = _stream.Read(buff, 0, buff.Length);
                response.Append(Encoding.UTF8.GetString(buff, 0, receivedBytes));
            } while (_stream.DataAvailable);


            var json = JObject.Parse(response.ToString());

            List<string> colNames = new List<string>();
            foreach (var child in json["columns"])
            {
                colNames.Add(child.Value<string>());
            }

            DataTable table = new DataTable();
            foreach (var name in colNames)
            {
                var column = table.Columns.Add();
                column.ColumnName = name;
                column.Caption = name;
            }

            foreach (JToken row in json["rows"])
            {
                List<object> list = new List<object>();
                foreach (var name in colNames)
                {
                    list.Add(row[name]);
                }
                table.Rows.Add(list.ToArray());
            }

            return table;
        }

        //public DataTable[] Send_Recieve(string[] msgArray)
        //{
        //    List<DataTable> messages = new List<DataTable>();

        //    foreach (string msg in msgArray)
        //    {
        //        messages.Add(Send_Recieve(msg));
        //    }
        //    return messages.ToArray();
        //}

        public void Open()
        {
            if (IsOpen)
                throw new Exception("Session already open");
            try
            {
                _client.Connect(_serverIp, _serverPort);
                IsOpen = true;
            }
            catch (Exception e)
            {
                Log.Print($"Не удалось подлючиться к серверу {ServerAddress}");
                MessageBox.Show("Не удалось подлючиться к серверу");
                throw new Exception("Connection Error");
            }
        }

        public void Open(string serverIp, int serverPort)
        {
            _serverIp = serverIp;
            _serverPort = serverPort;

            if (IsOpen)
                throw new Exception("Session already open");
            try
            {
                _client.Connect(_serverIp, _serverPort);
                IsOpen = true;
            }
            catch (Exception)
            {
                Log.Print($"Не удалось подлючиться к серверу {ServerAddress}");
                MessageBox.Show("Не удалось подлючиться к серверу");
                throw new Exception("Connection Error");
            }
        }

        public void Close()
        {
            _client.Close();
            Log.Print($"Закрыто соединение с сервером {ServerAddress}");
        }
    }
}
