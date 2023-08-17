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
    public partial class radtemplate : Form
    {
        DataTable tempview = new DataTable();
        string cs = globalvar.cos;
        int gncompid = globalvar.gnCompid;
        bool glNewTemp = true;
        public radtemplate()
        {
            InitializeComponent();
        }

        private void radtemplate_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Radiology Template Setup >>";
            getTemplates();
        }
        private void getTemplates()
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_radstandardresults  " + ncompid;
            tempview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(tempview);
                if (tempview.Rows.Count > 0)
                {
                    tempGrid.AutoGenerateColumns = false;
                    tempGrid.DataSource = tempview.DefaultView;
                    tempGrid.Columns[0].DataPropertyName = "rd_title";
                    tempGrid.Columns[1].DataPropertyName = "rd_code";
                    ndConnHandle.Close();
                    tempGrid.Focus();
                    for (int i = 0; i < 10; i++)
                    {
                        //                        DataGridViewRow drow = new  DataGridViewRow();
                        tempview.Rows.Add();
                    }
                    comboBox1.DataSource = tempview;
                    comboBox1.DisplayMember = "rd_title";
                    comboBox1.ValueMember = "rd_id";
                    comboBox1.SelectedIndex = -1;
                }
            }
        }//end of getTemplates

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
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
        }



        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            if ((textBox1.Text.ToString().Trim()!="" ||Convert.ToInt32(comboBox1.SelectedValue)>0) && tempCont.Text.ToString()!="")
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                SaveButton.Enabled = false;
                SaveButton.BackColor = Color.Gainsboro;
            }

        }
        #endregion

        private void button7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection ndconn = new SqlConnection(cs))
            {
                if(glNewTemp)
                {
                    string cpatquery = "Insert Into RadStandardResults (rd_title,rd_name,compid) values (@temptitle,@tempcont,@nCompID) ";
                    SqlDataAdapter radins = new SqlDataAdapter();
                    radins.InsertCommand = new SqlCommand(cpatquery, ndconn);
                    radins.InsertCommand.Parameters.Add("@temptitle", SqlDbType.Char).Value = textBox1.Text.ToString().Trim();
                    radins.InsertCommand.Parameters.Add("@tempcont", SqlDbType.Char).Value = tempCont.Text.ToString().Trim(); ;
                    radins.InsertCommand.Parameters.Add("@nCompID", SqlDbType.Int).Value = gncompid;
                    ndconn.Open();
                    radins.InsertCommand.ExecuteNonQuery();
                    ndconn.Close();
                }
                else
                {
                    int tempid = Convert.ToInt32(comboBox1.SelectedValue);
                    string cpatquery1 = "update RadStandardResults set rd_name = @tempcont where rd_id=@nRdID ";
                    SqlDataAdapter radupd = new SqlDataAdapter();
                    radupd.UpdateCommand = new SqlCommand(cpatquery1, ndconn);
                    radupd.UpdateCommand.Parameters.Add("@tempcont", SqlDbType.Char).Value = tempCont.Text.ToString().Trim(); ;
                    radupd.UpdateCommand.Parameters.Add("@nRdID", SqlDbType.Int).Value = tempid; 
                    ndconn.Open();
                    radupd.UpdateCommand.ExecuteNonQuery();
                    ndconn.Close();
                }
                textBox1.Text = "";
                tempCont.Text = "";
                comboBox1.SelectedIndex = -1;
                getTemplates();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            glNewTemp = true;
            comboBox1.Visible = false;
            textBox1.Visible = true;
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyData == Keys.Tab || e.KeyData==Keys.Enter)
            {
                AllClear2Go();
            }
        }

        private void comboBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab || e.KeyData == Keys.Enter)
            {
                AllClear2Go();
            }
        }

        private void tempCont_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab || e.KeyData == Keys.Enter)
            {
                AllClear2Go();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            glNewTemp = false;
            textBox1.Visible = false;
            comboBox1.Visible = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dselval = Convert.ToString(comboBox1.SelectedValue).Trim();
            if (dselval != "System.Data.DataRowView" && dselval != "")
            {
                tempCont.Text = tempview.Rows[Convert.ToInt32(comboBox1.SelectedIndex)]["rd_name"].ToString();
            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            glNewTemp = true;
            textBox1.Visible = true;
            comboBox1.Visible = false;
            textBox1.Text = "";
            tempCont.Text = string.Empty;
        }
    }
}
