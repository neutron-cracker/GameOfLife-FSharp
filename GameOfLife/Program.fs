open System

[<EntryPoint>]
let main args =
    printfn "Arguments passed to function : %A" args
    let maximumCounter = 100000   

    async {
        let board = GameOfLife.Board(30, 60)
        board.FillRandom()
        board.Print()
        
        for i = 0 to maximumCounter do
            do! Async.Sleep(0)
            board.MoveNext()
            board.Print()
    } |> Async.RunSynchronously
    
    // Return 0. This indicates success.
    0