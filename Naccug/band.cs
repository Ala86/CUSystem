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
    public partial class band : Form
    {
        string dcos = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        public band()
        {
            InitializeComponent();
        }

        private void band_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Staff Band Setup >>";
            getBand();
        }



        public void getBand()
        {
            string dsql = "Select ban_name,nlowsal,nhighsal,nleavdays,nlowcov,nhighcov,med_Allo  from band order by ban_name";
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
                    tabGrid.Columns[0].DataPropertyName = "ban_name";
                    tabGrid.Columns[1].DataPropertyName = "nlowsal";
                    tabGrid.Columns[2].DataPropertyName = "nhighsal";
                    tabGrid.Columns[3].DataPropertyName = "nleavedays";
                    tabGrid.Columns[4].DataPropertyName = "med_allo";
                    ndConnHandle.Close();
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
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
            if (textBox1.Text != "" && textBox2.Text.Trim() != "" && textBox3.Text.Trim() != "" && textBox4.Text.Trim() != "" && textBox5.Text.Trim() != "")
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

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string tcDesc = textBox1.Text.Trim().ToString();
            decimal lowsal = (textBox2.Text.Trim().ToString()!="" ? Convert.ToDecimal(textBox2.Text.Trim().ToString()) : 0.00m);
            decimal highsal = (textBox3.Text.Trim().ToString() != "" ? Convert.ToDecimal(textBox3.Text.Trim().ToString()) : 0.00m);
            decimal livdays = (textBox4.Text.Trim().ToString() != "" ? Convert.ToInt32(textBox4.Text.Trim().ToString()) : 0);
            decimal medall= (textBox5.Text.Trim().ToString() != "" ? Convert.ToDecimal(textBox5.Text.Trim().ToString()) : 0.00m);
            using (SqlConnection ndConnHandle1 = new SqlConnection(dcos))
            {
                ndConnHandle1.Open();
                string dsql1= "Insert Into band (ban_name, nlowsal, nhighsal, nleavdays, med_allo, compid) values("+"'"+tcDesc+ "'," +lowsal+","+highsal+","+livdays+","+medall+","+ncompid + ")";
                MessageBox.Show("we will be using " + dsql1);
                SqlDataAdapter medcommand = new SqlDataAdapter();
                medcommand.InsertCommand = new SqlCommand(dsql1, ndConnHandle1);
                medcommand.InsertCommand.ExecuteNonQuery();
            }
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            getBand();
        }
    }
}
