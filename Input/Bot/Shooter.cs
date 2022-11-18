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
        new BuyProperty(property).Execute();
    }

    public override async Task OnTurnCompleted()
    {
        new EndTurn(WhoseTurn).Execute();
    }

    public Command ReceiveDeal(Player offeree, Player offeror, Offer offer) //obviously
    {
        var offerRate = Bank.TotalNetWorth(offer.PropertiesToOffer, offer.MoneyToOffer) /
                        Bank.TotalNetWorth(offer.PropertiesToOffer, offer.MoneyToOffer);

        var offerRateRange = new Random().Next(110, 130) / 100f;
        Console.WriteLine($"offer rate rate {offerRateRange}");//debug

        if (offerRate >= offerRateRange) return new AcceptOffer(offeree, offeror, offer);
        else return new DeclineOffer(offeree, offeror);
    }
    
    public Command OnLandingOnUnOwnedProperty(Player whoseTurn, Board.Property property)
    {
        if(property.GetPrice() > whoseTurn.GetMoney()) return new RollDice(WhoseTurn);
        else return new OpenAuction(property);
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