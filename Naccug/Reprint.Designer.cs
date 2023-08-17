namespace WinTcare
{
    partial class Reprint
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
            this.button1 = new System.Windows.Forms.Button();
            this.cashviewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(239, 59);
            this.button1.TabIndex = 0;
            this.button1.Text = "Reprint";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cashviewer
            // 
            this.cashviewer.ActiveViewIndex = -1;
            this.cashviewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cashviewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.cashviewer.Location = new System.Drawing.Point(4, 78);
            this.cashviewer.Name = "cashviewer";
            this.cashviewer.Size = new System.Drawing.Size(260, 87);
            this.cashviewer.TabIndex = 1;
            // 
            // Reprint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 76);
            this.Controls.Add(this.cashviewer);
            this.Controls.Add(this.button1);
            this.Name = "Reprint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reprint";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer cashviewer;
    }
}