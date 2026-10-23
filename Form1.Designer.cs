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
            brightnessTrack = new TrackBar();
            label1 = new Label();
            label2 = new Label();
            manStatusBox = new ComboBox();
            submitButton = new Button();
            infoButton = new Button();
            settingsButton = new Button();
            runOnStartup = new CheckBox();
            notifyIcon1 = new NotifyIcon(components);
            msgRecCheck = new CheckBox();
            oncallColorBox = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            offCallColor = new ComboBox();
            label6 = new Label();
            msgRecColor = new ComboBox();
            infoText = new Label();
            rotateColor = new CheckBox();
            ConnectionStatusText = new Label();
            loginButton = new Button();
            ((System.ComponentModel.ISupportInitialize)brightnessTrack).BeginInit();
            SuspendLayout();
            // 
            // brightnessTrack
            // 
            brightnessTrack.Location = new Point(636, 14);
            brightnessTrack.Margin = new Padding(5);
            brightnessTrack.Maximum = 255;
            brightnessTrack.Name = "brightnessTrack";
            brightnessTrack.Size = new Size(134, 45);
            brightnessTrack.SmallChange = 10;
            brightnessTrack.TabIndex = 1;
            brightnessTrack.Scroll += trackBar1_Scroll;
            brightnessTrack.ValueChanged += trackBar1_Scroll;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(518, 14);
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
            // manStatusBox
            // 
            manStatusBox.DropDownStyle = ComboBoxStyle.DropDownList;
            manStatusBox.FormattingEnabled = true;
            manStatusBox.Location = new Point(183, 42);
            manStatusBox.Margin = new Padding(5);
            manStatusBox.Name = "manStatusBox";
            manStatusBox.Size = new Size(154, 26);
            manStatusBox.TabIndex = 4;
            // 
            // submitButton
            // 
            submitButton.Location = new Point(363, 42);
            submitButton.Margin = new Padding(5);
            submitButton.Name = "submitButton";
            submitButton.Size = new Size(96, 26);
            submitButton.TabIndex = 5;
            submitButton.Text = "Submit";
            submitButton.UseVisualStyleBackColor = true;
            submitButton.Click += button1_Click;
            // 
            // infoButton
            // 
            infoButton.FlatStyle = FlatStyle.Popup;
            infoButton.Font = new Font("Arial Rounded MT Bold", 14.25F, FontStyle.Italic);
            infoButton.ForeColor = SystemColors.ControlDark;
            infoButton.Location = new Point(11, 294);
            infoButton.Margin = new Padding(4);
            infoButton.Name = "infoButton";
            infoButton.Size = new Size(163, 54);
            infoButton.TabIndex = 8;
            infoButton.Text = "Info";
            infoButton.TextAlign = ContentAlignment.MiddleLeft;
            infoButton.UseVisualStyleBackColor = true;
            infoButton.Click += Info_Click;
            // 
            // settingsButton
            // 
            settingsButton.FlatStyle = FlatStyle.Popup;
            settingsButton.Font = new Font("Arial Rounded MT Bold", 14.25F, FontStyle.Italic);
            settingsButton.ForeColor = SystemColors.ControlDark;
            settingsButton.Location = new Point(11, 14);
            settingsButton.Margin = new Padding(4);
            settingsButton.Name = "settingsButton";
            settingsButton.Size = new Size(163, 55);
            settingsButton.TabIndex = 9;
            settingsButton.Text = "Settings";
            settingsButton.TextAlign = ContentAlignment.MiddleLeft;
            settingsButton.UseVisualStyleBackColor = true;
            settingsButton.Click += button2_Click;
            // 
            // runOnStartup
            // 
            runOnStartup.AutoSize = true;
            runOnStartup.Location = new Point(186, 82);
            runOnStartup.Name = "runOnStartup";
            runOnStartup.Size = new Size(129, 22);
            runOnStartup.TabIndex = 10;
            runOnStartup.Text = "Run on Startup";
            runOnStartup.UseVisualStyleBackColor = true;
            runOnStartup.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // notifyIcon1
            // 
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // msgRecCheck
            // 
            msgRecCheck.AutoSize = true;
            msgRecCheck.Location = new Point(186, 110);
            msgRecCheck.Name = "msgRecCheck";
            msgRecCheck.Size = new Size(197, 22);
            msgRecCheck.TabIndex = 11;
            msgRecCheck.Text = "Message Recieved Alert";
            msgRecCheck.UseVisualStyleBackColor = true;
            // 
            // oncallColorBox
            // 
            oncallColorBox.DropDownStyle = ComboBoxStyle.DropDownList;
            oncallColorBox.FormattingEnabled = true;
            oncallColorBox.Location = new Point(665, 87);
            oncallColorBox.Margin = new Padding(5);
            oncallColorBox.Name = "oncallColorBox";
            oncallColorBox.Size = new Size(105, 26);
            oncallColorBox.TabIndex = 12;
            oncallColorBox.SelectedValueChanged += oncallColorBox_SelectionChanged;
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
            // offCallColor
            // 
            offCallColor.DropDownStyle = ComboBoxStyle.DropDownList;
            offCallColor.FormattingEnabled = true;
            offCallColor.Location = new Point(665, 123);
            offCallColor.Margin = new Padding(5);
            offCallColor.Name = "offCallColor";
            offCallColor.Size = new Size(105, 26);
            offCallColor.TabIndex = 15;
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
            // msgRecColor
            // 
            msgRecColor.DropDownStyle = ComboBoxStyle.DropDownList;
            msgRecColor.FormattingEnabled = true;
            msgRecColor.Location = new Point(665, 159);
            msgRecColor.Margin = new Padding(5);
            msgRecColor.Name = "msgRecColor";
            msgRecColor.Size = new Size(105, 26);
            msgRecColor.TabIndex = 17;
            // 
            // infoText
            // 
            infoText.AutoSize = true;
            infoText.Font = new Font("Arial", 10F);
            infoText.Location = new Point(181, 14);
            infoText.Name = "infoText";
            infoText.Size = new Size(45, 16);
            infoText.TabIndex = 19;
            infoText.Text = "label7";
            // 
            // rotateColor
            // 
            rotateColor.AutoSize = true;
            rotateColor.Location = new Point(186, 138);
            rotateColor.Name = "rotateColor";
            rotateColor.Size = new Size(180, 22);
            rotateColor.TabIndex = 20;
            rotateColor.Text = "Rotating Status Light?";
            rotateColor.UseVisualStyleBackColor = true;
            rotateColor.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // ConnectionStatusText
            // 
            ConnectionStatusText.BorderStyle = BorderStyle.FixedSingle;
            ConnectionStatusText.Location = new Point(650, 331);
            ConnectionStatusText.Name = "ConnectionStatusText";
            ConnectionStatusText.Size = new Size(122, 26);
            ConnectionStatusText.TabIndex = 21;
            // 
            // loginButton
            // 
            loginButton.FlatStyle = FlatStyle.Popup;
            loginButton.Font = new Font("Arial Rounded MT Bold", 14.25F, FontStyle.Italic);
            loginButton.ForeColor = SystemColors.ControlDark;
            loginButton.Location = new Point(11, 218);
            loginButton.Margin = new Padding(4);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(163, 54);
            loginButton.TabIndex = 22;
            loginButton.Text = "Login";
            loginButton.TextAlign = ContentAlignment.MiddleLeft;
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += btnLogin_Click;
            // 
            // Form1
            // 
            AccessibleName = "Busy Light";
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 366);
            Controls.Add(loginButton);
            Controls.Add(ConnectionStatusText);
            Controls.Add(rotateColor);
            Controls.Add(infoText);
            Controls.Add(label6);
            Controls.Add(msgRecColor);
            Controls.Add(label5);
            Controls.Add(offCallColor);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(oncallColorBox);
            Controls.Add(msgRecCheck);
            Controls.Add(runOnStartup);
            Controls.Add(settingsButton);
            Controls.Add(infoButton);
            Controls.Add(submitButton);
            Controls.Add(manStatusBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(brightnessTrack);
            Font = new Font("Arial", 12F);
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
            ((System.ComponentModel.ISupportInitialize)brightnessTrack).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private TrackBar brightnessTrack;
        private Label label1;
        private Label label2;
        private ComboBox manStatusBox;
        private Button submitButton;
        private Button infoButton;
        private Button settingsButton;
        private CheckBox runOnStartup;
        private NotifyIcon notifyIcon1;
        private CheckBox msgRecCheck;
        private ComboBox oncallColorBox;
        private Label label3;
        private Label label4;
        private Label label5;
        private ComboBox offCallColor;
        private Label label6;
        private ComboBox msgRecColor;
        private Label infoText;
        private CheckBox rotateColor;
        private Label ConnectionStatusText;
        private Button loginButton;
    }
}
