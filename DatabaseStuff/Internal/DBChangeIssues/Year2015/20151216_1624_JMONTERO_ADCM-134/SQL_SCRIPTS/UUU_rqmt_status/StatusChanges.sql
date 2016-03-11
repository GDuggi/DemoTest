--Author: Javier Montero
BEGIN
declare 
@error_msg		nvarchar(2000),
@errorid		int,
@errorsev		int,
@errorline		int

BEGIN TRY
BEGIN TRANSACTION rollb
	
	
	insert into ConfirmMgr.STATUS(code, descr, active_flag)
	values ('EXT_REVIEW','External Review','Y');
	
	update ConfirmMgr.rqmt_status set status_code='EXT_REVIEW' where status_code='CRDT';

	delete from ConfirmMgr.status where code='CRDT';

	insert into ConfirmMgr.status (code, descr, active_flag)
	values ('SIGNED','Signed Paper','Y');

	update ConfirmMgr.rqmt_status set status_code='SIGNED' where status_code='EXCTD';

	delete from ConfirmMgr.status where code='EXCTD';
	
	COMMIT TRANSACTION rollb

END TRY
BEGIN CATCH 
	ROLLBACK TRANSACTION rollb

	SELECT @error_msg = ERROR_MESSAGE(),
	       @errorid = ERROR_NUMBER(),
		   @errorline = ERROR_LINE(),
		   @errorsev = ERROR_SEVERITY()
	PRINT 'ACTIONS ROLLING BACK'
	PRINT 'Error ID: '+CAST(@errorid as nvarchar(max)) + ' Line: ' + CAST(@errorline as nvarchar(max)) + ' Severity: ' + CAST(@errorsev as nvarchar(max)) + ' Messsage: ' + @error_msg
END CATCH
END
GO