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
    public partial class DeathConfirm : Form
    {
        public DeathConfirm()
        {
            InitializeComponent();
        }

        private void DeathConfirm_Load(object sender, EventArgs e)
        {
        //    this.ControlBox = false;
            this.Text = globalvar.cLocalCaption + "<< Confirmation of Death >>";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
