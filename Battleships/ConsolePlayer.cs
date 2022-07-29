using Battleships.GameSystem;
using Battleships.GameSystem.PlayerApi;

namespace Battleships;

internal class ConsolePlayer : IBattleshipPlayer
{
    private readonly int _areaWidth;

    public ConsolePlayer(int areaWidth)
    {
        _areaWidth = areaWidth;
    }

    public ShipBuilder GetShipBuilder(string shipName, int length)
    {
        Func<InitialShipPosition> positionBuilder = () =>
        {
            (Position position, Direction direction) = GetShip(shipName);

            return new InitialShipPosition(position, direction);
        };

        Action validAction = () => { };

        Action<InvalidPositionReason> invalidPosition =
            reason => Console.WriteLine("Given coordinates are invalid - {0}", reason);

        return new ShipBuilder(positionBuilder, validAction, invalidPosition);
    }

    public void HandleEnemyShotInfo(Position position, HitResult shotResult, string? shipName)
    {
        Console.WriteLine("Enemy shot at position {0}", position);
        switch (shotResult)
        {
            case HitResult.Miss:
                Console.WriteLine("He missed");
                break;
            case HitResult.Hit:
                Console.WriteLine("He hit your ship {0}", shipName);
                break;
            case HitResult.Sink:
                Console.WriteLine("He sank your ship {0}", shipName);
                break;
        }
    }

    public ShootingHandler HandleShooting()
    {
        Func<Position> positionProvider = () => GetPosition("Where do you want to shot?");
        Action<HitResult> resultHandler = (result) =>
        {
            switch (result)
            {
                case HitResult.Miss:
                    Console.WriteLine("You missed!");
                    break;
                case HitResult.Hit:
                    Console.WriteLine("You hit the enemy!");
                    break;
                case HitResult.Sink:
                    Console.WriteLine("You sank enemy's ship!");
                    break;
            }
        };

        return new ShootingHandler(positionProvider, resultHandler);
    }

    public void HandleGameEnd(PlayerResult playerResult)
    {
        switch (playerResult)
        {
            case PlayerResult.Winner:
                Console.WriteLine(" YOU WON ");
                break;
            case PlayerResult.Looser:
                Console.WriteLine(" YOU LOST ");
                break;
        }
    }

    private (Position position, Direction direction) GetShip(string name)
    {
        Position position = GetPosition($"Type top-left position of ship {name} - in form [A-J][1-10], for example C7");
        Direction direction = GetDirection($"In which direction ship {name} should go? R - right, D - down");

        return (position, direction);
    }

    private Position GetPosition(string message)
    {
        while (true)
        {
            Console.WriteLine(message);

            string? positionString = Console.ReadLine();

            Position position;
            if (string.IsNullOrEmpty(positionString) || !Position.TryParse(positionString, _areaWidth, out position))
            {
                Console.WriteLine("Invalid value {0}. Try typing again", positionString);
                continue;
            }

            return position;
        }
    }

    private Direction GetDirection(string message)
    {
        while (true)
        {
            Console.WriteLine(message);
            string? directionStr = Console.ReadLine();

            Direction direction;
            if (string.IsNullOrWhiteSpace(directionStr) || !DirectionExtensions.TryParse(directionStr, out direction))
            {
                Console.WriteLine("Invalid value {0}. Try typing again", directionStr);
                continue;
            }

            return direction;
        }
    }
}