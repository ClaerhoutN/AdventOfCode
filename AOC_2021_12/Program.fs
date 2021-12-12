open System

let toTuple2 (tup:'a[]) = (tup.[0], tup.[1])
let isLowerAlpha (s:string) = s.ToCharArray() |> Seq.forall (fun c -> c >= 'a' && c <= 'z')

type cave = {Name: string; connectedCaves: string list}

let rec countConnections caves ``from`` ``to`` pathsVisited=
    let count = ``from``.connectedCaves 
                |> List.map (fun c -> 
                                let fromCave = caves |> List.find (fun c2 -> c2.Name=c)
                                if fromCave.Name="start"
                                   || (pathsVisited |> List.exists (fun (c1, c2) -> (c1, c2) = (``from``,fromCave)))
                                   || (``from``.Name |> isLowerAlpha && pathsVisited |> List.exists (fun (c1, c2) -> c1 = ``from``))
                                then 0
                                elif fromCave = ``to`` then 1
                                else (countConnections caves fromCave ``to``) ((``from``,fromCave)::pathsVisited) )
                |> List.sum
    count


[<EntryPoint>]
let main argv =
    let input = AOC.Util.InputHelper.GetInputLines<string>("https://adventofcode.com/2021/day/12/input", lineSeparatorRegex = "\n") 
                    |> Async.AwaitTask |> Async.RunSynchronously
                    |> List.ofSeq
    let connections = (input |> List.map (fun s -> s.Split('-') |> toTuple2))
                    |> List.append
                        (input |> List.map (fun s -> s.Split('-') |> Array.rev |> toTuple2))
    let caves = connections |> List.groupBy (fun (cave1, cave2) -> cave1)
                            |> List.map (fun (start, ccaves) -> { cave.Name=start; connectedCaves=ccaves |> List.map snd})
    let firstCave = caves |> List.find (fun cave -> cave.Name="start")
    let endCave = caves |> List.find (fun cave -> cave.Name="end")
    let count = countConnections caves firstCave endCave []
    printfn "%i" count
    0