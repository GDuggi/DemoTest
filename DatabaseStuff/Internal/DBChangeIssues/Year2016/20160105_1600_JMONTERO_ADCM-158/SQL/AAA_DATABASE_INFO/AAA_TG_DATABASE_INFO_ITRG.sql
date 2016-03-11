IF OBJECT_ID('TG_DATABASE_INFO_ITRG','TR') IS NOT NULL
EXEC ('DROP TRIGGER ConfirmMgr.TG_DATABASE_INFO_ITRG')
GO

CREATE trigger [ConfirmMgr].[TG_DATABASE_INFO_ITRG]
on [ConfirmMgr].[DATABASE_INFO]
for insert
as
/******************************************************************************
*
* AUTHOR:		JAVIER MONTERO - 01/05/2016
* DB:			SQL SERVER 2008 OR HIGHER
* VERSION:		1.0
* DESCRIPTION:  TRIGGER FOR INSERT DATABASE INFORMATION AFTER UPGRADES
* DEPENDECIES:  TABLE DATABASE_INFO IS REQUIERED
*
*******************************************************************************/
declare @num_rows    int,
        @hostname    nchar(128)

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
      'I'
   from inserted

return;



