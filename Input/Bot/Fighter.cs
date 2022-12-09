namespace MonopolyTerminal;
using static Monopoly.Player;
using static Monopoly;
using static Monopoly.Board;

public class Fighter : Input
{
    public override async Task OnDiceReady()
    {
        new RollDice(WhoseTurn).Execute();
    }

    public override async Task OnDoesNotHaveEnoughMoney()
    {
        while(true)
        {
            if (!TrySelling())
            {
                new DeclareBankruptcy(WhoseTurn).Execute();
                break;
            }
            else if (WhoseTurn.GetMoney() >= 0)
            {
                Engine.OnTurnCompleted.Invoke();
                break;
            } 
        }

        bool TrySelling()
        {
            foreach (var property in WhoseTurn.Properties)
            {
                if (!property.IsMortgaged() && !(property is Street s && s.HasHouses))
                {
                    new MortgageProperty(WhoseTurn, property).Execute();
                    return true;
                }
                if (property is Board.Street ss && ss.HasHouses)
                {
                    new SellHouse(WhoseTurn, ss).Execute();
                    return true;
                }
        
            }
            return false;//so sad
        }
    }

    public override async Task OnBidOrFold(Player bidder, int mostBid, CancellationToken token)
    {
        var property = WhoseTurn.GetCurrentOccupation() as Board.Property;//must be
        var newBid = mostBid + new Random().Next(5, 50);

        if (bidder.HasEnoughMoney(newBid) && property.GetPrice() <= newBid) new Bid(bidder, newBid).Execute();  
    }

    public override async Task OnInJail()
    {
        if(WhoseTurn.HasEnoughMoney(50) || Board.Jail.GetPrisoner(WhoseTurn).ExceededJailPeriod()) 
            new GetOutOfJail(WhoseTurn).Execute();
        else new StayInJail(WhoseTurn).Execute();
    }


}