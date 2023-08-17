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
using CrystalDecisions.CrystalReports.Engine;

namespace WinTcare
{
    public partial class REPMemberByAccount : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        DataTable prodview = new DataTable();

        int amale = 2;
        int afemale = 2;
        //  string filepath;
        public REPMemberByAccount()
        {
            InitializeComponent();
        }
        public void GETDATA()
        {
            int nbranch = Convert.ToInt32(comboBox1.SelectedValue);
            int Pproduct = Convert.ToInt32(comboBox6.SelectedValue);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPCustomerByAccount.rpt");
            SqlCommand cmd = new SqlCommand("SPCUSTOMERACCOUNTREP", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@branch_id", nbranch);
            cmd.Parameters.AddWithValue("@Product", Pproduct);
            cmd.Parameters.AddWithValue("@From", textBox6.Text);
            cmd.Parameters.AddWithValue("@To", textBox5.Text);
            cmd.Parameters.AddWithValue("@CustType", textBox1.Text);
            cmd.Parameters.AddWithValue("@CustType1", textBox2.Text);
            cmd.Parameters.AddWithValue("@CustType2", textBox3.Text);
            cmd.Parameters.AddWithValue("@activeMember", textBox10.Text);
            cmd.Parameters.AddWithValue("@Closemember", textBox11.Text);
            cmd.Parameters.AddWithValue("@GENDER", amale);
            cmd.Parameters.AddWithValue("@GENDER1", afemale);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OpenFromDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OpenToDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ClosedFromDate", SqlDbType.Date)).Value = dateTimePicker4.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ClosedToDate", SqlDbType.Date)).Value = dateTimePicker4.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPCUSTOMERACCOUNTSET ds = new SPCUSTOMERACCOUNTSET();
            sda.Fill(ds, "SPCUSTOMERACCOUNTREP");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox9.Visible = false;
            textBox8.Visible = false;
            textBox3.Visible = false;
            textBox5.Visible = false;
            comboBox6.Visible = false;
            label13.Visible = false;
            textBox6.Visible = false;
            textBox10.Visible = false;
            textBox11.Visible = false;
            checkBox1.Visible = false;
            checkBox2.Visible = false;
            checkBox3.Visible = false;
            checkBox4.Visible = false;
            checkBox5.Visible = false;
            checkBox6.Visible = false;
            checkBox7.Visible = false;
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            dateTimePicker3.Visible = false;
            dateTimePicker4.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label6.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            button1.Visible = false;

            GETDATA();
            crystalReportViewer1.MaximumSize = MaximumSize;
            WindowState = FormWindowState.Maximized;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

            int che3 = 1;
            if (checkBox6.Checked)
            {
                textBox10.Text = che3.ToString();
                dateTimePicker3.Enabled = false;
                dateTimePicker4.Enabled = false;

            }
            else
            {

                dateTimePicker3.Enabled = true;
                dateTimePicker4.Enabled = true;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            int che4 = 0;
            if (checkBox7.Checked)
            {
                textBox11.Text = che4.ToString();
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
            }
            else
            {

                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int che = 1;
            if (checkBox1.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox1.Text = che.ToString();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            int che1 = 2;
            if (checkBox2.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox2.Text = che1.ToString();
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                //checkBox4.Enabled = false;
                //checkBox5.Enabled = false;
            }
            else
            {

                textBox5.Enabled = true;
                textBox6.Enabled = true;
                //checkBox4.Enabled = true;
                //checkBox5.Enabled = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            int che2 = 3;
            if (checkBox3.Checked)
            {
                //  MessageBox.Show("saving is done");
                textBox3.Text = che2.ToString();
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                //checkBox4.Enabled = false;
                //checkBox5.Enabled = false;
            }
            else
            {

                textBox5.Enabled = true;
                textBox6.Enabled = true;
                //checkBox4.Enabled = true;
                //checkBox5.Enabled = true;
            }
        }
        void BranchCombo()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            SqlCommand cmd = new SqlCommand("[SPbranch]", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SPbranch]";
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Columns.Add("branchid", typeof(string));
            dt.Columns.Add("br_name", typeof(string));
            dt.Load(reader);

            comboBox1.ValueMember = "branchid";
            comboBox1.DisplayMember = "br_name";
            comboBox1.DataSource = dt;
            comboBox1.SelectedIndex = -1;
            ndConnHandle1.Close();
        }
        void getProduct()
        {
            string dsqlb = "select prd_name,prd_id from prodtype order by prd_name  ";
            prodview.Clear();

            using (SqlConnection ndConnHandle2 = new SqlConnection(cs))
            {
                ndConnHandle2.Open();
                SqlDataAdapter dab = new SqlDataAdapter(dsqlb, ndConnHandle2);
                dab.Fill(prodview);
                if (prodview.Rows.Count > 0)
                {
                    comboBox6.DataSource = prodview.DefaultView;
                    comboBox6.DisplayMember = "prd_name";
                    comboBox6.ValueMember = "prd_id";
                    comboBox6.SelectedIndex = -1;
                    ndConnHandle2.Close();
                }
            }
        }//en
        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void REPMemberByAccount_Load(object sender, EventArgs e)
        {
            BranchCombo();
            getProduct();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            int che = 1;
            if (checkBox4.Checked)
            {
                amale = che;
               // textBox4.Text = che.ToString().Trim();
                //  MessageBox.Show("saving is done");
                // textBox9.Text = che.ToString();
            }


            //if (checkBox4.Checked)
            //{
            //    textBox4.Text = true.ToString();
            //}
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            int che = 0;
            if (checkBox5.Checked)
            {
                afemale = che;
                // textBox7.Text = che.ToString().Trim();
                //  MessageBox.Show("saving is done");
                //  textBox11.Text = che.ToString();
            }

            //if (checkBox5.Checked)
            //{
            //    textBox7.Text = false.ToString();
            //}
        }

        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            this.Hide();
            REPMemberByAccount ala = new REPMemberByAccount();
            ala.ShowDialog();
        }
    }
}
