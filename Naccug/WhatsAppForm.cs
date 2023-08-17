using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhatsAppApi;

namespace WinTcare
{
    public partial class WhatsAppForm : Form
    {
        public WhatsAppForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // send (Button_Click)
            string from = "2209567888";
            string to = textBox1.Text;
            string msg = textBox2.Text;

            WhatsApp wa = new WhatsApp(from, "", "Ala", false, false);
            wa.OnConnectSuccess += () =>
            {
                MessageBox.Show("Connected to whatsapp...");
                wa.OnLoginSuccess += (phoneNumber, data) =>
                {

                    wa.SendMessage(to, msg);
                    MessageBox.Show("Message Sent...");
                };
                wa.OnLoginFailed += (data) =>
                {
                    MessageBox.Show("Login Failed : {0}", data);
                };
                wa.Login();
            };
            wa.OnConnectFailed += (ex) =>
            {
                MessageBox.Show("Connectin Failed...");
            };
            wa.Connect();
        }
    }
}
