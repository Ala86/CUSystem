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
    public partial class trn_institute : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        DataTable cityview = new DataTable();
        bool glNewIns = true;

        public trn_institute(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void trn_institute_Load(object sender, EventArgs e)
        {
            this.Text = dloca+ "<< Training Institution Setup >>";
            getTrain();
            getInsCountry();
            getFinCountry();
            getFinanciers();
            textBox1.Focus();
        }

  private void getInsCountry()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql1 = "exec tsp_GetCountry ";
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                DataTable couview = new DataTable();
                da1.Fill(couview);
                if (couview != null)
                {
                    comboBox21.DataSource = couview.DefaultView; 
                    comboBox21.DisplayMember = "cou_name";
                    comboBox21.ValueMember = "cou_id";
                    comboBox21.SelectedIndex = -1;
               }
                else { MessageBox.Show("Could not get country details, inform IT Dept immediately"); }
            }
        }

        private void getFinCountry()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string dsql1 = "exec tsp_GetCountry ";
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                DataTable couview1 = new DataTable();
                da1.Fill(couview1);
                if (couview1 != null)
                {
                    comboBox1.DataSource = couview1.DefaultView;
                    comboBox1.DisplayMember = "cou_name";
                    comboBox1.ValueMember = "cou_id";
                    comboBox1.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not get country details, inform IT Dept immediately"); }
            }
        }

        public void getTrain()
        {
            string dsql = "exec tsp_TrainingInstitutionAll";
            DataTable dtable = new DataTable();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(dtable);
                if (dtable.Rows.Count > 0)
                {
                    insGrid.AutoGenerateColumns = false;
                    insGrid.DataSource = dtable.DefaultView;
                    insGrid.Columns[0].DataPropertyName = "ins_name";
                    insGrid.Columns[1].DataPropertyName = "cou_name";
                    ndConnHandle.Close();
                }
            }
        }

        public void getFinanciers()
        {
            string dsql = "exec tsp_SponsorsAll";
            DataTable fintable = new DataTable();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(fintable);
                if (fintable.Rows.Count > 0)
                {
                    finGrid.AutoGenerateColumns = false;
                    finGrid.DataSource = fintable.DefaultView;
                    finGrid.Columns[0].DataPropertyName = "spo_name";
                    finGrid.Columns[1].DataPropertyName = "cou_name";
                    ndConnHandle.Close();
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                page01ok();
//                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                page01ok();
       //         AllClear2Go();
            }
        }//end of onkeydown

        #region Checking if all the mandatory conditions are satisfied
        //private void AllClear2Go()
        //{
        //    bool llbasic = comboBox21.SelectedIndex > -1 && comboBox20.SelectedIndex > -1 ? true : false;
        //    if (textBox1.Text != "" && textBox43.Text != "" && textBox2.Text != "" && llbasic)
        //    {
        //        insSaveButton.Enabled = true;
        //        insSaveButton.BackColor = Color.LawnGreen;
        //        insSaveButton.Select();
        //    }
        //    else
        //    {
        //        insSaveButton.Enabled = false;
        //        insSaveButton.BackColor = Color.Gainsboro;
        //        //*            MessageBox.Show("Invalid User or Password");
        //    }

        //}

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string tcDesc = textBox1.Text.Trim().ToString();
            int cou_id = Convert.ToInt32(comboBox1.SelectedValue);
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                string dsql1 = "Insert Into institution (ins_name,cou_id) values (" + "'" + textBox1.Text.Trim().ToString() + "'" +","+cou_id+ ")";
                SqlDataAdapter medcommand = new SqlDataAdapter();
                medcommand.InsertCommand = new SqlCommand(dsql1, ndConnHandle1);
                medcommand.InsertCommand.ExecuteNonQuery();
            }
            insSaveButton.Enabled = false;
            insSaveButton.BackColor = Color.Gainsboro;
            textBox1.Text = "";
            getTrain();
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
   //         getCountry();
     //       comboBox1.SelectedIndex = -1;
     //       comboBox1.SelectedValue.Equals(0); 
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
//            MessageBox.Show("The ")
//            if (textBox1.Text != "") { getCountry(); }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            MessageBox.Show("wer are leaving");
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex>-1 && comboBox1.SelectedValue.ToString()!="System.Data.DataRowView")
            {
                string dval = Convert.ToString(comboBox1.SelectedValue);
         //       AllClear2Go();
            }
        }

        private void comboBox1_SelectedValueChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_ValueMemberChanged(object sender, EventArgs e)
        {
//            MessageBox.Show("Value member changed");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }


        private void getcity(int tncouid,int tntime)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                if (tntime == 1)
                {
                    string basesql0 = "select city_name,city_id,cou_id from city where cou_id = " + tncouid + " order by city_name";
                    SqlDataAdapter baseda0 = new SqlDataAdapter(basesql0, ndConnHandle);
                    DataTable inscityview = new DataTable();
                    baseda0.Fill(inscityview);
                    if (inscityview.Rows.Count > 0)
                    {
                        comboBox20.DataSource = inscityview.DefaultView;
                        comboBox20.DisplayMember = "city_name";
                        comboBox20.ValueMember = "city_id";
                        comboBox20.SelectedIndex = -1;
                    }
                }
                else
                {
                    string basesql1 = "select city_name,city_id,cou_id from city where cou_id = " + tncouid + " order by city_name";
                    SqlDataAdapter baseda1 = new SqlDataAdapter(basesql1, ndConnHandle);
                    DataTable fincityview = new DataTable();
                    baseda1.Fill(fincityview);
                    if (fincityview.Rows.Count > 0)
                    {
                        comboBox2.DataSource = fincityview.DefaultView;
                        comboBox2.DisplayMember = "city_name";
                        comboBox2.ValueMember = "city_id";
                        comboBox2.SelectedIndex = -1;
                    }
                }
            }
        }
  

        private void page01ok() //training institution details input
        {
            bool llbasic = comboBox21.SelectedIndex > -1 && comboBox20.SelectedIndex > -1 ? true : false;
            if (textBox1.Text != "" && textBox43.Text != "" && textBox2.Text != "" && llbasic)
            {
                insSaveButton.Enabled = true;
                insSaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                insSaveButton.Enabled = false;
                insSaveButton.BackColor = Color.Gainsboro;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
            inst_id	int	Unchecked
            inst_name	char(100)	Unchecked
            cou_id	int	Unchecked
            city_id	int	Checked
            tel	char(20)	Checked
            email	char(50)	Checked
             */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox21_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox21.Focused)
            {
                getcity(Convert.ToInt16(comboBox21.SelectedValue), 1);
                page01ok();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Focused)
            {
                getcity(Convert.ToInt16(comboBox1.SelectedValue), 2);
            }
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void comboBox20_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox20.Focused )
            {
                page01ok();
            }
        }

        private void textBox43_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            page01ok();
        }

        private void insSaveButton_Click(object sender, EventArgs e)
        {
            updInsDetails();
        }


        private void updInsDetails()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                if (glNewIns)
                {
                    string cquery0 = "insert into institution (ins_name,tel,email,cou_id,city_id) ";
                    cquery0 += " values (@tins_name,@tTel,@temail,@tcou_id,@tcityid)";

                    SqlDataAdapter cuscommand0 = new SqlDataAdapter();
                    cuscommand0.InsertCommand = new SqlCommand(cquery0, ndConnHandle);

                    cuscommand0.InsertCommand.Parameters.Add("@tins_name", SqlDbType.VarChar).Value = textBox1.Text.ToString().Trim();
                    cuscommand0.InsertCommand.Parameters.Add("@tTel", SqlDbType.VarChar).Value = textBox43.Text.ToString().Trim();
                    cuscommand0.InsertCommand.Parameters.Add("@temail", SqlDbType.VarChar).Value = textBox2.Text.ToString().Trim();
                    cuscommand0.InsertCommand.Parameters.Add("@tcou_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox21.SelectedValue);
                    cuscommand0.InsertCommand.Parameters.Add("@tcityid", SqlDbType.Int).Value = Convert.ToInt16(comboBox20.SelectedValue);

                    ndConnHandle.Open();
                    cuscommand0.InsertCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                    MessageBox.Show("Institution details added successfully");
                }
                else
                {
                    string cquery = "update institution set ins_name = @tins_name,tel=@tTel,email=@temail,cou_id=@tcou_id,city_id=@tcityid ";
                    cquery += " where ins_id = @tinsid";

                    SqlDataAdapter cuscommand = new SqlDataAdapter();
                    cuscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle);

                    cuscommand.UpdateCommand.Parameters.Add("@tins_name", SqlDbType.VarChar).Value = textBox1.Text.ToString().Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tTel", SqlDbType.VarChar).Value = textBox43.Text.ToString().Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@temail", SqlDbType.VarChar).Value = textBox2.Text.ToString().Trim();
                    cuscommand.UpdateCommand.Parameters.Add("@tcou_id", SqlDbType.Int).Value = Convert.ToInt16(comboBox21.SelectedValue);
                    cuscommand.UpdateCommand.Parameters.Add("@tcityid", SqlDbType.Int).Value = Convert.ToInt16(comboBox20.SelectedValue);

                    ndConnHandle.Open();
                    cuscommand.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                    MessageBox.Show("Institution updated successfully");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            glNewIns = true;
        }
    }
}
