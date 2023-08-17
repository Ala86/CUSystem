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
    public partial class REPAgeAnalysis : Form
    {
        string cs = globalvar.cos;

        ReportDocument rprt = new ReportDocument();
        int amale = 2;
        int afemale = 2;

        public EventHandler CaptureEvent { get; private set; }
        public CRPCustomerByAccount cr { get; private set; }

        public REPAgeAnalysis()
        {
            InitializeComponent();
        }
        public void GETDATA()
        {
            int nbranch = Convert.ToInt32(comboBox2.SelectedValue);
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\CryAgeAnalysis.rpt");
            SqlCommand cmd = new SqlCommand("SPAgeAnysis", ndConnHandle1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@branch_id", nbranch);
            cmd.Parameters.AddWithValue("@From", textBox6.Text);
            cmd.Parameters.AddWithValue("@To", textBox5.Text);
            cmd.Parameters.AddWithValue("@GENDER", amale);
            cmd.Parameters.AddWithValue("@GENDER1", afemale);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            AgeAnalysisDataset ds = new AgeAnalysisDataset();
            sda.Fill(ds, "SPAgeAnysis");
            rprt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rprt;
        }
        private void REPAgeAnalysis_Load(object sender, EventArgs e)
        {
            BranchCombo();
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

            comboBox2.ValueMember = "branchid";
            comboBox2.DisplayMember = "br_name";
            comboBox2.DataSource = dt;
            comboBox2.SelectedIndex = -1;
            ndConnHandle1.Close();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
         //   label2.Visible = false;
            label6.Visible = false;
            label10.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            label6.Visible = false;
            comboBox2.Visible = false;
            label1.Visible = false;
         //   dateTimePicker1.Visible = false;
            button1.Visible = false;
            checkBox4.Visible = false;
            checkBox5.Visible = false;
            label7.Visible = false;
            GETDATA();

            WindowState = FormWindowState.Maximized;
        }



        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

            //ToolStrip ts;
            //ts = (ToolStrip)crystalReportViewer1.Controls[3];

            //ToolStripButton printbutton = new ToolStripButton();
            //printbutton.Image = ts.Items[1].Image;
            //ts.Items.Remove(ts.Items[1]);
            //ts.Items.Insert(1, printbutton);

            //ts.Items[1].Click += new EventHandler(this.CaptureEvent);
            //cr = new CRPCustomerByAccount();
            //this.crystalReportViewer1.ReportSource = cr;

            //// Get the ViewerToolbar control
            //System.Web.UI.Control oControl = CrystalReportViewer1.Controls[2];
            //Button oButton = new Button();
            //oButton.ID = "newButton";
            //oButton.Text = "My New Button";

            //oControl.Controls.Add(oButton);
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
    }
}
