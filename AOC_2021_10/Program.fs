open System                           

let characters = [('(',')', 3); ('[',']', 573); ('{','}', 11973); ('<','>', 251373)]
let getAutocompletionScore charStack:uint64 = 
    charStack |> List.fold (fun score c -> 
        score * 5UL  + uint64 (characters |> List.findIndex (fun (o, _, _) -> o = c)) + 1UL) 0UL
let rec getScores line charStack:int*uint64 =
    match line with
    |[] -> (0, getAutocompletionScore charStack)
    |character::tail ->
          if characters |> List.exists (fun (o, _, _) -> character = o) then getScores tail (character::charStack)
          elif characters |> List.exists (fun (o, c, _) -> (o, c) = (charStack.[0], character)) then getScores tail (charStack.[1..])
          else (characters |> List.fold (fun fs (_, c, s) -> if character = c then s else fs) 0), 0UL
        

[<EntryPoint>]
let main argv = 
    let inputLines = AOC.Util.InputHelper.GetInputLines<string>("https://adventofcode.com/2021/day/10/input", lineSeparatorRegex = "\n") 
                    |> Async.AwaitTask |> Async.RunSynchronously
                    |> List.ofSeq
    let scores = inputLines |> List.map (fun line -> getScores (line |> Seq.toList) [])
    let errorScore = scores |> List.sumBy(fst)
    let completionScores = scores |> List.filter (fun (s1, s2) -> s2 <> 0UL) |> List.map (snd) |> List.sort
    printfn "syntax error score: %i" errorScore
    printfn "median auto completion score: %i" completionScores.[completionScores.Length / 2]
    0