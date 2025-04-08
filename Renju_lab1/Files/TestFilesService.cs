using Renju_lab1.Board;

namespace Renju_lab1.Files;

public class TestFilesService(int boardSize = 19)
{
    /// <summary>
    /// Reads test cases from a specified file path and returns a list of test cases. Test cases should not have spaces between.
    /// </summary>
    /// <param name="path">The path of the file containing the test cases.</param>
    /// <returns>A list of test cases read from the file, or an empty list if the file is invalid or no test cases exist.</returns>
    public List<TestCase> ReadTestCases(string path)
    {
        if (!File.Exists(path)) return [];

        var lines = File.ReadLines(path);
        using var enumerator = lines.GetEnumerator();
        if (!enumerator.MoveNext()) return []; // empty file

        if (!int.TryParse(enumerator.Current, out int testCountNumber)) return []; // invalid file

        if (testCountNumber is <= 0 or > 11) return []; // invalid cases count
        
        var testCases = new List<TestCase>();
        var testCase = new TestCase(boardSize);
        
        var linesLeftInFile = testCountNumber * boardSize; // if we have 2 test then out file must have 2 * 19 = 38 lines left (without first line)
        var curLine = 0;
        
        while (curLine <= linesLeftInFile)
        {
            if (!enumerator.MoveNext()) break;
            var line = enumerator.Current;
            for (int i = 0, cellPos = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c is ' ' or '\0') continue; // skip spaces
                testCase.Cells[curLine % boardSize, cellPos % boardSize] = CreateCell(c, curLine % boardSize, cellPos % boardSize);
                cellPos++;
            }
            curLine++;
            if (curLine % boardSize != 0) continue;
            
            testCases.Add(testCase);
            testCase = new TestCase(boardSize);
        }
        return testCases;
    }

    private Cell CreateCell(char c, int row, int col)
    {
        return c switch
        {
            '0' => new Cell(),
            '1' => new OccupiedCell(new Pawn(PawnColor.Black, new Position(row, col))),
            '2' => new OccupiedCell(new Pawn(PawnColor.White, new Position(row, col))),
            _ => throw new Exception("Invalid character")
        };
    }
    
    public BoardOperationResult FillBoardFromTestCase(Board.Board board, TestCase testCase)
    {
        foreach (var cell in testCase.Cells)
        {
            switch (cell)
            {
                case OccupiedCell occupiedCell:
                    switch (board.PlacePawn(occupiedCell.Pawn))
                    {
                        case OutOfBounds err: return err;
                        case CellIsOccupied err: return err;
                        case Success _: break;
                    }
                    break;
            }
        }
        return new Success();
    }
}