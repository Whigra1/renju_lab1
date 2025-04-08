using Renju_lab1.Board;

namespace Renju_lab1.Files;

public class TestCase
{
    public Cell[,] Cells { get; init; }

    public TestCase(int boardSize)
    {
        Cells = new Cell[boardSize, boardSize];
        for (var row = 0; row < boardSize; row++)
        {
            for (var col = 0; col < boardSize; col++)
            {
                Cells[row, col] = new Cell();
            }
        }
    }
}