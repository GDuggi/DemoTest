ALTER TRIGGER [ConfirmMgr].[TG_TRADE_SUMRY_JN_AER_DIU]
ON [ConfirmMgr].[TRADE_SUMMARY]
AFTER INSERT, UPDATE, DELETE
AS

/******************************************************************************
*
* AUTHOR:		JAVIER MONTERO - 07/27/2015
* DB:			SQL SERVER 2008 OR HIGHER
* VERSION:		1.0
* DESCRIPTION:  TRIGGER FOR DELETE ACTIONS ON TABLE TRADE_RQMT
* DEPENCIES:    TABLE TRADE_RQMT_JN IS REQUIERED
*
*******************************************************************************/
DECLARE 
@tmp_username		varchar(100),
@tmp_hostname		varchar(100),
@errormsg			varchar(4000),
@rows				int
BEGIN
	SET NOCOUNT ON;
	/*Here, the trigger get the hostname and user name that execute the deleted action*/
	
	SELECT @tmp_hostname = HOST_NAME(), @tmp_username = SUSER_NAME();
			
			/*If the user name and host name are null, the trigger don't execute any action then rollback the delete process*/
			IF @tmp_username IS NULL AND @tmp_hostname IS NULL
			BEGIN
				SET @errormsg = 'INSERT ON TRADE_SUMMARY_JN WAS ROLLBACKED BECAUSE USER_NAME OR HOST_NAME IS NULL'
				RAISERROR(@errormsg, 0,29000)
				
				ROLLBACK TRAN
				RETURN
			END	
	
		/*UPDATE*/
		IF EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted)
			BEGIN
			/*IF the ROWS exists in the inserted and exists in the deleted table the process shoot the action 
			to INSERT the values in the table TRADE_SUMMARY_JN*/
				INSERT ConfirmMgr.TRADE_SUMMARY_JN
				(jn.JN_ORACLE_USER,
				 jn.JN_DATETIME,
				 jn.JN_OPERATION,
				 jn.JN_HOST_NAME,
				 jn.ID,
				 jn.TRADE_ID,
				 jn.OPEN_RQMTS_FLAG,
				 jn.CATEGORY,
				 jn.LAST_UPDATE_TIMESTAMP_GMT,
				 jn.FINAL_APPROVAL_FLAG,
				 jn.LAST_TRD_EDIT_TIMESTAMP_GMT,
				 jn.OPS_DET_ACT_FLAG,
				 jn.READY_FOR_FINAL_APPROVAL_FLAG,
				 jn.HAS_PROBLEM_FLAG,
				 jn.TRANSACTION_SEQ,
				 jn.FINAL_APPROVAL_TIMESTAMP_GMT,
				 jn.CMT,
				 jn.CPTY_TRADE_ID
				 )
				SELECT 
				@tmp_username,
				GETDATE(),
				'U',
				@tmp_hostname,
				d.ID,
				d.TRADE_ID,
				d.OPEN_RQMTS_FLAG,
				d.CATEGORY,
				d.LAST_UPDATE_TIMESTAMP_GMT,
				d.FINAL_APPROVAL_FLAG,
				d.LAST_TRD_EDIT_TIMESTAMP_GMT,
				d.OPS_DET_ACT_FLAG,
				d.READY_FOR_FINAL_APPROVAL_FLAG,
				d.HAS_PROBLEM_FLAG,
				d.TRANSACTION_SEQ,
				d.FINAL_APPROVAL_TIMESTAMP_GMT,
				d.CMT,
				d.CPTY_TRADE_ID
				FROM inserted d
				INNER JOIN TRADE_SUMMARY tr 
				ON d.ID = tr.ID
				RETURN
		    END
		
		/*INSERT*/
		IF EXISTS(SELECT 1 FROM inserted) AND NOT EXISTS(SELECT * FROM deleted)			
			BEGIN
			/*If the ROWS exists in the inserted and not exists in the deleted table the process shoot the action 
			to INSERT the values in the table TRADE_SUMMARY_JN*/
			  INSERT ConfirmMgr.TRADE_SUMMARY_JN(
			  jn.JN_ORACLE_USER,
			  jn.JN_DATETIME,
			  jn.JN_OPERATION,
			  jn.JN_HOST_NAME,
			  jn.ID,
			  jn.TRADE_ID,
			  jn.OPEN_RQMTS_FLAG,
			  jn.CATEGORY,
			  jn.LAST_UPDATE_TIMESTAMP_GMT,
			  jn.FINAL_APPROVAL_FLAG,
			  jn.LAST_TRD_EDIT_TIMESTAMP_GMT,
			  jn.OPS_DET_ACT_FLAG,
			  jn.READY_FOR_FINAL_APPROVAL_FLAG,
			  jn.HAS_PROBLEM_FLAG,
			  jn.TRANSACTION_SEQ,
			  jn.FINAL_APPROVAL_TIMESTAMP_GMT,
			  jn.CMT,
			  jn.CPTY_TRADE_ID
			  )
				SELECT 
				@tmp_username,
				GETDATE(),
				'I',
				@tmp_hostname,
				d.ID,
				d.TRADE_ID,
				d.OPEN_RQMTS_FLAG,
				d.CATEGORY,
				d.LAST_UPDATE_TIMESTAMP_GMT,
				d.FINAL_APPROVAL_FLAG,
				d.LAST_TRD_EDIT_TIMESTAMP_GMT,
				d.OPS_DET_ACT_FLAG,
				d.READY_FOR_FINAL_APPROVAL_FLAG,
				d.HAS_PROBLEM_FLAG,
				d.TRANSACTION_SEQ,
				d.FINAL_APPROVAL_TIMESTAMP_GMT,
				d.CMT,
				d.CPTY_TRADE_ID
				FROM inserted d
				RETURN
			END

		 /*DELETE*/
		 IF NOT EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted)
			/*If the ROWS exists in the deleted table the process shoot the action to INSERT the values in the table TRADE_SUMMARY_JN*/
			BEGIN
				INSERT ConfirmMgr.TRADE_SUMMARY_JN(
			 	 jn.JN_ORACLE_USER,
				 jn.JN_DATETIME,
				 jn.JN_OPERATION,
				 jn.JN_HOST_NAME,
				 jn.ID,
				 jn.TRADE_ID,
				 jn.OPEN_RQMTS_FLAG,
				 jn.CATEGORY,
				 jn.LAST_UPDATE_TIMESTAMP_GMT,
				 jn.FINAL_APPROVAL_FLAG,
				 jn.LAST_TRD_EDIT_TIMESTAMP_GMT,
				 jn.OPS_DET_ACT_FLAG,
				 jn.READY_FOR_FINAL_APPROVAL_FLAG,
				 jn.HAS_PROBLEM_FLAG,
				 jn.TRANSACTION_SEQ,
				 jn.FINAL_APPROVAL_TIMESTAMP_GMT,
				 jn.CMT,
				 jn.CPTY_TRADE_ID
			)
			SELECT 
				@tmp_username,
				GETDATE(),
				'D',
				@tmp_hostname,
				d.ID,
				d.TRADE_ID,
				d.OPEN_RQMTS_FLAG,
				d.CATEGORY,
				d.LAST_UPDATE_TIMESTAMP_GMT,
				d.FINAL_APPROVAL_FLAG,
				d.LAST_TRD_EDIT_TIMESTAMP_GMT,
				d.OPS_DET_ACT_FLAG,
				d.READY_FOR_FINAL_APPROVAL_FLAG,
				d.HAS_PROBLEM_FLAG,
				d.TRANSACTION_SEQ,
				d.FINAL_APPROVAL_TIMESTAMP_GMT,
				d.CMT,
				d.CPTY_TRADE_ID
			FROM deleted d
			RETURN
			END

END
GO
PRINT 'TRIGGER TG_TRADE_SUMRY_JN_AER_DIU ALTERED WITH SUCCESS'
GO
