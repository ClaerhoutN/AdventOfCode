open System

let priority item = int item - (if item >= 'a' then 96 else 38)

[<EntryPoint>]
let main argv =
    let input = AOC.Util.InputHelper.GetInputLines<string>("https://adventofcode.com/2022/day/3/input", lineSeparatorRegex = "\n") |> Async.AwaitTask |> Async.RunSynchronously
    let compartmentIntersectionPrioritySum = input |> Seq.map (fun s -> [s.[..s.Length/2-1]|> Set.ofSeq; s.[s.Length/2..]|> Set.ofSeq] |> Set.intersectMany |> Seq.head)
                                                   |> Seq.sumBy priority
    let badgePrioritySum = seq { for i in 0..3..input.Count-3 -> Set.intersectMany [Set.ofSeq input.[i]; Set.ofSeq input.[i+1]; Set.ofSeq input.[i+2]] |> Seq.head }
                           |> Seq.sumBy priority
                
    printfn "%i %i" compartmentIntersectionPrioritySum badgePrioritySum
    0 // return an integer exit code