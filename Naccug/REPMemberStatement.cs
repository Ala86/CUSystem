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
    public partial class REPMemberStatement : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        public REPMemberStatement()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            comboBox1.Visible = false;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            button1.Visible = false;

            GETDATA();
            WindowState = FormWindowState.Maximized;
          
        }
       
        public void GETDATA()
        {
            string ncacctnumb = comboBox1.SelectedValue.ToString().Trim();
            //textBox3.Text = ncacctnumb;
            string tcCode = textBox1.Text.ToString().Trim().PadLeft(6, '0');
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPMemberStatement.rpt");
            SqlCommand cmd = new SqlCommand("SPstatement", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CUSTCODE", tcCode);
            cmd.Parameters.AddWithValue("@cacctnumb", ncacctnumb);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FromDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
          cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ToDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPMemberStatement ds = new SPMemberStatement();
            sda.Fill(ds, "SPstatement");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
         
        }

        private void REPMemberStatement_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // cacctnumbCombo();
            if (textBox1.Text.ToString().Trim() != "")
            {
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    string tcCode = textBox1.Text.ToString().Trim().PadLeft(6, '0');
                     ndConnHandle.Open();
                    string dsql2 = "select ccustfname,ccustmname,ccustlname,glmast.ccustcode,glmast.cacctnumb,nbookbal from cusreg, glmast ";
                    dsql2 += "where cusreg.ccustcode = glmast.ccustcode and cusreg.ccustcode = " + "'" + tcCode + "'";
                    SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                    DataTable ds2 = new DataTable();
                    da2.Fill(ds2);
                    if (ds2 != null)
                    {
                        comboBox1.DataSource = ds2.DefaultView;
                        comboBox1.DisplayMember = "cacctnumb";
                        comboBox1.ValueMember = "cacctnumb";
                        comboBox1.SelectedIndex = -1;
                           textBox2.Text = ds2.Rows[0]["ccustfname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim();
                      //  textBox3.Text = ds2.Rows[0]["cacctnumb"].ToString().Trim();

                    }
                }
            }
        }
        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (textBox1.Text.ToString().Trim() != "")
            {
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    
                    string tcCode = textBox1.Text.ToString().Trim().PadLeft(6, '0');
                    textBox1.Text = tcCode;
                    ndConnHandle.Open();
                    string dsql2 = "select ccustfname,ccustmname,ccustlname,glmast.ccustcode,glmast.cacctnumb,nbookbal from cusreg, glmast ";
                    dsql2 += "where cusreg.ccustcode = glmast.ccustcode and cusreg.ccustcode = " + "'" + tcCode + "'";
                    SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                    DataTable ds2 = new DataTable();
                    da2.Fill(ds2);
                    if (ds2 != null)
                    {
                       textBox2.Text = ds2.Rows[0]["ccustfname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustmname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim();
                        // comboBox1.Text = ds2.Rows[0]["cacctnumb"].ToString().Trim();
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_Validated(object sender, EventArgs e)
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
            //              comboBox1.DisplayMember = "cacctnumb";
            //             comboBox1.ValueMember = "cacctnumb";
            //             comboBox1.SelectedIndex = -1;
            //            // textBox2.Text = ds2.Rows[0]["ccustfname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim();
            //            textBox3.Text = ds2.Rows[0]["cacctnumb"].ToString().Trim();

            //        }
            //    }
            //}
        }

        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            this.Hide();
            REPMemberStatement ala = new REPMemberStatement();
            ala.ShowDialog();
        }
    }
}
