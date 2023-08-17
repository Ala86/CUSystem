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

namespace WinTcare
{
    public partial class todayList : Form
    {
        DataTable clientview = new DataTable();
        DataTable posview = new DataTable();
        int gnClientList = 0;
        public todayList()
        {
            InitializeComponent();
        }

        private void todayList_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Today's Client List>>";
            label36.Text = globalvar.gcCopyRight;
            getclientList();
            if(gnClientList>0)
            {
                firstclient();
            }
            clientgrid.Focus();
        }

        private void firstclient()
        {
            string firstcode = clientview.Rows[0]["ccustcode"].ToString();
            if (clientview.Rows.Count >= 0)
            {
                textBox1.Text = firstcode;      // clientview.Rows[0]["ccustcode"].ToString(); // row.Cells[2].Value.ToString();
                DateTime nstar = Convert.ToDateTime(clientview.Rows[0]["date_creat"]);       // Convert.ToDateTime(row.Cells[3].Value);
                textBox9.Text = Convert.ToString(DateTime.Now.Year - Convert.ToDateTime(clientview.Rows[0]["ddatebirth"]).Year);        //  row.Cells[2] nstar.Year);     
                textBox10.Text = (Convert.ToBoolean(clientview.Rows[0]["gender"])==true ? "Male" : "Female");
                textBox11.Text = clientview.Rows[0]["pc_tel"].ToString();       // row.Cells[5].Value.ToString();
                textBox2.Text = clientview.Rows[0]["vistime"].ToString().Substring(0, 8);      // row.Cells[6].Value.ToString().Substring(0, 8);
            }
            getvisit(firstcode);
            clientpos(firstcode);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void getclientList()
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_Today_Client_List  " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(clientview);
                if (clientview.Rows.Count >0 )
                {
                    gnClientList = clientview.Rows.Count;
                    label13.Text = "Number of Clients = " + clientview.Rows.Count.ToString();
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = clientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "fname";
                    clientgrid.Columns[1].DataPropertyName = "lname";
                    clientgrid.Columns[2].DataPropertyName = "ccustcode";
                    clientgrid.Columns[3].DataPropertyName = "ddatebirth";      // "age";
                    clientgrid.Columns[4].DataPropertyName = "gender";
                    clientgrid.Columns[5].DataPropertyName = "pc_tel";
                    clientgrid.Columns[6].DataPropertyName = "vistime";
                    //            clientgrid.Columns[7].DataPropertyName = "srvcent";
                    //156, 228, 250
//                    clientgrid.
                    ndConnHandle.Close();
                    for(int i =0;i<=30; i++)
                    {
                        //DataRow drow = new DataRow();
                        clientview.Rows.Add();

                    }
                }else { MessageBox.Show("No clients in queue today"); gnClientList = 0; }
            }
        }

          private void clientgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
  //         MessageBox.Show("We are in the clientgrid cell click");
//            if(e.RowIndex>=0)
               if (e.RowIndex >= 0)
                {

                //                DataGridViewRow row = this.clientgrid.Rows[e.RowIndex];
                //              textBox1.Text = row.Cells[2].Value.ToString();
                //            DateTime nstar = Convert.ToDateTime(row.Cells[3].Value);
                //          textBox9.Text = Convert.ToString(DateTime.Now.Year - Convert.ToDateTime(row.Cells[3].Value).Year);        //  row.Cells[2] nstar.Year);     
                //        textBox10.Text = (Convert.ToBoolean(row.Cells[4].Value) ? "Male" : "Female");
                //      textBox11.Text = row.Cells[5].Value.ToString();
                //    textBox2.Text = row.Cells[6].Value.ToString().Substring(0,8);

                textBox1.Text = clientview.Rows[e.RowIndex]["ccustcode"].ToString();
                DateTime nstar = Convert.ToDateTime(clientview.Rows[e.RowIndex]["date_creat"]);       // Convert.ToDateTime(row.Cells[3].Value);
                textBox9.Text = Convert.ToString(DateTime.Now.Year - Convert.ToDateTime(clientview.Rows[e.RowIndex]["ddatebirth"]).Year);        //  row.Cells[2] nstar.Year);     
                textBox10.Text = (Convert.ToBoolean(clientview.Rows[e.RowIndex]["gender"]) == true ? "Male" : "Female");
                textBox11.Text = clientview.Rows[e.RowIndex]["pc_tel"].ToString();       // row.Cells[5].Value.ToString();
                textBox2.Text = clientview.Rows[e.RowIndex]["vistime"].ToString().Substring(0, 8);      // row.Cells[6].Value.ToString().Substring(0, 8);
            }
            else
            {
                MessageBox.Show("Something is not right, inform IT Dept");
            }
            getvisit(textBox1.Text.ToString());
            clientpos(textBox1.Text.ToString());
        }

        private void getvisit(string tcCustCode)
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
//            string dsql = "exec tsp_GetVisit_new   " + ncompid + "," + tcCustCode;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                SqlDataReader cUserDetails = null;
                SqlCommand cGetUser = new SqlCommand("exec tsp_GetVisit_new @ncompid,@custcode", ndConnHandle);
                cGetUser.Parameters.Add("@ncompid", SqlDbType.VarChar).Value = ncompid;
                cGetUser.Parameters.Add("@custcode", SqlDbType.VarChar).Value = tcCustCode;
                ndConnHandle.Open(); 
                cUserDetails = cGetUser.ExecuteReader();
                cUserDetails.Read();
                if (cUserDetails.HasRows == true)
                {
                    textBox3.Text = cUserDetails.GetInt32(0).ToString();  
//           ndConnHandle.Close();
                }
            }
        }


        private void clientpos(string tcCode)
        {
            posview.Clear();
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_One_Client_Position   " + ncompid +",'"+tcCode.ToString().Trim()+"'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(posview);
                if (posview.Rows.Count > 0)
                {
//                    bool dcash = Convert.ToBoolean(posview.Rows[0]["wit_cor"]);
         /*           string  dcas = Convert.ToString(posview.Rows[0]["wit_cas"]);
                    string dcor = Convert.ToString(posview.Rows[0]["wit_cor"]);
                    string dins = Convert.ToString(posview.Rows[0]["wit_ins"]);
                    string dnhi = Convert.ToString(posview.Rows[0]["wit_nhi"]);*/

             //       MessageBox.Show("The cash,cor,ins,nhi flag in clientpos for "+tcCode+" is " + dcas+","+dcor+","+dins+","+dnhi);
                    checkBox1.Checked = true;
                    checkBox7.Checked = (Convert.ToBoolean(posview.Rows[0]["ready4triage"]) == true && Convert.ToBoolean(posview.Rows[0]["walkin"]) == true && Convert.ToBoolean(posview.Rows[0]["wit_cas"]) == true ? true : false);

                    panel2.BackColor = Color.DarkGreen;
                    panel5.BackColor = (Convert.ToBoolean(posview.Rows[0]["wit_cas"]) == true ? Color.DarkGreen : Color.White);
                    panel7.BackColor = (Convert.ToBoolean(posview.Rows[0]["wit_cas"]) == true ? Color.DarkGreen : Color.White);
                    checkBox3.Checked = (Convert.ToBoolean(posview.Rows[0]["wit_cas"]) == true && Convert.ToBoolean(posview.Rows[0]["ready4triage"]) == true ? true : false);

                    panel8.BackColor = (Convert.ToBoolean(posview.Rows[0]["wit_cas"]) == true && Convert.ToBoolean(posview.Rows[0]["ready4triage"]) == true ? Color.DarkGreen : Color.White);
                    panel10.BackColor = (Convert.ToBoolean(posview.Rows[0]["wit_cas"]) == true && Convert.ToBoolean(posview.Rows[0]["ready4triage"]) == true ? Color.DarkGreen : Color.White);

                    textBox4.Text = posview.Rows[0]["q_id"].ToString();
                }
            }
        }


/*        private void clientpos(string tcCustCode)
        {
            string cs = globalvar.cos;
            DataTable posview = new DataTable();
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_One_Client_Position " + ncompid + ",'" + tcCustCode + "',";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(posview);
                if (posview.Rows.Count > 0)
                {
                    checkBox1.Checked = true;
//                    checkBox2.Checked = (Convert.ToBoolean(posview.Rows[0]["ready4triage"]) == true && Convert.ToBoolean(posview.Rows[0]["walkin"]) == true && Convert.ToBoolean(posview.Rows[0]["wit_cas"]) == true ? true : false);
  //                  checkBox3.Checked = (Convert.ToBoolean(posview.Rows[0]["ready4triage"]) == true && Convert.ToBoolean(posview.Rows[0]["walkin"])==true && Convert.ToBoolean(posview.Rows[0]["wit_cas"]) == true ? true : false);
                    panel2.BackColor = Color.DarkGreen;
    //                panel5.BackColor = Color.DarkGreen;
      //              panel6.BackColor = (cUserDetails.GetBoolean(Color.DarkGreen;
        //            panel7.BackColor = Color.DarkGreen;
                    textBox4.Text = posview.Rows[0]["q_id"].ToString();
                }
            }
        }*/

                    

                    //            MessageBox.Show("inside client position");
      //              string ncompid = globalvar.gnCompid.ToString().Trim();
        //    using (SqlConnection ndConnHandle = new SqlConnection(cs))
       /*     {
                //                SqlDataReader cUserDetails = null;
                SqlDataAdapter cUserDetails = new SqlDataAdapter();
                SqlCommand cGetUser = new SqlCommand("exec tsp_One_Client_Position @ncompid,@custcode", ndConnHandle);
                cGetUser.Parameters.Add("@ncompid", SqlDbType.VarChar).Value = ncompid;
                cGetUser.Parameters.Add("@custcode", SqlDbType.VarChar).Value = tcCustCode;
                ndConnHandle.Open();
                cUserDetails.ex
                cUserDetails = cGetUser.ExecuteReader();
                cUserDetails.Read();
                if (cUserDetails.HasRows == true)
                {
                    checkBox1.Checked = true; 
                    checkBox3.Checked = (cUserDetails.GetBoolean(3) == true && cUserDetails.GetBoolean(42) == true && cUserDetails.GetBoolean(18) == false ? true : false);
                    panel2.BackColor = Color.DarkGreen;
                    panel5.BackColor = Color.DarkGreen;
                    panel6.BackColor =(cUserDetails.GetBoolean(  Color.DarkGreen;
                    panel7.BackColor = Color.DarkGreen;

                    textBox4.Text = cUserDetails["srv_name"].ToString();
                    //                    if(cUserDetails.GetBoolean(3)==true && cUserDetails.GetBoolean(42)==true && cUserDetails.GetBoolean(18)==false)
                    //                  {
                    //                        MessageBox.Show("we have finished at cashier");
                    //                    checkBox3.Checked = true;
                    //		.check3.Value = Iif(wit_cas And ready4triage And !lwalkin, 1, 0) && Paypoint && cashier
                    //      .shape7.BackColor = Iif(wit_cas And ready4triage And !lwalkin, Rgb(0, 180, 0), Rgb(255, 255, 255))
                    //we would like to know if this client has finished with cashier
                    //              }
                    //                    textBox3.Text = cUserDetails.GetInt32(0).ToString();
                    //                    ndConnHandle.Close();
                }
                else { MessageBox.Show("cuserdetails is empty"); }*/
            
            /*
            With Thisform.pageframe1.paGE1
    sn = SQLExec(gnConnHandle, "exec tsp_One_Client_Position ?gnCompid, ?gcPatientCode", "ClView")

    If sn> 0 And Reccount() > 0
 * brow
 
         gnPat_ID = pat_id
 
         gcClientAccount = cacctnumb
 
         gnClientAge = p_age
 
         gnClientRes = res_type
 
         gnVisitNo = visno
 
         lLab = sent2lab
 
         lRad = sent2rad
 
         lOpt = sent2opt
 
         lPre = prescribed
 
         lRip = sent2pat
 
         lwalkin = Iif(walkin,.T.,.F.)
         .text3.Value = visno
         .check1.Value = 1 && Registration

         .shape4.BackColor = Iif(!wit_cas And !lwalkin, Rgb(0, 180, 0), Rgb(255, 255, 255))
         .shaPE3.BackColor = Iif(!lwalkin, Rgb(0, 180, 0), Rgb(255, 255, 255))

         .shape5.BackColor = Iif(wit_cas And !lwalkin, Rgb(0, 180, 0), Rgb(255, 255, 255))
         .check1.BackStyle = 1
         .check1.BackColor = Rgb(0, 180, 0)
         .check1.ForeColor = Rgb(255, 255, 255)
 *       .shaPE3.BackColor = Rgb(0, 180, 0)
         .shaPE12.BackColor = Iif(wit_cas And !lwalkin, Rgb(0, 180, 0), Rgb(255, 255, 255))

         .check2.Value = Iif(cor_conf Or ins_conf Or nhi_conf, 1, 0) && Cover Management
         .shape6.BackColor = Iif((cor_conf Or ins_conf Or nhi_conf) And atCopay = 0, Rgb(0, 180, 0), Rgb(255, 255, 255))


        .shape8.BackColor = Iif((cor_conf Or ins_conf Or nhi_conf) And atCopay = 0, Rgb(0, 180, 0),Rgb(255, 255, 255))
		.shape13.BackColor = Iif(wit_cas And !lwalkin And(lpaid Or lconsult Or lreturned Or ready4triage), Rgb(0, 180, 0), Rgb(255, 255, 255))
        .shape9.BackColor = Iif(!lwalkin And(((cor_conf Or ins_conf Or nhi_conf) And atCopay = 0) Or ready4triage) ,Rgb(0, 180, 0),Rgb(255, 255, 255))

		.check4.Value = Iif(ltriage And !lwalkin, 1, 0) && Triage
        .shape10.BackColor = Iif((ltriage Or lconsult) And !lwalkin,Rgb(0, 180, 0),Rgb(255, 255, 255))

		.line29.BorderColor = Iif(lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .shape14.BackColor = Iif(lwalkin, Rgb(0, 180, 0), Rgb(255, 255, 255))

        .check5.Value = Iif(lconsult And !lwalkin, 1, 0) && Consultation
        .shape11.BackColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And !lwalkin,Rgb(0, 180, 0),Rgb(255, 255, 255))
		.shape44.BackColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And wit_cas, Rgb(0, 180, 0),Rgb(255, 255, 255))
		.shape45.BackColor = Iif(wit_cas And(atLab = 1 Or atRad = 1 Or atopt = 1 Or atPha = 1), Rgb(0, 180, 0), Rgb(255, 255, 255))
        .shape46.BackColor = Iif((wit_cas And lwalkin) Or(sent2lab Or sent2rad Or sent2opt Or prescribed) ,Rgb(0, 180, 0),Rgb(255, 255, 255))
*       .shape46.BackColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And !wit_cas ,Rgb(0, 180, 0),Rgb(255, 255, 255))
		.shape47.BackColor = Iif(atLab = 1 Or atRad = 1 Or atopt = 1 Or atPha = 1, Rgb(0, 180, 0), Rgb(255, 255, 255))
        .shape48.BackColor = Iif(atLab = 1 Or atRad = 1 Or atopt = 1 Or atPha = 1 Or(!wit_cas And lconsult And !lwalkin), Rgb(0, 180, 0), Rgb(255, 255, 255))

* **********************Arrows
        .line1.BorderColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And !wit_cas And !lwalkin,Rgb(255, 255, 255),Rgb(255, 0, 0))
		.label4.ForeColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And !wit_cas And !lwalkin,Rgb(255, 255, 255),Rgb(255, 0, 0))

		.line37.Visible = Iif(lwalkin,.T.,.T.)
        .label44.Visible = Iif(lwalkin,.T.,.T.)

        .line37.BorderColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And !wit_cas And !lwalkin,Rgb(255, 255, 255),Rgb(255, 0, 0))
		.label44.ForeColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And !wit_cas And !lwalkin,Rgb(255, 255, 255),Rgb(255, 0, 0))

		.line2.BorderColor = Iif(atLab = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label5.ForeColor = Iif(atLab = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line3.BorderColor = Iif(atRad = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label8.ForeColor = Iif(atRad = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line36.Visible = Iif(lwalkin,.T.,.F.)
        .label43.Visible = Iif(lwalkin,.T.,.F.)
        .line36.BorderColor = Iif(lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label43.ForeColor = Iif(lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))


        .line4.BorderColor = Iif(atopt = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label10.ForeColor = Iif(atopt = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line5.BorderColor = Iif(atPha = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label11.ForeColor = Iif(atPha = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line6.BorderColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And !lwalkin,Rgb(255, 255, 255),Rgb(255, 0, 0))
		.label12.ForeColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And !lwalkin,Rgb(255, 255, 255),Rgb(255, 0, 0))

		.line7.BorderColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And wit_cas, Rgb(255, 255, 255),Rgb(255, 0, 0))
		.label13.ForeColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And wit_cas, Rgb(255, 255, 255),Rgb(255, 0, 0))

		.line8.BorderColor = Iif(wit_cas And(atLab = 1 Or atRad = 1 Or atopt = 1 Or atPha = 1), Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label14.ForeColor = Iif(wit_cas And(atLab = 1 Or atRad = 1 Or atopt = 1 Or atPha = 1), Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line9.BorderColor = Iif(ltriage And !lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label15.ForeColor = Iif(ltriage And !lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line10.BorderColor = Iif(atLab = 1 Or atRad = 1 Or atopt = 1 Or atPha = 1 Or(!wit_cas And lconsult And !lwalkin), Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label16.ForeColor = Iif(atLab = 1 Or atRad = 1 Or atopt = 1 Or atPha = 1 Or(!wit_cas And lconsult And !lwalkin), Rgb(255, 255, 255), Rgb(255, 0, 0))

        .shape22.BackColor = Iif(atopt = 1 Or atPha = 1, Rgb(0, 180, 0), Rgb(255, 255, 255))

        .line11.BorderColor = Iif(atopt = 1 Or atPha = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label17.ForeColor = Iif(atopt = 1 Or atPha = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line12.BorderColor = Iif(atRad = 1 Or atopt = 1 Or atPha = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label18.ForeColor = Iif(atRad = 1 Or atopt = 1 Or atPha = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line13.BorderColor = Iif(atPha = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label19.ForeColor = Iif(atPha = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line14.BorderColor = Iif(wit_cas And ready4triage And !lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label20.ForeColor = Iif(wit_cas And ready4triage And !lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line15.BorderColor = Iif(!wit_cas And !lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label21.ForeColor = Iif(!wit_cas And !lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line16.BorderColor = Iif(cor_conf Or ins_conf Or nhi_conf, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label22.ForeColor = Iif(cor_conf Or ins_conf Or nhi_conf, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line17.BorderColor = Iif(((cor_conf Or ins_conf Or nhi_conf) Or ready4triage) And !lwalkin,Rgb(255, 255, 255),Rgb(255, 0, 0))
		.label23.ForeColor = Iif(((cor_conf Or ins_conf Or nhi_conf) Or ready4triage) And !lwalkin,Rgb(255, 255, 255),Rgb(255, 0, 0))

		.line18.BorderColor = Iif(sent2Admit, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label24.ForeColor = Iif(sent2Admit, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line19.BorderColor = Iif(atPat = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label25.ForeColor = Iif(atPat = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line20.BorderColor = Iif(discharged, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label26.ForeColor = Iif(discharged, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line21.BorderColor = Iif(discharged, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label27.ForeColor = Iif(discharged, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line22.BorderColor = Iif(wit_cas And !lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label28.ForeColor = Iif(wit_cas And !lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line23.BorderColor = Iif(!lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label29.ForeColor = Iif(!lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line24.BorderColor = Iif(wit_cas And !lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label30.ForeColor = Iif(wit_cas And !lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0))



*   .line25.BorderColor = Iif(wit_cas, Rgb(255, 255, 255), Rgb(255, 0, 0)) Funeral home, TBD
*   .label31.ForeColor = Iif(wit_cas, Rgb(255, 255, 255), Rgb(255, 0, 0)) Funeral home, TBD

		.line26.BorderColor = Iif(admitted, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label32.ForeColor = Iif(admitted, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line27.BorderColor = Iif(atCle = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label33.ForeColor = Iif(atCle = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))

        .line28.BorderColor = Iif(atCle = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label34.ForeColor = Iif(atCle = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))

*       .line29.BorderColor = Iif(atSel, Rgb(255, 255, 255), Rgb(255, 0, 0)) && self request, TBD
               .label35.ForeColor = Iif(lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0)) && self request, TBD


                     .line30.BorderColor = Iif(atPat = 1, Rgb(255, 255, 255), Rgb(255, 0, 0)) && self request, TBD
                              .label36.ForeColor = Iif(atPat = 1, Rgb(255, 255, 255), Rgb(255, 0, 0)) && self request, TBD

                                      .line35.BorderColor = Iif(atCopay = 1 And !lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0)) && Clients at cashier for co - pay
                                           .label42.ForeColor = Iif(atCopay = 1 And !lwalkin, Rgb(255, 255, 255), Rgb(255, 0, 0)) && Clients at Cashier for co - pay
                                               .shape49.BackColor = Iif(atCopay = 1 And !lwalkin, Rgb(0, 180, 0), Rgb(255, 255, 255)) && Clients at Cashier for co - pay

                                               * ************end of arrows



                                                       .shape15.BackColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And !wit_cas And !lwalkin, Rgb(0, 180, 0), Rgb(255, 255, 255))
                        *       .shape15.BackColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And !wit_cas,Rgb(0, 180, 0),Rgb(255, 255, 255))
		.shape16.BackColor = Iif((sent2lab Or sent2rad Or sent2opt Or prescribed) And !wit_cas And !lwalkin,Rgb(0, 180, 0),Rgb(255, 255, 255))

		.shape17.BackColor = Iif(atLab = 1, Rgb(0, 180, 0), Rgb(255, 255, 255)) && client is at lab but test not completed

.shape18.BackColor = Iif(atRad = 1, Rgb(0, 180, 0), Rgb(255, 255, 255)) && client is at rad but exams not completed
.shape19.BackColor = Iif(atopt = 1, Rgb(0, 180, 0), Rgb(255, 255, 255)) && client is at opt but proc not completed
.shape20.BackColor = Iif(atPha = 1, Rgb(0, 180, 0), Rgb(255, 255, 255)) && client is at pharmacy but not completed
		.shape21.BackColor = Iif(atRad = 1 Or atopt = 1 Or atPha = 1, Rgb(0, 180, 0), Rgb(255, 255, 255))
        .shape22.BackColor = Iif(atopt = 1 Or atPha = 1, Rgb(0, 180, 0), Rgb(255, 255, 255))
        .shape23.BackColor = Iif(atPha = 1, Rgb(0, 180, 0), Rgb(255, 255, 255))


        .check15.Value = Iif(wit_cas And(atLab = 1 Or atRad = 1 Or atopt = 1 Or atPha = 1), 1, 0) && cashier at second time

               .shape28.BackColor = Iif(discharged, Rgb(0, 180, 0), Rgb(255, 255, 255))
               .shape29.BackColor = Iif(discharged, Rgb(0, 180, 0), Rgb(255, 255, 255))
               .shape30.BackColor = Iif(atPat = 1, Rgb(0, 180, 0), Rgb(255, 255, 255))
               .shape31.BackColor = Iif(sent2Admit, Rgb(0, 180, 0), Rgb(255, 255, 255))

               .check10.Value = Iif(admitted, 1, 0)
               .check11.Value = Iif(admitted, 1, 0)
               .shape32.BackColor = Iif(admitted, Rgb(0, 180, 0), Rgb(255, 255, 255))
               .shape35.BackColor = Iif(atCle = 1, Rgb(0, 180, 0), Rgb(255, 255, 255))

       *       .check12.Value = Iif(sent2Pat, 1, 0) && Funeral Home
.shape34.BackColor = Iif(sent2pat, Rgb(0, 180, 0), Rgb(255, 255, 255))

.check13.Value = Iif(discharged, 1, 0) && Discharged
.check14.Value = Iif(lcleared, 1, 0) && cleared at security


        gnOldQueue = q_id
*   .text2.Value = Iif(q_id = 1, 'Out Patient Clinic', Iif(q_id = 2, 'TB Clinic', Iif(q_id = 3, 'MCH Clinic',;
            Iif(q_id = 4, 'CCC Clinic', Iif(q_id = 10, 'Physiotherapy', Iif(q_id = 28, 'Dental Clinic',;
            Iif(q_id = 29, 'Eye Clinic', Iif(q_id = 30, 'A&E Clinic', Iif(q_id = 31, 'ENT Clinic',;
            Iif(q_id = 32, 'GOPC Clinic', Iif(q_id = 33, 'MOPC Clinic', Iif(q_id = 34, 'SOPC Clinic',;
            Iif(q_id = 35, 'Dermatology', Iif(q_id = 36, 'Diabetic Clinic', Iif(q_id = 37, 'POPC Clinic',;
            Iif(q_id = 38, 'Post CS Clinic', Iif(q_id = 39, 'Neuro Surgery Clinic', Iif(q_id = 40, 'FNA Clinic',;
            Iif(q_id = 41, 'ECHO/ECG Clinic', Iif(q_id = 42, 'OGD Clinic', Iif(q_id = 43, 'BMA Clinic',;
            Iif(q_id = 44, 'Plastic Surgery Clinic', Iif(q_id = 998, 'In-Patient', '')))))))))))))))))))))))
		.text2.Value = srv_name

        If lLab         && laboratory

            Thisform.labtestdone

        Else
            .check6.Value = 0
            .shape24.BackColor = Rgb(255, 255, 255)
            .shape38.BackColor = Rgb(255, 255, 255)
            .line31.BorderColor = Rgb(255, 0, 0)
            .label37.ForeColor = Rgb(255, 0, 0)

        Endif

        If lRad && Radiology

            Thisform.radtestdone

        Else
            .check7.Value = 0
            .shape25.BackColor = Rgb(255, 255, 255)
            .shape43.BackColor = Rgb(255, 255, 255)

        Endif

        If lOpt && Procedure

            Thisform.opttestdone

        Else
            .check8.Value = 0
            .shape26.BackColor = Rgb(255, 255, 255)
            .shape42.BackColor = Rgb(255, 255, 255)
            .line33.BorderColor = Rgb(255, 0, 0)
            .label39.ForeColor = Rgb(255, 0, 0)

        Endif

        If lPre && Prescription

            Thisform.presdone

        Else
            .check9.Value = 0
            .shape27.BackColor = Rgb(255, 255, 255)
            .shape41.BackColor = Rgb(255, 255, 255)
            .line32.BorderColor = Rgb(255, 0, 0)
            .label38.ForeColor = Rgb(255, 0, 0)
            .line33.BorderColor = Rgb(255, 0, 0)
            .label39.ForeColor = Rgb(255, 0, 0)

        Endif

        .shape39.BackColor = Iif(.check6.Value = 1 Or.check7.Value = 1 Or.check8.Value = 1 Or.check9.Value = 1, Rgb(0, 180, 0), Rgb(255, 255, 255))
        .line34.BorderColor = Iif(.check6.Value = 1 Or.check7.Value = 1 Or.check8.Value = 1 Or.check9.Value = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))
        .label40.ForeColor = Iif(.check6.Value = 1 Or.check7.Value = 1 Or.check8.Value = 1 Or.check9.Value = 1, Rgb(255, 255, 255), Rgb(255, 0, 0))
        If lRip         && Pathology
            Thisform.ripdone
        Endif
    Else
*= sysmsg('Client position could not be determined')
    Endif
    lcold = Alias()
    sn = SQLExec(gnConnHandle, "select vistime from pat_visit where ccustcode = ?gcPatientCode and activesession=1", "ClView")
    If sn> 0 And Reccount() > 0
         .text1.Value = Chrtran(vistime, ':', '')
     Endif
     Select(lcold)
     .Refresh
 Endwith*/
      //  }


        private void clientgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("Row enter");
        }
    }
}
