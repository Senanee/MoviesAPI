SELECT 
  m.Genre AS genre, Split.value as splitValue
FROM [dbo].[Movies] AS m
CROSS APPLY STRING_SPLIT(m.Genre, ',') as Split

SELECT DISTINCT TRIM(Split.value) AS splitValue FROM [dbo].[Movies] AS m CROSS APPLY STRING_SPLIT(m.Genre, ',') AS Split

--drop view AvailableGenre

