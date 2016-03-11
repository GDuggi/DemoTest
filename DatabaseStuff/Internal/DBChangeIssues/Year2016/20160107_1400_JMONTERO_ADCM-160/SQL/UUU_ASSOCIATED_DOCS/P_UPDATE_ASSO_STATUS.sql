SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [ConfirmMgr].[P_UPDATE_ASSO_STATUS]
@p_inb_id			int,
@p_file_name		varchar(150),
@p_trade_id			int,
@p_status			varchar(25),
@p_cdty_grp_code	varchar(20),
@p_cpty_sn			varchar(50),
@p_broker_sn		varchar(50),
@p_rqmt_id			int,
@p_rqmt_status		varchar(80),
@p_rqmt_type		varchar(20),
@p_sec_check		varchar(1),
@p_index_val		int
AS
/******************************************************************************
*
* AUTHOR:		Stanford Developers - 
* MODIFIED:		Javier Montero - 10/09/2015
* DB:			SQL SERVER 2012 OR HIGHER
* VERSION:		1.0
* DESCRIPTION:  This procedure use a Sequences and other own code of SQL Server		
				2012
* DEPENDECIES:  ConfirmMgr.SEQ_ASSOCIATE_DOCS & ConfirmMgr.associated_docs
* CHANGES:		
*******************************************************************************/
declare 
@p_id		int

BEGIN

	SET @p_id = 0
		begin
            set @p_id = (select id from ConfirmMgr.associated_docs
             where inbound_docs_id = @p_inb_id
               and index_val = @p_index_val            )            
        end ;


		IF @p_status = 'ASSOCIATED'
			BEGIN
                insert into ConfirmMgr.associated_docs
                       (id,inbound_docs_id,index_val,file_name,trade_id,
                        doc_status_code,associated_by,associated_dt,
                        cdty_group_code,cpty_sn,broker_sn,doc_type_code,
                        sec_validate_req_flag,trade_rqmt_id)
                values (NEXT VALUE FOR ConfirmMgr.SEQ_ASSOCIATE_DOCS,@p_inb_id,@p_index_val,@p_file_name,
                        @p_trade_id,@p_status,SUSER_NAME(),GETDATE(),@p_cdty_grp_code,@p_cpty_sn,@p_broker_sn,
                        @p_rqmt_type,@p_sec_check,@p_rqmt_id);
			 END
            else if @p_status = 'UNASSOCIATED'
				BEGIN
                insert into ConfirmMgr.associated_docs
                       (id,inbound_docs_id,index_val,file_name,trade_id,
                        doc_status_code,
                        cdty_group_code,cpty_sn,broker_sn,doc_type_code,
                        sec_validate_req_flag,trade_rqmt_id)
                values (NEXT VALUE FOR ConfirmMgr.SEQ_ASSOCIATE_DOCS,@p_inb_id,@p_index_val,@p_file_name,
                        @p_trade_id,@p_status,@p_cdty_grp_code,@p_cpty_sn,@p_broker_sn,
                        @p_rqmt_type,@p_sec_check,@p_rqmt_id);

				END

            else if @p_status = 'DISPUTED'

                insert into ConfirmMgr.associated_docs
                       (id,inbound_docs_id,index_val,file_name,trade_id,
                        doc_status_code,disputed_by,disputed_dt,
                        cdty_group_code,cpty_sn,broker_sn,doc_type_code,
                        sec_validate_req_flag,trade_rqmt_id,associated_by,associated_dt)
                values (NEXT VALUE FOR ConfirmMgr.SEQ_ASSOCIATE_DOCS,@p_inb_id,@p_index_val,@p_file_name,
                        @p_trade_id,@p_status,SUSER_NAME(),GETDATE(),@p_cdty_grp_code,@p_cpty_sn,@p_broker_sn,
                        @p_rqmt_type,@p_sec_check,@p_rqmt_id,SUSER_NAME(),GETDATE());

            else if @p_status = 'APPROVED'  or @p_status =  'PRE-APPROVED'
			BEGIN
                insert into ConfirmMgr.associated_docs
                       (id,inbound_docs_id,index_val,file_name,trade_id,
                        doc_status_code,final_approved_by,final_approved_dt,
                        cdty_group_code,cpty_sn,broker_sn,doc_type_code,
                        sec_validate_req_flag,trade_rqmt_id,associated_by,associated_dt)
                values (NEXT VALUE FOR ConfirmMgr.SEQ_ASSOCIATE_DOCS,@p_inb_id,@p_index_val,@p_file_name,
                        @p_trade_id,@p_status,SUSER_NAME(),GETDATE(),@p_cdty_grp_code,@p_cpty_sn,@p_broker_sn,
                        @p_rqmt_type,@p_sec_check,@p_rqmt_id,SUSER_NAME(),GETDATE());

            end

        else
		BEGIN
            if @p_status = 'ASSOCIATED'
			BEGIN
                update ConfirmMgr.associated_docs
                   set doc_status_code = @p_status,
                       file_name = @p_file_name,
                       index_val = @p_index_val,
                       trade_id = @p_trade_id,
                       trade_rqmt_id = @p_rqmt_id,
                       cpty_sn = @p_cpty_sn,
                       broker_sn = @p_broker_sn,
                       cdty_group_code = @p_cdty_grp_code,
                       doc_type_code = @p_rqmt_type,
                       sec_validate_req_flag = @p_sec_check,
                       associated_dt = GETDATE(),
                       associated_by = SUSER_NAME()
                 where id = @p_id;
			END
            else if  @p_status = 'UNASSOCIATED'
			BEGIN
                update ConfirmMgr.associated_docs
                   set doc_status_code = @p_status,
                       index_val = @p_index_val,
                       trade_id = 0,
                       trade_rqmt_id = 0,
                       file_name = @p_file_name,
                       sec_validate_req_flag = @p_sec_check,
                       cpty_sn = @p_cpty_sn,
                       broker_sn = @p_broker_sn,
                       cdty_group_code = @p_cdty_grp_code,
                       doc_type_code = null,
                       associated_dt = null,
                       associated_by = null,
                       final_approved_by = null,
                       final_approved_dt = null,
                       disputed_by = null,
                       disputed_dt = null
                 where id = @p_id;
				 END
            else if @p_status = 'DISPUTED'
			BEGIN
                update ConfirmMgr.associated_docs
                 set doc_status_code = @p_status,
                      associated_by = CASE WHEN associated_by =  'UNASSOCIATED' THEN SUSER_NAME() WHEN associated_by = NULL THEN SUSER_NAME() ELSE associated_by END, --decode(associated_by,'UNASSOCIATED',user,null,user,associated_by)
                      associated_dt  = CASE WHEN associated_dt = NULL THEN GETDATE() ELSE associated_dt END, --decode(associated_dt,null,sysdate,associated_dt),
                       trade_id = @p_trade_id,
                       trade_rqmt_id = @p_rqmt_id,
                       index_val = @p_index_val,
                       file_name = @p_file_name,
                       cpty_sn = @p_cpty_sn,
                       broker_sn = @p_broker_sn,
                       cdty_group_code = @p_cdty_grp_code,
                       doc_type_code = @p_rqmt_type,
                       sec_validate_req_flag = @p_sec_check,
                       disputed_dt = GETDATE(),
                       disputed_by = SUSER_NAME()
                 where id = @p_id;
				END
            else if @p_status = 'APPROVED'  or @p_status =  'PRE-APPROVED'
			BEGIN
                update ConfirmMgr.associated_docs
                 set doc_status_code = @p_status,
                       associated_by = CASE WHEN associated_by =  'UNASSOCIATED' THEN SUSER_NAME() WHEN associated_by = NULL THEN SUSER_NAME() ELSE associated_by END, --decode(associated_by,'UNASSOCIATED',user,null,user,associated_by),
                       associated_dt  = CASE WHEN associated_dt = NULL THEN GETDATE() ELSE associated_dt END,  --decode(associated_dt,null,sysdate,associated_dt),
                       index_val = @p_index_val,
                       trade_id = @p_trade_id,
                       trade_rqmt_id = @p_rqmt_id,
                       file_name = @p_file_name,
                       cpty_sn = @p_cpty_sn,
                       broker_sn = @p_broker_sn,
                       cdty_group_code = @p_cdty_grp_code,
                       doc_type_code = @p_rqmt_type,
                       sec_validate_req_flag = @p_sec_check,
                       final_approved_dt = GETDATE(),
                       final_approved_by = SUSER_NAME()
                 where id = @p_id;
				END;

        END --ELSE
		

END









