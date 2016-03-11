IF OBJECT_ID('ConfirmMgr.UDF_WEEKDAY_COUNT_BETWEEN_2_DAYS', 'FN') IS NOT NULL
DROP FUNCTION ConfirmMgr.UDF_WEEKDAY_COUNT_BETWEEN_2_DAYS
go
CREATE FUNCTION ConfirmMgr.UDF_WEEKDAY_COUNT_BETWEEN_2_DAYS
(
@StartDate      datetime,
@EndDate        datetime
)
RETURNS smallint
AS
BEGIN
RETURN (SELECT CASE WHEN @StartDate > @EndDate THEN -1
		ELSE (DATEDIFF(dd, @StartDate, @EndDate) + 1)
                -(DATEDIFF(wk, @StartDate, @EndDate) * 2)
                 -(CASE WHEN DATENAME(dw, @StartDate) = 'Sunday' THEN 1 ELSE 0 END)
					-(CASE WHEN DATENAME(dw, @EndDate) = 'Saturday' THEN 1 ELSE 0 END) - 1
					END)
END
GO
