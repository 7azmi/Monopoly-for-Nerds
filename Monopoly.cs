using System.Drawing;
//using System.Runtime.CompilerServices;

namespace Monopoly_for_Nerds;

using static Monopoly.Board;
using static Monopoly.Engine;

public partial class Monopoly
{
    public Monopoly(Player[] players)
    {
        _allPlayers = players;

        ActivePlayers = new List<Player>(_allPlayers);

        //ActivePlayers = _allPlayers.ToList();

        WhoseTurn = ActivePlayers.First();
        
        Init(this);
        
        Start();
    }


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
            
            OnAuction?.Invoke(onSale);
        }
    }
}