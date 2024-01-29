open System

[<EntryPoint>]
let main args =
    printfn "Arguments passed to function : %A" args
    let maximumCounter = 100

    async {
        let mutable matrix = GameOfLife.Start.initialMatrix
        GameOfLife.Start.printMatrix matrix
        let areaLength = Array2D.length1 matrix + 3
        
        for i = 0 to maximumCounter do
            do! Async.Sleep(1000)
            matrix <- GameOfLife.Next.calculateNext matrix

            let struct (x, y) = Console.GetCursorPosition()
            Console.SetCursorPosition(x, y - areaLength)
            GameOfLife.Start.printMatrix matrix
    } |> Async.RunSynchronously
    
    // Return 0. This indicates success.
    0