open System
open AOC.Util

let getFirstReachableBus timestamp busses = 
    busses 
    |> Seq.map (fun bus -> bus, bus * int (ceil (float timestamp / float bus)))
    |> Seq.minBy (fun (bus, ts) -> ts)

[<EntryPoint>]
let main argv =
    let input = InputHelper.GetInputLines<string>("https://adventofcode.com/2020/day/13/input")
                        |> Async.AwaitTask |> Async.RunSynchronously
                        |> List.ofSeq
    let timestampReady = int input.[0]
    let allBusses = input.[1].Split(',') 
                    |> Seq.filter (fun bus -> bus <> "x") 
                    |> Seq.map (fun bus -> int bus)
                    |> List.ofSeq
    let (bus, timestampDeparture) = getFirstReachableBus timestampReady allBusses                            
    printfn "%i" (bus * (timestampDeparture - timestampReady))
    0