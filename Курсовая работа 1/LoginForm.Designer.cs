namespace Курсовая_работа_1
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loginTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.loginErrorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // loginTextBox
            // 
            this.loginTextBox.Location = new System.Drawing.Point(92, 87);
            this.loginTextBox.Name = "loginTextBox";
            this.loginTextBox.Size = new System.Drawing.Size(118, 20);
            this.loginTextBox.TabIndex = 0;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(92, 120);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(118, 20);
            this.PasswordTextBox.TabIndex = 1;
            // 
            // loginButton
            // 
            this.loginButton.Font = new System.Drawing.Font("Papyrus", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.loginButton.Location = new System.Drawing.Point(102, 196);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(96, 38);
            this.loginButton.TabIndex = 2;
            this.loginButton.Text = "Войти";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Логин";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Пароль";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Papyrus", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(121, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 22);
            this.label3.TabIndex = 5;
            this.label3.Text = "Вход";
            // 
            // loginErrorLabel
            // 
            this.loginErrorLabel.AutoSize = true;
            this.loginErrorLabel.BackColor = System.Drawing.Color.Salmon;
            this.loginErrorLabel.Location = new System.Drawing.Point(77, 152);
            this.loginErrorLabel.Name = "loginErrorLabel";
            this.loginErrorLabel.Size = new System.Drawing.Size(151, 13);
            this.loginErrorLabel.TabIndex = 6;
            this.loginErrorLabel.Text = "Неверный логин или пароль";
            this.loginErrorLabel.Visible = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuBar;
            this.ClientSize = new System.Drawing.Size(294, 271);
            this.Controls.Add(this.loginErrorLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.loginTextBox);
            this.MaximumSize = new System.Drawing.Size(310, 310);
            this.MinimumSize = new System.Drawing.Size(310, 310);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox loginTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label loginErrorLabel;
    }
}