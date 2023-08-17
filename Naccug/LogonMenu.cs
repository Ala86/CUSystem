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
    public partial class LogonScreen : Form
    {
        public LogonScreen()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainMenu myMainMenu = new MainMenu();
            myMainMenu.Show();
         /*   AdminMenu myAdminMenu = new AdminMenu();
            myAdminMenu.Show();*/
        }
    }
}
