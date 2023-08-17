using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinTcare
{
    public partial class MedicalCover : Form
    {
        //     private string nLocalCaption;nLocalCaption
        public MedicalCover()
        {
            InitializeComponent();
        }

        private void MedicalCover_Load(object sender, EventArgs e)
        {
//            this.Text = @nLocalCaption;
            this.Text = globalvar.cLocalCaption + "<<Medical Cover>>";
            this.label4.MaximumSize = new Size(50, 0);
            this.label4.AutoSize = true;
 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox3.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            groupBox3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
