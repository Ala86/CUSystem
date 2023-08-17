using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TclassLibrary;

namespace WinTcare
{
    public partial class doctors : Form
    {
        public doctors()
        {
            InitializeComponent();
        }

        private void doctors_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Doctors Setup >>";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            /*
             With Thisform
	This.Enabled = .F.
	If Thisform.optiongroup1.Value =2		&&external doctor
		.getaccount							&&get account codes
		.updateusertable 					&&insert doctor into the user table with default password
	Endif
	gcDoctorCode='8'+Padl(Alltrim(Str(gnDoctorID)),5,'0')		&&Internal Doctors are staff, so their codes start from 800,000 to 950,000
	ldr_fname=.text2.Value
	ldr_mname=.text3.Value
	ldr_lname=.text4.Value
	lcompid=gnCompID
	linternal=Iif(.optiongroup1.Value=1,1,0)
	lcphoto=.image1.Picture

	With .pageframe1.page1
		lgender=Iif(.optiongroup1.Value=1,0,1)
		lcou_id=.combo1.Value
		lcAddress=.edit1.Value
		lpin=.text1.Value
		ldob=.text5.Value
		lidtype=.combo2.Value
		lidno=.text7.Value
		ltel=.text2.Value
		lofftel=.text3.Value
		lhomtel=.text4.Value
		lemail=.text6.Value
	Endwith

	With .pageframe1.page2
		ldr_qualif=.combo1.Value
		ldr_special=.combo2.Value
		lpla_doc=.text1.Value
		lyear_doc=.text2.Value
		lint_place=.text3.Value
		lint_stdate=.text4.Value
		lint_eddate=.text7.Value
		lnispecial=.check1.Value
	Endwith
	With .pageframe1.page3
		lexp1_place=.text1.Value
		lexp1_stdate=.text4.Value
		lexp1_eddate=.text7.Value

		lexp2_place=Alltrim(.text2.Value)
		lexp2_stdate=.text5.Value
		lexp2_eddate=.text8.Value

		lexp3_place=.text3.Value
		lexp3_stdate=.text6.Value
		lexp3_eddate=.text9.Value
	Endwith

	With .pageframe1.page4
		lreg_date=.text4.Value
		llicenceno=.text1.Value
		lreg_stat=.combo1.Value
		lreg_autor=.combo2.Value
		lgp_entry=.text5.Value		&&.combo4.Value
		lissue_date=.text2.Value
		lexp_date=.text3.Value
		lrating=.combo3.Value
	Endwith


*******************update the dashboard******************************
	gnDnYear=Year(gdSysDate)
	gnDmonth=Month(gdSysDate)
*********************Dashboard update*********************


	If gcInputStat='N'		&&new doctor
		sn=SQLExec(gnConnHandle,"insert into doctors (dr_id,doccode,dr_fname,dr_mname,dr_lname,dr_special,dr_qualif,tel,email,compid,cphoto,gender,cou_id,cAddress, "+;
			"pin,dob,idtype,idno,offtel,homtel,pla_doc,year_doc,int_place,int_stdate,int_eddate,exp1_place,exp1_stdate,exp1_eddate,exp2_place,exp2_stdate,exp2_eddate,"+;
			"exp3_place,exp3_stdate,exp3_eddate,reg_date,licenceno,reg_stat,reg_autor,gp_entry,issue_date,exp_date,rating,isinternal,oprcode,lispecial) "+;
			"values "+;
			"(?gnDoctorID,?gcDoctorCode,?ldr_fname,?ldr_mname,?ldr_lname,?ldr_special,?ldr_qualif,?ltel,?lemail,?lcompid,?lcphoto,?lgender,?lcou_id,?lcAddress,"+;
			"?lpin,?ldob,?lidtype,?lidno,?lofftel,?lhomtel,?lpla_doc,?lyear_doc,?lint_place,?lint_stdate,?lint_eddate,?lexp1_place,?lexp1_stdate,?lexp1_eddate,?lexp2_place,?lexp2_stdate,?lexp2_eddate,"+;
			"?lexp3_place,?lexp3_stdate,?lexp3_eddate,?lreg_date,?llicenceno,?lreg_stat,?lreg_autor,?lgp_entry,?lissue_date,?lexp_date,?lrating,?linternal,?gcDocOprCode,?lnispecial)","docupd")
		If !(sn>0)
			=sysmsg('Could not add new Doctor record')
		Else
************generate an account for doctor
			Thisform.crea_acc
			gcDuptype='D1'
			gnDdoctors=1
			Thisform.updatedashboard
		Endif
	Else
		sn=SQLExec(gnConnHandle,"update doctors  set doccode=?gcDoctorCode,dr_fname=?ldr_fname,dr_mname=?ldr_mname,dr_lname=?ldr_lname,dr_special=?ldr_special,"+;
			"dr_qualif=?ldr_qualif,tel=?ltel,email=?lemail,cphoto=?lcphoto,gender=?lgender,cou_id=?lcou_id,cAddress=?lcAddress, "+;
			"pin=?lpin,dob=?ldob,idtype=?lidtype,idno=?lidno,offtel=?lofftel,homtel=?lhomtel,pla_doc=?lpla_doc,year_doc=?lyear_doc,int_place=?lint_place,"+;
			"int_stdate=?lint_stdate,int_eddate=?lint_eddate,exp1_place=?lexp1_place,exp1_stdate=?lexp1_stdate,exp1_eddate=?lexp1_eddate,"+;
			"exp2_place=?lexp2_place,exp2_stdate=?lexp2_stdate,exp2_eddate=?lexp2_eddate,"+;
			"exp3_place=?lexp3_place,exp3_stdate=?lexp3_stdate,exp3_eddate=?lexp3_eddate,reg_date=?lreg_date,licenceno=?llicenceno,"+;
			"reg_stat=?lreg_stat,reg_autor=?lreg_autor,gp_entry=?lgp_entry,issue_date=?lissue_date,exp_date=?lexp_date,"+;
			"rating=?lrating,isinternal=?linternal,oprcode=?gcDocOprCode,lispecial=?lnispecial where dr_id =?gnDoctorID","docupd")
		If !(sn>0)
			=sysmsg('Could not update Doctor record')
		Else
			bn=SQLExec(gnConnHandle,"update DoctorCharges set LocalAdultCons=?lnLocalAdultCons,LocalChildCons=?lnLocalChildCons,LocalAdultTest=?lnLocalAdultTest,"+;
				"LocalChildTest=?lnLocalChildTest,LocalAdultExam=?lnLocalAdultExam,LocalChildExam=?lnLocalChildExam,LocalAdultProc=?lnLocalAdultProc,"+;
				"LocalChildProc=?lnLocalChildProc,ForenAdultCons=?lnForenAdultCons,ForenChildCons=?lnForenChildCons,ForenAdultTest=?lnForenAdultTest,"+;
				"ForenChildTest=?lnForenChildTest,ForenAdultExam=?lnForenAdultExam,ForenChildExam=?lnForenChildExam,ForenAdultProc=?lnForenAdultProc,"+;
				"ForenChildProc=?lnForenChildProc,cashpay=?lnCashPay,coverpay=?lnCoverPay  where dr_id =?gnDoctorID","docchupd")
			If !(bn>0)
				=sysmsg("Could not update Doctor's Charges")
			Endif
************update dash board
*			gcDuptype='D1'
*			gnDdoctors=1
*			Thisform.updatedashboard
		Endif
	Endif

	Store '' To .text2.Value,.text3.Value,.text4.Value,.image1.Picture

	With .pageframe1.page1
		Store 0 To .combo1.Value,.combo2.Value
		.optiongroup1.Value=1
		Store '' To .edit1.Value,.text1.Value,.text7.Value,.text2.Value,.text3.Value,.text4.Value,.text6.Value
		.text5.Value={}
	Endwith

	With .pageframe1.page2
		Store 0 To .combo1.Value,.combo2.Value,.text2.Value
		Store '' To .text1.Value,.text3.Value
		Store {} To .text4.Value,.text7.Value
	Endwith

	With .pageframe1.page3
		Store '' To.text1.Value,.text2.Value,.text3.Value
		Store {} To .text4.Value,.text7.Value,.text5.Value,.text8.Value,.text6.Value,.text9.Value
	Endwith

	With .pageframe1.page4
		Store {} To .text4.Value,.text2.Value,.text3.Value
		Store '' To .text1.Value,.text2.Value
		Store 0 To .combo1.Value,.combo2.Value,.combo3.Value
	Endwith

	Store .F. To .text2.Enabled,.text3.Enabled,.text4.Enabled
	=SQLExec(gnConnHandle,"exec sp_UsersNotDoctors ?gnCompid","userlistV")
	Thisform.definegrid
	Thisform.pageframe1.page1.SetFocus
	.Refresh
Endwith
             */
        }
    }
}
