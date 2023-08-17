using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TclassLibrary

{
  public partial class chgPassword : Form
    {
        string cs1 = string.Empty;
        string duser = string.Empty;
        string dNewpassw = string.Empty;
        string dNewpasswText = string.Empty;
        string dNewpasswConfirmed = string.Empty;
        bool glLettersOnly = false;
        bool glNumbersOnly = false;
        bool glPunctuation = false;
        bool glSymbol = false;
        bool glPassWordMatch = false;

        public chgPassword(string cs, string suser)
        {
            InitializeComponent();
            cs1 = cs;
            duser = suser;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chgPassword_Load(object sender, EventArgs e)
        {
            this.Text = "<< Change password>>";
            textBox1.Text = duser;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                AllClear2Go();
            }
        }

        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            if (glPassWordMatch)

            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
                SaveButton.Focus();
            }
            else
            {
                SaveButton.Enabled = false;
                SaveButton.BackColor = Color.Gainsboro;
            }
        }
        #endregion 


        private void comparePassword(string dpassword)
        {
            using (SqlConnection nConnHandle = new SqlConnection(cs1))
            {
                nConnHandle.Open();
                string duser = textBox1.Text.Trim();

                SqlDataReader cUserDetails = null;
                SqlCommand cGetUser = new SqlCommand("exec tsp_getUser @myUserID,@myUserPASS", nConnHandle);
                cGetUser.Parameters.Add("@myUserID", SqlDbType.VarChar).Value = textBox1.Text.Trim();
                cGetUser.Parameters.Add("@myUserPASS", SqlDbType.VarChar).Value = dpassword;
                cUserDetails = cGetUser.ExecuteReader();
                cUserDetails.Read();
                if (cUserDetails.HasRows == true)
                {
                    textBox3.Enabled = textBox4.Enabled =  true;
                    textBox3.Focus();
                }
                else
                {
                    textBox3.Enabled = textBox4.Enabled = false;
                    textBox2.Text = "";
                    textBox2.Focus();
                }
            }
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            if(textBox2.Text !="")
            {
                string dpass = tclassChkpassWord.dpassWord(textBox2.Text.Trim().Trim().ToUpper());
                comparePassword(dpass);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {
                string cupdquery = "update susers set userpassword = @userpass where oprcode=@userid";
                SqlDataAdapter psupdCommand = new SqlDataAdapter();
                psupdCommand.UpdateCommand = new SqlCommand(cupdquery, ndConnHandle3);
                psupdCommand.UpdateCommand.Parameters.Add("@userid", SqlDbType.VarChar).Value = duser;
                psupdCommand.UpdateCommand.Parameters.Add("@userpass", SqlDbType.VarChar).Value = dNewpasswConfirmed;
                ndConnHandle3.Open();
                psupdCommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
            MessageBox.Show("Your password has been changed, you may now use the new password");
            this.Close();
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                dNewpassw = tclassChkpassWord.dpassWord(textBox3.Text.Trim());
                if(dNewpassw.Trim().Length<=5)
                {
                    textBox3.Text = textBox4.Text = "";
                    textBox3.Focus();
                }
            }
            AllClear2Go();
        }

        private void textBox4_Validated(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                dNewpasswConfirmed = tclassChkpassWord.dpassWord(textBox4.Text.Trim());
                if (dNewpassw != "" && dNewpasswConfirmed != "" && dNewpassw == dNewpasswConfirmed)
                {
                    glPassWordMatch = true;
                }
                else
                {
                    MessageBox.Show("New password not confirmed, please try again");
                    textBox3.Text = textBox4.Text = "";
                    glPassWordMatch = false;
                }
                AllClear2Go();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if(textBox3.Text !="")
            {
                dNewpasswText = textBox3.Text.Trim();
                passComplex(dNewpasswText);
            }
        }

        private void passComplex(string dpassw)
        {
            glLettersOnly = glNumbersOnly = glPunctuation = glSymbol = false;

            foreach (char c in dpassw)
            {
                if(char.IsLetterOrDigit(c))
                {
                    glLettersOnly = true;
                }

                if (char.IsDigit(c))
                {
                    glNumbersOnly = true;
                }

                if(char.IsPunctuation(c))
                {
                    glPunctuation = true;
                }

                if (char.IsSymbol(c))
                {
                    glSymbol = true;
                }

            }
            int passlen = dpassw.Trim().Length;
            bool glLowSec = (glLettersOnly && !glNumbersOnly) || (!glLettersOnly && glNumbersOnly) && !glPunctuation && !glSymbol ? true : false;
            bool glMedSec = (glLettersOnly && glNumbersOnly) && (!glPunctuation && !glSymbol) ? true : false;
            bool glHigSec = (glLettersOnly && glNumbersOnly) && (glPunctuation || glSymbol) && passlen <= 6 ? true : false;
            bool glMaxSec = (glLettersOnly && glNumbersOnly) && (glPunctuation || glSymbol) && passlen>6 ? true : false;

            textBox5.BackColor = (glLowSec ? Color.Green : glMedSec ? Color.Orchid : glHigSec ? Color.OrangeRed : glMaxSec ? Color.Red : Color.Gainsboro);
            textBox5.Text = glLowSec ? "Low Security " : glMedSec ? "Medium Security" : glHigSec? "High Security" : glMaxSec ? "Maximum Security" : "No Security Setup";
            textBox5.ForeColor = Color.White;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

