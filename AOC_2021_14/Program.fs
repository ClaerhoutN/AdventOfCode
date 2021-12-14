open System

let rec applyPairInsertion insertionRules count line = 
    if count = 0 then line 
    else
        let mapped =  line 
                      |> Seq.windowed 2 
                      |> Seq.map (fun substr -> 
                            match (insertionRules |> List.tryFind (fun r -> fst r = (substr |> System.String))) with
                            |Some(x) -> [|substr.[0];(snd x); substr.[1]|]
                            |None -> substr )
                      |> List.ofSeq
        mapped |> List.fold (fun s1 s2 -> s1 + (s2.[1..] |> System.String)) (mapped.[0].[0] |> string)
               |> applyPairInsertion insertionRules (count-1)

[<EntryPoint>]
let main argv =
    let input = AOC.Util.InputHelper.GetInputLines<string>("https://adventofcode.com/2021/day/14/input", lineSeparatorRegex = "\n") 
                    |> Async.AwaitTask |> Async.RunSynchronously
                    |> List.ofSeq
    let line = input.[0];
    let insertionRules = input.[1..] |> List.map (fun rule -> (rule.[0..1], rule.[6]))
    let mappedLine = line |> applyPairInsertion insertionRules 20
    let groupedCounts = mappedLine |> Seq.countBy(fun t -> t)
    let leastOccurring = groupedCounts |> Seq.minBy snd
    let mostOccurring = groupedCounts |> Seq.maxBy snd
    printfn "%i" (snd mostOccurring - snd leastOccurring)
    0