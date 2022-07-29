namespace Battleships.GameSystem.PlayerHandling;

internal readonly struct Area
{
    public Area(Position topLeft, Position bottomRight)
    {
        TopLeft = topLeft;
        BottomRight = bottomRight;
    }

    public Position TopLeft { get; }
    public Position BottomRight { get; }

    public bool Overlaps(Area other)
    {
        if (TopLeft.Column > other.BottomRight.Column || other.TopLeft.Column > BottomRight.Column)
        {
            return false;
        }

        if (TopLeft.Row > other.BottomRight.Row || other.TopLeft.Row > BottomRight.Row)
        {
            return false;
        }

        return true;
    }
}