module Part1
    ( doPart1
    ) where
import Data.List.Split
import Data.Char (isDigit)

tuplify3Consecutive :: [[Char]] -> [([Char], [Char], [Char])]
tuplify3Consecutive (above:line:below:remainder) = (above,line,below):tuplify3Consecutive (line:below:remainder)
tuplify3Consecutive _ = []

isAdjacentToSymbol :: [Char] -> [Char] -> [Char] -> [(Char, Bool)]
isAdjacentToSymbol (a1:a2:a3:a4) (b1:b2:b3:b4) (c1:c2:c3:c4) = (b2, any (\n -> n `notElem` ('.':['0'..'9'])) [a1, a2, a3, b1, b3, c1, c2, c3]):isAdjacentToSymbol (a2:a3:a4) (b2:b3:b4) (c2:c3:c4)
isAdjacentToSymbol ".." (b1:b2:b3:b4) (c1:c2:c3:c4) = isAdjacentToSymbol "..." (b1:b2:b3:b4) (c1:c2:c3:c4)
isAdjacentToSymbol (a1:a2:a3:a4) (b1:b2:b3:b4) ".." = isAdjacentToSymbol (a1:a2:a3:a4) (b1:b2:b3:b4) "..."
isAdjacentToSymbol _ _ _ = []

getPartNumbers :: [Char] -> [Char] -> [Char] -> [Int]
getPartNumbers line above below = 
    let _isAdjacentToSymbol = isAdjacentToSymbol (('.':above) ++ ['.']) (('.':line) ++ ['.']) (('.':below) ++ ['.'])
        (_, numbers, _) = foldl partNumbersFold ("", [], False) (_isAdjacentToSymbol ++ [('.', False)])
    in numbers

partNumbersFold :: (String, [Int], Bool) -> (Char, Bool) -> (String, [Int], Bool)
partNumbersFold (prefix, result, isPartNumber) (c, _isAdjacentToSymbol) = 
    if isDigit c then
        (if _isAdjacentToSymbol
        then (prefix ++ [c], result, True)
        else (prefix ++ [c], result, isPartNumber))
    else ("", if isPartNumber then (read prefix :: Int):result else result, False)

getPartNumberSumForLine :: ([Char], [Char], [Char]) -> Int
getPartNumberSumForLine (above, line, below) = sum (getPartNumbers line above below)

doPart1 :: IO ()
doPart1 = do
    fileContents <- readFile "C:\\Users\\Administrator\\source\\repos\\AdventOfCode\\inputFiles\\2023_3.txt"
    let lineList = splitOn "\n" fileContents
    let partNumberSum = sum $ map getPartNumberSumForLine $ tuplify3Consecutive (("":lineList) ++ [""])
    print partNumberSum
    return ()
