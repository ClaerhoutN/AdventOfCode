open AOC.Util
open System

[<Struct>] 
type BoatState = { X: int; Y: int; Direction: char }

let moveInRange _from _to i = if i < _from then _to - (_from - i - 1) elif i > _to then _from + i - _to - 1 else i
let rec handleInstruction (instruction:string) (boatState:BoatState):BoatState = 
    let amount = instruction.[1..] |> int
    let switchDirection direction leftright amount = 
        let directions = ['N';'E';'S';'W']
        directions.[(List.findIndex (fun d -> d = direction) directions 
                            + (if leftright = 'L' then -amount/90 else amount/90))
                            |> moveInRange 0 3]
    match instruction.[0] with
    |'N' -> { boatState with Y = (boatState.Y+amount) }
    |'S' -> { boatState with Y = (boatState.Y-amount) }
    |'E' -> { boatState with X = (boatState.X+amount) }
    |'W' -> { boatState with X = (boatState.X-amount) }
    |'R'|'L' -> { boatState with Direction = switchDirection boatState.Direction instruction.[0] amount }
    |'F' -> handleInstruction ((boatState.Direction |> string) + instruction.[1..]) boatState
    |_ -> invalidArg (nameof instruction) (sprintf "Value passed in was %s." instruction)

[<EntryPoint>]
let main argv =
    let directions = InputHelper.GetInputLines<string>("https://adventofcode.com/2020/day/12/input")
                    |> Async.AwaitTask |> Async.RunSynchronously
                    |> List.ofSeq
    let boatState = directions |> List.fold (fun bs ins -> handleInstruction ins bs) { BoatState.X = 0; Y = 0; Direction = 'E' }
    printfn "%i" (abs boatState.X + abs boatState.Y)
    0