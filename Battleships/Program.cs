using Battleships.GameSystem;
using Battleships.GameSystem.PlayerApi;

namespace Battleships
{
    internal class Program
    {
        const int AreaSize = 10;

        private static readonly IReadOnlyList<InitialShipRequest> GameShips = new InitialShipRequest[]
        {
            new("Battleship", 5),
            new("Destroyer #1", 4),
            new("Destroyer #2", 4)
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to battleships!");

            BattleshipGame game = new(
                AreaSize, 
                GameShips, 
                new ComputerPlayer(AreaSize), 
                new ConsolePlayer(AreaSize));


            // in real game all the actions should be task based with cancellation support
            game.PlayGame();

            Console.ReadLine();
        }
    }
}