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

    public partial class REPauditTrail : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        public REPauditTrail()
        {
            InitializeComponent();
        }
        void AuditTrailCombo()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            SqlCommand cmd = new SqlCommand("[SPaudit_trail_Setup]", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SPaudit_trail_Setup]";
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Columns.Add("code", typeof(string));
            dt.Columns.Add("codename", typeof(string));
            dt.Load(reader);

            comboBox1.ValueMember = "code";
            comboBox1.DisplayMember = "codename";
            comboBox1.DataSource = dt;
            comboBox1.SelectedIndex = -1;
            ndConnHandle1.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            label1.Visible = false;
            label20.Visible = false;
            label14.Visible = false;
           // label6.Visible = false;
            //textBox1.Visible = false;
            comboBox1.Visible = false;
           // comboBox2.Visible = false;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            button1.Visible = false;

            GETDATA();
            WindowState = FormWindowState.Maximized;
        }
        public void GETDATA()
        {
            // int nbranch = Convert.ToInt32(comboBox1.SelectedValue);
            // string tcCode = textBox1.Text.ToString().Trim();
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\Cryaudittrailrpt.rpt");
            SqlCommand cmd = new SqlCommand("SPauditTrial", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@audit_type", comboBox1.Text);
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FromDate", SqlDbType.Date)).Value = dateTimePicker1.Text;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ToDate", SqlDbType.Date)).Value = dateTimePicker2.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPauditTrialDataset ds = new SPauditTrialDataset();
            sda.Fill(ds, "SPauditTrial");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
        }

        private void REPauditTrail_Load(object sender, EventArgs e)
        {
            AuditTrailCombo();
        }
    }
}
