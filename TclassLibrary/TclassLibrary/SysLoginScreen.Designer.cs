namespace TclassLibrary
{
    partial class SysLoginScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SysLoginScreen));
            this.okButton = new System.Windows.Forms.Button();
            this.UserID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.UserPassword = new System.Windows.Forms.TextBox();
            this.panel15 = new System.Windows.Forms.Panel();
            this.label113 = new System.Windows.Forms.Label();
            this.panel15.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.BackColor = System.Drawing.Color.White;
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(174, 163);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(165, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "Login";
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // UserID
            // 
            this.UserID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.UserID.Cursor = System.Windows.Forms.Cursors.Default;
            this.UserID.Location = new System.Drawing.Point(174, 105);
            this.UserID.Name = "UserID";
            this.UserID.Size = new System.Drawing.Size(165, 20);
            this.UserID.TabIndex = 17;
            this.UserID.TextChanged += new System.EventHandler(this.UserID_TextChanged_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(91, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(91, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "User ID";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(392, 146);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 40);
            this.button1.TabIndex = 212;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UserPassword
            // 
            this.UserPassword.Location = new System.Drawing.Point(173, 131);
            this.UserPassword.Name = "UserPassword";
            this.UserPassword.PasswordChar = '*';
            this.UserPassword.Size = new System.Drawing.Size(165, 20);
            this.UserPassword.TabIndex = 19;
            //            this.UserPassword.TextChanged += new System.EventHandler(this.UserPassword_TextChanged_1);
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.Navy;
            this.panel15.Controls.Add(this.label113);
            this.panel15.Location = new System.Drawing.Point(175, 211);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(297, 26);
            this.panel15.TabIndex = 377;
            // 
            // label113
            // 
            this.label113.AutoSize = true;
            this.label113.ForeColor = System.Drawing.Color.White;
            this.label113.Location = new System.Drawing.Point(3, 7);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(289, 13);
            this.label113.TabIndex = 154;
            this.label113.Text = "Copyright NACCUG & T-care Systems Ltd, all rights Reserved";
            // 
            // LoginScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(244)))), ((int)(((byte)(253)))));
         //   this.BackgroundImage = global::TclassLibrary.Properties.Resources.naccugbkgr1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(467, 237);
            this.Controls.Add(this.panel15);
            this.Controls.Add(this.UserPassword);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.UserID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "T-Care Login Screen";
            this.Load += new System.EventHandler(this.SysLoginScreen_Load);
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox UserID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox UserPassword;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Label label113;
    }
}