using Busy_Light.RC;
using System;

namespace Busy_Light
{
  
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        ///  Clean up any resources being used.
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


        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            trackBar1 = new TrackBar();
            label1 = new Label();
            label2 = new Label();
            comboBox1 = new ComboBox();
            button1 = new Button();
            btnLogin = new Button();
            textBox1 = new TextBox();
            Info = new Button();
            button2 = new Button();
            checkBox1 = new CheckBox();
            notifyIcon1 = new NotifyIcon(components);
            checkBox2 = new CheckBox();
            comboBox2 = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            comboBox3 = new ComboBox();
            label6 = new Label();
            comboBox4 = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(636, 14);
            trackBar1.Margin = new Padding(5);
            trackBar1.Maximum = 255;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(134, 45);
            trackBar1.SmallChange = 10;
            trackBar1.TabIndex = 1;
            trackBar1.Scroll += trackBar1_Scroll;
            trackBar1.ValueChanged += trackBar1_Scroll;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(554, 14);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(86, 18);
            label1.TabIndex = 2;
            label1.Text = "Brightness:";
            label1.TextAlign = ContentAlignment.TopCenter;
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(183, 14);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(148, 18);
            label2.TabIndex = 3;
            label2.Text = "Manually Set Status:";
            label2.TextAlign = ContentAlignment.TopCenter;
            label2.Click += label2_Click;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(183, 42);
            comboBox1.Margin = new Padding(5);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(154, 26);
            comboBox1.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(363, 42);
            button1.Margin = new Padding(5);
            button1.Name = "button1";
            button1.Size = new Size(96, 26);
            button1.TabIndex = 5;
            button1.Text = "Submit";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(544, 333);
            btnLogin.Margin = new Padding(5);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(96, 26);
            btnLogin.TabIndex = 6;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // textBox1
            // 
            textBox1.AccessibleName = "textBox1";
            textBox1.Location = new Point(655, 333);
            textBox1.Margin = new Padding(5);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(127, 26);
            textBox1.TabIndex = 7;
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // Info
            // 
            Info.FlatStyle = FlatStyle.Popup;
            Info.Font = new Font("Arial Rounded MT Bold", 14.25F, FontStyle.Italic, GraphicsUnit.Point);
            Info.ForeColor = SystemColors.ControlDark;
            Info.Location = new Point(11, 294);
            Info.Margin = new Padding(4);
            Info.Name = "Info";
            Info.Size = new Size(163, 54);
            Info.TabIndex = 8;
            Info.Text = "Info";
            Info.TextAlign = ContentAlignment.MiddleLeft;
            Info.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Arial Rounded MT Bold", 14.25F, FontStyle.Italic, GraphicsUnit.Point);
            button2.ForeColor = SystemColors.ControlDark;
            button2.Location = new Point(11, 14);
            button2.Margin = new Padding(4);
            button2.Name = "button2";
            button2.Size = new Size(163, 55);
            button2.TabIndex = 9;
            button2.Text = "Settings";
            button2.TextAlign = ContentAlignment.MiddleLeft;
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(186, 82);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(129, 22);
            checkBox1.TabIndex = 10;
            checkBox1.Text = "Run on Startup";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // notifyIcon1
            // 
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(186, 110);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(197, 22);
            checkBox2.TabIndex = 11;
            checkBox2.Text = "Message Recieved Alert";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // comboBox2
            // 
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(665, 87);
            comboBox2.Margin = new Padding(5);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(105, 26);
            comboBox2.TabIndex = 12;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(636, 64);
            label3.Name = "label3";
            label3.Size = new Size(116, 18);
            label3.TabIndex = 13;
            label3.Text = "Custom Colors:";
            label3.Click += label3_Click_1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(560, 90);
            label4.Name = "label4";
            label4.Size = new Size(97, 18);
            label4.TabIndex = 14;
            label4.Text = "Oncall Color:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(560, 126);
            label5.Name = "label5";
            label5.Size = new Size(97, 18);
            label5.TabIndex = 16;
            label5.Text = "Offcall Color:";
            // 
            // comboBox3
            // 
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(665, 123);
            comboBox3.Margin = new Padding(5);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(105, 26);
            comboBox3.TabIndex = 15;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(468, 162);
            label6.Name = "label6";
            label6.Size = new Size(189, 18);
            label6.TabIndex = 18;
            label6.Text = "Message Recieved Color:";
            // 
            // comboBox4
            // 
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(665, 159);
            comboBox4.Margin = new Padding(5);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(105, 26);
            comboBox4.TabIndex = 17;
            // 
            // Form1
            // 
            AccessibleName = "Busy Light";
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 366);
            Controls.Add(label6);
            Controls.Add(comboBox4);
            Controls.Add(label5);
            Controls.Add(comboBox3);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(comboBox2);
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(button2);
            Controls.Add(Info);
            Controls.Add(textBox1);
            Controls.Add(btnLogin);
            Controls.Add(button1);
            Controls.Add(comboBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(trackBar1);
            Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(5);
            MaximumSize = new Size(800, 500);
            MinimumSize = new Size(800, 400);
            Name = "Form1";
            Text = "Busy Light";
            TransparencyKey = Color.FromArgb(255, 128, 0);
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private TrackBar trackBar1;
        private Label label1;
        private Label label2;
        private ComboBox comboBox1;
        private Button button1;
        private Button btnLogin;
        private TextBox textBox1;
        private Button Info;
        private Button button2;
        private CheckBox checkBox1;
        private NotifyIcon notifyIcon1;
        private CheckBox checkBox2;
        private ComboBox comboBox2;
        private Label label3;
        private Label label4;
        private Label label5;
        private ComboBox comboBox3;
        private Label label6;
        private ComboBox comboBox4;
    }
}
