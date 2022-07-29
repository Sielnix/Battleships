using Battleships.GameSystem.PlayerApi;

namespace Battleships.GameSystem.PlayerHandling;

internal readonly record struct InitialShipInfo(InitialShipPosition InitialPosition, int Length, string Name);