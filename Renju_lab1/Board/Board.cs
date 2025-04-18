using System.Drawing;
using Renju_lab1.Files;

namespace Renju_lab1.Board;

public record Cell;
public record OccupiedCell(Pawn Pawn) : Cell;

public class Board
{
    private Cell[,] Cells { get; init; }
    public int Size { get; init; }
    public Board(int size = BoardOptions.BoardSize)
    {
        Cells = new Cell[size, size];
        for (var row = 0; row < size; row++)
        {
            for (var col = 0; col < size; col++)
            {
                Cells[row, col] = new Cell();
            }
        }
        Size = size;
    }

    /// <summary>
    /// Places a pawn on the specified cell of the board. Checks for out-of-bounds or occupied cells before placement.
    /// </summary>
    /// <param name="pawn">The pawn to be placed, containing its color and position.</param>
    /// <returns>
    /// A result representing the outcome of the operation. Returns <see cref="OutOfBounds"/> if the position is outside the board,
    /// <see cref="CellIsOccupied"/> if the specified cell is already occupied, or <see cref="Success"/> if the pawn is placed successfully.
    /// </returns>
    public BoardOperationResult PlacePawn(Pawn pawn)
    {
        var row = pawn.Position.Row;
        var col = pawn.Position.Col;

        if (row < 0 || row >= Cells.GetLength(0) || col < 0 || col >= Cells.GetLength(1))
            return new OutOfBounds(pawn); // Out of bounds
        if (Cells[row, col] is OccupiedCell)
            return new CellIsOccupied(pawn); // Cell is not empty

        Cells[row, col] = new OccupiedCell(pawn);
        return new Success(); 
    }

    /// <summary>
    /// Draws a straight line of cells on the board between the specified start and end coordinates, limited by the specified size.
    /// </summary>
    /// <param name="x1">The starting x-coordinate of the line.</param>
    /// <param name="x2">The ending x-coordinate of the line.</param>
    /// <param name="y1">The starting y-coordinate of the line.</param>
    /// <param name="y2">The ending y-coordinate of the line.</param>
    /// <param name="size">The maximum number of cells to include in the line. Defaults to 5.</param>
    /// <returns>A list of cells representing the drawn line. If the coordinates are out of bounds or invalid, returns an empty list.</returns>
    public List<Cell> DrawLine(int x1, int x2, int y1, int y2, int size = 5)
    {
        if (x1 > x2) (x1, x2) = (x2, x1);
        if (y1 > y2) (y1, y2) = (y2, y1);
        
        if (x2 >= Size - 1) x2 = Size - 1;
        if (y2 >= Size - 1) y2 = Size - 1;
        
        if (
            x1 < 0 || x1 >= Size || x2 < 0 || x2 >= Size ||
            y1 < 0 || y1 >= Size || y2 < 0 || y2 >= Size) // Check borders
        {
            return [];
        }

        if (x1 == x2 && y1 == y2) return [Cells[y1, x1]];
        
        var result = new List<Cell>(size);
        
        var dx = Math.Abs(x2 - x1);
        var dy = Math.Abs(y2 - y1);
        var step = 1;
        
        for (var i = 0.5; i <= size; i += step)
        {
            if (dx == 0)
            {
                result.Add(Cells[(int) (i + y1), x1]);
                continue;
            }
            var y = (int) (i * dy / dx); // Triangle similarity formula :)
            result.Add(Cells[y + y1, (int)(i + x1)]);
        }
        return result;
    }

    /// <summary>
    /// Provides indexed access to the cells on the game board based on row and column indices.
    /// </summary>
    /// <param name="row">The row index of the cell to access.</param>
    /// <param name="col">The column index of the cell to access.</param>
    /// <returns>
    /// The cell at the specified position. If the indices are out of bounds, returns a default empty cell value.
    /// </returns>
    public Cell this[int row, int col]
    {
        get
        {
            // Add bounds checking if necessary
            if (row < 0 || row >= Size || col < 0 || col >= Size)
            {
                return new Cell();
            }
            return Cells[row, col];
        }
    }
}