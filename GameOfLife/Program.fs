open System

[<EntryPoint>]
let main args =
    printfn "Arguments passed to function : %A" args
    let maximumCounter = 100   

    async {
        let board = GameOfLife.Board(10, 10)
        board.SetInitial()
        board.Print()
        
        for i = 0 to maximumCounter do
            do! Async.Sleep(1000)
            board.MoveNext()
            board.Print()
    } |> Async.RunSynchronously
    
    // Return 0. This indicates success.
    0