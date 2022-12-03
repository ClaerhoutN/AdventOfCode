open System

let calculateScorePart1 p1 p2 = p2 + 1 + (match (p1, p2) with
                                            |(0,0)|(1,1)|(2,2) -> 3
                                            |(0,1)|(1,2)|(2,0) -> 6
                                            |(1,0)|(2,1)|(0,2) -> 0
                                            |(_,_) -> failwith "")
let calculateScorePart2 p1 lossWin = lossWin*3 + match (lossWin, p1) with
                                                    |(0,0)|(1,2)|(2,1) -> 3
                                                    |(0,2)|(1,1)|(2,0) -> 2
                                                    |(0,1)|(1,0)|(2,2) -> 1
                                                    |(_,_) -> failwith ""

[<EntryPoint>]
let main argv =
    let input = AOC.Util.InputHelper.GetInputLines<(char*char)>("https://adventofcode.com/2022/day/2/input", lineSeparatorRegex = "\n", argumentSeparatorRegex = " ")
                |> Async.AwaitTask |> Async.RunSynchronously
                |> Seq.map (fun (p1, p2) -> (-) <| int p1 <| int 'A', (-) <| int p2 <| int 'X')
                |> List.ofSeq
    let score1 = input |> List.sumBy((<||) calculateScorePart1)
    let score2 = input |> List.sumBy((<||) calculateScorePart2)
    printfn "%i %i" score1 score2
    0