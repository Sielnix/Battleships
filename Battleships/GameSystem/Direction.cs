namespace Battleships.GameSystem
{
    public enum Direction
    {
        Right,
        Down
    }

    public static class DirectionExtensions
    {
        public static bool TryParse(string directionStr, out Direction direction)
        {
            direction = default;
            if (string.IsNullOrWhiteSpace(directionStr))
            {
                return false;
            }

            directionStr = directionStr.Trim();
            if (Enum.TryParse(directionStr, true, out direction))
            {
                return true;
            }

            if (directionStr.Length != 1)
            {
                return false;
            }

            if (char.ToUpperInvariant(directionStr[0]) == 'R')
            {
                direction = Direction.Right;
                return true;
            }

            if (char.ToUpperInvariant(directionStr[0]) == 'D')
            {
                direction = Direction.Down;
                return true;
            }

            return false;
        }
    }
}
