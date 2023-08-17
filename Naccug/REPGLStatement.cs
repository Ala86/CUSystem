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
    public partial class REPGLStatement : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        public REPGLStatement()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
          //  label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
         //   comboBox1.Visible = false;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            button1.Visible = false;

            GETDATA();
            WindowState = FormWindowState.Maximized;
        }

        private void getmember(string mcode)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                DataTable ds2 = new DataTable();
                string dsq12 = "select glmast.cacctnumb,glmast.dopedate ,glmast.cacctname,glmast.nbookbal from glmast where  intcode = 1 and glmast.cacctnumb = " + "'" + textBox1.Text.Trim() + "'";
                SqlDataAdapter da2 = new SqlDataAdapter(dsq12, ndConnHandle);
                da2.Fill(ds2);

                if (ds2 != null && ds2.Rows.Count > 0)
                {
                    textBox2.Text = ds2.Rows[0]["cacctname"].ToString();
                }
                else
                {
                    MessageBox.Show("Account not found or May have a Balance ");
                    textBox1.Text = "";
                }
            }
        }
        public void GETDATA()
        {
            string ncacctnumb = textBox1.Text;
           // textBox3.Text = ncacctnumb;
           // string tcCode = textBox1.Text.ToString().Trim().PadLeft(6, '0');
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CryGLReport.rpt");
            SqlCommand cmd = new SqlCommand("SPGLstatement", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
         //   cmd.Parameters.AddWithValue("@CUSTCODE", tcCode);
            cmd.Parameters.AddWithValue("@cacctnumb", ncacctnumb);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FromDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ToDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            CryGLStatementDataSet ds = new CryGLStatementDataSet();
            sda.Fill(ds, "SPGLstatement");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //if (textBox1.Text.ToString().Trim() != "")
            //{
            //    using (SqlConnection ndConnHandle = new SqlConnection(cs))
            //    {
            //        string tcCode = textBox1.Text.ToString().Trim().PadLeft(6, '0');
            //        ndConnHandle.Open();
            //        string dsql2 = "select ccustfname,ccustmname,ccustlname,glmast.ccustcode,glmast.cacctnumb,nbookbal from cusreg, glmast ";
            //        dsql2 += "where cusreg.ccustcode = glmast.ccustcode and cusreg.ccustcode = " + "'" + tcCode + "'";
            //        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
            //        DataTable ds2 = new DataTable();
            //        da2.Fill(ds2);
            //        if (ds2 != null)
            //        {
            //            comboBox1.DataSource = ds2.DefaultView;
            //            comboBox1.DisplayMember = "cacctnumb";
            //            comboBox1.ValueMember = "cacctnumb";
            //            comboBox1.SelectedIndex = -1;
            //            textBox2.Text = ds2.Rows[0]["ccustfname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim();
            //        }
            //    }
            //}
        }

        private void REPGLStatement_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            getmember(textBox1.Text.Trim());
        }
    }
}
