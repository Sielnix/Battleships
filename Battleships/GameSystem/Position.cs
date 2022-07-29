namespace Battleships.GameSystem
{
    public readonly struct Position
    {
        private const int MaxAreaSize = 26;
        private const char FirstCharacter = 'A';

        public Position(byte column, byte row)
        {
            Column = column;
            Row = row;
        }

        public byte Column { get; }
        public byte Row { get; }

        public bool IsInArea(int areaWidth)
        {
            ValidateAreaSize(areaWidth);

            return Column < areaWidth && Row < areaWidth;
        }

        public Position GetEndPosition(int length, Direction direction)
        {
            ValidateLength(length);
            switch (direction)
            {
                case Direction.Right:
                    return new Position((byte)(Column + (byte)length - 1), Row);
                case Direction.Down:
                    return new Position(Column, (byte)(Row + (byte)length - 1));
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public static bool TryParse(string value, int areaWidth, out Position position)
        {
            position = default;
            ValidateAreaSize(areaWidth);


            char lastCharacter = (char)(FirstCharacter + areaWidth);

            value = value.Trim().ToUpperInvariant();

            if (value.Length < 2 || value.Length > 3)
            {
                return false;
            }

            char positionChar = value[0];
            if (positionChar < FirstCharacter || positionChar > lastCharacter)
            {
                return false;
            }

            byte row = (byte)(positionChar - FirstCharacter);

            if (!byte.TryParse(value.Substring(1), out byte column))
            {
                return false;
            }

            if (column >= areaWidth)
            {
                return false;
            }

            position = new Position(column, row);
            return true;
        }

        private static void ValidateAreaSize(int areaWidth)
        {
            if (areaWidth < 1 || areaWidth > MaxAreaSize)
            {
                throw new ArgumentOutOfRangeException(nameof(areaWidth));
            }
        }

        private static void ValidateLength(int length)
        {
            if (length < 1 || length > MaxAreaSize)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
        }

        public override string ToString()
        {
            char rowChar = (char)(FirstCharacter + Row);

            return rowChar.ToString() + (Column + 1);
        }
    }
}
