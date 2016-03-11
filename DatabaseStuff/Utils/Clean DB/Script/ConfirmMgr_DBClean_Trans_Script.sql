set nocount on

IF OBJECT_ID('#tables','U') IS NULL
create table #tables
(
   oid            int IDENTITY primary key,
   schema_id      int default 0 null,
   tablename      sysname null
)
go

insert into #tables (tablename) 
   values   ('XMIT_RESULT'),
			('XMIT_REQUEST'),
			('VAULTED_DOCS_BLOB'),
			('VAULTED_DOCS'),
			('ASSOCIATED_DOCS_BLOB'),
			('ASSOCIATED_DOCS'),
			('ASSOCIATED_DOCS_JN'),
			('INBOUND_DOC_USER_FLAG'),
			('INBOUND_DOC_USER_FLAG_JN'),
			('INBOUND_DOCS_BLOB'),
			('INBOUND_DOCS'),
			('INBOUND_DOCS_JN'),
			('TRADE_RQMT_CONFIRM_CREATOR'),
			('TRADE_RQMT_CONFIRM_CREATOR_JN '),
			('FAX_LOG_STATUS '),
			('TRADE_RQMT_CONFIRM_BLOB '),
			('RQMT_EXT_PROCESS_DATA '),
			('TRADE_RQMT_CONFIRM '),
			('TRADE_RQMT_CONFIRM_JN '),
			('FAX_LOG_SENT '),
			('TRADE_GROUP '),
			('TRADE_PRIORITY '),
			('TRADE_EXT_PROCESS_DATA '),
			('TRADE_APPR '),
			('TRADE_APPR_JN '),
			('TRADE_RQMT '),
			('TRADE_RQMT_JN '),
			('TRADE_DATA '),
			('TRADE_DATA_JN '),
			('TRADE_SUMMARY '),
			('TRADE_SUMMARY_JN '),
			('TRADE_NOTIFY '),
			('TRADE '),
			('IGNORED_NOTIFICATIONS '),
			('ALERT_MSG_LOG')
go


update a
set a.schema_id = t.schema_id
from #tables a join sys.tables t
on a.tablename = t.name  --changed by Javier Montero / Replace the where clause by ON
go

delete #tables
where schema_id is null

declare @tablename     sysname,
        @schema_name   sysname,
        @smsg          varchar(800),
        @sql           varchar(max),
        @oid           int
        
select @oid = min(oid)
from #tables

BEGIN TRAN T1;
while @oid is not null
begin
   select @schema_name = SCHEMA_NAME(schema_id),
          @tablename = tablename
   from #tables
   where oid = @oid
   
   set @sql = 'delete from ' + @schema_name + '.' + @tablename
   begin try
     exec(@sql)
   end try
   begin catch
     ROLLBACK TRAN T1;
     set @smsg = '=> Failed to truncate the ''' + @schema_name + '.' + @tablename + ''' table due to the error:'
     RAISERROR(@smsg, 0, 1) WITH NOWAIT
     set @smsg = '==> ERROR: ' + ERROR_MESSAGE()
     RAISERROR(@smsg, 0, 1) WITH NOWAIT
     break
   end catch
   set @smsg = CAST(@@ROWCOUNT AS VARCHAR(10))+ ' => Records in the ''' + @schema_name + '.' + @tablename + ''' table was truncated successfully!'
   RAISERROR(@smsg, 0, 1) WITH NOWAIT

   select @oid = min(oid)
   from #tables 
   where oid > @oid  
end
Commit TRAN T1;
if object_id('tempdb..#tables', 'U') is not null
   exec('drop table #tables')
go
PRINT 'CONFIRMATION MANAGER DATABASE CLEAN UP FINISHED!!!!'
GO