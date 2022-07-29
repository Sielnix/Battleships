namespace Battleships.GameSystem.PlayerApi;

public readonly record struct ShipBuilder(Func<InitialShipPosition> PositionBuilder, Action ValidAction, Action<InvalidPositionReason> InvalidAction);