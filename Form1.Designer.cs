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
            // Form1
            // 
            AccessibleName = "Busy Light";
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 361);
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
            MaximumSize = new Size(800, 400);
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
    }
}
