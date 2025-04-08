using Renju_lab1;
using Renju_lab1.Board;
using Renju_lab1.Files;

const int boardSize = 19;

int Main()
{
    var testFilesService = new TestFilesService();
    var testCases = testFilesService.ReadTestCases("./Assets/testCase6.txt");

    for (var i = 0; i < testCases.Count; i++)
    {
        var board = new Board(boardSize);
        switch (testFilesService.FillBoardFromTestCase(board, testCases[i]))
        {
            case OutOfBounds oErr:
                Console.Error.WriteLine($"Test case {i} failed: {oErr.Message}");
                continue;
            case CellIsOccupied err:
                Console.Error.WriteLine($"Test case {i} failed: {err.Message}");
                continue;
        }
    
        var game = new Renju(board);
        var result = game.Run();

        switch (result)
        {
            case Draw:
                Console.WriteLine("0");
                break;
            case Winner winner:
                Console.WriteLine($"{(int)winner.Pawn.Color}\n{winner.Pawn.Position.Row + 1} {winner.Pawn.Position.Col + 1}");
                break;
        }
    }
    return 0;
}

Main();


