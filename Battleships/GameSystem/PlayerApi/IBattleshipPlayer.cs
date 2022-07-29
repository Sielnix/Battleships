namespace Battleships.GameSystem.PlayerApi;

public interface IBattleshipPlayer
{
    ShipBuilder GetShipBuilder(string shipName, int length);

    ShootingHandler HandleShooting();

    void HandleEnemyShotInfo(Position position, HitResult shotResult, string? shipName);
    void HandleGameEnd(PlayerResult playerResult);
}