using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using TclassLibrary;
using System.Data.SqlClient;
using System.Net;
using System.Net.NetworkInformation;

namespace WinTcare
{
    public partial class FRMLock : Form
    {
        string cs = globalvar.cos;
        int ncompid = 0;// globalvar.gnCompid;
        DateTime gdSystemDate = globalvar.gdSysDate;
        public FRMLock()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //  string duser = this.UserID.Text.Trim().ToUpper();
            using (SqlConnection nConnHandle = new SqlConnection(cs))
            {
                nConnHandle.Open();
                string dpass = this.chkpassword(this.textBox1.Text.Trim().ToUpper());
              //  MessageBox.Show("This is the password" + dpass);
                string dnewpass = tclassChkpassWord.dpassWord(textBox1.Text.Trim().ToUpper());
                string duser = globalvar.gcUserid.Trim();
                SqlDataReader cUserDetails = null;
                SqlCommand cGetUser = new SqlCommand("exec tsp_getUser @myUserID,@myUserPASS", nConnHandle);
                cGetUser.Parameters.Add("@myUserID", SqlDbType.VarChar).Value = duser;      // this.UserID.Text;
                cGetUser.Parameters.Add("@myUserPASS", SqlDbType.VarChar).Value = dpass;    // this.UserPassword.Text;
                cUserDetails = cGetUser.ExecuteReader();
                cUserDetails.Read();
                if (cUserDetails.HasRows == true)
                {
                  //  MessageBox.Show("You are inside 1!!");
                    string finUserID = Convert.ToString(cUserDetails["oprcode"]).Trim().ToUpper();
                    string finUserPASS = Convert.ToString(cUserDetails["userpassword"]).Trim().ToUpper();
                    //  int nmodule = 8;// cUserDetails.GetInt32(2);
                    int nUserNumb = Convert.ToInt16(cUserDetails["usernumb"]);
                    if (duser == finUserID && dpass == finUserPASS)
                    {
                  //      MessageBox.Show("You are inside 2!!");
                        this.Hide();
                        MainMenu mymenu = new MainMenu();
                        mymenu.Enabled = true;
                        mymenu.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        MessageBox.Show("Invalid User Details");
                        // this.UserID.Text = "";
                        this.textBox1.Text = "";
                        //   okButton.Enabled = false;
                        //  okButton.BackColor = Color.LightGray;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid User Details");
                    // this.UserID.Text = "";
                    this.textBox1.Text = "";
                  //  button1.Enabled = false;
                  //  button1.BackColor = Color.LightGray;
                    //   this.UserID.Focus();
                }
                //   nConnHandle.Close();
            }
        }

        private void FRMLock_Load(object sender, EventArgs e)
        {
            MainMenu mymenu = new MainMenu();
            mymenu.Enabled = false;
            string user = globalvar.gcUserid;
            string ala = "Only the '" + user + "' can  Unlock this instance of ICCUSoft";
            label4.Text = ala;
        }
        private string chkpassword(string dpassword)
        {
            int i = 0;
            int j = 0;
            int l = 0;
            string ncrptd = "";
            string t = "";
            int k = this.textBox1.Text.Length;
            string xpswd = this.textBox1.Text.ToUpper();        // this.Text.ToUpper();
            string ypswd = xpswd.Substring(k - 1, 1);                 //This is the last character

            for (i = 1; i < k; i++)
            {
                ypswd = ypswd + xpswd.Substring(k - (i + 1), 1);
            }
            if (ypswd.Length > 5)
            {
                t = ypswd.Substring(0, 1);
                for (j = 1; j < ypswd.Length; j++)
                {
                    if (j % 2 > 0)
                    {
                        t = t + ypswd.Substring(j, 1);
                    }
                    else
                    {
                        t = ypswd.Substring(j, 1) + t;
                    }
                }
            }
            j = t.Length % 10;
            for (i = 0; i < t.Length; i++)
            {
                int d = Convert.ToChar(t.Substring(i, 1));
                l = d + j;
                if (l > 90)
                {
                    j = (l) % 90;
                }
                ncrptd = ncrptd + Convert.ToString(l);
                j = (i + d + j) % 60;
            }
            return ncrptd;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to proceed to exit from the Application", "Application Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Application.Exit();
                //  MainMenu mymenu = new MainMenu();
                // mymenu.Close();
                //  this.Close();
                //  LoginScreen loginscreen = new LoginScreen();
                //  loginscreen.ShowDialog();
            }
            else
            {

            }
        }
    }
}
