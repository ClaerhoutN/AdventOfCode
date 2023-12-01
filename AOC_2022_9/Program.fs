open System

let rec addPositionVisited (oldX, oldY) (newX, newY) positionsVisited = 
    let xDir = if newX > oldX then 1 else if newX = oldX then 0 else -1;
    let yDir = if newY > oldY then 1 else if newY = oldY then 0 else -1;
    let (x, y) = (oldX+xDir, oldY+yDir)
    let tVisited = if(List.contains (x, y) positionsVisited) then positionsVisited else (x, y)::positionsVisited
    if (x, y) = (newX, newY) then tVisited else addPositionVisited (x, y) (newX, newY) tVisited
let getHeadPosition (x, y) step = match step with 
                                  |"L",l -> (x-l), y
                                  |"R",r -> (x+r), y
                                  |"U",u -> x, (y-u)
                                  |"D",d -> x, (y+d)
                                  |_ -> failwith ""
let getTailPosition (xHead, yHead) (xTail, yTail) = 
    let xDir = if xHead > xTail then 1 else -1;
    let yDir = if yHead > yTail then 1 else -1;
    match (abs (xHead-xTail), abs (yHead-yTail)) with
    |(0,0)|(1,0)|(0,1)|(1,1) -> (xTail, yTail)
    |(1,_)|(0,_) -> (xHead, yHead-1*yDir)
    |(_,1)|(_,0) -> (xHead-1*xDir, yHead)
    |_ -> (xHead-1*xDir, yHead-1*yDir)


let rec getPositionsVisited steps positionHead positionTail positionsVisited =
    match steps with
    |step::tail -> let posHead = getHeadPosition positionHead step
                   let posTail = getTailPosition posHead positionTail
                   getPositionsVisited tail posHead posTail (addPositionVisited positionTail posTail positionsVisited)
    |[] -> positionsVisited

[<EntryPoint>]
let main argv =
    let steps = AOC.Util.InputHelper.GetInputLines<(string*string)>("https://adventofcode.com/2022/day/9/input") |> Async.AwaitTask |> Async.RunSynchronously
                |> Seq.map (fun (a, b) -> (a, int b)) |> List.ofSeq
    let positionsVisited = getPositionsVisited steps (0,0) (0,0) [(0,0)]
    printfn "%i" positionsVisited.Length
    0