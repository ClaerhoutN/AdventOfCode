open System

let rec resolveInputSignal (signal:string) (allInputSignals:List<string>) norec = 
    let resolveSignal num = allInputSignals |> Seq.find (fun s -> resolveInputSignal s allInputSignals true = num)
    let signalSeq = signal |> Set.ofSeq
    match signal.Length with
    |2 -> 1|4 -> 4|3 -> 7|7 -> 8
    |5 ->
        if norec then -1
        else
        let resolvedSignal4 = resolveSignal 4 |> Set.ofSeq
        let unifiedWith4 = Set.union signalSeq resolvedSignal4
        let resolvedSignal1 = resolveSignal 1 |> Set.ofSeq
        let unifiedWith1 = Set.union signalSeq resolvedSignal1
        match unifiedWith4.Count with
        |7 -> 2
        |6|5 -> match unifiedWith1.Count with
                |5 -> 3
                |_ -> 5
        |_ -> failwith ""
    |6 -> 
        if norec then -1
        else
        let missingCharacter = ['a'..'g'] |> Seq.except signalSeq |> Seq.exactlyOne
        if ((resolveSignal 1).Contains(missingCharacter)) then 6 
        elif ((resolveSignal 4).Contains(missingCharacter)) then 0 
        else 9
    |_ -> -1
                                    
let getOutputNumberFromOutputSignal (outputSignal:string) (resolvedInputSignals:List<string>) =  
    let numIndex = resolvedInputSignals 
                    |> List.findIndex 
                    (fun ris -> (Seq.forall (fun (rs:char) -> (outputSignal.Contains(rs))) ris) 
                                && (Seq.forall (fun (os:char) -> (ris.Contains(os))) outputSignal))
    numIndex + int '0' |> char

let decodeNumber inputSignals outputSignals = 
    let resolvedInputSignals = inputSignals |> List.sortBy (fun signal -> resolveInputSignal signal inputSignals false)
    outputSignals 
    |> List.map (fun outputSignal -> getOutputNumberFromOutputSignal outputSignal resolvedInputSignals) 
    |> Array.ofList

[<EntryPoint>]
let main argv = 
    let inputData = AOC.Util.InputHelper.GetInputLines<string>("https://adventofcode.com/2021/day/8/input", lineSeparatorRegex = "\n") 
                    |> Async.AwaitTask |> Async.RunSynchronously
                    |> List.ofSeq
    let count1478 = inputData 
                    |> List.map (fun s -> s.[(s.IndexOf('|')+2)..].Split(' ') |> List.ofArray |> List.map (fun s2 -> s2.Length))
                    |> List.collect (fun sl -> sl)
                    |> List.filter (fun sl -> match sl with 
                                              |2|3|4|7 -> true
                                              |_ -> false)
                    |> List.length
    printfn "%i" count1478
    let decodedNumbers = inputData |> List.map (fun line -> 
                                        let splittedLine = line.Split(" | ")
                                        decodeNumber (splittedLine.[0].Split(' ') |> List.ofArray) (splittedLine.[1].Split(' ') |> List.ofArray)
                                        |> System.String |> int)
    printfn "%i" (List.sum decodedNumbers)
    0