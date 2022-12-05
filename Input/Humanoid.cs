using static MonopolyTerminal.Monopoly;
using static MonopolyTerminal.Monopoly.Board;
using static MonopolyTerminal.Monopoly.Player;
using static MonopolyTerminal.TypingSimulator;
namespace MonopolyTerminal.Human;

public class Humanoid : Input
{
    public Humanoid(IPlatform platform) : base(platform)
    {
        Platform = platform;
    }

    public override async Task OnTurn()
    {
        Platform.ReadInput();
    }

    public override async Task OnBidOrFold(Player bidder, int mostBid, CancellationToken token)
    {
        var s = Console.ReadLine();
        Human.Terminal.Log("humanoid: " +mostBid + 1);

        new Bid(bidder, mostBid + 1).Execute();
    }
}