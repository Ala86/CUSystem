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
    public partial class bnk : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        DataTable branchview = new DataTable();
        DataTable acctview = new DataTable();
        int gnBrchID = 0;
        int gnBnkID = 0;
        public bnk()
        {
            InitializeComponent();
        }

        private void bnk_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + " << Bank Setup  >>";
            branchGrid.Columns["visdate"].HeaderCell.ToolTipText = "Double click to add account";
            getbank();
        }


        private void getbank()
        {
            using (SqlConnection dconn = new SqlConnection(cs))
            {
                //************Getting banks
                string dsql12 = "select bnk_name,bnk_id from bank order by bnk_name ";
                SqlDataAdapter da12 = new SqlDataAdapter(dsql12, dconn);
                DataTable ds12 = new DataTable();
                da12.Fill(ds12);
                if (ds12 != null)
                {
                    comboBox1.DataSource = ds12.DefaultView;
                    comboBox1.DisplayMember = "bnk_name";
                    comboBox1.ValueMember = "bnk_id";
                    comboBox1.SelectedIndex = -1;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs,"Bank setup","Bank","bnk_name",ncompid);
            d2code.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            insertbranch();
            saveBranch.Enabled = false;
            saveBranch.BackColor = Color.Gainsboro;
            branchview.Clear();
            acctview.Clear();
            getBranches(Convert.ToInt32(comboBox1.SelectedValue));
            getBnkAccts(Convert.ToInt32(comboBox1.SelectedValue));
            textBox1.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            richTextBox1.Text = "";
        }

        private void branchok()
        {
            if(textBox1.Text !="" && textBox3.Text!="" && textBox4.Text!="" && richTextBox1.Text !="" )
            {
                saveBranch.Enabled = true;
                saveBranch.BackColor = Color.LawnGreen;
            }
            else
            {
                saveBranch.Enabled = false ;
                saveBranch.BackColor = Color.Gainsboro;
            }
        }
        private void insertbranch()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = "Insert Into bnk_branch(bnk_id,bnkbr_name,addr,tel,email,compid) values (@lbnkid,@lbrancname,@laddr,@ltel,@lemail,@lcompid)";

                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@lbnkid", SqlDbType.Int).Value = Convert.ToInt32(comboBox1.SelectedValue);
                insCommand.InsertCommand.Parameters.Add("@lbrancname", SqlDbType.VarChar).Value = textBox1.Text.Trim();
                insCommand.InsertCommand.Parameters.Add("@laddr", SqlDbType.VarChar).Value = richTextBox1.Text.Trim();
                insCommand.InsertCommand.Parameters.Add("@ltel", SqlDbType.VarChar).Value = textBox3.Text.Trim();
                insCommand.InsertCommand.Parameters.Add("@lemail", SqlDbType.VarChar).Value = textBox4.Text.Trim();
                insCommand.InsertCommand.Parameters.Add("@lcompid", SqlDbType.Int).Value = globalvar.gnCompid;
                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.Focused )
            {
                branchview.Clear();
                acctview.Clear();
                getBranches(Convert.ToInt32(comboBox1.SelectedValue));
                getBnkAccts(Convert.ToInt32(comboBox1.SelectedValue));
            }
        }

        private void getBranches(int bankid)
        {
            branchview.Clear();
            string dsql1 = "exec tsp_bnk_branch  " + bankid;

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter bnkAdp = new SqlDataAdapter(dsql1, ndConnHandle);
                bnkAdp.Fill(branchview);
                if (branchview.Rows.Count > 0)
                {
                    branchGrid.AutoGenerateColumns = false;
                    branchGrid.DataSource = branchview.DefaultView;
                    branchGrid.Columns[0].DataPropertyName = "branchid";
                    branchGrid.Columns[1].DataPropertyName = "bnkbr_name";
                    branchGrid.Columns[2].DataPropertyName = "addr";
                    branchGrid.Columns[3].DataPropertyName = "tel";
                    branchGrid.Columns[4].DataPropertyName = "email";
                    ndConnHandle.Close();
                }
            }
        }

        private void getBnkAccts(int bankid)
        {
            string dsql1 = "exec tsp_bnk_accounts  " + bankid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter bnkAdp = new SqlDataAdapter(dsql1, ndConnHandle);
                bnkAdp.Fill(acctview);
                if (acctview.Rows.Count > 0)
                {
                    acctGrid.AutoGenerateColumns = false;
                    acctGrid.DataSource = acctview.DefaultView;
                    acctGrid.Columns[0].DataPropertyName = "bnkacct";
                    acctGrid.Columns[1].DataPropertyName = "cacctname";
                    acctGrid.Columns[2].DataPropertyName = "br_name";
                    acctGrid.Columns[3].DataPropertyName = "nbookbal";
                    ndConnHandle.Close();
                }
            }
        }



        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            branchok();
        }

        private void richTextBox1_Validated(object sender, EventArgs e)
        {
            branchok();
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            branchok();
        }

        private void textBox4_Validated(object sender, EventArgs e)
        {
            branchok();
        }

        private void branchGrid_DoubleClick(object sender, EventArgs e)
        {
            gnBnkID = Convert.ToInt32(comboBox1.SelectedValue);
            gnBrchID = Convert.ToInt32(branchview.Rows[branchGrid.CurrentCell.RowIndex]["branchid"]);
//            MessageBox.Show("We will add an acount for this bank"+gnBnkID);
            addAcct dact = new WinTcare.addAcct(gnBnkID,gnBrchID);
            dact.ShowDialog();
            branchview.Clear();
            acctview.Clear();
            getBranches(Convert.ToInt32(comboBox1.SelectedValue));
            getBnkAccts(Convert.ToInt32(comboBox1.SelectedValue));
        }
    }
}

