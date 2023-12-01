open System
let addIfMinus1 inc v = if v = -1 then v + inc else v;
let digits = ["one"; "two"; "three"; "four"; "five"; "six"; "seven"; "eight"; "nine";]
let firstDigit (line:string) = 
                            digits 
                            |> List.indexed 
                            |> List.minBy (fun (i, numberName) -> 
                                let i1 = line.IndexOf(numberName)
                                let i2 = line.IndexOf(string (i+1))
                                if i1 = -1 && i2 = -1 then 1000 
                                else if i1 = -1 then i2 
                                else if i2 = -1 then i1 
                                else Math.Min(i1, i2)) 
                            |> fst 
                            |> (+) 1
let lastDigit (line:string) = 
                            digits 
                            |> List.indexed 
                            |> List.maxBy (fun (i, numberName) -> 
                                Math.Max(line.LastIndexOf(numberName), line.LastIndexOf(string (i+1)))) 
                            |> fst  
                            |> (+) 1


[<EntryPoint>]
let main argv =
    let input = AOC.Util.InputHelper.GetInputLines<char[]>("https://adventofcode.com/2023/day/1/input", lineSeparatorRegex = "\n", argumentSeparatorRegex = "")
                |> Async.AwaitTask |> Async.RunSynchronously
    let digitPairSeq = input 
                        |> Seq.map(fun line -> 
                                        (int (Array.find Char.IsDigit line) - 0x30)
                                        ,(int (Array.findBack Char.IsDigit line) - 0x30))
    let sum = digitPairSeq |> Seq.sumBy(fun (c1, c2) -> c1 * 10 + c2)
    printfn "%i" sum

    let digitPairSeq2 = input 
                        |> Seq.map(fun line ->
                                        let strLine = System.String line
                                        let i1 = (firstDigit strLine)
                                        let i2 = (lastDigit strLine)
                                        (i1, i2))
    let sum2 = digitPairSeq2 |> Seq.sumBy(fun (c1, c2) -> c1 * 10 + c2)
    printfn "%i" sum2
    0