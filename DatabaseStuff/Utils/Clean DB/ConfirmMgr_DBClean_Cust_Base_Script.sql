set nocount on

create table #tables
(
   oid            int IDENTITY primary key,
   schema_id      int default 0 null,
   tablename      sysname null
)
go

insert into #tables (tablename) 
   values   ('XYZ'),
			(''),
			('Jack to add')
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
if object_id('tempdb..#tables', 'U') is not null
   exec('drop table #tables')
go
