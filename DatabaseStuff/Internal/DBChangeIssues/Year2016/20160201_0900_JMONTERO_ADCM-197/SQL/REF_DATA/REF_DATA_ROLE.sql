IF OBJECT_ID('tempdb..#ROLE_TEMP')IS NOT NULL
EXEC ('DROP TABLE #ROLE_TEMP')
GO
PRINT 'Startup Loading Ref Data on ROLE Table'
GO

CREATE TABLE #ROLE_TEMP(
	[CODE] [varchar](10) NOT NULL,
	[DESCR] [varchar](60) NOT NULL,
	[ACTIVE_FLAG] [varchar](1) NOT NULL 
) 
  
GO
PRINT 'LOADING DATA ON TEMP TABLE';
INSERT INTO #ROLE_TEMP (CODE, DESCR, ACTIVE_FLAG) VALUES ('ACCESS', 'can logon and only view the data', 'Y')
INSERT INTO #ROLE_TEMP (CODE, DESCR, ACTIVE_FLAG) VALUES ('CNTRCT-APP', 'Can approve contracts', 'Y')
INSERT INTO #ROLE_TEMP (CODE, DESCR, ACTIVE_FLAG) VALUES ('FNAPP', 'Can final approve trades', 'Y')
INSERT INTO #ROLE_TEMP (CODE, DESCR, ACTIVE_FLAG) VALUES ('FNAPP-OVR', 'can final approve trades that have open rqmts (not ready for final approval)', 'Y')
INSERT INTO #ROLE_TEMP (CODE, DESCR, ACTIVE_FLAG) VALUES ('UPDATE', 'can modify data, add requirements, etc', 'Y')

PRINT 'LOADING DATA ON ROLE TABLE';
Declare
@pkey	    varchar(10),
@errormsg	nvarchar(max)

BEGIN 
	SELECT @pkey = min(CODE) FROM #ROLE_TEMP;

	WHILE @pkey IS NOT NULL
	BEGIN 
		IF NOT EXISTS(SELECT 1 FROM ConfirmMgr.ROLE WHERE CODE = @pkey)
		BEGIN
			BEGIN TRY
				INSERT INTO ConfirmMgr.ROLE
				SELECT C.CODE, C.DESCR, C.ACTIVE_FLAG
				FROM #ROLE_TEMP C
				WHERE C.CODE = @pkey
			END TRY
			BEGIN CATCH
				SELECT @errormsg = 'ERROR MESSAGE: ' + ERROR_MESSAGE() + ' LINE: ' + CAST(ERROR_LINE() AS nvarchar);
				THROW 51000, @errormsg, 1
			END CATCH 
		END
		SELECT @pkey = min(CODE) FROM #ROLE_TEMP WHERE CODE > @pkey
	END
	IF OBJECT_ID('tempdb..#ROLE_TEMP')IS NOT NULL
	EXEC ('DROP TABLE #ROLE_TEMP');
END
GO
PRINT 'LOAD REF DATA ON ROLE TABLE FINISHED'
GO








