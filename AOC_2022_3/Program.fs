open System

[<EntryPoint>]
let main argv =
    let input = AOC.Util.InputHelper.GetInputLines<string>("https://adventofcode.com/2022/day/3/input", lineSeparatorRegex = "\n") |> Async.AwaitTask |> Async.RunSynchronously
    let priority item = int item - (if item >= 'a' then 96 else 38)
    let compartmentIntersectionPrioritySum = input |> Seq.sumBy (fun s -> Seq.splitInto 2 s |> Seq.map Set.ofSeq |> Set.intersectMany |> Seq.head |> priority)
    let badgePrioritySum = Seq.sumBy priority (seq { for i in 0..3..input.Count-3 -> Set.intersectMany [for i2 in 0..2 -> Set.ofSeq input.[i+i2]] |> Seq.head })
                
    printfn "%i %i" compartmentIntersectionPrioritySum badgePrioritySum
    0