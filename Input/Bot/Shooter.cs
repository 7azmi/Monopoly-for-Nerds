using MonopolyTerminal.Enums;
using MonopolyTerminal.Human;
using MonopolyTerminal.Terminal;
using static MonopolyTerminal.Monopoly;
using static MonopolyTerminal.Monopoly.Player;
using static MonopolyTerminal.Monopoly.Player.SetOffer;
using static MonopolyTerminal.TypingSimulator;

namespace MonopolyTerminal;


public class Shooter : Input
{
    public Shooter(IPlatform platform) : base(platform)
    {
        Platform = platform;
    }
    
    public override async Task OnTurn()
    {
        
    }

    public override async Task OnDiceReady()
    {
        new RollDice(WhoseTurn).Execute();
    }

    public override async Task OnBuyOrBid(Board.Property property)
    {
        //todo change this logic
        if(property.GetPrice() <= WhoseTurn.GetMoney()) new BuyProperty(property).Execute();
        else new OpenAuction(property).Execute();
    }
    public override async Task OnBidOrFold(Player bidder, int mostBid, CancellationToken token)
    {
        var property = WhoseTurn.GetCurrentOccupation() as Board.Property;//must be
        var newBid = mostBid + new Random().Next(5, 50);

        await Task.Delay(new Random().Next(200, 500), token);
        //Console.WriteLine($"{bidder.GetName()} bids {newBid}");
        var isLastBidder = Auction.MostBidder == bidder;
        if (!isLastBidder && newBid <= property.GetPrice() / 2 && bidder.HasEnoughMoney(newBid))
            new Bid(bidder, newBid).Execute();
    }
    public override async Task OnTurnCompleted()
    {
        new EndTurn(WhoseTurn).Execute();
    }

    public override async Task OnReceiveDeal(Player offeror, Player offeree, Offer offer)
    {
        var offerRate = Bank.TotalNetWorth(offer.PropertiesToOffer, offer.MoneyToOffer) /
                        Bank.TotalNetWorth(offer.PropertiesToOffer, offer.MoneyToOffer);

        var offerRateRange = new Random().Next(110, 130) / 100f;
        Human.Terminal.Log($"offer rate rate {offerRateRange}");//debug

        if (offerRate >= offerRateRange) new AcceptOffer(offeree, offeror, offer).Execute();
        else new DeclineOffer(offeree, offeror).Execute();
    }

    public override async Task OnInJail()
    {
        if(WhoseTurn.HasEnoughMoney(50)) new GetOutOfJail(WhoseTurn).Execute();
        else new StayInJail(WhoseTurn).Execute();
    }

    public override async Task OnDoesNotHaveEnoughMoney()
    {
        var commands = new List<Command>();
        
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
                if (property is Board.Street s && s.HasHouses)
                {
                    new SellHouse(WhoseTurn, s).Execute();
                    return true;
                }
        
                if (!property.IsMortgaged())
                {
                    new MortgageProperty(WhoseTurn, property).Execute();
                    return true;
                }
            }
            return false;//so sad
        } 
    }


    // public Command OnLandingOnOwnedProperty(Player whoseTurn, Board.Property property, int bill)
    // {
    //     if (whoseTurn.GetMoney() >= bill) return new PayRent(whoseTurn, property);
    //     else return OnLackOfFunding(WhoseTurn, property, PaymentReason.Rental,bill);
    // }

    // public Command[] OnLackOfFunding(Player whoseTurn, Board.Property property,PaymentReason reason, int bill)
    // {
    //     var commands = new List<Command>();
    //     
    //     // do
    //     // {
    //     //     if(!TrySelling(out var c))return new DeclareBankruptcy(whoseTurn);
    //     // }
    //     while (!whoseTurn.HasEnoughMoney(bill));
    //
    //     return commands.ToArray();
    //
    //     bool TrySelling(out Command command)
    //     {
    //         foreach (var property in whoseTurn.Properties)
    //         {
    //             if (property is Board.Street s && s.HasHouses)
    //             {
    //                 new SellHouse(whoseTurn, s);
    //                 return true;
    //             }
    //
    //             if (!property.IsMortgaged())
    //             {
    //                 new MortgageProperty(whoseTurn, property);
    //                 return true;
    //             }
    //         }
    //         return false;//so sad
    //     } 
    // }
}