print ' '
print 'Updating database version info ...'
go

IF EXISTS(SELECT 1 FROM ConfirmMgr.database_info WHERE owner_code = 'CONFIRMGR')
BEGIN
	update ConfirmMgr.database_info
	set major_revnum = '1.1.5',
    minor_revnum = '0',
    last_touch_date = getdate(),
    script_reference = 'dbver1.1.1000.1',
    note = 'dbver1.1.5 ADCM-188'
	where owner_code = 'CONFIRMGR'
END
ELSE
BEGIN
	INSERT INTO ConfirmMgr.database_info
	(major_revnum, minor_revnum, last_touch_date, script_reference, note)
	VALUES
	('1.1.5', '0', GETDATE(),'dbver1.1.1000.1', 'dbver1.1.5 ADCM-188')
END;

go