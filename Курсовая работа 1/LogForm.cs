using System;
using System.Windows.Forms;

namespace Курсовая_работа_1
{
    public partial class LogForm : Form
    {
        public LogForm()
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
            logTextBox.Text += $"[{DateTime.Now}] {msg}\n";
        }
    }
}
