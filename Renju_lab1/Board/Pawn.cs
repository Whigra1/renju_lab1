namespace Renju_lab1.Board;
public enum PawnColor
{
    None,
    Black,
    White
}

public record Pawn(PawnColor Color, Position Position);