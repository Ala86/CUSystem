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
    public partial class overTime : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        decimal nOvt100 = 0.00m;
        decimal nOvt50 = 0.00m;
        int nweekshrs = 0;

        DataTable filtview = new DataTable();
        DataTable staffview = new DataTable();
        DataTable empOvtview = new DataTable();
        public overTime(string tcCos, int tnCompid, string tcLoca,decimal tnOvt100,decimal tnOvt50,int tnWeekHrs)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
            nOvt50 = tnOvt50;
            nOvt100 = tnOvt100;
            nweekshrs = tnWeekHrs;
        }

        private void overTime_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Staff Overtime >>";
            getstaff();
            ovtGrid.Columns["rpay"].SortMode = DataGridViewColumnSortMode.NotSortable;
            ovtGrid.Columns["rpay"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            ovtGrid.Columns["hpay"].SortMode = DataGridViewColumnSortMode.NotSortable;
            ovtGrid.Columns["hpay"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            ovtGrid.Columns["tpay"].SortMode = DataGridViewColumnSortMode.NotSortable;
            ovtGrid.Columns["tpay"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
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
//                textBox12.Text = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["nbasic"].ToString();
                textBox12.Text = Convert.ToDecimal(staffview.Rows[ClientGrid.CurrentCell.RowIndex]["nbasic"]).ToString("N2");
                string lcperiod = DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + DateTime.Today.Year.ToString().Trim().PadLeft(4, '0');
                textBox2.Text = nOvt50.ToString("N2");
                textBox13.Text = nOvt100.ToString("N2"); 
                textBox53.Text = tcemp;
                empOvtDetails(tcemp,lcperiod);
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

            if (textBox14.Text.ToString().Trim() != "" || textBox15.Text.ToString().Trim() !="")
            {
                textBox3.Text = textBox14.Text != "" ? (nOvt50 * Convert.ToDecimal(textBox14.Text) * ((Convert.ToDecimal(textBox12.Text) * 12) / (nweekshrs * 52))).ToString("N2") : ""; //&& using 40 hours per week
                textBox1.Text = textBox15.Text != "" ? (nOvt100 * Convert.ToDecimal(textBox15.Text) * ((Convert.ToDecimal(textBox12.Text) * 12) / (nweekshrs * 52))).ToString("N2") : ""; //&& using 40 hours per week
                textBox4.Text = (textBox14.Text != "" ? Convert.ToDecimal(textBox14.Text) : 0.00m + textBox1.Text != "" ? Convert.ToDecimal(textBox15.Text) : 0.00m).ToString("N2");
                /*
                 	If .text3.Value >0
		.text5.Value=gnOvt50*.text1.Value*((.text7.Value*12)/(gnWeekHrs*52))			&& using 40 hours per week
	Else
		.text5.Value = 0.00
	Endif

	If .text4.Value >0
		.text6.Value=gnOvt100*.text2.Value*((.text7.Value*12)/(gnWeekHrs*52))			&& using 40 hours per week
	Else
		.text6.Value = 0.00
	Endif


                 */
                ovtSaveButton.Enabled = true;
                ovtSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                ovtSaveButton.Enabled = false;
                ovtSaveButton.BackColor = Color.Gainsboro;
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
                    string lcperiod = DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + DateTime.Today.Year.ToString().Trim().PadLeft(4, '0');
                    textBox53.Text = tcemp;
                    empOvtDetails(tcemp,lcperiod);
                }
                ndConnHandle.Close();
            }
        }

        private void empOvtDetails(string tcStaffNo,string tcperiod)
        {
            empOvtview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
        //        execute tsp_staffOT '022020',30,'000037'
                string empsql = "exec tsp_staffOT '" +tcperiod+"',"+ ncompid + ",'" + tcStaffNo + "'";
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                daemp.Fill(empOvtview);
                if (empOvtview.Rows.Count > 0)
                {
                    int totRotHrs = 0;
                    int totHotHrs = 0;
                    decimal totRotPay = 0.00m;
                    decimal totHotPay = 0.00m;

                    ovtGrid.AutoGenerateColumns = false;
                    ovtGrid.DataSource = empOvtview.DefaultView;
                    ovtGrid.Columns[0].DataPropertyName = "rotpay";
                    ovtGrid.Columns[1].DataPropertyName = "hotpay";
                    ovtGrid.Columns[2].DataPropertyName = "totovt";

                    for(int j=0; j<empOvtview.Rows.Count;j++)
                    {
                        totRotHrs = totRotHrs + Convert.ToInt16(empOvtview.Rows[j]["rothrs"]);
                        totHotHrs = totHotHrs + Convert.ToInt16(empOvtview.Rows[j]["hothrs"]);

                        totRotPay = totRotPay + Convert.ToDecimal(empOvtview.Rows[j]["rotpay"]);
                        totHotPay = totHotPay + Convert.ToDecimal(empOvtview.Rows[j]["hotpay"]);
                    }
                    textBox8.Text = totRotHrs.ToString("N2");
                    textBox7.Text = totHotHrs.ToString("N2");
                    textBox10.Text = (totRotHrs + totHotHrs).ToString("N2");

                    textBox6.Text = totRotPay.ToString("N2");
                    textBox5.Text = totHotPay.ToString("N2");
                    textBox9.Text = (totRotPay + totHotPay).ToString("N2"); 
                }
            }
        }


        private void updOvtDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {

                string lcperiod = DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + DateTime.Today.Year.ToString().Trim().PadLeft(4, '0');
                int notType = (textBox14.Text != "" && textBox15.Text == "" ? 1 : (textBox14.Text == "" && textBox15.Text !="" ? 2 : 3));
                string cquery = "Insert Into ovtime (staffno,cperiod,otdate,rothrs,rotpay,hothrs,hotpay,ottype,compid) ";
                cquery += "values (@tstaffno,@tcperiod,@totdate,@trothrs,@trotpay,@thothrs,@thotpay,@tottype,@tcompid)";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.InsertCommand.Parameters.Add("@tstaffno", SqlDbType.VarChar).Value = textBox53.Text.ToString().Trim();
                cuscommand.InsertCommand.Parameters.Add("@tcperiod", SqlDbType.Char).Value = lcperiod;
                cuscommand.InsertCommand.Parameters.Add("@totdate", SqlDbType.DateTime).Value = Convert.ToDateTime(otDate.Value);
                cuscommand.InsertCommand.Parameters.Add("@trothrs", SqlDbType.Int).Value = textBox14.Text!="" ? Convert.ToInt16(textBox14.Text):0 ;
                cuscommand.InsertCommand.Parameters.Add("@trotpay", SqlDbType.Decimal).Value = textBox3.Text!=""? Convert.ToDecimal(textBox3.Text) : 0.00m;
                cuscommand.InsertCommand.Parameters.Add("@thothrs", SqlDbType.Int).Value = textBox15.Text !="" ? Convert.ToInt16(textBox15.Text) : 0 ;
                cuscommand.InsertCommand.Parameters.Add("@thotpay", SqlDbType.Decimal).Value = textBox1.Text!="" ? Convert.ToDecimal(textBox1.Text) :0.00m ;
                cuscommand.InsertCommand.Parameters.Add("@tottype", SqlDbType.Int).Value =notType ;
                cuscommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle.Open();
                cuscommand.InsertCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Staff Overtime details added successfully");
            }
        }

        private void initvariables()
        {
            textBox14.Text = textBox15.Text = textBox3.Text = textBox1.Text = textBox4.Text = textBox8.Text = textBox7.Text = textBox10.Text = 
                textBox6.Text = textBox5.Text = textBox2.Text =  textBox9.Text = "";
            ovtSaveButton.Enabled = false;
            ovtSaveButton.BackColor = Color.Gainsboro;
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

        private void comboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox17.Focused)
            {
                int dfilt = (radioButton8.Checked ? 0 : (radioButton14.Checked ? 1 : (radioButton9.Checked ? 2 : (radioButton13.Checked ? 3 : (radioButton10.Checked ? 4 : (radioButton11.Checked ? 5 :
                    (radioButton15.Checked ? 6 : 7)))))));
                getstaffByFilter(dfilt);
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

        private void button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ovtSaveButton_Click(object sender, EventArgs e)
        {
            updOvtDetails();
            initvariables();
            getstaff();
        }

        private void textBox14_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox15_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void ClientGrid_Click(object sender, EventArgs e)
        {
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox12.Text = Convert.ToDecimal(staffview.Rows[ClientGrid.CurrentCell.RowIndex]["nbasic"]).ToString("N2");
            string lcperiod = DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + DateTime.Today.Year.ToString().Trim().PadLeft(4, '0');
            textBox53.Text = tcemp;
            empOvtDetails(tcemp,lcperiod);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
