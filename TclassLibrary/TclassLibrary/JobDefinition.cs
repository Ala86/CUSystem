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

namespace TclassLibrary
{
    public partial class JobDefinition : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable staffview = new DataTable();
        DataTable filtview = new DataTable();
        DataTable empGriview = new DataTable();

        public JobDefinition(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void JobDefinition_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Job Definition >>";
            getbasics();
            getjobs();
        }

        private void getjobs()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                staffview.Clear();
                ndConnHandle.Open();
                string dsql = "exec tsp_JobDefinition " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(staffview);
                jobGrid.AutoGenerateColumns = false;
                jobGrid.DataSource = staffview.DefaultView;
                jobGrid.Columns[0].DataPropertyName = "des_name";
                jobGrid.Columns[1].DataPropertyName = "fun_name";
                jobGrid.Columns[2].DataPropertyName = "dep_name";
                jobGrid.Columns[3].DataPropertyName = "jobboss";
                //jobGrid.Columns[4].DataPropertyName = "dage";
                //jobGrid.Columns[5].DataPropertyName = "dgender";
                ndConnHandle.Close();
//                textBox45.Text = staffview.Rows.Count.ToString();
            }
        }
        private void getbasics()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {

                string basesql10 = "select des_name,des_id from designation order by des_name";
                SqlDataAdapter baseda10 = new SqlDataAdapter(basesql10, ndConnHandle);
                DataTable desview = new DataTable();
                baseda10.Fill(desview);
                if (desview.Rows.Count > 0)
                {
                    comboBox3.DataSource = desview.DefaultView;
                    comboBox3.DisplayMember = "des_name";
                    comboBox3.ValueMember = "des_id";
                    comboBox3.SelectedIndex = -1;
                }

                string basesql11 = "select fun_name,fun_id from jobfunction order by fun_name";
                SqlDataAdapter baseda11 = new SqlDataAdapter(basesql11, ndConnHandle);
                DataTable repview = new DataTable();
                baseda11.Fill(repview);
                if (repview.Rows.Count > 0)
                {
                    comboBox7.DataSource = repview.DefaultView;
                    comboBox7.DisplayMember = "fun_name";
                    comboBox7.ValueMember = "fun_id";
                    comboBox7.SelectedIndex = -1;
                }


                string basesql13 = "select dep_name,dep_id from dept order by dep_name";
                SqlDataAdapter baseda13 = new SqlDataAdapter(basesql13, ndConnHandle);
                DataTable depview = new DataTable();
                baseda13.Fill(depview);
                if (depview.Rows.Count > 0)
                {
                    comboBox1.DataSource = depview.DefaultView;
                    comboBox1.DisplayMember = "dep_name";
                    comboBox1.ValueMember = "dep_id";
                    comboBox1.SelectedIndex = -1;
                }

            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                page01ok();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                page01ok();
            }
        }

        private void page01ok() //staff basic details 
        {
            if (textBox1.Text != "" && comboBox1.SelectedIndex > -1 && comboBox3.SelectedIndex > -1 && 
                comboBox7.SelectedIndex > -1)
            {
                jobSaveButton.Enabled = true;
                jobSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                jobSaveButton.Enabled = false;
                jobSaveButton.BackColor = Color.Gainsboro;
            }
        }


        private void button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox7.Focused )
            {
                page01ok();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Focused)
            {
                page01ok();
            }
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Focused)
            {
                page01ok();
            }
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void jobSaveButton_Click(object sender, EventArgs e)
        {
            updJobDetails();
            initvariables();
            getjobs();
        }

        private void initvariables()
        {
            comboBox1.SelectedIndex = comboBox3.SelectedIndex = comboBox7.SelectedIndex =  -1;
            textBox1.Text = "";
            jobSaveButton.Enabled = false;
            jobSaveButton.BackColor = Color.Gainsboro;
        }


        private void updJobDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                /*
des_name,grade,grade_poin,dept,jobfun,compid 
                 */
                //                fn = SQLExec(gnConnHandle, "Insert Into stf_griv (nstaffid,comdate,gri_id,gri_details,act_id,compagainst,compl) " +;
                //              "VALUES (?gnStaffID,?.text5.value,?.combo1.value,?.edit1.value,?lnCompl,?.text6.value,?.check1.value)","actins")

                string cquery = "Insert Into designation (des_name,grade,grade_poin,dept,jobfun,reports2,compid) ";
                cquery += "values (@tdes_name,@tgrade,@tgrade_poin,@tdept,@tjobfun,@treports2,@tcompid) ";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.InsertCommand.Parameters.Add("@tdes_name", SqlDbType.VarChar).Value = textBox1.Text.ToString().Trim();
                cuscommand.InsertCommand.Parameters.Add("@tgrade", SqlDbType.Int).Value = dGrade.Text!="" ? Convert.ToInt16(dGrade.Text) : 0;
                cuscommand.InsertCommand.Parameters.Add("@tgrade_poin", SqlDbType.Int).Value = dGpoint.Text!="" ?  Convert.ToInt16(dGpoint.Text) : 0;
                cuscommand.InsertCommand.Parameters.Add("@tdept", SqlDbType.Int).Value = Convert.ToInt16(comboBox1.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@tjobfun", SqlDbType.Int).Value = Convert.ToInt16(comboBox1.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@treports2", SqlDbType.Int).Value = Convert.ToInt16(comboBox3.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle.Open();
                cuscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Job designation added successfully");
            }
        }

        private void dGrade_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void dGpoint_Validated(object sender, EventArgs e)
        {
            page01ok();
        }
    }
}
