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
    public partial class trn_institute : Form
    {
        string dcos = globalvar.cos;
        public trn_institute()
        {
            InitializeComponent();
        }

        private void trn_institute_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Training Institution Setup >>";
            getTrain();
            getCountry();
            comboBox1.SelectedIndex = -1;
     //       comboBox1.SelectedValue.Equals(0);
            textBox1.Focus();
        }

  private void getCountry()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(dcos))
            {
                string dsql1 = "exec tsp_GetCountry ";
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                if (ds1 != null)
                {
                    comboBox1.DataSource = ds1.Tables[0];
                    comboBox1.DisplayMember = "cou_name";
                    comboBox1.ValueMember = "cou_id";
                }
                else { MessageBox.Show("Could not get country details, inform IT Dept immediately"); }
            }
        }
        public void getTrain()
        {
            string dsql = "exec sp_TrainingInstitutionAll";
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
                    tabGrid.Columns[0].DataPropertyName = "ins_name";
                    tabGrid.Columns[1].DataPropertyName = "cou_name";
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
            if (textBox1.Text != "" && Convert.ToInt32(comboBox1.SelectedValue)>0)
            {
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
                saveButton.Select();
            }
            else
            {
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
                //*            MessageBox.Show("Invalid User or Password");
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
            int cou_id = Convert.ToInt32(comboBox1.SelectedValue);
            using (SqlConnection ndConnHandle1 = new SqlConnection(dcos))
            {
                ndConnHandle1.Open();
                string dsql1 = "Insert Into institution (ins_name,cou_id) values (" + "'" + textBox1.Text.Trim().ToString() + "'" +","+cou_id+ ")";
//                string dsql1 = "Insert Into apraisal (appr_name,maxscore,compid) values (" + "'" + textBox1.Text.Trim().ToString() + "'" + "," + textBox2.Text.Trim().ToString() + "," + ncompid + ")";
//                MessageBox.Show("we will be using " + dsql1);
                SqlDataAdapter medcommand = new SqlDataAdapter();
                medcommand.InsertCommand = new SqlCommand(dsql1, ndConnHandle1);
                medcommand.InsertCommand.ExecuteNonQuery();
            }
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            textBox1.Text = "";
            getTrain();
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
   //         getCountry();
     //       comboBox1.SelectedIndex = -1;
     //       comboBox1.SelectedValue.Equals(0); 
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
//            MessageBox.Show("The ")
//            if (textBox1.Text != "") { getCountry(); }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            MessageBox.Show("wer are leaving");
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex>-1 && comboBox1.SelectedValue.ToString()!="System.Data.DataRowView")
            {
                string dval = Convert.ToString(comboBox1.SelectedValue);
                AllClear2Go();
            }
        }

        private void comboBox1_SelectedValueChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_ValueMemberChanged(object sender, EventArgs e)
        {
//            MessageBox.Show("Value member changed");
        }
    }
}
