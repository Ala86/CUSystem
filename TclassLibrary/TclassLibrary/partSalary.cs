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
    public partial class partSalary : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable staffview = new DataTable();
        DataTable filtview = new DataTable();
        DataTable empPatview = new DataTable();

        public partSalary(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void partSalary_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Part Salary >>";
            getstaff();
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
                //textBox12.Text = Convert.ToDecimal(staffview.Rows[ClientGrid.CurrentCell.RowIndex]["nbasic"]).ToString("N2");
                string lcperiod = DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + DateTime.Today.Year.ToString().Trim().PadLeft(4, '0');
                //textBox2.Text = nOvt50.ToString("N2");
                //textBox13.Text = nOvt100.ToString("N2");
                textBox53.Text = tcemp;
                empPatDetails(tcemp, lcperiod);
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

            if (textBox1.Text.ToString().Trim() != "" || textBox2.Text.ToString().Trim() != "")
            {
                patSaveButton.Enabled = true;
                patSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                patSaveButton.Enabled = false;
                patSaveButton.BackColor = Color.Gainsboro;
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
                    empPatDetails(tcemp, lcperiod);
                }
                ndConnHandle.Close();
            }
        }

        private void empPatDetails(string tcStaffNo, string tcperiod)
        {
            empPatview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                int nperiod = Convert.ToInt16(tcperiod.Substring(0, 2));
                string empsql = "exec tsp_getstaff_One " + ncompid + ",'" + tcStaffNo + "'";
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                DataTable empview = new DataTable();
                daemp.Fill(empview);
                if (empview.Rows.Count > 0)
                {
                    textBox4.Text = !Convert.IsDBNull(empview.Rows[0]["nbasic"]) ? Convert.ToDecimal(empview.Rows[0]["nbasic"]).ToString("N2") : "";
                    int days = DateTime.DaysInMonth(DateTime.Now.Year ,DateTime.Now.Month);
                    textBox14.Text = (Convert.ToDecimal(textBox4.Text) / days).ToString("N2");
                }

                string emptaxable = "exec tsp_getAllowance " + ncompid + ",'" + tcStaffNo +"'";// + nperiod + "," + 1;
                SqlDataAdapter datax = new SqlDataAdapter(emptaxable, ndConnHandle);
                DataTable emptaxview = new DataTable();
                datax.Fill(emptaxview);
                if (emptaxview.Rows.Count > 0)
                {
                    decimal totTaxtPay = 0.00m;
                    decimal totNonTaxtPay = 0.00m;
                    for (int i = 0; i < emptaxview.Rows.Count; i++)
                    {
                        totTaxtPay = totTaxtPay + (Convert.ToBoolean(emptaxview.Rows[i]["taxable"]) ? Convert.ToDecimal(emptaxview.Rows[i]["amount"]) : 0.00m);
                        totNonTaxtPay = totNonTaxtPay + (!Convert.ToBoolean(emptaxview.Rows[i]["taxable"]) ? Convert.ToDecimal(emptaxview.Rows[i]["amount"]) : 0.00m);
                    }
                    textBox3.Text = totTaxtPay.ToString("N2");// !Convert.IsDBNull(emptaxview.Rows[0]["amount"]) ? Convert.ToDecimal(emptaxview.Rows[0]["amount"]).ToString("N2") : "";
                }

                //string empNontaxable = "exec tsp_getAllowance " + ncompid + ",'" + tcStaffNo + "'," + nperiod + "," + 0;
                //SqlDataAdapter dantax = new SqlDataAdapter(empNontaxable, ndConnHandle);
                //DataTable empntaxview = new DataTable();
                //dantax.Fill(empntaxview);
                //if (empntaxview.Rows.Count > 0)
                //{
                //    decimal totNonTaxtPay = 0.00m;
                //    for (int k = 0; k < empntaxview.Rows.Count; k++)
                //    {
                //        totNonTaxtPay = totNonTaxtPay + Convert.ToDecimal(empntaxview.Rows[k]["amount"]);
                //    }
                //    textBox8.Text = totNonTaxtPay.ToString("N2");// !Convert.IsDBNull(empntaxview.Rows[0]["amount"]) ? Convert.ToDecimal(empntaxview.Rows[0]["amount"]).ToString("N2") : "";
                //}

                string empOt = "exec tsp_StaffOT " + ncompid + ",'" + tcStaffNo + "','" + tcperiod + "'";
                SqlDataAdapter daot = new SqlDataAdapter(empOt, ndConnHandle);
                DataTable empotview = new DataTable();
                daot.Fill(empotview);
                if (empotview.Rows.Count > 0)
                {
                    decimal totRotPay = 0.00m;
                    decimal totHotPay = 0.00m;

                    for (int j = 0; j < empotview.Rows.Count; j++)
                    {
                        totRotPay = totRotPay + Convert.ToDecimal(empotview.Rows[j]["rotpay"]);
                        totHotPay = totHotPay + Convert.ToDecimal(empotview.Rows[j]["hotpay"]);
                    }
                    textBox5.Text = (totRotPay + totHotPay).ToString("N2");
                }
                textBox12.Text = ((textBox4.Text != "" ? Convert.ToDecimal(textBox4.Text) : 0.00m) +
                                  (textBox3.Text != "" ? Convert.ToDecimal(textBox3.Text) : 0.00m) +
                                  (textBox5.Text != "" ? Convert.ToDecimal(textBox5.Text) : 0.00m)).ToString("N2");
                decimal nTaxAmt = taxCalculation.calcTax("220", Convert.ToDecimal(textBox12.Text));
                textBox10.Text = nTaxAmt.ToString("N2");

                textBox9.Text = ((textBox12.Text != "" ? Convert.ToDecimal(textBox12.Text) : 0.00m) +
                                  (textBox5.Text != "" ? Convert.ToDecimal(textBox5.Text) : 0.00m) -
                                  (textBox6.Text != "" ? Convert.ToDecimal(textBox6.Text) : 0.00m)-
                                  (textBox7.Text != "" ? Convert.ToDecimal(textBox7.Text) : 0.00m)-
                                  (textBox10.Text != "" ? Convert.ToDecimal(textBox10.Text) : 0.00m)).ToString("N2");
            }
        }        


        private void updPatDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {

                string lcperiod = DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + DateTime.Today.Year.ToString().Trim().PadLeft(4, '0');
                string cquery = "update saldet set partsal=1,nnewsal=@lnPartSal*12 where compid=@tcompid and cperiod=@tcPeriod and staffno=@tcStaffNo";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.UpdateCommand.Parameters.Add("@tstaffno", SqlDbType.VarChar).Value = textBox53.Text.ToString().Trim();
                cuscommand.UpdateCommand.Parameters.Add("@tcperiod", SqlDbType.Char).Value = lcperiod;
                cuscommand.UpdateCommand.Parameters.Add("@lnPartSal", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox15.Text); 
                cuscommand.UpdateCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle.Open();
                cuscommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Staff Part Salary details added successfully");
            }
        }

        private void initvariables()
        {
            textBox4.Text = textBox3.Text = textBox8.Text = textBox5.Text = textBox6.Text = textBox7.Text = textBox10.Text =
                textBox13.Text = textBox14.Text = textBox15.Text = textBox16.Text = textBox17.Text = textBox18.Text = textBox1.Text = textBox9.Text = "";
            patSaveButton.Enabled = false;
            patSaveButton.BackColor = Color.Gainsboro;
//            getstaff();
        }

        private void ClientGrid_Click(object sender, EventArgs e)
        {
            initvariables();
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            string lcperiod = DateTime.Today.Month.ToString().Trim().PadLeft(2, '0') + DateTime.Today.Year.ToString().Trim().PadLeft(4, '0');
            textBox53.Text = tcemp;
            empPatDetails(tcemp, lcperiod);
            page01ok();
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

        private void textBox1_Validated(object sender, EventArgs e)
        {
            if(textBox1.Text !="" )
            {
                int numdays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                if (Convert.ToInt16(textBox1.Text) <= numdays)   
            {
                    textBox15.Text = (Convert.ToDecimal(textBox1.Text) * Convert.ToDecimal(textBox14.Text)).ToString("N2");

                    textBox16.Text = ((textBox15.Text != "" ? Convert.ToDecimal(textBox15.Text) : 0.00m) +
                    (textBox3.Text != "" ? Convert.ToDecimal(textBox3.Text) : 0.00m) +
                    (textBox5.Text != "" ? Convert.ToDecimal(textBox5.Text) : 0.00m)).ToString("N2");
                    decimal nTaxAmt = taxCalculation.calcTax("220", Convert.ToDecimal(textBox16.Text));
                    textBox18.Text = nTaxAmt.ToString("N2");

                    textBox17.Text = ((textBox16.Text != "" ? Convert.ToDecimal(textBox16.Text) : 0.00m) +
                                      (textBox5.Text != "" ? Convert.ToDecimal(textBox5.Text) : 0.00m) -
                                      (textBox6.Text != "" ? Convert.ToDecimal(textBox6.Text) : 0.00m) -
                                      (textBox7.Text != "" ? Convert.ToDecimal(textBox7.Text) : 0.00m) -
                                      (textBox18.Text != "" ? Convert.ToDecimal(textBox18.Text) : 0.00m)).ToString("N2");


                }
            }
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            /*
             With Thisform
	If !Empty(This.Value)
		Store 0.00 To .text1.Value,.text2.Value,.text5.Value,.text6.Value
		Release gcStaffNo,gnRecNo
		Public gcStaffNo,gnRecNo
		gcStaffNo=This.Value
		Select staff
		Set Order To STAFFID   && STAFFID
		If Seek(gcStaffNo)
			Store .T. To .text1.Enabled,.text2.Enabled,.command2.enabled
			.text9.Value=Alltrim(firstname)+' '+Alltrim(midname)+;
				' '+Alltrim(lastname)
			gnBasic=nbasic/12
			gnRecNo=Recno()
			.text7.Value=nbasic/12
			gnHrlySal=nbasic/(52*36.5)
			gcStaffNo=staffno
			Set Order To FullName
			Thisform.otcalc
			Select otview
		Else
			Store .F. To .text1.Enabled,.text2.Enabled,.command2.enabled
			.text9.Value='No such staff found'
			.text7.Value=0
		Endif
	Endif
	.Refresh
Endwith

             */
        }

        private void button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void patSaveButton_Click(object sender, EventArgs e)
        {
            updPatDetails();
            initvariables();
            getstaff();
        }
    }
}
