open System
open AOC.Util

let executeAgingCycle (src:uint64[]) (buffer:uint64[]) =
    for i in [8..-1..0] do
        if i <> 0 then
            buffer.[i-1] <- src.[i]
        else
            buffer.[6] <- buffer.[6] + src.[i]
            buffer.[8] <- src.[i]
    (buffer, src)

let rec ageFishes n fishStateCounts buffer = 
    if n = 0 then fishStateCounts 
    else 
        let newFishStateCounts, newBuffer = executeAgingCycle fishStateCounts buffer
        ageFishes (n-1) newFishStateCounts newBuffer

[<EntryPoint>]
let main argv =
    let fishes = AOC.Util.InputHelper.GetInputLines<int>("https://adventofcode.com/2021/day/6/input", lineSeparatorRegex = ",") 
                |> Async.AwaitTask |> Async.RunSynchronously
                |> List.ofSeq
    let countedFishes = List.countBy (fun f -> f) fishes
    let fishStateCounts = [for i in 0..8 -> countedFishes
                                            |> Seq.tryFind (fun (k, v) -> k = i)] 
                                            |> Seq.map (fun kv -> match kv with |Some(k, v) -> (uint64)v |None -> 0UL)
                                            |> Array.ofSeq
    let agedFishes = Array.zeroCreate<uint64> 9 |> ageFishes 256 fishStateCounts
    printfn "%i" (Array.sum agedFishes)
    0