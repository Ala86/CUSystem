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
using TclassLibrary;

namespace WinTcare
{
    public partial class coversetup : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        DataTable countryview = new DataTable();
        DataTable cityview = new DataTable();
        DataTable insuview = new DataTable();
        bool glNewInstitution = true;
        public coversetup()
        {
            InitializeComponent();
        }

        private void coversetup_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Institution Setup >>";
            getcountry();
           getinsurance();
            //                getcity();
            textBox1.Focus();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                AllClear2Go();
            }
        }

        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            // string newpass = UserPassWord.Text.ToUpper();
            if (textBox1.Text != "" && textBox3.Text != "" && textBox3.Text != "" && textBox5.Text != ""
                && Convert.ToInt32(comboBox1.SelectedIndex)>-1 && Convert.ToInt32(comboBox2.SelectedIndex)>-1 && richTextBox1.Text.Trim().ToString() != "" )
            {
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
                saveButton.Select();
            }
            else
            {
                saveButton.Enabled = false;
                //*            MessageBox.Show("Invalid User or Password");
            }

        }
        #endregion 

        private void getinsurance()
        {
            using (SqlConnection ndconnhandle = new SqlConnection(cs))
            {
                //************Getting insurance                
                string dsql3 = "exec sp_Insurance  "+ncompid;
                SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndconnhandle);
                da3.Fill(insuview);
                if (insuview != null)
                {
                    insuGrid.AutoGenerateColumns = false;
                    insuGrid.DataSource = insuview.DefaultView;
                    insuGrid.Columns[0].DataPropertyName = "insu_name";
                    insuGrid.Columns[1].DataPropertyName = "city_name";
                    insuGrid.Columns[2].DataPropertyName = "tel";
                    insuGrid.Columns[3].DataPropertyName = "email";
                    for(int i=0; i<10; i++)
                    {
                        insuview.Rows.Add();
                    }
                }
            }
        }
        private void getcountry()
        {
            using (SqlConnection ndconnhandle = new SqlConnection(cs))
            {
                //************Getting country                
                string dsql1 = "Select cou_name,cou_id from country order by cou_name ";
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndconnhandle);
                da1.Fill(countryview);
                if (countryview != null)
                {
                    comboBox1.DataSource = countryview.DefaultView;
                    comboBox1.DisplayMember = "cou_name";
                    comboBox1.ValueMember = "cou_id";
                    comboBox1.SelectedIndex = -1;
                    textBox1.Focus();
                }
                else { MessageBox.Show("Country Master file is empty, inform IT Dept immediately"); }
            }
        }

        private void getcity(int couid)
        {
            using (SqlConnection ndconnhandle = new SqlConnection(cs))
            {
                //************Getting city                
                string dsql1 = "Select city_name,city_id,cou_id from city where cou_id="+couid+" order by city_name";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql1, ndconnhandle);
                da2.Fill(cityview);
                if (cityview != null)
                {
                    comboBox2.DataSource = cityview.DefaultView;
                    comboBox2.DisplayMember = "city_name";
                    comboBox2.ValueMember = "city_id";
                    comboBox2.SelectedIndex = -1;
                }
                else { MessageBox.Show("City Master file is empty, inform IT Dept immediately"); }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Now we are going to insert the institution");
            /*
             if new institution, then incsert
             		sn=SQLExec(gnConnHandle,"insert into insurance (claimform,insu_name,cou_id,city_id,caddress,tel,pobox,email,compid,ins_type,insu_code,lsmart) "+;
			"values (?gcClaimForm,?lcInsName,?.combo1.value,?.combo2.value,?.edit1.value,?.text1.value,?.text4.value,?.text6.value,?gnCompid,?lnInsType,?.text3.value,?lsmart)","corupd")
				Thisform.crea_acc
                else update
            sn=SQLExec(gnConnHandle,"update insurance set claimform=?gcClaimForm,insu_name=?.text5.value,cou_id=?.combo1.value,city_id=?.combo2.value,"+;
			"caddress=?.edit1.value,tel=?.text1.value,pobox=?.text4.value,email=?.text6.value,insu_code=?.text3.value,lsmart=?lsmart where insu_id=?gnInsID","corupd")
            endif 
             */
            //        string cs = globalvar.cos;
            //            int ncompid = globalvar.gnCompid;
            //          int lnVisno = (this.radioButton9.Checked == true ? 1 : Int32.Parse(this.textBox18.Text));
            //        string tcCode = textBox1.Text.ToString().Trim();
            int instype = (radioButton1.Checked ? 1 : (radioButton2.Checked ? 2 : 3));
            bool lsmart = checkBox1.Checked;
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                if(glNewInstitution)  //new institution
                {
                    string dsql1 = "insert into insurance (claimform,insu_name,cou_id,city_id,caddress,tel,pobox,email,compid,ins_type,insu_code,lsmart) ";
                  dsql1 += "values (@cClaimForm,@lcInsName,@couid,@cityid,@daddr,@telnum,@dpobox,@demail,@dCompid,@nInsType,@inscode,@smart)";
                    SqlDataAdapter covercommand = new SqlDataAdapter();
                    covercommand.InsertCommand = new SqlCommand(dsql1, ndConnHandle1);

                    covercommand.InsertCommand.Parameters.Add("@lcInsName", SqlDbType.VarChar).Value = textBox1.Text.Trim().ToString();
                    covercommand.InsertCommand.Parameters.Add("@inscode", SqlDbType.Char).Value = string.Empty;
                    covercommand.InsertCommand.Parameters.Add("@couid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                    covercommand.InsertCommand.Parameters.Add("@cityid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                    MessageBox.Show("step 0");
                    covercommand.InsertCommand.Parameters.Add("@daddr", SqlDbType.Char).Value = richTextBox1.Text.Trim().ToString();
                    covercommand.InsertCommand.Parameters.Add("@telnum", SqlDbType.VarChar).Value = textBox3.Text.Trim().ToString();
                    covercommand.InsertCommand.Parameters.Add("@dpobox", SqlDbType.Char).Value = textBox4.Text.Trim().ToString();
                    covercommand.InsertCommand.Parameters.Add("@demail", SqlDbType.Char).Value = textBox5.Text.Trim().ToString();
                    covercommand.InsertCommand.Parameters.Add("@cClaimForm", SqlDbType.Char).Value = textBox6.Text.ToString().Trim();
                    MessageBox.Show("step 1");

                    covercommand.InsertCommand.Parameters.Add("@dCompid", SqlDbType.Int).Value = ncompid;
                    covercommand.InsertCommand.Parameters.Add("@nInsType", SqlDbType.Int).Value = instype;
                    covercommand.InsertCommand.Parameters.Add("@smart", SqlDbType.Bit).Value = lsmart;
                    MessageBox.Show("step 2");

                    covercommand.InsertCommand.ExecuteNonQuery();
                }
                else
                {
                    //editing existing institution
                    string dsql2 = "update insurance set claimform=@cClaimForm,insu_name=@lcInsName,cou_id=@couid,city_id=@cityid,";
                    dsql2 += "caddress=@daddr,tel=@telnum,pobox=@dpobox,email=@demail,insu_code=@inscode,lsmart=@smart where insu_id=@gnInsID";
                    SqlDataAdapter coverupdate = new SqlDataAdapter();
                      coverupdate.UpdateCommand = new SqlCommand(dsql2, ndConnHandle1);

                      coverupdate.UpdateCommand.Parameters.Add("@cClaimForm", SqlDbType.Char).Value = textBox6.Text.ToString().Trim();
                      coverupdate.UpdateCommand.Parameters.Add("@lcInsName", SqlDbType.VarChar).Value = textBox1.Text.Trim().ToString();
                      coverupdate.UpdateCommand.Parameters.Add("@couid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                      coverupdate.UpdateCommand.Parameters.Add("@cityid", SqlDbType.Int).Value = comboBox2.SelectedValue;

                      coverupdate.UpdateCommand.Parameters.Add("@daddr", SqlDbType.Char).Value = richTextBox1.Text.Trim().ToString();
                      coverupdate.UpdateCommand.Parameters.Add("@telnum", SqlDbType.VarChar).Value = textBox3.Text.Trim().ToString();
                      coverupdate.UpdateCommand.Parameters.Add("@dpobox", SqlDbType.Char).Value = textBox4.Text.Trim().ToString();
                      coverupdate.UpdateCommand.Parameters.Add("@demail", SqlDbType.Char).Value = textBox5.Text.Trim().ToString();
                    coverupdate.UpdateCommand.Parameters.Add("@inscode", SqlDbType.Char).Value = string.Empty;
                      coverupdate.UpdateCommand .Parameters.Add("@smart", SqlDbType.Bit).Value = lsmart;
                    coverupdate.UpdateCommand.ExecuteNonQuery();
                }
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Insurer";
            textBox1.Focus();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Corporate";
            textBox1.Focus();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "NHIF";
            textBox1.Focus();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
//            getcity(Convert.ToInt32(comboBox1.SelectedValue));
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            getcity(Convert.ToInt32(comboBox1.SelectedValue));
        }

        private void coversetup_KeyPress(object sender, KeyPressEventArgs e)
        {
//            if (e.KeyChar == (char)Keys.Tab) { MessageBox.Show("we are going to all clear"); AllClear2Go(); } else { MessageBox.Show("You have not tabbed"); }
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab) {AllClear2Go(); } 
        }


         private void richTextBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab) { AllClear2Go(); }
        }

        private void textBox3_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab) { AllClear2Go(); }
        }

        private void textBox4_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab) { AllClear2Go(); }
        }

        private void textBox5_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab) { AllClear2Go(); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
             With Thisform 
	sn=Createobject('sn_image')
	sn.Show
	If !Empty(gcTmpFile)
		.image1.Picture=gcTmpFile
	Endif
Endwith
Thisform.Refresh

             */
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*
             With Thisform
	gcClaimForm=justFName( getfile('frx','Select Claim Form')) 
	If !Empty(gcClaimForm)
		.text2.Value=gcClaimForm
	Endif
Endwith
Thisform.Refresh
             */
        }

        private void getdetails()
        {
            //************Get insurance details            
            MessageBox.Show("inside get details");     
     if(insuview.Rows.Count >0)
            {
                comboBox1.SelectedValue = Convert.ToInt32(insuview.Rows[0]["cou_id"]);
                comboBox2.SelectedValue = Convert.ToInt32(insuview.Rows[0]["city_id"]);
                richTextBox1.Text = insuview.Rows[0]["caddress"].ToString();
                textBox1.Text = insuview.Rows[0]["insu_name"].ToString();
                textBox3.Text = insuview.Rows[0]["tel"].ToString();
                textBox4.Text = insuview.Rows[0]["pobox"].ToString();
                textBox5.Text = insuview.Rows[0]["email"].ToString();

            } else { MessageBox.Show("insview is empty"); }

        }
        private void button5_Click(object sender, EventArgs e)
        {
            /*
             DO FORM bouquets.scx
             */
            bouquets dbou = new bouquets();
            dbou.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            glNewInstitution = false;
            comboBox3.Visible = true;
            textBox1.Visible = false;
            insuview.Clear();
            cityview.Clear();
            getinsurance();
//            using (SqlConnection ndconnhandle = new SqlConnection(cs))
  //          {
    //            string dsql3 = "exec sp_Insurance  " + ncompid;
      //          SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndconnhandle);
        //        da3.Fill(insuview);
                if (insuview != null)
                {
                    comboBox3.DataSource = insuview.DefaultView;
                    comboBox3.DisplayMember = "insu_name";
                    comboBox3.ValueMember = "insu_id";
                    comboBox3.SelectedIndex = -1;
                }
          //  }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text.ToString() != "System.Data.DataRowView" && Convert.ToInt32(comboBox3.SelectedIndex.ToString())>-1 && comboBox3.SelectedValue.ToString() != "System.Data.DataRowView" && Convert.ToInt32(comboBox3.SelectedValue)>0)
            {
                    richTextBox1.Text = insuview.Rows[comboBox3.SelectedIndex]["caddress"].ToString();
                    textBox1.Text = insuview.Rows[comboBox3.SelectedIndex]["insu_name"].ToString();
                    textBox3.Text = insuview.Rows[comboBox3.SelectedIndex]["tel"].ToString();
                    textBox4.Text = insuview.Rows[comboBox3.SelectedIndex]["pobox"].ToString();
                    textBox5.Text = insuview.Rows[comboBox3.SelectedIndex]["email"].ToString();
                    comboBox1.SelectedValue = insuview.Rows[comboBox3.SelectedIndex]["cou_id"].ToString();
                    cityview.Clear();
                    getcity(Convert.ToInt32(comboBox1.SelectedValue));
                    comboBox2.SelectedValue = insuview.Rows[comboBox3.SelectedIndex]["city_id"].ToString();
            }
        }


    }
}
