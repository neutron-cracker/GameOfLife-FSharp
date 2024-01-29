namespace GameOfLife

open System;

type Board (height : int, width : int) =
    let mutable internalMatrix = Array2D.create height width false    
    let safeWidth width' =
        match width' with
        | -1 -> (width - 1)
        | x when x = width -> 0
        | _ -> width'
    let safeHeight height' =
        match height' with
        | -1 -> (height - 1)
        | x when x = height -> 0
        | _ -> height'
    let getSafeIndex (width', height') = (safeWidth width', safeHeight height')
    let relativeCoordinates = [
        for x in -1 .. 1 do
            for y in -1 .. 1 do
                if x <> 0 || y <> 0 then
                    yield (x, y)
    ]
    let neighborsForCoordinate (a, b) =
        relativeCoordinates
        |> List.map (fun (x, y) -> (x + a, y + b))
        |> List.map getSafeIndex
    let ToTuple a b = (a, b)
    let numberOfNeighbors a b =
        let index = (a, b)
        neighborsForCoordinate index
        |> List.filter (fun (a, b) -> internalMatrix[a, b])
        |> List.length
    let calculateNewValue (neighborCount, oldValue) =
        match (neighborCount, oldValue) with
        | (3, _) | (2, true) -> true
        | _ -> false
    let getPrintingText() =
        let printCell value = if value then "[X]" else "   "
        let printLine =
            let middlePart = String.replicate width "---"
            "+" + middlePart + "+\n"

        let textMatrix = Array2D.map printCell internalMatrix
        let allTextRows =
            seq {
                for n in 0 .. (height - 1) do
                    textMatrix[n, *]
                    |> Seq.fold (+) ""
                    |> (fun x -> $"|{x}|\n")
            }
            |> Seq.insertAt height (printLine)
            |> Seq.insertAt 0 (printLine)
        
        Seq.fold (+) "" allTextRows
    member this.Height with get () = height
    member this.Width with get () = width
    member this.SetInitial() = do
        internalMatrix[7,5] <- true
        internalMatrix[5,6] <- true
        internalMatrix[7,6] <- true
        internalMatrix[6,7] <- true
        internalMatrix[7,7] <- true
    member this.MoveNext() = do
        let f a b value = (numberOfNeighbors a b, value)
        let newMatrix = Array2D.mapi f internalMatrix
                        |> Array2D.map calculateNewValue
        internalMatrix <- newMatrix
    member this.Print() = do
        let text = getPrintingText()
        printf $"%s{text}\n"   
        
        // Reset cursor
        let struct (x, y) = Console.GetCursorPosition()
        Console.SetCursorPosition(x, y - height - 3)