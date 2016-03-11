IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'P_INSERT_XMIT_REQUEST')
DROP PROCEDURE ConfirmMgr.P_INSERT_XMIT_REQUEST
GO

CREATE PROCEDURE ConfirmMgr.P_INSERT_XMIT_REQUEST 
	@p_trade_rqmt_confirm_id  int = null,
	@p_associated_docs_id     int = null,
	@p_xmit_method_ind		  varchar(1),
	@p_xmit_dest			  varchar(max),
	@p_sent_by_user			  varchar(20)

AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @error_msg		nvarchar(max),
				@error_id		int,
				@error_sev		int,
				@error_sta		int,
				@error_line		int;

	if (@p_trade_rqmt_confirm_id is null and 
	    @p_associated_docs_id is null)	
		THROW 50001, 'Validation Error: A trade_rqmnt_confirm_id or a associated_docs_id needs to be specified', 1;			

	if (@p_xmit_method_ind is null or (@p_xmit_method_ind <> 'E' and @p_xmit_method_ind <> 'F'))
		THROW 50001, 'Validation Error: xmit_method_ind must be E (Email) or F (Fax)', 1;			

	if (@p_xmit_dest is null or len(@p_xmit_dest) = 0)
		THROW 50001, 'Validation Error: xmit_dest must be specified', 1;

	if (@p_sent_by_user is null or len(@p_sent_by_user ) = 0)
		THROW 50001, 'Validation Error: sent_by_user must be specified', 1;	
	
	BEGIN TRANSACTION 
	BEGIN TRY
		DECLARE @nextId int;

		select @nextId = next value for ConfirmMgr.SEQ_XMIT_REQUEST;

		INSERT INTO [ConfirmMgr].[XMIT_REQUEST]
				   ([ID]
				   ,[TRADE_RQMT_CONFIRM_ID]
				   ,[ASSOCIATED_DOCS_ID]
				   ,[XMIT_METHOD_IND]
				   ,[XMIT_DEST]
				   ,[SENT_BY_USER]
				   ,[REQUEST_TIMESTAMP])
			 VALUES
				   (@nextId
				   ,@p_trade_rqmt_confirm_id 
				   ,@p_associated_docs_id
				   ,@p_xmit_method_ind
				   ,@p_xmit_dest
				   ,@p_sent_by_user	
				   ,GetDate());

			if (@p_associated_docs_id is not null)
			BEGIN
				update ASSOCIATED_DOCS 
					set XMIT_VALUE = @p_xmit_dest,
						XMIT_STATUS_CODE = 'QUEUED'
				where id = @p_associated_docs_id
			end 

		COMMIT TRANSACTION;

		select @nextId;    
		return @nextId;

	end TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		
			IF @@ERROR > 0
				SELECT @error_msg  = 'PROCEDURE  P_INSERT_XMIT_REQUEST FAIL: ' + ERROR_MESSAGE(),
					   @error_id = ERROR_NUMBER(),
					   @error_sev = ERROR_SEVERITY(),
					   @error_sta = ERROR_STATE(),
					   @error_line = ERROR_LINE();
				RAISERROR(@error_msg, @error_id, @error_sev, @error_sta, @error_line) WITH NOWAIT
				return -1;
	END CATCH	
END
GO
PRINT 'PROCEDURE ConfirmMgr.P_INSERT_XMIT_REQUEST CREATED SUCCESSFULLY'
GO
