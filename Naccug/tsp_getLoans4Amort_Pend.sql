USE [NA_SOF001]
GO
/****** Object:  StoredProcedure [dbo].[tsp_getLoans4Amort_Pend]    Script Date: 14-Sep-20 12:58:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ONtsp_getLoans4Amort_Pend
GO

ALTER  PROCEDURE  [dbo].[tsp_getLoans4Amort_Pend]   
@compid    int
AS 
BEGIN 	SET NOCOUNT ON; 
select LOAN_DET.lamort,  cusreg.ccustcode,ltrim(rtrim(ccustname))+ltrim(rtrim(ccustfname))+' '+ltrim(rtrim(ccustlname)) as membername,loan_det.loan_appl_date,loanstart_date,maturity_date,loan_id,
principal_amt as loanamt,TOTAL_INTEREST as totinterest, lduration_num as loandur,repayment_amt as payamt, PRINCIPAL_AMT+TOTAL_INTEREST as totpay,
nofpayments,loan_interest,cusreg.cust_type,loan_appl_date, lapproved,lissued,lamort,
loantype = (select prd_name from prodtype where loan_det.LOAN_TYPE_ID = prodtype.prd_id ) ,
loantype1 = (select prd_id from prodtype where loan_det.LOAN_TYPE_ID = prodtype.prd_id )   
from loan_det,cusreg 
where cusreg.compid=@compid and  
cusreg.ccustcode = loan_det.ccustcode and 
lreject = 0 and 
lamort = 0 
order by loan_id
END







