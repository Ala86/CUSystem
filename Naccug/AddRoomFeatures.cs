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
    public partial class AddRoomFeatures : Form
    {
        public AddRoomFeatures()
        {
            InitializeComponent();
        }

        private void AddRoomFeatures_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Add Room Features >>";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
