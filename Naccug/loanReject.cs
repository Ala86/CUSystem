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
    public partial class loanReject : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string dmember = string.Empty;
        string loantype = string.Empty;
        double loanamt = 0.00;
        int dloanid = 0;
        public loanReject(string member,string loanprod,double loanAmount,int loanid)
        {
            InitializeComponent();
            dmember = member;
            loantype = loanprod;
            loanamt = loanAmount;
            dloanid = loanid;
        }

        private void loanReject_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + " << Loan Rejection  >>";
            textBox1.Text = dmember;
            textBox2.Text = loantype;
            textBox3.Text = loanamt.ToString("N2");
            rejreason();
        }

        private void rejreason()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                //************Getting banks
                string dsql12 = "select rej_name,rej_id from loanreject order by rej_name ";
                SqlDataAdapter da12 = new SqlDataAdapter(dsql12, dcon);
                DataTable ds12 = new DataTable();
                da12.Fill(ds12);
                if (ds12 != null)
                {
                    comboBox1.DataSource = ds12.DefaultView;
                    comboBox1.DisplayMember = "rej_name";
                    comboBox1.ValueMember = "rej_id";
                    comboBox1.SelectedIndex = -1;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please switch Biometric reader on");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                if (MessageBox.Show("Do you want to reject the loan", "Loan Reject", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    string cglquery = "update loan_det set lreject = 1,lrejreason= @drejreason, rejectdate = convert(date,getdate()) where loan_id=@loanid";
                    SqlDataAdapter insCommand = new SqlDataAdapter();
                    insCommand.UpdateCommand = new SqlCommand(cglquery, nConnHandle2);
                    insCommand.UpdateCommand.Parameters.Add("@drejreason", SqlDbType.Int).Value = Convert.ToInt32(comboBox1.SelectedValue);
                    insCommand.UpdateCommand.Parameters.Add("@loanid", SqlDbType.Int).Value = dloanid;
                    nConnHandle2.Open();
                    insCommand.UpdateCommand.ExecuteNonQuery();
                    nConnHandle2.Close();
                    this.Close();
                }
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Focused )
            {
                saveButton.Enabled   = Convert.ToInt32(comboBox1.SelectedValue) > 0 ? true : false;
                saveButton.BackColor = Convert.ToInt32(comboBox1.SelectedValue) > 0 ? Color.LawnGreen : Color.Gainsboro; 
            }
        }
    }
}

