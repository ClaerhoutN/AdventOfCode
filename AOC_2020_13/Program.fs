open System
open AOC.Util

let getFirstReachableBus timestamp busses = 
    busses 
    |> Seq.map (fun bus -> bus, bus * int (ceil (float timestamp / float bus)))
    |> Seq.minBy (fun (bus, ts) -> ts)

let rec gcd a b = if a = 0UL then b else gcd (b%a) a
let rec power x y m = 
    if y = 0UL then 1UL
    else
        let mutable p = (power x (y / 2UL) m) % m;
        p <- (p * p) % m;
        if y % 2UL = 0UL then p else (x * p) % m;
let modInverse  a m = 
    let g = gcd a m
    let t = power a (m-2UL) m
    let b = (a * t % m = 1UL)
    t

[<EntryPoint>]
let main argv =
    let input = InputHelper.GetInputLines<string>("https://adventofcode.com/2020/day/13/input")
                        |> Async.AwaitTask |> Async.RunSynchronously
                        |> List.ofSeq

    let timestampReady = int input.[0]
    let allBusses = input.[1].Split(',') 
                    |> Seq.filter (fun bus -> bus <> "x") 
                    |> Seq.map (fun bus -> int bus)
                    |> List.ofSeq
    let (bus, timestampDeparture) = getFirstReachableBus timestampReady allBusses                            
    printfn "%i" (bus * (timestampDeparture - timestampReady))

    //ref.: https://www.geeksforgeeks.org/chinese-remainder-theorem-set-2-implementation/
    let productAllBusses = allBusses |> List.fold (fun b1 b2 -> uint64 b1 * uint64 b2) 1UL;
    let numAndRem = input.[1].Split(',') 
                    |> Seq.indexed  
                    |> Seq.filter (fun (i, b) -> b <> "x")
                    |> Seq.map (fun (i, b) -> 
                                    let b2 = int b
                                    let i2 = int i
                                    let x = (b2 - i2) % b2
                                    (uint64 (if x < 0 then b2 + x else x), uint64 b))
    let rem_pp_inv = numAndRem
                    |> Seq.map (fun (rem, num) -> 
                        let a = productAllBusses / num
                        rem, a, modInverse a num)
                    |> List.ofSeq
    let xByChineseRemainderTheorem = (rem_pp_inv |> List.fold (fun t (rem, pp, inv) -> t + rem * pp * inv) 0UL) % productAllBusses
    printfn "%i" (xByChineseRemainderTheorem)

    0