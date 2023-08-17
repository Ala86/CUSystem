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
    public partial class FRMLoanShedule : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        public FRMLoanShedule()
        {
            InitializeComponent();
        }
        public void GETDATA(string mcode)
        {
            string ncacctnumb = comboBox1.SelectedValue.ToString().Trim();
            // string tcCode = textBox1.Text.ToString().Trim();
            string tcCode = mcode.PadLeft(6, '0');// Convert.ToString(membcode.Text).Trim().PadLeft(6, '0');
            textBox1.Text = tcCode;
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRYLoanShedule.rpt");
            SqlCommand cmd = new SqlCommand("SPLoanShedule", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ccustcode", tcCode);
            cmd.Parameters.AddWithValue("@cacctnumb", ncacctnumb);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPLoanShedule ds = new SPLoanShedule();
            sda.Fill(ds, "SPLoanShedule");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
             textBox1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            textBox2.Visible = false;
            comboBox1.Visible = false;
             button1.Visible = false;
            GETDATA(textBox1.Text.ToString());
            WindowState = FormWindowState.Maximized;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.ToString().Trim() != "")
            {
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    string tcCode = textBox1.Text.ToString().Trim().PadLeft(6, '0');
                    ndConnHandle.Open();
                    string dsql2 = "select ccustfname,ccustmname,ccustlname,glmast.ccustcode,glmast.cacctnumb,nbookbal from cusreg, glmast ";
                    dsql2 += "where cusreg.ccustcode = glmast.ccustcode and cusreg.ccustcode = " + "'" + tcCode + "' and acode in ('130','131')";
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
    }
}
