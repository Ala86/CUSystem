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
    public partial class bouquets : Form
    {
        public bouquets()
        {
            InitializeComponent();
        }

        private void bouquets_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Bouquets Setup >>";
        }

        private void getinsurance()
        {
            /*
             With Thisform
	sn=SQLExec(gnConnHandle,"exec sp_Insurance ?gnCompid","InsView")
	If !(sn>0 And Reccount()>0)
		=sysmsg("No insurance companies defined, inform IT Dept")
	Endif
	.combo3.Value=0
	.combo2.value=0
*	.label7.visible=.f.
	Do Case
	Case This.Value =1			&&Insurance
		Select insView
		Set Filter To ins_type = 1
		Locate
	Case This.Value =2			&&Corporate
		Select insView
		Set Filter To ins_type = 2
		Locate
	Case This.Value =3			&&nhif
		Select insView
		Set Filter To ins_type = 3
		Locate
	Endcase
	.text5.Visible=Iif(Inlist(This.Value,1,2),.F.,.T.)
	.text5.Value=Iif(Inlist(This.Value,1,2),'','NHIF')
	Locate
	.Refresh
Endwith
             */
        }
        private void getbouquets()
        {
            /*
             With Thisform
	If Used('tempService')
		Select tempService
		Zap
	Endif
	sr1=SQLExec(gnConnHandle,"exec tsp_BouquetsInsServiceCovered ?gnCompID, ?gnInsID","feesview")
	If sr1>0 And Reccount()>0
		lns=Reccount()
		Locate
		Do While !Eof()
			lprodid=service_id
			lproname=servce_name
			lcash=cashpay
			lcover=coverpay
			Insert Into tempService (pre_auth,restricted,selitem,serv_name,serv_id,bcashpay,bcoverpay) Values (.f.,.F.,.F.,lproname,lprodid,lcash,lcover)
			Select feesView
			Skip
		Enddo
		With Thisform.labgrid
			Select tempService
			ln=Reccount()
			Locate
			.RecordSource = 'tempService'
			.column1.ControlSource = 'selitem'
			.column2.ControlSource = 'serv_name'
			.column3.ControlSource = 'bcashpay'
			.column4.ControlSource = 'bcoverpay'
			.column5.ControlSource = 'restricted'
		Endwith
	Else
		=sysmsg('No Services found, Inform IT Dept')
	Endif
	If Used('tempDrug')
		Select tempDrug
		Zap
	Endif
	sn=SQLExec(gnConnHandle,"exec tsp_BouquetsInsProductCovered ?gnCompid,?gnInsID","stockView")
	If sn>0 And Reccount()>0
		Locate
		Do While !Eof()
			lprodid=product_id
			lproname=prod_name
			lcash = cashpay
			lcover = coverpay
			Insert Into tempDrug (pre_auth,restricted,selitem,prod_name,product_id,bcashpay,bcoverpay) Values (.f.,.F.,.F.,lproname,lprodid,lcash,lcover)
			Select stockView
			Skip
		Enddo
		With Thisform.prodGrid
			Select tempDrug
			Locate
			.RecordSource = 'tempDrug'
			.column1.ControlSource = 'selitem'
			.column2.ControlSource = 'prod_name'
			.column3.ControlSource = 'bcashpay'
			.column4.ControlSource = 'bcoverpay'
			.column5.ControlSource = 'restricted'
		Endwith
	Else
		=sysmsg('No Products found, Inform IT Dept')
	Endif
Endwith

             */
        }

        private void getservices()
        {
            /*
             If Used('tempService')
	Select tempService
	Zap
Endif

fr1=SQLExec(gnConnHandle,"exec tsp_getServicecentres  ?gnCompID","serviceView")
If fr1>0 And Reccount()>0
	Select serviceView
	Locate
	Do While !Eof()
		gnSrvID=srv_id
		sr1=SQLExec(gnConnHandle,"exec tsp_GetTcareCoveredServices ?gnCompID, ?gnSrvID,?gnInsID","dSrvView")
		If sr1>0 And Reccount()>0
			lns=Reccount()
			Locate
			Do While !Eof()
				lprodid=Iif(!Isnull(srv_id),srv_id,0)
				lproname=Iif(!Isnull(servce_name),servce_name,'')
				lcSrvCode=Iif(!Isnull(srv_code),srv_code,'')
				lnSrvfee=Iif(!Isnull(coverpay),coverpay,0.00)
				lcSrvName=Iif(!Isnull(srv_name),srv_name,'')
				lrestricted=Iif(lrestrict,.T.,.F.)
				Insert Into tempService (srv_name,pre_auth,srv_code,restricted,selitem,serv_name,serv_id,bcoverpay) Values (lcSrvName,.F.,lcSrvCode,lrestricted,.F.,lproname,lprodid,lnSrvfee)
				Select dsrvView
				Skip
			Enddo
			glNoService=.F.
	*		Thisform.command2.Visible=.F.
		Else
			glNoService=.T.
	*		Thisform.command2.Visible=.T.
		Endif
		Select serviceView
		Skip
	Enddo
	Select tempService
	With Thisform.pageframe1.page1.servGrid
		Select tempService
		ln=Reccount()
		glServSelected = Iif(ln>0,.T.,.F.)
		Locate
		.RecordSource = 'tempService '
		.column1.ControlSource = 'selitem'
		.column2.ControlSource = 'serv_name'
		.column4.ControlSource = 'bcoverpay'
		.column5.ControlSource = 'restricted'
		.column3.ControlSource = 'srv_name'
		.SetAll("dynamicbackcolor","IIF(tempservice.restricted,RGB(255,0,0), RGB(255,255,255))", "Column")
	Endwith
ENDIF
*thisform.command2.Visible = glNoService 
Thisform.Refresh

             */
        }

        private void getdrusg ()
        {
            /*
             If Used('tempDrug')
	Select tempDrug
	Zap
Endif

sr1=SQLExec(gnConnHandle,"exec tsp_GetTcareCoveredDrugs ?gnCompID,?gnInsID","dDrgView")
If sr1>0 And Reccount()>0
	lns=Reccount()
	Locate
	Do While !Eof()
		lproname=Iif(!Isnull(prod_name),prod_name,'')
		lcProdCode=Iif(!Isnull(prod_code),prod_code,'')
		lnSrvfee=Iif(!Isnull(coverpay),coverpay,0.00)
		lrestricted=Iif(lrestrict,.T.,.F.)
		lprea=pre_auth
		Insert Into  tempDrug (pre_auth,prod_code,restricted,selitem,prod_name,bcoverpay) Values (lprea,lcProdCode,lrestricted,.F.,lproname,lnSrvfee)
		Select dDrgView
		Skip
	Enddo
	glNoDrugs=.F.
Else
	glNoDrugs=.T.
Endif

With Thisform.pageframe1.page2.drugrid
	Select tempDrug
	ln=Reccount()
	glDrugSelected = Iif(ln>0,.T.,.F.)
	Locate
	.RecordSource = 'tempDrug '
	.column1.ControlSource = 'pre_auth'
	.column2.ControlSource = 'prod_name'
	.column4.ControlSource = 'bcoverpay'
	.column5.ControlSource = 'restricted'
	.SetAll("dynamicbackcolor","IIF(tempDrug.restricted,RGB(255,0,0), RGB(255,255,255))", "Column")
Endwith
*Thisform.command2.Visible = glNoDrugs
Thisform.Refresh

             */
        }

        private void getconsumables()
        {
            /*
             If Used('tempconsumable')
	Select tempconsumable
	Zap
Endif

sr1=SQLExec(gnConnHandle,"exec tsp_GetTcareCoveredConsu ?gnCompID,?gnInsID","dDrgView")
If sr1>0 And Reccount()>0
	lns=Reccount()
	Locate
	Do While !Eof()
		lproname=Iif(!Isnull(prod_name),prod_name,'')
		lcProdCode=Iif(!Isnull(prod_code),prod_code,'')
		lnSrvfee=Iif(!Isnull(coverpay),coverpay,0.00)
		lrestricted=Iif(lrestrict,.T.,.F.)
		Insert Into  tempconsumable(pre_auth,prod_code,restricted,selitem,prod_name,bcoverpay) Values (.F.,lcProdCode,lrestricted,.F.,lproname,lnSrvfee)
		Select dDrgView
		Skip
	Enddo
	glNoCons=.F.
Else
	glNoCons=.T.
Endif

With Thisform.pageframe1.page3.consgrid 
	Select tempconsumable
	ln=Reccount()
	glConsSelected = Iif(ln>0,.T.,.F.)
	Locate
	.RecordSource = 'tempconsumable'
	.column1.ControlSource = 'selitem'
	.column2.ControlSource = 'prod_name'
	.column4.ControlSource = 'bcoverpay'
	.column5.ControlSource = 'restricted'
	.SetAll("dynamicbackcolor","IIF(tempconsumable.restricted,RGB(255,0,0), RGB(255,255,255))", "Column")
Endwith
*Thisform.command2.Visible = glNoCons
Thisform.Refresh

             */
        }

        private void getsupplies()
        {
            /*
             If Used('tempsupply ')
	Select tempsupply 
	Zap
Endif

sr1=SQLExec(gnConnHandle,"exec tsp_GetTcareCoveredSupply ?gnCompID,?gnInsID","dDrgView")
If sr1>0 And Reccount()>0
	lns=Reccount()
	Locate
	Do While !Eof()
		lproname=Iif(!Isnull(prod_name),prod_name,'')
		lcProdCode=Iif(!Isnull(prod_code),prod_code,'')
		lnSrvfee=Iif(!Isnull(coverpay),coverpay,0.00)
		lrestricted=Iif(lrestrict,.T.,.F.)
		Insert Into  tempsupply(pre_auth,prod_code,restricted,selitem,prod_name,bcoverpay) Values (.F.,lcProdCode,lrestricted,.F.,lproname,lnSrvfee)
		Select dDrgView
		Skip
	Enddo
	glNoSupp=.F.
Else
	glNoSupp=.T.
Endif

With Thisform.pageframe1.page4.suppgrid 
	Select tempsupply 
	ln=Reccount()
	glSuppSelected = Iif(ln>0,.T.,.F.)
	Locate
	.RecordSource = 'tempsupply '
	.column1.ControlSource = 'selitem'
	.column2.ControlSource = 'prod_name'
	.column4.ControlSource = 'bcoverpay'
	.column5.ControlSource = 'restricted'
	.SetAll("dynamicbackcolor","IIF(tempsupply .restricted,RGB(255,0,0), RGB(255,255,255))", "Column")
Endwith
*Thisform.command2.Visible = glNoSupp
Thisform.Refresh

             */
        }
        
        private void getrdata()
        {
            /*
             With Thisform
	If Used('tempService')
		Select tempService
		Zap
	Endif
	sr1=SQLExec(gnConnHandle,"exec tsp_GetTcareServices ?gnCompID","feesview")
	If sr1>0 And Reccount()>0
		lns=Reccount()
		Locate
		Do While !Eof()
			lprodid=Iif(!Isnull(service_id),service_id,0)
			lcSrvCode=IIF(!ISNULL(srv_code),srv_code,'')
			lproname=Iif(!Isnull(servce_name),servce_name,'')
			lcash=Iif(!Isnull(cashpay),cashpay,0.00)
			lcover=Iif(!Isnull(coverpay),coverpay,0.00)
			Insert Into tempService (srv_code,restricted,selitem,serv_name,serv_id,bcashpay,bcoverpay) Values (lcSrvCode,.F.,.F.,lproname,lprodid,lcash,lcover)
			Select feesView
			Skip
		Enddo
		With Thisform.pageframe1.page2.labgrid
			Select tempService
			ln=Reccount()
			Locate
			.RecordSource = 'tempService'
			.column1.ControlSource = 'selitem'
			.column2.ControlSource = 'serv_name'
			.column4.ControlSource = 'bcoverpay'
			.column5.ControlSource = 'restricted'
		Endwith
	Else
		=sysmsg('No Default Services found, Inform IT Dept')
	Endif
	If Used('tempDrug')
		Select tempDrug
		Zap
	Endif
	sn=SQLExec(gnConnHandle,"exec tsp_GetTcareProducts ?gnCompid","stockView")
	If sn>0 And Reccount()>0
		Locate
		Do While !Eof()
			lprodid=Iif(!Isnull(product_id),product_id,0)
			lcProdCode=prod_code
			lproname=Iif(!Isnull(prod_name),prod_name,'')
			lcash = Iif(!Isnull(cashpay),cashpay,0.00)
			lcover = Iif(!Isnull(coverpay),coverpay,0.00)
			Insert Into tempDrug (prod_code,restricted,selitem,prod_name,product_id,bcashpay,bcoverpay) Values (lcProdCode,.F.,.F.,lproname,lprodid,lcash,lcover)
			Select stockView
			Skip
		Enddo
		With Thisform.pageframe2.page13.prodGrid
			Select tempDrug
			Locate
			.RecordSource = 'tempDrug'
			.column1.ControlSource = 'selitem'
			.column2.ControlSource = 'prod_name'
			.column4.ControlSource = 'bcoverpay'
			.column5.ControlSource = 'restricted'
		Endwith
	Else
		=sysmsg('No Default Products found, Inform IT Dept')
	Endif
Endwith

             */
        }

        private void tempexclusions()
        {
            /*
             Parameters tntime,tnProServ,tcItemCode
If tntime=1
	If tnProServ=1		&&product
		sn=SQLExec(gnConnHandle,"INSERT INTO exclusions(ins_id,ins_type,compid, bou_id,prod_code,memb_inst,proserv) "+;
			"VALUES (?gnInsID,1,1,?gnBouID,?tcItemCode,0,1)","itemview")
		If !(sn>0)
			=sysmsg("Could not update exclusions, Inform IT Dept")
		Endif
	Else				&&service
		sn=SQLExec(gnConnHandle,"INSERT INTO exclusions(ins_id,ins_type,compid, bou_id,srv_code,memb_inst,proserv) "+;
			"VALUES (?gnInsID,1,1,?gnBouID,?tcItemCode,0,0)","itemview")
		If !(sn>0)
			=sysmsg("Could not update exclusions, Inform IT Dept")
		Endif
	Endif
Else
	If tnProServ=1		&&product
		fn=SQLExec(gnConnHandle,"delete from exclusions where ins_id=?gnInsID AND bou_id=?gnBouID and prod_code=?tcItemCode","prodel")
	Else				&&Service
		fn=SQLExec(gnConnHandle,"delete from exclusions where ins_id=?gnInsID AND bou_id=?gnBouID and srv_code=?tcItemCode","srvdel")
	Endif
Endif

             */
        }
        private void button2_Click(object sender, EventArgs e)
        {
            /*
             DO FORM addbouquet.scx
             */
        }


        private void button1_Click(object sender, EventArgs e)
        {
            /*
             If Used('tempService')
	Select tempService
	Zap
Endif

If Used('tempDrug')
	Select tempDrug 
	Zap
Endif

If Used('tempconsumable')
	Select tempconsumable
	Zap
Endif

If Used('tempsupply')
	Select tempsupply 
	Zap
Endif

*If glNoService
	fr1=SQLExec(gnConnHandle,"exec tsp_getServicecentres  ?gnCompID","serviceView")
	If fr1>0 And Reccount()>0
		Select serviceView
		Locate
		Do While !Eof()
			gnSrvID=srv_id
			sr1=SQLExec(gnConnHandle,"exec tsp_GetTcareCoveredServicesTemplate ?gnCompID, ?gnSrvID","dSrvView")
			If sr1>0 And Reccount()>0
				lns=Reccount()
				Locate
				Do While !Eof()
					lprodid=Iif(!Isnull(srv_id),srv_id,0)
					lproname=Iif(!Isnull(servce_name),servce_name,'')
					lcSrvCode=Iif(!Isnull(srv_code),srv_code,'')
					lnSrvfee=Iif(!Isnull(servce_fee),servce_fee,0.00)  &&=Iif(!Isnull(coverpay),coverpay,0.00)
					lcSrvName=Iif(!Isnull(srv_name),srv_name,'')
					lrestricted=Iif(lrestrict=1,.T.,.F.)
					Insert Into tempService (srv_name,pre_auth,srv_code,restricted,selitem,serv_name,serv_id,bcoverpay) Values (lcSrvName,.F.,lcSrvCode,lrestricted,.F.,lproname,lprodid,lnSrvfee)
					Select dsrvView
					Skip
				Enddo
			Endif
			Select serviceView
			Skip
		Enddo
	ENDIF
	Select tempService
	With Thisform.pageframe1.page1.servGrid
		Select tempService
		ln=Reccount()
		glServSelected = Iif(ln>0,.T.,.F.)
		Locate
		.RecordSource = 'tempService '
		.column1.ControlSource = 'selitem'
		.column2.ControlSource = 'serv_name'
		.column4.ControlSource = 'bcoverpay'
		.column5.ControlSource = 'restricted'
		.column3.ControlSource = 'srv_name'
		.SetAll("dynamicbackcolor","IIF(tempservice.restricted,RGB(255,0,0), RGB(255,255,255))", "Column")
	Endwith
*Endif

*If glNoDrugs
	sr1=SQLExec(gnConnHandle,"exec tsp_GetTcareCoveredDrugsTemplate ?gnCompID","dDrgView")
	If sr1>0 And Reccount()>0
		lns=Reccount()
		Locate
		Do While !Eof()
			lproname=Iif(!Isnull(prod_name),prod_name,'')
			lcProdCode=Iif(!Isnull(prod_code),prod_code,'')
			lnSrvfee=Iif(!Isnull(coverpay),coverpay,0.00)
			lrestricted=Iif(lrestrict=1,.T.,.F.)
			Insert Into  tempDrug (pre_auth,prod_code,restricted,selitem,prod_name,bcoverpay) Values (.F.,lcProdCode,lrestricted,.F.,lproname,lnSrvfee)
			Select dDrgView
			Skip
		Enddo
		glNoDrugs=.F.
	Else
		glNoDrugs=.T.
	Endif

	With Thisform.pageframe1.page2.drugrid
		Select tempDrug
		ln=Reccount()
		glDrugSelected = Iif(ln>0,.T.,.F.)
		Locate
		.RecordSource = 'tempDrug '
		.column1.ControlSource = 'selitem'
		.column2.ControlSource = 'prod_name'
		.column4.ControlSource = 'bcoverpay'
		.column5.ControlSource = 'restricted'
		.SetAll("dynamicbackcolor","IIF(tempDrug.restricted,RGB(255,0,0), RGB(255,255,255))", "Column")
	Endwith
*ENDIF

*If glNoCons
	sr1=SQLExec(gnConnHandle,"exec tsp_GetTcareCoveredConsuTemplate ?gnCompID","dConView")
	If sr1>0 And Reccount()>0
		lns=Reccount()
		Locate
		Do While !Eof()
			lproname=Iif(!Isnull(prod_name),prod_name,'')
			lcProdCode=Iif(!Isnull(prod_code),prod_code,'')
			lnSrvfee=Iif(!Isnull(coverpay),coverpay,0.00)
			lrestricted=Iif(lrestrict=1,.T.,.F.)
			Insert Into  tempconsumable(pre_auth,prod_code,restricted,selitem,prod_name,bcoverpay) Values (.F.,lcProdCode,lrestricted,.F.,lproname,lnSrvfee)
			Select dConView
			Skip
		Enddo
		glNoCons=.F.
	Else
		glNoCons=.T.
	Endif

	With Thisform.pageframe1.page3.consgrid 
		Select tempconsumable
		ln=Reccount()
		glConsSelected = Iif(ln>0,.T.,.F.)
		Locate
		.RecordSource = 'tempconsumable'
		.column1.ControlSource = 'selitem'
		.column2.ControlSource = 'prod_name'
		.column4.ControlSource = 'bcoverpay'
		.column5.ControlSource = 'restricted'
		.SetAll("dynamicbackcolor","IIF(tempconsumable.restricted,RGB(255,0,0), RGB(255,255,255))", "Column")
	Endwith
*Endif

*If glNoSupp
	sr1=SQLExec(gnConnHandle,"exec tsp_GetTcareCoveredSupplyTemplate ?gnCompID","dSupView")
	If sr1>0 And Reccount()>0
		lns=Reccount()
		Locate
		Do While !Eof()
			lproname=Iif(!Isnull(prod_name),prod_name,'')
			lcProdCode=Iif(!Isnull(prod_code),prod_code,'')
			lnSrvfee=Iif(!Isnull(coverpay),coverpay,0.00)
			lrestricted=Iif(lrestrict=1,.T.,.F.)
			Insert Into  tempsupply (pre_auth,prod_code,restricted,selitem,prod_name,bcoverpay) Values (.F.,lcProdCode,lrestricted,.F.,lproname,lnSrvfee)
			Select dSupView
			Skip
		Enddo
		glNoDrugs=.F.
	Else
		glNoDrugs=.T.
	Endif

	With Thisform.pageframe1.page4.suppgrid 
		Select tempsupply 
		ln=Reccount()
		glDrugSelected = Iif(ln>0,.T.,.F.)
		Locate
		.RecordSource = 'tempsupply'
		.column1.ControlSource = 'selitem'
		.column2.ControlSource = 'prod_name'
		.column4.ControlSource = 'bcoverpay'
		.column5.ControlSource = 'restricted'
		.SetAll("dynamicbackcolor","IIF(tempsupply .restricted,RGB(255,0,0), RGB(255,255,255))", "Column")
	Endwith
*Endif

Thisform.Refresh
             */
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            /*
             With Thisform
	This.Enabled=.F.
	gnInsType=.optiongroup3.Value
*	lcBou_name=.text3.Value
	lnCorePay=.text1.Value
	lnCorePer=.text4.Value
	lnConsPay=.text2.Value
	lnVisLim=.text8.Value
	lnVisRbate=.text9.Value
	If .optiongroup1.Value=1		&&Services based on institutions
		sn=SQLExec(gnConnHandle,"update insurance set corepay = ?lnCorePay,conspay=?lnConsPay,co_payPercent=?lnCorePer,vislim=?lnVisLim,visrbate=?lnVisRbate where insu_id=?gnInsID","bouin")
		If !(sn>0 )
			=sysmsg("Could not update insurance records , inform IT DEPT")
		Else
			.updatebouquetproducts()
			.updatebouquetservices()
		Endif
	Else							&&Services based on bouquets
		sn=SQLExec(gnConnHandle,"update insurance set corepay = ?lnCorePay,conspay=?lnConsPay,co_payPercent=?lnCorePer where insu_id=?gnInsID","bouin")
		If !(sn>0 )
			=sysmsg("Could not update insurance records , inform IT DEPT")
		Else
*			Wait Window 'ins,bou,lim,rbate '+Str(gnInsid)+Str(gnBouid)+Str(lnVisLim)+','+Str(lnVisRbate)
			gn=SQLExec(gnConnHandle,"update bouquets set corepay = ?lnCorePay,conspay=?lnConsPay,corePercent=?lnCorePer,vislim=?lnVisLim,visrbate=?lnVisRbate where ins_id=?gnInsID and bou_id=?gnBouID","bouin")
			If !(gn>0)
				=sysmsg('Bouquet Basic Items not updated')
			Else
				.updatebouquetproducts()
				.updatebouquetservices()
			Endif
		Endif
	Endif
	gcInputStatus='N'
	Store '' To .TEXT5.Value
	Store 0 To .combo2.Value,.combo3.Value
	.optiongroup3.Value=1
	.combo2.Visible=.T.
	.command2.Visible =.F.
	Thisform.definegrid()
	Select tempDrug
	Set Filter To
	Locate
	Store 0.00 To  .text1.Value,.text2.Value,.text8.value,.text9.value 
	Store .T. To  .nav_butt1.command1.Enabled,.nav_butt1.command3.Enabled,.nav_butt1.command4.Enabled,.nav_butt1.command6.Enabled
	.Refresh
Endwith

             */
        }

private void updatebouquetservices()
        {
            /*
             fn=SQLExec(gnConnHandle,"delete from bouquetsitems where ins_id = ?gnInsID and proserv=0","OldpView")
            If !(fn>0)
                =sysmsg('Something is wrong')
            Else
                Select tempService
                Set Filter To 
                Locate
                Thisform.text6.Value =Reccount()
                lncount=1
                Do While !Eof()
                    lnproid=serv_id
                    lRes=restricted
                    lnCover=bcoverpay
                    lcSrvCode=srv_code
                    lPreAuth=pre_auth
                    Thisform.text7.Value=lncount
                    sn=SQLExec(gnConnHandle,"INSERT INTO bouquetsitems (pre_auth,srv_code,ins_id,ins_type,compid,lrestrict,coverpay) "+;
                        "VALUES (?lPreAuth,?lcSrvCode,?gnInsID,?gnInsType,?gnCompid,?lRes,?lnCover)","itemview")
                    If !(sn>0)
                        =sysmsg("Could not update bouquets service items, Inform IT Dept")
                    Endif
                    Select tempService
                    Skip
                    lncount=lncount+1
                Enddo
            Endif
            Select tempService
            Zap

                         */
        }
        private void updatebouquetproducts()
        {
            /*
             fn=SQLExec(gnConnHandle,"delete from bouquetsitems where ins_id = ?gnInsID and proserv=1","OldpView")
If !(fn>0)
	=sysmsg('Something is wrong')
Else
	Select tempDrug
	Set Filter To
	Thisform.text6.Value =Reccount()
	lnCount=1
	Locate
	Do While !Eof()
		lnproid=product_id
		lRes=restricted
		lcProdCode=prod_code
		lnCover=bcoverpay
		lcname=prod_name
		Thisform.text7.Value=lnCount
		sn=SQLExec(gnConnHandle,"INSERT INTO bouquetsitems (prod_code,ins_id,ins_type,compid,proserv,lrestrict,coverpay) "+;
			"VALUES (?lcProdCode,?gnInsID,?gnInsType,?gnCompid,1,?lRes,?lnCover)","itemview")
		If !(sn>0)
			=sysmsg("Could not insert bouquets product items, Inform IT Dept")
		Endif
		Select tempDrug
		Skip
		lnCount=lnCount+1
	ENDDO
	
	Select tempDrug
	Set Filter To
	Thisform.text6.Value =Reccount()
	lnCount=1
	Locate
	Do While !Eof()
		lnproid=product_id
		lRes=restricted
		lcProdCode=prod_code
		lnCover=bcoverpay
		lcname=prod_name
		Thisform.text7.Value=lnCount
		sn=SQLExec(gnConnHandle,"INSERT INTO bouquetsitems (prod_code,ins_id,ins_type,compid,proserv,lrestrict,coverpay) "+;
			"VALUES (?lcProdCode,?gnInsID,?gnInsType,?gnCompid,1,?lRes,?lnCover)","itemview")
		If !(sn>0)
			=sysmsg("Could not insert bouquets product items, Inform IT Dept")
		Endif
		Select tempDrug
		Skip
		lnCount=lnCount+1
	ENDDO

Select tempconsumable 
	Set Filter To
	Thisform.text6.Value =Reccount()
	lnCount=1
	Locate
	Do While !Eof()
		lnproid=product_id
		lRes=restricted
		lcProdCode=prod_code
		lnCover=bcoverpay
		lcname=prod_name
		Thisform.text7.Value=lnCount
		sn=SQLExec(gnConnHandle,"INSERT INTO bouquetsitems (prod_code,ins_id,ins_type,compid,proserv,lrestrict,coverpay) "+;
			"VALUES (?lcProdCode,?gnInsID,?gnInsType,?gnCompid,1,?lRes,?lnCover)","itemview")
		If !(sn>0)
			=sysmsg("Could not insert bouquets product items, Inform IT Dept")
		Endif
		Select tempconsumable 
		Skip
		lnCount=lnCount+1
	ENDDO

	Select tempsupply 
	Set Filter To
	Thisform.text6.Value =Reccount()
	lnCount=1
	Locate
	Do While !Eof()
		lnproid=product_id
		lRes=restricted
		lcProdCode=prod_code
		lnCover=bcoverpay
		lcname=prod_name
		Thisform.text7.Value=lnCount
		sn=SQLExec(gnConnHandle,"INSERT INTO bouquetsitems (prod_code,ins_id,ins_type,compid,proserv,lrestrict,coverpay) "+;
			"VALUES (?lcProdCode,?gnInsID,?gnInsType,?gnCompid,1,?lRes,?lnCover)","itemview")
		If !(sn>0)
			=sysmsg("Could not insert bouquets product items, Inform IT Dept")
		Endif
		Select tempsupply 
		Skip
		lnCount=lnCount+1
	ENDDO
	Select tempDrug
	ZAP
	SELECT tempconsumable 
	ZAP
	SELECT tempsupply 
	ZAP
Endif

             */
             }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            /*
         thisform.getservices
		Thisform.getdrugs
		Thisform.getconsumables
		Thisform.getsupplies
             */
            getservices();
            getdrusg();
            getconsumables();
            getsupplies();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
             			fn=SQLExec(gnConnHandle,"exec tsp_GetInsBouquets ?gnCompid,?gnInsType,?gnInsID","BouquetView")
			If fn>0 And Reccount()>0
				.combo3.Value=0
				Thisform.combo3.RowSource = 'BouquetView.bou_name,bou_id'
			Endif

             */
        }

        private void button3_Click(object sender, EventArgs e)
        {
           Close();
        }
    }
}
