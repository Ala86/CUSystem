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
    public partial class succPlan : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable jobview = new DataTable();
        DataTable staffview = new DataTable();
        DataTable incombview = new DataTable();

        public succPlan(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void succPlan_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Succession Planning ";
            dplancode.Text = DateTime.Today.Year.ToString().Trim().Substring(0,2)+DateTime.Today.Month.ToString().Trim().PadLeft(2,'0')+
               DateTime.Today.Day.ToString().Trim().PadLeft(2,'0')+ GetClient_Code.clientCode_int(cs, "succ_id").ToString().Trim().PadLeft(3, '0'); 
            getbasics();
            makeviews();
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
                    comboBox7.DataSource = desview.DefaultView;
                    comboBox7.DisplayMember = "des_name";
                    comboBox7.ValueMember = "des_id";
                    comboBox7.SelectedIndex = -1;
                }

                string[] sucPrior = new string[3];
                sucPrior[0] = "High";
                sucPrior[1] = "Medium";
                sucPrior[2] = "Low";
                comboBox6.DataSource = sucPrior;
                comboBox6.SelectedIndex = -1;

                string[] jobCrit = new string[3];
                jobCrit[0] = "High";
                jobCrit[1] = "Medium";
                jobCrit[2] = "Low";
                comboBox1.DataSource = jobCrit;
                comboBox1.SelectedIndex = -1;
            }
        }

        private void makeviews()
        {
            jobview.Columns.Add("jobid");
            jobview.Columns.Add("job_name");
            jobview.Columns.Add("job_dept");
            jobview.Columns.Add("job_brch");
            jobview.Columns.Add("job_crit");
            jobview.Columns.Add("job_prio");
            jobview.Columns.Add("job_incu");
            jobview.Columns.Add("job_reti");

            jobGrid.AutoGenerateColumns = false;
            jobGrid.DataSource = jobview.DefaultView;
            jobGrid.Columns[0].DataPropertyName = "job_name";
            jobGrid.Columns[1].DataPropertyName = "job_dept";
            jobGrid.Columns[2].DataPropertyName = "job_brch";
            jobGrid.Columns[3].DataPropertyName = "job_crit";
            jobGrid.Columns[4].DataPropertyName = "job_prio";
            jobGrid.Columns[5].DataPropertyName = "job_incu";
            jobGrid.Columns[6].DataPropertyName = "job_reti";
            jobGrid.Columns[7].DataPropertyName = "jobid";


            staffview.Columns.Add("staffno");
            staffview.Columns.Add("staff_name");
            staffview.Columns.Add("staff_job");
            staffview.Columns.Add("ready_now");
            staffview.Columns.Add("ready_two");
            staffview.Columns.Add("ready_tre");
            staffview.Columns.Add("t2retire");
            staffview.Columns.Add("selstaff");
            staffview.Columns.Add("des_id");
            staffview.Columns.Add("tcurjob");
            staffview.Columns.Add("suprexp");

            staffGrid.AutoGenerateColumns = false;
            staffGrid.DataSource = staffview.DefaultView;
            staffGrid.Columns[0].DataPropertyName = "staff_name";
            staffGrid.Columns[1].DataPropertyName = "staff_job";
            staffGrid.Columns[2].DataPropertyName = "ready_now";
            staffGrid.Columns[3].DataPropertyName = "ready_two";
            staffGrid.Columns[4].DataPropertyName = "ready_tre";
            staffGrid.Columns[5].DataPropertyName = "t2retire";
            staffGrid.Columns[6].DataPropertyName = "selstaff";
            staffGrid.Columns[7].DataPropertyName = "staffno";
            staffGrid.Columns[8].DataPropertyName = "des_id";
            staffGrid.Columns[9].DataPropertyName = "tcurjob";
            staffGrid.Columns[10].DataPropertyName = "suprexp";
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

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.Focused)
            {
                jobview.Clear();
                staffview.Clear();
                comboBox1.SelectedIndex = -1;
                comboBox6.SelectedIndex = -1;
                getIncumbent(Convert.ToInt16(comboBox7.SelectedValue));
                popJobs();
            }
        }

        private void getIncumbent(int tnJobID)
        {
            //           select des_id, staffno, ltrim(rtrim(firstname)) + ' ' + ltrim(rtrim(midname)) + ' ' + ltrim(rtrim(lastname)) as fullname  from staff where des_id = 84 order by firstname, midname, lastname
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                incombview.Clear();
                string basesql10 = "exec tsp_getJobHolders @ncompid,@jobid"; 
                SqlDataAdapter baseda10 = new SqlDataAdapter(basesql10, ndConnHandle);
                baseda10.SelectCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = ncompid;
                baseda10.SelectCommand.Parameters.Add("@jobid", SqlDbType.Int).Value = tnJobID;
                ndConnHandle.Open();
                baseda10.SelectCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                baseda10.Fill(incombview);
                if (incombview.Rows.Count > 0)
                {
                    comboBox2.DataSource = incombview.DefaultView;
                    comboBox2.DisplayMember = "fullname";
                    comboBox2.ValueMember = "staffno";
                    comboBox2.SelectedIndex = -1;
                }
            }
        }

        private void popJobs()
        {
            if (comboBox6.SelectedIndex > -1 && comboBox1.SelectedIndex > -1 && comboBox7.SelectedIndex > -1 && comboBox2.SelectedIndex > -1)
            {
                DataRow drow = jobview.NewRow();
                drow["jobid"] = comboBox7.SelectedValue;
                drow["job_name"] = comboBox7.Text.ToString().Trim();
                drow["job_dept"] = incombview.Rows[Convert.ToInt16(comboBox2.SelectedIndex)]["depname"].ToString().Trim();
                drow["job_brch"] = incombview.Rows[Convert.ToInt16(comboBox2.SelectedIndex)]["br_name"].ToString().Trim();
                drow["job_crit"] = comboBox6.Text.ToString().Trim();
                drow["job_prio"] = comboBox1.Text.ToString().Trim();
                drow["job_incu"] = comboBox2.Text.ToString().Trim();
                drow["job_reti"] = incombview.Rows[Convert.ToInt16(comboBox2.SelectedIndex)]["t2retire"].ToString().Trim();
                jobview.Rows.Add(drow);
            }
        }


        private void popStaff(string tcStaffNo)
        {
            bool exists = staffview.Select().ToList().Exists(row => row["staffno"].ToString() == tcStaffNo);  //dt.Select().ToList().Exists(row => row["Quarter"].ToString().ToUpper() == "Q9");
            if(exists )
            {
                MessageBox.Show("Talent already uploaded");
            }
            else
            {
                DataRow drow = staffview.NewRow();
                drow["staffno"] = textBox6.Text.ToString().Trim();
                drow["staff_name"] = textBox10.Text.ToString().Trim();
                drow["staff_job"] = textBox11.Text.ToString().Trim();
                drow["ready_now"] = radioButton18.Checked ? "Yes" : "";
                drow["ready_two"] = radioButton22.Checked ? "Yes" : "";
                drow["ready_tre"] = radioButton1.Checked ? "Yes" : "";
                drow["t2retire"] = textBox14.Text.ToString().Trim();
                drow["selstaff"] = true;
                drow["des_id"] = textBox17.Text.ToString().Trim(); 
                drow["tcurjob"] = textBox9.Text.ToString().Trim();
                drow["suprexp"] = textBox8.Text.ToString().Trim();

                staffview.Rows.Add(drow);
                int rnow = textBox5.Text != "" ? Convert.ToInt16(textBox5.Text) : 0;
                int rin2 = textBox12.Text != "" ? Convert.ToInt16(textBox12.Text) : 0;
                int rin3 = textBox1.Text != "" ? Convert.ToInt16(textBox1.Text) : 0;

                textBox5.Text = (rnow + 1).ToString();
                textBox12.Text = (rin2 + 1).ToString();
                textBox1.Text = (rin3 + 1).ToString();
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Focused)
            {
                popJobs();
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.Focused)
            {
                popJobs();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            using (var findform = new FindClient(cs, ncompid, dloca, 2, "staff"))
            {
                var dresult = findform.ShowDialog();
                if (dresult == DialogResult.OK)
                {
                    string dRetValue = findform.returnValue;
                    textBox6.Text = dRetValue;
                    getStaffRest(dRetValue);
                }
            }
        }

        private void getStaffRest(string tcStaffNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string basesql10 = "tsp_getActiveStaff_One @compid,@tcStaffNo";
                SqlDataAdapter baseda10 = new SqlDataAdapter(basesql10, ndConnHandle);
                baseda10.SelectCommand.Parameters.Add("@compid", SqlDbType.Int).Value = ncompid;
                baseda10.SelectCommand.Parameters.Add("@tcStaffNo", SqlDbType.Char).Value = tcStaffNo;
                ndConnHandle.Open();
                baseda10.SelectCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                DataTable desview = new DataTable();
                baseda10.Fill(desview);
                if (desview.Rows.Count > 0)
                {
                    textBox6.Text = desview.Rows[0]["staffno"].ToString().Trim();
                    textBox10.Text = desview.Rows[0]["fullname"].ToString().Trim();
                    textBox11.Text = desview.Rows[0]["desname"].ToString().Trim();
                    textBox13.Text = desview.Rows[0]["depname"].ToString().Trim();
                    textBox2.Text = desview.Rows[0]["ban_name"].ToString().Trim();
                    textBox7.Text = desview.Rows[0]["lenserv"].ToString().Trim();
                    label20.Text = Convert.ToInt16(textBox7.Text) > 1 ? "Years" : "Year";
                    textBox14.Text = desview.Rows[0]["t2retire"].ToString().Trim();
                    label30.Text = Convert.ToInt16(textBox14.Text) > 1 ? "years to retire" : "year to retire";
                    textBox17.Text = desview.Rows[0]["des_id"].ToString().Trim();
                    textBox3.Text = desview.Rows[0]["dgender"].ToString().Trim();
                    textBox4.Text = desview.Rows[0]["dage"].ToString().Trim();
                    textBox15.Text = desview.Rows[0]["dsuperv"].ToString().Trim();
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            using (var findform = new FindClient(cs, ncompid, dloca, 2, "staff"))
            {
                var dresult = findform.ShowDialog();
                if (dresult == DialogResult.OK)
                {
                    string dRetValue = findform.returnValue;
                    textBox6.Text = dRetValue;
                    getIncumbentRest(dRetValue);
                }
            }
        }

        private void getIncumbentRest(string tcStaffNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {

                string basesql10 = "select * from staff where staffno = " + tcStaffNo;
                SqlDataAdapter baseda10 = new SqlDataAdapter(basesql10, ndConnHandle);
                DataTable desview = new DataTable();
                baseda10.Fill(desview);
                if (desview.Rows.Count > 0)
                {

                }
            }
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            page01ok();
        }



        private void radioButton22_CheckedChanged(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                string dRetValue = textBox6.Text.ToString().Trim();
                getStaffRest(dRetValue);
            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            page01ok();

        }

        private void textBox9_Validated(object sender, EventArgs e)
        {
            if (textBox9.Text != "")
            {
                label32.Text = Convert.ToInt16(textBox9.Text.ToString().Trim()) > 1 ? "years" : "year";
                page01ok();
            }
        }

        private void textBox8_Validated(object sender, EventArgs e)
        {
            if (textBox8.Text != "")
            {
                label31.Text = Convert.ToInt16(textBox8.Text.ToString().Trim()) > 1 ? "years" : "year";
                page01ok();
            }
        }

        private void page01ok()
        {
            bool lready = radioButton18.Checked || radioButton22.Checked || radioButton1.Checked ? true : false;
            if (comboBox7.SelectedIndex > -1 && textBox6.Text != "" && textBox8.Text != "" && textBox9.Text != "")// && comboBox3.SelectedIndex > -1)
            {
                talSaveButton.Enabled = true;
                talSaveButton.BackColor = Color.LawnGreen;
                confirmPlan.Enabled = false;
                confirmPlan.BackColor = Color.Gainsboro;
            }
            else
            {
                talSaveButton.Enabled = false;
                talSaveButton.BackColor = Color.Gainsboro;
            }
        }

        private void ok2Confirm()
        {
            bool lready = jobGrid.Rows.Count > 0 && staffGrid.Rows.Count >0 ? true : false;
            if (lready )
            {
                confirmPlan.Enabled = true;
                confirmPlan.BackColor = Color.LawnGreen;
                talSaveButton.Enabled = false;
                talSaveButton.BackColor = Color.Gainsboro;
            }
            else
            {
                confirmPlan.Enabled = false;
                confirmPlan.BackColor = Color.Gainsboro;
            }
        }

        private void talSaveButton_Click(object sender, EventArgs e)
        {
            popStaff(textBox6.Text.ToString().Trim());
            initvariables();
            ok2Confirm();
        }



        private void updPlan()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
//               string tcSucPlan = dpla (textBox16.Text.ToString().Replace('-', ' ').Replace('=', ' ').Replace('>', ' ').Trim()).rep;
  //              MessageBox.Show("Plan code " + tcSucPlan);

                string fn = "Insert Into Succ_plan (job_tit,job_own,Job_dep,job_brch,job_prio,job_crit,compid,plan_code) ";
                fn += "VALUES (@tjob_tit,@tjob_own,@tJob_dep,@tjob_brch,@tjob_prio,@tjob_crit,@tcompid,@tplan_code)";
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(fn, ndConnHandle);

                cuscommand.InsertCommand.Parameters.Add("@tjob_tit", SqlDbType.Int).Value = Convert.ToInt16(comboBox7.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@tjob_own", SqlDbType.Char).Value = comboBox2.SelectedValue.ToString().Trim();
                cuscommand.InsertCommand.Parameters.Add("@tJob_dep", SqlDbType.Int).Value = incombview.Rows[Convert.ToInt16(comboBox2.SelectedIndex)]["dep_id"].ToString().Trim();
                cuscommand.InsertCommand.Parameters.Add("@tjob_brch", SqlDbType.Int).Value = incombview.Rows[Convert.ToInt16(comboBox2.SelectedIndex)]["br_id"].ToString().Trim();
                cuscommand.InsertCommand.Parameters.Add("@tjob_crit", SqlDbType.Int).Value = Convert.ToInt16(comboBox6.SelectedIndex);
                cuscommand.InsertCommand.Parameters.Add("@tjob_prio", SqlDbType.Int).Value = Convert.ToInt16(comboBox1.SelectedIndex);
                cuscommand.InsertCommand.Parameters.Add("@tplan_code", SqlDbType.Char).Value = dplancode.Text.ToString() ;
                cuscommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle.Open();
                cuscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Succession plan added successfully");
                updateClient_Code ucc = new updateClient_Code();
                ucc.updClient(cs, "succ_id");
            }
        }


        private int getjobID(string tcPlanCode)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string fn = "select succ_id from succ_plan where plan_code = '" + tcPlanCode + "'";
                SqlDataAdapter baseda10 = new SqlDataAdapter(fn, ndConnHandle);
                DataTable desview = new DataTable();
                baseda10.Fill(desview);
                if (desview.Rows.Count > 0)
                {
                    int nSucc_id = Convert.ToInt16(desview.Rows[0]["succ_id"]);
                    return nSucc_id;
                }
                else
                {
                    int nSucc_id1 = 0;
                    return nSucc_id1;
                }
            }
        }


        private void initvariables()
        {
            textBox6.Text = textBox10.Text = textBox11.Text = textBox13.Text = textBox15.Text = textBox2.Text = textBox7.Text = textBox4.Text =
                textBox14.Text = textBox3.Text = textBox8.Text = textBox9.Text = "";
            radioButton18.Checked = radioButton22.Checked = radioButton1.Checked = false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Focused)
            {
                popJobs();
            }
        }

        private void staffGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            staffGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void staffGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (staffGrid.Focused)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)staffGrid.Rows[e.RowIndex].Cells[6];
                DataGridViewRow drow = (DataGridViewRow)staffGrid.Rows[e.RowIndex];
                int rmRow = drow.Index;
                bool chkVal = !Convert.IsDBNull(chk.Value) ? Convert.ToBoolean(chk.Value) : false;
                if (!chkVal)
                {
                    staffview.Rows.RemoveAt(rmRow);
                }
            }
        }

        private void confirmPlan_Click(object sender, EventArgs e)
        {
            updPlan();
            int tnSucc_ID = getjobID(dplancode.Text);// textBox16.Text.ToString().Replace('-', ' ').Replace('=', ' ').Replace('>', ' ').Trim());
            foreach (DataRow drow in staffview.Rows)
            {
                string dtalent = drow["staffno"].ToString();
                int dtalDes = Convert.ToInt16(drow["des_id"]);
                int dreadywhen = (drow["ready_now"].ToString().Trim() == "Yes" ? 0 : (drow["ready_two"].ToString().Trim() =="Yes" ? 1 : 2)) ;
                int tncurjob = Convert.ToInt16(drow["tcurjob"]);
                int tnsupexp = Convert.ToInt16(drow["suprexp"]);
                updPlanDetails(tnSucc_ID,dtalent,dtalDes,dreadywhen,tncurjob,tnsupexp);
            }
            reinitvariables();
        }

        private void updPlanDetails(int tnSuID,string dtalent, int tndes,int tnready,int tnjob,int tnexp)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {

                string fn = "Insert Into Succ_plan_stf (succ_id,talent,des_id,readywhen,time_curjob,suprexp,compid) ";
                fn += "VALUES (@tsucc_id,@ttalent,@tdes_id,@treadywhen,@ttime_curjob,@tsuprexp,@tcompid)";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(fn, ndConnHandle);

                cuscommand.InsertCommand.Parameters.Add("@tsucc_id", SqlDbType.Int).Value = tnSuID;
                cuscommand.InsertCommand.Parameters.Add("@ttalent", SqlDbType.Char).Value = dtalent;
                cuscommand.InsertCommand.Parameters.Add("@tdes_id", SqlDbType.Int).Value = tndes;
                cuscommand.InsertCommand.Parameters.Add("@treadywhen", SqlDbType.Int).Value = tnready;
                cuscommand.InsertCommand.Parameters.Add("@ttime_curjob", SqlDbType.Int).Value = tnjob;
                cuscommand.InsertCommand.Parameters.Add("@tsuprexp", SqlDbType.Int).Value = tnexp;
                cuscommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle.Open();
                cuscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Succession plan Details added successfully");
            }
        }

        private void reinitvariables()
        {
            jobview.Clear();
            staffview.Clear();
            textBox6.Text = textBox10.Text = textBox11.Text = textBox13.Text = textBox15.Text = textBox2.Text = textBox7.Text = textBox4.Text =
                textBox14.Text = textBox3.Text = textBox8.Text = textBox9.Text = "";
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox6.SelectedIndex = comboBox7.SelectedIndex = -1;
            radioButton18.Checked = radioButton22.Checked = radioButton1.Checked = false;
            dplancode.Text = DateTime.Today.Year.ToString().Trim().Substring(0, 2) + DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') +
               DateTime.Today.Day.ToString().Trim().PadLeft(2, '0') + GetClient_Code.clientCode_int(cs, "succ_id").ToString().Trim().PadLeft(3, '0');
        }
    }
}
