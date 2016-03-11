print 'Creating UNIQUE constraint for the TRADE_GROUP.TRADE_ID if NOT EXISTS ...'
go
if not exists (select 1
               from sys.indexes
               where name = 'TRADE_GROUP_UQ1' and
                     is_unique_constraint = 1)
   exec('alter table ConfirmMgr.TRADE_GROUP add constraint TRADE_GROUP_UQ1 UNIQUE (TRADE_ID)')
else
   print '=> The unique constraint ''TRADE_GROUP_UQ1'' exists in database already!'
go
