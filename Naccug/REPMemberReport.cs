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
    public partial class REPMemberReport : Form
    {
        string cs = globalvar.cos;
        string ala;
        ReportDocument rprt = new ReportDocument();
        int amale = 2;
        int afemale = 2;

        int mem_type1 = 4;
        int mem_type2 = 4;
        int mem_type3 = 4;

        int active = 2;
        int close = 2;
        public object Server { get; private set; }

        public REPMemberReport()
        {
            InitializeComponent();
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
        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            comboBox5.Visible = false;
            comboBox6.Visible = false;
            comboBox7.Visible = false;
            comboBox8.Visible = false;
            comboBox10.Visible = false;

            checkBox4.Visible = false;
            checkBox5.Visible = false;
            comboBox9.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;
            textBox8.Visible = false;
          //  textBox9.Visible = false;
            textBox10.Visible = false;
            checkBox1.Visible = false;
            checkBox2.Visible = false;
            checkBox3.Visible = false;
            //checkBox4.Visible = false;
           // checkBox5.Visible = false;
            checkBox6.Visible = false;
           checkBox8.Visible = false;
            groupBox1.Visible = false;
           // groupBox2.Visible = false;
            groupBox3.Visible = false;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            dateTimePicker3.Visible = false;
            dateTimePicker4.Visible = false;

            label10.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
            label18.Visible = false;
            label20.Visible = false;
            //label9.Visible = false;
            //label10.Visible = false;
            button1.Visible = false;

            GETDATA();
            crystalReportViewer1.MaximumSize = MaximumSize;
            WindowState = FormWindowState.Maximized;
        }

        public void GETDATA()
        {
            // int ala = Convert.ToInt32(comboBox10.SelectedValue);
            // MessageBox.Show("This is code" + mem_type2);
           // MessageBox.Show("This is code" + mem_type3);
            //  int ala = Convert.ToInt32(textBox9.Text);
            //  int ala1 = Convert.ToInt32(textBox11.Text);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPCustomerReport.rpt");
            SqlCommand cmd = new SqlCommand("SPCUSTOMEREP", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@From", textBox6.Text);
            cmd.Parameters.AddWithValue("@To", textBox5.Text);
            cmd.Parameters.AddWithValue("@CustType", mem_type1);
            cmd.Parameters.AddWithValue("@CustType1", mem_type2);
            cmd.Parameters.AddWithValue("@CustType2", mem_type3);
            cmd.Parameters.AddWithValue("@activemember", active);
            cmd.Parameters.AddWithValue("@Closemember", close);
            cmd.Parameters.AddWithValue("@GENDERMale", amale);
            cmd.Parameters.AddWithValue("@GENDERFemale", afemale);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OpenFromDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OpenToDate", SqlDbType.Date)).Value =   dateTimePicker2.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ClosedFromDate", SqlDbType.Date)).Value = dateTimePicker3.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ClosedToDate", SqlDbType.Date)).Value = dateTimePicker4.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPCUSTOMEREP ds = new SPCUSTOMEREP();
            sda.Fill(ds, "SPCUSTOMEREP");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
             int che3 = 1;
            if (checkBox6.Checked)
            {
                active = che3;
                 // textBox8.Text = che3.ToString();
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
            // string che4 = "D";
            //if (checkBox7.Checked)
            //{
            //     textBox9.Text = che4;
            //    //dateTimePicker1.Enabled = false;
            //    //dateTimePicker2.Enabled = false;
            //}
            ////else
            ////{

            //    dateTimePicker1.Enabled = true;
            //    dateTimePicker2.Enabled = true;
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int che = 1;
            if (checkBox1.Checked)
            {
                mem_type1 = che;
                //  MessageBox.Show("saving is done");
              //  textBox1.Text = che.ToString();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            int che1 = 2;
            if (checkBox2.Checked)
            {
                mem_type2 = che1;
                //  MessageBox.Show("saving is done");
              //  textBox2.Text = che1.ToString();
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                checkBox4.Checked = true;
                checkBox5.Checked = true;

                checkBox4.Enabled = false;
                checkBox5.Enabled = false;
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
                mem_type3 = che2;
                //  MessageBox.Show("saving is done");
              //  textBox3.Text = che2.ToString();
                textBox5.Enabled = false;
                textBox6.Enabled = false;

                checkBox4.Checked = true;
                checkBox5.Checked = true;

                checkBox4.Enabled = false;
                checkBox5.Enabled = false;
            }
            else
            {

                textBox5.Enabled = true;
                textBox6.Enabled = true;
                //checkBox4.Enabled = true;
                //checkBox5.Enabled = true;
            }
        }

        private void REPMemberReport_Load(object sender, EventArgs e)
        {
            BranchCombo();
            comtrib();
            comblvelPoverty();
            combleveledu();
            comgender();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();

               
                //************Getting country
                string dsqcl0 = "exec tsp_GetCountry  ";
                SqlDataAdapter dac0 = new SqlDataAdapter(dsqcl0, ndConnHandle);
                DataSet dsc0 = new DataSet();
                DataTable corCoun = new DataTable();

                dac0.Fill(dsc0);
                dac0.Fill(corCoun);

                if (dsc0 != null)
                {
                    comboBox3.DataSource = dsc0.Tables[0];
                    comboBox3.DisplayMember = "cou_name";
                    comboBox3.ValueMember = "cou_id";
                    comboBox3.SelectedIndex = -1;

               }

              
                //************Getting counties/regions
                string dsql2 = "select coun_name,coun_id from county order by coun_name ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                DataTable ds2c = new DataTable();
                da2.Fill(ds2);
                da2.Fill(ds2c);
                if (ds2 != null)
                {
                    comboBox2.DataSource = ds2.DefaultView;
                    comboBox2.DisplayMember = "coun_name";
                    comboBox2.ValueMember = "coun_id";
                    comboBox2.SelectedIndex = -1;

                }
                //                else { MessageBox.Show("Could not get regions , inform IT Dept immediately"); }


                //= opendbf('area', 'area_id')
                string dsql3 = "select area_name,area_id from area order by area_name ";
                SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle);
                DataTable ds3 = new DataTable();
                DataTable ds3c = new DataTable();
                da3.Fill(ds3);
                da3.Fill(ds3c);
                if (ds3 != null)
                {
                    comboBox5.DataSource = ds3.DefaultView;
                    comboBox5.DisplayMember = "area_name";
                    comboBox5.ValueMember = "area_id";
                    comboBox5.SelectedIndex = -1;
                }

              string dsql5 = "select ward_name,ward_id from ward order by ward_name ";
                SqlDataAdapter da5 = new SqlDataAdapter(dsql5, ndConnHandle);
                DataTable ds5 = new DataTable();
                DataTable ds5c = new DataTable();
                da5.Fill(ds5);
                da5.Fill(ds5c);
                if (ds5 != null)
                {
                    comboBox4.DataSource = ds5.DefaultView;
                    comboBox4.DisplayMember = "ward_name";
                    comboBox4.ValueMember = "ward_id";
                    comboBox4.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find ward Types, inform IT Dept immediately"); }

                //get marital status
                string dsql6 = "select mar_name,mar_id from marystat order by mar_name ";
                SqlDataAdapter da6 = new SqlDataAdapter(dsql6, ndConnHandle);
                DataTable ds6 = new DataTable();
                da6.Fill(ds6);
                if (ds6 != null)
                {
                    comboBox8.DataSource = ds6.DefaultView;
                    comboBox8.DisplayMember = "mar_name";
                    comboBox8.ValueMember = "mar_id";
                    comboBox8.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find marital status, inform IT Dept immediately"); }

                 }
        }

        private void comtrib()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql2 = "select tname,code from tribe_setup order by tname ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                DataTable ds2c = new DataTable();
                da2.Fill(ds2);
                da2.Fill(ds2c);
                if (ds2 != null)
                {
                    comboBox6.DataSource = ds2.DefaultView;
                    comboBox6.DisplayMember = "tname";
                    comboBox6.ValueMember = "code";
                    comboBox6.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find Tribes, inform IT Dept immediately"); }

            }
        }

        private void combleveledu()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql2 = "select tname,code from levelOfedu order by tname ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                DataTable ds2c = new DataTable();
                da2.Fill(ds2);
                da2.Fill(ds2c);
                if (ds2 != null)
                {
                    comboBox7.DataSource = ds2.DefaultView;
                    comboBox7.DisplayMember = "tname";
                    comboBox7.ValueMember = "code";
                    comboBox7.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find Tribes, inform IT Dept immediately"); }

            }
        }
        private void comblvelPoverty()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql2 = "select pname,code from povertylevel order by code ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                DataTable ds2c = new DataTable();
                da2.Fill(ds2);
                da2.Fill(ds2c);
                if (ds2 != null)
                {
                    comboBox9.DataSource = ds2.DefaultView;
                    comboBox9.DisplayMember = "pname";
                    comboBox9.ValueMember = "code";
                    comboBox9.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find Tribes, inform IT Dept immediately"); }

            }
        }
        private void comgender()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql2 = "select gname,code from gender order by gname ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                DataTable ds2c = new DataTable();
                da2.Fill(ds2);
                da2.Fill(ds2c);
                if (ds2 != null)
                {
                    comboBox10.DataSource = ds2.DefaultView;
                    comboBox10.DisplayMember = "gname";
                    comboBox10.ValueMember = "code";
                    comboBox10.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find Gender, inform IT Dept immediately"); }

            }
        }
       
       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            int che4 = 0;
            if (checkBox8.Checked)
            {
                close = che4;
               // textBox10.Text = che4.ToString();
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
            }
            else
            {

                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            REPMemberReport ala = new REPMemberReport();
            ala.ShowDialog();
        }

        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            this.Hide();
            REPMemberReport ala = new REPMemberReport();
            ala.ShowDialog();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            int che = 1;
            if (checkBox5.Checked)
            {
                amale = che;
                //  MessageBox.Show("saving is done");
                // textBox9.Text = che.ToString();
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            int che = 0;
            if (checkBox4.Checked)
            {
                afemale = che;
                //  MessageBox.Show("saving is done");
              //  textBox11.Text = che.ToString();
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
