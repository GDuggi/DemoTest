
ALTER TRIGGER [ConfirmMgr].[TG_TRADERQMTCONFIRM_JN_AER_DIU]
ON [ConfirmMgr].[TRADE_RQMT_CONFIRM]
AFTER INSERT, UPDATE, DELETE
AS

/******************************************************************************
*
* AUTHOR:		JAVIER MONTERO - 07/27/2015
* DB:			SQL SERVER 2008 OR HIGHER
* VERSION:		1.0
* DESCRIPTION:  TRIGGER FOR DELETE ACTIONS ON TABLE TRADE_RQMT_CONFIRM
* DEPENDECIES:  TABLE TRADE_RQMT_CONFIRM_JN IS REQUIERED
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
				SET @errormsg = 'INSERT ON TRADE_RQMT_CONFIRM_JN WAS ROLLBACKED BECAUSE USER_NAME OR HOST_NAME IS NULL'
				RAISERROR(@errormsg, 0,29000)
				
				ROLLBACK TRAN
				RETURN
			END	
	
		/*UPDATE*/
		IF EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted)
			BEGIN
			/*IF the ROWS exists in the inserted and exists in the deleted table the process shoot the action 
			to INSERT the values in the table TRADE_RQMT_CONFIRM_JN*/
			   INSERT ConfirmMgr.TRADE_RQMT_CONFIRM_JN(
			 	 jn.JN_ORACLE_USER,
				 jn.JN_DATETIME,
				 jn.JN_OPERATION,
				 jn.JN_HOST_NAME,
				 jn.ID,
				 jn.RQMT_ID,
				 jn.TRADE_ID,
				 jn.NEXT_STATUS_CODE,
				 jn.CONFIRM_LABEL,
				 jn.CONFIRM_CMT,
				 jn.FAX_TELEX_IND,
				 jn.FAX_TELEX_NUMBER,
				 jn.XMIT_STATUS_IND,
				 jn.XMIT_ADDR,
				 jn.XMIT_CMT,
				 jn.XMIT_TIMESTAMP_GMT,
				 jn.ACTIVE_FLAG,
				 jn.TEMPLATE_NAME,
				 jn.PREPARER_CAN_SEND_FLAG
				)
			SELECT 
				@tmp_username,
				GETDATE(),
				'U',
				@tmp_hostname,
				d.ID,
				d.RQMT_ID,
				d.TRADE_ID,
				d.NEXT_STATUS_CODE,
				d.CONFIRM_LABEL,
				d.CONFIRM_CMT,
				d.FAX_TELEX_IND,
				d.FAX_TELEX_NUMBER,
				d.XMIT_STATUS_IND,
				d.XMIT_ADDR,
				d.XMIT_CMT,
				d.XMIT_TIMESTAMP_GMT,
				d.ACTIVE_FLAG,
				d.TEMPLATE_NAME,
				d.PREPARER_CAN_SEND_FLAG
			FROM inserted d
			INNER JOIN TRADE_RQMT_CONFIRM tr 
			ON d.ID = tr.ID
			RETURN
		    END
		
		/*INSERT*/
		IF EXISTS(SELECT 1 FROM inserted) AND NOT EXISTS(SELECT * FROM deleted)			
			BEGIN
			/*If the ROWS exists in the inserted and not exists in the deleted table the process shoot the action 
			to INSERT the values in the table TRADE_RQMT_CONFIRM_JN*/
			 INSERT ConfirmMgr.TRADE_RQMT_CONFIRM_JN(
			 	 jn.JN_ORACLE_USER,
				 jn.JN_DATETIME,
				 jn.JN_OPERATION,
				 jn.JN_HOST_NAME,
				 jn.ID,
				 jn.RQMT_ID,
				 jn.TRADE_ID,
				 jn.NEXT_STATUS_CODE,
				 jn.CONFIRM_LABEL,
				 jn.CONFIRM_CMT,
				 jn.FAX_TELEX_IND,
				 jn.FAX_TELEX_NUMBER,
				 jn.XMIT_STATUS_IND,
				 jn.XMIT_ADDR,
				 jn.XMIT_CMT,
				 jn.XMIT_TIMESTAMP_GMT,
				 jn.ACTIVE_FLAG,
				 jn.TEMPLATE_NAME,
				 jn.PREPARER_CAN_SEND_FLAG
				)
			SELECT 
				@tmp_username,
				GETDATE(),
				'I',
				@tmp_hostname,
				d.ID,
				d.RQMT_ID,
				d.TRADE_ID,
				d.NEXT_STATUS_CODE,
				d.CONFIRM_LABEL,
				d.CONFIRM_CMT,
				d.FAX_TELEX_IND,
				d.FAX_TELEX_NUMBER,
				d.XMIT_STATUS_IND,
				d.XMIT_ADDR,
				d.XMIT_CMT,
				d.XMIT_TIMESTAMP_GMT,
				d.ACTIVE_FLAG,
				d.TEMPLATE_NAME,
				d.PREPARER_CAN_SEND_FLAG
				FROM inserted d
			RETURN
			END

		 /*DELETE*/
		 IF NOT EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted)
			/*If the ROWS exists in the deleted table the process shoot the action to INSERT the values in the table TRADE_RQMT_CONFIRM_JN*/
			BEGIN
				INSERT ConfirmMgr.TRADE_RQMT_CONFIRM_JN(
			 	 jn.JN_ORACLE_USER,
				 jn.JN_DATETIME,
				 jn.JN_OPERATION,
				 jn.JN_HOST_NAME,
				 jn.ID,
				 jn.RQMT_ID,
				 jn.TRADE_ID,
				 jn.NEXT_STATUS_CODE,
				 jn.CONFIRM_LABEL,
				 jn.CONFIRM_CMT,
				 jn.FAX_TELEX_IND,
				 jn.FAX_TELEX_NUMBER,
				 jn.XMIT_STATUS_IND,
				 jn.XMIT_ADDR,
				 jn.XMIT_CMT,
				 jn.XMIT_TIMESTAMP_GMT,
				 jn.ACTIVE_FLAG,
				 jn.TEMPLATE_NAME,
				 jn.PREPARER_CAN_SEND_FLAG
				)
			SELECT 
				@tmp_username,
				GETDATE(),
				'D',
				@tmp_hostname,
				d.ID,
				d.RQMT_ID,
				d.TRADE_ID,
				d.NEXT_STATUS_CODE,
				d.CONFIRM_LABEL,
				d.CONFIRM_CMT,
				d.FAX_TELEX_IND,
				d.FAX_TELEX_NUMBER,
				d.XMIT_STATUS_IND,
				d.XMIT_ADDR,
				d.XMIT_CMT,
				d.XMIT_TIMESTAMP_GMT,
				d.ACTIVE_FLAG,
				d.TEMPLATE_NAME,
				d.PREPARER_CAN_SEND_FLAG
				FROM deleted d
			RETURN
			END

END
