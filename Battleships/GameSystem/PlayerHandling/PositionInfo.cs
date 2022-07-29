namespace Battleships.GameSystem.PlayerHandling;

internal class PositionInfo
{
    public PositionInfo(Ship? ship)
    {
        Ship = ship;
        Position = ship == null ? MyPosition.Empty : MyPosition.ShipNotShot;
    }

    public MyPosition Position { get; set; }
    public Ship? Ship { get; }
}