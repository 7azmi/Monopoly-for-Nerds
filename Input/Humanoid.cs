using MonopolyTerminal.Interfaces;
using static MonopolyTerminal.Monopoly;
using static MonopolyTerminal.Monopoly.Board;
using static MonopolyTerminal.Monopoly.Player;
using static MonopolyTerminal.TypingSimulator;
namespace MonopolyTerminal.Human;

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
        
        Human.Terminal.Log("humanoid: " +mostBid + 1);
        
        new Bid(bidder, mostBid + 1).Execute();
    }

}