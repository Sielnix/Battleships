namespace Battleships.GameSystem.PlayerApi;

public readonly struct ShootingHandler
{
    public ShootingHandler(Func<Position> positionProvider, Action<HitResult> resultHandler)
    {
        PositionProvider = positionProvider;
        ResultHandler = resultHandler;
    }

    public Func<Position> PositionProvider { get; }
    public Action<HitResult> ResultHandler { get; }
}