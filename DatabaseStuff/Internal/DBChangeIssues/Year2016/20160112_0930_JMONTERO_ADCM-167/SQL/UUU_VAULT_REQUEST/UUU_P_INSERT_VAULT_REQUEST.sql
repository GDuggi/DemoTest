ALTER PROCEDURE [ConfirmMgr].[P_INSERT_VAULT_REQUEST]
@p_trade_rqmt_confirm_id           int,
@p_associated_docs_id           int,
@p_sent_flag      varchar(1),
@p_metadata   varchar(500)
AS
/******************************************************************************
*
* AUTHOR:		JAVIER MONTERO - 11/02/2015
* MODIFIED:     JAVIER MONTERO - 01/12/2016 - ADCM-165
* DB:			SQL SERVER 2008 OR HIGHER
* VERSION:		1.0
* DESCRIPTION:  
* DEPENCIES:    TABLE VAULTED_DOCS IS REQUIERED
*
*******************************************************************************/
BEGIN

DECLARE
@error_msg		nvarchar(max),
@error_id		int,
@error_sev		int,
@error_sta		int,
@error_line		int


    IF @p_trade_rqmt_confirm_id is null and @p_associated_docs_id is null
	     BEGIN
			SET @error_msg = 'PROCEDURE P_INSERT_VAULT_REQUEST FAIL: TRADE_RQMT_CONFIRM_ID AND ASSOCITATED_DOCS_ID ARE NULL. ONE FIELD MUST HAVE A VALUE'
			RAISERROR(@error_msg,1,1) WITH NOWAIT;
			
		 END
	ELSE IF @p_trade_rqmt_confirm_id is not null and @p_associated_docs_id is not null
	     BEGIN
			SET @error_msg = 'PROCEDURE P_INSERT_VAULT_REQUEST FAIL: TRADE_RQMT_CONFIRM_ID AND ASSOCITATED_DOCS_ID HAVE VALUES. ONE FIELD MUST BE NULL.'
			RAISERROR(@error_msg,1,1) WITH NOWAIT;			
			
		 END
	ELSE IF @p_associated_docs_id is not null and @p_sent_flag = 'N' 
	     BEGIN
			SET @error_msg = 'PROCEDURE P_INSERT_VAULT_REQUEST FAIL: WHEN AN ASSOCIATED_DOC IS VAULTED, THEN SENT FLAG MUST BE Y.'
			RAISERROR(@error_msg,1,1) WITH NOWAIT;			
			
		 END
    ELSE
	BEGIN

	Declare @id as INT
    set @id = next value for confirmmgr.seq_VAULTED_DOCS
	
		BEGIN TRY
		  BEGIN TRANSACTION TRAN01

		 INSERT INTO ConfirmMgr.VAULTED_DOCS
		 (id, trade_rqmt_confirm_id, associated_docs_id, sent_flag, metadata, request_timestamp, processed_flag)
									 VALUES
									 (@id
                                      ,@p_trade_rqmt_confirm_id
									  ,@p_associated_docs_id
                                      ,@p_sent_flag
									  ,@p_metadata
                                      ,GETDATE()
                                      ,'N');
		
		
						  
	   if @p_associated_docs_id is not null 	   
		BEGIN
		insert into ConfirmMgr.VAULTED_DOCS_BLOB
			(ID, VAULTED_DOCS_ID, IMAGE_FILE_EXT, DOC_BLOB)
								select next value for confirmmgr.seq_VAULTED_DOCS_BLOB
								,@id
								,case when @p_sent_flag = 'Y' then ad.markup_image_file_ext else ad.orig_image_file_ext end 
						    	,case when @p_sent_flag = 'Y' then cast(ad.markup_image_blob as varbinary(max)) else cast(ad.orig_image_blob as varbinary(max)) end 
								FROM CONFIRMMGR.ASSOCIATED_DOCS_BLOB ad
									WHERE AD.ASSOCIATED_DOCS_ID = @p_associated_docs_id
		
		END
	   if @p_trade_rqmt_confirm_id is not null 
	   		BEGIN
			insert into ConfirmMgr.VAULTED_DOCS_BLOB
			(ID, VAULTED_DOCS_ID, IMAGE_FILE_EXT, DOC_BLOB)
								select next value for confirmmgr.seq_VAULTED_DOCS_BLOB
								,@id
								,image_file_ext
						    	,cast(doc_blob as varbinary(max))
								FROM CONFIRMMGR.TRADE_RQMT_CONFIRM_BLOB TRC
								WHERE trc.trade_rqmt_confirm_ID = @p_trade_rqmt_confirm_id;;
				END

				COMMIT TRANSACTION TRAN01
		END TRY

		BEGIN CATCH
			ROLLBACK TRANSACTION TRAN01
			
			IF @@ERROR > 0
				SELECT @error_msg  = 'PROCEDURE  P_INSERT_VAULT_REQUEST FAIL: ' + ERROR_MESSAGE(),
					   @error_id = ERROR_NUMBER(),
					   @error_sev = ERROR_SEVERITY(),
					   @error_sta = ERROR_STATE(),
					   @error_line = ERROR_LINE();
				RAISERROR(@error_msg, @error_id, @error_sev, @error_sta, @error_line) WITH NOWAIT

		END CATCH
	END



END

