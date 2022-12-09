using MonopolyTerminal.Enums;
using MonopolyTerminal.Human;
using MonopolyTerminal.Terminal;
using static MonopolyTerminal.Monopoly;
using static MonopolyTerminal.Monopoly.Player;
using static MonopolyTerminal.Monopoly.Player.SetOffer;
using static MonopolyTerminal.TypingSimulator;
using static MonopolyTerminal.Monopoly.Board;

namespace MonopolyTerminal;


public class ChessMaster : Input
{


    private List<Player> OtherActivePlayers => ActivePlayers.Except(new[] { Me }).ToList();
    
    
    
    public override async Task OnTurn()
    {
        
    }

    public override async Task OnDiceReady()
    {
        new RollDice(WhoseTurn).Execute();
    }

    public override async Task OnBuyOrBid(Board.Property property)
    {
        if(property is Company) new OpenAuction(property).Execute();
        else if (WhoseTurn.HasEnoughMoney(property.GetPrice()))
        {
            if (property is Street s)
                if(PotentialStreetSet(s)) new BuyProperty(property).Execute();
                else new OpenAuction(property).Execute();
             if(property is Railroad r) new BuyProperty(property).Execute();
        } 
        else new OpenAuction(property).Execute();

        bool PotentialStreetSet(Street s)
        {
            return s.GetStreetSet().All(s => !s.HasOwner || s.GetOwner() == WhoseTurn);
        }
        /*bool ShouldIBuyRailRoad(Railroad r)
        {
            return r.GetStreetSet().All(s => !s.HasOwner || s.GetOwner() == WhoseTurn);
        }*/
    }
    public override async Task OnBidOrFold(Player bidder, int mostBid, CancellationToken token)
    {
        var property = WhoseTurn.GetCurrentOccupation() as Board.Property;//must be
        var newBid = mostBid + new Random().Next(5, 50);

        await Task.Delay(new Random().Next(300, 1000), token);
        
        var isLastBidder = Auction.MostBidder == bidder;
        if (!isLastBidder && newBid <= property.GetPrice() / 2 && bidder.HasEnoughMoney(newBid))
            new Bid(bidder, newBid).Execute();
    }
    public override async Task OnTurnCompleted()
    {
        BuildHouses();
        try
        {
        SetDeal();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        Human.Terminal.Log("hey1");
        new EndTurn(WhoseTurn).Execute();
        Human.Terminal.Log("hey2");
    }

    private void SetDeal()
    {
        var missingTooth = Me.Streets.Where(street => !street.IsCompleteSetProperty
                                                      && street.GetStreetSet().Where(street1 => street1.GetOwner() != Me).ToList().Count == 1);//hope it works
        var stolenTooth = Me.Streets.Where(street => !street.IsCompleteSetProperty
                                                     && street.GetStreetSet().
                                                         Where(street1 => street1.GetOwner() != Me || street1.GetOwner() == null).ToList().Count == 1);
        var offer = new SetOffer(OtherActivePlayers.First(), new Offer(new []{Me.Properties.First()}, null, 500, 10));

        try
        {
        if(offer.IsLegal()) offer.Execute();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void BuildHouses()
    {
        var hasSet = WhoseTurn.HasSet;
        if (!hasSet) return; 
        foreach (var street in WhoseTurn.Streets)
        {
            if (street.PlayerCanBuildHouseHere && WhoseTurn.HasEnoughMoney(street.GetHousePrice() * 3));
            new BuildHouse(WhoseTurn, street).Execute();
        }
    }

    public override async Task OnReceiveDeal(Player offeror, Player offeree, Offer offer)
    {
        var offerRate = Bank.TotalNetWorth(offer.PropertiesToOffer, offer.MoneyToOffer) /
                        Bank.TotalNetWorth(offer.PropertiesToOffer, offer.MoneyToOffer);

        var offerRateRange = new Random().Next(110, 130) / 100f;
        Human.Terminal.Log($"offer rate rate {offerRateRange}");//debug

        Thread.Sleep(2000);
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