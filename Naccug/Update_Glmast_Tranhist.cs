using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTcare
{
    class Update_Glmast_Tranhist
    {
        private void updateGlmast(string tcAcctNumb, decimal tnTranAmnt)
        {
            /*
             * Procedure UpdateGlmast
            Parameters tcAcctNumb, tnTranamt
            lnUpdAmtPos=Iif(tnTranamt>0.00, tnTranamt,0.00)
            lnUpdAmtNeg=Iif(tnTranamt<0.00, tnTranamt,0.00)
            lnUpdAmt=Iif(lnUpdAmtPos>0.00, lnUpdAmtPos, lnUpdAmtNeg)

            gn=SQLExec(gnConnHandle,"update glmast set nbookbal=?tnTranamt+nbookbal,ncleabal=?tnTranamt+ncleabal where cacctnumb=?tcAcctNumb","myresult")
            If !(gn>0)
                =Messagebox("Book balance could not be updated",0,"Update failure")
            Else
                If !Inlist(Left(tcAcctNumb,3),'104','999')								&&This process is for internal accounts only

                    en=SQLExec(gnConnHandle,"select 1 from daybal where  cacctnumb=?tcAcctNumb and convert(date,baldate)=convert(date,getdate()) ","mydaybal")      &&Check to see if accounts exists for today
                    If !(en>0 And Reccount()>0)  										&&There is no record for this patient for today, so we will insert a new record for today.Remember these are internal accounts and not patient accounts
                       tn = SQLExec(gnConnHandle, 'select baldate,nclosbal from daybal where cacctnumb=?tcAcctNumb order by baldate', 'myrunview')

                        If tn>0 And Reccount()>0
                            Go Bott

                            lnClosBal=nclosbal
                            fn = SQLExec(gnConnHandle, "exec tsp_InsertDailyBalance ?tcAcctNumb,?lnClosBal", "mydaybal") && insert a new record with running balance of last transaction
                             Else

                            fn=SQLExec(gnConnHandle,"exec tsp_InsertDailyBalance ?tcAcctNumb,0","mydaybal") &&insert a new record for the first time in the table, nopenbal = 0.00

                        Endif
                    Endif


                    fn=SQLExec(gnConnHandle,"exec tsp_UpdateDailyBalance ?tcAcctNumb,?lnUpdAmtPos,?lnUpdAmtNeg,?lnUpdAmt","mydaybal")

                    If !(fn>0)
                        =sysmsg("Daily Balance table could not be updated")

                    Endif
                Endif
            Endif
*/
        }

        private void updateTransactionHistory(string tcAcctNumb,decimal tnTranAmnt)
        {
            /*                     *          
                Procedure UpdateTransactionHistory
                Parameter tcAcctNumb,tnTranamt,tcDesc,tcVoucher,tcChqno,gcUserID,tnNewBal,tcTranCode,lnServID,lnPatID,lnPaid,tcContra,lnWaiveAmt,qty,unitprice,tcReceipt,lnCashpay,visno,isproduct,srvid,tcSrv_code,tcProd_code
                lcCustcode = Right(tcAcctNumb, 6)

                * If     glGloNoCon Or glGloNoLab Or glGloNoRad Or glGloNoPre Or glGloNoOpt
                 * llFree = 1
                 *   = sysmsg('there is a freebee here')
                 * Else
                 * llFree = 0
                 * Endif
                 * ********code below deprecated because of multiple records instead of one in stacktable, causing transactions to share cstack values and creating duplicates at cashier and pharmacy
                  * ln = SQLExec(gnConnHandle, "select * from stacktable", "genresult")
                  * If ln>0 And Reccount()>0
                *	lcStack=Padl(Alltrim(Str(stackcount)),11,'0')
                *Endif

                ln = SQLExec(gnConnHandle, "select stackcount from patient_code", "genresult")
                If ln>0 And Reccount()>0
                    lcStack=Padl(Alltrim(Str(stackcount)),12,'0')
                Endif

                tn = SQLExec(gnConnHandle, "Insert Into tranhist (cacctnumb,ntranamnt,ctrandesc,dpostdate,dtrandate,dvaluedate,cvoucherno," +;
                    "receiptno,cchqno,cuserid,nnewbal,cstack,ctrancode,compid,sverified,serv_id,pat_id,lpaid,ccontra,nwaiveamt,ctrantime,quantity,unit_price,lcashpay,visno,ccustcode,isproduct,srv_id,srv_code,prod_code,lfreebee) "+;
                    "VALUES "+;
                    "(?tcAcctNumb,?tntranamt,?tcDesc,CONVERT(date,GETDATE()),CONVERT (date, GETDATE()),CONVERT (Date, GETDATE()),?tcVoucher,?tcReceipt,"+;
                    "?tcChqno,?gcUserID,?tnNewBal,?lcStack,?tcTranCode,?gnCompid,1,?lnServID,?lnPatID,?lnPaid,?tcContra,?lnWaiveAmt,CONVERT (Time, GETDATE()),?qty,?unitprice,?lnCashpay,?visno,?lcCustcode,?isproduct,?srvid,?tcSrv_code,?tcProd_Code,?glFreeBee)","updtv")
                If !(tn>0)
                    =sysmsg('Could not update Transaction History File, inform IT DEPT')
                Else
                    tn = SQLExec(gnConnHandle, "Insert Into todayhist (cacctnumb,ntranamnt,ctrandesc,dpostdate,dtrandate,dvaluedate,cvoucherno," +;
                        "receiptno,cchqno,cuserid,nnewbal,cstack,ctrancode,compid,sverified,serv_id,pat_id,lpaid,ccontra,nwaiveamt,ctrantime,quantity,unit_price,lcashpay,visno,ccustcode,isproduct,srv_id,srv_code,prod_code,lfreebee) "+;
                        "VALUES "+;
                        "(?tcAcctNumb,?tntranamt,?tcDesc,CONVERT(date,GETDATE()),CONVERT (date, GETDATE()),CONVERT (Date, GETDATE()),?tcVoucher,?tcReceipt,"+;
                        "?tcChqno,?gcUserID,?tnNewBal,?lcStack,?tcTranCode,?gnCompid,1,?lnServID,?lnPatID,?lnPaid,?tcContra,?lnWaiveAmt,CONVERT (Time, GETDATE()),?qty,?unitprice,?lnCashpay,?visno,?lcCustcode,?isproduct,?srvid,?tcSrv_code,?tcProd_Code,?glFreeBee)","updtv")
                Endif

                fn = SQLExec(gnConnHandle, "update patient_code set stackcount=stackcount+1", "")
                If !(fn>0)
                    =sysmsg('Critical Error : Could not update stack count, inform IT DEPT immediately')
                Endif

                Function CheckLastBalance
                Parameters tcAcctNumb
                If Empty(tcAcctNumb)

                    lnLastBal=0.00
                Else
                    sn = SQLExec(gnConnHandle, "select nbookbal from glmast where cacctnumb=?tcAcctNumb", "lastbalance")

                    If sn>0 And Reccount()>0
                        lnLastBal=nbookbal
                    Else
                        = sysmsg('Critical error, Account ' + tcAcctNumb + ' is not found or variable empty.')

                        lnLastBal=0.00
                    Endif
                Endif
                Return lnLastBal*/
        }
    }

}
