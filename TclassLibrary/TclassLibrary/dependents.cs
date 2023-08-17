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
    public partial class dependents : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable staffview = new DataTable();
        DataTable empDepview = new DataTable();
        DataTable filtview = new DataTable();
        public dependents(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dependents_Load(object sender, EventArgs e)
        {
            this.Text = dloca+ "<< Dependents Setup >>";
            getstaff();
            getDependents();
            depGrid.Columns["depdob"].DefaultCellStyle.Format = "dd/MM/yyyy";
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
                empDepDetails(tcemp);
            }
        }

        private void getDependents()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {

                string noksql1 = "select id_name,idtype from id_type order by id_name";
                SqlDataAdapter nokbase1 = new SqlDataAdapter(noksql1, ndConnHandle);
                DataTable idview = new DataTable();
                nokbase1.Fill(idview);
                if (idview.Rows.Count > 0)
                {
                    comboBox2.DataSource = idview.DefaultView;
                    comboBox2.DisplayMember = "id_name";
                    comboBox2.ValueMember = "idtype";
                    comboBox2.SelectedIndex = -1;
                }

                string noksql3 = "select dep_name,dep_id from depen_tp " ;
                SqlDataAdapter nokbase3 = new SqlDataAdapter(noksql3, ndConnHandle);
                DataTable relview = new DataTable();
                nokbase3.Fill(relview);
                if (relview.Rows.Count > 0)
                {
                    comboBox1.DataSource = relview.DefaultView;
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

            DateTime dtoday = Convert.ToDateTime(DateTime.Today.ToShortDateString());

            bool lDepOK = radioButton1.Checked || radioButton2.Checked ? true : false;

            if (comboBox2.SelectedIndex > -1 && comboBox1.SelectedIndex > -1 && textBox3.Text !="" && textBox4.Text !="" && textBox5.Text !="" &&  lDepOK)
            {
                depSaveButton.Enabled = true;
                depSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                depSaveButton.Enabled = false;
                depSaveButton.BackColor = Color.Gainsboro;
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
                   textBox45.Text = staffview.Rows.Count.ToString();
                    string tcemp = staffview.Rows[0]["staffno"].ToString();
                    string tcempname = staffview.Rows[0]["fullname"].ToString();
                    textBox53.Text = tcemp;
                    empDepDetails(tcemp);
                }
                ndConnHandle.Close();
              }
        }

        private void empDepDetails(string tcStaffNo)
        {
            empDepview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string empsql = "exec tsp_StaffDepe " + ncompid + ",'" + tcStaffNo + "'";
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                daemp.Fill(empDepview);
                if (empDepview.Rows.Count > 0)
                {
                    depGrid.AutoGenerateColumns = false;
                    depGrid.DataSource = empDepview.DefaultView;
                    depGrid.Columns[0].DataPropertyName = "depname";
                    depGrid.Columns[1].DataPropertyName = "dgender";
                    depGrid.Columns[2].DataPropertyName = "rel_name";
                    depGrid.Columns[3].DataPropertyName = "dob";
                }
            }
        }
        private void updDepDetails(string tcStaffno)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                //	SN=SQLExec(gnConnHandle,"Insert Into staffdepe (staffno,d_firstname,d_midname,d_lastname,dob,relations,picturef,gender,compid,idtype,id_numb) "+;
           //     "values (?.text1.value,?.text8.Value,?.text5.Value,?.text6.value,?.text7.Value,?.combo2.value,?.image2.Picture,?.optiongroup1.value,?gnCompID,?.combo7.value,?.text17.value)","insdepe")

                string cquery = "Insert Into staffdepe (staffno,d_firstname,d_midname,d_lastname,dob,relations,gender,idtype,id_numb,compid) ";
                cquery += "values (@tstaffno,@td_firstname,@td_midname,@td_lastname,@tdob,@trelations,@tgender,@tidtype,@tid_numb,@tcompid)";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.InsertCommand.Parameters.Add("@tstaffno", SqlDbType.Char).Value = tcStaffno;
                cuscommand.InsertCommand.Parameters.Add("@td_firstname", SqlDbType.VarChar).Value = textBox3.Text.ToString().Trim() ;
                cuscommand.InsertCommand.Parameters.Add("@td_midname", SqlDbType.VarChar).Value = textBox1.Text.ToString().Trim(); ;
                cuscommand.InsertCommand.Parameters.Add("@td_lastname", SqlDbType.VarChar).Value = textBox4.Text.ToString().Trim(); ;
                cuscommand.InsertCommand.Parameters.Add("@tdob", SqlDbType.DateTime).Value = Convert.ToDateTime(dob.Value);
                cuscommand.InsertCommand.Parameters.Add("@trelations", SqlDbType.Int).Value = Convert.ToInt16(comboBox1.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@tgender", SqlDbType.Bit).Value = radioButton1.Checked ? true : false ;
                cuscommand.InsertCommand.Parameters.Add("@tidtype", SqlDbType.Int).Value =Convert.ToInt16(comboBox2.SelectedValue) ;
                cuscommand.InsertCommand.Parameters.Add("@tid_numb", SqlDbType.VarChar).Value = textBox5.Text.ToString().Trim(); ; ;
                cuscommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle.Open();
                cuscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Staff Discipline details added successfully");
            }
        }

        private void initvariables()
        {
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
            textBox3.Text = textBox4.Text = textBox5.Text = textBox1.Text = "";
            radioButton1.Checked = radioButton2.Checked = false;
            //disDate.Value = DateTime.Today;
            //disDetails.Text = "";
        }



        private void button25_Click(object sender, EventArgs e)
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

        private void comboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox17.Focused)
            {
                int dfilt = (radioButton8.Checked ? 0 : (radioButton14.Checked ? 1 : (radioButton9.Checked ? 2 : (radioButton13.Checked ? 3 : (radioButton10.Checked ? 4 : (radioButton11.Checked ? 5 :
                    (radioButton15.Checked ? 6 : 7)))))));
                getstaffByFilter(dfilt);
            }
        }

        private void ClientGrid_Click(object sender, EventArgs e)
        {
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox53.Text = tcemp;
            empDepDetails(tcemp);
        }

        private void button1_Click(object sender, EventArgs e)
        {
         twocode d2code = new twocode(cs, "Dependent Types", "depen_tp", "dep_name", ncompid);
            d2code.ShowDialog();
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox4_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            page01ok();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            page01ok();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            page01ok();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox5_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            page01ok();
        }

        private void depSaveButton_Click(object sender, EventArgs e)
        {
            updDepDetails(textBox53.Text.ToString().Trim());
            initvariables();
            getstaff();
        }
    }
}
