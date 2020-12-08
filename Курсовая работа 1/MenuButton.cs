using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсовая_работа_1
{
    public static class MenuButton
    {
        static private int _TabIndex = 1;
        static private int _ButtonIndex = 1;
        static private string _name => "MenuButton" + _ButtonIndex++.ToString();

        public static Button getMenuButton(string text)
        {
            Button button = getButton(text);
            return button;
        }

        public static Button getMenuButton(string text, FlowLayoutPanel panel)
        {
            Button button = getButton(text);
            panel.Controls.Add(button);

            return button;
        }

        private static Button getButton(string text)
        {
            Button button = new Button();

            button.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button.Location = new System.Drawing.Point(1, 2);
            button.Margin = new System.Windows.Forms.Padding(0);
            button.Name = _name;
            button.Size = new System.Drawing.Size(166, 38);
            button.TabIndex = _TabIndex++;
            button.Text = text;
            button.UseVisualStyleBackColor = true;

            return button;
        }
    }
}
