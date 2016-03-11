set nocount on

IF OBJECT_ID('#tables','U') IS NULL
create table #tables
(
   oid            int IDENTITY primary key,
   schema_id      int default 0 null,
   tablename      sysname null
)
go

INSERT into #tables (tablename) 
   values('TRANS_TRANSMISSION'),
		 ('TRANS_ALT_NODES'),
		 ('TRANS_REQUEST'),
		 ('TRANS_PROCESS_LOG'),
		 ('TRANS_ATTACHMENTS'),
		 ('TRANS_ERROR_MESSAGES'),
		 ('TRANS_NOTIFICATION'),
		 ('TRANS_REQUEST_NOTIFICATION'),
		 ('TRANS_REPAIR_HISTORY'),
		 ('TRANS_MESSAGES'),
		 ('TRANS_PROCESS_INFO'),
		 ('TRANS_VAULT'),
		 ('TRANS_PROCESS_INFO'),
		 ('EIPP_SET_LINK'),
		 ('RIGHTFAX_NOTIFICATION'),
		 ('SET_ADDRESS'),
		 ('Survey') 
go

update a
set a.schema_id = t.schema_id
from #tables a join sys.tables t
on a.tablename = t.name
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
PRINT 'FAX OUT DATABASE CLEAN UP FINISHED!!!'
GO
