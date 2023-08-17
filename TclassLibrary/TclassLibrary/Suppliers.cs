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
    public partial class Suppliers : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable supplierview = new DataTable();
        DataTable cityview = new DataTable();
        DataTable filtview = new DataTable();
        DataTable suppview = new DataTable();
        DataTable empDisview = new DataTable();
        string gcInputstat = "A";
        public Suppliers(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void Suppliers_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Suplier Management >>";
            getsuppliers();
            getBasics();
            textBox6.Text = GetClient_Code.clientCode_int(cs, "suppid").ToString().PadLeft(6, '0');
            getfirst();
            //       ClientGrid.Click;
//            tabPage5.Focus();
        }

        private void getsuppliers()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                suppview.Clear();
                ndConnHandle.Open();
                string dsql = "exec tsp_getSupplier " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(suppview);
                ClientGrid.AutoGenerateColumns = false;
                ClientGrid.DataSource = suppview.DefaultView;
                ClientGrid.Columns[0].DataPropertyName = "supp_no";
                ClientGrid.Columns[1].DataPropertyName = "sup_name";
                ClientGrid.Columns[2].DataPropertyName = "biznature";
                ClientGrid.Columns[3].DataPropertyName = "supcat";
                ClientGrid.Columns[4].DataPropertyName = "supclass";
                ndConnHandle.Close();
                textBox45.Text = suppview.Rows.Count.ToString();
                string tcemp = suppview.Rows[ClientGrid.CurrentCell.RowIndex]["supp_no"].ToString();
                string tcempname = suppview.Rows[ClientGrid.CurrentCell.RowIndex]["sup_name"].ToString();
                textBox53.Text = tcemp;
                supBasicDetails(tcemp);
            }
        }

        private void getfirst()
        {
            string tcemp = suppview.Rows[0]["supp_no"].ToString();
            string tcempname = suppview.Rows[0]["sup_name"].ToString();

            textBox6.Text = tcemp;
            textBox9.Text = tcempname;

            textBox46.Text = tcemp;
            textBox48.Text = tcempname;

            textBox53.Text = tcemp;
            textBox54.Text = tcempname;
            supBasicDetails(tcemp);
        }
        private void getBasics()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                string basesql13a = "select biz_id,biz_name from biz_nature order by biz_name ";
                SqlDataAdapter baseda13a = new SqlDataAdapter(basesql13a, ndConnHandle);
                DataTable absview = new DataTable();
                baseda13a.Fill(absview);
                if (absview.Rows.Count > 0)
                {
                    comboBox11.DataSource = absview.DefaultView;
                    comboBox11.DisplayMember = "biz_name";
                    comboBox11.ValueMember = "biz_id";
                    comboBox11.SelectedIndex = -1;
                }

                string basesql13b = "select supcat_id,supcat_name from suppl_cat order by supcat_name";
                SqlDataAdapter baseda13b = new SqlDataAdapter(basesql13b, ndConnHandle);
                DataTable absviewb = new DataTable();
                baseda13b.Fill(absviewb);
                if (absviewb.Rows.Count > 0)
                {
                    comboBox9.DataSource = absviewb.DefaultView;
                    comboBox9.DisplayMember = "supcat_name";
                    comboBox9.ValueMember = "supcat_id";
                    comboBox9.SelectedIndex = -1;
                }

                
                string basesql13c = "select class_id, class_name  from supp_class order by class_name";
                SqlDataAdapter baseda13c = new SqlDataAdapter(basesql13c, ndConnHandle);
                DataTable absviewc = new DataTable();
                baseda13c.Fill(absviewc);
                if (absviewc.Rows.Count > 0)
                {
                    comboBox10.DataSource = absviewc.DefaultView;
                    comboBox10.DisplayMember = "class_name";
                    comboBox10.ValueMember = "class_id";
                    comboBox10.SelectedIndex = -1;
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
                ndConnHandle.Close();
            }
        }

        private void gettown(int tncouid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                cityview.Clear();
                ndConnHandle.Open();
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
                ndConnHandle.Close();
            }
        }

        private void getbnkBranch(int tnbnkid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
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
                ndConnHandle.Close();
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

        private void page01ok() //supplier basic details 
        {

            if (textBox6.Text.Trim() !="" && textBox9.Text.Trim() !="" && comboBox11.SelectedIndex > -1 && comboBox9.SelectedIndex > -1 && comboBox10.SelectedIndex > -1 && 
                comboBox13.SelectedIndex > -1 && comboBox12.SelectedIndex > -1 && textBox7.Text.Trim() != "" && textBox21.Text.Trim() != "" && textBox19.Text.Trim() != "" && 
                textBox17.Text.Trim() != "")
            {
                supSaveButton.Enabled = true;
                supSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                supSaveButton.Enabled = false;
                supSaveButton.BackColor = Color.Gainsboro;
            }
        }

        private void page02ok() //supplier credit details 
        {
            DateTime dtoday = Convert.ToDateTime(DateTime.Today.ToShortDateString());

            bool lDateOk = crdEdDate.Value >= dtoday && crdEdDate.Value > crdStDate.Value ? true : false; 

            if ((textBox10.Text.Trim() != "" || textBox1.Text.Trim() != "") && textBox16.Text.Trim() != "" && lDateOk && comboBox6.SelectedIndex > -1 && comboBox5.SelectedIndex > -1 && 
                textBox2.Text.Trim() != "")
            {
                supCredSave.Enabled = true;
                supCredSave.BackColor = Color.LawnGreen;
            }
            else
            {
                supCredSave.Enabled = false;
                supCredSave.BackColor = Color.Gainsboro;
            }
        }


        private void getfiltview(int tnfiltype)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
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
                ndConnHandle.Close();
            }
        }

        private void getstaffByFilter(int tnFilter)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                supplierview.Clear();
                ndConnHandle.Open();
                switch (tnFilter)
                {
                    case 0:     // all staff
                        string dsql0 = "exec tsp_getActiveStaff " + ncompid;
                        SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndConnHandle);
                        da0.Fill(supplierview);
                        break;
                    case 1: //by branch
                        string dsql1 = "exec tsp_getActiveStaffByBranch " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                        da1.Fill(supplierview);
                        break;
                    case 2: //by department
                        string dsql2 = "exec tsp_getActiveStaffByDept " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                        da2.Fill(supplierview);
                        break;
                    case 3:     // designation
                        string dsql3 = "exec tsp_getActiveStaffByDesig " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle);
                        da3.Fill(supplierview);
                        break;
                    case 4: //band
                        string dsql4 = "exec tsp_getActiveStaffByBand " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da4 = new SqlDataAdapter(dsql4, ndConnHandle);
                        da4.Fill(supplierview);
                        break;
                    case 5: //by ethnicity
                        string dsql5 = "exec tsp_getActiveStaffByEth " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da5 = new SqlDataAdapter(dsql5, ndConnHandle);
                        da5.Fill(supplierview);
                        break;
                    case 6:     // cost centre
                        string dsql6 = "exec tsp_getActiveStaffByCoscen " + ncompid + "," + comboBox17.SelectedValue;
                        SqlDataAdapter da6 = new SqlDataAdapter(dsql6, ndConnHandle);
                        da6.Fill(supplierview);
                        break;
                    case 10: //female staff
                        string dsql10 = "exec tsp_getActiveStaffFemale " + ncompid;
                        SqlDataAdapter da10 = new SqlDataAdapter(dsql10, ndConnHandle);
                        da10.Fill(supplierview);
                        break;
                    case 11: //by department
                        string dsql11 = "exec tsp_getActiveStaffMale " + ncompid;
                        SqlDataAdapter da11 = new SqlDataAdapter(dsql11, ndConnHandle);
                        da11.Fill(supplierview);
                        break;
                }
                if (supplierview.Rows.Count > 0)
                {
                    ClientGrid.AutoGenerateColumns = false;
                    ClientGrid.DataSource = supplierview.DefaultView;
                    ClientGrid.Columns[0].DataPropertyName = "staffno";
                    ClientGrid.Columns[1].DataPropertyName = "fullname";
                    ClientGrid.Columns[2].DataPropertyName = "depname";
                    ClientGrid.Columns[3].DataPropertyName = "desname";
                    ClientGrid.Columns[4].DataPropertyName = "dage";
                    ClientGrid.Columns[5].DataPropertyName = "dgender";
                    string tcemp1 = supplierview.Rows[0]["staffno"].ToString();
                    //                    textBox46.Text = tcemp1;
                    textBox45.Text = supplierview.Rows.Count.ToString();
                    string tcemp = supplierview.Rows[0]["staffno"].ToString();
                    string tcempname = supplierview.Rows[0]["fullname"].ToString();
                    textBox53.Text = tcemp;
                    supBasicDetails(tcemp);
                }
                ndConnHandle.Close();
                //string tcemp1 = supplierview.Rows[0]["staffno"].ToString();
                //textBox46.Text = tcemp1;
                //textBox45.Text = supplierview.Rows.Count.ToString();
            }
        }

        //private void empDisDetails(string tcStaffNo)
        //{
        //    empDisview.Clear();

        //    using (SqlConnection ndConnHandle = new SqlConnection(cs))
        //    {
        //        ndConnHandle.Open();
        //        string empsql = "exec tsp_empDisp " + ncompid + ",'" + tcStaffNo + "'";
        //        SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
        //        daemp.Fill(empDisview);
        //        if (empDisview.Rows.Count > 0)
        //        {
        //            supGrid.AutoGenerateColumns = false;
        //            supGrid.DataSource = empDisview.DefaultView;
        //            supGrid.Columns[0].DataPropertyName = "adate";
        //            supGrid.Columns[1].DataPropertyName = "act_name";
        //            supGrid.Columns[2].DataPropertyName = "reason";
        //        }
        //        ndConnHandle.Close();
        //    }
        //}


        private void initvariables()
        {
            textBox9.Text = textBox5.Text = textBox7.Text = textBox21.Text = textBox19.Text = textBox17.Text = richTextBox1.Text = "";
            comboBox11.SelectedIndex = comboBox9.SelectedIndex = comboBox10.SelectedIndex = comboBox13.SelectedIndex = comboBox12.SelectedIndex = -1;
            supSaveButton.Enabled = false;
            supSaveButton.BackColor = Color.Gainsboro;
            getsuppliers();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        //private void perSaveButton_Click(object sender, EventArgs e)
        //{
        //    basicDetails(textBox6.Text.ToString().Trim());
        //    initvariables();
        //    updateClient_Code updc = new updateClient_Code();
        //    updc.updClient(cs, "suppid");
        //    textBox6.Text = GetClient_Code.clientCode_int(cs, "suppid").ToString().PadLeft(6, '0');
        //}


        private void comboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox13.Focused )
            {
                gettown(Convert.ToInt16(comboBox13.SelectedValue));
                page01ok();
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox6.Focused )
            {
                getbnkBranch(Convert.ToInt16(comboBox6.SelectedValue));
            }
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox11.Focused )
            {
                page01ok();
            }
        }

        private void textBox5_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox9.Focused)
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

        private void richTextBox1_Validated(object sender, EventArgs e)
        {
                page01ok();
        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox12.Focused)
            {
                page01ok();
            }
        }

        private void textBox7_Validated(object sender, EventArgs e)
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

        private void button17_Click(object sender, EventArgs e)
        {
            gcInputstat = "N";
            initvariables();
            textBox6.Text = GetClient_Code.clientCode_int(cs, "suppid").ToString().PadLeft(6, '0');
        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void ClientGrid_Click(object sender, EventArgs e)
        {
            string tcemp = suppview.Rows[ClientGrid.CurrentCell.RowIndex]["supp_no"].ToString();
            string tcempname = suppview.Rows[ClientGrid.CurrentCell.RowIndex]["sup_name"].ToString();

            textBox6.Text = tcemp;
            textBox9.Text = tcempname;

            textBox46.Text = tcemp;
            textBox48.Text = tcempname;

            textBox53.Text = tcemp;
            textBox54.Text = tcempname;

            supBasicDetails(tcemp);
        }

        private void tabPage5_Enter(object sender, EventArgs e)
        {
            //if(tabPage5.Focused )
            //{
            //    getBasics();
            //    string tcemp = suppview.Rows[ClientGrid.CurrentCell.RowIndex]["supp_no"].ToString();
            //    string tcempname = suppview.Rows[ClientGrid.CurrentCell.RowIndex]["sup_name"].ToString();
            //    textBox6.Text = tcemp;
            //    textBox9.Text = tcempname;
            //    supBasicDetails(tcemp);
            //}
        }

        private void supBasicDetails(string tcSuppNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string empsql = "exec tsp_getSupplier_one " + ncompid + ",'" + tcSuppNo + "'";
                SqlDataAdapter daemp = new SqlDataAdapter(empsql, ndConnHandle);
                DataTable supview = new DataTable();
                daemp.Fill(supview);
                if (supview.Rows.Count > 0)
                {
                    textBox6.Text = supview.Rows[0]["supp_no"].ToString();
                    textBox9.Text = supview.Rows[0]["sup_name"].ToString();
                    textBox5.Text = !Convert.IsDBNull(supview.Rows[0]["reg_no"]) ? supview.Rows[0]["reg_no"].ToString() : "";

                    richTextBox1.Text = !Convert.IsDBNull(supview.Rows[0]["addr"]) ? supview.Rows[0]["addr"].ToString() : "";
                    textBox7.Text = !Convert.IsDBNull(supview.Rows[0]["contactnam"]) ? supview.Rows[0]["contactnam"].ToString() : "";
                    textBox21.Text = !Convert.IsDBNull(supview.Rows[0]["contpost"]) ?  supview.Rows[0]["contpost"].ToString() : "";
                    textBox19.Text = !Convert.IsDBNull(supview.Rows[0]["contphone"]) ? supview.Rows[0]["contphone"].ToString() : "";
                    textBox17.Text = !Convert.IsDBNull(supview.Rows[0]["email"]) ? supview.Rows[0]["email"].ToString() : "";
                    comboBox11.SelectedValue = !Convert.IsDBNull(supview.Rows[0]["biz_type"]) ? Convert.ToInt16(supview.Rows[0]["biz_type"]) : 0;
                    comboBox9.SelectedValue = !Convert.IsDBNull(supview.Rows[0]["sup_cat"]) ? Convert.ToInt16(supview.Rows[0]["sup_cat"]) : 0;
                    comboBox10.SelectedValue = !Convert.IsDBNull(supview.Rows[0]["sup_class"]) ? Convert.ToInt16(supview.Rows[0]["sup_class"]) : 0;
                    comboBox13.SelectedValue = !Convert.IsDBNull(supview.Rows[0]["country"]) ? Convert.ToInt16(supview.Rows[0]["country"]) : 0;
                    int counid = !Convert.IsDBNull(supview.Rows[0]["country"]) ? Convert.ToInt16(supview.Rows[0]["country"]) : 0;
                    gettown(counid);
                    comboBox12.SelectedValue = !Convert.IsDBNull(supview.Rows[0]["city"]) ? Convert.ToInt16(supview.Rows[0]["city"]) : 0;
                    //gettown(Convert.ToInt16(comboBox13.SelectedValue));
                }
            }
        }

        private void supSaveButton_Click(object sender, EventArgs e)
        {
            updbasicDetails(textBox6.Text.ToString().Trim());
            initvariables();
            updateClient_Code updc = new updateClient_Code();
            updc.updClient(cs, "suppid");
            textBox6.Text = GetClient_Code.clientCode_int(cs, "suppid").ToString().PadLeft(6, '0');
        }

        private void updbasicDetails(String tcSuppNo)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                if (gcInputstat == "N")
                {
                    string cquery = "Insert Into suppliers (supp_no,sup_name,biz_type,reg_no,sup_cat,sup_class,addr,country,city,contactnam,contpost,contphone,email,compid)";
                    cquery += "Values (@tsupp_no,@tsup_name,@tbiz_type,@treg_no,@tsup_cat,@tsup_class,@taddr,@tcountry,@tcity,@tcontactnam,@tcontpost,@tcontphone,@temail,@tcompid)";

                    SqlDataAdapter cuscommand = new SqlDataAdapter();
                    cuscommand.InsertCommand = new SqlCommand(cquery, ndConnHandle);

                    cuscommand.InsertCommand.Parameters.Add("@tsupp_no", SqlDbType.Char).Value = tcSuppNo;// textBox6.Text.ToString().Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tsup_name", SqlDbType.Char).Value = textBox9.Text.ToString().Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tbiz_type", SqlDbType.Int).Value = Convert.ToInt16(comboBox11.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@treg_no", SqlDbType.VarChar).Value = textBox5.Text.ToString().Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tsup_cat", SqlDbType.Int).Value = Convert.ToInt16(comboBox9.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@tsup_class", SqlDbType.Int).Value = Convert.ToInt16(comboBox10.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@taddr", SqlDbType.VarChar).Value = richTextBox1.Text.ToString().Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tcountry", SqlDbType.Int).Value = Convert.ToInt16(comboBox13.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@tcity", SqlDbType.Int).Value = Convert.ToInt16(comboBox12.SelectedValue);
                    cuscommand.InsertCommand.Parameters.Add("@tcontactnam", SqlDbType.VarChar).Value = textBox7.Text.ToString().Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tcontpost", SqlDbType.VarChar).Value = textBox21.Text.ToString().Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tcontphone", SqlDbType.VarChar).Value = textBox19.Text.ToString().Trim();
                    cuscommand.InsertCommand.Parameters.Add("@temail", SqlDbType.VarChar).Value = textBox17.Text.ToString().Trim();
                    cuscommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                    ndConnHandle.Open();
                    cuscommand.InsertCommand.ExecuteNonQuery();  //Insert new record
                    ndConnHandle.Close();
                    MessageBox.Show("New Supplier Basic details added successfully");
                }
                else
                {
                    string cquery = "update suppliers set sup_name = @tsup_name,biz_type=@tbiz_type,reg_no=@treg_no,sup_cat=@tsup_cat,sup_class=@tsup_class,addr=@taddr,country=@tcountry,";
                    cquery += "city=@tcity,contactnam=@tcontactnam,contpost=@tcontpost,contphone=@tcontphone,email=@temail where supp_no = @tsupp_no and compid = @tcompid";

                    SqlDataAdapter cuscommand1 = new SqlDataAdapter();
                    cuscommand1.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                    cuscommand1.UpdateCommand.Parameters.Add("@tsupp_no", SqlDbType.Char).Value = tcSuppNo;// textBox6.Text.ToString().Trim();
                    cuscommand1.UpdateCommand.Parameters.Add("@tsup_name", SqlDbType.Char).Value = textBox9.Text.ToString().Trim();
                    cuscommand1.UpdateCommand.Parameters.Add("@tbiz_type", SqlDbType.Int).Value = Convert.ToInt16(comboBox11.SelectedValue);
                    cuscommand1.UpdateCommand.Parameters.Add("@treg_no", SqlDbType.VarChar).Value = textBox5.Text.ToString().Trim();
                    cuscommand1.UpdateCommand.Parameters.Add("@tsup_cat", SqlDbType.Int).Value = Convert.ToInt16(comboBox9.SelectedValue);
                    cuscommand1.UpdateCommand.Parameters.Add("@tsup_class", SqlDbType.Int).Value = Convert.ToInt16(comboBox10.SelectedValue);
                    cuscommand1.UpdateCommand.Parameters.Add("@taddr", SqlDbType.VarChar).Value = richTextBox1.Text.ToString().Trim();
                    cuscommand1.UpdateCommand.Parameters.Add("@tcountry", SqlDbType.Int).Value = Convert.ToInt16(comboBox13.SelectedValue);
                    cuscommand1.UpdateCommand.Parameters.Add("@tcity", SqlDbType.Int).Value = Convert.ToInt16(comboBox12.SelectedValue);
                    cuscommand1.UpdateCommand.Parameters.Add("@tcontactnam", SqlDbType.VarChar).Value = textBox7.Text.ToString().Trim();
                    cuscommand1.UpdateCommand.Parameters.Add("@tcontpost", SqlDbType.VarChar).Value = textBox21.Text.ToString().Trim();
                    cuscommand1.UpdateCommand.Parameters.Add("@tcontphone", SqlDbType.VarChar).Value = textBox19.Text.ToString().Trim();
                    cuscommand1.UpdateCommand.Parameters.Add("@temail", SqlDbType.VarChar).Value = textBox17.Text.ToString().Trim();
                    cuscommand1.UpdateCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;

                    ndConnHandle.Open();
                    cuscommand1.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                    MessageBox.Show("Supplier Basic details updated successfully");
                }
            }
        }

        private void comboBox6_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if(comboBox6.Focused )
            {
                getbnkBranch(Convert.ToInt16(comboBox6.SelectedValue));
                page02ok();
            }
        }

        private void textBox10_Validated(object sender, EventArgs e)
        {
            if(textBox10.Text != "")
            {
                textBox10.Text = Convert.ToDecimal(textBox10.Text).ToString("N2");
                page02ok();
            }
        }

        private void textBox12_Validated(object sender, EventArgs e)
        {
            page02ok();
        }

        private void textBox16_Validating(object sender, CancelEventArgs e)
        {
            page02ok();
        }

        private void hireDate_ValueChanged(object sender, EventArgs e)
        {
            page02ok();
        }

        private void confirmDate_ValueChanged(object sender, EventArgs e)
        {
            page02ok();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            page02ok();
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            page02ok();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
