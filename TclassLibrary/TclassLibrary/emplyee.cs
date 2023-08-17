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
    public partial class emplyee : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string cLoca = string.Empty;
        string gcInputstat = "N";
        DataTable staffview = new DataTable();
        DataTable filtview = new DataTable();
        DataTable nokcityview = new DataTable();
        DataTable cityview = new DataTable();
        DataTable fhomeview = new DataTable();
        DataTable mhomeview = new DataTable();
        DataTable defaCityview = new DataTable();
        DataTable empHisview = new DataTable();
        DataTable empSecview = new DataTable();
        DataTable lanview = new DataTable();
        DataTable empPriorview = new DataTable();
        bool glNewLang = false;
        bool glNewHistory = true;
        bool glNewPrior = true;
        public emplyee(string cos, int dcompid, string dloca)
        {
            InitializeComponent();
            cs = cos;
            ncompid = dcompid;
            cLoca = dloca;
        }

        private void emplyee_Load(object sender, EventArgs e)
        {
            this.Text = cLoca + "<< Employee basic Data >>";
            getstaff();
            getbasics();
            textBox6.Text = GetClient_Code.clientCode_int(cs, "staffid").ToString().PadLeft(6, '0');
            textBox46.Text = GetClient_Code.clientCode_int(cs, "staffid").ToString().PadLeft(6, '0');
            empHistGrid.Columns["dFrDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
            empHistGrid.Columns["dToDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
            priorGrid.Columns["pFrDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
            priorGrid.Columns["pToDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
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
//                getPayDetails();
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
                }
                ndConnHandle.Close();
                textBox45.Text = staffview.Rows.Count.ToString();
            }
        }

        private void getbasics()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string basesql0 = "select mar_name,mar_id from marystat order by mar_name  ";
                SqlDataAdapter baseda0 = new SqlDataAdapter(basesql0, ndConnHandle);
                DataTable marview = new DataTable();
                baseda0.Fill(marview);
                if (marview.Rows.Count > 0)
                {
                    comboBox10.DataSource = marview.DefaultView;
                    comboBox10.DisplayMember = "mar_name";
                    comboBox10.ValueMember = "mar_id";
                    comboBox10.SelectedIndex = -1;
                }

                string basesql1 = "select ban_name,ban_id from band order by ban_name";
                SqlDataAdapter baseda1 = new SqlDataAdapter(basesql1, ndConnHandle);
                DataTable bandview = new DataTable();
                baseda1.Fill(bandview);
                if (bandview.Rows.Count > 0)
                {
                    comboBox7.DataSource = bandview.DefaultView;
                    comboBox7.DisplayMember = "ban_name";
                    comboBox7.ValueMember = "ban_id";
                    comboBox7.SelectedIndex = -1;
                }

                string basesql2 = "select tit_name,tit_id from titres order by tit_name";
                SqlDataAdapter baseda2 = new SqlDataAdapter(basesql2, ndConnHandle);
                DataTable titview = new DataTable();
                baseda2.Fill(titview);
                if (titview.Rows.Count > 0)
                {
                    comboBox14.DataSource = titview.DefaultView;
                    comboBox14.DisplayMember = "tit_name";
                    comboBox14.ValueMember = "tit_id";
                    comboBox14.SelectedIndex = -1;
                }

                string basesql3 = "select id_name,idtype from id_type order by id_name";
                SqlDataAdapter baseda3 = new SqlDataAdapter(basesql3, ndConnHandle);
                DataTable idview = new DataTable();
                baseda3.Fill(idview);
                if (idview.Rows.Count > 0)
                {
                    comboBox11.DataSource = idview.DefaultView;
                    comboBox11.DisplayMember = "id_name";
                    comboBox11.ValueMember = "idtype";
                    comboBox11.SelectedIndex = -1;
                }

                string basesql4 = "select cou_name,cou_id,idd_code from country order by cou_name";
                SqlDataAdapter baseda4 = new SqlDataAdapter(basesql4, ndConnHandle);
                DataTable couview = new DataTable();
                baseda4.Fill(couview);
                if (couview.Rows.Count > 0)
                {
                    comboBox9.DataSource = couview.DefaultView;
                    comboBox9.DisplayMember = "cou_name";
                    comboBox9.ValueMember = "cou_id";
                    comboBox9.SelectedIndex = -1;
                }

                string basesql5 = "select eth_name,eth_id from ethnies order by eth_name";
                SqlDataAdapter baseda5 = new SqlDataAdapter(basesql5, ndConnHandle);
                DataTable ethview = new DataTable();
                baseda5.Fill(ethview);
                if (ethview.Rows.Count > 0)
                {
                    comboBox16.DataSource = ethview.DefaultView;
                    comboBox16.DisplayMember = "eth_name";
                    comboBox16.ValueMember = "eth_id";
                    comboBox16.SelectedIndex = -1;
                }

                string basesql6 = "select rel_name,rel_id from relgion order by rel_name";
                SqlDataAdapter baseda6 = new SqlDataAdapter(basesql6, ndConnHandle);
                DataTable relview = new DataTable();
                baseda6.Fill(relview);
                if (relview.Rows.Count > 0)
                {
                    comboBox18.DataSource = relview.DefaultView;
                    comboBox18.DisplayMember = "rel_name";
                    comboBox18.ValueMember = "rel_id";
                    comboBox18.SelectedIndex = -1;
                }

                string basesql7 = "select cou_name,cou_id,idd_code from country order by cou_name";
                SqlDataAdapter baseda7 = new SqlDataAdapter(basesql7, ndConnHandle);
                DataTable homview = new DataTable();
                baseda7.Fill(homview);
                if (homview.Rows.Count > 0)
                {
                    comboBox13.DataSource = homview.DefaultView;
                    comboBox13.DisplayMember = "cou_name";
                    comboBox13.ValueMember = "cou_id";
                    comboBox13.SelectedIndex = -1;
                }

                string basesql8 = "select dis_name,dis_id from disa_type order by dis_name";
                SqlDataAdapter baseda8 = new SqlDataAdapter(basesql8, ndConnHandle);
                DataTable disview = new DataTable();
                baseda8.Fill(disview);
                if (disview.Rows.Count > 0)
                {
                    comboBox23.DataSource = disview.DefaultView;
                    comboBox23.DisplayMember = "dis_name";
                    comboBox23.ValueMember = "dis_id";
                    comboBox23.SelectedIndex = -1;
                }

            }
        }

        private void getempDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string basesql9 = "select br_name,branchid from branch where compobnk=1 order by br_name";
                SqlDataAdapter baseda9 = new SqlDataAdapter(basesql9, ndConnHandle);
                DataTable brcview = new DataTable();
                baseda9.Fill(brcview);
                if (brcview.Rows.Count > 0)
                {
                    comboBox8.DataSource = brcview.DefaultView;
                    comboBox8.DisplayMember = "br_name";
                    comboBox8.ValueMember = "branchid";
                    comboBox8.SelectedIndex = -1;
                }

                string basesql10 = "select des_name,des_id from designation order by des_name";
                SqlDataAdapter baseda10 = new SqlDataAdapter(basesql10, ndConnHandle);
                DataTable desview = new DataTable();
                baseda10.Fill(desview);
                if (desview.Rows.Count > 0)
                {
                    comboBox1.DataSource = desview.DefaultView;
                    comboBox1.DisplayMember = "des_name";
                    comboBox1.ValueMember = "des_id";
                    comboBox1.SelectedIndex = -1;
                }

                string basesql11 = "select des_name,des_id from designation order by des_name";
                SqlDataAdapter baseda11 = new SqlDataAdapter(basesql11, ndConnHandle);
                DataTable repview = new DataTable();
                baseda11.Fill(repview);
                if (repview.Rows.Count > 0)
                {
                    comboBox3.DataSource = repview.DefaultView;
                    comboBox3.DisplayMember = "des_name";
                    comboBox3.ValueMember = "des_id";
                    comboBox3.SelectedIndex = -1;
                }


                string basesql12 = "select cos_name,cos_id from costcent order by cos_name";
                SqlDataAdapter baseda12 = new SqlDataAdapter(basesql12, ndConnHandle);
                DataTable cosview = new DataTable();
                baseda12.Fill(cosview);
                if (cosview.Rows.Count > 0)
                {
                    comboBox4.DataSource = cosview.DefaultView;
                    comboBox4.DisplayMember = "cos_name";
                    comboBox4.ValueMember = "cos_id";
                    comboBox4.SelectedIndex = -1;
                }


                string basesql13 = "select dep_name,dep_id from dept order by dep_name";
                SqlDataAdapter baseda13 = new SqlDataAdapter(basesql13, ndConnHandle);
                DataTable depview = new DataTable();
                baseda13.Fill(depview);
                if (depview.Rows.Count > 0)
                {
                    comboBox2.DataSource = depview.DefaultView;
                    comboBox2.DisplayMember = "dep_name";
                    comboBox2.ValueMember = "dep_id";
                    comboBox2.SelectedIndex = -1;
                }


                string basesql14 = "select bnk_name,bnk_id from bank order by bnk_name";
                SqlDataAdapter baseda14 = new SqlDataAdapter(basesql14, ndConnHandle);
                DataTable bnkview = new DataTable();
                baseda14.Fill(bnkview);
                if (bnkview.Rows.Count > 0)
                {
                    comboBox6.DataSource = bnkview.DefaultView;
                    comboBox6.DisplayMember = "bnk_name";
                    comboBox6.ValueMember = "bnk_id";
                    comboBox6.SelectedIndex = -1;
                }
            }
        }

        private void gettown(int tncouid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                cityview.Clear();
                string basesql0 = "select city_name,city_id,cou_id from city where cou_id = " + tncouid + " order by city_name";
                SqlDataAdapter baseda0 = new SqlDataAdapter(basesql0, ndConnHandle);
                baseda0.Fill(cityview);
                if (cityview.Rows.Count > 0)
                {
                    comboBox12.DataSource = cityview.DefaultView;
                    comboBox12.DisplayMember = "city_name";
                    comboBox12.ValueMember = "city_id";
                    comboBox12.SelectedIndex = -1;
                }
            }
        }

        private void getnoktown(int tncouid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                nokcityview.Clear();
                string basesql0 = "select city_name,city_id,cou_id from city where cou_id = " + tncouid + " order by city_name";
                SqlDataAdapter baseda0 = new SqlDataAdapter(basesql0, ndConnHandle);
                baseda0.Fill(nokcityview);
                if (nokcityview.Rows.Count > 0)
                {
                    comboBox20.DataSource = nokcityview.DefaultView;
                    comboBox20.DisplayMember = "city_name";
                    comboBox20.ValueMember = "city_id";
                    comboBox20.SelectedIndex = -1;
                }
            }
        }
        private void getbnkBranch(int tnbnkid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string basesql19 = "select bnk_id, bnkbr_name from bnk_branch where bnk_ID = "+tnbnkid+" order by bnkbr_name";
                SqlDataAdapter baseda19 = new SqlDataAdapter(basesql19, ndConnHandle);
                DataTable bnrcview = new DataTable();
                baseda19.Fill(bnrcview);
                if (bnrcview.Rows.Count > 0)
                {
                    comboBox5.DataSource = bnrcview.DefaultView;
                    comboBox5.DisplayMember = "bnkbr_name";
                    comboBox5.ValueMember = "bnk_id";
                    comboBox5.SelectedIndex = -1;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
//                page01ok();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
       //         page01ok();
            }
        }

        private void page01ok() //staff basic details 
        {
            MessageBox.Show("inside page 01 ok");
            bool llGender = radioButton1.Checked || radioButton2.Checked ? true : false;
            bool lAgeOK = DateTime.Today.Year - dateTimePicker19.Value.Year > 18 ? true : false;
            if (textBox6.Text != "" && textBox7.Text != "" && textBox8.Text != "" && textBox9.Text != "" && textBox11.Text != "" && textBox5.Text != "" &&
                textBox14.Text != "" && textBox17.Text != "" && textBox19.Text != "" && textBox21.Text != "" && llGender && comboBox11.SelectedIndex > -1 &&
                comboBox9.SelectedIndex > -1 && comboBox16.SelectedIndex > -1 && comboBox18.SelectedIndex > -1 && comboBox13.SelectedIndex > -1 && lAgeOK)
            {
                perSaveButton.Enabled = true;
                perSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                perSaveButton.Enabled = false;
                perSaveButton.BackColor = Color.Gainsboro;
            }
        }


        private void page02ok() //staff employment details 
        {
            bool lbnk = (comboBox6.SelectedIndex > -1 ? (comboBox5.SelectedIndex > -1 && textBox2.Text != "" ? true : false) : true);

            if (comboBox1.SelectedIndex > -1 && comboBox3.SelectedIndex > -1 && comboBox4.SelectedIndex > -1 && comboBox2.SelectedIndex > -1 && comboBox8.SelectedIndex > -1 &&
                comboBox7.SelectedIndex > -1 && lbnk)
            {
                empSaveDetails.Enabled = true;
                empSaveDetails.BackColor = Color.LawnGreen;
            }
            else
            {
                empSaveDetails.Enabled = false;
                empSaveDetails.BackColor = Color.Gainsboro;
            }
        }

        private void page03ok()  // next of kin details 
        {
            if (textBox22.Text != "" && textBox27.Text != "" && textBox28.Text != "" && textBox29.Text != "" && 
                comboBox15.SelectedIndex > -1 && comboBox21.SelectedIndex > -1 && comboBox45.SelectedIndex > -1 )
            {
                nokSaveButton.Enabled = true;
                nokSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                nokSaveButton.Enabled = false;
                nokSaveButton.BackColor = Color.Gainsboro;
            }
        }

        private void page04ok() //disability and parent information
        {
            bool ddisa = comboBox23.SelectedIndex <= -1 ? true : (comboBox46.SelectedIndex > -1 && acquireDate.Value < DateTime.Today ? true : false);
            if (textBox24.Text != "" && textBox32.Text != "" &&  comboBox48.SelectedIndex > -1 && comboBox22.SelectedIndex > -1 && 
                comboBox50.SelectedIndex > -1 && comboBox25.SelectedIndex > -1 && ddisa)
            {
                disSaveButton.Enabled = true;
                disSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                disSaveButton.Enabled = false;
                disSaveButton.BackColor = Color.Gainsboro;
            }
        }

        private void paypageok()  // payroll details 
        {
            if (textBox40.Text != "" && textBox43.Text != "" && textBox44.Text !="" && 
                comboBox29.SelectedIndex > -1 && comboBox33.SelectedIndex > -1 && comboBox34.SelectedIndex > -1)
            {
                paySaveButton.Enabled = true;
                paySaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                paySaveButton.Enabled = false;
                paySaveButton.BackColor = Color.Gainsboro;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox13_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox13.Focused)
            {
                gettown(Convert.ToInt16(comboBox13.SelectedValue));
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Marital Status", "marystat", "mar_name", ncompid);
            d2code.ShowDialog();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            twocode d2code = new twocode(cs, "Title Types", "titres", "tit_name", ncompid);
            d2code.ShowDialog();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            onecode d1code = new onecode(cs, "ID Type Setup", "id_type", "id_name");
            d1code.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Bonding dbond = new TclassLibrary.Bonding(cs, ncompid, cLoca);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            updEmpDetails();
            initEmpvariables();
        }

        private void updEmpDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string cquery = "update staff set dep_id = @tldep_id,des_id=@tldes_id,cos_id=@tlcos_id,report2=@tlReport2,nhif=@tlnhif,ssno=@tlssno,pin=@tlpin,nbrchid=@dbranchid,";
                cquery += "grade = @tlgrade,grade_poin=@tlgradepoint,nbankid=@tlbnk_ID,bbranch_id=@tlBnkBr_ID,bnk_acct=@tlcBnkAcct,nbandid = @tlnBandid,doe = @hiredate, confdate=@dconfdate,";
                cquery += "dretire=@dretire where staffno = @tlstaffno";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.UpdateCommand.Parameters.Add("@tlstaffno", SqlDbType.VarChar).Value = textBox46.Text.ToString().Trim();
                cuscommand.UpdateCommand.Parameters.Add("@tldep_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox2.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tldes_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox1.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tlcos_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox4.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tlReport2", SqlDbType.Int).Value = Convert.ToInt16(comboBox3.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tlnhif", SqlDbType.VarChar).Value = textBox12.Text.ToString().Trim() != "" ? textBox12.Text.ToString().Trim() : "";
                cuscommand.UpdateCommand.Parameters.Add("@tlssno", SqlDbType.VarChar).Value = textBox16.Text.ToString().Trim() != "" ? textBox16.Text.ToString().Trim() : "";
                cuscommand.UpdateCommand.Parameters.Add("@tlpin", SqlDbType.VarChar).Value = textBox13.Text.ToString().Trim() != "" ? textBox13.Text.ToString().Trim() : "";

                cuscommand.UpdateCommand.Parameters.Add("@tlgrade", SqlDbType.Int).Value = textBox1.Text != "" ? Convert.ToInt16(textBox1.Text) : 0;
                cuscommand.UpdateCommand.Parameters.Add("@tlgradepoint", SqlDbType.Int).Value = textBox3.Text != "" ? Convert.ToInt16(textBox3.Text) : 0;
                cuscommand.UpdateCommand.Parameters.Add("@tlbnk_ID", SqlDbType.Int).Value = comboBox6.SelectedIndex > -1 ? Convert.ToInt16(comboBox6.SelectedValue) : 0;
                cuscommand.UpdateCommand.Parameters.Add("@tlBnkBr_ID", SqlDbType.Int).Value = comboBox5.SelectedIndex > -1 ? Convert.ToInt16(comboBox5.SelectedValue) : 0;
                cuscommand.UpdateCommand.Parameters.Add("@tlcBnkAcct", SqlDbType.VarChar).Value = textBox2.Text != "" ? textBox2.Text : "";
                cuscommand.UpdateCommand.Parameters.Add("@tlnBandid", SqlDbType.Int).Value = Convert.ToInt16(comboBox7.SelectedValue);

                cuscommand.UpdateCommand.Parameters.Add("@hiredate", SqlDbType.DateTime).Value = Convert.ToDateTime(hireDate.Text);
                cuscommand.UpdateCommand.Parameters.Add("@dconfdate", SqlDbType.DateTime).Value = Convert.ToDateTime(confirmDate.Text);
                cuscommand.UpdateCommand.Parameters.Add("@dretire", SqlDbType.DateTime).Value = Convert.ToDateTime(retireDate.Text);
                cuscommand.UpdateCommand.Parameters.Add("@dbranchid", SqlDbType.Int).Value = Convert.ToInt16(comboBox8.SelectedValue);

                ndConnHandle.Open();
                cuscommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Staff employment details updated successfully");
            }
        }

        private void updNokDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                //int cityid = Convert.ToInt16(comboBox20.SelectedValue);
                //MessageBox.Show("city id is " + cityid);
                string cquery = "update staff set nokfname=@tcnokfname,noklname=@tcnoklname,nokrelation=@tnokrelation,noktel=@tnoktel,nokidtype=@tnokidtype,nokidnumb=@tnokidnumb,";
                cquery += "nokemail=@tcnokemail,noktit=@tcnoktit,nokcou_id=@tcnokcou_id,nokcity_id=@tcnokcity_id,nokaddr=@tcnokaddr,nokpobox=@tcnokpobox where staffno = @tlstaffno";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.UpdateCommand.Parameters.Add("@tlstaffno", SqlDbType.VarChar).Value = textBox50.Text.ToString().Trim();
                cuscommand.UpdateCommand.Parameters.Add("@tcnokfname", SqlDbType.VarChar).Value = textBox28.Text.ToString() .Trim();
                cuscommand.UpdateCommand.Parameters.Add("@tcnoklname", SqlDbType.VarChar).Value = textBox29.Text.ToString().Trim();
                cuscommand.UpdateCommand.Parameters.Add("@tnokrelation", SqlDbType.Int).Value = Convert.ToInt16(comboBox45.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tnoktel", SqlDbType.VarChar).Value = textBox22.Text.ToString().Trim();
                cuscommand.UpdateCommand.Parameters.Add("@tnokidtype", SqlDbType.Int).Value = Convert.ToInt16(comboBox15.SelectedValue) ;
                cuscommand.UpdateCommand.Parameters.Add("@tnokidnumb", SqlDbType.VarChar).Value = textBox27.Text.ToString().Trim();
                cuscommand.UpdateCommand.Parameters.Add("@tcnokemail", SqlDbType.VarChar).Value = textBox26.Text.ToString().Trim() ;
                cuscommand.UpdateCommand.Parameters.Add("@tcnoktit", SqlDbType.Int).Value = Convert.ToInt16(comboBox19.SelectedValue) ;
                cuscommand.UpdateCommand.Parameters.Add("@tcnokcou_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox21.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tcnokcity_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox20.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tcnokaddr", SqlDbType.VarChar).Value = richTextBox4.Text.ToString().Trim();
                cuscommand.UpdateCommand.Parameters.Add("@tcnokpobox", SqlDbType.VarChar).Value = textBox30.Text.ToString().Trim();

                ndConnHandle.Open();
                cuscommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Staff next of kin details updated successfully");
            }
        }

        private void initEmpvariables()
        {
            textBox6.Text = textBox7.Text = textBox9.Text = textBox8.Text = textBox11.Text = textBox5.Text = textBox15.Text = textBox21.Text = textBox19.Text = richTextBox2.Text = richTextBox1.Text = textBox17.Text = textBox14.Text = "";
            comboBox14.SelectedIndex = comboBox10.SelectedIndex = comboBox11.SelectedIndex = comboBox9.SelectedIndex = comboBox16.SelectedIndex = comboBox18.SelectedIndex = comboBox13.SelectedIndex = comboBox12.SelectedIndex = -1;
            radioButton1.Checked = radioButton2.Checked = false;
        }
        private void comboBox6_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox6.Focused)
            {
                getbnkBranch(Convert.ToInt16(comboBox6.SelectedValue));
            }
        }

        private void tabPage6_Enter(object sender, EventArgs e)
        {
            getempDetails();
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox46.Text = tcemp;
            textBox48.Text = tcempname;
            persEmpDetails(tcemp);
        }

        private void persEmpDetails(string tcStaffNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string empsql = "exec tsp_getActiveStaff_One " + ncompid + ",'" + tcStaffNo + "'";
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                DataTable empview = new DataTable();
                daemp.Fill(empview);
                if (empview.Rows.Count > 0)
                {
                    hireDate.Text = empview.Rows[0]["doe"].ToString();
                    confirmDate.Text = empview.Rows[0]["confdate"].ToString();
                    retireDate.Text = empview.Rows[0]["dretire"].ToString();
                    textBox12.Text = empview.Rows[0]["nhif"].ToString();
                    textBox16.Text = empview.Rows[0]["ssno"].ToString();
                    textBox13.Text = empview.Rows[0]["pin"].ToString();
                    textBox4.Text = "Active Staff";
                    textBox1.Text = empview.Rows[0]["grade"].ToString();
                    textBox3.Text = empview.Rows[0]["grade_poin"].ToString();

                    comboBox1.SelectedValue = Convert.ToInt16(empview.Rows[0]["des_id"]);
                    comboBox2.SelectedValue = Convert.ToInt16(empview.Rows[0]["dep_id"]);
                    comboBox3.SelectedValue = Convert.ToInt16(empview.Rows[0]["report2"]);
                    comboBox4.SelectedValue = Convert.ToInt16(empview.Rows[0]["cos_id"]);
                    comboBox5.SelectedValue = Convert.ToInt16(empview.Rows[0]["bbranch_id"]);
                    comboBox6.SelectedValue = Convert.ToInt16(empview.Rows[0]["nbankid"]);
                    comboBox7.SelectedValue = Convert.ToInt16(empview.Rows[0]["nbandid"]);
                    comboBox8.SelectedValue = Convert.ToInt16(empview.Rows[0]["nbrchid"]);
                }
            }
        }

        private void persNokDetails(string tcStaffNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string noksql = "exec tsp_getActiveStaff_One " + ncompid + ",'" + tcStaffNo + "'";
                SqlDataAdapter danok = new SqlDataAdapter(noksql, ndConnHandle);
                DataTable nokview = new DataTable();
                danok.Fill(nokview);
                if (nokview.Rows.Count > 0)
                {
                    comboBox19.SelectedValue = !Convert.IsDBNull(nokview.Rows[0]["noktit"]) ? Convert.ToInt16(nokview.Rows[0]["noktit"]) : 0;
                    textBox28.Text = !Convert.IsDBNull(nokview.Rows[0]["nokfname"]) ? nokview.Rows[0]["nokfname"].ToString() : "";
                    textBox29.Text = !Convert.IsDBNull(nokview.Rows[0]["noklname"]) ? nokview.Rows[0]["noklname"].ToString() : "";
                    comboBox45.SelectedValue = !Convert.IsDBNull(nokview.Rows[0]["nokrelation"]) ? nokview.Rows[0]["nokrelation"] : 0;
                    textBox22.Text = !Convert.IsDBNull(nokview.Rows[0]["noktel"]) ? nokview.Rows[0]["noktel"].ToString() : "";
                    comboBox15.SelectedValue = !Convert.IsDBNull(nokview.Rows[0]["nokidtype"]) ? nokview.Rows[0]["nokidtype"] : 0;
                    textBox26.Text = !Convert.IsDBNull(nokview.Rows[0]["nokemail"]) ? nokview.Rows[0]["nokemail"].ToString() : "";
                    textBox27.Text = !Convert.IsDBNull(nokview.Rows[0]["nokidnumb"]) ? nokview.Rows[0]["nokidnumb"].ToString() : "";
                    comboBox19.SelectedValue = !Convert.IsDBNull(nokview.Rows[0]["noktit"]) ? nokview.Rows[0]["noktit"] : 0;
                    comboBox21.SelectedValue = !Convert.IsDBNull(nokview.Rows[0]["nokcou_id"]) ? nokview.Rows[0]["nokcou_id"] : 0;
                    int ncouid = !Convert.IsDBNull(nokview.Rows[0]["nokcou_id"]) ? Convert.ToInt16(nokview.Rows[0]["nokcou_id"]) : 0;
                    int ncity  = !Convert.IsDBNull(nokview.Rows[0]["nokcity_id"]) ? Convert.ToInt16(nokview.Rows[0]["nokcity_id"]) : 0;
                    showTown(ncouid, ncity, 1);
                    richTextBox4.Text = !Convert.IsDBNull(nokview.Rows[0]["nokaddr"]) ? nokview.Rows[0]["nokaddr"].ToString() : "";
                    textBox30.Text = !Convert.IsDBNull(nokview.Rows[0]["nokpobox"]) ? nokview.Rows[0]["nokpobox"].ToString() : "";
                }
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            personalDetails();
            initPersvariables();
            updateClient_Code updc = new updateClient_Code();
            updc.updClient(cs, "staffid");
            textBox6.Text = GetClient_Code.clientCode_int(cs, "staffid").ToString().PadLeft(6, '0');
        }

        private void initPersvariables()
        {
            textBox6.Text = textBox7.Text = textBox9.Text = textBox8.Text = textBox11.Text = textBox5.Text = textBox15.Text = textBox21.Text = textBox19.Text = richTextBox2.Text = richTextBox1.Text = textBox17.Text = textBox14.Text = "";
            comboBox14.SelectedIndex = comboBox10.SelectedIndex = comboBox11.SelectedIndex = comboBox9.SelectedIndex = comboBox16.SelectedIndex = comboBox18.SelectedIndex = comboBox13.SelectedIndex = comboBox12.SelectedIndex = -1;
            radioButton1.Checked = radioButton2.Checked = false;
            perSaveButton.Enabled = false;
            perSaveButton.BackColor = Color.Gainsboro;
        }

        private void personalDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                if (gcInputstat == "N")
                {
                    string cquery = "Insert Into staff (staffno,fileRef,dataentryd,firstname,midname,lastname,marystat,gender,idtype,idnumb,nid,dob,placbirth,ethny,religion,";
                    cquery += "address1,cou_id,city_id,pobox,mobilenb,homenb,remark,compid,emailnb,stitle) ";
                    cquery += "Values (@tlstaffno,@tlfileref,convert(date,getdate()),@tlfirstname,@tlmidname,@tllastname,@tlmarystat,@tlgender,@tlidtype,@tlidnumb,@tgnNationID,@tldob,@tlplacbirth,@tlethny,@tlreligion,";
                    cquery += "@tlcHomeAddr,@tlcou_id,@tlnCity,@tlcPoBox,@tlcMobile,@tlcHomeTel,@tlcRemarks,@tgnCompID,@tlcEmail,@tlstitle)";
                    SqlDataAdapter cuscommand = new SqlDataAdapter();
                    cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                    cuscommand.InsertCommand.Parameters.Add("@tlstaffno", SqlDbType.VarChar).Value = textBox6.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tlfileref", SqlDbType.VarChar).Value = textBox7.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tlfirstname", SqlDbType.VarChar).Value = textBox9.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tlmidname", SqlDbType.VarChar).Value = textBox8.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tllastname", SqlDbType.VarChar).Value = textBox11.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tlmarystat", SqlDbType.Int).Value = Convert.ToInt16(comboBox10.SelectedValue);

                    cuscommand.InsertCommand.Parameters.Add("@tlgender", SqlDbType.Bit).Value = radioButton1.Checked ? false : true;
                    cuscommand.InsertCommand.Parameters.Add("@tlidtype", SqlDbType.Int).Value = Convert.ToInt16(comboBox11.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@tlidnumb", SqlDbType.VarChar).Value = textBox5.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tgnNationID", SqlDbType.Int).Value = Convert.ToInt16(comboBox9.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@tldob", SqlDbType.DateTime).Value = Convert.ToDateTime(dateTimePicker19.Text);
                    cuscommand.InsertCommand.Parameters.Add("@tlplacbirth", SqlDbType.VarChar).Value = textBox14.Text.Trim();

                    cuscommand.InsertCommand.Parameters.Add("@tlethny", SqlDbType.Int).Value = Convert.ToInt16(comboBox16.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@tlreligion", SqlDbType.Int).Value = Convert.ToInt16(comboBox18.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@tlcHomeAddr", SqlDbType.VarChar).Value = richTextBox1.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tlcou_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox13.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@tlnCity", SqlDbType.Int).Value = Convert.ToInt16(comboBox12.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@tlcPoBox", SqlDbType.VarChar).Value = textBox15.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tlcMobile", SqlDbType.VarChar).Value = textBox21.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tlcHomeTel", SqlDbType.VarChar).Value = textBox19.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tlcRemarks", SqlDbType.VarChar).Value = richTextBox2.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tgnCompID", SqlDbType.Int).Value = ncompid;
                    cuscommand.InsertCommand.Parameters.Add("@tlcEmail", SqlDbType.VarChar).Value = textBox17.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tlstitle", SqlDbType.Int).Value = Convert.ToInt16(comboBox14.SelectedValue);

                    ndConnHandle.Open();
                    cuscommand.InsertCommand.ExecuteNonQuery();  //Insert new record
                    ndConnHandle.Close();
                    MessageBox.Show("New staff personal details added successfully");
                }
                else
                {
                    string cquery = "update staff set firstname=@tlfirstname, midname=@tlmidname, lastname=@tllastname, marystat=@tlmarystat, gender=@tlgender, idtype=@tlidtype, idnumb=@tlidnumb, ";
                    cquery += "nid =@tgnNationID, dob=@tldob, placbirth=@tlplacbirth, ethny=@tlethny, religion=@tlreligion, ";
                    cquery += "address1=@tlcHomeAddr,cou_id=@tlcou_id,city_id=@tlnCity,pobox=@tlcPoBox,mobilenb=@tlcMobile,homenb=@tlcHomeTel,remark=@tlcRemarks,emailnb=@tlcEmail,stitle=@tlstitle";

                    SqlDataAdapter cuscommand = new SqlDataAdapter();
                    cuscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                    cuscommand.UpdateCommand.Parameters.Add("@tlfirstname", SqlDbType.VarChar).Value = textBox9.Text.Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tlmidname", SqlDbType.VarChar).Value = textBox8.Text.Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tllastname", SqlDbType.VarChar).Value = textBox11.Text.Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tlmarystat", SqlDbType.Int).Value = Convert.ToInt16(comboBox10.SelectedValue);

                    cuscommand.UpdateCommand.Parameters.Add("@tlgender", SqlDbType.Bit).Value = radioButton1.Checked ? false : true;
                    cuscommand.UpdateCommand.Parameters.Add("@tlidtype", SqlDbType.Int).Value = Convert.ToInt16(comboBox11.SelectedValue);
                    cuscommand.UpdateCommand.Parameters.Add("@tlidnumb", SqlDbType.VarChar).Value = textBox5.Text.Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tgnNationID", SqlDbType.Int).Value = Convert.ToInt16(comboBox9.SelectedValue);
                    cuscommand.UpdateCommand.Parameters.Add("@tldob", SqlDbType.DateTime).Value = Convert.ToDateTime(dateTimePicker19.Text);
                    cuscommand.UpdateCommand.Parameters.Add("@tlplacbirth", SqlDbType.VarChar).Value = textBox14.Text.Trim();

                    cuscommand.UpdateCommand.Parameters.Add("@tlethny", SqlDbType.Int).Value = Convert.ToInt16(comboBox16.SelectedValue);
                    cuscommand.UpdateCommand.Parameters.Add("@tlreligion", SqlDbType.Int).Value = Convert.ToInt16(comboBox18.SelectedValue);
                    cuscommand.UpdateCommand.Parameters.Add("@tlcHomeAddr", SqlDbType.VarChar).Value = richTextBox1.Text.Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tlcou_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox13.SelectedValue);
                    cuscommand.UpdateCommand.Parameters.Add("@tlnCity", SqlDbType.Int).Value = Convert.ToInt16(comboBox12.SelectedValue);
                    cuscommand.UpdateCommand.Parameters.Add("@tlcPoBox", SqlDbType.VarChar).Value = textBox15.Text.Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tlcMobile", SqlDbType.VarChar).Value = textBox21.Text.Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tlcHomeTel", SqlDbType.VarChar).Value = textBox19.Text.Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tlcRemarks", SqlDbType.VarChar).Value = richTextBox2.Text.Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tlcEmail", SqlDbType.VarChar).Value = textBox17.Text.Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tlstitle", SqlDbType.Int).Value = Convert.ToInt16(comboBox14.SelectedValue);

                    ndConnHandle.Open();
                    cuscommand.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                    MessageBox.Show("Staff personal details updated successfully");
                }
            }
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            employDetais();
        }

        private void employDetais()
        {
            /*
             This.Enabled =.F.
With Thisform.pageframe1.page1				&&Staff contact details
	lcHomeAddr=.edit1.Value
	lnCou=gnNationID
	lnCity=.combo6.Value
	lcPobox=.text1.Value
	lcMobile=Chrtran(Alltrim(.text11.Value),'-','')
	lcHomeTel=Alltrim(.text3.Value)+Alltrim(.text7.Value)
	lcRemarks=.edit2.Value
	lcEmail=.text4.Value
Endwith

With Thisform.pageframe1.page2				&&Next of kin
	lcNOKtit=.combo6.Value
	lcNOKFname=.text3.Value
	lcNOKLname=.text5.Value
	lcNOKHomeAddr=.edit1.Value
	lnNOKCou=.combo2.Value
	lnNOKCity=.combo1.Value
	lcNOKPobox=.text2.Value
	lcNOKEmail=.text1.Value
	lcNOKPhone=Chrtran(Alltrim(.text11.Value),'-','')
	lcNOKRel=.text4.Value
	lnNOKidtype=.combo3.Value
	lcNOKid=.text7.Value
Endwith


With Thisform.pageframe1.page3				&&Disability
	lnDisaType=.combo6.Value
	lcAcq=.text3.Value
	ldDate=.text4.Value
	lcPriv=.edit1.Value
Endwith


With Thisform.pageframe1.page4				&&parents information
	lcdad_name=.text2.Value
	lcdad_addr=.text1.Value
	lndad_nan=.combo1.Value
	lndad_dec=Iif(.check1.Value,1,0)

	lcmum_name=.text5.Value
	lcmum_addr=.text3.Value
	lnmum_nan=.combo2.Value
	lnmum_dec=Iif(.check2.Value,1,0)
Endwith


With Thisform
	lfirstname=.text3.Value
	lmidname=.text4.Value
	llastname=.text5.Value
	lstaffno=.text1.Value
	ldep_id=.combo4.Value
	ldes_id=.combo5.Value
	lcos_id=.combo7.Value
	lstitle=.combo6.Value
	lReport2=.combo8.Value
	ldataentryd=gdSysDate
	laddress1=lcHomeAddr
	lcity_id=lnCity
	lidtype=.combo9.Value
	lidnumb=.text20.Value
	lcou_id=gnNationID
	lhomenb=lcHomeTel
	lmobilenb=lcMobile
	lemailnb=lcEmail
	lplacbirth=.text15.Value
	ldob=.text14.Value
	ldoe=.text25.Value
	lmarystat=.combo3.Value
	lcphoto=gcPhoto
	lldisable=lnDisaType
	lbranch=.combo2.Value
	lReport2=.combo8.Value
	ldretire=.text17.Value
	lssno=.text6.Value
	lnhif=.text21.Value
	lnbrchid=.combo2.Value
	lgrade_poin=.text26.Value
	lconfdate=.text27.Value
	lnid=.combo1.Value
	lpin=.text8.Value
	lpobox=lcPobox
	lplacbirth=.text15.Value
	lmarystat=.combo3.Value
	lgender=Iif(.optiongroup1.Value=1,0,1)
	lethny=.combo10.Value
	lreligion=.combo11.Value
	lfileref=.text12.Value
	lgrade=.text22.Value
	lbnk_ID=.combo14.Value
	lnBandid=.combo12.Value
	lBnkBr_ID=.combo13.Value
	lcBnkAcct=.text2.Value
	If gcInputstat='N'
		sn=SQLExec(gnConnHandle,"Insert Into staff (staffno,fileRef,dataentryd,firstname,midname,lastname,marystat,gender,idtype,idnumb,nid,dob,placbirth,ethny,religion,"+;
			"doe,des_id,report2,cos_id,dep_id,branch,grade,grade_poin,nhif,ssno,pin,confdate,empstatus,address1,cou_id,city_id,pobox,mobilenb,homenb,remark,compid,dretire,nbrchid,"+;
			"emailnb,stitle,photo,nbankid,bbranch_id,bnk_acct,nokfname,noklname,nokrelation,noktel,nbandid,nokidtype,nokidnumb,"+;
			"dad_name,dad_addr,dad_nan,dad_dec,mum_name,mum_addr,mum_nan,mum_dec) "+;
			"Values (?lstaffno,?lfileref,?gdSysDate,?lfirstname,?lmidname,?llastname,?lmarystat,?lgender,?lidtype,?lidnumb,?lcou_id,?ldob,"+;
			"?lplacbirth,?lethny,?lreligion,?ldoe,?ldes_id,?lreport2,?lcos_id,?ldep_id,?lbranch,?lgrade,"+;
			"?lgrade_poin,?lnhif,?lssno,?lpin,?lconfdate,'A',?lcHomeAddr,?gnNationID,?lnCity,?lcPoBox,?lcMobile,?lcHomeTel,?lcRemarks,?gnCompID,"+;
			"?ldretire,?lbranch,?lcEmail,?lstitle,?lcphoto,?lbnk_ID,?lBnkBr_ID,?lcBnkAcct,?lcNOKFname,?lcNOKLname,?lcNOKRel,?lcNOKPhone,?lnBandid,?lnNOKidtype,?lcNOKid,"+;
			"?lcdad_name,?lcdad_addr,?lndad_nan,?lndad_dec,?lcmum_name,?lcmum_addr,?lnmum_nan,?lnmum_dec)","staffin")
		If sn>0
			Thisform.createaccount
			Thisform.updatepatientcode
		Else
			=sysmsg('Could not insert employee details, inform IT DEPT')
		Endif
	Else
		sn=SQLExec(gnConnHandle,"update staff set firstname=?lfirstname,midname=?lmidname,lastname=?llastname,staffno=?lstaffno,dep_id=?ldep_id,des_id=?ldes_id,cos_id=?lcos_id,cou_id=?gnNationID,idtype=?lidtype,idnumb=?lidnumb,"+;
			"placbirth=?lplacbirth,marystat=?lmarystat,gender=?lgender,stitle=?lstitle,ethny=?lethny,religion=?lreligion,fileref=?lfileref,report2=?lReport2,nhif=?.text21.value,ssno=?.text6.value,pin=?.text8.value,"+;
			"grade=?.text22.value,grade_poin=?.text26.value,nbankid=?lbnk_ID,bbranch_id=?lBnkBr_ID,bnk_acct=?lcBnkAcct,emailnb=?lcEmail,nokfname=?lcNOKFname,noklname=?lcNOKLname, "+;
			"nokrelation=?lcNOKRel,noktel=?lcNOKPhone,mobilenb=?lcMobile,nbandid=?lnBandid,dob=?lDob,doe=?lDoe,"+;
			"dad_name=?lcdad_name,dad_addr=?lcdad_addr,dad_nan=?lndad_nan,dad_dec=?lndad_dec,mum_name=?lcmum_name,mum_addr=?lcmum_addr,mum_nan=?lnmum_nan,mum_dec=?lnmum_dec where staffno=?gcStaffNo","staffupd")
		If sn<0
			=sysmsg('could not update employee details')
		Endif
	Endif
	gcInputstat='E'
*	Store .T. To .command16.Enabled,.command17.Enabled,.command18.Enabled,.command19.Enabled,.command20.Enabled,.command21.Enabled
	Store .F. To .text21.Enabled,.text20.Enabled, .combo10.Enabled,.combo11.Enabled,.combo9.Enabled,.combo8.Enabled,.text8.Enabled,.text6.Enabled,.text12.Enabled,;
		.combo5.Enabled,.combo7.Enabled,.text1.Enabled,.text3.Enabled,.combo3.Enabled,.text4.Enabled,.text5.Enabled,.text14.Enabled,.text15.Enabled,;
		.text22.Enabled,.text26.Enabled,.text25.Enabled,.text27.Enabled
	Store .F. To .command1.Enabled,.command10.Enabled,.command15.Enabled,.command2.Enabled,.command3.Enabled,.command6.Enabled,.command7.Enabled,.command8.Enabled,;
		.text21.Enabled,.text20.Enabled, .combo10.Enabled,.combo11.Enabled,.combo9.Enabled,.combo8.Enabled,.text8.Enabled,.text6.Enabled,.text12.Enabled,;
		.combo5.Enabled,.combo7.Enabled,.text3.Enabled,.combo3.Enabled,.text4.Enabled,.text5.Enabled,.text14.Enabled,.text15.Enabled,;
		.text22.Enabled,.text26.Enabled,.text25.Enabled,.text27.Enabled,.combo1.Enabled,.combo4.Enabled,.combo6.Enabled,.combo2.Enabled,.command13.Enabled,.command26.Enabled,.command23.Enabled,;
		.command25.Enabled,.combo12.Enabled,.combo14.Enabled,.combo13.Enabled,.text2.Enabled,.command18.Enabled,.command19.Enabled,.command20.Enabled,.command21.Enabled

*****************************
	Store 0 To .text16.Value,.text22.Value,.text26.Value,.combo6.Value,.combo3.Value,.combo9.Value,.combo1.Value,.combo10.Value,.combo11.Value,.combo5.Value,.combo8.Value,;
		.combo7.Value,.combo4.Value,.combo2.Value,.combo12.Value,.combo12.Value,.combo14.Value,.combo13.Value
	Store {} To .text14.Value,.text25.Value,.text27.Value,.text17.Value
	.text18.Value='Active'
	Store '' To .text21.Value,.text8.Value,.text6.Value,.text1.Value,.text3.Value,.text4.Value,.text5.Value,.text15.Value,.image1.Picture,;
		.text20.Value,.text2.Value,.text12.Value,.text18.Value
	With Thisform.pageframe1
		With .page1
			Store '' To .edit1.Value,.text1.Value,.text2.Value,.text11.Value,.text3.Value,.text7.Value,.text4.Value,.edit2.Value
			Store 0 To .combo1.Value,.combo6.Value
		Endwith
		With .page2
			Store '' To .edit1.Value,.text1.Value,.text2.Value,.text3.Value,.text5.Value,.text6.Value,.text4.Value,.text11.Value
			Store 0 To .combo1.Value,.combo2.Value,.combo6.Value
		Endwith
		With .page3
			Store '' To .edit1.Value,.text3.Value,.text4.Value
			Store 0 To .combo6.Value
		Endwith
		With .page4
			Store '' To .text1.Value,.text2.Value,.text3.Value,.text5.Value
			Store 0 To .combo1.Value,.combo2.Value
			Store .F. To .check1.Value,.check2.Value
		Endwith

		.Refresh
	Endwith
**********************
	.Refresh
Endwith

             */
        }

        private void tabPage9_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void comboBox14_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox14.Focused)
            {
                page01ok();
            }
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox10.Focused)
            {
                page01ok();
            }
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox11.Focused)
            {
                page01ok();
            }
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox9.Focused)
            {
                page01ok();
            }
        }

        private void comboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox16.Focused)
            {
                page01ok();
            }
        }

        private void comboBox18_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox18.Focused)
            {
                page01ok();
            }
        }


        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox12.Focused)
            {
     //           gettown(Convert.ToInt16(comboBox12.SelectedValue));
                page01ok();
            }
        }

        private void textBox9_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox8_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox11_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox5_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox14_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox21_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox19_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox17_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox10_Validated(object sender, EventArgs e)
        {
            page02ok();
        }

        private void textBox12_Validated(object sender, EventArgs e)
        {
            page02ok();
        }

        private void textBox16_Validated(object sender, EventArgs e)
        {
            page02ok();
        }

        private void textBox13_Validated(object sender, EventArgs e)
        {
            page02ok();
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            page02ok();
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            page02ok();
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            page02ok();
        }

        private void textBox4_Validated(object sender, EventArgs e)
        {
            page02ok();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Focused)
            {
                page02ok();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Focused)
            {
                page02ok();
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.Focused)
            {
                page02ok();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Focused)
            {
                page02ok();
            }
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox8.Focused)
            {
                page02ok();
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.Focused)
            {
                page02ok();
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.Focused)
            {
                page02ok();
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.Focused)
            {
                page02ok();
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked)
            {
                getfiltview(2);
            }
        }

        private void comboBox17_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox17.Focused)
            {
                int dfilt = (radioButton8.Checked ? 0 : (radioButton14.Checked ? 1 : (radioButton9.Checked ? 2 : (radioButton13.Checked ? 3 : (radioButton10.Checked ? 4 : (radioButton11.Checked ? 5 :
                    (radioButton15.Checked ? 6 : 7)))))));
                getstaffByFilter(dfilt);
            }
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton16.Checked)
            {
                textBox47.Enabled = true;
                comboBox17.Enabled = false;
            }
            else
            {
                textBox47.Enabled = false;
                comboBox17.Enabled = true;
            }

        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton14.Checked)
            {
                getfiltview(1);
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

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            comboBox17.Enabled = radioButton8.Checked || radioButton16.Checked ? false : true;
            getstaff();
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

        private void ClientGrid_Click(object sender, EventArgs e)
        {
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();

            textBox46.Text = tcemp;
            textBox48.Text = tcempname;

            textBox50.Text = tcemp;
            textBox49.Text = tcempname;

            textBox23.Text = tcemp;
            textBox31.Text = tcempname;

            textBox51.Text = tcemp;
            textBox52.Text = tcempname;

            textBox53.Text = tcemp;
            textBox54.Text = tcempname;

            textBox39.Text = tcemp;
            textBox36.Text = tcempname;

            textBox42.Text = tcemp;
            textBox41.Text = tcempname;

            persEmpDetails(tcemp);
            persLanDetails(tcemp);
            persNokDetails(tcemp);
            persdisDetails(tcemp);
            empHistDetails(tcemp);
            empPriDetails(tcemp);
            empPayDetails(tcemp);
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            getnokDetails();
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox50.Text = tcemp;
            textBox49.Text = tcempname;
            persNokDetails(tcemp);
        }

        private void getnokDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string noksql0 = "select tit_name,tit_id from titres order by tit_name";
                SqlDataAdapter nokbase0 = new SqlDataAdapter(noksql0, ndConnHandle);
                DataTable titview = new DataTable();
                nokbase0.Fill(titview);
                if (titview.Rows.Count > 0)
                {
                    comboBox19.DataSource = titview.DefaultView;
                    comboBox19.DisplayMember = "tit_name";
                    comboBox19.ValueMember = "tit_id";
                    comboBox19.SelectedIndex = -1;
                }

                string noksql1 = "select id_name,idtype from id_type order by id_name";
                SqlDataAdapter nokbase1 = new SqlDataAdapter(noksql1, ndConnHandle);
                DataTable idview = new DataTable();
                nokbase1.Fill(idview);
                if (idview.Rows.Count > 0)
                {
                    comboBox15.DataSource = idview.DefaultView;
                    comboBox15.DisplayMember = "id_name";
                    comboBox15.ValueMember = "idtype";
                    comboBox15.SelectedIndex = -1;
                }

                string noksql2 = "select cou_name,cou_id,idd_code from country order by cou_name";
                SqlDataAdapter nokbase2 = new SqlDataAdapter(noksql2, ndConnHandle);
                DataTable couview = new DataTable();
                nokbase2.Fill(couview);
                if (couview.Rows.Count > 0)
                {
                    comboBox21.DataSource = couview.DefaultView;
                    comboBox21.DisplayMember = "cou_name";
                    comboBox21.ValueMember = "cou_id";
                    comboBox21.SelectedIndex = -1;
                }

                string noksql3 = "exec tsp_Memb_relation " + ncompid; ;
                SqlDataAdapter nokbase3 = new SqlDataAdapter(noksql3, ndConnHandle);
                DataTable relview = new DataTable();
                nokbase3.Fill(relview);
                if (relview.Rows.Count > 0)
                {
                    comboBox45.DataSource = relview.DefaultView;
                    comboBox45.DisplayMember = "rel_name";
                    comboBox45.ValueMember = "rel_id";
                    comboBox45.SelectedIndex = -1;
                }
            }
        }
    


        private void comboBox21_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox21.Focused)
            {
                getnoktown(Convert.ToInt16(comboBox21.SelectedValue));
            }
            page03ok();
        }

        private void nokSaveButton_Click(object sender, EventArgs e)
        {
            updNokDetails();
            initNokvariables();
        }

        private void initNokvariables()
        {
            textBox22.Text = textBox27.Text = textBox28.Text = textBox29.Text = textBox30.Text = textBox26.Text = textBox15.Text =  "";
            comboBox15.SelectedIndex = comboBox19.SelectedIndex = comboBox20.SelectedIndex = comboBox21.SelectedIndex =  -1;
        }

        private void comboBox19_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox19.Focused )
            {
                page03ok();
            }
        }

        private void comboBox21_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox20_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox20.Focused)
            {
                page03ok();
            }

        }

        private void comboBox15_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox15.Focused)
            {
                page03ok();
            }

        }

        private void comboBox45_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox45.Focused)
            {
                page03ok();
            }
        }

        private void textBox28_Validated(object sender, EventArgs e)
        {
            page03ok();
        }

        private void textBox29_Validated(object sender, EventArgs e)
        {
            page03ok();
        }

        private void richTextBox4_Validated(object sender, EventArgs e)
        {
            page03ok();
        }

        private void textBox27_Validated(object sender, EventArgs e)
        {
            page03ok();
        }

        private void textBox30_Validated(object sender, EventArgs e)
        {
            page03ok();
        }

        private void textBox22_Validated(object sender, EventArgs e)
        {
            page03ok();
        }

        private void textBox26_Validated(object sender, EventArgs e)
        {
            page03ok();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox32_Validated(object sender, EventArgs e)
        {
            page04ok();
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            getdisDetails();
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox23.Text = tcemp;
            textBox31.Text = tcempname;
            persdisDetails(tcemp);
        }


        private void getdisDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dissql2 = "select cou_name,cou_id,idd_code from country order by cou_name";
                SqlDataAdapter nokbase2 = new SqlDataAdapter(dissql2, ndConnHandle);
                DataTable couview0 = new DataTable();
                nokbase2.Fill(couview0);
                if (couview0.Rows.Count > 0)
                {
                    comboBox48.DataSource = couview0.DefaultView;
                    comboBox48.DisplayMember = "cou_name";
                    comboBox48.ValueMember = "cou_id";
                    comboBox48.SelectedIndex = -1;
                }

                string dissql21 = "select cou_name,cou_id,idd_code from country order by cou_name";
                SqlDataAdapter nokbase21 = new SqlDataAdapter(dissql21, ndConnHandle);
                DataTable couview1 = new DataTable();
                nokbase21.Fill(couview1);
                if (couview1.Rows.Count > 0)
                {
                    comboBox22.DataSource = couview1.DefaultView;
                    comboBox22.DisplayMember = "cou_name";
                    comboBox22.ValueMember = "cou_id";
                    comboBox22.SelectedIndex = -1;
                }

                string dissql22 = "select cou_name,cou_id,idd_code from country order by cou_name";
                SqlDataAdapter nokbase22 = new SqlDataAdapter(dissql22, ndConnHandle);
                DataTable couview2 = new DataTable();
                nokbase22.Fill(couview2);
                if (couview2.Rows.Count > 0)
                {
                    comboBox50.DataSource = couview2.DefaultView;
                    comboBox50.DisplayMember = "cou_name";
                    comboBox50.ValueMember = "cou_id";
                    comboBox50.SelectedIndex = -1;
                }


                string dissql23 = "select cou_name,cou_id,idd_code from country order by cou_name";
                SqlDataAdapter nokbase23 = new SqlDataAdapter(dissql23, ndConnHandle);
                DataTable couview3 = new DataTable();
                nokbase23.Fill(couview3);
                if (couview3.Rows.Count > 0)
                {
                    comboBox25.DataSource = couview3.DefaultView;
                    comboBox25.DisplayMember = "cou_name";
                    comboBox25.ValueMember = "cou_id";
                    comboBox25.SelectedIndex = -1;
                }


                string dissql4 = "select dis_name,dis_id from disa_type order by dis_name";
                SqlDataAdapter disbase4 = new SqlDataAdapter(dissql4, ndConnHandle);
                DataTable disview = new DataTable();
                disbase4.Fill(disview);
                if (disview.Rows.Count > 0)
                {
                    comboBox23.DataSource = disview.DefaultView;
                    comboBox23.DisplayMember = "dis_name";
                    comboBox23.ValueMember = "dis_id";
                    comboBox23.SelectedIndex = -1;
                }


                string dissql5 = "select acq_time, acq_id from disa_time order by acq_time";
                SqlDataAdapter nokbase5 = new SqlDataAdapter(dissql5, ndConnHandle);
                DataTable aqview = new DataTable();
                nokbase5.Fill(aqview);
                if (aqview.Rows.Count > 0)
                {
                    comboBox46.DataSource = aqview.DefaultView;
                    comboBox46.DisplayMember = "acq_time";
                    comboBox46.ValueMember = "acq_id";
                    comboBox46.SelectedIndex = -1;
                }


            }
        }

        private void showTown(int tncouid,int tncityid,int ntime)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                cityview.Clear();
                string basesql0 = "select city_name,city_id,cou_id from city where cou_id = " + tncouid + " order by city_name";
                SqlDataAdapter baseda0 = new SqlDataAdapter(basesql0, ndConnHandle);
                baseda0.Fill(defaCityview);
                if (defaCityview.Rows.Count > 0)
                {
                    switch (ntime)
                    {
                        case 1:
                            comboBox20.DataSource = defaCityview.DefaultView;
                            comboBox20.DisplayMember = "city_name";
                            comboBox20.ValueMember = "city_id";
                            comboBox20.SelectedValue = tncityid;
                            break;
                        case 2:
                            comboBox47.DataSource = defaCityview.DefaultView;
                            comboBox47.DisplayMember = "city_name";
                            comboBox47.ValueMember = "city_id";
                            comboBox47.SelectedValue = tncityid;
                            break;
                        case 3:
                            comboBox49.DataSource = defaCityview.DefaultView;
                            comboBox49.DisplayMember = "city_name";
                            comboBox49.ValueMember = "city_id";
                            comboBox49.SelectedValue = tncityid;
                            break;
                    }
                }
                //else { MessageBox.Show("Cities not yet defined for country code " + tncouid); }
            }
        }
        private void persdisDetails(string tcStaffNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string empsql = "exec tsp_getActiveStaff_One " + ncompid + ",'" + tcStaffNo + "'";
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                DataTable disaview = new DataTable();
                daemp.Fill(disaview);
                if (disaview.Rows.Count > 0)
                {
                    textBox32.Text = !Convert.IsDBNull(disaview.Rows[0]["dad_name"]) ? disaview.Rows[0]["dad_name"].ToString() : ""; 
                    textBox24.Text = !Convert.IsDBNull(disaview.Rows[0]["mum_name"]) ? disaview.Rows[0]["mum_name"].ToString() : "";
                    acquireDate.Value = !Convert.IsDBNull(disaview.Rows[0]["disa_date"]) ? Convert.ToDateTime(disaview.Rows[0]["disa_date"]) : DateTime.Now;

                    checkBox1.Checked = !Convert.IsDBNull(disaview.Rows[0]["dad_dec"]) ? Convert.ToBoolean(disaview.Rows[0]["dad_dec"]) : false;
                    checkBox2.Checked = !Convert.IsDBNull(disaview.Rows[0]["mum_dec"]) ? Convert.ToBoolean(disaview.Rows[0]["mum_dec"]) : false;


                    if (!Convert.IsDBNull(disaview.Rows[0]["dad_couid"]))
                    {
                        comboBox48.SelectedValue = Convert.ToInt16(disaview.Rows[0]["dad_couid"]);
                        showTown(Convert.ToInt16(disaview.Rows[0]["dad_couid"]), Convert.ToInt16(disaview.Rows[0]["dad_cityid"]), 2);
                    }
                    comboBox22.SelectedValue = !Convert.IsDBNull(disaview.Rows[0]["dad_nan"]) ? Convert.ToInt16(disaview.Rows[0]["dad_nan"]) : 0;

                    if(!Convert.IsDBNull(disaview.Rows[0]["mum_couid"]))
                    {
                        comboBox50.SelectedValue = Convert.ToInt16(disaview.Rows[0]["mum_couid"]);
                        showTown(Convert.ToInt16(disaview.Rows[0]["mum_couid"]), Convert.ToInt16(disaview.Rows[0]["mum_cityid"]), 3);
                    }
                    comboBox25.SelectedValue = !Convert.IsDBNull(disaview.Rows[0]["mum_nan"]) ? Convert.ToInt16(disaview.Rows[0]["mum_nan"]) : 0;

                    comboBox23.SelectedValue = !Convert.IsDBNull(disaview.Rows[0]["disa_type"]) ? Convert.ToInt16(disaview.Rows[0]["disa_type"]) : 0;
                    comboBox46.SelectedValue = !Convert.IsDBNull(disaview.Rows[0]["disa_time"]) ? Convert.ToInt16(disaview.Rows[0]["disa_time"]) : 0;
                }
            }
        }

        private void updDisDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string cquery = "update staff set dad_name=@tcdad_name,dad_couid=@tdad_couid,dad_cityid=@tdad_cityid,dad_nan=@tdad_nan,dad_dec=@tdad_dec,mum_name=@tmum_name,mum_couid=@tmum_couid,mum_cityid=@tmum_cityid,";
                cquery += "mum_nan = @tmum_nan,mum_dec = @tmum_dec,disa_type = @tdisa_type,disa_time = @tdisa_time,disa_date = @tdisa_date where staffno = @tlstaffno";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.UpdateCommand.Parameters.Add("@tlstaffno", SqlDbType.VarChar).Value = textBox23.Text.ToString().Trim();

                cuscommand.UpdateCommand.Parameters.Add("@tcdad_name", SqlDbType.VarChar).Value = textBox32.Text.ToString().Trim();
                cuscommand.UpdateCommand.Parameters.Add("@tdad_couid", SqlDbType.Int).Value = Convert.ToInt16(comboBox48.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tdad_cityid", SqlDbType.Int).Value = Convert.ToInt16(comboBox47.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tdad_nan", SqlDbType.Int).Value = Convert.ToInt16(comboBox22.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tdad_dec", SqlDbType.Bit).Value = checkBox1.Checked;

                cuscommand.UpdateCommand.Parameters.Add("@tmum_name", SqlDbType.VarChar).Value = textBox24.Text.ToString().Trim();
                cuscommand.UpdateCommand.Parameters.Add("@tmum_couid", SqlDbType.Int).Value = Convert.ToInt16(comboBox50.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tmum_cityid", SqlDbType.Int).Value = Convert.ToInt16(comboBox49.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tmum_nan", SqlDbType.Int).Value = Convert.ToInt16(comboBox25.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tmum_dec", SqlDbType.Bit).Value = checkBox2.Checked;

                cuscommand.UpdateCommand.Parameters.Add("@tdisa_type", SqlDbType.Int).Value = Convert.ToInt16(comboBox23.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tdisa_time", SqlDbType.Int).Value = Convert.ToInt16(comboBox46.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tdisa_date", SqlDbType.DateTime).Value = acquireDate.Value;

                ndConnHandle.Open();
                cuscommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Staff parent and disability details updated successfully");
            }
        }

        private void updHisDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {

                if (glNewHistory) //new employment history added
                {
                    string cquery0 = "Insert Into emp_history (staffno,des_id,dep_id,fromdate,todate,grade,gradepoint,postnumb,branchid,compid,empl_type) ";
                    cquery0 += "values (@lstaffno,@ldes_id,@ldep_id,@lfromdate,@ltodate,@lgrade,@lgradepoint,@lpostnumb,@lbranchid,@lcompid,@lempl_type)";
                    SqlDataAdapter hiscommand = new SqlDataAdapter();
                    hiscommand.InsertCommand = new SqlCommand(cquery0, ndConnHandle);

                    hiscommand.InsertCommand.Parameters.Add("@lstaffno", SqlDbType.VarChar).Value = textBox53.Text.ToString().Trim();
                    hiscommand.InsertCommand.Parameters.Add("@ldes_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox31.SelectedValue);
                    hiscommand.InsertCommand.Parameters.Add("@ldep_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox30.SelectedValue);
                    hiscommand.InsertCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = Convert.ToInt16(comboBox32.SelectedValue);
                    hiscommand.InsertCommand.Parameters.Add("@lempl_type", SqlDbType.Int).Value = Convert.ToInt16(comboBox26.SelectedValue);
                    
                    hiscommand.InsertCommand.Parameters.Add("@lfromdate", SqlDbType.DateTime).Value = Convert.ToDateTime(empFromDate.Value);
                    hiscommand.InsertCommand.Parameters.Add("@ltodate", SqlDbType.DateTime).Value = Convert.ToDateTime(empToDate.Value);
                    hiscommand.InsertCommand.Parameters.Add("@lgrade", SqlDbType.Decimal).Value = textBox34.Text != "" ? Convert.ToInt16(textBox34.Text) : 0; 
                    hiscommand.InsertCommand.Parameters.Add("@lgradepoint", SqlDbType.Decimal).Value = textBox35.Text != "" ? Convert.ToInt16(textBox35.Text) : 0;
                    hiscommand.InsertCommand.Parameters.Add("@lpostnumb", SqlDbType.Int).Value = Convert.ToInt16(textBox33.Text);
                    hiscommand.InsertCommand.Parameters.Add("@lcompid", SqlDbType.Int).Value = ncompid;

                    ndConnHandle.Open();
                    hiscommand.InsertCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                    MessageBox.Show("Staff employment history added successfully");
                }
                else             //employment history amended with biometric and one cannot change own
                {
//                    MessageBox.Show("We will be amending an old record");
                    string cquery1 = "update emp_history set des_id = @ldes_id,dep_id = @ldep_id,fromdate = @lfromdate,todate = @ltodate,grade = @lgrade,";
                    cquery1+= " gradepoint = @lgradepoint,branchid=@lbranchid,empl_type = @lempl_typer where staffno = @lstaffno and postnumb =  @lpostnumb";

                    SqlDataAdapter hiscommand1 = new SqlDataAdapter();
                    hiscommand1.UpdateCommand = new SqlCommand(cquery1, ndConnHandle);

                    hiscommand1.UpdateCommand.Parameters.Add("@lstaffno", SqlDbType.VarChar).Value = textBox53.Text.ToString().Trim();
                    hiscommand1.UpdateCommand.Parameters.Add("@ldes_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox31.SelectedValue);
                    hiscommand1.UpdateCommand.Parameters.Add("@ldep_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox30.SelectedValue);
                    hiscommand1.UpdateCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = Convert.ToInt16(comboBox32.SelectedValue);
                    hiscommand1.UpdateCommand.Parameters.Add("@lempl_type", SqlDbType.Int).Value = Convert.ToInt16(comboBox26.SelectedValue);

                    hiscommand1.UpdateCommand.Parameters.Add("@lfromdate", SqlDbType.DateTime).Value = Convert.ToDateTime(empFromDate.Value);
                    hiscommand1.UpdateCommand.Parameters.Add("@ltodate", SqlDbType.DateTime).Value = Convert.ToDateTime(empToDate.Value);
                    hiscommand1.UpdateCommand.Parameters.Add("@lgrade", SqlDbType.Decimal).Value = textBox34.Text != "" ? Convert.ToInt16(textBox34.Text) : 0;
                    hiscommand1.UpdateCommand.Parameters.Add("@lgradepoint", SqlDbType.Decimal).Value = textBox35.Text != "" ? Convert.ToInt16(textBox35.Text) : 0;
                    hiscommand1.UpdateCommand.Parameters.Add("@lpostnumb", SqlDbType.Int).Value = Convert.ToInt16(textBox33.Text);

                    ndConnHandle.Open();
                    hiscommand1.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                    MessageBox.Show("Staff employment history updated successfully");
                }
            }
        }

        private void empHistDetails(string tcStaffNo)
        {
            empHisview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string empsql = "exec tsp_empHist " + ncompid + ",'" + tcStaffNo + "'";
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                daemp.Fill(empHisview);
                if (empHisview.Rows.Count > 0)
                {
                    empHistGrid.AutoGenerateColumns = false;
                    empHistGrid.DataSource = empHisview.DefaultView;
                    empHistGrid.Columns[0].DataPropertyName = "dep_name";
                    empHistGrid.Columns[1].DataPropertyName = "des_name";
                    empHistGrid.Columns[2].DataPropertyName = "br_name";
                    empHistGrid.Columns[3].DataPropertyName = "fromdate";
                    empHistGrid.Columns[4].DataPropertyName = "todate";
                    empHistGrid.Columns[5].DataPropertyName = "postnumb";
                }
                //else
                //{
                //    textBox33.Text = textBox34.Text = textBox35.Text = "";
                //    comboBox30.SelectedValue = comboBox31.SelectedValue = comboBox32.SelectedValue = 0;
                //    empFromDate.Value = empToDate.Value = DateTime.Now;
                //}
            }
        }
        private void empHistEditDetails(string tcStaffNo,int tnpostno)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string empsql = "exec tsp_empHist_one " + ncompid + ",'" + tcStaffNo + "',"+tnpostno;
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                DataTable empview = new DataTable();
                daemp.Fill(empview);
                if (empview.Rows.Count > 0)
                {
                    textBox33.Text = !Convert.IsDBNull(empview.Rows[0]["postnumb"]) ? empview.Rows[0]["postnumb"].ToString() : "";
                    textBox34.Text = !Convert.IsDBNull(empview.Rows[0]["grade"]) ? Convert.ToInt16(empview.Rows[0]["grade"]).ToString() : "";
                    textBox35.Text = !Convert.IsDBNull(empview.Rows[0]["gradepoint"]) ? Convert.ToInt16(empview.Rows[0]["gradepoint"]).ToString() : "";
                    comboBox30.SelectedValue = !Convert.IsDBNull(empview.Rows[0]["dep_id"]) ? Convert.ToInt16(empview.Rows[0]["dep_id"]) : 0;
                    comboBox31.SelectedValue = !Convert.IsDBNull(empview.Rows[0]["des_id"]) ? Convert.ToInt16(empview.Rows[0]["des_id"]) : 0;
                    comboBox32.SelectedValue = !Convert.IsDBNull(empview.Rows[0]["branchid"]) ? Convert.ToInt16(empview.Rows[0]["branchid"]) : 0;
                    comboBox26.SelectedValue = !Convert.IsDBNull(empview.Rows[0]["empl_type"]) ? Convert.ToInt16(empview.Rows[0]["empl_type"]) : 0;
                    empFromDate.Value = !Convert.IsDBNull(empview.Rows[0]["fromdate"]) ? Convert.ToDateTime(empview.Rows[0]["fromdate"]) : DateTime.Now;
                    empToDate.Value = !Convert.IsDBNull(empview.Rows[0]["todate"]) ? Convert.ToDateTime(empview.Rows[0]["todate"]) : DateTime.Now;
                }
                else
                {
                    textBox33.Text = textBox34.Text = textBox35.Text = "";
                    comboBox30.SelectedValue = comboBox31.SelectedValue = comboBox32.SelectedValue = 0;
                    empFromDate.Value = empToDate.Value = DateTime.Now;
                }
            }
        }

        private void comboBox48_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox48.Focused)
            {
                getHomeTown(Convert.ToInt16(comboBox48.SelectedValue),1);
            }
            page04ok();
        }

        private void comboBox50_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox50.Focused )
            {
                getHomeTown(Convert.ToInt16(comboBox50.SelectedValue), 2);
            }
            page04ok();
        }

        private void getHomeTown(int tncouid,int ntype)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                if(ntype ==1) //father's home town
                {
                    fhomeview.Clear();
                    string basesql0 = "select city_name,city_id,cou_id from city where cou_id = " + tncouid + " order by city_name";
                    SqlDataAdapter baseda0 = new SqlDataAdapter(basesql0, ndConnHandle);
                    baseda0.Fill(fhomeview);
                    if (fhomeview.Rows.Count > 0)
                    {
                        comboBox47.DataSource = fhomeview.DefaultView;
                        comboBox47.DisplayMember = "city_name";
                        comboBox47.ValueMember = "city_id";
                        comboBox47.SelectedIndex = -1;
                    }
                }
                else
                {
                    mhomeview.Clear();
                    string basesql0 = "select city_name,city_id,cou_id from city where cou_id = " + tncouid + " order by city_name";
                    SqlDataAdapter baseda0 = new SqlDataAdapter(basesql0, ndConnHandle);
                    baseda0.Fill(mhomeview);
                    if (mhomeview.Rows.Count > 0)
                    {
                        comboBox49.DataSource = mhomeview.DefaultView;
                        comboBox49.DisplayMember = "city_name";
                        comboBox49.ValueMember = "city_id";
                        comboBox49.SelectedIndex = -1;
                    }
                }
            }
        }

        private void comboBox47_SelectedValueChanged(object sender, EventArgs e)
        {
            page04ok();
        }

        private void comboBox22_SelectedValueChanged(object sender, EventArgs e)
        {
            page04ok();
        }

        private void textBox24_Validated(object sender, EventArgs e)
        {
            page04ok();
        }

        private void comboBox49_SelectedValueChanged(object sender, EventArgs e)
        {
            page04ok();
        }

        private void comboBox25_SelectedValueChanged(object sender, EventArgs e)
        {
            page04ok();
        }

        private void comboBox23_SelectedValueChanged(object sender, EventArgs e)
        {
            page04ok();
        }

        private void comboBox46_SelectedValueChanged(object sender, EventArgs e)
        {
            page04ok();
        }

        private void acquireDate_ValueChanged(object sender, EventArgs e)
        {
            page04ok();
        }

        private void disSaveButton_Click(object sender, EventArgs e)
        {
            updDisDetails();
        }

        private void tabPage11_Enter(object sender, EventArgs e)
        {
            getlanDetails();
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox51.Text = tcemp;
            textBox52.Text = tcempname;
            persLanDetails(tcemp);

        }

        private void getlanDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string basesql00 = "exec tsp_langtypes  ";
                SqlDataAdapter baseda00 = new SqlDataAdapter(basesql00, ndConnHandle);
                DataTable lanview = new DataTable();
                baseda00.Fill(lanview);
                if (lanview.Rows.Count > 0)
                {
                    comboBox38.DataSource = lanview.DefaultView;
                    comboBox38.DisplayMember = "lan_name";
                    comboBox38.ValueMember = "lan_id";
                    comboBox38.SelectedIndex = -1;
                }
            }
        }

        private void persLanDetails(string tcStaffNo)
        {
            lanview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string lansql = "exec tsp_lang " + "'" + tcStaffNo + "'";
                SqlDataAdapter dalan = new SqlDataAdapter(lansql, ndConnHandle);
                dalan.Fill(lanview);
                if (lanview.Rows.Count > 0)
                {
                    lanGrid.AutoGenerateColumns = false;
                    lanGrid.DataSource = lanview.DefaultView;
                    lanGrid.Columns[0].DataPropertyName = "lan_name";
                    lanGrid.Columns[1].DataPropertyName = "lanLevel";
                    lanGrid.Columns[2].DataPropertyName = "lan_id";
                }
            }
        }



        private void tabPage7_Enter(object sender, EventArgs e)
        {
            gethisdetails();
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox53.Text = tcemp;
            textBox54.Text = tcempname;
            textBox33.Text = getPostNumb(tcemp).ToString();
            empHistDetails(tcemp);
            empHistGrid.Focus();
        }

        private int getPostNumb(string tcStaffNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                int dnewPost = 0;
                string basesql1312 = "select postnumb from emp_history where compid = "+ncompid+" and staffno = "+"'"+tcStaffNo+"'"+" order by postnumb desc";
                SqlDataAdapter baseda1312 = new SqlDataAdapter(basesql1312, ndConnHandle);
                DataTable depview2 = new DataTable();
                baseda1312.Fill(depview2);
                if (depview2.Rows.Count > 0)
                {
                    dnewPost = Convert.ToInt16(depview2.Rows[0]["postnumb"]) + 1;
                    return dnewPost;
                }
                else
                {
                    dnewPost = 1;
                    return dnewPost;
                }
            }
        }

        private void gethisdetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string basesql131 = "select dep_name,dep_id from dept order by dep_name";
                SqlDataAdapter baseda131 = new SqlDataAdapter(basesql131, ndConnHandle);
                DataTable depview = new DataTable();
                baseda131.Fill(depview);
                if (depview.Rows.Count > 0)
                {
                    comboBox30.DataSource = depview.DefaultView;
                    comboBox30.DisplayMember = "dep_name";
                    comboBox30.ValueMember = "dep_id";
                    comboBox30.SelectedIndex = -1;
                }

                string desigsql = "select des_name,des_id from designation order by des_name";
                SqlDataAdapter dadesign = new SqlDataAdapter(desigsql, ndConnHandle);
                DataTable desview = new DataTable();
                dadesign.Fill(desview);
                if (desview.Rows.Count > 0)
                {
                    comboBox31.DataSource = desview.DefaultView;
                    comboBox31.DisplayMember = "des_name";
                    comboBox31.ValueMember = "des_id";
                    comboBox31.SelectedIndex = -1;
                }

                string basesql130 = "select br_name,branchid from branch where compobnk=1 order by br_name";
                SqlDataAdapter baseda130 = new SqlDataAdapter(basesql130, ndConnHandle);
                DataTable brview = new DataTable();
                baseda130.Fill(brview);
                if (brview.Rows.Count > 0)
                {
                    comboBox32.DataSource = brview.DefaultView;
                    comboBox32.DisplayMember = "br_name";
                    comboBox32.ValueMember = "branchid";
                    comboBox32.SelectedIndex = -1;
                }

                string basesql132 = "select emp_name,emp_id from empl_type order by emp_name";
                SqlDataAdapter baseda132 = new SqlDataAdapter(basesql132, ndConnHandle);
                DataTable emptypeview = new DataTable();
                baseda132.Fill(emptypeview);
                if (emptypeview.Rows.Count > 0)
                {
                    comboBox26.DataSource = emptypeview.DefaultView;
                    comboBox26.DisplayMember = "emp_name";
                    comboBox26.ValueMember = "emp_id";
                    comboBox26.SelectedIndex = -1;
                }
            }
        }
        private void page07ok() //employment history 
        {
            bool dempH = comboBox30.SelectedIndex > -1 && comboBox31.SelectedIndex > -1 && comboBox32.SelectedIndex > - 1 && comboBox26.SelectedIndex > -1 ? true : false;
            bool dempD = empFromDate.Value <= empToDate.Value ? true : false;
            if (textBox33.Text != "" && dempH && dempD)
            {
                hisSaveButton.Enabled = true;
                hisSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                hisSaveButton.Enabled = false;
                hisSaveButton.BackColor = Color.Gainsboro;
            }
        }


        private void comboBox38_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox38.Focused)
            {
                page11ok();
            }
        }


        private void page11ok() //Language ability 
        {
            if (comboBox38.SelectedIndex > -1 && (radioButton6.Checked || radioButton7.Checked || radioButton5.Checked )) 
            {
                lanSaveButton.Enabled = true;
                lanSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                lanSaveButton.Enabled = false;
                lanSaveButton.BackColor = Color.Gainsboro;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            page11ok();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            page11ok();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            page11ok();
        }

        private void lanSaveButton_Click(object sender, EventArgs e)
        {
            glNewLang = checkNewLan(textBox51.Text.ToString().Trim());
            updateLanguage();
            radioButton5.Checked = radioButton6.Checked = radioButton7.Checked = false;
            comboBox38.SelectedIndex = -1;
            persLanDetails(textBox51.Text.ToString().Trim());
            glNewLang = false;

        }

        private bool checkNewLan(string tcStaffNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                bool tlNewCheck = false;
                string dupsql = "select 1 from languages where staffno = " + "'" + tcStaffNo + "'" + " and lan_id = " + Convert.ToInt16(comboBox38.SelectedValue);
                SqlDataAdapter dupcheck = new SqlDataAdapter(dupsql, ndConnHandle);
                DataTable dupview = new DataTable();
                dupcheck.Fill(dupview);
                if (dupview.Rows.Count > 0)
                {
                    tlNewCheck = false;
                    return tlNewCheck;
                }
                else
                {
                    tlNewCheck = true;
                    return tlNewCheck;
                }
            }
        }


        private void updateLanguage()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                int lnLanLevel = (radioButton6.Checked ? 1 : (radioButton5.Checked ? 2 : 3));
//                if (gcInputstat == "N")
                if(glNewLang)
                {
                    string cquery = "insert into languages (staffno,lan_id,lan_level,compid) values (@tcStaffNo,@tnLanid,@tnLanLevel,@tnCompid)";
                    
                    SqlDataAdapter cuscommand = new SqlDataAdapter();
                    cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                    cuscommand.InsertCommand.Parameters.Add("@tcStaffNo", SqlDbType.VarChar).Value = textBox51.Text.Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tnLanid", SqlDbType.Int).Value = Convert.ToInt16(comboBox38.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@tnLanLevel", SqlDbType.Int).Value = lnLanLevel;
                    cuscommand.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;

                    ndConnHandle.Open();
                    cuscommand.InsertCommand.ExecuteNonQuery();  //Insert new record
                    ndConnHandle.Close();
                    MessageBox.Show("New language details added successfully");
                }
                else
                {
                    string cquery = "update languages set lan_level=@tnLanLevel,compid=@tnCompid where staffno=@tcStaffNo and lan_id=@tnLanid";

                    SqlDataAdapter cuscommand = new SqlDataAdapter();
                    cuscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                    cuscommand.UpdateCommand.Parameters.Add("@tcStaffNo", SqlDbType.VarChar).Value = textBox51.Text.Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tnLanid", SqlDbType.Int).Value = Convert.ToInt16(comboBox38.SelectedValue);
                    cuscommand.UpdateCommand.Parameters.Add("@tnLanLevel", SqlDbType.Int).Value = lnLanLevel;
                    cuscommand.UpdateCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;

                    ndConnHandle.Open();
                    cuscommand.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                    MessageBox.Show("Language details updated successfully");
                }
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox33_Validated(object sender, EventArgs e)
        {
            page07ok();
        }

        private void comboBox30_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox30.Focused )
            {
                page07ok();
            }
        }

        private void page13ok()
        {
            bool dempH = comboBox24.SelectedIndex > -1 && comboBox27.SelectedIndex > -1 && comboBox28.SelectedIndex > -1 ? true : false;
            bool dempD = dateAppt.Value <= dateQuit.Value ? true : false;
            if (textBox38.Text != "" && dempH && dempD)
            {
                priSaveButton.Enabled = true;
                priSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                priSaveButton.Enabled = false;
                priSaveButton.BackColor = Color.Gainsboro;
            }
        }
        private void comboBox31_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox31.Focused)
            {
                page07ok();
            }
        }

        private void comboBox32_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox32.Focused)
            {
                page07ok();
            }
        }

        private void empFromDate_Validated(object sender, EventArgs e)
        {
            if (empFromDate.Focused)
            {
                page07ok();
            }
        }

        private void empToDate_Validated(object sender, EventArgs e)
        {
            if (empToDate.Focused)
            {
                page07ok();
            }
        }

        private void hisSaveButton_Click(object sender, EventArgs e)
        {
            glNewHistory = checkNewOld(textBox53.Text.ToString().Trim());
            updHisDetails();
            empHistDetails(textBox53.Text.ToString().Trim());
            hisSaveButton.Enabled = false;
            hisSaveButton.BackColor = Color.Gainsboro;
            glNewHistory = false;
        }

        private bool checkNewOld(string tcStaffNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                bool tlNewCheck = false;
                string dupsql = "select 1 from emp_history where staffno = " + "'" + tcStaffNo + "'"+" and postnumb = "+"'"+textBox33.Text.ToString().Trim()+"'";
                SqlDataAdapter dupcheck = new SqlDataAdapter(dupsql, ndConnHandle);
                DataTable dupview = new DataTable();
                dupcheck.Fill(dupview);
                if (dupview.Rows.Count > 0)
                {
                    tlNewCheck = false;
                    return tlNewCheck;
                }
                else
                {
                    tlNewCheck = true;
                    return tlNewCheck;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            glNewHistory = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void empHistGrid_Click(object sender, EventArgs e)
        {
            string dstaffno = textBox53.Text.ToString().Trim();
           int npostno  = Convert.ToInt16(empHisview.Rows[empHistGrid.CurrentCell.RowIndex]["postnumb"]);
            empHistEditDetails(dstaffno, npostno);
        }

        private void hisNewButton_Click(object sender, EventArgs e)
        {
            textBox33.Text = getPostNumb(textBox53.Text.ToString().Trim()).ToString();
            comboBox30.SelectedIndex = comboBox31.SelectedIndex = comboBox32.SelectedIndex = -1;
            textBox35.Text = textBox34.Text = "";
            empFromDate.Value = empToDate.Value = DateTime.Now;
        }


        private void button3_Click_1(object sender, EventArgs e)
        {
            //		sn=SQLExec(gnConnHandle,"Insert Into secondments (nstaffid,sec_type,sec_num,des_id,fromdate,todate,secComp,remarks) "+;
//            "values (?gnStaffID,?lcSecType,?.text3.Value,?.combo1.Value,?.text6.Value,?.text7.Value,?.text5.value,?.edit1.Value)","insSec")

        }

        private int getSecNumb(string tcStaffNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                int dnewPost = 0;
                string basesql1312 = "select sec_num from secondments where compid = " + ncompid + " and staffno = " + "'" + tcStaffNo + "'" + " order by sec_num desc";
                SqlDataAdapter baseda1312 = new SqlDataAdapter(basesql1312, ndConnHandle);
                DataTable depview2 = new DataTable();
                baseda1312.Fill(depview2);
                if (depview2.Rows.Count > 0)
                {
                    dnewPost = Convert.ToInt16(depview2.Rows[0]["sec_num"]) + 1;
                    return dnewPost;
                }
                else
                {
                    dnewPost = 1;
                    return dnewPost;
                }
            }
        }



        private void comboBox26_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox26.Focused)
            {
                page07ok();
            }
        }

        private void lanNewButton_Click(object sender, EventArgs e)
        {
            gcInputstat = "E";
            radioButton6.Checked = radioButton5.Checked = radioButton7.Checked = false;
            comboBox38.SelectedIndex = -1;
        }

        private void lanGrid_Click(object sender, EventArgs e)
        {
            string dstaffno = textBox51.Text.ToString().Trim();
           int nlanid = Convert.ToInt16(lanview.Rows[lanGrid.CurrentCell.RowIndex]["lan_id"]);
            empLangEditDetails(dstaffno,nlanid);
        }

        private void empLangEditDetails(string tcStaffNo,int tnLanid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string lansql = "exec tsp_lang_one " + ncompid + ",'" + tcStaffNo + "'," + tnLanid;
                SqlDataAdapter dalan = new SqlDataAdapter(lansql, ndConnHandle);
                DataTable emplanview = new DataTable();
                dalan.Fill(emplanview);
                if (emplanview.Rows.Count > 0)
                {
                    comboBox38.SelectedValue = !Convert.IsDBNull(emplanview.Rows[0]["lan_id"]) ? Convert.ToInt16(emplanview.Rows[0]["lan_id"]) : 0;
                    int nlanlev = !Convert.IsDBNull(emplanview.Rows[0]["lan_level"]) ? Convert.ToInt16(emplanview.Rows[0]["lan_level"]) : 0;
                    radioButton6.Checked = nlanlev == 1 ? true : false;
                    radioButton5.Checked = nlanlev == 2 ? true : false;
                    radioButton7.Checked = nlanlev == 3 ? true : false;

                    //lanLevel= case when lan_level=1 then 'Beginner' else case when lan_level =2 then 'Normal' else 'Native Speaker' end end   

                    //comboBox31.SelectedValue = !Convert.IsDBNull(empview.Rows[0]["des_id"]) ? Convert.ToInt16(empview.Rows[0]["des_id"]) : 0;
                    //comboBox32.SelectedValue = !Convert.IsDBNull(empview.Rows[0]["branchid"]) ? Convert.ToInt16(empview.Rows[0]["branchid"]) : 0;
                    //comboBox26.SelectedValue = !Convert.IsDBNull(empview.Rows[0]["empl_type"]) ? Convert.ToInt16(empview.Rows[0]["empl_type"]) : 0;
                    //empFromDate.Value = !Convert.IsDBNull(empview.Rows[0]["fromdate"]) ? Convert.ToDateTime(empview.Rows[0]["fromdate"]) : DateTime.Now;
                    //empToDate.Value = !Convert.IsDBNull(empview.Rows[0]["todate"]) ? Convert.ToDateTime(empview.Rows[0]["todate"]) : DateTime.Now;
                }
                else
                {
                    MessageBox.Show("Languages not yet defined for staff");
                    //textBox33.Text = textBox34.Text = textBox35.Text = "";
                    //comboBox30.SelectedValue = comboBox31.SelectedValue = comboBox32.SelectedValue = 0;
                    //empFromDate.Value = empToDate.Value = DateTime.Now;
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabPage13_Enter(object sender, EventArgs e)
        {
            getpridetails();
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox39.Text = tcemp;
            textBox36.Text = tcempname;
            empPriDetails(tcemp);
            priorGrid.Focus();
        }

        private void getpridetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string basesql131 = "select dep_name,dep_id from dept order by dep_name";
                SqlDataAdapter baseda131 = new SqlDataAdapter(basesql131, ndConnHandle);
                DataTable depview = new DataTable();
                baseda131.Fill(depview);
                if (depview.Rows.Count > 0)
                {
                    comboBox24.DataSource = depview.DefaultView;
                    comboBox24.DisplayMember = "dep_name";
                    comboBox24.ValueMember = "dep_id";
                    comboBox24.SelectedIndex = -1;
                }

                string desigsql = "select des_name,des_id from designation order by des_name";
                SqlDataAdapter dadesign = new SqlDataAdapter(desigsql, ndConnHandle);
                DataTable desview = new DataTable();
                dadesign.Fill(desview);
                if (desview.Rows.Count > 0)
                {
                    comboBox27.DataSource = desview.DefaultView;
                    comboBox27.DisplayMember = "des_name";
                    comboBox27.ValueMember = "des_id";
                    comboBox27.SelectedIndex = -1;
                }

                string basesql130 = "select br_name,branchid from branch where compobnk=1 order by br_name";
                SqlDataAdapter baseda130 = new SqlDataAdapter(basesql130, ndConnHandle);
                DataTable brview = new DataTable();
                baseda130.Fill(brview);
                if (brview.Rows.Count > 0)
                {
                    comboBox28.DataSource = brview.DefaultView;
                    comboBox28.DisplayMember = "br_name";
                    comboBox28.ValueMember = "branchid";
                    comboBox28.SelectedIndex = -1;
                }

            }

        }

        private void empPriDetails(string tcStaffNo)
        {
            empPriorview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string empsql = "exec tsp_empPrior " + ncompid + ",'" + tcStaffNo + "'";
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                daemp.Fill(empPriorview);
                if (empPriorview.Rows.Count > 0)
                {
                    priorGrid.AutoGenerateColumns = false;
                    priorGrid.DataSource = empPriorview.DefaultView;
                    priorGrid.Columns[0].DataPropertyName = "secComp";
                    priorGrid.Columns[1].DataPropertyName = "dep_name";
                    priorGrid.Columns[2].DataPropertyName = "des_name";
                    priorGrid.Columns[3].DataPropertyName = "app_date";
                    priorGrid.Columns[4].DataPropertyName = "quit_date";
                }
            }
        }

        private void textBox38_Validated(object sender, EventArgs e)
        {
                page13ok();
        }

        private void comboBox24_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox24.Focused)
            {
                page13ok();
            }
        }

        private void comboBox27_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox27.Focused)
            {
                page13ok();
            }
        }

        private void comboBox28_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox28.Focused)
            {
                page13ok();
            }
        }

        private void dateAppt_Validated(object sender, EventArgs e)
        {
                page13ok();
        }

        private void dateQuit_Validated(object sender, EventArgs e)
        {
                page13ok();
        }

        private void dremarks_Validated(object sender, EventArgs e)
        {
                page13ok();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void priSaveButton_Click(object sender, EventArgs e)
        {
//            glNewPrior = checkNewOld(textBox53.Text.ToString().Trim());
            updPriDetails();
            empPriDetails(textBox39.Text.ToString().Trim());
            priSaveButton.Enabled = false;
            priSaveButton.BackColor = Color.Gainsboro;
            comboBox24.SelectedIndex = comboBox27.SelectedIndex = comboBox28.SelectedIndex = -1;
            textBox38.Text = dremarks.Text =  "";
            dateAppt.Value = dateQuit.Value = DateTime.Now;
  //          glNewHistory = false;
        }

        private void updPriDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                if (glNewPrior) //new prior employment history added
                {
//                    MessageBox.Show("tHE COMPANY ID IS " + ncompid);
                    string cquery0 = "insert into premploy (staffno,des_id,dep_id,branchid,secComp,app_date,quit_date,nduties,compid) ";
                    cquery0 += "values (@lstaffno,@ldes_id,@ldep_id,@lbranchid,@secComp,@lfromdate,@ltodate,@nduties,@lcompid)";
                    SqlDataAdapter hiscommand = new SqlDataAdapter();
                    hiscommand.InsertCommand = new SqlCommand(cquery0, ndConnHandle);

                    hiscommand.InsertCommand.Parameters.Add("@lstaffno", SqlDbType.VarChar).Value = textBox39.Text.ToString().Trim();
                    hiscommand.InsertCommand.Parameters.Add("@ldes_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox27.SelectedValue);
                    hiscommand.InsertCommand.Parameters.Add("@ldep_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox24.SelectedValue);
                    hiscommand.InsertCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = Convert.ToInt16(comboBox28.SelectedValue);
                    hiscommand.InsertCommand.Parameters.Add("@secComp", SqlDbType.VarChar).Value = textBox38.Text != "" ? textBox38.Text.ToString().Trim() : "";

                    hiscommand.InsertCommand.Parameters.Add("@lfromdate", SqlDbType.DateTime).Value = Convert.ToDateTime(dateAppt.Value);
                    hiscommand.InsertCommand.Parameters.Add("@ltodate", SqlDbType.DateTime).Value = Convert.ToDateTime(dateQuit.Value);

                    hiscommand.InsertCommand.Parameters.Add("@nduties", SqlDbType.VarChar).Value = dremarks.Text != "" ? dremarks.Text.ToString().Trim() : "";
                    hiscommand.InsertCommand.Parameters.Add("@lcompid", SqlDbType.Int).Value = ncompid;

                    ndConnHandle.Open();
                    hiscommand.InsertCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                    MessageBox.Show("Staff Prior Employment added successfully");
                }
                else             //prior employment history amended with biometric and one cannot change own
                {
                    
                     MessageBox.Show("We will be amending an old pre employment  record");
                    string cquery1 = "update premploy set des_id=@ldes_id,dep_id=@ldep_id,branchid=@lbranchid,secComp=@secComp,app_date=@lfromdate,quit_date=@ltodate,nduties=@nduties ";
                    cquery1 += " where staffno = @lstaffno and secComp = @secComp ";

                    SqlDataAdapter hiscommand1 = new SqlDataAdapter();
                    hiscommand1.UpdateCommand = new SqlCommand(cquery1, ndConnHandle);

                    hiscommand1.UpdateCommand.Parameters.Add("@lstaffno", SqlDbType.VarChar).Value = textBox39.Text.ToString().Trim();
                    hiscommand1.UpdateCommand.Parameters.Add("@ldes_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox27.SelectedValue);
                    hiscommand1.UpdateCommand.Parameters.Add("@ldep_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox24.SelectedValue);
                    hiscommand1.UpdateCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = Convert.ToInt16(comboBox28.SelectedValue);
                    hiscommand1.UpdateCommand.Parameters.Add("@secComp", SqlDbType.VarChar).Value = textBox38.Text != "" ? textBox38.Text.ToString().Trim() : "";

                    hiscommand1.UpdateCommand.Parameters.Add("@lfromdate", SqlDbType.DateTime).Value = Convert.ToDateTime(dateAppt.Value);
                    hiscommand1.UpdateCommand.Parameters.Add("@ltodate", SqlDbType.DateTime).Value = Convert.ToDateTime(dateQuit.Value);

                    hiscommand1.UpdateCommand.Parameters.Add("@nduties", SqlDbType.VarChar).Value = dremarks.Text != "" ? dremarks.Text.ToString().Trim() : "";
                    hiscommand1.UpdateCommand.Parameters.Add("@lcompid", SqlDbType.Int).Value = ncompid;

                    ndConnHandle.Open();
                    hiscommand1.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                    MessageBox.Show("Staff Prior Employment updated successfully");
                }
            }
        }

        private void priNewButton_Click(object sender, EventArgs e)
        {
//            textBox33.Text = getPostNumb(textBox53.Text.ToString().Trim()).ToString();
            comboBox24.SelectedIndex = comboBox27.SelectedIndex = comboBox28.SelectedIndex = -1;
            textBox38.Text = "";
            dateQuit.Value = dateAppt.Value = DateTime.Now;
            glNewPrior = true;
        }

        private void priorGrid_Click(object sender, EventArgs e)
        {
            string dstaffno = textBox39.Text.ToString().Trim();
           string dsecComp = empPriorview.Rows[priorGrid.CurrentCell.RowIndex]["secComp"].ToString().Trim();
            empPriorEditDetails(dstaffno,dsecComp);
        }

        private void empPriorEditDetails(string tcStaffNo,string tcsecComp)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
//                MessageBox.Show("We will be showing edit details here");
                string empsql6 = "exec tsp_empPrior_One " + ncompid + ",'" + tcStaffNo + "','" + tcsecComp+"'";
                SqlDataAdapter daemp6 = new SqlDataAdapter(empsql6, ndConnHandle);
                DataTable empview6 = new DataTable();
                daemp6.Fill(empview6);
                if (empview6.Rows.Count > 0)
                {
                    textBox38.Text = !Convert.IsDBNull(empview6.Rows[0]["secComp"]) ? empview6.Rows[0]["secComp"].ToString() : "";
                    dremarks.Text = !Convert.IsDBNull(empview6.Rows[0]["nduties"]) ? empview6.Rows[0]["nduties"].ToString() : "";

                    comboBox24.SelectedValue = !Convert.IsDBNull(empview6.Rows[0]["dep_id"]) ? Convert.ToInt16(empview6.Rows[0]["dep_id"]) : 0;
                    comboBox27.SelectedValue = !Convert.IsDBNull(empview6.Rows[0]["des_id"]) ? Convert.ToInt16(empview6.Rows[0]["des_id"]) : 0;
                    comboBox28.SelectedValue = !Convert.IsDBNull(empview6.Rows[0]["branchid"]) ? Convert.ToInt16(empview6.Rows[0]["branchid"]) : 0;

                    dateAppt.Value = !Convert.IsDBNull(empview6.Rows[0]["app_date"]) ? Convert.ToDateTime(empview6.Rows[0]["app_date"]) : DateTime.Now;
                    dateQuit.Value = !Convert.IsDBNull(empview6.Rows[0]["quit_date"]) ? Convert.ToDateTime(empview6.Rows[0]["quit_date"]) : DateTime.Now;
                    glNewPrior = false;
                }
            }
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox33_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox33.Focused )
            {
                paypageok();
            }
        }

        private void comboBox34_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox34.Focused)
            {
                paypageok();
            }
        }

        private void comboBox29_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox29.Focused)
            {
                paypageok();
            }
        }

        private void textBox40_Validated(object sender, EventArgs e)
        {
            paypageok();
        }

        private void textBox43_Validated(object sender, EventArgs e)
        {
            paypageok();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            paypageok();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            paypageok();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            paypageok();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            paypageok();
        }

        private void paySaveButton_Click(object sender, EventArgs e)
        {
            updPayDetails();
            initvariables();
        }


        private void empPayDetails(string tcStaffNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string empPaysql = "exec tsp_empPay " + ncompid + ",'" + tcStaffNo + "'";
                SqlDataAdapter daempPay = new SqlDataAdapter(empPaysql, ndConnHandle);
                DataTable empPayview = new DataTable();
                daempPay.Fill(empPayview);
                if (empPayview.Rows.Count > 0)
                {
                    comboBox33.SelectedValue =!Convert.IsDBNull(empPayview.Rows[0]["npaytype"]) ? Convert.ToInt16(empPayview.Rows[0]["npaytype"]) : 0;
                    comboBox34.SelectedValue = !Convert.IsDBNull(empPayview.Rows[0]["payper"]) ? Convert.ToInt16(empPayview.Rows[0]["payper"]) : 0;
                    comboBox29.SelectedValue = !Convert.IsDBNull(empPayview.Rows[0]["pmttype"]) ? Convert.ToInt16(empPayview.Rows[0]["pmttype"]) : 0;
                    textBox44.Text = !Convert.IsDBNull(empPayview.Rows[0]["nbasic"]) ? Convert.ToDecimal(empPayview.Rows[0]["nbasic"]).ToString("N2") : "";
                    textBox40.Text = !Convert.IsDBNull(empPayview.Rows[0]["noverday"]) ? Convert.ToDecimal(empPayview.Rows[0]["noverday"]).ToString("N2") : "";
                    textBox43.Text = !Convert.IsDBNull(empPayview.Rows[0]["noverwk"]) ?  Convert.ToDecimal(empPayview.Rows[0]["noverwk"]).ToString("N2") : "";
                    checkBox3.Checked = !Convert.IsDBNull(empPayview.Rows[0]["csocex"]) ? Convert.ToBoolean(empPayview.Rows[0]["csocex"]) : false;
                    checkBox4.Checked = !Convert.IsDBNull(empPayview.Rows[0]["lconfirm"]) ? Convert.ToBoolean(empPayview.Rows[0]["lconfirm"]) : false;
                    checkBox5.Checked = !Convert.IsDBNull(empPayview.Rows[0]["xpat"]) ? Convert.ToBoolean(empPayview.Rows[0]["xpat"]) : false;
                    checkBox6.Checked = !Convert.IsDBNull(empPayview.Rows[0]["ltaxfree"]) ? Convert.ToBoolean(empPayview.Rows[0]["ltaxfree"]) : false;
                } 
            }
        }
        private void updPayDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                bool lallowed = radioButton1.Checked ? true : false;

                 //we would like to revisit the loan limit issue
                string cquery = "update staff set nbasic = @tnSalary,npaytype = @tnpaytype, payper = @tnpayper, pmttype = @tnpmttype,csocex =  @llSosec, ltaxfree = @tltaxfree, " ;
                cquery+= "lconfirm=@tlconfirm, noverday=@tnnoverday, noverwk=@tnnoverwk,xpat=@lxpat Where staffno = @tcStaffNo and compid = @tncompid";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                cuscommand.UpdateCommand.Parameters.Add("@tcStaffNo", SqlDbType.Char).Value =textBox42.Text.ToString().Trim();
                cuscommand.UpdateCommand.Parameters.Add("@tnSalary", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox44.Text);
                cuscommand.UpdateCommand.Parameters.Add("@tnpaytype", SqlDbType.Int).Value = Convert.ToInt16(comboBox33.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tnpayper", SqlDbType.Int).Value = Convert.ToInt16(comboBox34.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@tnpmttype", SqlDbType.Int).Value = Convert.ToInt16(comboBox29.SelectedValue);
                cuscommand.UpdateCommand.Parameters.Add("@llSosec", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox3.Checked);
                cuscommand.UpdateCommand.Parameters.Add("@tltaxfree", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox6.Checked);
                cuscommand.UpdateCommand.Parameters.Add("@tlconfirm", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox4.Checked);
                cuscommand.UpdateCommand.Parameters.Add("@tnnoverday", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox40.Text);
                cuscommand.UpdateCommand.Parameters.Add("@tnnoverwk", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox43.Text);
                cuscommand.UpdateCommand.Parameters.Add("@lxpat", SqlDbType.Bit).Value =Convert.ToBoolean(checkBox5.Checked);
                cuscommand.UpdateCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;

                ndConnHandle.Open();
                cuscommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Staff payroll details updated successfully");
            }
        }

        private void initvariables()
        {
            comboBox29.SelectedIndex = comboBox33.SelectedIndex = comboBox34.SelectedIndex = -1;
            textBox44.Text = textBox40.Text = textBox43.Text = "";
            checkBox3.Checked = checkBox4.Checked = checkBox6.Checked = checkBox5.Checked = false;
            paySaveButton.Enabled = false;
            paySaveButton.BackColor = Color.Gainsboro;
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            getPayDetails();
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            string tcempname = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["fullname"].ToString();
            textBox42.Text = tcemp;
            textBox41.Text = tcempname;
            empPayDetails(tcemp);
        }

        private void getPayDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
       //         MessageBox.Show("We are inside getpaydetails method");
                string basesql0 = "select pay_name,pay_id from paytype ";
                SqlDataAdapter baseda0 = new SqlDataAdapter(basesql0, ndConnHandle);
                DataTable payrollview = new DataTable();
                baseda0.Fill(payrollview);
                if (payrollview.Rows.Count > 0)
                {
                    comboBox33.DataSource = payrollview.DefaultView;
                    comboBox33.DisplayMember = "pay_name";
                    comboBox33.ValueMember = "pay_id";
                    comboBox33.SelectedIndex = -1;
                }

                string basesql1 = "select per_name, per_id from payper_type";
                SqlDataAdapter baseda1 = new SqlDataAdapter(basesql1, ndConnHandle);
                DataTable payperview = new DataTable();
                baseda1.Fill(payperview);
                if (payperview.Rows.Count > 0)
                {
                    comboBox34.DataSource = payperview.DefaultView;
                    comboBox34.DisplayMember = "per_name";
                    comboBox34.ValueMember = "per_id";
                    comboBox34.SelectedIndex = -1;
                }

                string basesql2 = " select pmt_name,pmt_id from pmt_type ";
                SqlDataAdapter baseda2 = new SqlDataAdapter(basesql2, ndConnHandle);
                DataTable pmtview = new DataTable();
                baseda2.Fill(pmtview);
                if (pmtview.Rows.Count > 0)
                {
                    comboBox29.DataSource = pmtview.DefaultView;
                    comboBox29.DisplayMember = "pmt_name";
                    comboBox29.ValueMember = "pmt_id";
                    comboBox29.SelectedIndex = -1;
                }
            }
        }

        private void textBox37_Validated(object sender, EventArgs e)
        {
            paypageok();
        }

        private void textBox44_Validated(object sender, EventArgs e)
        {
            paypageok();

        }

        private void comboBox6_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if(comboBox6.Focused )
            {
                getbnkBranch(Convert.ToInt16(comboBox6.SelectedValue)); 
            }
        }
    }
}
