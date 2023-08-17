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
    public partial class MedHistory : Form
    {
        string cs = globalvar.cos;
        string gcCustCode = "";
        int ncompid = globalvar.gnCompid;
        DataTable visview = new DataTable();
        DataTable clientview = new DataTable();
        DataTable consulview = new DataTable();
        DataTable labview = new DataTable();
        DataTable radview = new DataTable();
        DataTable optview = new DataTable();
        DataTable medview = new DataTable();
        DataTable diaview = new DataTable();
        DataTable consmaview = new DataTable();


        int gnVisno = 0;
        public MedHistory(string tcCode)
        {
            InitializeComponent();
            gcCustCode = tcCode;
       //     MessageBox.Show("The client id is " + gcCustCode);
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void MedHistory_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Client Medical History >>";
            if(gcCustCode.Trim()!=string.Empty)
            {
                textBox6.Text = gcCustCode;
                getclient(gcCustCode);
                getvisits(gcCustCode);
                //clientallergy
                //getipid
            }
            else
            {
                textBox10.Text = "";
                textBox13.Text = "";
                textBox4.Text = "";
                textBox6.Text = "";
                visview.Clear();
                textBox6.Enabled = true;
                textBox6.Focus();
            }
        }

        private void getclient(string tcDcode)
        {
        //    MessageBox.Show("Client code in getclient is " + tcDcode);
            string dsql = "exec tsp_Patient_Code  " + ncompid + "," + "'" + tcDcode + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                clientview.Clear();
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(clientview);
                if (clientview !=null)
                {
                    textBox10.Text = clientview.Rows[0]["fname"].ToString();
                    textBox13.Text = clientview.Rows[0]["lname"].ToString();
                    //gnVisno = getVisitNumber.visitno(cs, ncompid.ToString(), tcDcode);
                }else { MessageBox.Show("We have not found the client "+tcDcode); }
            }

        }

        private void getvisits(string tcCode)
        {
            string dsql = "Exec tsp_Reports_MedicalBio_Visits  " + ncompid + "," + "'" + tcCode + "'";
            visview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(visview);
                if (visview.Rows.Count > 0)
                {
                    visGrid.AutoGenerateColumns = false;
                    visGrid.DataSource = visview.DefaultView;
                    visGrid.Columns[0].DataPropertyName = "visdate";      // "age";
                    visGrid.Columns[1].DataPropertyName = "visno";
                    visGrid.Columns[2].DataPropertyName = "vistime";

                    ndConnHandle.Close();
                    for (int i = 0; i < 3; i++)
                    {
                        visview.Rows.Add();
                    }
                }
                else { MessageBox.Show("we have not found any visits for this client"); }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
         //       AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
           //     AllClear2Go();
            }
        }
        private void visGrid_Click(object sender, EventArgs e)
        {
            string dcCode = textBox6.Text.ToString();
            DataRow drow = visview.Rows[visGrid.CurrentCell.RowIndex];// clientview.Rows[clientgrid.CurrentCell.RowIndex];
            gnVisno = Convert.ToInt16(drow["visno"]);
            dconsultant(dcCode);
            labtests(dcCode);
            prescriptions(dcCode);
            exams(dcCode);
        }



        private void dconsultant(string dCode)
        {
            string dsql = "Exec tsp_Reports_MedicalBio_Cons "+ncompid+","+"'"+dCode+"'"+","+gnVisno;
            consulview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(consulview);
                if (consulview.Rows.Count > 0)
                {
                    textBox4.Text = consulview.Rows[0]["doc_name"].ToString();
                } 
            }
        }
        private void diagnosis()
        {
            /*
             With This
	sn=SQLExec(gnConnHandle,"Exec tsp_Reports_MedicalBio_Diagnosis ?gnCompid,?gcTempCustCode,?gnVisNo","diagView")
	If sn>0 And Reccount()>0
		Set Filter To !finaldiag &&AND visno=gnVisNo
		Locate
		.edit4.Value= Alltrim(icd_name)
		Set Filter To finaldiag &&AND visno=gnVisNo
		Locate
		.edit5.Value=Alltrim(icd_name)
	Endif
	.Refresh
Endwith
             */
        }

        private void labtests(string tcCode)
        {
            string dsql1 = "Exec tsp_Reports_MedicalBio_LabTest_New  " + ncompid + "," + "'" + tcCode + "'" + "," + gnVisno;
            labview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql1, ndConnHandle);
                da.Fill(labview);
                if (labview.Rows.Count > 0)
                {
                    labGrid.AutoGenerateColumns = false;
                    labGrid.DataSource = labview.DefaultView;
                    labGrid.Columns[0].DataPropertyName = "test_name";
                    labGrid.Columns[1].DataPropertyName = "par_name";
                    labGrid.Columns[2].DataPropertyName = "dvalue";
                    labGrid.Columns[3].DataPropertyName = "dunit";
                    labGrid.Columns[4].DataPropertyName = "dlow";
                    labGrid.Columns[5].DataPropertyName = "dhigh";
                }
            }
        }

        private void exams(string tcCode)
        {
            /*
             sn=SQLExec(gnConnHandle,"Exec tsp_Reports_MedicalBio_radTest ?gnCompid,?gcTempCustCode,?gnVisno","radView1")
            If sn>0 And Reccount()>0
                Locate
                Do While !Eof()
                    ldDate=test_date
                    lcItemName=item_name
                    lcResult=Alltrim(testresult)
                    lnQty=1					&&quantity
                    lnUnitPrice=0.00		&&unit_price
                    lnTotalCost=0.00		&&lnQty*lnUnitPrice
                    lcDosage=''				&&Alltrim(dosage)
                    lnitem=itemno
                    lcConsult=Alltrim(testconclude)
                    Insert Into radview (visdate,item_name, quantity, unit_price, total_cost,testresult,testconclude,tltcare,itemno) ;
                        VALUES (ldDate,lcItemName,lnQty,lnUnitPrice,lnTotalCost,lcResult,lcConsult,.T.,lnitem)
                    Select radView1
                    Skip
                Enddo
            Endif
            */
            string dsql1 = "Exec tsp_Reports_MedicalBio_radTest  " + ncompid + "," + "'" + tcCode + "'" + "," + gnVisno;
            radview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql1, ndConnHandle);
                da.Fill(radview);
                if (radview.Rows.Count > 0)
                {
                    radGrid.AutoGenerateColumns = false;
                    radGrid.DataSource = radview.DefaultView;
                    radGrid.Columns[0].DataPropertyName = "test_date";
                    radGrid.Columns[1].DataPropertyName = "item_name".ToString().Trim();
                }
            }
        }

        private void prescriptions(string tcCode)
        {
            string dsql1 = "Exec tsp_Reports_MedicalBio_Drugs " + ncompid + "," + "'" + tcCode + "'" + "," + gnVisno;
            medview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql1, ndConnHandle);
                da.Fill(medview);
                if (medview.Rows.Count > 0)
                {
                    medGrid.AutoGenerateColumns = false;
                    medGrid.DataSource = medview.DefaultView;
                    medGrid.Columns[0].DataPropertyName = "disp_date";
                    medGrid.Columns[1].DataPropertyName = "prod_name".ToString().Trim();
                    medGrid.Columns[2].DataPropertyName = "quantity";
                    medGrid.Columns[3].DataPropertyName = "dosage";
                }
            }
        }

        private void procs()
        {
            /*
             sn=SQLExec(gnConnHandle,"Exec tsp_Reports_MedicalBio_optTest ?gnCompid,?gcTempCustCode,?gnVisNo","proView")
            If sn>0 And Reccount()>0
                With This.pageframe1.pAGE3.proGrid  
                    .RecordSource = 'proView'
                    .column1.ControlSource = 'ttod(test_date)'
                    .column2.ControlSource = 'item_name'
                    .column3.ControlSource = 'testresult'
                Endwith
            Endif
           */
        }

        private void consumables()
        {
            /*
             sn=SQLExec(gnConnHandle,"Exec tsp_Reports_MedicalBio_Consumables ?gnCompid,?gcTempCustCode,?gnVisNo ","consView1")
            If sn>0 And Reccount()>0
                Locate
                Do While !Eof()
                    ldDate=dpostdate		&&disp_date		&&visdate
                    lcItemName=prod_name
                    lnQty=quantity
                    lnUnitPrice=ABS(unit_price)
                    lnTotalCost=lnQty*lnUnitPrice
            *		lcDosage=Alltrim(dosage)
                    Insert Into consView (visdate,item_name, quantity, unit_price, total_cost) ;
                        VALUES (ldDate,lcItemName,lnQty,lnUnitPrice,lnTotalCost)
                    Select consView1
                    Skip
                Enddo
            ENDIF
            */
        }



        private void clientallergy()
        {
            /*
             sm=SQLExec(gnConnHandle,"exec tsp_Reports_MedicalBio_Allergy ?gnCompID,?gcTempCustCode","hallergyView")
If sm>0 And Reccount()>0
	With Thisform.pageframe1.paGE6.histAllergyGrid 
		.RecordSource = 'hallergyView'
		.column1.ControlSource = 'alltrim(all_name)'
		.column2.ControlSource = 'dmy(TTOD(visdate))'
		.Refresh
	Endwith
endif
             */
        }
        private void getipid()
        {
            /*
             With Thisform
	tm=SQLExec(gnConnHandle,"select ipid  from admission where ccustcode=?gcTempCustCode","admView")
	If tm>0 And Reccount()>0
		.text6.Value=ipid
	Endif
	.Refresh
Endwith


             */
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            /*
             With Thisform
	Store '' To .text1.Value,.text4.value,.text5.value,thisform.pageframe1.paGE1.labGrid.RecordSource,thisform.pageframe1.pAGE2.exagrid.RecordSource,;
	 .visitgrid.RecordSource,thisform.pageframe1.page4.drugGrid.RecordSource,.edit1.value,.edit2.value
	 .text1.enabled =.t.
	.text1.SetFocus
	.Refresh
Endwith
             */
            textBox10.Text = "";
            textBox13.Text = "";
            textBox4.Text = "";
            textBox6.Text = "";
            visview.Clear();
            textBox6.Enabled = true;
            textBox6.Focus();
        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
           string cCode = textBox6.Text.ToString();
//            MessageBox.Show("ncompid,ccode " +  ncompid + "," + cCode);
  //        int   gnVisno1 = getVisitNumber.visitno(cs, ncompid.ToString(), cCode);
    //        MessageBox.Show("We are going into getclient with ccustcode, visit number  "+cCode+"," +gnVisno1);
            getclient(cCode);
             getvisits(cCode);

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void radGrid_Click(object sender, EventArgs e)
        {
            if (radview.Rows.Count > 0)
            {
                DataRow drow = radview.Rows[radGrid.CurrentCell.RowIndex];
              examResult.Text = drow["testresult"].ToString();
              examCons.Text = drow["testconclude"].ToString();
            }

        }
    }
}
