module GameOfLife.Next
    let private safeWidth totalWidth width =
        match width with
        | -1 -> (totalWidth - 1)
        | x when x = totalWidth -> 0
        | _ -> width

    let private safeHeight totalHeight height =
        match height with
        | -1 -> (totalHeight - 1)
        | x when x = totalHeight -> 0
        | _ -> height

    let private getSafeIndex (matrix : bool array2d) (width, height) = (safeWidth (Array2D.length1 matrix) width, safeHeight (Array2D.length2 matrix) height)
    
    let private relativeCoordinates = [
        for x in -1 .. 1 do
            for y in -1 .. 1 do
                if x <> 0 || y <> 0 then
                    yield (x, y)
    ]
    
    let private neighborsForCoordinate matrix (a, b) =
        relativeCoordinates
        |> List.map (fun (x, y) -> (x + a, y + b))
        |> List.map (getSafeIndex matrix)
    
    let private ToTuple a b = (a, b)
    let private numberOfNeighbors matrix a b =
        let index = (a, b)
        neighborsForCoordinate matrix index
        |> List.filter (fun (a, b) -> matrix[a, b])
        |> List.length

    let private calculateNewValue (neighborCount, oldValue) =
        match (neighborCount, oldValue) with
        | (3, _) | (2, true) -> true
        | _ -> false
    
    let calculateNext matrix =
        let f a b value = (numberOfNeighbors matrix a b, value)
        Array2D.mapi f matrix
        |> Array2D.map calculateNewValue
        
