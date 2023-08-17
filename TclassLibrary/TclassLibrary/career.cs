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
    public partial class career : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable staffview = new DataTable();
        DataTable carview = new DataTable();
        DataTable filtview = new DataTable();
        DataTable empCarview = new DataTable();

        public career(string tcCos, int tnCompid, string tcLoca)
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

        private void career_Load(object sender, EventArgs e)
        {
            this.Text =dloca + "<< Career Planning >>";
            getstaff();
            getCareer();
            carGrid.Columns["dfr"].DefaultCellStyle.Format = "dd/MM/yyyy";
            carGrid.Columns["dto"].DefaultCellStyle.Format = "dd/MM/yyyy";
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
                empCarDetails(tcemp);
            }
        }

        private void getCareer()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string basesql13a = "SELECT cre_id,cre_name  FROM  creason order by cre_name";
                SqlDataAdapter baseda13a = new SqlDataAdapter(basesql13a, ndConnHandle);
                baseda13a.Fill(carview);
                if (carview.Rows.Count > 0)
                {
                    comboBox1.DataSource = carview.DefaultView;
                    comboBox1.DisplayMember = "cre_name";
                    comboBox1.ValueMember = "cre_id";
                    comboBox1.SelectedIndex = -1;
                }

                string basesql10 = "select des_name,des_id from designation order by des_name";
                SqlDataAdapter baseda10 = new SqlDataAdapter(basesql10, ndConnHandle);
                DataTable desview = new DataTable();
                baseda10.Fill(desview);
                if (desview.Rows.Count > 0)
                {
                    comboBox2.DataSource = desview.DefaultView;
                    comboBox2.DisplayMember = "des_name";
                    comboBox2.ValueMember = "des_id";
                    textBox1.Text = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["desname"].ToString();
                }

                string basesql101 = "select des_name,des_id from designation order by des_name";
                SqlDataAdapter baseda101 = new SqlDataAdapter(basesql101, ndConnHandle);
                DataTable desview1 = new DataTable();
                baseda101.Fill(desview1);
                if (desview1.Rows.Count > 0)
                {
                    comboBox3.DataSource = desview1.DefaultView;
                    comboBox3.DisplayMember = "des_name";
                    comboBox3.ValueMember = "des_id";
                    comboBox3.SelectedIndex = -1;
                }


                string basesql13 = "select dep_name,dep_id from dept order by dep_name";
                SqlDataAdapter baseda13 = new SqlDataAdapter(basesql13, ndConnHandle);
                DataTable depview = new DataTable();
                baseda13.Fill(depview);
                if (depview.Rows.Count > 0)
                {
                    comboBox4.DataSource = depview.DefaultView;
                    comboBox4.DisplayMember = "dep_name";
                    comboBox4.ValueMember = "dep_id";
                    comboBox4.SelectedIndex = -1;
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
            DateTime dcarFromdate = Convert.ToDateTime(carFromDate.Value.ToShortDateString());
            DateTime dcarTodate = Convert.ToDateTime(carToDate.Value.ToShortDateString());

            bool lCarOK = dcarFromdate <= dtoday && dcarTodate <=dtoday && dcarFromdate <= dcarTodate  ? true : false;

            if (comboBox1.SelectedIndex > -1 && comboBox2.SelectedIndex > -1 && comboBox3.SelectedIndex > -1 && comboBox4.SelectedIndex > -1 && 
              carDetails.Text!= "" && lCarOK )
            {
                carSaveButton.Enabled = true;
                carSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                carSaveButton.Enabled = false;
                carSaveButton.BackColor = Color.Gainsboro;
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
                    empCarDetails(tcemp);
                }
                ndConnHandle.Close();
                //string tcemp1 = staffview.Rows[0]["staffno"].ToString();
                //textBox46.Text = tcemp1;
                //textBox45.Text = staffview.Rows.Count.ToString();
            }
        }

        private void empCarDetails(string tcStaffNo)
        {
            empCarview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string empsql = "exec tsp_empCrer " + ncompid + ",'" + tcStaffNo + "'";
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                daemp.Fill(empCarview);
                if (empCarview.Rows.Count > 0)
                {
                    carGrid.AutoGenerateColumns = false;
                    carGrid.DataSource = empCarview.DefaultView;
                    carGrid.Columns[0].DataPropertyName = "dfrom";
                    carGrid.Columns[1].DataPropertyName = "dto";
                    carGrid.Columns[2].DataPropertyName = "des_name";
                    carGrid.Columns[3].DataPropertyName = "dep_name";
                }
            }
        }
        private void updCarDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                /*
	fn=SQLExec(gnConnHandle,"Insert Into career (nstaffid,dfrom,dto,des_id,cre_id,dep_id,report2,cre_dets) "+;
		"VALUES (?gnStaffID,?.text6.value,?.text5.value,?.combo1.value,?.combo2.value,?.combo3.value,?.combo4.value,?.edit1.value)","actins")

 */
                string cquery = "Insert Into career (staffno,dfrom,dto,des_id,cre_id,dep_id,report2,cre_dets,compid) ";
                cquery += "values (@tstaffno,@tdfrom,@tdto,@tdes_id,@tcre_id,@tdep_id,@treport2,@tcre_dets,@tcompid)";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle);
                cuscommand.InsertCommand.Parameters.Add("@tstaffno", SqlDbType.VarChar).Value = textBox53.Text.ToString().Trim();
                cuscommand.InsertCommand.Parameters.Add("@tdfrom", SqlDbType.DateTime).Value = Convert.ToDateTime(carFromDate.Value);
                cuscommand.InsertCommand.Parameters.Add("@tdto", SqlDbType.DateTime).Value = Convert.ToDateTime(carToDate.Value);
                cuscommand.InsertCommand.Parameters.Add("@tdes_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox2.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@tcre_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox1.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@tdep_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox4.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@treport2", SqlDbType.Int).Value = Convert.ToInt16(comboBox3.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@tcre_dets", SqlDbType.VarChar).Value = carDetails.Text.ToString().Trim();
                cuscommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle.Open();
                cuscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Staff Career details added successfully");
            }
        }

        private void initvariables()
        {
            carFromDate.Value = carToDate.Value = DateTime.Today;
            carDetails.Text = "";
            carSaveButton.Enabled = false;
            carSaveButton.BackColor = Color.Gainsboro;
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox3.SelectedIndex = comboBox4.SelectedIndex = -1;
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

        private void carSaveButton_Click(object sender, EventArgs e)
        {
            updCarDetails();
            initvariables();
            getstaff();
        }

        private void carFromDate_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void carToDate_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.Focused)
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

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.Focused)
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

        private void carDetails_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Designation Setup", "designation", "des_name", ncompid);
            d2code.ShowDialog();
            getCareer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Department Setup", "dept", "dep_name", ncompid);
            d2code.ShowDialog();
            getCareer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "Career Reasons Setup", "creason", "cre_name");
            d1code.ShowDialog();
            getCareer();
        }

        private void ClientGrid_Click(object sender, EventArgs e)
        {
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox1.Text = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["desname"].ToString();
            textBox53.Text = tcemp;
            empCarDetails(tcemp);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
