using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

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

    public class Message
    {
        public string Text { get; private set; }
        public DataTable Table { get; private set; }

        public Message(string msg)
        {
            Text = msg;
            Table = new DataTable();
        }

        public Message(string msg, DataTable table)
        {
            Text = msg;
            if (!table.IsInitialized)
                throw new Exception("Table not Initialized");
            Table = table;
        }
    }

    public class Session
    {
        private Socket local_socket;
        private string _local_IP = "127.0.0.1";
        private int _local_port = 16881;
        private IPEndPoint _serverEndPoint;
        public string ServerAddress => _serverEndPoint.ToString();
        public bool IsOpen { get; private set; }

        public Session(IPAddress serverIp, int serverPort)
        {
            IsOpen = false;
            local_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            bool IsBinded = false;
            while (!IsBinded)
            {
                try
                {
                    local_socket.Bind(new IPEndPoint(IPAddress.Parse(_local_IP), _local_port));
                    IsBinded = true;
                }
                catch (SocketException)
                {
                    _local_port++;
                }
            }

            _serverEndPoint = new IPEndPoint(serverIp, serverPort);
        }

        public Message Send_Recieve(Message msg)
        {
            if (!IsOpen)
                throw new Exception("Соединение не установлено. Сначала используйте Open()");

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream messageStream = new MemoryStream();

            formatter.Serialize(messageStream, msg);
            try
            {
                local_socket.Send(messageStream.ToArray());
            }
            catch (Exception)
            {
                IsOpen = false;
                Log.Print($"Потеряно соединение с сервером {ServerAddress}");
                MessageBox.Show("Потеряно соединение с сервером");
                throw new Exception("Потеряно соединение с сервером");
            }
            messageStream.Close();
            messageStream.Dispose();

            messageStream = new MemoryStream();

            byte[] buffer = new byte[2048];
            int waitingTime = 0;
            int sleepTime = 100;
            while (local_socket.Available == 0)
            {
                if (waitingTime > 5000)
                {
                    IsOpen = false;
                    Log.Print($"Потеряно соединение с сервером {ServerAddress}");
                    MessageBox.Show("Потеряно соединение с сервером");
                    throw new Exception("Потеряно соединение с сервером");
                }
                Thread.Sleep(sleepTime);
                waitingTime += sleepTime;
            }

            do
            {
                int receiveLength = local_socket.Receive(buffer);
                messageStream.Write(buffer, 0, receiveLength);
            } while (local_socket.Available > 0);

            return (Message)formatter.Deserialize(messageStream);
        }

        public Message[] Send_Recieve(Message[] msgArray)
        {
            List<Message> messages = new List<Message>();

            foreach (Message msg in msgArray)
                messages.Add(Send_Recieve(msg));
            
            return messages.ToArray();
        }

            public void Open()
        {
            if (IsOpen)
                throw new Exception("Session already open");
            try
            {
                local_socket.Connect(_serverEndPoint);
                IsOpen = true;
            }
            catch (Exception)
            {
                Log.Print($"Не удалось подлючиться к серверу {ServerAddress}");
                MessageBox.Show("Не удалось подлючиться к серверу");
                throw new Exception("Connection Error");
            }
        }

        public void Open(IPAddress serverIp, int serverPort)
        {
            _serverEndPoint = new IPEndPoint(serverIp, serverPort);

            if (IsOpen)
                throw new Exception("Session already open");
            try
            {
                local_socket.Connect(_serverEndPoint);
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
            local_socket.Close();
            Log.Print($"Закрыто соединение с сервером {ServerAddress}");
        }
    }
}
