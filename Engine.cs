namespace Monopoly_for_Nerds;

public partial class Monopoly
{
    public static class Engine
    {
        //actions
        public static Action<Monopoly> OnInitialization;
        public static Action<Player> OnStart;
        public static Action<Player> OnPlayerTurn;
        public static Action<Board.Place> OnLanding;
        public static Action<Board.Place, Player> OnMovingOnPlace;
        public static Action<Board.Property, Player, Player, int> OnLandingOnOwnedProperty;
        public static Action<Board.Property> OnLandingOnUnownedProperty;
        public static Action<Board.Property, Player, int> OnAuction;
        public static Action<Player> OnInJail;
        public static Action<Player, Player> OnSetDeal;
        public static Action<int> OnLandingActionComplete;


        static void NextPlayer()
        {
            var current = _activePlayers.Peek();
            _activePlayers.Dequeue();
            _activePlayers.Enqueue(current); //go back boi
        }
    }
}