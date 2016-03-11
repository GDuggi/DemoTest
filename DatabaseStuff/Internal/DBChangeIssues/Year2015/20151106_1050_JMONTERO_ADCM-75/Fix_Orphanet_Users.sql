/**********************************************************************************************************
*
* AUTHOR:		Javier Montero - 11/02/2015
* MODIFIED:		
* DB:			SQL SERVER 2012 OR HIGHER
* VERSION:		1.0
* DESCRIPTION:  This script fix the orphanet user in the DB also create the login and map the user 
* DEPENDECIES:   
* CHANGES:		
**********************************************************************************************************/

declare 
@logins			sysname,
@errormsg		varchar(4000),
@errorid		int,
@errorstate		int,
@errorsev		int,
@errorline		int,
@min			varchar(200),
@sql			nvarchar(max)

BEGIN
	
	IF OBJECT_ID('tempdb..#usersfix','U') IS NOT NULL
	EXEC ('DROP TABLE tempdb..#usersfix');


	BEGIN TRY
		CREATE TABLE #usersfix (username varchar(200), id_user varbinary(max)); /*Creating temp table to get the orphanet users*/

		INSERT INTO #usersfix
		EXEC sp_change_users_login 'REPORT'; /*With this procedure we get the list of orphanet users present in the DB*/

		SELECT @min = MIN(username) FROM #usersfix WHERE username NOT LIKE 'dbo'
		WHILE(@min) IS NOT NULL
		BEGIN
			/*It validate if the orphanet user exists in the SQL Server Logins, if not the user will be created here*/
			IF NOT EXISTS(SELECT 1 FROM sys.server_principals WHERE type IN ('S', 'U') AND name NOT LIKE '%##%' AND name = @min)
				BEGIN
					PRINT 'USER ' + @min+' NOT EXISTS ON SERVER '
					declare @usu sysname, @pass sysname
					SELECT @usu = cast(@min as sysname)
					SELECT @pass = cast(@min as sysname)
					SET @sql = N'CREATE LOGIN [' + cast(@min as nvarchar) + ']'
					SET @sql = @sql + N' WITH PASSWORD = ''' + cast(@min as nvarchar) + ''''
					SET @sql = @sql + N', DEFAULT_DATABASE = [master], CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF'
					EXEC sp_executesql @sql
					PRINT @min + ' USER CREATED SUCCESSFULLY!!! ' 
					/*The users was created in SQL Server Logins but is orphanet yet then this section fix the issue and map the user with the DB*/
					EXEC sp_change_users_login 'AUTO_FIX', @min; 
					PRINT 'USER ' + @min + ' MAPPED ON DB WITH SUCCESS!!! ' 
					
				END
			ELSE
				BEGIN
					PRINT 'FIXING ORPHANET USERS '
					EXEC sp_change_users_login 'AUTO_FIX', @min; /*Fixing the orphanet users*/
					PRINT 'USER ' + @min + ' FIXED'

				END

			SELECT @min = MIN(username) FROM #usersfix WHERE username > @min AND username NOT LIKE 'dbo'
		END 
		
		IF OBJECT_ID('#usersfix','U') IS NOT NULL
		EXEC ('DROP TABLE tempdb..#usersfix');
		PRINT 'TEMP TABLE tempdb..#usersfix DROPPED, HAVE A NICE DAY :-)'
	
	END TRY

	BEGIN CATCH
		SELECT @errormsg = 'ERROR: ' + ERROR_MESSAGE(),
			   @errorid = ERROR_NUMBER(),
			   @errorsev = ERROR_SEVERITY(),
			   @errorstate = ERROR_STATE(),
			   @errorline = ERROR_LINE()

		RAISERROR( @errormsg, @errorid, @errorline, @errorstate, @errorsev) WITH LOG, NOWAIT
		PRINT @errormsg
	END CATCH
	
END


