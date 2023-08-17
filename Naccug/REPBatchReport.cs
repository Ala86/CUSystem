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
    public partial class REPBatchReport : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        public REPBatchReport()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        void BatchCombo()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            SqlCommand cmd = new SqlCommand("[SPBatchSetup]", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SPBatchSetup]";
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Columns.Add("bat_id", typeof(string));
            dt.Columns.Add("bat_name", typeof(string));
            dt.Load(reader);

            comboBox1.ValueMember = "bat_id";
            comboBox1.DisplayMember = "bat_name";
            comboBox1.DataSource = dt;
            comboBox1.SelectedIndex = -1;
            ndConnHandle1.Close();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void REPBatchReport_Load(object sender, EventArgs e)
        {
            BatchCombo();
        }
        public void GETDATA()
        {
            int ncity = Convert.ToInt32(comboBox1.SelectedValue);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CRPBatch.rpt");
            SqlCommand cmd = new SqlCommand("SPBatchRep", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@batch_id", ncity);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SPBatchSet ds = new SPBatchSet();
            sda.Fill(ds, "SPBatchRep");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            comboBox1.Visible = false;
            dateTimePicker1.Visible = false;
            groupBox1.Visible = false;
            button1.Visible = false;
            GETDATA();
            crystalReportViewer1.MaximumSize = MaximumSize;
            WindowState = FormWindowState.Maximized;
        }
    }
}
