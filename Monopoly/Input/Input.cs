using static Monopoly.Monopoly;
using static Monopoly.Monopoly.Board;
using static Monopoly.Monopoly.Player;
using static Monopoly.Monopoly.Player.SetOffer;

namespace Monopoly;


public abstract class Input
{
    protected IPlatform Platform { get; set; }
    protected Player Me { get; private set; }

    internal void Init(Player player)
    {
        Me = player;
        Platform = Monopoly.Platform;
    }
    public virtual async Task OnAnyTime(Player player) {}
    public virtual async Task OnTurn() {}
    public virtual async Task OnDiceReady() {}
    public virtual async Task OnDiceReadyAgain(int getDoubles){}
    public virtual async Task OnInJail(){}
    public virtual async Task OnBuyOrBid(Board.Property property){}
    public virtual async Task OnBidOrFold(Player bidder, int mostBid, CancellationToken token){}
    public virtual async Task OnReceiveDeal(Player offeror, Player offeree, Offer offer){}
    public virtual async Task OnTurnCompleted(){}
    public virtual async Task OnDoesNotHaveEnoughMoney(){}
}