using Battleships.GameSystem.PlayerApi;
using Battleships.GameSystem.PlayerHandling;

namespace Battleships.GameSystem
{
    public class BattleshipGame
    {
        private readonly BattleshipPlayerStore _player1;
        private readonly BattleshipPlayerStore _player2;

        private bool _isEnd = false;

        public BattleshipGame(int areaWidth, IReadOnlyList<InitialShipRequest> shipRequest, IBattleshipPlayer player1, IBattleshipPlayer player2)
        {
            _player1 = new BattleshipPlayerStore(player1, BuildShips(shipRequest, player1, areaWidth), areaWidth);
            _player2 = new BattleshipPlayerStore(player2, BuildShips(shipRequest, player2, areaWidth), areaWidth);
        }

        private static IReadOnlyList<InitialShipInfo> BuildShips(IReadOnlyList<InitialShipRequest> shipRequest, IBattleshipPlayer player, int areaWidth)
        {
            List<InitialShipInfo> result = new(shipRequest.Count);
            foreach (var initialShipRequest in shipRequest)
            {
                InitialShipPosition initialShipPosition;
                ShipBuilder builder = player.GetShipBuilder(initialShipRequest.ShipName, initialShipRequest.Length);
                do
                {
                    initialShipPosition = builder.PositionBuilder();
                    if (IsValid(initialShipPosition, initialShipRequest.Length, areaWidth, result, out InvalidPositionReason invalidReason))
                    {
                        builder.ValidAction();
                        break;
                    }

                    builder.InvalidAction(invalidReason);
                } while (true);

                result.Add(new InitialShipInfo(initialShipPosition, initialShipRequest.Length, initialShipRequest.ShipName));
            }

            return result;
        }

        private static bool IsValid(InitialShipPosition position, int length, int areaWidth, IReadOnlyList<InitialShipInfo> existingShips, out InvalidPositionReason invalidReason)
        {
            invalidReason = default;

            Position startPosition = position.Position;
            Position endPosition = startPosition.GetEndPosition(length, position.Direction);

            if (!startPosition.IsInArea(areaWidth)
                || !endPosition.IsInArea(areaWidth))
            {
                invalidReason = InvalidPositionReason.OutsideArea;
                return false;
            }

            Area area = new Area(startPosition, endPosition);
            if (existingShips
                .Select(s => new Area(s.InitialPosition.Position, s.InitialPosition.Position.GetEndPosition(s.Length, s.InitialPosition.Direction)))
                .Any(ea => ea.Overlaps(area)))
            {
                invalidReason = InvalidPositionReason.CoverOtherShip;
                return false;
            }

            return true;
        }

        public void PlayGame()
        {
            while (!_isEnd)
            {
                if (ApplyRound(_player1, _player2) || ApplyRound(_player2, _player1))
                {
                    _isEnd = true;
                }
            }
        }

        private static bool ApplyRound(BattleshipPlayerStore shooter, BattleshipPlayerStore receiver)
        {
            ShootingHandler p1Handler = shooter.HandleShooting();
            Position position = p1Handler.PositionProvider();
            HitResult shotResult = receiver.TakeAShot(position);
            p1Handler.ResultHandler(shotResult);

            if (receiver.IsDead())
            {
                shooter.HandleGameEnd(PlayerResult.Winner);
                receiver.HandleGameEnd(PlayerResult.Looser);

                return true;
            }

            return false;
        }
    }
}
