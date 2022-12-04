open System

let isRangeOverlappingFully (a1, a2) (b1, b2)  = (a1 >= b1 && a2 <= b2) || (b1 >= a1 && b2 <= a2)
let isRangeOverlappingPartially (a1, a2) (b1, b2) = (a2 >= b1 && a2 <= b2) || (a1 >= b1 && a1 <= b2) || (b2 >= a1 && b2 <= a2) || (b1 >= a1 && b1 <= a2)

[<EntryPoint>]
let main argv =
    let input = AOC.Util.InputHelper.GetInputLines<string[]>("https://adventofcode.com/2022/day/4/input", lineSeparatorRegex = "\n", argumentSeparatorRegex = ",") |> Async.AwaitTask |> Async.RunSynchronously
                |> Seq.map (fun sArr -> let rangeArr = sArr |> Array.map (fun range -> let sections = range.Split '-'
                                                                                       (int sections.[0], int sections.[1]))
                                        (rangeArr.[0], rangeArr.[1]))
                |> Seq.toList
    let fullyOverlappingRanges = input |> List.where ((<||)isRangeOverlappingFully)
    let partiallyOverlappingRanges = input |> List.where ((<||)isRangeOverlappingPartially)
    printfn "%i %i" fullyOverlappingRanges.Length partiallyOverlappingRanges.Length
    0