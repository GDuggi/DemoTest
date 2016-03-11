IF OBJECT_ID('TG_DATABASE_INFO_UTRG','TR') IS NOT NULL
EXEC ('DROP TRIGGER ConfirmMgr.TG_DATABASE_INFO_UTRG')
GO
CREATE trigger [ConfirmMgr].[TG_DATABASE_INFO_UTRG]
on [ConfirmMgr].[DATABASE_INFO]
for update
as
/***************************************************************************************
*
* AUTHOR:		JAVIER MONTERO - 01/05/2016
* DB:			SQL SERVER 2008 OR HIGHER
* VERSION:		1.0
* DESCRIPTION:  TRIGGER FOR UPDATE DATABASE INFORMATION AFTER UPGRADES ON TABLE INFO
* DEPENDECIES:  TABLE DATABASE_INFO IS REQUIERED
*
****************************************************************************************/
declare @num_rows         int,
        @hostname         nchar(128)

select @num_rows = @@rowcount
if @num_rows = 0
   return

   select @hostname = hostname
   from master.dbo.sysprocesses
   where spid = @@spid
   
   insert into ConfirmMgr.dbupgrade_log 
      (owner_code, 
       major_revnum,
       minor_revnum,
       last_touch_date,
       data_source,
       usage,
       script_reference,
       note,
       upgrade_date,
       upgraded_by,
       hostname,
       opcode)
   select 
      owner_code, 
      major_revnum,
      minor_revnum,
      last_touch_date,
      data_source,
      usage,
      script_reference,
      note, 
      getdate(),
      suser_name(),
      @hostname,
      'U'
   from deleted

return
