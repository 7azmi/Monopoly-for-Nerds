

//using System.Runtime.CompilerServices;


using System.Reflection;
using static System.Threading.Tasks.Task;
using static MonopolyTerminal.Monopoly.Board;
using static MonopolyTerminal.Monopoly.Engine;

namespace MonopolyTerminal;
public partial class Monopoly
{

    public static GameSettings GameSettings;
    
    private readonly Player[] _allPlayers;


    public static List<Player> ActivePlayers;
    public static Player[] AllPlayers { get; private set; }
    public static Player WhoseTurn { get; set; }
    public Monopoly(GameSettings gameSettings)
    {
        GameSettings = gameSettings;
        
        AllPlayers = gameSettings.Players;

        ActivePlayers = new List<Player>(AllPlayers);

        //ActivePlayers = _allPlayers.ToList();

        WhoseTurn = ActivePlayers.First();
        
        Init(this);
        
        Start();
    }

    public static class Auction
    {
        static CancellationTokenSource _source = new ();

        private static int _auctionTimer = 0;
        private const int AuctionDuration = 40;

        static Auction()
        {
//            Console.WriteLine("hey1!");
            OnBid += (_, _, _) =>
            {
                _auctionTimer = 0;
                _source.Cancel();
                _source = new CancellationTokenSource();
                GetBiddersResponses(_source);
                //Console.WriteLine("hey!");
            };
        }
        public static int MostBid = 0;
        public static Player MostBidder;
        public static Property OnSale;

        //private static CancellationTokenSource s_cts;

        public static void OpenAuction(Property onSale)
        {
            OnSale = onSale;
            MostBidder = null;
            MostBid = 0;
            
            //await WhenAny(tasks);
            //Thread.Sleep(10000);

            //var d =  Delay(100);
            
            
            GetBiddersResponses(_source);
            for (_auctionTimer = 0; _auctionTimer < AuctionDuration; _auctionTimer++)
            { 
                Thread.Sleep(100);
                //Console.cu
                if (_auctionTimer % 10 == 0 && AuctionDuration - _auctionTimer < 40)
                {
                    //Console.Beep();
                    Human.Terminal.Log(((AuctionDuration - _auctionTimer) * .1f).ToString());
                }
            }

            MostBidder.CloseAuction(onSale);
        }

        private static async Task GetBiddersResponses(CancellationTokenSource source)
        {
            var bidders = new List<Player>(ActivePlayers);

            var tasks = new Task[bidders.Count];

            for (var i = 0; i < bidders.Count; i++)
                tasks[i] = bidders[i].GetInput().OnBidOrFold(bidders[i], MostBid, source.Token);
        }
    }
}