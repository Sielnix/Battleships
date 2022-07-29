using Battleships.GameSystem.PlayerApi;

namespace Battleships.GameSystem.PlayerHandling;

internal class BattleshipPlayerStore
{
    private readonly IBattleshipPlayer _player;

    // row, col
    private readonly PositionInfo?[][] _myState;
    private readonly List<Ship> _ships;

    public BattleshipPlayerStore(IBattleshipPlayer player, IReadOnlyList<InitialShipInfo> positions, int areaWidth)
    {
        _player = player;

        (PositionInfo?[][]? state, List<Ship>? ships) = BuildInitialState(positions, areaWidth);

        _myState = state;
        _ships = ships;
    }

    private static (PositionInfo?[][] state, List<Ship> ships) BuildInitialState(
        IReadOnlyList<InitialShipInfo> initialShips, int areaWidth)
    {
        PositionInfo?[][] state = new PositionInfo?[areaWidth][];
        List<Ship> ships = new();

        for (int i = 0; i < areaWidth; i++)
        {
            state[i] = new PositionInfo?[areaWidth];
        }

        foreach (var initialShip in initialShips)
        {
            int col = initialShip.InitialPosition.Position.Column;
            int row = initialShip.InitialPosition.Position.Row;

            Ship ship = new(initialShip.Name);
            ships.Add(ship);

            for (int j = 0; j < initialShip.Length; j++)
            {
                int finalCol = col + (initialShip.InitialPosition.Direction == Direction.Right ? j : 0);
                int finalRow = row + (initialShip.InitialPosition.Direction == Direction.Down ? j : 0);

                PositionInfo posInfo = new(ship);
                state[finalRow][finalCol] = posInfo;
                ship.OccupiedPositions.Add(posInfo);
            }
        }

        return (state, ships);
    }

    public ShootingHandler HandleShooting()
    {
        return _player.HandleShooting();
    }

    public HitResult TakeAShot(Position position)
    {
        HitResult result = ApplyShot(position, out string? shipName);

        _player.HandleEnemyShotInfo(position, result, shipName);

        return result;
    }

    public bool IsDead()
    {
        return _ships.All(s => s.IsSunk());
    }

    public void HandleGameEnd(PlayerResult playerResult)
    {
        _player.HandleGameEnd(playerResult);
    }

    private HitResult ApplyShot(Position position, out string? shipName)
    {
        PositionInfo? positionInfo = _myState[position.Row][position.Column];
        shipName = positionInfo?.Ship?.Name;

        if (positionInfo == null)
        {
            return HitResult.Miss;
        }

        if (positionInfo.Position != MyPosition.ShipNotShot)
        {
            return HitResult.Miss;
        }

        positionInfo.Position = MyPosition.ShipShot;

        if (positionInfo.Ship == null)
        {
            // should never happen
            throw new InvalidOperationException("Invalid state");
        }

        if (positionInfo.Ship.IsSunk())
        {
            return HitResult.Sink;
        }

        return HitResult.Hit;
    }
}