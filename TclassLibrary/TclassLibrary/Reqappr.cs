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
    public partial class Reqappr : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        string tcuserid = string.Empty;
        DataTable itemview = new DataTable();
        DataTable autoview = new DataTable();

        public Reqappr(string tcCos, int tnCompid, string tcLoca,string gcUserid)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
            tcuserid = gcUserid;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Reqappr_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Requisition Approval >>";
            getRequisition();
            getAuthorisers();
        }


        private void getRequisition()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                /*
                 
        .column1.ControlSource =  "preqno"
		.column2.ControlSource =  "prodname"
		.column3.ControlSource =  "staffname"
		.column4.ControlSource =  "ordate"
		.column5.ControlSource =  "ordtime"
		.column6.ControlSource =  "srvname"
		.column7.ControlSource =  "Requested"
		.column8.ControlSource =  "approved"
		.column9.ControlSource =  "proper(staffpos)"
		.column10.ControlSource =  "topurchase"
		.column11.ControlSource =  "purapproved"

                 */
                itemview.Clear();
                ndConnHandle.Open();
                string dsql = "exec tsp_CentreRequisition_All " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(itemview);
                itemGrid.AutoGenerateColumns = false;
                itemGrid.DataSource = itemview.DefaultView;
                itemGrid.Columns[0].DataPropertyName = "preqno";
                itemGrid.Columns[1].DataPropertyName = "prod_name";
                itemGrid.Columns[2].DataPropertyName = "ord_date";
                itemGrid.Columns[3].DataPropertyName = "ord_time";
                itemGrid.Columns[4].DataPropertyName = "staffname";
                itemGrid.Columns[5].DataPropertyName = "dep_name";
                itemGrid.Columns[6].DataPropertyName = "des_name";
                itemGrid.Columns[7].DataPropertyName = "requested";
                itemGrid.Columns[8].DataPropertyName = "approved";
                itemGrid.Columns[9].DataPropertyName = "topurchase";
                itemGrid.Columns[10].DataPropertyName = "purapproved";

                ndConnHandle.Close();
//                textBox45.Text = itemview.Rows.Count.ToString();
  //              string tcemp = itemview.Rows[itemGrid.CurrentCell.RowIndex]["staffno"].ToString();
    //            string tcempname = itemview.Rows[itemGrid.CurrentCell.RowIndex]["fullname"].ToString();
//                textBox53.Text = tcemp;
  //              empAbsDetails(tcemp);
            }
        }

        private void getAuthorisers()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                autoview.Clear();
                ndConnHandle.Open();
                string dsql = "exec tsp_RequestAuthoriser_All " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(autoview);
                autGrid.AutoGenerateColumns = false;
                autGrid.DataSource = autoview.DefaultView;
                autGrid.Columns[0].DataPropertyName = "deptname";
                autGrid.Columns[1].DataPropertyName = "principal";
                autGrid.Columns[2].DataPropertyName = "proxy1";
                autGrid.Columns[3].DataPropertyName = "proxy2";

                ndConnHandle.Close();
                //textBox45.Text = itemview.Rows.Count.ToString();
                //string tcemp = itemview.Rows[itemGrid.CurrentCell.RowIndex]["staffno"].ToString();
                //string tcempname = itemview.Rows[itemGrid.CurrentCell.RowIndex]["fullname"].ToString();
            }
            //sn=SQLExec(gnConnHandle,"exec tsp_CentreRequisition_All ?gnCompid","Orderview")

        }
            /*
                sn=SQLExec(gnConnHandle,"exec sp_GetDept ?gnCompID","deptview")
        If !(sn>0 And Reccount()>0)
            =sysmsg("Department file is empty")
        Else
            .combo1.RowSource = 'allt(deptview.dep_name),dep_id'
        Endif

        sn=SQLExec(gnConnHandle,"exec tsp_GetExecutiveHod ?gnCompID,?gcUserid","HodView")
        If !(sn>0 And Reccount()>0)
            =sysmsg("User not found in HOD Group,inform IT DEPT")
            Thisform.nav_butt1.command6.Enabled =.F.
            .label3.Visible =.F.
            glHod=.F.
        Else
            Release gcHodName,gcHodDepName
            Public gcHodName,gcHodDepName
            gcHodName =Alltrim(username)
            gcHodDepName=Alltrim(dep_name)
            glHod=.T.
            gnUserDepID=dep_id
            Thisform.nav_butt1.command6.Enabled =.T.
        Endif

    *exec tsp_GetAutorizers 24,'jkamau'

        sn=SQLExec(gnConnHandle,"exec tsp_GetAutorizers_one ?gnCompID,?gcStaffNumber","proView")
        If !(sn>0 And Reccount()>0)
            =sysmsg('User not an authorizing officer')
            glAutorizer=.F.
        Else
            glAutorizer=.T.
            .label15.Caption =Alltrim(dep_name)
            .text8.Value  =Alltrim(firstname)+' '+Alltrim(lastname)
        Endif

        If .F.
            sn=SQLExec(gnConnHandle,"exec tsp_GetExecutivePro ?gnCompID,?gcUserID","proView")
            If !(sn>0 And Reccount()>0)
                glPro=.F.
            Else
                glPro=.T.
                .text8.Value =Alltrim(username)
            Endif
        Endif
             */
        }
}
