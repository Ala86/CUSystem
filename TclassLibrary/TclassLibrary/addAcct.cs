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
using TclassLibrary;

namespace WinTcare
{
    public partial class addAcct : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;

        int gnBankID = 0;
        int gnBrancid = 0;
        public addAcct(string tcCos, int tnCompid, string tcLoca, int bnkid, int branchid)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
            gnBrancid = branchid;
            gnBankID = bnkid;
//            MessageBox.Show("The branchid is " + gnBrancid);
        }


        private void addAcct_Load(object sender, EventArgs e)
        {
            this.Text = dloca + " << Bank Setup  >>";
            getbranch(gnBrancid);
            getbnkAcct();

        }

        private void getbnkAcct()
        {
            using (SqlConnection dconn = new SqlConnection(cs))
            {
                //************check for bank accounts that have not been attached
                string dsql12 = "exec tsp_bnk_accounts_new "+ncompid;
                SqlDataAdapter da12 = new SqlDataAdapter(dsql12, dconn);
                DataTable ds12 = new DataTable();
                da12.Fill(ds12);
                if (ds12 != null)
                {
                    comboBox1.DataSource = ds12.DefaultView;
                    comboBox1.DisplayMember = "cacctname";
                    comboBox1.ValueMember = "cacctnumb";
                    comboBox1.SelectedIndex = -1;
                }
            }

        }
             private void  getbranch(int branchid)
             {
                 using (SqlConnection dconn = new SqlConnection(cs))
                 {
                     string sqlstring = "select bnkbr_name,addr,tel,email  from bnk_branch where branchid = " + branchid;
                     SqlDataAdapter brch = new SqlDataAdapter(sqlstring, dconn);
                     DataTable branchview = new DataTable();
                     brch.Fill(branchview);
                     if(branchview!= null && branchview.Rows.Count>0)
                     {
                         textBox1.Text = branchview.Rows[0]["bnkbr_name"].ToString();
                         textBox2.Text = branchview.Rows[0]["addr"].ToString();
                         textBox3.Text = branchview.Rows[0]["tel"].ToString();
                         textBox4.Text = branchview.Rows[0]["email"].ToString();
                     }

                 }
             }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.Focused)
            {
                if(Convert.ToInt32(comboBox1.SelectedValue) >0)
                {
                    saveButton.Enabled = true;
                    saveButton.BackColor = Color.LawnGreen;
                }
                else
                {
                    saveButton.Enabled = false;
                    saveButton.BackColor = Color.Gainsboro;
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = "Insert Into bnk_Accounts(bnk_id,bnkacct,branchid,compid) values (@lbnkid,@lacct,@lbranch,@lcompid)";

                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@lbnkid", SqlDbType.Int).Value = gnBankID;
                insCommand.InsertCommand.Parameters.Add("@lacct", SqlDbType.VarChar).Value = comboBox1.SelectedValue.ToString().Trim();
                insCommand.InsertCommand.Parameters.Add("@lbranch", SqlDbType.Int).Value = gnBrancid;
                insCommand.InsertCommand.Parameters.Add("@lcompid", SqlDbType.Int).Value = globalvar.gnCompid;
                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();
                this.Close();
            }
        }
    }
}
