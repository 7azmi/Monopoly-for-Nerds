using Monopoly.Interfaces;
using static Monopoly.Monopoly;
using static Monopoly.Monopoly.Board;
using static Monopoly.Monopoly.Player;
namespace Monopoly.Human;

public class Humanoid : Input
{
    public override async Task OnDiceReady()
    {
        //WhoseTurn.PlayerState 
        WhoseTurn.State |= PlayerState.ReadyForRolling;
        //Platform.ReadInput();
    }

    public override async Task OnBidOrFold(Player bidder, int mostBid, CancellationToken token)
    {
        var s = Console.ReadLine();
        
        Platform.Log("humanoid: " +mostBid + 1);
        
        new Bid(bidder, mostBid + 1).Execute();
    }

}