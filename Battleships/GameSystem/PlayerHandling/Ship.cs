namespace Battleships.GameSystem.PlayerHandling;

internal class Ship
{
    public Ship(string name)
    {
        Name = name;
        OccupiedPositions = new List<PositionInfo>();
    }

    public string Name { get; }

    public List<PositionInfo> OccupiedPositions { get; }

    public bool IsSunk()
    {
        return OccupiedPositions.All(p => p.Position == MyPosition.ShipShot);
    }
}