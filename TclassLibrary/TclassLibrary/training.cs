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
    public partial class training : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable staffview = new DataTable();
        DataTable filtview = new DataTable();
        DataTable trnbrview = new DataTable();
        bool glNewTrain = true;
        bool glNewBond = true;

        public training(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void grievance_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Training Management >>";
            getstaff();
            getTrndetails();
            //            comboBox44.Focus();
            guarValue.Mask = "000,000,000.00";
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
  //              page01ok();
            }
        }

        private void page01ok() //basic training details input
        {
            bool llbasic = comboBox44.SelectedIndex > -1 && comboBox43.SelectedIndex > -1 && comboBox42.SelectedIndex > -1 && comboBox40.SelectedIndex > -1 && tDuration.Text!=""  ? true : false;
            bool dDates = effDate.Value <= schDate.Value && effDate.Value <= actDate.Value ? true : false;
            bool lbonded = checkBox3.Checked ? (bondPeriod.Text !="" && textBox2.Text!="" && guaAddr.Text !="" && textBox3.Text !="" && textBox4.Text != "" && Convert.ToInt16(comboBox2.SelectedValue) >0 && 
                textBox5.Text != "" && Convert.ToInt16(comboBox1.SelectedValue)>0 && guarValue.Text !="" && Convert.ToInt16(comboBox3.SelectedValue)>0 ? true : false) : true;

            if (tDuration.Text != "" && textBox43.Text != "" && textBox44.Text != "" && llbasic && dDates && lbonded)
            {
                trnSaveButton.Enabled = true;
                trnSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                trnSaveButton.Enabled = false;
                trnSaveButton.BackColor = Color.Gainsboro;
            }
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
                string tcemp0 = staffview.Rows[0]["staffno"].ToString();
                textBox46.Text = tcemp0;

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

        private void initvariables()
        {
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox3.SelectedIndex = comboBox40.SelectedIndex = comboBox42.SelectedIndex = comboBox43.SelectedIndex = comboBox44.SelectedIndex = -1;
            bondPeriod.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = guarValue.Text =  textBox43.Text = textBox44.Text = guaAddr.Text = textBox7.Text =  "";
            checkBox3.Checked = false;
            effDate.Value = schDate.Value = actDate.Value = bondDate.Value = DateTime.Now;

            tDuration.Text = "";
            trnSaveButton.Enabled = false;
            trnSaveButton.BackColor = Color.Gainsboro;
        }
        private void trainingDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                if (glNewTrain)   //Entering the details a new training
                {
                    string ddur = tDuration.Text.ToString();

                    string cquery0 = "Insert Into training (trn_name,qual_type,qual_name,institut,duration,eff_date,sch_compl,act_compl,sponsor,bond,compid) ";
                    cquery0 += "values (@ttrn_name,@tqual_type,@tqual_name,@tinstitut,@tduration,@teff_date,@tsch_compl,@tact_compl,@tsponsor,@bond,@tcompid)";
                    SqlDataAdapter inscomm = new SqlDataAdapter();
                    inscomm.InsertCommand = new SqlCommand(cquery0, ndConnHandle);

                    inscomm.InsertCommand.Parameters.Add("@ttrn_name", SqlDbType.VarChar).Value = textBox43.Text.Trim();
                    inscomm.InsertCommand.Parameters.Add("@tqual_type", SqlDbType.Int).Value = Convert.ToInt16(comboBox43.SelectedValue);
                    inscomm.InsertCommand.Parameters.Add("@tqual_name", SqlDbType.VarChar).Value = textBox44.Text.ToString().Trim();
                    inscomm.InsertCommand.Parameters.Add("@tinstitut", SqlDbType.Int).Value = Convert.ToInt16(comboBox42.SelectedValue);
                    inscomm.InsertCommand.Parameters.Add("@tduration", SqlDbType.Int).Value = Convert.ToInt16(tDuration.Text);
                    inscomm.InsertCommand.Parameters.Add("@teff_date", SqlDbType.DateTime).Value = Convert.ToDateTime(effDate.Value);
                    inscomm.InsertCommand.Parameters.Add("@tsch_compl", SqlDbType.DateTime).Value = Convert.ToDateTime(schDate.Value);
                    inscomm.InsertCommand.Parameters.Add("@tact_compl", SqlDbType.DateTime).Value = Convert.ToDateTime(actDate.Value);
                    inscomm.InsertCommand.Parameters.Add("@tsponsor", SqlDbType.Int).Value = Convert.ToInt16(comboBox40.SelectedValue);
                    inscomm.InsertCommand.Parameters.Add("@bond", SqlDbType.Bit).Value = Convert.ToBoolean(checkBox3.Checked);
                    inscomm.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;
                    ndConnHandle.Open();
                    inscomm.InsertCommand.ExecuteNonQuery();  //Insert new record
                    ndConnHandle.Close();
                    MessageBox.Show("New staff training details added successfully");
                    if (checkBox3.Checked)
                    {
                        insBondingDetails(textBox46.Text);
                    }
                    initvariables();

                }
                else             //amending the details of an old training, this should be biometric controlled
                {
                    string cquery1 = "Insert Into training (trn_name,qual_type,qual_name,gen_subj,hon_subj,institut,duration,eff_date,sch_compl,act_compl,sponsor,";
                    cquery1 += "bond,bond_prd,bond_date,bond_guarant,gua_addr,gua_pobox,gua_tel,gua_idtype,gua_idnumb,gua_type,gua_value,gua_relation,compid) ";
                    cquery1 += "values ";
                    cquery1 += "(@ttrn_name,@tqual_type,@tqual_name,@tgen_subj,@thon_subj,@tinstitut,@tduration,@teff_date,@tsch_compl,@tact_compl,@tsponsor,";
                    cquery1 += "@tbond,@tbond_prd,@tbond_date,@tbond_guarant,@tgua_addr,@tgua_pobox,@tgua_tel,@tgua_idtype,@tgua_idnumb,@tgua_type,@tgua_value,@tgua_relation,@tcompid)";
                    SqlDataAdapter updcomm = new SqlDataAdapter();
                    updcomm.UpdateCommand = new SqlCommand(cquery1, ndConnHandle);

                    if (checkBox3.Checked)
                    {
                        updBondingDetails(textBox46.Text);
                    }
                    initvariables();
                }
            }
        }

        private void insBondingDetails(string tcStaffNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                if (glNewBond)   //Entering the bonding details
                {

                    string cqueryb = "Insert Into tbond (cStaffNo,trn_id,bondDate,bondPeriod,grantor,guaAddr,guatel,guamail,gua_idtype,gua_idnumb,coll_type,coll_value,stf_relation) ";
                    cqueryb += "values (@tcStaffNo,@ttrn_id,@tbondDate,@tbondPeriod,@tgrantor,@tguaAddr,@tguatel,@tguamail,@tgua_idtype,@tgua_idnumb,@tcoll_type,@tcoll_value,@tstf_relation)";
                    SqlDataAdapter inscomm = new SqlDataAdapter();
                    inscomm.InsertCommand = new SqlCommand(cqueryb, ndConnHandle);

                    inscomm.InsertCommand.Parameters.Add("@tcStaffNo", SqlDbType.VarChar).Value = textBox46.Text.Trim();
                    inscomm.InsertCommand.Parameters.Add("@ttrn_id", SqlDbType.Int).Value = 1;//we will come back to this and use the autogenerated training ID 
                    inscomm.InsertCommand.Parameters.Add("@tbondDate", SqlDbType.DateTime).Value = bondDate.Value;
                    inscomm.InsertCommand.Parameters.Add("@tbondPeriod", SqlDbType.Int).Value = Convert.ToInt16(bondPeriod.Text);
                    inscomm.InsertCommand.Parameters.Add("@tgrantor", SqlDbType.VarChar).Value = textBox2.Text.ToString().Trim();
                    inscomm.InsertCommand.Parameters.Add("@tguaAddr", SqlDbType.VarChar).Value = guaAddr.Text.ToString().Trim();
                    inscomm.InsertCommand.Parameters.Add("@tguatel", SqlDbType.VarChar).Value  = textBox3.Text.ToString().Trim();
                    inscomm.InsertCommand.Parameters.Add("@tguamail", SqlDbType.VarChar).Value = textBox4.Text.ToString().Trim();
                    inscomm.InsertCommand.Parameters.Add("@tgua_idtype", SqlDbType.Int).Value  = Convert.ToInt16(comboBox2.SelectedValue);
                    inscomm.InsertCommand.Parameters.Add("@tgua_idnumb", SqlDbType.VarChar).Value = textBox5.Text.ToString().Trim();
                    inscomm.InsertCommand.Parameters.Add("@tcoll_type", SqlDbType.Int).Value = Convert.ToInt16(comboBox1.SelectedValue) ;
                    inscomm.InsertCommand.Parameters.Add("@tcoll_value", SqlDbType.Decimal).Value = Convert.ToDecimal(guarValue.Text);
                    inscomm.InsertCommand.Parameters.Add("@tstf_relation", SqlDbType.Int).Value = Convert.ToInt16(comboBox3.SelectedValue) ;
                    ndConnHandle.Open();
                    inscomm.InsertCommand.ExecuteNonQuery();   
                    ndConnHandle.Close();
                    MessageBox.Show("New staff training Bond details added successfully");
                }
            }
        }

        private void updBondingDetails(string tcStaffNo)
        {

        }

        private void tabPage5_Enter(object sender, EventArgs e)
        {
            if(tabPage5.Focused)
            {
                MessageBox.Show("We are in the page 5 enter ");
                //          getTrndetails();
                //        comboBox44.Focus();
            }
        }

        private void getTrndetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                //get identification number
                string dsql4 = "exec tsp_GetIDTYPE ";
                SqlDataAdapter da4 = new SqlDataAdapter(dsql4, ndConnHandle);
                DataTable ds4 = new DataTable();
                da4.Fill(ds4);
                if (ds4 != null)
                {
                    comboBox2.DataSource = ds4.DefaultView;
                    comboBox2.DisplayMember = "id_name";
                    comboBox2.ValueMember = "idtype";
                    comboBox2.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not get ID Types, inform IT Dept immediately"); }

                //tsp_GetRelation
                string dsql7 = "exec tsp_GetFamilyRelation ";
                SqlDataAdapter da7 = new SqlDataAdapter(dsql7, ndConnHandle);
                DataTable ds7 = new DataTable();
                da7.Fill(ds7);
                if (ds7 != null)
                {
                    comboBox3.DataSource = ds7.DefaultView;
                    comboBox3.DisplayMember = "fam_relation";
                    comboBox3.ValueMember = "fam_id";
                    comboBox3.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find relations, inform IT Dept immediately"); }


                //sponsor list
                string sposql = "SELECT spo_name,spo_id FROM  sponsors order by spo_name  ";
                SqlDataAdapter daspo = new SqlDataAdapter(sposql, ndConnHandle);
                DataTable sponview = new DataTable();
                daspo.Fill(sponview);
                if (sponview!=null && sponview.Rows.Count>0)
                {
                    comboBox40.DataSource = sponview.DefaultView; 
                    comboBox40.DisplayMember = "spo_name";
                    comboBox40.ValueMember = "spo_id";
                    comboBox40.SelectedIndex = -1;
                }

                //collateral type
                string basesql131 = "SELECT coll_name,coll_id FROM  collateral order by coll_name ";
                SqlDataAdapter baseda131 = new SqlDataAdapter(basesql131, ndConnHandle);
                DataTable collview = new DataTable();
                baseda131.Fill(collview);
                if (collview.Rows.Count > 0)
                {
                    comboBox1.DataSource = collview.DefaultView;
                    comboBox1.DisplayMember = "coll_name";
                    comboBox1.ValueMember = "coll_id";
                    comboBox1.SelectedIndex = -1;
                }

                //training type
                string desigsql = "SELECT trn_name,trn_id  FROM  train_type order by trn_name";
                SqlDataAdapter dadesign = new SqlDataAdapter(desigsql, ndConnHandle);
                DataTable desview = new DataTable();
                dadesign.Fill(desview);
                if (desview.Rows.Count > 0)
                {
                    comboBox44.DataSource = desview.DefaultView;
                    comboBox44.DisplayMember = "trn_name";
                    comboBox44.ValueMember = "trn_id";
                    comboBox44.SelectedIndex = -1;
                }


                //training institutions
                string basesql130 = "exec tsp_TrainingInstitutionAll";
                SqlDataAdapter baseda130 = new SqlDataAdapter(basesql130, ndConnHandle);
                baseda130.Fill(trnbrview);
                if (trnbrview.Rows.Count > 0)
                {
                    comboBox42.DataSource = trnbrview.DefaultView;
                    comboBox42.DisplayMember = "ins_name";
                    comboBox42.ValueMember = "ins_id";
                    comboBox42.SelectedIndex = -1;
                }

                //qualification type
                string basesql132 = "SELECT qua_name,qua_id FROM  qual_typ order by qua_name";
                SqlDataAdapter baseda132 = new SqlDataAdapter(basesql132, ndConnHandle);
                DataTable emptypeview = new DataTable();
                baseda132.Fill(emptypeview);
                if (emptypeview.Rows.Count > 0)
                {
                    comboBox43.DataSource = emptypeview.DefaultView;
                    comboBox43.DisplayMember = "qua_name";
                    comboBox43.ValueMember = "qua_id";
                    comboBox43.SelectedIndex = -1;
                }
            }
        }

        private void radioButton14_CheckedChanged_1(object sender, EventArgs e)
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
                    textBox46.Text = tcemp1;
                    textBox45.Text = staffview.Rows.Count.ToString();
                }
                ndConnHandle.Close();
                //string tcemp1 = staffview.Rows[0]["staffno"].ToString();
                //textBox46.Text = tcemp1;
                //textBox45.Text = staffview.Rows.Count.ToString();
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

        private void button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox44_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox44.Focused)
            {
                page01ok();
            }
        }

        private void comboBox43_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox43.Focused)
            {
                page01ok();
            }
        }

        private void comboBox42_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox42.Focused)
            {
                int dcouid = Convert.ToInt16(trnbrview.Rows[comboBox42.SelectedIndex]["cou_id"]);
                textBox7.Text = getDcountry(dcouid);
                page01ok();
            }
        }

        private string getDcountry(int tnIns)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
//                MessageBox.Show("We will be taking the instiution id =" + tnIns);
                string insquery = "select institution.cou_id,cou_name,ins_name from institution, country where institution.cou_id = country.cou_id  and institution.cou_id = " + tnIns;
                SqlDataAdapter baseCou = new SqlDataAdapter(insquery, ndConnHandle);
                DataTable dcouview = new DataTable();
                baseCou.Fill(dcouview);
                if (dcouview.Rows.Count > 0)
                {
                    string couname = dcouview.Rows[0]["cou_name"].ToString().Trim();
                    return couname;
                }
                else { return ""; }
            }
        }

        private void comboBox40_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox40.Focused)
            {
                page01ok();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Focused)
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

        private void textBox43_Validated(object sender, EventArgs e)
        {
                page01ok();
        }

        private void textBox44_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox41_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void guaAddr_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox4_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox5_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void effDate_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void schDate_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void actDate_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void bondDate_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            panel4.Enabled = checkBox3.Checked ? true : false;
            page01ok();
        }

        private void trnSaveButton_Click(object sender, EventArgs e)
        {
            trainingDetails();
        }

        private void hisNewButton_Click(object sender, EventArgs e)
        {
            glNewTrain = true ;
        }

        private void tDuration_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void bondDate_ValueChanged(object sender, EventArgs e)
        {
            page01ok();
        }

        private void bondPeriod_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox2_Validated_1(object sender, EventArgs e)
        {
            page01ok();
        }

        private void guaAddr_Validated_1(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox3_Validated_1(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox4_Validated_1(object sender, EventArgs e)
        {
            page01ok();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if(Focused )
            {
                page01ok();
            }
        }

        private void textBox5_Validated_1(object sender, EventArgs e)
        {
            page01ok();
        }

        private void comboBox1_Validated(object sender, EventArgs e)
        {
            if(comboBox1.Focused )
            {
                page01ok();
            }
        }

        private void textBox6_Validated_1(object sender, EventArgs e)
        {
            page01ok();
        }

        private void comboBox3_Validated(object sender, EventArgs e)
        {
            if(comboBox3.Focused )
            {
                page01ok();
            }
        }

        private void ClientGrid_Click(object sender, EventArgs e)
        {
            string tcemp = staffview.Rows[ClientGrid.CurrentCell.RowIndex]["staffno"].ToString();
            textBox46.Text = tcemp;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            getstaff();
        }
    }
}
