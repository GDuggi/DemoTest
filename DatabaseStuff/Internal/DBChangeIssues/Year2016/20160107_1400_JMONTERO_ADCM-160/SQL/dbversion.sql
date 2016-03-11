print ' '
print 'Updating database version info ...'
go

IF EXISTS(SELECT 1 FROM dbo.database_info WHERE owner_code = 'CONFIRMGR')
BEGIN
	update dbo.database_info
	set major_revnum = '1.1.3',
    minor_revnum = '0',
    last_touch_date = getdate(),
    script_reference = 'dbver1.1.1000.1',
    note = 'dbver1.1.3 asof SPK11'
	where owner_code = 'CONFIRMGR'
END
ELSE
BEGIN
	INSERT INTO dbo.database_info
	(major_revnum, minor_revnum, last_touch_date, script_reference, note)
	VALUES
	('1.1.3', '0', GETDATE(),'dbver1.1.1000.1', 'dbver1.1.3 asof SPK11')
END;

go