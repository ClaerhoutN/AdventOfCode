open System
let rowCount = 10
let colCount = 10
let maxEnergy = 9
let rec flashIfNeeded (grid:int[][]) (flashedIndices:byref<List<(int*int)>>) row col = 
    match (grid.[row].[col] > maxEnergy && not (flashedIndices |> List.exists (fun (r, c) -> (r, c) = (row, col)))) with
    |false -> ()
    |true ->
        flashedIndices <- (row, col)::flashedIndices
        let indices = [for r in -1..1 -> [for c in -1..1 -> (row + r,col + c)]] |> List.collect (fun t -> t)
        for (nrow, ncol) in indices do
            if nrow >= 0 && nrow < rowCount 
                && ncol >= 0 && ncol < colCount 
                && (nrow, ncol) <> (row, col) then 
                    grid.[nrow].[ncol] <- grid.[nrow].[ncol] + 1
                    flashIfNeeded grid &flashedIndices nrow ncol

let startFlashing (grid:int[][]) = 
    let mutable flashedIndices = []
    for row in 0..rowCount-1 do 
        for col in 0..colCount-1 do
            flashIfNeeded grid &flashedIndices row col
    for row in 0..rowCount-1 do
        for col in 0..colCount-1 do
            if grid.[row].[col] > maxEnergy then 
                grid.[row].[col] <- 0
    flashedIndices.Length

let rec executeEnergyRound (grid:int[][]) count = 
    if count = 0 
        then 0
    else
        for row in 0..rowCount-1 do
            for col in 0..colCount-1 do
                grid.[row].[col] <- grid.[row].[col] + 1
        startFlashing grid + executeEnergyRound grid (count - 1)

let rec firstSimulFlash (grid:int[][]) round = 
    for row in 0..rowCount-1 do
        for col in 0..colCount-1 do
            grid.[row].[col] <- grid.[row].[col] + 1
    if startFlashing grid = (colCount*rowCount) then 
        round 
    else firstSimulFlash grid (round+1)


[<EntryPoint>]
let main argv =
    let inputLines = AOC.Util.InputHelper.GetInputLines<string>("https://adventofcode.com/2021/day/11/input", lineSeparatorRegex = "\n") 
                    |> Async.AwaitTask |> Async.RunSynchronously
                    |> List.ofSeq
    let inputGrid = [|for i in 0..rowCount-1 -> [|for j in 0..colCount-1 -> (string >> int) inputLines.[i].[j]|]|]
    //let flashes = executeEnergyRound inputGrid 100
    let simulFlashRound = firstSimulFlash inputGrid 1
    //printfn "%i" flashes
    printfn "%i" simulFlashRound
    0