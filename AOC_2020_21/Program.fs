open System
open AOC.Util
open System.Text.RegularExpressions

type Allergen = string
type Ingredient = {Name:string;mutable Allergen:Allergen option}

let isAllergenUnresolved allergen ingredients = not (ingredients |> Array.exists (fun i -> match i.Allergen with |None -> false |Some(x) -> x = allergen))
let getUnresolvedAllergens ingredients allergens = allergens |> List.filter (fun a -> isAllergenUnresolved a ingredients )
let hasUnresolvedAllergens ingredients allergens = (getUnresolvedAllergens ingredients allergens).Length > 1
let getUnresolvedIngredients ingredients = ingredients |> Array.filter (fun ingredient -> ingredient.Allergen.IsNone)
let rec setAllergenResolved allergen ingredientName foods =
    match foods with
    |(ingredients, allergens)::foodTail -> for ingredient in ingredients do
                                                match ingredient.Name = ingredientName with
                                                |true -> (ingredient.Allergen <- Some(allergen))
                                                |false -> ()
                                           setAllergenResolved allergen ingredientName foodTail
    |_ -> ()
let resolveAllergen allergen ingredients foods =
    match isAllergenUnresolved allergen ingredients with
    |true -> let unresolvedIngredients = getUnresolvedIngredients ingredients
             match unresolvedIngredients with
             |[|ingredient|] -> setAllergenResolved allergen ingredient.Name foods
             |[||] -> ()
             |_ -> let foodsWithAllergen = foods |> Seq.filter (fun (_, allergens) -> allergens |> List.contains allergen)
                   let foodsWithAllergen_WithoutResolvedIngredients = foodsWithAllergen |> Seq.map (fun (i, a) -> getUnresolvedIngredients i)
                   let commonUnresolvedIngredients = foodsWithAllergen_WithoutResolvedIngredients |> Seq.map (fun i -> Set.ofSeq i) |> Set.intersectMany |> List.ofSeq
                   match commonUnresolvedIngredients with
                   |[ingredient] -> setAllergenResolved allergen ingredient.Name foods
                   |_ -> ()
    |false -> ()
let resolveAllergens foods:(Ingredient[] * Allergen list) list =
    while foods |> List.exists ((<||) hasUnresolvedAllergens) do
        for (ingredients, allergens) in foods do
            for allergen in allergens do
                resolveAllergen allergen ingredients foods        
    foods

[<EntryPoint>]
let main argv =
    let inputLines = InputHelper.GetInputLines<string>("https://adventofcode.com/2020/day/21/input", lineSeparatorRegex="\n") |> Async.AwaitTask |> Async.RunSynchronously |> List.ofSeq
    let foods = inputLines |> List.map (fun i -> let _match = Regex.Match(i, " \(.*\)")
                                                 match _match.Success with
                                                 |true -> 
                                                    let allergens:Allergen list = _match.Value.[11.._match.Length-2].Split ", " |> List.ofSeq
                                                    let ingredients = i.[0.._match.Index-1].Split ' ' |> Array.map (fun f -> {Name = String.Intern(f); Allergen = None})
                                                    (ingredients, allergens)
                                                 |false -> failwith "")
                           |> resolveAllergens
    let unresolvedIngredients = foods |> Seq.fold (fun allIngredients (i, a) -> (i |> List.ofArray)@allIngredients) []
                                      |> Seq.filter (fun i -> i.Allergen = None) |> List.ofSeq
    let dangerousIngredientList = String.Join (",", (foods |> Seq.fold (fun allIngredients (i, a) -> (i |> List.ofArray)@allIngredients) []
                                  |> Seq.filter (fun i -> i.Allergen <> None) |> Seq.sortBy (fun i -> i.Allergen) |> Seq.map (fun i -> i.Name) |> Seq.distinct))
    printfn "%i %s" unresolvedIngredients.Length dangerousIngredientList
    0