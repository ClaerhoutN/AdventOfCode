module Part2
    ( doPart2
    ) where
import Data.List.Split
import Data.Char (isDigit)
import Data.List (nubBy)

data LinePosition = Top | Middle | Bottom deriving Eq
type GearPosition = (Int, LinePosition)
type GearRatioCandidate = (Int, GearPosition) -- gear ratio, gear position
type NumberPosition = (Int, GearPosition) -- number, index of asterisk, current line or bottom line

tuplify3Consecutive :: [[Char]] -> [([Char], [Char], [Char])]
tuplify3Consecutive (above:line:below:remainder) = (above,line,below):tuplify3Consecutive (line:below:remainder)
tuplify3Consecutive _ = []

getGearPositions :: Char -> Char -> Char -> Char -> Char -> Char -> Char -> Char -> [(Char, GearPosition)]
getGearPositions topLeft topMiddle topRight middleLeft middleRight bottomLeft bottomMiddle bottomRight =
    [(topLeft, (-1, Top)),
    (topMiddle, (0, Top)),
    (topRight, (1, Top)),
    (middleLeft, (-1, Middle)),
    (middleRight, (1, Middle)),
    (bottomLeft, (-1, Bottom)),
    (bottomMiddle, (0, Bottom)),
    (bottomRight, (1, Bottom))]

getAdjacentAsteriskPositions :: [Char] -> [Char] -> [Char] -> [(Char, [GearPosition])] --todo: make isAdjacentToAsterisk
getAdjacentAsteriskPositions (a1:a2:a3:a4) (b1:b2:b3:b4) (c1:c2:c3:c4) =
    let gearPositions = map snd $ filter (\(c, _) -> c == '*') (getGearPositions a1 a2 a3 b1 b3 c1 c2 c3)
    in (b2, gearPositions):getAdjacentAsteriskPositions (a2:a3:a4) (b2:b3:b4) (c2:c3:c4)
getAdjacentAsteriskPositions ".." (b1:b2:b3:b4) (c1:c2:c3:c4) = getAdjacentAsteriskPositions "..." (b1:b2:b3:b4) (c1:c2:c3:c4)
getAdjacentAsteriskPositions (a1:a2:a3:a4) (b1:b2:b3:b4) ".." = getAdjacentAsteriskPositions (a1:a2:a3:a4) (b1:b2:b3:b4) "..."
getAdjacentAsteriskPositions _ _ _ = []

getNumberPositions :: [Char] -> [Char] -> [Char] -> [NumberPosition]
getNumberPositions above line below =
    let adjacentAsteriskPositions = getAdjacentAsteriskPositions (('.':above) ++ ['.']) (('.':line) ++ ['.']) (('.':below) ++ ['.'])
        (_, _, numberPositions, _) = foldl numberPositionFold (0, "", [], []) (adjacentAsteriskPositions ++ [('.', [])])
    in numberPositions

numberPositionFold :: (Int, String, [NumberPosition], [GearPosition]) -> (Char, [GearPosition]) -> (Int, String, [NumberPosition], [GearPosition])
numberPositionFold (index, prefix, result, allAsteriskPositions) (c, adjacentAsteriskPositions) =
    if isDigit c then (index+1, prefix ++ [c], result, allAsteriskPositions ++ map (\(i, lp) -> (i+index, lp)) adjacentAsteriskPositions)
    else
        let number = read prefix :: Int
        in (index+1, "", map (\gp -> (number, gp)) (nubBy (==) allAsteriskPositions) ++ result, [])

--exclude GRC's and NP that overlap
filterGearRatioCandidates :: [GearRatioCandidate] -> [NumberPosition] -> ([GearRatioCandidate], [NumberPosition])
filterGearRatioCandidates grc ((vNp, (iNp, lpNp)):np) =
    let filteredGearRatioCandidates = filter (\(_, (i, _)) -> lpNp == Bottom || i /= iNp) grc -- bottom NP and middle GRC can never overlap
        (_grc, _np) = filterGearRatioCandidates filteredGearRatioCandidates np
    in (_grc, if length filteredGearRatioCandidates == length grc then (vNp, (iNp, lpNp)):_np else _np)
filterGearRatioCandidates [] np = ([], np)
filterGearRatioCandidates grc [] = (grc, [])

--previous number positions (prev. line), current number positions -> returns new GRC
getNewGearRatioCandidates :: [NumberPosition] -> [NumberPosition] -> ([GearRatioCandidate], [NumberPosition])
getNewGearRatioCandidates ((lastvNp, (lastiNp, _)):lastNumberPositions) numberPositions =
    let potentialGearRatioCandidates = filter (\(_, (iNp, lpNp)) -> lpNp /= Bottom && iNp == lastiNp) numberPositions
        newNumberPositions =
            if length potentialGearRatioCandidates > 1
            then filter (\(_, (iNp, _)) -> iNp /= lastiNp) numberPositions
            else numberPositions
        (remainderGrc, remainderNp) = getNewGearRatioCandidates lastNumberPositions newNumberPositions
    in case potentialGearRatioCandidates of
        [(vNp, (iNp, lpNp))] -> ((vNp*lastvNp, (iNp, lpNp)):remainderGrc, remainderNp)
        _ -> (remainderGrc, remainderNp)
getNewGearRatioCandidates [] np = ([], np)

getGearRatioCandidateByNumberPositionsOverlap :: [NumberPosition] -> GearRatioCandidate
getGearRatioCandidateByNumberPositionsOverlap [(vA, (iA, lpA)), (vB, (_, _))] = (vA*vB, (iA, lpA))
getGearRatioCandidateByNumberPositionsOverlap _ = (0,(0, Bottom))
--current number positions (practional), current number positions -> returns new GRC
getNewGearRatioCandidatesFromLine :: [NumberPosition] -> [NumberPosition] -> [GearRatioCandidate]
getNewGearRatioCandidatesFromLine [] _ = []
getNewGearRatioCandidatesFromLine _ [] = []
getNewGearRatioCandidatesFromLine ((_, (iNp, lpNp)):numberPositionsFraction) numberPositions =
    let potentialGearRatioCandidates = filter (\(_, (i, lp)) -> i == iNp && lp == lpNp) numberPositions
    in (if length potentialGearRatioCandidates == 2 then
        getGearRatioCandidateByNumberPositionsOverlap potentialGearRatioCandidates:getNewGearRatioCandidatesFromLine numberPositionsFraction numberPositions
        else getNewGearRatioCandidatesFromLine numberPositionsFraction numberPositions)

-- each lastNumberPosition is coupled with an Int that indcates how many lines ago it was passed
getGearRatios :: ([Int], [GearRatioCandidate], [(NumberPosition, Int)]) -> ([Char], [Char], [Char]) -> ([Int], [GearRatioCandidate], [(NumberPosition, Int)])
getGearRatios (gearRatios, gearRatioCandidates, lastNumberPositions) (above, line, below) =
    let numberPositions = getNumberPositions above line below
        (fiteredGearRatioCandidates, filteredNumberPositions) = filterGearRatioCandidates gearRatioCandidates numberPositions
        (newGearRatioCandidates, filteredNumberPositions2) = getNewGearRatioCandidates (map fst lastNumberPositions) filteredNumberPositions
        newGearRatioCandidates2 = newGearRatioCandidates ++ nubBy (==) (getNewGearRatioCandidatesFromLine filteredNumberPositions2 filteredNumberPositions2) -- add GRC from within current line with getNumberPositionOverlapWithNumberPositions
        --move TOP newGearRatioCandidates2 to fiteredGearRatioCandidates
        (newGearRatioCandidates3, fiteredGearRatioCandidates2) = foldl (\(ngrc, fgrc) (v, (i, lp)) -> 
                                                                        if lp == Top then (ngrc, (v, (i, lp)):fgrc) 
                                                                        else ((v, (i, lp)):ngrc, fgrc)) 
                                                                        ([], fiteredGearRatioCandidates) newGearRatioCandidates2
    in (gearRatios ++ map fst fiteredGearRatioCandidates2,
        newGearRatioCandidates3,
        map (\fnp -> (fnp, 1)) (filter (\(_, (_, lp)) -> lp == Middle || lp == Bottom) filteredNumberPositions2)
        ++ map (\(fnp, i) -> (fnp, i+1)) (filter (\((_, (_, lp)), i) -> i < 2 && lp == Bottom) lastNumberPositions)
        )

doPart2 :: IO ()
doPart2 = do
    fileContents <- readFile "C:\\Users\\Administrator\\source\\repos\\AdventOfCode\\inputFiles\\2023_3.txt"
    let lineList = splitOn "\n" fileContents
        (gearRatios, _, _) = foldl getGearRatios ([], [], []) (tuplify3Consecutive (("":lineList) ++ [""]))
    let gearRatioSum = sum gearRatios
    print gearRatioSum
    return ()
