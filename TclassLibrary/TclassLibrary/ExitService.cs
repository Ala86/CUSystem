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
    public partial class ExitService : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        int gnStaffDesig = 0;
        string dloca = string.Empty;
        string gcUserid = string.Empty;
        DataTable staffview = new DataTable();
        DataTable empExitView = new DataTable();
        DataTable filtview = new DataTable();
        DataTable xqview = new DataTable();

        public ExitService(string tcCos, int tnCompid, string tcLoca,string tcUserid)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
            gcUserid = tcUserid;
        }

        private void ExitService_Load(object sender, EventArgs e)
        {
            this.Text = dloca+ "<< Exit from Service >>";
            getExitQuestions();
            getstaff();
            getExitReasons();
        }

        private void getstaff()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                staffview.Clear();
                ndConnHandle.Open();
                string dsql = "exec tsp_getActiveStaff " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(staffview);
                ClientGrid.AutoGenerateColumns = false;
                ClientGrid.DataSource = staffview.DefaultView;
                ClientGrid.Columns[0].DataPropertyName = "staffno";
                ClientGrid.Columns[1].DataPropertyName = "fullname";
                ClientGrid.Columns[2].DataPropertyName = "depname";
                ClientGrid.Columns[3].DataPropertyName = "desname";
                ClientGrid.Columns[4].DataPropertyName = "dage";
                ClientGrid.Columns[5].DataPropertyName = "dgender";
                ndConnHandle.Close();
                textBox45.Text = staffview.Rows.Count.ToString();
                string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
                string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
                textBox53.Text = tcemp;
                empExitDetails(tcemp);
            }
        }

        private void getExitReasons()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string basesql13a = "select xit_name, xit_id from exitreasons order by xit_name";
                SqlDataAdapter baseda13a = new SqlDataAdapter(basesql13a, ndConnHandle);
                DataTable absview = new DataTable();
                baseda13a.Fill(absview);
                if (absview.Rows.Count > 0)
                {
                    comboBox2.DataSource = absview.DefaultView;
                    comboBox2.DisplayMember = "xit_name";
                    comboBox2.ValueMember = "xit_id";
                    comboBox2.SelectedIndex = -1;
                }
            }
        }

        private void getExitQuestions()
        {
            xqview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string basesql13a = "select xq_id, ltrim(rtrim(xq_name)) as xq_name, 0 as exc, 0 as gdd, 0 as fai, 0 as poo from exitquestions order by xq_name";
                SqlDataAdapter baseda13a = new SqlDataAdapter(basesql13a, ndConnHandle);
                baseda13a.Fill(xqview);
                if (xqview.Rows.Count > 0)
                {
                    exitGrid.AutoGenerateColumns = false;
                    exitGrid.DataSource = xqview.DefaultView;
                    exitGrid.Columns[0].DataPropertyName = "xq_name";
                    exitGrid.Columns[1].DataPropertyName = "exc";
                    exitGrid.Columns[2].DataPropertyName = "gdd";
                    exitGrid.Columns[3].DataPropertyName = "fai";
                    exitGrid.Columns[3].DataPropertyName = "poo";
                }
           //     else { MessageBox.Show("no questions found"); }
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
            bool lExtOK = exitDate.Value <= DateTime.Today ? true : false;

            if (comboBox2.SelectedIndex > -1 && exitDetails.Text.ToString().Trim() != "" && lExtOK)
            {
                extSaveButton.Enabled = true;
                extSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                extSaveButton.Enabled = false;
                extSaveButton.BackColor = Color.Gainsboro;
            }
        }

        private void empExitDetails(string tcStaffNo)
        {
            empExitView.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string empsql = "select ltrim(rtrim(xq_name)) as xq_name, exc, gdd, fai, poo, xitintv.xq_id from xitintv,exitquestions where xitintv.xq_id = exitquestions.xq_id and cstaffno = '" + tcStaffNo + "' order by xq_name";
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                daemp.Fill(empExitView);
                if (empExitView.Rows.Count > 0)
                {
                    exitGrid.AutoGenerateColumns = false;
                    exitGrid.DataSource = empExitView.DefaultView;
                    exitGrid.Columns[0].DataPropertyName = "xq_name";
                    exitGrid.Columns[1].DataPropertyName = "exc";
                    exitGrid.Columns[2].DataPropertyName = "gdd";
                    exitGrid.Columns[3].DataPropertyName = "fai";
                    exitGrid.Columns[4].DataPropertyName = "poo";
                }
                else
                {
                    getExitQuestions();
                }
            }
        }


        private void getfiltview(int tnfiltype)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                switch (tnfiltype)
                {
                    case 1: //branch 
                        filtview.Clear();
                        string basesql130 = "select br_name,branchid from branch where compobnk=1 order by br_name";
                        SqlDataAdapter baseda130 = new SqlDataAdapter(basesql130, ndConnHandle);
                        baseda130.Fill(filtview);
                        if (filtview.Rows.Count > 0)
                        {
                            comboBox17.DataSource = filtview.DefaultView;
                            comboBox17.DisplayMember = "br_name";
                            comboBox17.ValueMember = "branchid";
                            comboBox17.SelectedIndex = -1;
                        }
                        break;
                    case 2: //department 
                        filtview.Clear();
                        string basesql131 = "select dep_name,dep_id from dept order by dep_name";
                        SqlDataAdapter baseda131 = new SqlDataAdapter(basesql131, ndConnHandle);
                        baseda131.Fill(filtview);
                        if (filtview.Rows.Count > 0)
                        {
                            comboBox17.DataSource = filtview.DefaultView;
                            comboBox17.DisplayMember = "dep_name";
                            comboBox17.ValueMember = "dep_id";
                            comboBox17.SelectedIndex = -1;
                        }
                        break;
                    case 3: //designation
                        filtview.Clear();
                        string desigsql = "select des_name,des_id from designation order by des_name";
                        SqlDataAdapter dadesign = new SqlDataAdapter(desigsql, ndConnHandle);
                        dadesign.Fill(filtview);
                        if (filtview.Rows.Count > 0)
                        {
                            comboBox17.DataSource = filtview.DefaultView;
                            comboBox17.DisplayMember = "des_name";
                            comboBox17.ValueMember = "des_id";
                            comboBox17.SelectedIndex = -1;
                        }
                        break;
                    case 4: //band
                        filtview.Clear();
                        string bandsql = "select ban_name,ban_id from band order by ban_name";
                        SqlDataAdapter daband = new SqlDataAdapter(bandsql, ndConnHandle);
                        daband.Fill(filtview);
                        if (filtview.Rows.Count > 0)
                        {
                            comboBox17.DataSource = filtview.DefaultView;
                            comboBox17.DisplayMember = "ban_name";
                            comboBox17.ValueMember = "ban_id";
                            comboBox17.SelectedIndex = -1;
                        }
                        break;
                    case 5: //ethnicity 
                        filtview.Clear();
                        string ethsql = "select eth_name,eth_id from ethnies order by eth_name";
                        SqlDataAdapter daeth = new SqlDataAdapter(ethsql, ndConnHandle);
                        daeth.Fill(filtview);
                        if (filtview.Rows.Count > 0)
                        {
                            comboBox17.DataSource = filtview.DefaultView;
                            comboBox17.DisplayMember = "eth_name";
                            comboBox17.ValueMember = "eth_id";
                            comboBox17.SelectedIndex = -1;
                        }
                        break;
                    case 6: //cost centre 
                        filtview.Clear();
                        string cosql = "select cos_name,cos_id from costcent order by cos_name";
                        SqlDataAdapter dacos = new SqlDataAdapter(cosql, ndConnHandle);
                        dacos.Fill(filtview);
                        if (filtview.Rows.Count > 0)
                        {
                            comboBox17.DataSource = filtview.DefaultView;
                            comboBox17.DisplayMember = "cos_name";
                            comboBox17.ValueMember = "cos_id";
                            comboBox17.SelectedIndex = -1;
                        }
                        break;
                }
            }
        }


        private void getstaffByFilter(int tnFilter)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                staffview.Clear();
                ndConnHandle.Open();
                switch (tnFilter)
                {
                    case 0:     // all staff
                        string dsql0 = "exec tsp_getActiveStaff " + ncompid;
                        SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndConnHandle);
                        da0.Fill(staffview);
                        break;
                    case 1: //by branch
                        string dsql1 = "exec tsp_getActiveStaffByBranch " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                        da1.Fill(staffview);
                        break;
                    case 2: //by department
                        string dsql2 = "exec tsp_getActiveStaffByDept " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                        da2.Fill(staffview);
                        break;
                    case 3:     // designation
                        string dsql3 = "exec tsp_getActiveStaffByDesig " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle);
                        da3.Fill(staffview);
                        break;
                    case 4: //band
                        string dsql4 = "exec tsp_getActiveStaffByBand " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da4 = new SqlDataAdapter(dsql4, ndConnHandle);
                        da4.Fill(staffview);
                        break;
                    case 5: //by ethnicity
                        string dsql5 = "exec tsp_getActiveStaffByEth " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da5 = new SqlDataAdapter(dsql5, ndConnHandle);
                        da5.Fill(staffview);
                        break;
                    case 6:     // cost centre
                        string dsql6 = "exec tsp_getActiveStaffByCoscen " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da6 = new SqlDataAdapter(dsql6, ndConnHandle);
                        da6.Fill(staffview);
                        break;
                    case 10: //female staff
                        string dsql10 = "exec tsp_getActiveStaffFemale " + ncompid;
                        SqlDataAdapter da10 = new SqlDataAdapter(dsql10, ndConnHandle);
                        da10.Fill(staffview);
                        break;
                    case 11: //by department
                        string dsql11 = "exec tsp_getActiveStaffMale " + ncompid;
                        SqlDataAdapter da11 = new SqlDataAdapter(dsql11, ndConnHandle);
                        da11.Fill(staffview);
                        break;
                }
                if (staffview.Rows.Count > 0)
                {
                    ClientGrid.AutoGenerateColumns = false;
                    ClientGrid.DataSource = staffview.DefaultView;
                    ClientGrid.Columns[0].DataPropertyName = "staffno";
                    ClientGrid.Columns[1].DataPropertyName = "fullname";
                    ClientGrid.Columns[2].DataPropertyName = "depname";
                    ClientGrid.Columns[3].DataPropertyName = "desname";
                    ClientGrid.Columns[4].DataPropertyName = "dage";
                    ClientGrid.Columns[5].DataPropertyName = "dgender";
                    string tcemp1 = staffview.Rows[0]["staffno"].ToString();
                    //                    textBox46.Text = tcemp1;
                    textBox45.Text = staffview.Rows.Count.ToString();
                    string tcemp = staffview.Rows[0]["staffno"].ToString();
                    string tcempname = staffview.Rows[0]["fullname"].ToString();
                    textBox53.Text = tcemp;
                    empExitDetails(tcemp);
                }
                ndConnHandle.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            getstaff();
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton14.Checked)
            {
                getfiltview(1);
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked)
            {
                getfiltview(2);
            }
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton13.Checked)
            {
                getfiltview(3);
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked)
            {
                getfiltview(4);
            }
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton11.Checked)
            {
                getfiltview(5);
            }
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton15.Checked)
            {
                getfiltview(6);
            }
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            getstaffByFilter(10); //female staff
        }

        private void radioButton22_CheckedChanged(object sender, EventArgs e)
        {
            getstaffByFilter(11); //male staff
        }

        private void absSaveButton_Click(object sender, EventArgs e)
        {
            updExtDetails();
            initvariables();
            getstaff();
        }

        private void updExtDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {

                string cquery = "Insert Into exitserv (staffno,xit_id,xit_date,xit_det,des_id,compid)";
                cquery += "values (@tstaffno,@txit_id,@txit_date,@txit_det,@tdes_id,@tcompid)";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.InsertCommand.Parameters.Add("@tstaffno", SqlDbType.VarChar).Value = textBox53.Text.ToString().Trim();
                cuscommand.InsertCommand.Parameters.Add("@txit_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox2.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@txit_date", SqlDbType.DateTime).Value = Convert.ToDateTime(exitDate.Value);
                cuscommand.InsertCommand.Parameters.Add("@txit_det", SqlDbType.VarChar).Value = exitDetails.Text.ToString().Trim();
                cuscommand.InsertCommand.Parameters.Add("@tdes_id", SqlDbType.VarChar).Value = gnStaffDesig;
                cuscommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle.Open();
                cuscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Staff exit details added successfully");
            }
        }
        private void initvariables()
        {
            exitDetails.Text = "";
            exitDate.Value = DateTime.Now;
            comboBox2.SelectedIndex = -1;
        }

        private void ClientGrid_Click(object sender, EventArgs e)
        {
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox53.Text = tcemp;
            empExitDetails(tcemp);
        }

        private void comboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox17.Focused)
            {
                int dfilt = (radioButton8.Checked ? 0 : (radioButton14.Checked ? 1 : (radioButton9.Checked ? 2 : (radioButton13.Checked ? 3 : (radioButton10.Checked ? 4 : (radioButton11.Checked ? 5 :
                    (radioButton15.Checked ? 6 : 7)))))));
                getstaffByFilter(dfilt);
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void extSaveButton_Click(object sender, EventArgs e)
        {
            updExtDetails();
            initvariables();
            getstaff();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox2.Focused )
            {
                page01ok();
            }
        }

        private void exitDate_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void exitDetails_Validated(object sender, EventArgs e)
        {
            page01ok();
        }


        private void exitGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            exitGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void exitGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void exitGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (exitGrid.CurrentCell is DataGridViewCheckBoxCell)
            {
                bool dcellval = Convert.ToBoolean(exitGrid.CurrentCell.Value);
                int dcol = exitGrid.CurrentCell.ColumnIndex;
                int drow = exitGrid.CurrentCell.RowIndex;
                if (dcellval)
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        if (i != dcol)
                        {
                            exitGrid.Rows[drow].Cells[i].Value = false;
                        }
                    }
                } //else { MessageBox.Show("The cell is not checked"); }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow  dvrow in exitGrid.Rows)
            {
                int xq_id = Convert.ToInt16(xqview.Rows[dvrow.Index]["xq_id"]);
                string xq_string = xqview.Rows[dvrow.Index]["xq_name"].ToString().Trim();
                string tcStaff = textBox53.Text.ToString().Trim();

                bool tlexc        = !Convert.IsDBNull(dvrow.Cells["dexcel"].Value) ? Convert.ToBoolean(dvrow.Cells["dexcel"].Value) : false;
                bool tlgo         = !Convert.IsDBNull(dvrow.Cells["dgood"].Value) ? Convert.ToBoolean(dvrow.Cells["dgood"].Value) : false ;
                bool tlfa         = !Convert.IsDBNull(dvrow.Cells["dfair"].Value) ? Convert.ToBoolean(dvrow.Cells["dfair"].Value) : false;
                bool tlpo         = !Convert.IsDBNull(dvrow.Cells["dpoor"].Value) ? Convert.ToBoolean(dvrow.Cells["dpoor"].Value) : false; ;

                bool tlDup = checkDuplicates(tcStaff, xq_id);
                if(tlDup)
                {
                    updIntQuestions(tcStaff, false, xq_id,tlexc,tlgo,tlfa,tlpo);
                }
                else
                {
                    updIntQuestions(tcStaff, true, xq_id, tlexc, tlgo, tlfa, tlpo);
                }
            }
        }

        private bool checkDuplicates(string tcStaffNo, int tnXq)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string susp1 = "select 1 from  xitIntv where cstaffno = '" + tcStaffNo + "' and xq_id = " + tnXq; 
                SqlDataAdapter da21 = new SqlDataAdapter(susp1, ndConnHandle3);
                ndConnHandle3.Open();
                DataTable ds1 = new DataTable();
                da21.Fill(ds1);
                if (ds1 != null && ds1.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        private void updIntQuestions(string tcStaffNo, bool tlinsupd, int tnXqId, bool tllex, bool tllgd, bool tllfa, bool tllpo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                if(tlinsupd)   //insert new set of questions 
                {
                    string cquery = "Insert Into xitintv (xq_id,cstaffno,exc,gdd,fai,poo,cuserid,compid)";
                    cquery += "values (@txq_id,@tcstaffno,@texc,@tgdd,@tfai,@tpoo,@tcuserid,@tcompid)";

                    SqlDataAdapter cuscommand = new SqlDataAdapter();
                    cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                    cuscommand.InsertCommand.Parameters.Add("@txq_id", SqlDbType.Int).Value = tnXqId;
                    cuscommand.InsertCommand.Parameters.Add("@tcstaffno", SqlDbType.VarChar).Value = tcStaffNo;
                    cuscommand.InsertCommand.Parameters.Add("@texc", SqlDbType.Bit).Value = tllex;
                    cuscommand.InsertCommand.Parameters.Add("@tgdd", SqlDbType.Bit).Value = tllgd;
                    cuscommand.InsertCommand.Parameters.Add("@tfai", SqlDbType.Bit).Value = tllfa;
                    cuscommand.InsertCommand.Parameters.Add("@tpoo", SqlDbType.Bit).Value = tllpo;
                    cuscommand.InsertCommand.Parameters.Add("@tcuserid", SqlDbType.VarChar).Value = gcUserid;
                    cuscommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                    ndConnHandle.Open();
                    cuscommand.InsertCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                    MessageBox.Show("Staff Interview questions added successfully");
                }
                else           //update an old set of questions 
                {
                    string cquery = "update xitintv set exc=@texc,gdd=@tgdd,fai=@tfai,poo=@tpoo,cuserid=@tcuserid where  xq_id=@txq_id and cstaffno=@tcstaffno";

                    SqlDataAdapter cuscommand1 = new SqlDataAdapter();
                    cuscommand1.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                    cuscommand1.UpdateCommand.Parameters.Add("@txq_id", SqlDbType.Int).Value = tnXqId;
                    cuscommand1.UpdateCommand.Parameters.Add("@tcstaffno", SqlDbType.VarChar).Value = tcStaffNo;
                    cuscommand1.UpdateCommand.Parameters.Add("@texc", SqlDbType.Bit).Value = tllex;
                    cuscommand1.UpdateCommand.Parameters.Add("@tgdd", SqlDbType.Bit).Value = tllgd;
                    cuscommand1.UpdateCommand.Parameters.Add("@tfai", SqlDbType.Bit).Value = tllfa;
                    cuscommand1.UpdateCommand.Parameters.Add("@tpoo", SqlDbType.Bit).Value = tllpo;
                    cuscommand1.UpdateCommand.Parameters.Add("@tcuserid", SqlDbType.VarChar).Value = gcUserid;

                    ndConnHandle.Open();
                    cuscommand1.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                    MessageBox.Show("Staff Interview questions updated successfully");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Exit Reason Setup", "exitreasons", "xit_name", ncompid);
            d2code.ShowDialog();
            getExitReasons();
        }
    }
}
