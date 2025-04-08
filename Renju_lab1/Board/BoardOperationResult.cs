namespace Renju_lab1.Board;

public record BoardOperationResult(string Message);
public record Success(): BoardOperationResult ("");
public record OutOfBounds(Pawn Pawn) : BoardOperationResult($"Pawn is out of bounds. Pawn pos is row: ${Pawn.Position.Row}, col: ${Pawn.Position.Col}");
public record CellIsOccupied(Pawn Pawn) : BoardOperationResult($"Cell is occupied. Pawn pos is row: ${Pawn.Position.Row}, col: ${Pawn.Position.Col}");


