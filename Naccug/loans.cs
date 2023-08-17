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

namespace WinTcare
{
    public partial class loans : Form
    {
        string dcos = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        public loans()
        {
            InitializeComponent();
        }

        private void loans_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Loan Parameters Setup >>";
            getLoanPara();
        }


        public void getLoanPara()
        {
            string dsql = "select loan_name,loan_id,int_rate,amt_cap,dur_cap,def_dur from loans order by loan_name ";
            DataTable dtable = new DataTable();
            using (SqlConnection ndConnHandle = new SqlConnection(dcos))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(dtable);
                if (dtable.Rows.Count > 0)
                {
                    tabGrid.AutoGenerateColumns = false;
                    tabGrid.DataSource = dtable.DefaultView;
                    tabGrid.Columns[0].DataPropertyName = "loan_name";
                    tabGrid.Columns[1].DataPropertyName = "int_rate";
                    tabGrid.Columns[2].DataPropertyName = "def_dur";
                    tabGrid.Columns[3].DataPropertyName = "amt_cap";
                    tabGrid.Columns[4].DataPropertyName = "dur_cap";
                    ndConnHandle.Close();
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode==Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                AllClear2Go();
            }
        }//end of onkeydown

        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            if (textBox1.Text != "" && textBox2.Text.Trim() != "" && textBox4.Text.Trim() != "")
            {
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
                saveButton.Select();
            }
            else
            {
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
            }

        }
        #endregion

        private void saveButton_Click(object sender, EventArgs e)
        {
            /*
             With Thisform
	This.Enabled = .F.
	sn=SQLExec(gnConnHandle,"Insert Into Loans (loan_name,int_rate,def_dur,amt_cap,dur_cap) values (?.text1.Value, ?.text3.value,?.TEXT2.Value,?.text6.value,?.text7.value)","loaninsert")
	If !(sn>0)
		=Messagebox("Could not insert new record",0,"Insert failure")
	Endif
	Store "" To .text1.Value,.TEXT2.Value
	Store 0.00 To  .text3.Value
	.definegrid
	.Refresh
	.text1.SetFocus
Endwith

*/     
            decimal intrate = (textBox2.Text.ToString() != "" ? Convert.ToDecimal(textBox2.Text.ToString()) : 0.00m);
            decimal amtcap = (textBox3.Text.ToString() != "" ? Convert.ToDecimal(textBox3.Text.ToString()) : 0.00m);
            int defdur = (textBox4.Text.ToString() != "" ? Convert.ToInt32(textBox4.Text.ToString()) : 0);
            int durcap = (textBox5.Text.ToString() != "" ? Convert.ToInt32(textBox5.Text.ToString()) : 0);
            //        int lconfirm = (checkBox1.Checked ? 1 : 0);
            string tcDesc = textBox1.Text.Trim().ToString();
            using (SqlConnection ndConnHandle1 = new SqlConnection(dcos))
            {
                ndConnHandle1.Open();
                string dsql = "Insert Into Loans (compid,loan_name,int_rate,def_dur,amt_cap,dur_cap) values (" + ncompid + "," + "'" + tcDesc + "'" + "," + intrate + "," + defdur + "," + amtcap + "," + durcap + ")";
                MessageBox.Show("going with " + dsql);
                SqlDataAdapter medcommand = new SqlDataAdapter();
                medcommand.InsertCommand = new SqlCommand(dsql, ndConnHandle1);
                medcommand.InsertCommand.ExecuteNonQuery();
            }
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            getLoanPara();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
