

//using System.Runtime.CompilerServices;


using static MonopolyTerminal.Monopoly.Board;
using static MonopolyTerminal.Monopoly.Engine;

namespace MonopolyTerminal;
public partial class Monopoly
{
    public Monopoly(GameSettings gameSettings)
    {
        GameSettings = gameSettings;
        
        _allPlayers = gameSettings.Players;

        ActivePlayers = new List<Player>(_allPlayers);

        //ActivePlayers = _allPlayers.ToList();

        WhoseTurn = ActivePlayers.First();
        
        Init(this);
        
        Start();
    }

    public static GameSettings GameSettings;
    
    private readonly Player[] _allPlayers;
    public static List<Player> ActivePlayers;
    public static Player WhoseTurn { get; set; }

    public static class Auction
    {
        public static int MostBid = 0;
        public static Player MostBidder;
        public static Property OnSale;
        
        public static void OpenAuction(Property onSale)
        {
            OnSale = onSale;
            MostBidder = null;
            MostBid = 0;
        }
    }
}