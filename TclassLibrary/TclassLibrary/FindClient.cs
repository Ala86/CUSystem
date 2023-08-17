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
    public partial class FindClient : Form
    {

        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        string tcTable = string.Empty;

        DataTable findMemberView = new DataTable();
        bool glMultipleSelect = false;
        public string returnValue { get; set; }

        public FindClient(string tcCos,int tnCompid, string tcLoca, int seltype,string tcTableName)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
            tcTable = tcTableName;
            radioButton1.Text = (tcTable == "cusreg" ? "Member ID" : (tcTable == "patients" ? "Client ID" : "Staff ID"));
            label1.Text = (tcTable == "cusreg" ? "Member ID" : (tcTable == "patients" ? "Client ID" : "Staff ID"));

            radioButton5.Text = (tcTable == "cusreg" ? "Payroll ID" : (tcTable == "patients" ? "IP ID" : "Employee ID"));
            findMemberList.Columns[1].HeaderText=(tcTable == "cusreg" ? "Member ID" : (tcTable == "patients" ? "Client ID" : "Staff ID"));
            findMemberList.Columns[7].HeaderText = (tcTable == "cusreg" ? "Payroll ID" : (tcTable == "patients" ? "IP ID" : "Employee ID"));

            if (seltype==1) //multiple is not allowed
            {
                findMemberList.Columns["mulSelect"].ReadOnly = true;
                glMultipleSelect = false;
            }
            else
            {
                findMemberList.Columns["mulSelect"].ReadOnly = false;
                glMultipleSelect = true;
            }
        }

        private void FindClient_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Member  Search >>";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            findMemberView.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            if (radioButton1.Checked)
            {
                label1.Text = (tcTable == "cusreg" ? "Member ID" : (tcTable == "patients" ? "Client ID" : "Staff ID"));
//                label1.Text = "Client ID";
                label2.Visible = false;
                label3.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
            }
            textBox1.Focus();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            findMemberView.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            if (radioButton2.Checked)
            {
                label1.Text = "First Name" ;
                label2.Visible = true ;
                label3.Visible = true ;
                textBox2.Visible = true;
                textBox3.Visible = true;
            }
            textBox1.Focus();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            findMemberView.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            if (radioButton3.Checked)
            {
                label1.Text = "Tel. No.";
                label2.Visible = false;
                label3.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
            }
            textBox1.Focus();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            findMemberView.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            if (radioButton4.Checked)
            {
                label1.Text = "ID Numb.";
                label2.Visible = false;
                label3.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
            }
            textBox1.Focus();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            findMemberView.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            if (radioButton5.Checked)
            {
                label1.Text = "IP ID";
                label2.Visible = false;
                label3.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
            }
            textBox1.Focus();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
//                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
  //              AllClear2Go();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            if(radioButton1.Checked )
            {
                //if(textBox1.Text !="")
                //{
                //    textBox1.Text = textBox1.Text.Trim().ToString().PadLeft(6, '0');
                //}
                searchClient();
            }
        }

        private void searchClient()
        {
            string tcCustcode = textBox1.Text!="" ? textBox1.Text.Trim().ToString().PadLeft(6,'0') :"";
            textBox1.Text = tcCustcode;// != "" ? tcCustcode : "";
//            MessageBox.Show("We are inside searchclient with code "+tcCustcode);
            if (radioButton1.Checked && tcCustcode.Length==6)
            {
                getrdata(1, tcCustcode);
            }
            else
            {
                if (radioButton2.Checked)
                {
                    if (textBox1.Text != string.Empty && textBox2.Text == string.Empty && textBox3.Text == string.Empty) //search by first name only
                    {
                        getname(1, textBox1.Text.Trim().ToString(), "", "");
                    }

                    if (textBox1.Text == string.Empty && textBox2.Text != string.Empty && textBox3.Text == string.Empty) //search by middle name only
                    {
                        getname(2, "", textBox2.Text.Trim().ToString(), "");
                    }

                    if (textBox1.Text == string.Empty && textBox2.Text == string.Empty && textBox3.Text != string.Empty) //search by last name only
                    {
                        getname(3, "", "", textBox3.Text.Trim().ToString());
                    }


                    if (textBox1.Text != string.Empty && textBox2.Text != string.Empty && textBox3.Text == string.Empty) //search by first and middle name only
                    {
                        getname(4, textBox1.Text.Trim().ToString(), textBox2.Text.Trim().ToString(), "");
                    }


                    if (textBox1.Text != string.Empty && textBox2.Text == string.Empty && textBox3.Text != string.Empty) //search by first and last name only
                    {
                        getname(5, textBox1.Text.Trim().ToString(), "", textBox3.Text.Trim().ToString());
                    }

                    if (textBox1.Text == string.Empty && textBox2.Text != string.Empty && textBox3.Text != string.Empty) //search by middle and last name only
                    {
                        getname(6, "", textBox2.Text.Trim().ToString(), textBox3.Text.Trim().ToString());
                    }

                    if (textBox1.Text != string.Empty && textBox2.Text != string.Empty && textBox3.Text != string.Empty) //search by first, middle and last name
                    {
                        getname(7, textBox1.Text.Trim().ToString(), textBox2.Text.Trim().ToString(), textBox3.Text.Trim().ToString());
                    }
                }
                else
                {
                    string tcTel = textBox1.Text.Trim().ToString();
                    if (radioButton3.Checked)
                    {
                        getrdata(2, tcTel);
                    }
                    else
                    {
                        string tcid = textBox1.Text.Trim().ToString();
                        if (radioButton4.Checked)
                        {
                            //          MessageBox.Show("Search by ID number "+tcid );
                            getrdata(3, tcid);
                        }
                        else
                        {
                            string tcip = textBox1.Text.Trim().ToString();
                            if (radioButton5.Checked)
                            {
                                //            MessageBox.Show("Search by IP ID "+tcip);
                                getrdata(4, tcip);
                            }
                            else
                            {
                                string tcnok = textBox1.Text.Trim().ToString();
                                //          MessageBox.Show("Search by NOK ID "+tcnok);
                                getrdata(5, tcnok);
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tcCustcode = textBox1.Text.Trim().ToString();
            if (radioButton1.Checked)
            {
                getrdata(1, tcCustcode);
            }
            else
            {
                if(radioButton2.Checked )
                {
                    if(textBox1.Text!=string.Empty && textBox2.Text==string.Empty && textBox3.Text ==string.Empty) //search by first name only
                    {
                        getname(1, textBox1.Text.Trim().ToString(), "", "");
                    }

                    if (textBox1.Text == string.Empty && textBox2.Text != string.Empty && textBox3.Text == string.Empty) //search by middle name only
                    {
                        getname(2,"", textBox2.Text.Trim().ToString(), "");
                    }

                    if (textBox1.Text == string.Empty && textBox2.Text == string.Empty && textBox3.Text != string.Empty) //search by last name only
                    {
                        getname(3,"","", textBox3.Text.Trim().ToString());
                    }


                    if (textBox1.Text != string.Empty && textBox2.Text != string.Empty && textBox3.Text == string.Empty) //search by first and middle name only
                    {
                        getname(4, textBox1.Text.Trim().ToString(), textBox2.Text.Trim().ToString(), "");
                    }


                    if (textBox1.Text != string.Empty && textBox2.Text == string.Empty && textBox3.Text != string.Empty) //search by first and last name only
                    {
                        getname(5, textBox1.Text.Trim().ToString(), "", textBox3.Text.Trim().ToString());
                    }

                    if (textBox1.Text == string.Empty && textBox2.Text != string.Empty && textBox3.Text != string.Empty) //search by middle and last name only
                    {
                        getname(6,"", textBox2.Text.Trim().ToString(), textBox3.Text.Trim().ToString());
                    }

                    if (textBox1.Text != string.Empty && textBox2.Text != string.Empty && textBox3.Text != string.Empty) //search by first, middle and last name
                    {
                        getname(7, textBox1.Text.Trim().ToString(), textBox2.Text.Trim().ToString(), textBox3.Text.Trim().ToString());
                    }
                }
                else
                {
                    string tcTel = textBox1.Text.Trim().ToString();
                    if (radioButton3.Checked )
                    {
                        getrdata(2, tcTel);
                    }
                    else
                    {
                        string tcid = textBox1.Text.Trim().ToString();
                        if(radioButton4.Checked)
                        {
                  //          MessageBox.Show("Search by ID number "+tcid );
                            getrdata(3, tcid);
                        }
                        else
                        {
                            string tcip = textBox1.Text.Trim().ToString();
                            if(radioButton5.Checked )
                            {
                    //            MessageBox.Show("Search by IP ID "+tcip);
                                getrdata(4, tcip);
                            }
                            else
                            {
                                string tcnok = textBox1.Text.Trim().ToString();
                      //          MessageBox.Show("Search by NOK ID "+tcnok);
                                getrdata(5, tcnok);
                            }
                        }
                    }
                }
            }
        }//end of search buttonm

        private void getrdata(int optionid, string searchkey)
        {
            findMemberView.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                switch (optionid)
                {
                    case 1:  //search by client id
//                      MessageBox.Show("inside getrdata with searchkey = " + searchkey + " and the table = " + tcTable+" and compid = "+ncompid);
//                      string dsql = "Exec tsp_New_Client_One " + ncompid + "," +"'"+ searchkey+"'"; this is the old one
                        string dsql = "Exec tsp_Search_Client_ID  " + ncompid +",'"+tcTable+"','" + searchkey + "'";  //this is the new one
                        SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                        da.Fill(findMemberView);
                        break;
                    case 2: //search by client telephone number
                        string dsqltel = "Exec Tsp_One_Client_Tel " + ncompid + "," + "'" + searchkey + "'";
                        SqlDataAdapter datel = new SqlDataAdapter(dsqltel, ndConnHandle);
                        datel.Fill(findMemberView);
                        break;
                    case 3:  //search by ID Number
                        string dsqlid = "Exec tsp_One_Client_ID " + ncompid + "," + "'" + searchkey + "'";
                        SqlDataAdapter daid = new SqlDataAdapter(dsqlid, ndConnHandle);
                        daid.Fill(findMemberView);
                        break;
                    case 4: //search by IP ID
                        string dsqlIP = "Exec tsp_GetIPID " + ncompid + "," + "'" + searchkey + "'";
                        SqlDataAdapter daIP = new SqlDataAdapter(dsqlIP, ndConnHandle);
                        daIP.Fill(findMemberView);
                        break;
                    case 5: //search by NOK ID
                        string dsqlnok = "Exec tsp_NOK_Search " + ncompid + "," + "'" + searchkey + "'";
                        SqlDataAdapter danok = new SqlDataAdapter(dsqlnok, ndConnHandle);
                        danok.Fill(findMemberView);
                        break;
                }
                if (findMemberView.Rows.Count > 0)
                {
                    findMemberList.AutoGenerateColumns = false;
                    findMemberList.DataSource = findMemberView.DefaultView;
                    findMemberList.Columns[1].DataPropertyName = tcTable == "staff" ? "staffno" : "ccustcode";
                    findMemberList.Columns[2].DataPropertyName = "fname";
                    findMemberList.Columns[3].DataPropertyName = "mname";
                    findMemberList.Columns[4].DataPropertyName = "lname";
                    findMemberList.Columns[5].DataPropertyName = tcTable == "staff" ? "mobilenb" : "ctel"; //"pc_tel";
                    findMemberList.Columns[6].DataPropertyName = tcTable == "staff" ? "idnumb" : "cpassno";// "cpassno";       
                    findMemberList.Columns[7].DataPropertyName = (tcTable == "cusreg" ? "payroll_id" : (tcTable == "patients" ? "IP ID" : "Employee ID"));  //"ipid";
                    ndConnHandle.Close();
                    //findMemberList.Focus();
                    //for (int i = 0; i < 4; i++)
                    //{
                    //    //                        DataGridViewRow drow = new  DataGridViewRow();
                    //    findMemberView.Rows.Add();
                    //}
                }
                else { MessageBox.Show("we could not find the client"); }
            }
        }

        private void getname(int optionid, string tcFname,string tcMname,string tcLname)
        {
            findMemberView.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                switch (optionid)
                {
                    case 1:             //search by first name only
                        string dsql = "exec tsp_SearchFirstName " + ncompid + "," + "'" + tcFname + "'";
                        SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                        da.Fill(findMemberView);
                        break;
                    case 2:             //search by middle name only
                        string dsql1 = "exec tsp_SearchMidName " + ncompid + "," + "'" + tcMname + "'";
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                        da1.Fill(findMemberView);
                        break;
                    case 3:             //search by last name only
                        string dsql2 = "exec tsp_SearchLastName " + ncompid + "," + "'" + tcLname + "'";
                        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                        da2.Fill(findMemberView);
                        break;
                    case 4:             //search by first and middle name only
                        string dsql3 = "exec tsp_SearchFirstMidName " + ncompid + "," + "'" + tcFname + "'" +"," + "'" + tcMname + "'";
                        SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle);
                        da3.Fill(findMemberView);
                        break;
                    case 5:             //search by first and last name only
                        string dsql4 = "exec tsp_SearchFirstLastName " + ncompid + "," + "'" + tcFname + "'" + "," + "'" + tcLname + "'";
                        SqlDataAdapter da4 = new SqlDataAdapter(dsql4, ndConnHandle);
                        da4.Fill(findMemberView);
                        break;
                    case 6:             //search by middle and last name only
                        string dsql5 = "exec tsp_SearchMidLastName " + ncompid + "," + "'" + tcMname + "'" + "," + "'" + tcLname + "'";
                        SqlDataAdapter da5 = new SqlDataAdapter(dsql5, ndConnHandle);
                        da5.Fill(findMemberView);
                        break;
                    case 7:             //search by first, middle and last name
                        string dsql6 = "exec tsp_SearchFirstMidLastName " + ncompid + "," + "'" + tcFname + "'" + "," + "'" + tcMname + "'" + "," + "'" + tcLname + "'";
                        SqlDataAdapter da6 = new SqlDataAdapter(dsql6, ndConnHandle);
                        da6.Fill(findMemberView);
                        break;
                }
                if (findMemberView.Rows.Count > 0)
                {
                    findMemberList.AutoGenerateColumns = false;
                    findMemberList.DataSource = findMemberView.DefaultView;
                    findMemberList.Columns[1].DataPropertyName = "ccustcode";
                    findMemberList.Columns[2].DataPropertyName = "fname";
                    findMemberList.Columns[3].DataPropertyName = "mname";
                    findMemberList.Columns[4].DataPropertyName = "lname";
                    findMemberList.Columns[5].DataPropertyName = "pc_tel";
                    findMemberList.Columns[6].DataPropertyName = "cpassno";      // "age";
                    findMemberList.Columns[7].DataPropertyName = "ipid";
                    ndConnHandle.Close();
                    findMemberList.Focus();
                    for (int i = 0; i < 4; i++)
                    {
                        //                        DataGridViewRow drow = new  DataGridViewRow();
                        findMemberView.Rows.Add();
                    }
                }
                else { MessageBox.Show("we could not find the client"); }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(glMultipleSelect)
            {
                MessageBox.Show("We will come back to multiple selection");
            }
            else
            {
                string dselval= tcTable == "staff" ? findMemberView.Rows[findMemberList.CurrentCell.RowIndex]["staffno"].ToString() : findMemberView.Rows[findMemberList.CurrentCell.RowIndex]["ccustcode"].ToString() ;
                returnValue = dselval; 
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void clientgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (findMemberList.Columns[e.ColumnIndex].Name == "itemSelect")
            {
                string dselval = tcTable == "staff" ? findMemberView.Rows[findMemberList.CurrentCell.RowIndex]["staffno"].ToString() : findMemberView.Rows[findMemberList.CurrentCell.RowIndex]["ccustcode"].ToString();
                returnValue = dselval; 
                this.DialogResult = DialogResult.OK;
                this.Close();
            } 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked) //multiple selection to activate checkbox in data grid view
            {
                MessageBox.Show("We will enable the multiple select");
                findMemberList.Columns["mulSelect"].ReadOnly = false; 
            }
            else
            {
                MessageBox.Show("We will disable the multiple select");
                findMemberList.Columns["mulSelect"].ReadOnly = true;
            }
        }
    }
}
