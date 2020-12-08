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
    public partial class logWindow : Form
    {
        public logWindow()
        {
            InitializeComponent();
            logTextBox.Text += '\n';
            Log.logTextBox = logTextBox;
        }
    }

    public static class Log
    {
        public static RichTextBox logTextBox;

        public static void Print(string msg)
        {
            logTextBox.Text += $"[{DateTime.Now.ToString()}] {msg}\n";
        }
    }
}
