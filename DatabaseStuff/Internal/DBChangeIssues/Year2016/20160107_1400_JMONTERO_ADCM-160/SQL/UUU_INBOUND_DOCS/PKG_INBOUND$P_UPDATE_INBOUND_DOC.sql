SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [ConfirmMgr].[PKG_INBOUND$P_UPDATE_INBOUND_DOC]

   @p_id int,
   @p_caller_ref varchar(250),
   @p_sent_to varchar(250),
   @p_file_name varchar(150), --modified by Javier Montero, column lenght increse to 150. ADCM-160
   @p_sender varchar(40),
   @p_cmt varchar(100),
   @p_doc_status_code varchar(25)
AS 
   BEGIN


      UPDATE ConfirmMgr.INBOUND_DOCS
         SET 
            CALLER_REF = @p_caller_ref, 
            SENT_TO = @p_sent_to, 
            FILE_NAME = @p_file_name, 
            SENDER = @p_sender, 
            CMT = @p_cmt, 
            DOC_STATUS_CODE = @p_doc_status_code
      WHERE INBOUND_DOCS.ID = @p_id

   END
