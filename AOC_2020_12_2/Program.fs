open AOC.Util
open System

[<Struct>] 
type Waypoint = { X: int; Y: int; }
type Boat = { X: int; Y: int; Waypoint: Waypoint}

let handleInstruction (instruction:string) (boat:Boat):Boat = 
    let amount = instruction.[1..] |> int
    let rotateWaypoint (waypoint:Waypoint) (boat:Boat) leftright amount =
        let (xDiff, yDiff) = (waypoint.X - boat.X, waypoint.Y - boat.Y)
        match (leftright, amount) with
        |('L',90)|('R',270) -> { waypoint with X = boat.X-yDiff; Y = boat.Y+xDiff }
        |('R',90)|('L',270) -> { waypoint with X = boat.X+yDiff; Y = boat.Y-xDiff }
        |('R',180)|('L',180) -> { waypoint with X = boat.X-xDiff; Y = boat.Y-yDiff }
        |_ -> failwithf "Value passed in was %c %i." leftright amount
    match instruction.[0] with
    |'N' -> { boat with Waypoint = { boat.Waypoint with Y = (boat.Waypoint.Y + amount)} }
    |'S' -> { boat with Waypoint = { boat.Waypoint with Y = (boat.Waypoint.Y - amount)} }
    |'E' -> { boat with Waypoint = { boat.Waypoint with X = (boat.Waypoint.X + amount)} }
    |'W' -> { boat with Waypoint = { boat.Waypoint with X = (boat.Waypoint.X - amount)} }
    |'R'|'L' -> { boat with Waypoint = rotateWaypoint boat.Waypoint boat instruction.[0] amount }
    |'F' -> 
            let (xDiff, yDiff) = (boat.Waypoint.X - boat.X, boat.Waypoint.Y - boat.Y)
            let (newBoatX, newBoatY) = (boat.X + xDiff*amount, boat.Y + yDiff*amount)
            { X = newBoatX; Y = newBoatY; Waypoint = { X = newBoatX + xDiff; Y = newBoatY + yDiff } }
    |_ -> invalidArg (nameof instruction) (sprintf "Value passed in was %s." instruction)

[<EntryPoint>]
let main argv =
    let directions = InputHelper.GetInputLines<string>("https://adventofcode.com/2020/day/12/input")
                    |> Async.AwaitTask |> Async.RunSynchronously
                    |> List.ofSeq
    let boat = directions |> List.fold (fun bs ins -> handleInstruction ins bs) { X = 0; Y = 0; Waypoint = { X = 10; Y = 1 }; }
    printfn "%i" (abs boat.X + abs boat.Y)
    0