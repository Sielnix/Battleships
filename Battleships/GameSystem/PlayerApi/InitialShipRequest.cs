namespace Battleships.GameSystem.PlayerApi;

public record struct InitialShipRequest(string ShipName, int Length);