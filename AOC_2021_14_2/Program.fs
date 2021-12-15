open System
open Microsoft.FSharp.Collections
open FSharp.Collections.ParallelSeq

type CountResults = (char * uint64) list
type CacheEntry = { OuterChars: char list; InnerCounts: CountResults; Depth: int; }
type Cache = CacheEntry list

let countsFromString line = line |> List.groupBy (fun c -> c)
                                 |> List.map (fun (c, cList) -> (c, (cList |> List.fold (fun agg c -> agg + 1UL) 0UL)))
let rec mergeCharacterCounts cCount1 cCount2 = 
    cCount1@cCount2 
        |> List.groupBy fst 
        |> List.map (fun group -> 
           snd group |> List.fold (fun c1 c2 -> (fst c1, snd c1 + snd c2)) (fst group, 0UL))

let rec getOrAddFromCache insertionRules cache depth outerChars = 
    match cache |> List.tryFind (fun entry -> entry.OuterChars = outerChars && entry.Depth = depth) with
    |Some(cc) -> (cc.InnerCounts, cache)
    |_ -> match insertionRules |> List.tryFind (fun (s, c) -> s = outerChars) with
          |Some(s, c) -> match depth with
                         |1 -> let newCacheEntry = { OuterChars = outerChars; Depth = depth; InnerCounts = countsFromString [c] }
                               (newCacheEntry.InnerCounts, newCacheEntry::cache)
                         |_ -> let (cr1, cache1) = getOrAddFromCache insertionRules cache (depth-1) [outerChars.[0];c]
                               let (cr2, cache2) = getOrAddFromCache insertionRules cache1 (depth-1) [c;outerChars.[1]]
                               let newCacheEntry = { OuterChars = outerChars; Depth = depth; 
                                                     InnerCounts = (countsFromString [c] |> mergeCharacterCounts <| cr1) |> mergeCharacterCounts cr2 }
                               (newCacheEntry.InnerCounts, newCacheEntry::cache2)
          |None -> let newCacheEntry = { OuterChars = outerChars; Depth = depth; InnerCounts = []; }
                   (newCacheEntry.InnerCounts, newCacheEntry::cache)
          

let rec applyPairInsertion insertionRules count depth cache line = 
    let recalculatedCache = line 
                            |> List.windowed 2
                            |> List.mapFold (fun _cache outerChars -> getOrAddFromCache insertionRules _cache depth outerChars) cache
    if count <= 1 
    then 
        fst recalculatedCache 
        |> List.fold (fun l1 l2 -> mergeCharacterCounts l1 l2) [] 
        |> mergeCharacterCounts (countsFromString line)
    else 
        applyPairInsertion insertionRules (count-1) (depth+1) (snd recalculatedCache) line

[<EntryPoint>]
let main argv =
    let input = AOC.Util.InputHelper.GetInputLines<string>("https://adventofcode.com/2021/day/14/input", lineSeparatorRegex = "\n") 
                    |> Async.AwaitTask |> Async.RunSynchronously
                    |> List.ofSeq
    let line = input.[0];
    let insertionRules = input.[1..] |> List.map (fun rule -> (rule.[0..1] |> List.ofSeq, rule.[6]))

    let count = 40;
    let counts = line |> List.ofSeq |> applyPairInsertion insertionRules count 1 []

    let leastOccurring = counts |> Seq.minBy snd
    let mostOccurring = counts |> Seq.maxBy snd
    printfn "%i" (snd mostOccurring - snd leastOccurring)
    0