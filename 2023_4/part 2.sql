DECLARE @Cards table([CardId] int, [Count] int, WinningNumberCount [int]);

--ensure all cards 1..201 exist in the table
INSERT INTO @Cards
SELECT value, 1, 0
FROM GENERATE_SERIES(1, 201);

--SET the WinningNumberCount
with WinningNumberCounts as (
	SELECT wn.CardId CardId, Count(1) cnt
	FROM [WinningNumbers] wn
	INNER JOIN [PlayerNumbers] pn on wn.Number = pn.Number and wn.CardId = pn.CardId
	group by wn.CardId
)
UPDATE c
SET c.WinningNumberCount = wnc.cnt
FROM @Cards c INNER JOIN WinningNumberCounts wnc ON c.CardId = wnc.CardId;

DECLARE @cardId int = 1;
WHILE(@cardId < 201)
BEGIN
	UPDATE c1
	SET c1.[Count] = c1.[Count] + c2.[Count]
	FROM @Cards c1
	INNER JOIN @Cards c2 ON c1.CardId > c2.CardId AND c1.CardId <= c2.CardId + c2.WinningNumberCount
	WHERE c2.CardId = @cardId;
	SET @cardId = @cardId + 1;
END

select SUM([Count]) from @Cards;