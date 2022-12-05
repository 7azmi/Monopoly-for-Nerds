using MonopolyTerminal.Terminal;
using static MonopolyTerminal.Monopoly;
using static MonopolyTerminal.Monopoly.Board;
using static MonopolyTerminal.Monopoly.Player;
using static MonopolyTerminal.Monopoly.Player.SetOffer;

namespace MonopolyTerminal;


public abstract class Input
{
    protected IPlatform Platform { get; set; }

    public Input(IPlatform platform)
    {
        Platform = platform;
    }
    //public Action<Command> OnTurn;
    //public Action<Command> OnRollDice;
    //public Action<Command> OnRollAgain;
    //public Action<Command> OnInJail;
    //public Action<Command> OnBuyOrBid;
    //public Action<Command> OnBidOrFold;
    //public Action<Command> OnReceiveDeal;
    //public Action<Command> OnEndTurn;
    //public Action<Command> OnDoesNotHaveEnoughMoney;

    public virtual async Task OnAnyTime(Player player)
    {
           
    }

    public virtual async Task OnTurn()
    {
        
    }
    public virtual async Task OnDiceReady() {}
    public virtual async Task OnDiceReadyAgain(int getDoubles){}
    public virtual async Task OnInJail(){}
    public virtual async Task OnBuyOrBid(Board.Property property){}
    public virtual async Task OnBidOrFold(Player bidder, int mostBid, CancellationToken token){}
    public virtual async Task OnReceiveDeal(Player offeror, Player offeree, Offer offer){}
    public virtual async Task OnTurnCompleted(){}
    public virtual async Task OnDoesNotHaveEnoughMoney(){}
}