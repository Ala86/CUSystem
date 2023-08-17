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
    public partial class staffSubs : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable staffview = new DataTable();
        DataTable subview = new DataTable();
        DataTable filtview = new DataTable();
        DataTable empSubview = new DataTable();

        public staffSubs(string tcCos, int tnCompid, string tcLoca)
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

        private void staffSubs_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Staff Subscription >>";
            getstaff();
            getSubs();
            stYear.Minimum = DateTime.Now.Year;
            edYear.Minimum = DateTime.Now.Year;
            comboBox1.Focus();
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
                empSubDetails(tcemp);
            }
        }

        private void getSubs()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {

                string allosql = "select cont_name,cont_id from contrib order by cont_name ";
                SqlDataAdapter alloadap = new SqlDataAdapter(allosql, ndConnHandle);
                alloadap.Fill(subview);
                if (subview.Rows.Count > 0)
                {
                    comboBox1.DataSource = subview.DefaultView;
                    comboBox1.DisplayMember = "cont_name";
                    comboBox1.ValueMember = "cont_id";
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
            //            DateTime dStartDate = Convert.ToDateTime(txtStartDate.Value.ToShortDateString());
            bool tlType = (textBox8.Text != "" && textBox7.Text != "") || (textBox6.Text != "" && textBox5.Text != "") ? true : false;
        //    bool lAlloOK = dStartDate >= dtoday ? true : false;

            if (comboBox1.SelectedIndex > -1 && tlType) 
            {
                subSaveButton.Enabled = true;
                subSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                subSaveButton.Enabled = false;
                subSaveButton.BackColor = Color.Gainsboro;
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
                    empSubDetails(tcemp);
                }
                ndConnHandle.Close();
                //string tcemp1 = staffview.Rows[0]["staffno"].ToString();
                //textBox46.Text = tcemp1;
                //textBox45.Text = staffview.Rows.Count.ToString();
            }
        }

        private void empSubDetails(string tcStaffNo)
        {
            empSubview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string empsql = "exec tsp_empSubs " + ncompid + ",'" + tcStaffNo + "'";
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                daemp.Fill(empSubview);
                if (empSubview.Rows.Count > 0)
                {
                    subGrid.AutoGenerateColumns = false;
                    subGrid.DataSource = empSubview.DefaultView;
                    subGrid.Columns[0].DataPropertyName = "cont_name";
                    subGrid.Columns[1].DataPropertyName = "mployer";
                    subGrid.Columns[2].DataPropertyName = "mployee";
                    subGrid.Columns[3].DataPropertyName = "mployerpay";
                    subGrid.Columns[4].DataPropertyName = "mployeepay";
                    subGrid.Columns[5].DataPropertyName = "end_per";
                }
            }
        }
        private void updSubDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string tcBeg = stMth.Value.ToString().Trim().PadLeft(2, '0') + stYear.Value.ToString().Trim();
                string tcEnd = edMth.Value.ToString().Trim().PadLeft(2, '0') + edYear.Value.ToString().Trim();

                string cquery = "Insert Into cont_det(staffno,mployer,mployee,mployerpay,mployeepay,beg_per,end_per,cont_id,compid)  ";
                cquery += "values (@tstaffno,@tmployer,@tmployee,@tmployerpay,@tmployeepay,@tbeg_per,@tend_per,@tcont_id,@tcompid)";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.InsertCommand.Parameters.Add("@tstaffno", SqlDbType.VarChar).Value = textBox53.Text.ToString().Trim();
                cuscommand.InsertCommand.Parameters.Add("@tmployer", SqlDbType.Decimal).Value = textBox8.Text !="" ?  Convert.ToDecimal(textBox8.Text) : 0.00m;
                cuscommand.InsertCommand.Parameters.Add("@tmployee", SqlDbType.Decimal).Value = textBox7.Text !="" ?  Convert.ToDecimal(textBox7.Text) : 0.00m;
                cuscommand.InsertCommand.Parameters.Add("@tmployerpay", SqlDbType.Decimal).Value = textBox6.Text !="" ? Convert.ToDecimal(textBox6.Text) : 0.00m ;
                cuscommand.InsertCommand.Parameters.Add("@tmployeepay", SqlDbType.Decimal).Value = textBox5.Text !="" ? Convert.ToDecimal(textBox5.Text) : 0.00m;
                cuscommand.InsertCommand.Parameters.Add("@tbeg_per", SqlDbType.Char).Value = tcBeg; ;
                cuscommand.InsertCommand.Parameters.Add("@tend_per", SqlDbType.Char).Value = tcEnd;
                cuscommand.InsertCommand.Parameters.Add("@tcont_id", SqlDbType.Int).Value  = Convert.ToInt16(comboBox1.SelectedValue);
                cuscommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle.Open();
                cuscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Staff Subscription details added successfully");
            }
        }

        private void ClientGrid_Click(object sender, EventArgs e)
        {
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox53.Text = tcemp;
            empSubDetails(tcemp);
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

        private void button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void subSaveButton_Click(object sender, EventArgs e)
        {
            updSubDetails();
            initvariables();
            getstaff();
        }

        private void initvariables()
        {
            comboBox1.SelectedIndex = -1;
           textBox5.Text = textBox6.Text = textBox7.Text = textBox8.Text =  "";
            stMth.Value = edMth.Value = 1;
            stYear.Value = edYear.Value = DateTime.Now.Year;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox8_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox7_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox5_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox10_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox9_Validated(object sender, EventArgs e)
        {
            page01ok();
        }
    }
}
