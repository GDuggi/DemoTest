print ' '
print 'Updating database version info ...'
go

IF EXISTS(SELECT 1 FROM ConfirmMgr.database_info WHERE owner_code = 'CONFIRMGR')
BEGIN
	update ConfirmMgr.database_info
	set major_revnum = '1',
    minor_revnum = '0',
	patch_level = '6',
    last_touch_date = getdate(),
    note = 'dbver1.0.6 ADCM-193'
	where owner_code = 'CONFIRMGR'
END
ELSE
BEGIN
	INSERT INTO ConfirmMgr.database_info
	(major_revnum, minor_revnum, patch_level, last_touch_date, note)
	VALUES
	('1', '0', '6',GETDATE(),'dbver1.0.6 ADCM-193')
END;

go