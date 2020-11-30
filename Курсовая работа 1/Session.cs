using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net.Sockets;

namespace Курсовая_работа_1
{
    class Message
    {
        public string text { get; set; }
        public DataTable data { get; }

        public Message()
        {
            text = "";
            data = new DataTable();
        }
    }

    class Session
    {
        Socket _server;

        public Session()
        {

        }
    }
}
