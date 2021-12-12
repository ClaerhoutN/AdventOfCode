open System

let toTuple2 (tup:'a[]) = (tup.[0], tup.[1])
let isLowerAlpha (s:string) = s.ToCharArray() |> Seq.forall (fun c -> c >= 'a' && c <= 'z')

type cave = {Name: string; connectedCaves: string list}

let isSigleSmallCaveVisitingMoreThanTwice ``from`` pathsVisited caves =
    if ``from``.Name |> isLowerAlpha 
     then 
     match (pathsVisited |> List.filter (fun (c1, c2) -> c1 = ``from``) |> List.length) with
     |0 -> false
     |1 -> 
        let cavesVisitedMoreThanTwice = caves 
                                        |> List.filter (fun c -> 
                                            c.Name |> isLowerAlpha
                                            && (pathsVisited 
                                                |> List.filter (fun (c1, c2) -> c1 = c) 
                                                |> List.length >= 2))
        cavesVisitedMoreThanTwice |> List.length > 0
     |_ -> true
    else false

let rec countConnectionsPart2 caves ``from`` ``to`` pathsVisited =
    let caveCountMapping c = 
        let toCave = caves |> List.find (fun c2 -> c2.Name=c)
        let repeatingSameCaveToCave = pathsVisited |> List.exists (fun (c1, c2) -> (c1, c2) = (``from``,toCave))
        if toCave.Name="start"
         || (repeatingSameCaveToCave && not (toCave.Name |> isLowerAlpha) && not (``from``.Name |> isLowerAlpha))
         || caves |> isSigleSmallCaveVisitingMoreThanTwice ``from`` pathsVisited
         then 0
        elif toCave = ``to`` then 1
        else (countConnectionsPart2 caves toCave ``to``) ((``from``,toCave)::pathsVisited)
    let count = ``from``.connectedCaves 
                |> List.map caveCountMapping
                |> List.sum
    count

[<EntryPoint>]
let main argv =
    let input = AOC.Util.InputHelper.GetInputLines<string>("https://adventofcode.com/2021/day/12/input", lineSeparatorRegex = "\n") 
                    |> Async.AwaitTask |> Async.RunSynchronously
                    |> List.ofSeq
    //let input = ["start-A"; "start-b"; "A-c"; "A-b"; "b-d"; "A-end"; "b-end"]
    let connections = (input |> List.map (fun s -> s.Split('-') |> toTuple2))
                    |> List.append
                        (input |> List.map (fun s -> s.Split('-') |> Array.rev |> toTuple2))
    let caves = connections |> List.groupBy (fun (cave1, cave2) -> cave1)
                            |> List.map (fun (start, ccaves) -> { cave.Name=start; connectedCaves=ccaves |> List.map snd})
    let firstCave = caves |> List.find (fun cave -> cave.Name="start")
    let endCave = caves |> List.find (fun cave -> cave.Name="end")
    let count = countConnectionsPart2 caves firstCave endCave []
    printfn "%i" count
    0