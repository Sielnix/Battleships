using Battleships.GameSystem;
using Battleships.GameSystem.PlayerApi;

namespace Battleships;

internal class ComputerPlayer : IBattleshipPlayer
{
    private readonly int _areaWidth;
    private readonly Random _random = new();

    public ComputerPlayer(int areaWidth)
    {
        _areaWidth = areaWidth;
    }

    public ShipBuilder GetShipBuilder(string shipName, int length)
    {
        Func<InitialShipPosition> positionBuilder = () =>
        {
            int directionVal = _random.Next(2);
            Direction direction = (Direction)directionVal;

            int maxCol = direction == Direction.Right ? _areaWidth - length : _areaWidth;
            int maxRow = direction == Direction.Down ? _areaWidth - length : _areaWidth;

            int col = _random.Next(maxCol);
            int row = _random.Next(maxRow);

            return new InitialShipPosition(new Position((byte)col, (byte)row), direction);
        };

        return new ShipBuilder(positionBuilder, () => { }, _ => { });
    }
        
    public void HandleEnemyShotInfo(Position position, HitResult shotResult, string? shipName)
    {
    }

    public void HandleGameEnd(PlayerResult playerResult)
    {
    }

    public ShootingHandler HandleShooting()
    {
        Func<Position> shootFunc = () =>
        {
            int col = _random.Next(_areaWidth);
            int row = _random.Next(_areaWidth);

            // completely random shot, we don't even check whether we had already shot at that position
            // easy mode for user
            // it could be greatly extend
            return new Position((byte)col, (byte)row);
        };

        return new ShootingHandler(shootFunc, _ => { });
    }
}