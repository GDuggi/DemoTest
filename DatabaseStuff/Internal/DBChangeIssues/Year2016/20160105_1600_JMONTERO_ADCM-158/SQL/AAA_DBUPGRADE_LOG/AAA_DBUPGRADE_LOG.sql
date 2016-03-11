IF OBJECT_ID('ConfirmMgr.DBUPGRADE_LOG','U') IS NOT NULL
EXEC ('DROP TABLE ConfirmMgr.DBUPGRADE_LOG')
GO
CREATE TABLE ConfirmMgr.DBUPGRADE_LOG
(
	oid					int IDENTITY(1,1),
	owner_code			varchar(10) NOT NULL,
	major_revnum		varchar(20) NULL,
	minor_revnum		VARCHAR(20) NULL,
	last_touch_date		datetime2   NULL,
	data_source			VARCHAR(50) NULL,
	usage				VARCHAR(30)	NULL,
	script_reference	VARCHAR(80) NULL,
	note				VARCHAR(255) NULL,
	upgrade_date		DATETIME2	NOT NULL,
	upgraded_by			NVARCHAR(60)NOT NULL,
	hostname			NVARCHAR(60) NULL,
	opcode				CHAR		NOT NULL

);

ALTER TABLE ConfirmMgr.DBUPGRADE_LOG
ADD CONSTRAINT PK_dbupgrade_log PRIMARY KEY CLUSTERED (oid);

PRINT 'TABLE DBUPGRADE_LOG CREATED WITH SUCCESS'
GO