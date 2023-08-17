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
    public partial class appRoval : Form
    {
        string cs = string.Empty;
        string userpass = string.Empty;
        int ncompid = 0;
        int nlimtype = 0;
        decimal nlimitamt = 0.00m;
        public bool ReturnValue = false;

        public appRoval(string tcCos,int tnCompid,int tnLimitType,decimal tnLimitAmt)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            nlimtype = tnLimitType;
            nlimitamt = tnLimitAmt;
        }

        private void appRoval_Load(object sender, EventArgs e)
        {
            this.Text =  "<< Process Authorisation Form >>";
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.ReturnValue = false;
            this.Close();
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string userpass = codedPassword.encryptPassword(cs, textBox2.Text.Trim().ToUpper());
                ndConnHandle.Open();
                string dsql1 = "exec tsp_GetAuthority " + "'"+userpass+"'" +","+ nlimtype +","+ nlimitamt; 
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                DataTable ds1 = new DataTable();
                da1.Fill(ds1);
                if(ds1.Rows.Count>0)
                {
                    if(ds1.Rows.Count >1)
                    {
                        MessageBox.Show("Shared Passwords cannot be used for Authorisation");
                        this.ReturnValue = false;
                        this.Close();
                    }
                    else
                    {
                        textBox1.Text = ds1.Rows[0]["oprcode"].ToString();
                        textBox4.Text = ds1.Rows[0]["username"].ToString();
                        SaveButton.Enabled = true;
                        SaveButton.BackColor = Color.LawnGreen;
                        SaveButton.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("User cannot authorise transaction");
                    this.ReturnValue = false;
                    this.Close();
                }
                ndConnHandle.Close();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            this.ReturnValue = true;
            this.Close();
        }
    }
}
