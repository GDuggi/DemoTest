IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'P_INSERT_XMIT_RESULT')
DROP PROCEDURE ConfirmMgr.P_INSERT_XMIT_RESULT
GO

CREATE PROCEDURE ConfirmMgr.P_INSERT_XMIT_RESULT
	@p_xmit_request_id int,	
	@p_xmit_status_ind	varchar(1),
	@p_xmit_method_ind	varchar(1) = null,
	@p_xmit_dest		varchar(max) = null,
	@p_xmit_cmt			varchar(max) = null
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @error_msg		nvarchar(max),
				@error_id		int,
				@error_sev		int,
				@error_sta		int,
				@error_line		int;

	if (@p_xmit_request_id is null)	
		THROW 50001, 'Validation Error: A xmit_request_id needs to be specified', 1;		

	if ((select count(id) from ConfirmMgr.XMIT_REQUEST where ID = @p_xmit_request_id) <= 0)	
		THROW 50000, 'Validation Error: Invalid xmit_request_id - no rows found', 1;			

	if (@p_xmit_method_ind is null or len(@p_xmit_method_ind) = 0)
	BEGIN
		select @p_xmit_method_ind = r.XMIT_METHOD_IND  from ConfirmMgr.XMIT_REQUEST r where r.ID = @p_xmit_request_id;
	END

	if (@p_xmit_dest is null or len(@p_xmit_dest) = 0)
	BEGIN
		select @p_xmit_dest = r.XMIT_DEST  from ConfirmMgr.XMIT_REQUEST r where r.ID = @p_xmit_request_id;		
	END			
	
	BEGIN TRANSACTION
	BEGIN TRY

		DECLARE @nextId int;
		select @nextId = next value for ConfirmMgr.SEQ_XMIT_RESULT;

		INSERT INTO [ConfirmMgr].[XMIT_RESULT]
				([ID]
				,[XMIT_REQUEST_ID]
				,[XMIT_STATUS_IND]
				,[XMIT_METHOD_IND]
				,[XMIT_DEST]
				,[XMIT_CMT]
				,[XMIT_TIMESTAMP])
			VALUES
				(@nextId
				,@p_xmit_request_id
				,@p_xmit_status_ind
				,@p_xmit_method_ind
				,@p_xmit_dest
				,@p_xmit_cmt
				,GetDate());

		DECLARE @TradeReqConfirmId int;		
		Select @TradeReqConfirmId = xr.TRADE_RQMT_CONFIRM_ID from ConfirmMgr.XMIT_REQUEST xr where xr.id = @p_xmit_request_id
		
		if (@TradeReqConfirmId is not null)
		BEGIN
			UPDATE trc
				SET 
					trc.XMIT_STATUS_IND = ISNULL(trc.XMIT_STATUS_IND, '') + ISNULL(
						CASE isnull(trc.XMIT_STATUS_IND, NULL)
							WHEN NULL THEN NULL
							ELSE ';'
						END, '') + ISNULL(@p_xmit_status_ind, ''), 
					trc.XMIT_ADDR = ISNULL(trc.XMIT_ADDR, '') + ISNULL(
						CASE isnull(trc.XMIT_ADDR, NULL)
							WHEN NULL THEN NULL
							ELSE ';'
						END, '') + ISNULL(@p_xmit_status_ind, '') + ':' + ISNULL(@p_xmit_dest, ''), 
					trc.XMIT_TIMESTAMP_GMT = sysdatetime()
				FROM ConfirmMgr.TRADE_RQMT_CONFIRM trc
				WHERE trc.ID = @TradeReqConfirmId;
				
			if (@p_xmit_status_ind = 'S' or @p_xmit_status_ind = 'F')
			BEGIN	
			
				DECLARE @trade_rqmt_id int;
				select @trade_rqmt_id = trc.RQMT_ID
					from ConfirmMgr.TRADE_RQMT_CONFIRM trc where trc.ID = @TradeReqConfirmId;

				UPDATE tr
					SET tr.STATUS = CASE 
										when @p_xmit_status_ind = 'S' then 'SENT'
										else 'FAIL'
									end
				 FROM ConfirmMgr.TRADE_RQMT  tr					
				 WHERE tr.ID = @trade_rqmt_id AND 
				 tr.STATUS IN 
					(
						SELECT rs.STATUS_CODE
						FROM ConfirmMgr.RQMT_STATUS  AS rs
						WHERE rs.TERMINAL_FLAG = 'N' AND rs.RQMT_CODE = tr.RQMT
					);
			END
		END	

		DECLARE @AssociatedDocId int;		
		Select @AssociatedDocId = xr.ASSOCIATED_DOCS_ID from ConfirmMgr.XMIT_REQUEST xr where xr.id = @p_xmit_request_id
		if (@AssociatedDocId is not null)
		BEGIN
			update ASSOCIATED_DOCS
				set XMIT_VALUE = @p_xmit_dest,
				XMIT_STATUS_CODE = 
					case @p_xmit_status_ind 
						when 'Q' then 'Queued'
						when 'S' then 'Success'
						when 'F' then 'Failed'
						else 'Unknown'
					end
			where id = @AssociatedDocId;						
		END

		COMMIT TRANSACTION	
	end TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		IF @@ERROR > 0
				SELECT @error_msg  = 'PROCEDURE  P_INSERT_XMIT_REQUEST FAIL: ' + ERROR_MESSAGE(),
					   @error_id = ERROR_NUMBER(),
					   @error_sev = ERROR_SEVERITY(),
					   @error_sta = ERROR_STATE(),
					   @error_line = ERROR_LINE();
				RAISERROR(@error_msg, @error_id, @error_sev, @error_sta, @error_line) WITH NOWAIT

	END CATCH
END
GO
PRINT 'PROCEDURE ConfirmMgr.P_INSERT_XMIT_RESULT CREATED SUCCESSFULLY'
GO