SELECT SUM(CardPoints) FROM
(
SELECT POWER(2, Count(*) - 1) CardPoints
  FROM [WinningNumbers] wn
  inner join [PlayerNumbers] pn on wn.Number = pn.Number and wn.CardId = pn.CardId
  group by wn.CardId
) _;