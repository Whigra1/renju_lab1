using System.Diagnostics.Metrics;
using Renju_lab1.Board;

namespace Renju_lab1;

public record GameResult;
public record Draw : GameResult;
public record Winner(Pawn Pawn) : GameResult;

public class Renju(Board.Board board)
{
    public GameResult Run()
    {
        for (var y = 0; y < board.Size; y++)
        {
            for (var x = 0; x < board.Size ; x++)
            {
                var cell = board[y,x];
                if (cell is OccupiedCell c && IsGameOver(c))
                {
                    return new Winner(c.Pawn);
                }
                
            }
        }
        return new Draw();
    }

    private bool IsGameOver (OccupiedCell currentCell)
    {

        var col = currentCell.Pawn.Position.Col;
        var row = board.Size - currentCell.Pawn.Position.Row - 1;

        List<List<Cell>> sides = [
            board.DrawLine(col, col, row, row - 5), // up
            board.DrawLine(col, col, row, row + 5), // down
            board.DrawLine(col, col - 5, row, row), // left
            board.DrawLine(col, col + 5, row, row), // right
            board.DrawLine(col, col - 5, row, row - 5), // diagonal up left
            board.DrawLine(col, col + 5, row, row - 5), // diagonal up right
            board.DrawLine(col, col - 5, row, row + 5), // diagonal down left
            board.DrawLine(col, col + 5, row, row + 5), // diagonal down right
        ];
        

        foreach (var cells in sides)
        {
            if (AllFiveSame(currentCell, cells)) return true;
        }
       
        return false;
    }
    
    private bool AllFiveSame(OccupiedCell currentCell, List<Cell> cells)
    {
        if (cells.Count != 5) return false;
        foreach (var cell in cells)
        {
            switch (cell)
            {
                case OccupiedCell c when c.Pawn.Color == currentCell.Pawn.Color:
                    continue;
                default:
                    return false;
            }
        }

        return true;
    }
}